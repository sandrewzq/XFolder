using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Platform.Storage;
using Files.Data.Contracts;
using Files.Data.Enums;
using Files.Data.Enums.Storage;
using Files.Data.Items;
using Files.Extensions;
using Files.Services.NativePlugin;
using Files.Utils.Storage.Helpers;
using Files.Utils.Storage.History;

namespace Files.Utils.Storage.Operations;

public sealed class FilesystemOperations : IDisposable
{
	private sealed record TransferPlanItem(IStorageItemWithPath Source, string Destination, NameCollisionOption Collision);

	private sealed record TransferTotals(long TotalBytes, long TotalItems);

	private sealed class TransferProgressState
	{
		private readonly IProgress<FileOperationProgress>? progress;

		private readonly long totalBytes;

		private readonly long totalItems;

		private long processedBytes;

		private long processedItems;

		public TransferProgressState(long totalBytes, long totalItems, IProgress<FileOperationProgress>? progress)
		{
			this.totalBytes = totalBytes;
			this.totalItems = totalItems;
			this.progress = progress;
		}

		public void AddBytes(long bytes, string currentPath)
		{
			if (bytes > 0)
			{
				processedBytes += bytes;
			}
			Report(isPreparing: false, currentPath);
		}

		public void AddItem(string currentPath)
		{
			processedItems++;
			Report(isPreparing: false, currentPath);
		}

		public void Report(bool isPreparing, string currentPath)
		{
			progress?.Report(new FileOperationProgress
			{
				IsPreparing = isPreparing,
				TotalBytes = totalBytes,
				ProcessedBytes = Math.Clamp(processedBytes, 0L, totalBytes),
				TotalItems = totalItems,
				ProcessedItems = Math.Clamp(processedItems, 0L, totalItems),
				CurrentPath = currentPath
			});
		}
	}

	private readonly IShellView? _associatedInstance;

	public FilesystemOperations(IShellView? associatedInstance = null)
	{
		_associatedInstance = associatedInstance;
	}

	public Task<(IStorageHistory, IStorageItem)> CreateAsync(IStorageItemWithPath source, CancellationToken cancellationToken = default(CancellationToken))
	{
		try
		{
			if (source.ItemType == FilesystemItemType.Directory)
			{
				if (!Directory.CreateDirectory(source.Path).Exists)
				{
					return Task.FromResult<(IStorageHistory, IStorageItem)>((null, null));
				}
				return Task.FromResult<(IStorageHistory, IStorageItem)>((new StorageHistory(FileOperationType.CreateNew, source, source), null));
			}
			if (source.ItemType == FilesystemItemType.File)
			{
				using (File.Create(source.Path))
				{
				}
				return Task.FromResult<(IStorageHistory, IStorageItem)>((new StorageHistory(FileOperationType.CreateNew, source, source), null));
			}
			return Task.FromResult<(IStorageHistory, IStorageItem)>((null, null));
		}
		catch (Exception)
		{
			return Task.FromResult<(IStorageHistory, IStorageItem)>((null, null));
		}
	}

	public async Task<IStorageHistory> DeleteAsync(ListedItem source, bool permanently, CancellationToken cancellationToken = default(CancellationToken))
	{
		return await DeleteAsync(source.ToStorageItemWithPath(), permanently, cancellationToken);
	}

	public async Task<IStorageHistory?> DeleteAsync(IStorageItemWithPath source, bool permanently, CancellationToken cancellationToken = default(CancellationToken))
	{
		if (source == null || string.IsNullOrWhiteSpace(source.Path))
		{
			return null;
		}
		if (cancellationToken.IsCancellationRequested)
		{
			return null;
		}
		bool flag = false;
		try
		{
			if (permanently)
			{
				if (File.Exists(source.Path))
				{
					File.Delete(source.Path);
					flag = true;
				}
				else if (Directory.Exists(source.Path))
				{
					Directory.Delete(source.Path, recursive: true);
					flag = true;
				}
				if (flag)
				{
					return new StorageHistory(FileOperationType.Delete, source, source);
				}
			}
			else
			{
				var (flag2, text) = MacPluginInterop.MoveFileOrDirectoryToTrash(source.Path);
				if (flag2)
				{
					IStorageItemWithPath destination = StorageHelpers.FromPathAndType(text ?? source.Path, source.ItemType);
					return new StorageHistory(FileOperationType.Recycle, source, destination);
				}
			}
		}
		catch (UnauthorizedAccessException)
		{
			return null;
		}
		catch (IOException)
		{
			return null;
		}
		catch (Exception)
		{
			return null;
		}
		await Task.CompletedTask;
		return null;
	}

