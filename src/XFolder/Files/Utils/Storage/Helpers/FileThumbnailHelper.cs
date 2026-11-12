using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Media.Imaging;
using Files.Helpers.Shared;
using Files.Services.NativePlugin;

namespace Files.Utils.Storage.Helpers;

public static class FileThumbnailHelper
{
	private sealed class ThumbnailCacheSnapshot
	{
		public string? DirectoryIconFileName { get; set; }

		public long DirectoryIconCapturedAtUnixSeconds { get; set; }

		public Dictionary<string, ThumbnailCacheEntry>? ExtensionIcons { get; set; }
	}

	private sealed class ThumbnailCacheEntry
	{
		public string IconFileName { get; set; } = string.Empty;

		public long CapturedAtUnixSeconds { get; set; }
	}

	/// <summary>
	/// LRU-ish bounded bitmap cache. Bitmaps are intentionally not disposed when
	/// evicted: a bitmap can still be bound to a visible image control and
	/// disposing it would break rendering. Removing the reference lets the GC
	/// reclaim the native bitmap once nothing is using it.
	/// </summary>
	private sealed class BitmapCache
	{
		private sealed class Entry
		{
			public Bitmap Bitmap = null!;

			public long LastAccessTicks;
		}

		private readonly ConcurrentDictionary<string, Entry> _entries = new ConcurrentDictionary<string, Entry>(StringComparer.Ordinal);

		private readonly int _maxEntries;

		private int _trimming;

		public BitmapCache(int maxEntries)
		{
			_maxEntries = maxEntries;
		}

		public bool TryGet(string key, [System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out Bitmap? bitmap)
		{
			if (_entries.TryGetValue(key, out Entry? entry))
			{
				Interlocked.Exchange(ref entry.LastAccessTicks, DateTime.UtcNow.Ticks);
				bitmap = entry.Bitmap;
				return true;
			}
			bitmap = null;
			return false;
		}

		public void Set(string key, Bitmap bitmap)
		{
			_entries[key] = new Entry
			{
				Bitmap = bitmap,
				LastAccessTicks = DateTime.UtcNow.Ticks
			};
			TrimIfNeeded();
		}

		public void Remove(string key)
		{
			_entries.TryRemove(key, out _);
		}

		private void TrimIfNeeded()
		{
			if (_entries.Count <= _maxEntries || Interlocked.Exchange(ref _trimming, 1) != 0)
			{
				return;
			}
			try
			{
				int num = _entries.Count - _maxEntries + _maxEntries / 4;
				if (num <= 0)
				{
					return;
				}
				foreach (string item in (from pair in _entries
					orderby Interlocked.Read(ref pair.Value.LastAccessTicks)
					select pair.Key).Take(num).ToList())
				{
					_entries.TryRemove(item, out _);
				}
			}
			finally
			{
				Volatile.Write(ref _trimming, 0);
			}
		}
	}

	public static ConcurrentDictionary<string, Bitmap> _dictionary;

	public static Bitmap? _dirThumbnail;

	private static readonly ConcurrentDictionary<string, Lazy<Task<Bitmap>>> _pendingTasks;

	private static readonly ConcurrentDictionary<string, long> _extensionCaptureTimes;

	private static long _directoryIconCapturedAtUnixSeconds;

	private static int _persistentCacheLoaded;

	private static readonly string _thumbnailCacheFilePath;

	private static readonly string _thumbnailCacheDirectoryPath;

	private static readonly Lazy<Task<Bitmap>> _dirIconTask;

	private static readonly BitmapCache _appPathDictionary;

	private static readonly ConcurrentDictionary<string, Lazy<Task<Bitmap>>> _pendingAppTasks;

	private static readonly BitmapCache _customFolderDictionary;

	private static readonly ConcurrentDictionary<string, Lazy<Task<Bitmap>>> _pendingFolderTasks;

	private static readonly BitmapCache _imageThumbnailDictionary;

	private static readonly ConcurrentDictionary<string, Lazy<Task<Bitmap?>>> _pendingImageTasks;

