using System.IO;
using Files.Data.Enums;
using Files.Utils.Storage.BaseStorageItems;

namespace Files.Utils.Storage.Helpers;

public sealed class StorageFileWithPath : IStorageItemWithPath
{
	public string Path { get; }

	public string Name => Item?.Name ?? System.IO.Path.GetFileName(Path);

	IBaseStorageItem IStorageItemWithPath.Item => Item;

	public BaseStorageFile Item { get; }

	public FilesystemItemType ItemType => FilesystemItemType.File;

	public StorageFileWithPath(BaseStorageFile file)
		: this(file, file.Path)
	{
	}

	public StorageFileWithPath(BaseStorageFile file, string path)
	{
		Item = file;
		Path = path;
	}
}
