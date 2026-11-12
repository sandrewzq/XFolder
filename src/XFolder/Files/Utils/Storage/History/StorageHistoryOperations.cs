using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Files.Data.Contracts;
using Files.Data.Enums;
using Files.Data.Enums.Storage;
using Files.Helpers.Shared;
using Files.Services.NativePlugin;
using Files.Utils.Storage.Helpers;
using Files.Utils.Storage.Operations;

namespace Files.Utils.Storage.History;

public sealed class StorageHistoryOperations : IStorageHistoryOperations, IDisposable
{
	private FilesystemHelpers helpers;

	private readonly CancellationToken cancellationToken;

	private readonly IShellView associatedInstance;

	public StorageHistoryOperations(IShellView associatedInstance, CancellationToken cancellationToken)
	{
		this.cancellationToken = cancellationToken;
		this.associatedInstance = associatedInstance;
		helpers = associatedInstance.FilesystemHelpers;
	}

	public async Task<ReturnResult> Undo(IStorageHistory history)
	{
		ReturnResult returnResult = ReturnResult.Success;
		try
		{
			switch (history.OperationType)
			{
			case FileOperationType.CreateNew:
				if (!IsHistoryNull(history.Source))
				{
					return await helpers.DeleteItemsAsync(history.Source, DeleteConfirmationPolicies.Never, permanently: true, registerHistory: false);
				}
				break;
			case FileOperationType.Rename:
			{
				if (IsHistoryNull(history))
				{
					break;
				}
				NameCollisionOption collision = NameCollisionOption.GenerateUniqueName;
				for (int i = 0; i < history.Destination.Count; i++)
				{
					string fileName = Path.GetFileName(history.Source[i].Path);
					returnResult = await helpers.RenameAsync(history.Destination[i], fileName, collision, registerHistory: false, showExtensionDialog: false);
					if (returnResult != ReturnResult.Success)
					{
						break;
					}
				}
				break;
			}
			case FileOperationType.Copy:
				if (!IsHistoryNull(history.Destination))
				{
					return await helpers.DeleteItemsAsync(history.Destination, DeleteConfirmationPolicies.Never, permanently: true, registerHistory: false);
				}
				break;
			case FileOperationType.ImageConvert:
				if (!IsHistoryNull(history.Destination))
				{
					returnResult = await helpers.DeleteItemsAsync(history.Destination, DeleteConfirmationPolicies.Never, permanently: true, registerHistory: false);
					RefreshParentDirectory(history.Destination[0].Path);
					return returnResult;
				}
				break;
			case FileOperationType.ImageRotate:
				if (!IsHistoryNull(history.Source) && !IsHistoryNull(history.Destination))
				{
					bool flag = history.Destination[0].Path == "Clockwise";
					returnResult = await UndoImageRotate(history, !flag);
					RefreshParentDirectory(history.Source[0].Path);
					return returnResult;
				}
				break;
			case FileOperationType.Move:
				if (!IsHistoryNull(history))
				{
					return await helpers.MoveItemsAsync(history.Destination, history.Source.Select((IStorageItemWithPath item) => item.Path), showDialog: false, registerHistory: false);
				}
				break;
			case FileOperationType.Recycle:
				if (!IsHistoryNull(history))
				{
					return await helpers.MoveItemsAsync(history.Destination, history.Source.Select((IStorageItemWithPath item) => item.Path), showDialog: false, registerHistory: false);
				}
				break;
			case FileOperationType.Delete:
				returnResult = ReturnResult.Success;
				break;
			default:
				returnResult = ReturnResult.Success;
				break;
			}
		}
		catch (Exception)
		{
			returnResult = ReturnResult.Failed;
		}
		return returnResult;
	}

