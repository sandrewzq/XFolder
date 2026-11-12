using System.IO;
using Files.Data.Enums;
using Files.Utils.Storage.BaseStorageItems;

namespace Files.Utils.Storage.Helpers;

public sealed class StorageFolderWithPath : IStorageItemWithPath
{
	public string Path { get; }

	public string Name => Item?.Name ?? System.IO.Path.GetFileName(Path.TrimEnd(new char[2]
	{
		System.IO.Path.DirectorySeparatorChar,
		System.IO.Path.AltDirectorySeparatorChar
	}));

	IBaseStorageItem IStorageItemWithPath.Item => Item;

	public BaseStorageFolder Item { get; }

	public FilesystemItemType ItemType => FilesystemItemType.Directory;

	public StorageFolderWithPath(BaseStorageFolder folder)
		: this(folder, folder.Path)
	{
	}

	public StorageFolderWithPath(BaseStorageFolder folder, string path)
	{
		Item = folder;
		Path = path;
	}
}