	public async Task<IStorageHistory> DeleteItemsAsync(IList<ListedItem> source, bool permanently, CancellationToken cancellationToken = default(CancellationToken))
	{
		return await DeleteItemsAsync(await source.Select((ListedItem item) => item.ToStorageItemWithPath()).ToListAsync(), permanently, cancellationToken);
	}

	public async Task<IStorageHistory> DeleteItemsAsync(IList<IStorageItemWithPath> source, bool permanently, CancellationToken cancellationToken = default(CancellationToken))
	{
		if (source == null || source.Count == 0)
		{
			return null;
		}
		List<IStorageHistory> rawStorageHistory = new List<IStorageHistory>();
		foreach (IStorageItemWithPath item in source)
		{
			if (!cancellationToken.IsCancellationRequested)
			{
				IStorageHistory storageHistory = await DeleteAsync(item, permanently, cancellationToken);
				if (storageHistory != null)
				{
					rawStorageHistory.Add(storageHistory);
				}
				continue;
			}
			break;
		}
		if (rawStorageHistory.Any())
		{
			return new StorageHistory(rawStorageHistory[0].OperationType, rawStorageHistory.SelectMany((IStorageHistory h) => h.Source).ToList(), rawStorageHistory.SelectMany((IStorageHistory h) => h.Destination).ToList());
		}
		return null;
	}

	public async Task<IStorageHistory?> RenameAsync(ListedItem source, string newName, CancellationToken cancellationToken = default(CancellationToken))
	{
		return await RenameAsync(source.ToStorageItemWithPath(), newName, cancellationToken);
	}

	public async Task<IStorageHistory?> RenameAsync(IStorageItemWithPath source, string newName, CancellationToken cancellationToken = default(CancellationToken))
	{
		try
		{
			string directoryName = Path.GetDirectoryName(source.Path);
			if (string.IsNullOrEmpty(directoryName))
			{
				return null;
			}
			string text = Path.Combine(directoryName, newName);
			if (source.ItemType == FilesystemItemType.File)
			{
				File.Move(source.Path, text);
			}
			else
			{
				if (source.ItemType != FilesystemItemType.Directory)
				{
					return null;
				}
				Directory.Move(source.Path, text);
			}
			IStorageItemWithPath destination = StorageHelpers.FromPathAndType(text, source.ItemType);
			return new StorageHistory(FileOperationType.Rename, source, destination);
		}
		catch
		{
			return null;
		}
	}

	public async Task<IStorageHistory> CopyAsync(ListedItem source, string destination, NameCollisionOption collision, CancellationToken cancellationToken = default(CancellationToken))
	{
		return await CopyAsync(source.ToStorageItemWithPath(), destination, collision, cancellationToken);
	}

	public async Task<IStorageHistory> CopyAsync(IStorageItemWithPath source, string destination, NameCollisionOption collision, CancellationToken cancellationToken = default(CancellationToken))
	{
		try
		{
			if (source == null || string.IsNullOrWhiteSpace(source.Path))
			{
				return null;
			}
			if (string.IsNullOrWhiteSpace(destination))
			{
				return null;
			}
			string fileName = Path.GetFileName(source.Path.TrimEnd(Path.DirectorySeparatorChar));
			string text = destination;
			if (source.Path == text && collision != NameCollisionOption.GenerateUniqueName)
			{
				return null;
			}
			if (source.ItemType == FilesystemItemType.Directory)
			{
				string value = Path.GetFullPath(source.Path).TrimEnd(Path.DirectorySeparatorChar) + Path.DirectorySeparatorChar;
				if ((Path.GetFullPath(text).TrimEnd(Path.DirectorySeparatorChar) + Path.DirectorySeparatorChar).StartsWith(value, StringComparison.OrdinalIgnoreCase))
				{
					return null;
				}
			}
			if (collision == NameCollisionOption.GenerateUniqueName && (File.Exists(text) || Directory.Exists(text)))
			{
				bool isFile = source.ItemType == FilesystemItemType.File;
				(string name, string path) tuple = FilesystemHelpers.GenerateUniqueName(Path.GetDirectoryName(destination), fileName, isFile);
				_ = tuple.name;
				text = tuple.path;
			}
			if (MacPluginInterop.CopyItem(source.Path, text, collision == NameCollisionOption.ReplaceExisting))
			{
				IStorageItemWithPath dest = StorageHelpers.FromPathAndType(text, source.ItemType);
				await Task.Yield();
				return new StorageHistory(FileOperationType.Copy, source, dest);
			}
			return null;
		}
		catch
		{
			return null;
		}
	}

