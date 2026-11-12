using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Files.Data.Items;
using Files.Helpers.Shared;
using Microsoft.Extensions.Logging;

namespace Files.Utils.Storage.Helpers;

public static class DriveHelpers
{
	private sealed record DiskUtilDriveInfo(string DeviceIdentifier, string VolumeName, string FilesystemName, string BusProtocol, bool IsUsb, bool IsExternalRemovable, bool IsSupportedUsbDrive, long TotalSize, long FreeSpace);

	public const string VolumesRootPath = "/Volumes";

	private static readonly StringComparer PathComparer = StringComparer.Ordinal;

	public static bool IsSupportedPlatform => OperatingSystem.IsMacOS();

	public static string NormalizeVolumePath(string path)
	{
		return PathHelpers.NormalizeDirectoryPath(path);
	}

	public static bool IsCandidateVolumePath(string path)
	{
		string text = NormalizeVolumePath(path);
		if (!string.IsNullOrWhiteSpace(text))
		{
			return text.StartsWith("/Volumes/", StringComparison.Ordinal);
		}
		return false;
	}

	public static IEnumerable<string> GetMountedVolumePaths()
	{
		if (!IsSupportedPlatform || !Directory.Exists("/Volumes"))
		{
			yield break;
		}
		IEnumerable<string> enumerable;
		try
		{
			enumerable = Directory.EnumerateFileSystemEntries("/Volumes");
		}
		catch (DirectoryNotFoundException)
		{
			yield break;
		}
		catch (IOException exception)
		{
			App.Logger.LogWarning(exception, "Failed to enumerate mounted volumes from {VolumesRootPath}", "/Volumes");
			yield break;
		}
		catch (UnauthorizedAccessException exception2)
		{
			App.Logger.LogWarning(exception2, "Access denied when enumerating mounted volumes from {VolumesRootPath}", "/Volumes");
			yield break;
		}
		foreach (string item in enumerable)
		{
			string text = NormalizeVolumePath(item);
			if (!string.IsNullOrWhiteSpace(text) && Directory.Exists(text))
			{
				yield return text;
			}
		}
	}

	public static async IAsyncEnumerable<DriveItem> GetMountedUsbDrivesAsync([EnumeratorCancellation] CancellationToken cancellationToken = default(CancellationToken))
	{
		foreach (string item in GetMountedVolumePaths().Distinct(PathComparer))
		{
			cancellationToken.ThrowIfCancellationRequested();
			DriveItem driveItem = await TryCreateDriveItemAsync(item, cancellationToken);
			if (driveItem != null)
			{
				yield return driveItem;
			}
		}
	}

	public static async Task<DriveItem?> TryCreateDriveItemAsync(string volumePath, CancellationToken cancellationToken = default(CancellationToken))
	{
		string normalizedPath = NormalizeVolumePath(volumePath);
		if (!IsCandidateVolumePath(normalizedPath) || !Directory.Exists(normalizedPath))
		{
			return null;
		}
		DiskUtilDriveInfo diskUtilDriveInfo = await GetDriveMetadataAsync(normalizedPath, cancellationToken);
		if ((object)diskUtilDriveInfo == null || !diskUtilDriveInfo.IsSupportedUsbDrive)
		{
			return null;
		}
		DriveInfo drive = TryCreateDriveInfo(normalizedPath);
		Files.Data.Items.DriveType driveType = GetDriveType(drive, diskUtilDriveInfo.IsExternalRemovable);
		string extendedDriveLabel = GetExtendedDriveLabel(drive, diskUtilDriveInfo.VolumeName);
		long totalSize = ((diskUtilDriveInfo.TotalSize > 0) ? diskUtilDriveInfo.TotalSize : TryGetTotalSize(drive));
		long freeSpace = ((diskUtilDriveInfo.FreeSpace > 0) ? diskUtilDriveInfo.FreeSpace : TryGetAvailableFreeSpace(drive));
		return await DriveItem.CreateFromPropertiesAsync(normalizedPath, diskUtilDriveInfo.DeviceIdentifier, extendedDriveLabel, driveType, diskUtilDriveInfo.FilesystemName, diskUtilDriveInfo.BusProtocol, diskUtilDriveInfo.IsUsb, diskUtilDriveInfo.IsExternalRemovable, totalSize, freeSpace);
	}

