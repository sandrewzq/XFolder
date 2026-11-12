using System.IO;
using Files.Data.Enums;

namespace Files.Utils.Storage.Helpers;

public static class StorageHelpers
{
	public static IStorageItemWithPath? FromPathAndType(string customPath)
	{
		if (File.Exists(customPath))
		{
			return new StorageFileWithPath(null, customPath);
		}
		if (Directory.Exists(customPath))
		{
			return new StorageFolderWithPath(null, customPath);
		}
		return null;
	}

	public static IStorageItemWithPath FromPathAndType(string customPath, FilesystemItemType? itemType)
	{
		if (itemType == FilesystemItemType.File)
		{
			return new StorageFileWithPath(null, customPath);
		}
		return new StorageFolderWithPath(null, customPath);
	}

	public static FilesystemResult<T> ToType<T, V>(FilesystemResult<V> result) where T : class
	{
		return new FilesystemResult<T>(result.Result as T, result.ErrorCode);
	}
}
