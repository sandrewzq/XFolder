using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Files.Data.Enums.Storage;
using Files.Services.NativePlugin;
using Files.Utils.Storage.BaseStorageItems;

namespace Files.Utils.Storage.StorageItems;

public sealed class MacSystemStorageFolder : BaseStorageFolder
{
	private readonly DirectoryInfo _dirInfo;

	public override string Name => _dirInfo.Name;

	public override string Path => _dirInfo.FullName;

	public override string DisplayName => _dirInfo.Name;

	public override bool IsAvailable => _dirInfo.Exists;

	public override DateTimeOffset DateCreated => _dirInfo.CreationTimeUtc;

	public MacSystemStorageFolder(string path)
	{
		_dirInfo = new DirectoryInfo(path);
	}

	public override async Task<BaseStorageFolder> GetParentAsync()
	{
		DirectoryInfo parent = _dirInfo.Parent;
		return (parent != null) ? new MacSystemStorageFolder(parent.FullName) : null;
	}

	public override async Task<IBaseStorageItem> GetItemAsync(string name)
	{
		string path = System.IO.Path.Combine(_dirInfo.FullName, name);
		if (File.Exists(path))
		{
			return new MacSystemStorageFile(path);
		}
		string path2 = System.IO.Path.Combine(_dirInfo.FullName, name);
		if (Directory.Exists(path2))
		{
			return new MacSystemStorageFolder(path2);
		}
		return null;
	}

	public override async Task<IReadOnlyList<IBaseStorageItem>> GetItemsAsync()
	{
		IEnumerable<IBaseStorageItem> first = ((IEnumerable<FileInfo>)_dirInfo.GetFiles()).Select((Func<FileInfo, IBaseStorageItem>)((FileInfo f) => new MacSystemStorageFile(f.FullName)));
		IEnumerable<IBaseStorageItem> second = ((IEnumerable<DirectoryInfo>)_dirInfo.GetDirectories()).Select((Func<DirectoryInfo, IBaseStorageItem>)((DirectoryInfo d) => new MacSystemStorageFolder(d.FullName)));
		return first.Concat(second).ToList();
	}

	public override async Task<BaseStorageFile> GetFileAsync(string name)
	{
		return new MacSystemStorageFile(System.IO.Path.Combine(_dirInfo.FullName, name));
	}

	public override async Task<IReadOnlyList<BaseStorageFile>> GetFilesAsync()
	{
		return ((IEnumerable<FileInfo>)_dirInfo.GetFiles()).Select((Func<FileInfo, BaseStorageFile>)((FileInfo f) => new MacSystemStorageFile(f.FullName))).ToList();
	}

	public override async Task<BaseStorageFolder> GetFolderAsync(string name)
	{
		return new MacSystemStorageFolder(System.IO.Path.Combine(_dirInfo.FullName, name));
	}

	public override async Task<IReadOnlyList<BaseStorageFolder>> GetFoldersAsync()
	{
		return ((IEnumerable<DirectoryInfo>)_dirInfo.GetDirectories()).Select((Func<DirectoryInfo, BaseStorageFolder>)((DirectoryInfo d) => new MacSystemStorageFolder(d.FullName))).ToList();
	}

	public override async Task<BaseStorageFile> CreateFileAsync(string desiredName)
	{
		string path = System.IO.Path.Combine(_dirInfo.FullName, desiredName);
		using (File.Create(path))
		{
		}
		return new MacSystemStorageFile(path);
	}

	public override async Task<BaseStorageFile> CreateFileAsync(string desiredName, CreationCollisionOption options)
	{
		string path = System.IO.Path.Combine(_dirInfo.FullName, desiredName);
		if (File.Exists(path))
		{
			switch (options)
			{
			case CreationCollisionOption.FailIfExists:
				throw new IOException("File exists");
			case CreationCollisionOption.ReplaceExisting:
				File.Delete(path);
				break;
			case CreationCollisionOption.OpenIfExists:
				return new MacSystemStorageFile(path);
			case CreationCollisionOption.GenerateUniqueName:
				path = System.IO.Path.Combine(_dirInfo.FullName, Guid.NewGuid().ToString() + "_" + desiredName);
				break;
			}
		}
		using (File.Create(path))
		{
		}
		return new MacSystemStorageFile(path);
	}

