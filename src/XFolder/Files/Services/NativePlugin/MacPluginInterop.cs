using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using Avalonia.Media.Imaging;

namespace Files.Services.NativePlugin;

public class MacPluginInterop
{
	private const string LibName = "FilesMacPlugin.dylib";

	static MacPluginInterop()
	{
		NativeLibrary.SetDllImportResolver(typeof(MacPluginInterop).Assembly, ImportResolver);
	}

	private static nint ImportResolver(string libraryName, Assembly assembly, DllImportSearchPath? searchPath)
	{
		if (libraryName != "FilesMacPlugin.dylib")
		{
			return IntPtr.Zero;
		}
		if (OperatingSystem.IsMacOS())
		{
			string[] array = new string[4]
			{
				Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "FilesMacPlugin.dylib"),
				Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "../MonoBundle", "FilesMacPlugin.dylib"),
				Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "../Resources", "FilesMacPlugin.dylib"),
				Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "../Frameworks", "FilesMacPlugin.dylib")
			};
			foreach (string text in array)
			{
				if (File.Exists(text) && NativeLibrary.TryLoad(text, out var handle))
				{
					return handle;
				}
			}
		}
		return IntPtr.Zero;
	}

	[DllImport("FilesMacPlugin.dylib")]
	public static extern void SetClipboardFilesWithMode(nint array, int count, int isCut);

	[DllImport("FilesMacPlugin.dylib")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ClipboardIsCutMode();

	[DllImport("FilesMacPlugin.dylib")]
	public static extern void ClearClipboard();

	[DllImport("FilesMacPlugin.dylib")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool IsShiftKeyPressed();

	[DllImport("FilesMacPlugin.dylib")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool IsControlKeyPressed();

	[DllImport("FilesMacPlugin.dylib")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool IsOptionKeyPressed();

	[DllImport("FilesMacPlugin.dylib")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool IsCommandKeyPressed();

	[DllImport("FilesMacPlugin.dylib")]
	[return: MarshalAs(UnmanagedType.I1)]
	public static extern bool ClipboardHasFiles();

	[DllImport("FilesMacPlugin.dylib")]
	public static extern nint GetClipboardFiles(out int count);

	[DllImport("FilesMacPlugin.dylib")]
	public static extern void FreeClipboardFiles(nint array, int count);

	[DllImport("FilesMacPlugin.dylib")]
	public static extern nint GetFileIconBytesByExt([MarshalAs(UnmanagedType.LPStr)] string ext, int width, int height, out int length);

	[DllImport("FilesMacPlugin.dylib")]
	public static extern void FreeIconBytes(nint ptr);

	[DllImport("FilesMacPlugin.dylib")]
	public static extern nint GetDirectoryIconBytes(int width, int height, out int length);

	[DllImport("FilesMacPlugin.dylib")]
	public static extern int MoveToTrash([MarshalAs(UnmanagedType.LPStr)] string path, out nint resultPath);

	[DllImport("FilesMacPlugin.dylib")]
	public static extern void FreeString(nint ptr);

	[DllImport("FilesMacPlugin.dylib", EntryPoint = "GetVolumeName")]
	private static extern nint GetVolumeNameNative([MarshalAs(UnmanagedType.LPUTF8Str)] string path);

	[DllImport("FilesMacPlugin.dylib", EntryPoint = "EjectVolume")]
	private static extern int EjectVolumeNative([MarshalAs(UnmanagedType.LPUTF8Str)] string path);

	[DllImport("FilesMacPlugin.dylib", EntryPoint = "GetLocalizedTypeDescription")]
	private static extern nint GetLocalizedTypeDescriptionNative([MarshalAs(UnmanagedType.LPUTF8Str)] string path);

	[DllImport("FilesMacPlugin.dylib", EntryPoint = "RemoveNativeMenuItemsAfter")]
	private static extern int RemoveNativeMenuItemsAfterNative([MarshalAs(UnmanagedType.LPUTF8Str)] string menuTitle, [MarshalAs(UnmanagedType.LPUTF8Str)] string itemTitle);

	[DllImport("FilesMacPlugin.dylib", EntryPoint = "InstallMainMenuKeyEquivalentPassthrough")]
	private static extern int InstallMainMenuKeyEquivalentPassthroughNative(nint topLevelMenuTitles, int topLevelMenuTitleCount, nint itemTitles, int itemTitleCount);

	[DllImport("FilesMacPlugin.dylib", EntryPoint = "ShowItemInfoWindows")]
	private static extern int ShowItemInfoWindowsNative(nint paths, int pathCount);

	public static int RemoveNativeMenuItemsAfter(string menuTitle, string itemTitle)
	{
		if (!OperatingSystem.IsMacOS() || string.IsNullOrWhiteSpace(menuTitle) || string.IsNullOrWhiteSpace(itemTitle))
		{
			return 0;
		}
		try
		{
			return RemoveNativeMenuItemsAfterNative(menuTitle, itemTitle);
		}
		catch (DllNotFoundException)
		{
			return 0;
		}
		catch (EntryPointNotFoundException)
		{
			return 0;
		}
	}

	public static int InstallMainMenuKeyEquivalentPassthrough(IEnumerable<string> topLevelMenuTitles, IEnumerable<string> itemTitles)
	{
		if (!OperatingSystem.IsMacOS())
		{
			return 0;
		}
		List<string> list = topLevelMenuTitles?.Where((string title) => !string.IsNullOrWhiteSpace(title)).ToList() ?? new List<string>();
		List<string> list2 = itemTitles?.Where((string title) => !string.IsNullOrWhiteSpace(title)).ToList() ?? new List<string>();
		List<nint> list3 = new List<nint>();
		nint num = IntPtr.Zero;
		nint num2 = IntPtr.Zero;
		try
		{
			num = AllocateUtf8StringArray(list, list3);
			num2 = AllocateUtf8StringArray(list2, list3);
			return InstallMainMenuKeyEquivalentPassthroughNative(num, list.Count, num2, list2.Count);
		}
		catch (DllNotFoundException)
		{
			return 0;
		}
		catch (EntryPointNotFoundException)
		{
			return 0;
		}
		finally
		{
			foreach (nint item in list3)
			{
				if (item != IntPtr.Zero)
				{
					Marshal.FreeHGlobal(item);
				}
			}
			if (num != IntPtr.Zero)
			{
				Marshal.FreeHGlobal(num);
			}
			if (num2 != IntPtr.Zero)
			{
				Marshal.FreeHGlobal(num2);
			}
		}
	}

	public static bool ShowItemInfoWindows(IEnumerable<string> paths)
	{
		if (!OperatingSystem.IsMacOS())
		{
			return false;
		}
		List<string> list = paths?.Where((string path) => !string.IsNullOrWhiteSpace(path)).Distinct(StringComparer.OrdinalIgnoreCase).ToList() ?? new List<string>();
		if (list.Count == 0)
		{
			return false;
		}
		List<nint> list2 = new List<nint>();
		nint num = IntPtr.Zero;
		try
		{
			num = AllocateUtf8StringArray(list, list2);
			return ShowItemInfoWindowsNative(num, list.Count) == 1;
		}
		catch (DllNotFoundException)
		{
			return false;
		}
		catch (EntryPointNotFoundException)
		{
			return false;
		}
		catch (BadImageFormatException)
		{
			return false;
		}
		finally
		{
			foreach (nint item in list2)
			{
				if (item != IntPtr.Zero)
				{
					Marshal.FreeHGlobal(item);
				}
			}
			if (num != IntPtr.Zero)
			{
				Marshal.FreeHGlobal(num);
			}
		}
	}

	private static nint AllocateUtf8StringArray(IReadOnlyList<string> strings, List<nint> allocatedStrings)
	{
		nint num = Marshal.AllocHGlobal(IntPtr.Size * (strings.Count + 1));
		for (int i = 0; i < strings.Count; i++)
		{
			byte[] bytes = Encoding.UTF8.GetBytes(strings[i] + "\0");
			nint num2 = Marshal.AllocHGlobal(bytes.Length);
			Marshal.Copy(bytes, 0, num2, bytes.Length);
			allocatedStrings.Add(num2);
			Marshal.WriteIntPtr(num, i * IntPtr.Size, num2);
		}
		Marshal.WriteIntPtr(num, strings.Count * IntPtr.Size, IntPtr.Zero);
		return num;
	}

	public static string? GetVolumeName(string path)
	{
		nint volumeNameNative = GetVolumeNameNative(path);
		if (volumeNameNative == IntPtr.Zero)
		{
			return null;
		}
		string result = Marshal.PtrToStringUTF8(volumeNameNative);
		FreeString(volumeNameNative);
		return result;
	}

	public static bool EjectVolume(string path)
	{
		if (!OperatingSystem.IsMacOS() || string.IsNullOrWhiteSpace(path) || !Directory.Exists(path))
		{
			return false;
		}
		try
		{
			return EjectVolumeNative(path) == 1;
		}
		catch (DllNotFoundException)
		{
			return false;
		}
		catch (EntryPointNotFoundException)
		{
			return false;
		}
		catch (BadImageFormatException)
		{
			return false;
		}
	}

	public static string? GetLocalizedTypeDescription(string path)
	{
		if (!OperatingSystem.IsMacOS() || string.IsNullOrWhiteSpace(path))
		{
			return null;
		}
		nint localizedTypeDescriptionNative = GetLocalizedTypeDescriptionNative(path);
		if (localizedTypeDescriptionNative == IntPtr.Zero)
		{
			return null;
		}
		string result = Marshal.PtrToStringUTF8(localizedTypeDescriptionNative);
		FreeString(localizedTypeDescriptionNative);
		return result;
	}

	[DllImport("FilesMacPlugin.dylib")]
	public static extern int OpenFileWithDefaultApplication([MarshalAs(UnmanagedType.LPUTF8Str)] string path);

	public static bool OpenFileWithDefaultApp(string path)
	{
		if (string.IsNullOrWhiteSpace(path))
		{
			return false;
		}
		if (File.Exists(path) || (Directory.Exists(path) && path.EndsWith(".app", StringComparison.OrdinalIgnoreCase)))
		{
			return OpenFileWithDefaultApplication(path) == 1;
		}
		return false;
	}

	[DllImport("FilesMacPlugin.dylib", EntryPoint = "ToggleQuickLookPreview")]
	private static extern int ToggleQuickLookPreviewNative([MarshalAs(UnmanagedType.LPUTF8Str)] string path);

	[DllImport("FilesMacPlugin.dylib", EntryPoint = "SwitchQuickLookPreview")]
	private static extern int SwitchQuickLookPreviewNative([MarshalAs(UnmanagedType.LPUTF8Str)] string path);

	[DllImport("FilesMacPlugin.dylib", EntryPoint = "IsQuickLookPreviewAvailable")]
	private static extern int IsQuickLookPreviewAvailableNative();

	[DllImport("FilesMacPlugin.dylib", EntryPoint = "IsQuickLookPreviewOpen")]
	private static extern int IsQuickLookPreviewOpenNative();

	public static bool ToggleQuickLookPreview(string path)
	{
		if (!OperatingSystem.IsMacOS() || string.IsNullOrWhiteSpace(path))
		{
			return false;
		}
		if (!File.Exists(path) && !Directory.Exists(path))
		{
			return false;
		}
		return ToggleQuickLookPreviewNative(path) == 1;
	}

	public static bool SwitchQuickLookPreview(string path)
	{
		if (!OperatingSystem.IsMacOS() || string.IsNullOrWhiteSpace(path))
		{
			return false;
		}
		if (!File.Exists(path) && !Directory.Exists(path))
		{
			return false;
		}
		return SwitchQuickLookPreviewNative(path) == 1;
	}

	public static bool IsQuickLookPreviewAvailable()
	{
		if (!OperatingSystem.IsMacOS())
		{
			return false;
		}
		return IsQuickLookPreviewAvailableNative() == 1;
	}

	public static bool IsQuickLookPreviewOpen()
	{
		if (!OperatingSystem.IsMacOS())
		{
			return false;
		}
		return IsQuickLookPreviewOpenNative() == 1;
	}

	[DllImport("FilesMacPlugin.dylib", EntryPoint = "MoveItem")]
	public static extern int MoveItemNative([MarshalAs(UnmanagedType.LPStr)] string src, [MarshalAs(UnmanagedType.LPStr)] string dst, int replace);

	[DllImport("FilesMacPlugin.dylib", EntryPoint = "CopyItem")]
	public static extern int CopyItemNative([MarshalAs(UnmanagedType.LPStr)] string src, [MarshalAs(UnmanagedType.LPStr)] string dst, int replace);

	public static (bool success, string? trashPath) MoveFileOrDirectoryToTrash(string path)
	{
		nint resultPath = IntPtr.Zero;
		int num = MoveToTrash(path, out resultPath);
		string item = null;
		if (resultPath != IntPtr.Zero)
		{
			item = Marshal.PtrToStringAnsi(resultPath);
			FreeString(resultPath);
		}
		return (success: num == 1, trashPath: item);
	}

	public static bool MoveItem(string src, string dst, bool replace = false)
	{
		if (string.IsNullOrWhiteSpace(src) || string.IsNullOrWhiteSpace(dst))
		{
			return false;
		}
		return MoveItemNative(src, dst, replace ? 1 : 0) == 1;
	}

	public static bool CopyItem(string src, string dst, bool replace = false)
	{
		if (string.IsNullOrWhiteSpace(src) || string.IsNullOrWhiteSpace(dst))
		{
			return false;
		}
		return CopyItemNative(src, dst, replace ? 1 : 0) == 1;
	}

	public static List<string> GetClipboardFileList()
	{
		int count = 0;
		nint clipboardFiles = GetClipboardFiles(out count);
		List<string> list = new List<string>();
		if (clipboardFiles != IntPtr.Zero && count > 0)
		{
			for (int i = 0; i < count; i++)
			{
				nint num = Marshal.ReadIntPtr(clipboardFiles, i * IntPtr.Size);
				if (num != IntPtr.Zero)
				{
					string text = Marshal.PtrToStringAnsi(num);
					if (text != null)
					{
						list.Add(text);
					}
				}
			}
			FreeClipboardFiles(clipboardFiles, count);
		}
		return list;
	}

	public static void SetClipboardFileList(IEnumerable<string> paths, bool isCut = false)
	{
		List<string> list = paths?.ToList() ?? new List<string>();
		if (list.Count == 0)
		{
			return;
		}
		nint num = Marshal.AllocHGlobal(IntPtr.Size * (list.Count + 1));
		List<nint> list2 = new List<nint>(list.Count);
		try
		{
			for (int i = 0; i < list.Count; i++)
			{
				byte[] bytes = Encoding.UTF8.GetBytes(list[i] + "\0");
				nint num2 = Marshal.AllocHGlobal(bytes.Length);
				Marshal.Copy(bytes, 0, num2, bytes.Length);
				list2.Add(num2);
				Marshal.WriteIntPtr(num, i * IntPtr.Size, num2);
			}
			Marshal.WriteIntPtr(num, list.Count * IntPtr.Size, IntPtr.Zero);
			SetClipboardFilesWithMode(num, list.Count, isCut ? 1 : 0);
		}
		finally
		{
			foreach (nint item in list2)
			{
				if (item != IntPtr.Zero)
				{
					Marshal.FreeHGlobal(item);
				}
			}
			if (num != IntPtr.Zero)
			{
				Marshal.FreeHGlobal(num);
			}
		}
	}

	public static byte[] GetFileIconBytesByExt(string ext, int width = 64, int height = 64)
	{
		int length = 0;
		nint fileIconBytesByExt = GetFileIconBytesByExt(ext, width, height, out length);
		if (fileIconBytesByExt == IntPtr.Zero || length == 0)
		{
			return Array.Empty<byte>();
		}
		byte[] array = new byte[length];
		Marshal.Copy(fileIconBytesByExt, array, 0, length);
		FreeIconBytes(fileIconBytesByExt);
		return array;
	}

	[DllImport("FilesMacPlugin.dylib")]
	public static extern nint GetFileIconBytesByPath([MarshalAs(UnmanagedType.LPUTF8Str)] string path, int width, int height, out int length);

	public static Bitmap? GetFileIconBitmapByExt(string ext, int width = 64, int height = 64)
	{
		byte[] fileIconBytesByExt = GetFileIconBytesByExt(ext, width, height);
		if (fileIconBytesByExt.Length == 0)
		{
			return null;
		}
		using MemoryStream stream = new MemoryStream(fileIconBytesByExt);
		return new Bitmap(stream);
	}

	public static Bitmap? GetFileIconBitmapByPath(string path, int width = 64, int height = 64)
	{
		int length = 0;
		nint fileIconBytesByPath = GetFileIconBytesByPath(path, width, height, out length);
		if (fileIconBytesByPath == IntPtr.Zero || length == 0)
		{
			return null;
		}
		byte[] array = new byte[length];
		Marshal.Copy(fileIconBytesByPath, array, 0, length);
		FreeIconBytes(fileIconBytesByPath);
		using MemoryStream stream = new MemoryStream(array);
		return new Bitmap(stream);
	}

	public static Bitmap? GetMacDirectoryIconBitmap(int width = 64, int height = 64)
	{
		int length = 0;
		nint directoryIconBytes = GetDirectoryIconBytes(width, height, out length);
		if (directoryIconBytes == IntPtr.Zero || length == 0)
		{
			return null;
		}
		byte[] array = new byte[length];
		Marshal.Copy(directoryIconBytes, array, 0, length);
		FreeIconBytes(directoryIconBytes);
		using MemoryStream stream = new MemoryStream(array);
		return new Bitmap(stream);
	}

	[DllImport("FilesMacPlugin.dylib")]
	public static extern nint GetFolderIconBytes([MarshalAs(UnmanagedType.LPUTF8Str)] string path, int width, int height, out int length);

	public static Bitmap? GetFolderIconBitmap(string path, int width = 64, int height = 64)
	{
		int length = 0;
		nint folderIconBytes = GetFolderIconBytes(path, width, height, out length);
		if (length == -1)
		{
			return null;
		}
		if (folderIconBytes == IntPtr.Zero || length == 0)
		{
			return null;
		}
		byte[] array = new byte[length];
		Marshal.Copy(folderIconBytes, array, 0, length);
		FreeIconBytes(folderIconBytes);
		using MemoryStream stream = new MemoryStream(array);
		return new Bitmap(stream);
	}

	[DllImport("FilesMacPlugin.dylib")]
	public static extern nint GetSidebarIconBytesByPath([MarshalAs(UnmanagedType.LPUTF8Str)] string path, int width, int height, out int length);

	public static Bitmap? GetSidebarIconBitmapByPath(string path, int width = 64, int height = 64)
	{
		int length = 0;
		nint sidebarIconBytesByPath = GetSidebarIconBytesByPath(path, width, height, out length);
		if (sidebarIconBytesByPath == IntPtr.Zero || length == 0)
		{
			return null;
		}
		byte[] array = new byte[length];
		Marshal.Copy(sidebarIconBytesByPath, array, 0, length);
		FreeIconBytes(sidebarIconBytesByPath);
		using MemoryStream stream = new MemoryStream(array);
		return new Bitmap(stream);
	}

	[DllImport("FilesMacPlugin.dylib")]
	public static extern nint GetImageThumbnailBytes([MarshalAs(UnmanagedType.LPUTF8Str)] string path, int width, int height, out int length);

	[DllImport("FilesMacPlugin.dylib", EntryPoint = "CanConvertImageToFormat")]
	private static extern int CanConvertImageToFormatNative([MarshalAs(UnmanagedType.LPUTF8Str)] string path, int targetFormat);

	[DllImport("FilesMacPlugin.dylib", EntryPoint = "ConvertImageToFormat")]
	private static extern int ConvertImageToFormatNative([MarshalAs(UnmanagedType.LPUTF8Str)] string sourcePath, [MarshalAs(UnmanagedType.LPUTF8Str)] string destinationPath, int targetFormat);

	public static Bitmap? GetImageThumbnailBitmap(string path, int width = 64, int height = 64)
	{
		int length = 0;
		nint imageThumbnailBytes = GetImageThumbnailBytes(path, width, height, out length);
		if (imageThumbnailBytes == IntPtr.Zero || length == 0)
		{
			return null;
		}
		byte[] array = new byte[length];
		Marshal.Copy(imageThumbnailBytes, array, 0, length);
		FreeIconBytes(imageThumbnailBytes);
		using MemoryStream stream = new MemoryStream(array);
		return new Bitmap(stream);
	}

	public static bool CanConvertImageToFormat(string path, int targetFormat)
	{
		if (!OperatingSystem.IsMacOS() || string.IsNullOrWhiteSpace(path) || !File.Exists(path))
		{
			return false;
		}
		try
		{
			return CanConvertImageToFormatNative(path, targetFormat) == 1;
		}
		catch (DllNotFoundException)
		{
			return false;
		}
		catch (EntryPointNotFoundException)
		{
			return false;
		}
		catch (BadImageFormatException)
		{
			return false;
		}
	}

	public static bool ConvertImageToFormat(string sourcePath, string destinationPath, int targetFormat)
	{
		if (!OperatingSystem.IsMacOS() || string.IsNullOrWhiteSpace(sourcePath) || string.IsNullOrWhiteSpace(destinationPath) || !File.Exists(sourcePath))
		{
			return false;
		}
		try
		{
			return ConvertImageToFormatNative(sourcePath, destinationPath, targetFormat) == 1;
		}
		catch (DllNotFoundException)
		{
			return false;
		}
		catch (EntryPointNotFoundException)
		{
			return false;
		}
		catch (BadImageFormatException)
		{
			return false;
		}
	}

	[DllImport("FilesMacPlugin.dylib")]
	public static extern int ShowAlertDialog([MarshalAs(UnmanagedType.LPUTF8Str)] string title, [MarshalAs(UnmanagedType.LPUTF8Str)] string message, [MarshalAs(UnmanagedType.LPUTF8Str)] string button1, [MarshalAs(UnmanagedType.LPUTF8Str)] string? button2 = null, [MarshalAs(UnmanagedType.LPUTF8Str)] string? button3 = null);
}
