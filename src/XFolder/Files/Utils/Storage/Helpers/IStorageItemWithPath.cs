using Files.Data.Enums;
using Files.Utils.Storage.BaseStorageItems;

namespace Files.Utils.Storage.Helpers;

public interface IStorageItemWithPath
{
	string Name { get; }

	string Path { get; }

	IBaseStorageItem Item { get; }

	FilesystemItemType ItemType { get; }
}
