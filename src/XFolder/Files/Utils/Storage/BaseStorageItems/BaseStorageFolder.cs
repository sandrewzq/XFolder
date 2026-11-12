using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Files.Data.Enums.Storage;

namespace Files.Utils.Storage.BaseStorageItems;

public abstract class BaseStorageFolder : IBaseStorageItem
{
	public abstract string Name { get; }

	public abstract string Path { get; }

	public abstract string DisplayName { get; }

	public abstract bool IsAvailable { get; }

	public abstract DateTimeOffset DateCreated { get; }

	public abstract Task<BaseStorageFolder> GetParentAsync();

	public abstract Task<IBaseStorageItem> GetItemAsync(string name);

	public abstract Task<IReadOnlyList<IBaseStorageItem>> GetItemsAsync();

	public abstract Task<BaseStorageFile> GetFileAsync(string name);

	public abstract Task<IReadOnlyList<BaseStorageFile>> GetFilesAsync();

	public abstract Task<BaseStorageFolder> GetFolderAsync(string name);

	public abstract Task<IReadOnlyList<BaseStorageFolder>> GetFoldersAsync();

	public abstract Task<BaseStorageFile> CreateFileAsync(string desiredName);

	public abstract Task<BaseStorageFile> CreateFileAsync(string desiredName, CreationCollisionOption options);

	public abstract Task<BaseStorageFolder> CreateFolderAsync(string desiredName);

	public abstract Task<BaseStorageFolder> CreateFolderAsync(string desiredName, CreationCollisionOption options);

	public abstract Task<BaseStorageFolder> MoveAsync(BaseStorageFolder destinationFolder);

	public abstract Task<BaseStorageFolder> MoveAsync(BaseStorageFolder destinationFolder, NameCollisionOption option);

	public abstract Task DeleteAsync();

	public abstract Task DeleteAsync(StorageDeleteOption option);

	public abstract Task RenameAsync(string desiredName);

	public abstract Task RenameAsync(string desiredName, NameCollisionOption option);

	public abstract bool IsEqual(IBaseStorageItem item);

	public abstract bool IsOfType(StorageItemTypes type);
}