	public async Task<ReturnResult> Redo(IStorageHistory history)
	{
		ReturnResult returnResult = ReturnResult.Success;
		try
		{
			switch (history.OperationType)
			{
			case FileOperationType.CreateNew:
				if (IsHistoryNull(history.Source))
				{
					break;
				}
				foreach (IStorageItemWithPath item in history.Source)
				{
					returnResult = (await helpers.CreateAsync(item, registerHistory: false)).Item1;
					if (returnResult != ReturnResult.Success)
					{
						break;
					}
				}
				break;
			case FileOperationType.Rename:
			{
				if (IsHistoryNull(history))
				{
					break;
				}
				NameCollisionOption collision = NameCollisionOption.GenerateUniqueName;
				for (int i = 0; i < history.Source.Count; i++)
				{
					string fileName = Path.GetFileName(history.Destination[i].Path);
					returnResult = await helpers.RenameAsync(history.Source[i], fileName, collision, registerHistory: false, showExtensionDialog: false);
					if (returnResult != ReturnResult.Success)
					{
						break;
					}
				}
				break;
			}
			case FileOperationType.Copy:
				if (!IsHistoryNull(history))
				{
					return await helpers.CopyItemsAsync(history.Source, history.Destination.Select((IStorageItemWithPath item) => item.Path), showDialog: false, registerHistory: false);
				}
				break;
			case FileOperationType.ImageConvert:
				if (!IsHistoryNull(history))
				{
					returnResult = RedoImageConvert(history);
					RefreshParentDirectory(history.Destination[0].Path);
					return returnResult;
				}
				break;
			case FileOperationType.ImageRotate:
				if (!IsHistoryNull(history.Source) && !IsHistoryNull(history.Destination))
				{
					bool rotateClockwise = history.Destination[0].Path == "Clockwise";
					returnResult = await UndoImageRotate(history, rotateClockwise);
					RefreshParentDirectory(history.Source[0].Path);
					return returnResult;
				}
				break;
			case FileOperationType.Move:
				if (!IsHistoryNull(history))
				{
					return await helpers.MoveItemsAsync(history.Source, history.Destination.Select((IStorageItemWithPath item) => item.Path), showDialog: false, registerHistory: false);
				}
				break;
			case FileOperationType.Recycle:
				if (!IsHistoryNull(history.Source))
				{
					return await helpers.DeleteItemsAsync(history.Source, DeleteConfirmationPolicies.Never, permanently: false, registerHistory: false);
				}
				break;
			case FileOperationType.Delete:
				returnResult = ReturnResult.Success;
				break;
			default:
				returnResult = ReturnResult.Success;
				break;
			}
		}
		catch (Exception)
		{
			returnResult = ReturnResult.Failed;
		}
		return returnResult;
	}

	public void Dispose()
	{
		helpers?.Dispose();
		helpers = null;
	}

	private bool IsHistoryNull(IStorageHistory history)
	{
		if (!IsHistoryNull(history.Source))
		{
			if (history.Destination != null)
			{
				return IsHistoryNull(history.Destination);
			}
			return false;
		}
		return true;
	}

	private bool IsHistoryNull(IEnumerable<IStorageItemWithPath> source)
	{
		return !source.All(HasPath);
	}

	private static bool HasPath(IStorageItemWithPath item)
	{
		if (item != null)
		{
			return !string.IsNullOrWhiteSpace(item.Path);
		}
		return false;
	}

	private void RefreshParentDirectory(string path)
	{
		string directoryName = Path.GetDirectoryName(path);
		if (!string.IsNullOrWhiteSpace(directoryName))
		{
			associatedInstance.InstanceViewModel?.SelectedTab?.LayoutViewModel?.RefreshItems(directoryName);
		}
	}

	private static ReturnResult RedoImageConvert(IStorageHistory history)
	{
		string path = history.Source[0].Path;
		string path2 = history.Destination[0].Path;
		if (File.Exists(path2))
		{
			return ReturnResult.AlreadyExists;
		}
		int num;
		switch (Path.GetExtension(path2).ToLowerInvariant())
		{
		case ".png":
			num = 1;
			break;
		case ".jpg":
		case ".jpeg":
			num = 2;
			break;
		case ".bmp":
			num = 3;
			break;
		default:
			num = 0;
			break;
		}
		int num2 = num;
		if (num2 == 0)
		{
			return ReturnResult.BadArgumentException;
		}
		if (!MacPluginInterop.ConvertImageToFormat(path, path2, num2))
		{
			return ReturnResult.Failed;
		}
		return ReturnResult.Success;
	}

	private static async Task<ReturnResult> UndoImageRotate(IStorageHistory history, bool rotateClockwise)
	{
		foreach (IStorageItemWithPath item in history.Source)
		{
			FileThumbnailHelper.InvalidateImageThumbnail(item.Path);
		}
		if ((await Task.WhenAll(history.Source.Select((IStorageItemWithPath item) => Task.Run(() => ImageRotationHelper.Rotate90(item.Path, rotateClockwise))))).Any((bool x) => !x))
		{
			return ReturnResult.Failed;
		}
		return ReturnResult.Success;
	}
}