	public async Task<IStorageHistory> CopyItemsAsync(IList<ListedItem> source, IList<string> destination, IList<NameCollisionOption> collisions, CancellationToken cancellationToken = default(CancellationToken))
	{
		return await CopyItemsAsync(await source.Select((ListedItem i) => i.ToStorageItemWithPath()).ToListAsync(), destination, collisions, cancellationToken);
	}

	public async Task<IStorageHistory> CopyItemsAsync(IList<IStorageItemWithPath> source, IList<string> destination, IList<NameCollisionOption> collisions, CancellationToken cancellationToken = default(CancellationToken))
	{
		List<IStorageHistory> rawStorageHistory = new List<IStorageHistory>();
		for (int i = 0; i < source.Count; i++)
		{
			List<IStorageHistory> list = rawStorageHistory;
			list.Add(await CopyAsync(source[i], destination[i], collisions[i], cancellationToken));
		}
		if (rawStorageHistory.Any() && rawStorageHistory.All((IStorageHistory item) => item != null))
		{
			FileOperationType operationType = rawStorageHistory[0].OperationType;
			return new StorageHistory(operationType, await rawStorageHistory.SelectMany((IStorageHistory item) => item.Source).ToListAsync(), await rawStorageHistory.SelectMany((IStorageHistory item) => item.Destination).ToListAsync());
		}
		return null;
	}

	public async Task<IStorageHistory?> CopyItemsAsync(IList<IStorageItemWithPath> source, IList<string> destination, IList<NameCollisionOption> collisions, IProgress<FileOperationProgress>? progress, CancellationToken cancellationToken = default(CancellationToken))
	{
		try
		{
			List<TransferPlanItem> plan = BuildTransferPlan(source, destination, collisions, isMove: false);
			if (plan.Count == 0)
			{
				return null;
			}
			TransferTotals transferTotals = await Task.Run(() => CalculateTransferTotals(plan, progress, cancellationToken), cancellationToken);
			TransferProgressState progressState = new TransferProgressState(transferTotals.TotalBytes, transferTotals.TotalItems, progress);
			List<IStorageHistory> rawStorageHistory = new List<IStorageHistory>();
			progressState.Report(isPreparing: false, string.Empty);
			foreach (TransferPlanItem item in plan)
			{
				cancellationToken.ThrowIfCancellationRequested();
				IStorageItemWithPath storageItemWithPath = await CopyResolvedAsync(item.Source, item.Destination, item.Collision == NameCollisionOption.ReplaceExisting, progressState, item.Source.Path, cancellationToken);
				if (storageItemWithPath == null)
				{
					return null;
				}
				rawStorageHistory.Add(new StorageHistory(FileOperationType.Copy, item.Source, storageItemWithPath));
			}
			if (rawStorageHistory.Count == plan.Count)
			{
				return new StorageHistory(FileOperationType.Copy, rawStorageHistory.SelectMany((IStorageHistory storageHistory) => storageHistory.Source).ToList(), rawStorageHistory.SelectMany((IStorageHistory storageHistory) => storageHistory.Destination).ToList());
			}
			return null;
		}
		catch (OperationCanceledException)
		{
			throw;
		}
		catch
		{
			return null;
		}
	}

	public async Task<IStorageHistory> MoveAsync(ListedItem source, string destination, NameCollisionOption collision, CancellationToken cancellationToken = default(CancellationToken))
	{
		return await MoveAsync(source.ToStorageItemWithPath(), destination, collision, cancellationToken);
	}