	public static async Task RefreshDriveAsync(DriveItem drive, CancellationToken cancellationToken = default(CancellationToken))
	{
		DriveItem driveItem = await TryCreateDriveItemAsync(drive.Path, cancellationToken);
		if (driveItem != null)
		{
			drive.DeviceID = driveItem.DeviceID;
			drive.Text = driveItem.Text;
			drive.Type = driveItem.Type;
			drive.Filesystem = driveItem.Filesystem;
			drive.BusProtocol = driveItem.BusProtocol;
			drive.IsUsb = driveItem.IsUsb;
			drive.IsExternal = driveItem.IsExternal;
			drive.MaxSpace = driveItem.MaxSpace;
			drive.FreeSpace = driveItem.FreeSpace;
			drive.Root = driveItem.Root;
		}
	}

	public static Files.Data.Items.DriveType GetDriveType(DriveInfo? drive, bool isExternalRemovable)
	{
		if (drive == null)
		{
			if (!isExternalRemovable)
			{
				return Files.Data.Items.DriveType.Unknown;
			}
			return Files.Data.Items.DriveType.Removable;
		}
		try
		{
			return drive.DriveType switch
			{
				System.IO.DriveType.CDRom => Files.Data.Items.DriveType.CDRom, 
				System.IO.DriveType.Fixed => isExternalRemovable ? Files.Data.Items.DriveType.Removable : Files.Data.Items.DriveType.Fixed, 
				System.IO.DriveType.Network => Files.Data.Items.DriveType.Network, 
				System.IO.DriveType.NoRootDirectory => Files.Data.Items.DriveType.NoRootDirectory, 
				System.IO.DriveType.Ram => Files.Data.Items.DriveType.Ram, 
				System.IO.DriveType.Removable => Files.Data.Items.DriveType.Removable, 
				_ => isExternalRemovable ? Files.Data.Items.DriveType.Removable : Files.Data.Items.DriveType.Unknown, 
			};
		}
		catch (ArgumentException)
		{
			return isExternalRemovable ? Files.Data.Items.DriveType.Removable : Files.Data.Items.DriveType.Unknown;
		}
		catch (IOException)
		{
			return isExternalRemovable ? Files.Data.Items.DriveType.Removable : Files.Data.Items.DriveType.Unknown;
		}
	}

	public static string GetExtendedDriveLabel(DriveInfo? drive, string fallbackLabel)
	{
		if (drive == null)
		{
			return fallbackLabel;
		}
		try
		{
			return string.IsNullOrWhiteSpace(drive.VolumeLabel) ? fallbackLabel : drive.VolumeLabel;
		}
		catch (ArgumentException)
		{
			return fallbackLabel;
		}
		catch (IOException)
		{
			return fallbackLabel;
		}
		catch (UnauthorizedAccessException)
		{
			return fallbackLabel;
		}
	}

	private static DriveInfo? TryCreateDriveInfo(string path)
	{
		try
		{
			return new DriveInfo(path);
		}
		catch (ArgumentException)
		{
			return null;
		}
		catch (IOException)
		{
			return null;
		}
	}

	private static long TryGetAvailableFreeSpace(DriveInfo? drive)
	{
		if (drive == null)
		{
			return 0L;
		}
		try
		{
			return drive.AvailableFreeSpace;
		}
		catch (ArgumentException)
		{
			return 0L;
		}
		catch (IOException)
		{
			return 0L;
		}
		catch (UnauthorizedAccessException)
		{
			return 0L;
		}
	}

	private static long TryGetTotalSize(DriveInfo? drive)
	{
		if (drive == null)
		{
			return 0L;
		}
		try
		{
			return drive.TotalSize;
		}
		catch (ArgumentException)
		{
			return 0L;
		}
		catch (IOException)
		{
			return 0L;
		}
		catch (UnauthorizedAccessException)
		{
			return 0L;
		}
	}

