using System;
using System.IO;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using Files.Helpers.Shared;
using Files.Utils.Storage.BaseStorageItems;
using Files.Utils.Storage.StorageItems;

namespace Files.Data.Items;

public sealed class DriveItem : ObservableObject
{
	private string path = string.Empty;

	private string deviceId = string.Empty;

	private string text = string.Empty;

	private string filesystem = string.Empty;

	private string busProtocol = string.Empty;

	private DriveType type = DriveType.Unknown;

	private long maxSpace;

	private long freeSpace;

	private bool isUsb;

	private bool isExternal;

	private BaseStorageFolder? root;

	public string Path
	{
		get
		{
			return path;
		}
		set
		{
			SetProperty(ref path, value, "Path");
		}
	}

	public string DeviceID
	{
		get
		{
			return deviceId;
		}
		set
		{
			SetProperty(ref deviceId, value, "DeviceID");
		}
	}

	public string Text
	{
		get
		{
			return text;
		}
		set
		{
			SetProperty(ref text, value, "Text");
		}
	}

	public string Filesystem
	{
		get
		{
			return filesystem;
		}
		set
		{
			SetProperty(ref filesystem, value, "Filesystem");
		}
	}

	public string BusProtocol
	{
		get
		{
			return busProtocol;
		}
		set
		{
			SetProperty(ref busProtocol, value, "BusProtocol");
		}
	}

	public DriveType Type
	{
		get
		{
			return type;
		}
		set
		{
			SetProperty(ref type, value, "Type");
		}
	}

	public long MaxSpace
	{
		get
		{
			return maxSpace;
		}
		set
		{
			SetProperty(ref maxSpace, value, "MaxSpace");
		}
	}

	public long FreeSpace
	{
		get
		{
			return freeSpace;
		}
		set
		{
			SetProperty(ref freeSpace, value, "FreeSpace");
		}
	}

	public bool IsUsb
	{
		get
		{
			return isUsb;
		}
		set
		{
			SetProperty(ref isUsb, value, "IsUsb");
		}
	}

	public bool IsExternal
	{
		get
		{
			return isExternal;
		}
		set
		{
			SetProperty(ref isExternal, value, "IsExternal");
		}
	}

	public BaseStorageFolder? Root
	{
		get
		{
			return root;
		}
		set
		{
			SetProperty(ref root, value, "Root");
		}
	}

	public bool IsRemovable
	{
		get
		{
			DriveType driveType = Type;
			if (driveType == DriveType.Removable || driveType == DriveType.CDRom)
			{
				return true;
			}
			return false;
		}
	}

	public long UsedSpace => Math.Max(0L, MaxSpace - FreeSpace);

	public string Id => Path;

	public string Name => Text;

	public static Task<DriveItem> CreateFromPropertiesAsync(string path, string deviceId, string label, DriveType type, string filesystem = "", string busProtocol = "", bool isUsb = false, bool isExternal = false, long totalSize = 0L, long freeSpace = 0L)
	{
		string text = PathHelpers.NormalizeDirectoryPath(path);
		string value = (string.IsNullOrWhiteSpace(label) ? System.IO.Path.GetFileName(text) : label);
		if (string.IsNullOrWhiteSpace(value))
		{
			value = text;
		}
		return Task.FromResult(new DriveItem
		{
			Path = text,
			DeviceID = (string.IsNullOrWhiteSpace(deviceId) ? text : deviceId),
			Text = value,
			Type = type,
			Filesystem = filesystem,
			BusProtocol = busProtocol,
			IsUsb = isUsb,
			IsExternal = isExternal,
			MaxSpace = totalSize,
			FreeSpace = freeSpace,
			Root = (string.IsNullOrWhiteSpace(text) ? null : new MacSystemStorageFolder(text))
		});
	}
}