	public async Task<IStorageHistory> MoveAsync(IStorageItemWithPath source, string destination, NameCollisionOption collision, CancellationToken cancellationToken = default(CancellationToken))
	{
		try
		{
			if (source == null || string.IsNullOrWhiteSpace(source.Path))
			{
				return null;
			}
			if (string.IsNullOrWhiteSpace(destination))
			{
				return null;
			}
			string fileName = Path.GetFileName(source.Path.TrimEnd(Path.DirectorySeparatorChar));
			string text = destination;
			if (source.Path == text)
			{
				return null;
			}
			if (source.ItemType == FilesystemItemType.Directory)
			{
				string value = Path.GetFullPath(source.Path).TrimEnd(Path.DirectorySeparatorChar) + Path.DirectorySeparatorChar;
				if ((Path.GetFullPath(text).TrimEnd(Path.DirectorySeparatorChar) + Path.DirectorySeparatorChar).StartsWith(value, StringComparison.OrdinalIgnoreCase))
				{
					return null;
				}
			}
			if (collision == NameCollisionOption.GenerateUniqueName && (File.Exists(text) || Directory.Exists(text)))
			{
				bool isFile = source.ItemType == FilesystemItemType.File;
				(string name, string path) tuple = FilesystemHelpers.GenerateUniqueName(Path.GetDirectoryName(destination), fileName, isFile);
				_ = tuple.name;
				text = tuple.path;
			}
			if (MacPluginInterop.MoveItem(source.Path, text, collision == NameCollisionOption.ReplaceExisting))
			{
				IStorageItemWithPath dest = StorageHelpers.FromPathAndType(text, source.ItemType);
				await Task.Yield();
				return new StorageHistory(FileOperationType.Move, source, dest);
			}
			return null;
		}
		catch
		{
			return null;
		}
	}

	public async Task<IStorageHistory> MoveItemsAsync(IList<ListedItem> source, IList<string> destination, IList<NameCollisionOption> collisions, CancellationToken cancellationToken = default(CancellationToken))
	{
		return await MoveItemsAsync(await source.Select((ListedItem i) => i.ToStorageItemWithPath()).ToListAsync(), destination, collisions, cancellationToken);
	}

	public async Task<IStorageHistory?> MoveItemsAsync(IList<IStorageItemWithPath> source, IList<string> destination, IList<NameCollisionOption> collisions, CancellationToken cancellationToken = default(CancellationToken))
	{
		List<IStorageHistory> rawStorageHistory = new List<IStorageHistory>();
		for (int i = 0; i < source.Count; i++)
		{
			List<IStorageHistory> list = rawStorageHistory;
			list.Add(await MoveAsync(source[i], destination[i], collisions[i]));
		}
		if (rawStorageHistory.Any() && rawStorageHistory.All((IStorageHistory item) => item != null))
		{
			FileOperationType operationType = rawStorageHistory[0].OperationType;
			return new StorageHistory(operationType, await rawStorageHistory.SelectMany((IStorageHistory item) => item.Source).ToListAsync(), await rawStorageHistory.SelectMany((IStorageHistory item) => item.Destination).ToListAsync());
		}
		return null;
	}