	private static async Task<DiskUtilDriveInfo?> GetDriveMetadataAsync(string volumePath, CancellationToken cancellationToken)
	{
		ProcessStartInfo processStartInfo = new ProcessStartInfo
		{
			FileName = "/usr/sbin/diskutil",
			UseShellExecute = false,
			RedirectStandardOutput = true,
			RedirectStandardError = true,
			CreateNoWindow = true
		};
		processStartInfo.ArgumentList.Add("info");
		processStartInfo.ArgumentList.Add("-plist");
		processStartInfo.ArgumentList.Add(volumePath);
		using Process process = new Process
		{
			StartInfo = processStartInfo
		};
		try
		{
			if (!process.Start())
			{
				App.Logger.LogWarning("Failed to start diskutil for mounted volume {VolumePath}", volumePath);
				return null;
			}
		}
		catch (InvalidOperationException exception)
		{
			App.Logger.LogWarning(exception, "diskutil could not inspect mounted volume {VolumePath}", volumePath);
			return null;
		}
		catch (Win32Exception exception2)
		{
			App.Logger.LogWarning(exception2, "diskutil is unavailable while inspecting mounted volume {VolumePath}", volumePath);
			return null;
		}
		Task<string> standardOutputTask = process.StandardOutput.ReadToEndAsync(cancellationToken);
		Task<string> standardErrorTask = process.StandardError.ReadToEndAsync(cancellationToken);
		await process.WaitForExitAsync(cancellationToken);
		string standardOutput = await standardOutputTask;
		string text = await standardErrorTask;
		if (process.ExitCode != 0)
		{
			App.Logger.LogWarning("diskutil exited with code {ExitCode} for mounted volume {VolumePath}: {StandardError}", process.ExitCode, volumePath, text.Trim());
			return null;
		}
		return ParseDriveMetadata(volumePath, standardOutput);
	}

	private static DiskUtilDriveInfo? ParseDriveMetadata(string volumePath, string plistContent)
	{
		XDocument xDocument;
		try
		{
			xDocument = XDocument.Parse(plistContent);
		}
		catch (XmlException exception)
		{
			App.Logger.LogWarning(exception, "Failed to parse diskutil plist for mounted volume {VolumePath}", volumePath);
			return null;
		}
		XElement xElement = xDocument.Root?.Element("dict");
		if (xElement == null)
		{
			App.Logger.LogWarning("diskutil returned an unexpected plist for mounted volume {VolumePath}", volumePath);
			return null;
		}
		Dictionary<string, string> values = ParseScalarDictionary(xElement);
		string text = GetString(values, "DeviceIdentifier");
		string text2 = GetString(values, "VolumeName");
		string filesystemName = GetString(values, "FilesystemName");
		string text3 = GetString(values, "BusProtocol");
		bool flag = GetBool(values, "Internal");
		bool flag2 = GetBool(values, "Removable");
		bool flag3 = GetBool(values, "RemovableMedia");
		bool flag4 = GetBool(values, "RemovableMediaOrExternalDevice");
		bool flag5 = GetBool(values, "Ejectable");
		long totalSize = GetLong(values, "TotalSize");
		long freeSpace = GetLong(values, "FreeSpace");
		bool flag6 = text3.Contains("USB", StringComparison.OrdinalIgnoreCase);
		bool flag7 = !flag && (flag6 | flag2 | flag3 | flag4 | flag5);
		bool isSupportedUsbDrive = flag6 || (flag7 & flag4);
		return new DiskUtilDriveInfo(string.IsNullOrWhiteSpace(text) ? volumePath : text, string.IsNullOrWhiteSpace(text2) ? Path.GetFileName(volumePath) : text2, filesystemName, text3, flag6, flag7, isSupportedUsbDrive, totalSize, freeSpace);
	}

	private static Dictionary<string, string> ParseScalarDictionary(XElement dictionary)
	{
		Dictionary<string, string> dictionary2 = new Dictionary<string, string>(StringComparer.Ordinal);
		List<XElement> list = dictionary.Elements().ToList();
		for (int i = 0; i < list.Count - 1; i++)
		{
			if (string.Equals(list[i].Name.LocalName, "key", StringComparison.Ordinal))
			{
				string value = list[i].Value;
				XElement xElement = list[i + 1];
				Dictionary<string, string> dictionary3 = dictionary2;
				string key = value;
				string localName = xElement.Name.LocalName;
				string value2;
				if (localName == "true")
				{
					value2 = bool.TrueString;
				}
				else
				{
					value2 = ((!(localName == "false")) ? xElement.Value : bool.FalseString);
				}
				dictionary3[key] = value2;
			}
		}
		return dictionary2;
	}

	private static string GetString(IReadOnlyDictionary<string, string> values, string key)
	{
		if (!values.TryGetValue(key, out string value))
		{
			return string.Empty;
		}
		return value;
	}

	private static bool GetBool(IReadOnlyDictionary<string, string> values, string key)
	{
		string value;
		bool result = default;
		return (values.TryGetValue(key, out value) && bool.TryParse(value, out result)) & result;
	}

	private static long GetLong(IReadOnlyDictionary<string, string> values, string key)
	{
		if (!values.TryGetValue(key, out string value) || !long.TryParse(value, out var result))
		{
			return 0L;
		}
		return result;
	}
}