	static FileThumbnailHelper()
	{
		_dictionary = new ConcurrentDictionary<string, Bitmap>();
		_dirThumbnail = null;
		_pendingTasks = new ConcurrentDictionary<string, Lazy<Task<Bitmap>>>();
		_extensionCaptureTimes = new ConcurrentDictionary<string, long>();
		_dirIconTask = new Lazy<Task<Bitmap>>(() => Task.Run(() => MacPluginInterop.GetMacDirectoryIconBitmap() ?? throw new InvalidOperationException("无法获取默认目录图标")));
		_appPathDictionary = new BitmapCache(256);
		_pendingAppTasks = new ConcurrentDictionary<string, Lazy<Task<Bitmap>>>();
		_customFolderDictionary = new BitmapCache(256);
		_pendingFolderTasks = new ConcurrentDictionary<string, Lazy<Task<Bitmap>>>();
		_imageThumbnailDictionary = new BitmapCache(512);
		_pendingImageTasks = new ConcurrentDictionary<string, Lazy<Task<Bitmap>>>();
		string text = Path.Combine(PathHelpers.GetAppDataDirectory(), "cache");
		Directory.CreateDirectory(text);
		_thumbnailCacheFilePath = Path.Combine(text, "thumbnail_cache.json");
		_thumbnailCacheDirectoryPath = Path.Combine(text, "thumbnail");
		Directory.CreateDirectory(_thumbnailCacheDirectoryPath);
	}

	public static async Task EnsureDefaultIconsPreloadedAsync()
	{
		await LoadPersistentCacheAsync();
		await GetMacDirectoryIconBitmapAsync();
	}

	public static async Task LoadPersistentCacheAsync()
	{
		if (Interlocked.Exchange(ref _persistentCacheLoaded, 1) != 1)
		{
			await Task.Run((Action)LoadPersistentCache);
		}
	}

	public static async Task SavePersistentCacheAsync()
	{
		await Task.Run((Action)SavePersistentCache);
	}

	public static Bitmap? GetDefaultFolderIcon()
	{
		return _dirThumbnail;
	}

	public static Bitmap? GetDefaultFileIcon(string ext)
	{
		if (ext == null)
		{
			return null;
		}
		_dictionary.TryGetValue(ext, out Bitmap value);
		return value;
	}

	public static Bitmap GetThumbnailByExtension(string ext)
	{
		return GetThumbnailByExtensionAsync(ext).GetAwaiter().GetResult();
	}

	public static async Task<Bitmap> GetThumbnailByExtensionAsync(string ext)
	{
		if (ext == null)
		{
			ext = "";
		}
		if (_dictionary.TryGetValue(ext, out Bitmap value))
		{
			return value;
		}
		return await _pendingTasks.GetOrAdd(ext, (string k) => new Lazy<Task<Bitmap>>(() => Task.Run(() =>
		{
			try
			{
				Bitmap bitmap = MacPluginInterop.GetFileIconBitmapByExt(k) ?? GetMacDirectoryIconBitmap();
				_dictionary[k] = bitmap;
				_extensionCaptureTimes[k] = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
				return bitmap;
			}
			finally
			{
				_pendingTasks.TryRemove(k, out Lazy<Task<Bitmap>> _);
			}
		}))).Value;
	}

	public static async Task<Bitmap> GetMacDirectoryIconBitmapAsync()
	{
		if (_dirThumbnail != null)
		{
			return _dirThumbnail;
		}
		Bitmap? result = (_dirThumbnail = await _dirIconTask.Value);
		_directoryIconCapturedAtUnixSeconds = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
		return result;
	}

	public static Bitmap GetMacDirectoryIconBitmap()
	{
		if (_dirThumbnail != null)
		{
			return _dirThumbnail;
		}
		return GetMacDirectoryIconBitmapAsync().GetAwaiter().GetResult();
	}

	public static async Task<Bitmap> GetAppIconBitmapAsync(string path)
	{
		if (_appPathDictionary.TryGet(path, out Bitmap? value))
		{
			return value;
		}
		return await _pendingAppTasks.GetOrAdd(path, (string k) => new Lazy<Task<Bitmap>>(() => Task.Run(async () =>
		{
			try
			{
				Bitmap bitmap = MacPluginInterop.GetFileIconBitmapByPath(k);
				if (bitmap != null)
				{
					_appPathDictionary.Set(k, bitmap);
				}
				else
				{
					bitmap = await GetMacDirectoryIconBitmapAsync();
				}
				return bitmap;
			}
			finally
			{
				// Do not keep a faulted task around: a failure would otherwise be
				// cached forever for this path.
				_pendingAppTasks.TryRemove(k, out Lazy<Task<Bitmap>> _);
			}
		}))).Value;
	}