	public async Task<IStorageHistory?> MoveItemsAsync(IList<IStorageItemWithPath> source, IList<string> destination, IList<NameCollisionOption> collisions, IProgress<FileOperationProgress>? progress, CancellationToken cancellationToken = default(CancellationToken))
	{
		try
		{
			List<TransferPlanItem> plan = BuildTransferPlan(source, destination, collisions, isMove: true);
			if (plan.Count == 0)
			{
				return null;
			}
			List<IStorageHistory> rawStorageHistory = new List<IStorageHistory>();
			List<TransferPlanItem> fallbackPlan = new List<TransferPlanItem>();
			foreach (TransferPlanItem item2 in plan)
			{
				cancellationToken.ThrowIfCancellationRequested();
				if (TryMoveDirect(item2, out IStorageHistory history))
				{
					if (history != null)
					{
						rawStorageHistory.Add(history);
						progress?.Report(new FileOperationProgress
						{
							TotalItems = plan.Count,
							ProcessedItems = rawStorageHistory.Count,
							CurrentPath = item2.Source.Path
						});
					}
				}
				else
				{
					fallbackPlan.Add(item2);
				}
			}
			if (fallbackPlan.Count > 0)
			{
				TransferTotals transferTotals = await Task.Run(() => CalculateTransferTotals(fallbackPlan, progress, cancellationToken), cancellationToken);
				TransferProgressState progressState = new TransferProgressState(transferTotals.TotalBytes, transferTotals.TotalItems, progress);
				progressState.Report(isPreparing: false, string.Empty);
				foreach (TransferPlanItem item in fallbackPlan)
				{
					cancellationToken.ThrowIfCancellationRequested();
					IStorageItemWithPath storageItemWithPath = await CopyResolvedAsync(item.Source, item.Destination, item.Collision == NameCollisionOption.ReplaceExisting, progressState, item.Source.Path, cancellationToken);
					if (storageItemWithPath == null)
					{
						return null;
					}
					if (cancellationToken.IsCancellationRequested)
					{
						DeletePathQuietly(item.Destination);
						cancellationToken.ThrowIfCancellationRequested();
					}
					DeletePath(item.Source.Path);
					rawStorageHistory.Add(new StorageHistory(FileOperationType.Move, item.Source, storageItemWithPath));
				}
			}
			if (rawStorageHistory.Count == plan.Count)
			{
				return new StorageHistory(FileOperationType.Move, rawStorageHistory.SelectMany((IStorageHistory storageHistory) => storageHistory.Source).ToList(), rawStorageHistory.SelectMany((IStorageHistory storageHistory) => storageHistory.Destination).ToList());
			}
			return null;
		}
		catch (OperationCanceledException)
		{
			throw;
		}
		catch
		{
			return null;
		}
	}

	private static void CopyDirectoryRecursively(string sourceDir, string destDir)
	{
		DirectoryInfo directoryInfo = new DirectoryInfo(sourceDir);
		if (!directoryInfo.Exists)
		{
			throw new DirectoryNotFoundException(sourceDir);
		}
		Directory.CreateDirectory(destDir);
		FileInfo[] files = directoryInfo.GetFiles();
		foreach (FileInfo fileInfo in files)
		{
			string destFileName = Path.Combine(destDir, fileInfo.Name);
			fileInfo.CopyTo(destFileName, overwrite: true);
		}
		DirectoryInfo[] directories = directoryInfo.GetDirectories();
		foreach (DirectoryInfo directoryInfo2 in directories)
		{
			string destDir2 = Path.Combine(destDir, directoryInfo2.Name);
			CopyDirectoryRecursively(directoryInfo2.FullName, destDir2);
		}
	}

	private static List<TransferPlanItem> BuildTransferPlan(IList<IStorageItemWithPath> source, IList<string> destination, IList<NameCollisionOption> collisions, bool isMove)
	{
		if (source.Count == 0 || source.Count != destination.Count || source.Count != collisions.Count)
		{
			return new List<TransferPlanItem>();
		}
		List<TransferPlanItem> list = new List<TransferPlanItem>(source.Count);
		for (int i = 0; i < source.Count; i++)
		{
			string text = ResolveDestinationPath(source[i], destination[i], collisions[i], isMove);
			if (text == null)
			{
				return new List<TransferPlanItem>();
			}
			list.Add(new TransferPlanItem(source[i], text, collisions[i]));
		}
		return list;
	}

	private static string? ResolveDestinationPath(IStorageItemWithPath source, string destination, NameCollisionOption collision, bool isMove)
	{
		if (source == null || string.IsNullOrWhiteSpace(source.Path) || string.IsNullOrWhiteSpace(destination))
		{
			return null;
		}
		string text = destination;
		if (string.Equals(source.Path, text, StringComparison.Ordinal) && (isMove || collision != NameCollisionOption.GenerateUniqueName))
		{
			return null;
		}
		if (source.ItemType == FilesystemItemType.Directory && IsSubPath(text, source.Path) && !string.Equals(source.Path, text, StringComparison.Ordinal))
		{
			return null;
		}
		if (collision == NameCollisionOption.GenerateUniqueName && PathExists(text))
		{
			string directoryName = Path.GetDirectoryName(text);
			if (string.IsNullOrEmpty(directoryName))
			{
				return null;
			}
			string fileName = Path.GetFileName(source.Path.TrimEnd(Path.DirectorySeparatorChar));
			text = FilesystemHelpers.GenerateUniqueName(directoryName, fileName, source.ItemType == FilesystemItemType.File).path;
		}
		if (collision == NameCollisionOption.FailIfExists && PathExists(text))
		{
			return null;
		}
		return text;
	}

