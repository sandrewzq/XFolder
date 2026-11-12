using System;
using System.IO;
using System.Threading.Tasks;
using Files.Data.Enums.Storage;

namespace Files.Utils.Storage.BaseStorageItems;

public abstract class BaseStorageFile : IBaseStorageItem
{
	public abstract string Name { get; }

	public abstract string Path { get; }

	public abstract string ContentType { get; }

	public abstract string FileType { get; }

	public abstract string DisplayType { get; }

	public abstract long Size { get; }

	public abstract bool IsAvailable { get; }

	public abstract DateTimeOffset DateCreated { get; }

	public abstract Task<BaseStorageFolder> GetParentAsync();

	public abstract Task<Stream> OpenAsync(FileMode fileMode, FileAccess fileAccess);

	public abstract Task<Stream> OpenReadAsync();

	public abstract Task<BaseStorageFile> CopyAsync(BaseStorageFolder destinationFolder);

	public abstract Task<BaseStorageFile> CopyAsync(BaseStorageFolder destinationFolder, string desiredNewName);

	public abstract Task<BaseStorageFile> CopyAsync(BaseStorageFolder destinationFolder, string desiredNewName, NameCollisionOption option);

	public abstract Task MoveAsync(BaseStorageFolder destinationFolder);

	public abstract Task MoveAsync(BaseStorageFolder destinationFolder, string desiredNewName);

	public abstract Task MoveAsync(BaseStorageFolder destinationFolder, string desiredNewName, NameCollisionOption option);

	public abstract Task RenameAsync(string desiredName);

	public abstract Task RenameAsync(string desiredName, NameCollisionOption option);

	public abstract Task DeleteAsync();

	public abstract Task DeleteAsync(StorageDeleteOption option);

	public abstract Task<string> ReadTextAsync(int maxLength = -1);

	public abstract Task WriteTextAsync(string text);

	public abstract Task WriteBytesAsync(byte[] dataBytes);
}