	public static async Task<Bitmap> GetFolderIconBitmapAsync(string path)
	{
		if (_customFolderDictionary.TryGet(path, out Bitmap? value))
		{
			return value;
		}
		return await _pendingFolderTasks.GetOrAdd(path, (string k) => new Lazy<Task<Bitmap>>(() => Task.Run(async () =>
		{
			try
			{
				Bitmap folderIconBitmap = MacPluginInterop.GetFolderIconBitmap(k);
				if (folderIconBitmap != null)
				{
					_customFolderDictionary.Set(k, folderIconBitmap);
					return folderIconBitmap;
				}
				return await GetMacDirectoryIconBitmapAsync();
			}
			finally
			{
				_pendingFolderTasks.TryRemove(k, out Lazy<Task<Bitmap>> _);
			}
		}))).Value;
	}

	public static async Task<Bitmap?> GetImageThumbnailAsync(string path, int width = 64, int height = 64)
	{
		if (_imageThumbnailDictionary.TryGet(path, out Bitmap? value))
		{
			return value;
		}
		return await _pendingImageTasks.GetOrAdd(path, (string k) => new Lazy<Task<Bitmap>>(() => Task.Run(() =>
		{
			try
			{
				Bitmap imageThumbnailBitmap = MacPluginInterop.GetImageThumbnailBitmap(k, width, height);
				if (imageThumbnailBitmap != null)
				{
					_imageThumbnailDictionary.Set(k, imageThumbnailBitmap);
				}
				return imageThumbnailBitmap;
			}
			finally
			{
				_pendingImageTasks.TryRemove(k, out Lazy<Task<Bitmap?>> _);
			}
		}))).Value;
	}

	public static void InvalidateImageThumbnail(string path)
	{
		if (!string.IsNullOrWhiteSpace(path))
		{
			_imageThumbnailDictionary.Remove(path);
			_pendingImageTasks.TryRemove(path, out Lazy<Task<Bitmap?>> _);
		}
	}

	private static void LoadPersistentCache()
	{
		if (!File.Exists(_thumbnailCacheFilePath))
		{
			return;
		}
		try
		{
			ThumbnailCacheSnapshot thumbnailCacheSnapshot = JsonSerializer.Deserialize<ThumbnailCacheSnapshot>(File.ReadAllText(_thumbnailCacheFilePath));
			if (thumbnailCacheSnapshot == null)
			{
				return;
			}
			if (!string.IsNullOrWhiteSpace(thumbnailCacheSnapshot.DirectoryIconFileName) && thumbnailCacheSnapshot.DirectoryIconCapturedAtUnixSeconds > 0 && !IsExpired(thumbnailCacheSnapshot.DirectoryIconCapturedAtUnixSeconds))
			{
				string text = NormalizeCacheIconFileName(thumbnailCacheSnapshot.DirectoryIconFileName);
				Bitmap bitmap = ((text == null) ? null : LoadBitmapFromFile(GetCacheIconFilePath(text)));
				if (bitmap != null)
				{
					_dirThumbnail = bitmap;
					_directoryIconCapturedAtUnixSeconds = thumbnailCacheSnapshot.DirectoryIconCapturedAtUnixSeconds;
				}
			}
			if (thumbnailCacheSnapshot.ExtensionIcons == null)
			{
				return;
			}
			foreach (KeyValuePair<string, ThumbnailCacheEntry> extensionIcon in thumbnailCacheSnapshot.ExtensionIcons)
			{
				if (extensionIcon.Key != null && extensionIcon.Value != null && !IsExpired(extensionIcon.Value.CapturedAtUnixSeconds))
				{
					Bitmap bitmap2 = null;
					string text2 = NormalizeCacheIconFileName(extensionIcon.Value.IconFileName);
					if (!string.IsNullOrWhiteSpace(text2))
					{
						bitmap2 = LoadBitmapFromFile(GetCacheIconFilePath(text2));
					}
					if (bitmap2 != null)
					{
						_dictionary[extensionIcon.Key] = bitmap2;
						_extensionCaptureTimes[extensionIcon.Key] = extensionIcon.Value.CapturedAtUnixSeconds;
					}
				}
			}
		}
		catch
		{
		}
	}