	private static bool IsSubPath(string path, string parentPath)
	{
		string text = Path.GetFullPath(path).TrimEnd(Path.DirectorySeparatorChar) + Path.DirectorySeparatorChar;
		string value = Path.GetFullPath(parentPath).TrimEnd(Path.DirectorySeparatorChar) + Path.DirectorySeparatorChar;
		return text.StartsWith(value, StringComparison.OrdinalIgnoreCase);
	}

	private static TransferTotals CalculateTransferTotals(IEnumerable<TransferPlanItem> plan, IProgress<FileOperationProgress>? progress, CancellationToken cancellationToken)
	{
		long num = 0L;
		long num2 = 0L;
		foreach (TransferPlanItem item in plan)
		{
			foreach (string item2 in EnumerateTransferPaths(item.Source.Path, cancellationToken))
			{
				cancellationToken.ThrowIfCancellationRequested();
				num2++;
				num += TryGetFileLength(item2);
				progress?.Report(new FileOperationProgress
				{
					IsPreparing = true,
					TotalBytes = num,
					TotalItems = num2,
					CurrentPath = item.Source.Path
				});
			}
		}
		return new TransferTotals(num, num2);
	}

	private static IEnumerable<string> EnumerateTransferPaths(string root, CancellationToken cancellationToken)
	{
		cancellationToken.ThrowIfCancellationRequested();
		if (File.Exists(root))
		{
			yield return root;
		}
		else
		{
			if (!Directory.Exists(root))
			{
				yield break;
			}
			yield return root;
			if (IsSymbolicLink(root))
			{
				yield break;
			}
			Stack<string> stack = new Stack<string>();
			stack.Push(root);
			while (stack.Count > 0)
			{
				cancellationToken.ThrowIfCancellationRequested();
				string current = stack.Pop();
				string[] array;
				try
				{
					array = Directory.GetFiles(current);
				}
				catch
				{
					array = Array.Empty<string>();
				}
				string[] array2 = array;
				foreach (string text in array2)
				{
					cancellationToken.ThrowIfCancellationRequested();
					yield return text;
				}
				string[] array3;
				try
				{
					array3 = Directory.GetDirectories(current);
				}
				catch
				{
					array3 = Array.Empty<string>();
				}
				array2 = array3;
				foreach (string directory in array2)
				{
					cancellationToken.ThrowIfCancellationRequested();
					yield return directory;
					if (!IsSymbolicLink(directory))
					{
						stack.Push(directory);
					}
				}
			}
		}
	}

	private static long TryGetFileLength(string path)
	{
		try
		{
			if (File.Exists(path))
			{
				return new FileInfo(path).Length;
			}
		}
		catch
		{
		}
		return 0L;
	}

	private static async Task<IStorageItemWithPath?> CopyResolvedAsync(IStorageItemWithPath source, string destination, bool replace, TransferProgressState progressState, string displayPath, CancellationToken cancellationToken)
	{
		if (source.ItemType == FilesystemItemType.Directory)
		{
			await CopyDirectoryAsync(source.Path, destination, replace, progressState, displayPath, cancellationToken);
			return StorageHelpers.FromPathAndType(destination, FilesystemItemType.Directory);
		}
		await CopyFileAsync(source.Path, destination, replace, progressState, displayPath, cancellationToken);
		return StorageHelpers.FromPathAndType(destination, FilesystemItemType.File);
	}

	private static async Task CopyDirectoryAsync(string sourcePath, string destinationPath, bool replace, TransferProgressState progressState, string displayPath, CancellationToken cancellationToken)
	{
		string copyDestination = destinationPath;
		string tempDestination = null;
		if (replace && PathExists(destinationPath))
		{
			tempDestination = CreateTemporarySiblingPath(destinationPath);
			copyDestination = tempDestination;
		}
		try
		{
			if (IsSymbolicLink(sourcePath))
			{
				CopySymbolicLink(sourcePath, copyDestination);
			}
			else
			{
				await CopyDirectoryContentsAsync(sourcePath, copyDestination, progressState, displayPath, cancellationToken);
				TryCopyDirectoryMetadata(sourcePath, copyDestination);
			}
			progressState.AddItem(displayPath);
			if (tempDestination != null)
			{
				ReplacePath(tempDestination, destinationPath);
			}
		}
		catch
		{
			DeletePathQuietly(copyDestination);
			throw;
		}
	}