	public override async Task<BaseStorageFolder> CreateFolderAsync(string desiredName)
	{
		string path = System.IO.Path.Combine(_dirInfo.FullName, desiredName);
		Directory.CreateDirectory(path);
		return new MacSystemStorageFolder(path);
	}

	public override async Task<BaseStorageFolder> CreateFolderAsync(string desiredName, CreationCollisionOption options)
	{
		string path = System.IO.Path.Combine(_dirInfo.FullName, desiredName);
		if (Directory.Exists(path))
		{
			switch (options)
			{
			case CreationCollisionOption.FailIfExists:
				throw new IOException("Directory exists");
			case CreationCollisionOption.ReplaceExisting:
				Directory.Delete(path, recursive: true);
				break;
			case CreationCollisionOption.OpenIfExists:
				return new MacSystemStorageFolder(path);
			case CreationCollisionOption.GenerateUniqueName:
				path = System.IO.Path.Combine(_dirInfo.FullName, Guid.NewGuid().ToString() + "_" + desiredName);
				break;
			}
		}
		Directory.CreateDirectory(path);
		return new MacSystemStorageFolder(path);
	}

	public override async Task<BaseStorageFolder> MoveAsync(BaseStorageFolder destinationFolder)
	{
		string text = System.IO.Path.Combine(destinationFolder.Path, Name);
		Directory.Move(_dirInfo.FullName, text);
		return new MacSystemStorageFolder(text);
	}

	public override async Task<BaseStorageFolder> MoveAsync(BaseStorageFolder destinationFolder, NameCollisionOption option)
	{
		string text = System.IO.Path.Combine(destinationFolder.Path, Name);
		if (Directory.Exists(text))
		{
			switch (option)
			{
			case NameCollisionOption.FailIfExists:
				throw new IOException("Directory exists");
			case NameCollisionOption.ReplaceExisting:
				Directory.Delete(text, recursive: true);
				break;
			}
		}
		Directory.Move(_dirInfo.FullName, text);
		return new MacSystemStorageFolder(text);
	}

	public override async Task DeleteAsync()
	{
		MacPluginInterop.MoveFileOrDirectoryToTrash(_dirInfo.FullName);
	}

	public override async Task DeleteAsync(StorageDeleteOption option)
	{
		if (option == StorageDeleteOption.PermanentDelete)
		{
			Directory.Delete(_dirInfo.FullName, recursive: true);
		}
		else
		{
			await DeleteAsync();
		}
	}

	public override async Task RenameAsync(string desiredName)
	{
		string destDirName = System.IO.Path.Combine(_dirInfo.Parent.FullName, desiredName);
		Directory.Move(_dirInfo.FullName, destDirName);
	}

	public override async Task RenameAsync(string desiredName, NameCollisionOption option)
	{
		string text = System.IO.Path.Combine(_dirInfo.Parent.FullName, desiredName);
		if (Directory.Exists(text))
		{
			switch (option)
			{
			case NameCollisionOption.FailIfExists:
				throw new IOException("Directory exists");
			case NameCollisionOption.ReplaceExisting:
				Directory.Delete(text, recursive: true);
				break;
			}
		}
		Directory.Move(_dirInfo.FullName, text);
	}

	public override bool IsEqual(IBaseStorageItem item)
	{
		return string.Equals(Path, item.Path, StringComparison.OrdinalIgnoreCase);
	}

	public override bool IsOfType(StorageItemTypes type)
	{
		return type == StorageItemTypes.Folder;
	}
}
