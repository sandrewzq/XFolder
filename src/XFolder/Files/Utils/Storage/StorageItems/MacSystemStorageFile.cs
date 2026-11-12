using System;
using System.IO;
using System.Threading.Tasks;
using Files.Data.Enums.Storage;
using Files.Services.NativePlugin;
using Files.Utils.Storage.BaseStorageItems;

namespace Files.Utils.Storage.StorageItems;

public sealed class MacSystemStorageFile : BaseStorageFile
{
	private readonly FileInfo _fileInfo;

	public override string Name => _fileInfo.Name;

	public override string Path => _fileInfo.FullName;

	public override string ContentType => "";

	public override string FileType => _fileInfo.Extension;

	public override string DisplayType => "File";

	public override long Size
	{
		get
		{
			if (!_fileInfo.Exists)
			{
				return 0L;
			}
			return _fileInfo.Length;
		}
	}

	public override bool IsAvailable => _fileInfo.Exists;

	public override DateTimeOffset DateCreated => _fileInfo.CreationTimeUtc;

	public MacSystemStorageFile(string path)
	{
		_fileInfo = new FileInfo(path);
	}

	public override Task<BaseStorageFolder> GetParentAsync()
	{
		return Task.FromResult((BaseStorageFolder)new MacSystemStorageFolder(_fileInfo.Directory.FullName));
	}

	public override Task<Stream> OpenAsync(FileMode fileMode, FileAccess fileAccess)
	{
		return Task.FromResult((Stream)File.Open(_fileInfo.FullName, fileMode, fileAccess));
	}

	public override Task<Stream> OpenReadAsync()
	{
		return Task.FromResult((Stream)_fileInfo.OpenRead());
	}

	public override async Task<BaseStorageFile> CopyAsync(BaseStorageFolder destinationFolder)
	{
		string text = System.IO.Path.Combine(destinationFolder.Path, Name);
		File.Copy(_fileInfo.FullName, text, overwrite: false);
		return new MacSystemStorageFile(text);
	}

	public override async Task<BaseStorageFile> CopyAsync(BaseStorageFolder destinationFolder, string desiredNewName)
	{
		string text = System.IO.Path.Combine(destinationFolder.Path, desiredNewName);
		File.Copy(_fileInfo.FullName, text, overwrite: false);
		return new MacSystemStorageFile(text);
	}

	public override async Task<BaseStorageFile> CopyAsync(BaseStorageFolder destinationFolder, string desiredNewName, NameCollisionOption option)
	{
		string text = System.IO.Path.Combine(destinationFolder.Path, desiredNewName);
		if (File.Exists(text) && option == NameCollisionOption.FailIfExists)
		{
			throw new IOException("File exists");
		}
		File.Copy(_fileInfo.FullName, text, option == NameCollisionOption.ReplaceExisting);
		return new MacSystemStorageFile(text);
	}

	public override async Task MoveAsync(BaseStorageFolder destinationFolder)
	{
		string destFileName = System.IO.Path.Combine(destinationFolder.Path, Name);
		File.Move(_fileInfo.FullName, destFileName);
	}

	public override async Task MoveAsync(BaseStorageFolder destinationFolder, string desiredNewName)
	{
		string destFileName = System.IO.Path.Combine(destinationFolder.Path, desiredNewName);
		File.Move(_fileInfo.FullName, destFileName);
	}

	public override async Task MoveAsync(BaseStorageFolder destinationFolder, string desiredNewName, NameCollisionOption option)
	{
		string text = System.IO.Path.Combine(destinationFolder.Path, desiredNewName);
		bool flag = File.Exists(text);
		if (flag && option == NameCollisionOption.FailIfExists)
		{
			throw new IOException("File exists");
		}
		if (flag && option == NameCollisionOption.ReplaceExisting)
		{
			File.Delete(text);
		}
		File.Move(_fileInfo.FullName, text);
	}

	public override async Task RenameAsync(string desiredName)
	{
		string destFileName = System.IO.Path.Combine(_fileInfo.DirectoryName, desiredName);
		File.Move(_fileInfo.FullName, destFileName);
	}

	public override async Task RenameAsync(string desiredName, NameCollisionOption option)
	{
		string text = System.IO.Path.Combine(_fileInfo.DirectoryName, desiredName);
		bool flag = File.Exists(text);
		if (flag && option == NameCollisionOption.FailIfExists)
		{
			throw new IOException("File exists");
		}
		if (flag && option == NameCollisionOption.ReplaceExisting)
		{
			File.Delete(text);
		}
		File.Move(_fileInfo.FullName, text);
	}

	public override async Task DeleteAsync()
	{
		MacPluginInterop.MoveFileOrDirectoryToTrash(_fileInfo.FullName);
	}

	public override async Task DeleteAsync(StorageDeleteOption option)
	{
		if (StorageDeleteOption.PermanentDelete == option)
		{
			File.Delete(_fileInfo.FullName);
		}
		else
		{
			await DeleteAsync();
		}
	}

	public override async Task<string> ReadTextAsync(int maxLength = -1)
	{
		using StreamReader reader = new StreamReader(_fileInfo.FullName);
		if (maxLength < 0)
		{
			return await reader.ReadToEndAsync();
		}
		char[] buffer = new char[maxLength];
		return new string(buffer, 0, await reader.ReadAsync(buffer, 0, maxLength));
	}

	public override async Task WriteTextAsync(string text)
	{
		await File.WriteAllTextAsync(_fileInfo.FullName, text);
	}

	public override async Task WriteBytesAsync(byte[] dataBytes)
	{
		await File.WriteAllBytesAsync(_fileInfo.FullName, dataBytes);
	}
}