	private static async Task CopyDirectoryContentsAsync(string sourcePath, string destinationPath, TransferProgressState progressState, string displayPath, CancellationToken cancellationToken)
	{
		cancellationToken.ThrowIfCancellationRequested();
		Directory.CreateDirectory(destinationPath);
		string[] files = Directory.GetFiles(sourcePath);
		foreach (string text in files)
		{
			cancellationToken.ThrowIfCancellationRequested();
			string destinationPath2 = Path.Combine(destinationPath, Path.GetFileName(text));
			await CopyFileAsync(text, destinationPath2, replace: false, progressState, displayPath, cancellationToken);
		}
		files = Directory.GetDirectories(sourcePath);
		foreach (string directory in files)
		{
			cancellationToken.ThrowIfCancellationRequested();
			string targetDirectoryPath = Path.Combine(destinationPath, Path.GetFileName(directory));
			if (IsSymbolicLink(directory))
			{
				CopySymbolicLink(directory, targetDirectoryPath);
				progressState.AddItem(displayPath);
			}
			else
			{
				await CopyDirectoryContentsAsync(directory, targetDirectoryPath, progressState, displayPath, cancellationToken);
				TryCopyDirectoryMetadata(directory, targetDirectoryPath);
				progressState.AddItem(displayPath);
			}
		}
	}

	private static async Task CopyFileAsync(string sourcePath, string destinationPath, bool replace, TransferProgressState progressState, string displayPath, CancellationToken cancellationToken)
	{
		string directoryName = Path.GetDirectoryName(destinationPath);
		if (!string.IsNullOrEmpty(directoryName))
		{
			Directory.CreateDirectory(directoryName);
		}
		string copyDestination = destinationPath;
		string tempDestination = null;
		if (replace && PathExists(destinationPath))
		{
			tempDestination = CreateTemporarySiblingPath(destinationPath);
			copyDestination = tempDestination;
		}
		try
		{
			if (IsSymbolicLink(sourcePath))
			{
				CopySymbolicLink(sourcePath, copyDestination);
				progressState.AddItem(displayPath);
				if (tempDestination != null)
				{
					ReplacePath(tempDestination, destinationPath);
				}
				return;
			}
			await using FileStream sourceStream = new FileStream(sourcePath, FileMode.Open, FileAccess.Read, FileShare.Read, 1048576, FileOptions.Asynchronous | FileOptions.SequentialScan);
			await using FileStream destinationStream = new FileStream(copyDestination, FileMode.CreateNew, FileAccess.Write, FileShare.None, 1048576, FileOptions.Asynchronous | FileOptions.SequentialScan);
			byte[] buffer = new byte[1048576];
			while (true)
			{
				int num;
				int bytesRead = (num = await sourceStream.ReadAsync(buffer, cancellationToken));
				if (num <= 0)
				{
					break;
				}
				cancellationToken.ThrowIfCancellationRequested();
				await destinationStream.WriteAsync(buffer.AsMemory(0, bytesRead), cancellationToken);
				progressState.AddBytes(bytesRead, displayPath);
			}
			TryCopyFileMetadata(sourcePath, copyDestination);
			progressState.AddItem(displayPath);
			if (tempDestination != null)
			{
				ReplacePath(tempDestination, destinationPath);
			}
		}
		catch
		{
			DeletePathQuietly(copyDestination);
			throw;
		}
	}

	private static bool TryMoveDirect(TransferPlanItem item, out IStorageHistory? history)
	{
		history = null;
		try
		{
			string directoryName = Path.GetDirectoryName(item.Destination);
			if (!string.IsNullOrEmpty(directoryName))
			{
				Directory.CreateDirectory(directoryName);
			}
			if (item.Source.ItemType == FilesystemItemType.Directory)
			{
				if (PathExists(item.Destination))
				{
					return false;
				}
				Directory.Move(item.Source.Path, item.Destination);
			}
			else
			{
				File.Move(item.Source.Path, item.Destination, item.Collision == NameCollisionOption.ReplaceExisting);
			}
			IStorageItemWithPath destination = StorageHelpers.FromPathAndType(item.Destination, item.Source.ItemType);
			history = new StorageHistory(FileOperationType.Move, item.Source, destination);
			return true;
		}
		catch
		{
			return false;
		}
	}