	public static void SavePersistentCache()
	{
		try
		{
			Directory.CreateDirectory(_thumbnailCacheDirectoryPath);
			Dictionary<string, ThumbnailCacheEntry> dictionary = new Dictionary<string, ThumbnailCacheEntry>();
			HashSet<string> hashSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
			foreach (KeyValuePair<string, Bitmap> item in _dictionary)
			{
				string key = item.Key;
				Bitmap value = item.Value;
				if (key == null || value == null)
				{
					continue;
				}
				long capturedAtUnixSeconds = (_extensionCaptureTimes.TryGetValue(key, out var value2) ? value2 : DateTimeOffset.UtcNow.ToUnixTimeSeconds());
				if (!IsExpired(capturedAtUnixSeconds))
				{
					string text = BuildExtensionIconFileName(key);
					string cacheIconFilePath = GetCacheIconFilePath(text);
					if (SaveBitmapToFile(value, cacheIconFilePath))
					{
						hashSet.Add(text);
						dictionary[key] = new ThumbnailCacheEntry
						{
							IconFileName = text,
							CapturedAtUnixSeconds = capturedAtUnixSeconds
						};
					}
				}
			}
			string text2 = null;
			long num = _directoryIconCapturedAtUnixSeconds;
			if (_dirThumbnail != null)
			{
				if (num <= 0)
				{
					num = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
				}
				if (!IsExpired(num))
				{
					text2 = BuildDirectoryIconFileName();
					string cacheIconFilePath2 = GetCacheIconFilePath(text2);
					if (SaveBitmapToFile(_dirThumbnail, cacheIconFilePath2))
					{
						hashSet.Add(text2);
					}
					else
					{
						text2 = null;
					}
				}
			}
			string contents = JsonSerializer.Serialize(new ThumbnailCacheSnapshot
			{
				DirectoryIconFileName = text2,
				DirectoryIconCapturedAtUnixSeconds = num,
				ExtensionIcons = dictionary
			});
			File.WriteAllText(_thumbnailCacheFilePath, contents);
			CleanupIconCacheFiles(hashSet);
		}
		catch
		{
		}
	}

	private static bool IsExpired(long capturedAtUnixSeconds)
	{
		if (capturedAtUnixSeconds <= 0)
		{
			return true;
		}
		return DateTimeOffset.UtcNow.ToUnixTimeSeconds() - capturedAtUnixSeconds > 604800;
	}

	private static Bitmap? LoadBitmapFromFile(string filePath)
	{
		if (string.IsNullOrWhiteSpace(filePath) || !File.Exists(filePath))
		{
			return null;
		}
		try
		{
			using FileStream stream = File.OpenRead(filePath);
			return new Bitmap(stream);
		}
		catch
		{
			return null;
		}
	}

	private static bool SaveBitmapToFile(Bitmap bitmap, string filePath)
	{
		try
		{
			using FileStream stream = File.Create(filePath);
			bitmap.Save(stream);
			return true;
		}
		catch
		{
			return false;
		}
	}

	private static string BuildDirectoryIconFileName()
	{
		return "directory_icon.png";
	}

	private static string BuildExtensionIconFileName(string ext)
	{
		byte[] inArray = SHA256.HashData(Encoding.UTF8.GetBytes(ext));
		return "ext_" + Convert.ToHexString(inArray) + ".png";
	}

	private static string GetCacheIconFilePath(string fileName)
	{
		return Path.Combine(_thumbnailCacheDirectoryPath, fileName);
	}

	private static string? NormalizeCacheIconFileName(string? fileName)
	{
		if (string.IsNullOrWhiteSpace(fileName))
		{
			return null;
		}
		string fileName2 = Path.GetFileName(fileName);
		if (!string.IsNullOrWhiteSpace(fileName2))
		{
			return fileName2;
		}
		return null;
	}

	private static void CleanupIconCacheFiles(HashSet<string> validFileNames)
	{
		try
		{
			if (!Directory.Exists(_thumbnailCacheDirectoryPath))
			{
				return;
			}
			foreach (string item in Directory.EnumerateFiles(_thumbnailCacheDirectoryPath))
			{
				string fileName = Path.GetFileName(item);
				if (!validFileNames.Contains(fileName))
				{
					File.Delete(item);
				}
			}
		}
		catch
		{
		}
	}
}