	private static string[] SafeGetFiles(string path)
	{
		try
		{
			return Directory.GetFiles(path);
		}
		catch
		{
			return Array.Empty<string>();
		}
	}

	private static string[] SafeGetDirectories(string path)
	{
		try
		{
			return Directory.GetDirectories(path);
		}
		catch
		{
			return Array.Empty<string>();
		}
	}

	private static bool PathExists(string path)
	{
		if (!File.Exists(path))
		{
			return Directory.Exists(path);
		}
		return true;
	}

	private static bool IsSymbolicLink(string path)
	{
		try
		{
			return (File.GetAttributes(path) & FileAttributes.ReparsePoint) != 0;
		}
		catch
		{
			return false;
		}
	}

	private static void CopySymbolicLink(string sourcePath, string destinationPath)
	{
		string directoryName = Path.GetDirectoryName(destinationPath);
		if (!string.IsNullOrEmpty(directoryName))
		{
			Directory.CreateDirectory(directoryName);
		}
		FileSystemInfo obj = ((Directory.Exists(sourcePath) && !File.Exists(sourcePath)) ? ((FileSystemInfo)new DirectoryInfo(sourcePath)) : ((FileSystemInfo)new FileInfo(sourcePath)));
		string linkTarget = obj.LinkTarget;
		if (string.IsNullOrEmpty(linkTarget))
		{
			throw new IOException(sourcePath);
		}
		if (obj is DirectoryInfo)
		{
			Directory.CreateSymbolicLink(destinationPath, linkTarget);
		}
		else
		{
			File.CreateSymbolicLink(destinationPath, linkTarget);
		}
	}

	private static string CreateTemporarySiblingPath(string destinationPath)
	{
		string text = Path.GetDirectoryName(destinationPath);
		if (string.IsNullOrEmpty(text))
		{
			text = Path.GetTempPath();
		}
		string text2;
		do
		{
			text2 = Path.Combine(text, $".xfolder-transfer-{Guid.NewGuid():N}.tmp");
		}
		while (PathExists(text2));
		return text2;
	}

	private static void ReplacePath(string sourcePath, string destinationPath)
	{
		string text = CreateTemporarySiblingPath(destinationPath);
		bool flag = false;
		try
		{
			if (PathExists(destinationPath))
			{
				MovePath(destinationPath, text);
				flag = true;
			}
			MovePath(sourcePath, destinationPath);
			if (flag)
			{
				DeletePathQuietly(text);
			}
		}
		catch
		{
			if (flag && !PathExists(destinationPath) && PathExists(text))
			{
				MovePath(text, destinationPath);
			}
			throw;
		}
	}

	private static void MovePath(string sourcePath, string destinationPath)
	{
		if (Directory.Exists(sourcePath) && !File.Exists(sourcePath))
		{
			Directory.Move(sourcePath, destinationPath);
		}
		else
		{
			File.Move(sourcePath, destinationPath);
		}
	}

	private static void DeletePath(string path)
	{
		if (Directory.Exists(path) && !File.Exists(path))
		{
			Directory.Delete(path, recursive: true);
		}
		else if (File.Exists(path))
		{
			File.Delete(path);
		}
	}

	private static void DeletePathQuietly(string path)
	{
		try
		{
			DeletePath(path);
		}
		catch
		{
		}
	}

	private static void TryCopyFileMetadata(string sourcePath, string destinationPath)
	{
		try
		{
			File.SetCreationTimeUtc(destinationPath, File.GetCreationTimeUtc(sourcePath));
			File.SetLastWriteTimeUtc(destinationPath, File.GetLastWriteTimeUtc(sourcePath));
			File.SetAttributes(destinationPath, File.GetAttributes(sourcePath));
		}
		catch
		{
		}
	}

	private static void TryCopyDirectoryMetadata(string sourcePath, string destinationPath)
	{
		try
		{
			Directory.SetCreationTimeUtc(destinationPath, Directory.GetCreationTimeUtc(sourcePath));
			Directory.SetLastWriteTimeUtc(destinationPath, Directory.GetLastWriteTimeUtc(sourcePath));
		}
		catch
		{
		}
	}

	public void Dispose()
	{
	}
}
