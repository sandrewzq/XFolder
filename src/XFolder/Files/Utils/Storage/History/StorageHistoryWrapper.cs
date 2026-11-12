using System;
using System.Collections.Generic;
using System.IO;
using CommunityToolkit.Mvvm.ComponentModel;
using Files.Data.Enums;
using Files.Utils.Storage.Helpers;

namespace Files.Utils.Storage.History;

public sealed class StorageHistoryWrapper : ObservableObject, IDisposable
{
	private int index = -1;

	private List<IStorageHistory> histories = new List<IStorageHistory>();

	public bool CanRedo()
	{
		int num = index + 1;
		if (num >= 0 && num < histories.Count)
		{
			return CanRedoHistory(histories[num]);
		}
		return false;
	}

	public bool CanUndo()
	{
		if (index >= 0 && index < histories.Count)
		{
			return CanUndoHistory(histories[index]);
		}
		return false;
	}

	public IStorageHistory GetCurrentHistory()
	{
		return histories[index];
	}

	public void AddHistory(IStorageHistory? history)
	{
		if (history != null)
		{
			index++;
			histories.Insert(index, history);
			histories.RemoveRange(index + 1, histories.Count - index - 1);
			NotifyStateChanged();
		}
	}

	public void RemoveHistory(IStorageHistory history, bool decreaseIndex = true)
	{
		if (history != null)
		{
			histories.RemoveRange(index + 1, histories.Count - index - 1);
			if (decreaseIndex)
			{
				index--;
			}
			histories.Remove(history);
			NotifyStateChanged();
		}
	}

	public void ModifyCurrentHistory(IStorageHistory newHistory)
	{
		histories[index].Modify(newHistory);
	}

	public void DecreaseIndex()
	{
		index--;
		NotifyStateChanged();
	}

	public void IncreaseIndex()
	{
		index++;
		NotifyStateChanged();
	}

	public void Dispose()
	{
		histories.Clear();
		index = -1;
		NotifyStateChanged();
	}

	private void NotifyStateChanged()
	{
		OnPropertyChanged("CanUndo");
		OnPropertyChanged("CanRedo");
	}

	private static bool CanUndoHistory(IStorageHistory history)
	{
		return history.OperationType switch
		{
			FileOperationType.CreateNew => AllItemsExist(history.Source), 
			FileOperationType.Rename => AllItemsExist(history.Destination), 
			FileOperationType.Copy => AllItemsExist(history.Destination), 
			FileOperationType.ImageConvert => AllItemsExist(history.Destination), 
			FileOperationType.ImageRotate => AllItemsExist(history.Source), 
			FileOperationType.Move => AllItemsExist(history.Destination), 
			FileOperationType.Recycle => AllItemsExist(history.Destination), 
			FileOperationType.Delete => false, 
			_ => false, 
		};
	}

	private static bool CanRedoHistory(IStorageHistory history)
	{
		return history.OperationType switch
		{
			FileOperationType.CreateNew => AllItemsMissing(history.Source), 
			FileOperationType.Rename => AllItemsExist(history.Source), 
			FileOperationType.Copy => AllItemsExist(history.Source), 
			FileOperationType.ImageConvert => AllItemsExist(history.Source) && AllItemsMissing(history.Destination), 
			FileOperationType.ImageRotate => AllItemsExist(history.Source), 
			FileOperationType.Move => AllItemsExist(history.Source), 
			FileOperationType.Recycle => AllItemsExist(history.Source), 
			FileOperationType.Delete => false, 
			_ => false, 
		};
	}

	private static bool AllItemsExist(IEnumerable<IStorageItemWithPath>? items)
	{
		if (items == null)
		{
			return false;
		}
		bool result = false;
		foreach (IStorageItemWithPath item in items)
		{
			result = true;
			if (!ItemExists(item))
			{
				return false;
			}
		}
		return result;
	}

	private static bool AllItemsMissing(IEnumerable<IStorageItemWithPath>? items)
	{
		if (items == null)
		{
			return false;
		}
		bool result = false;
		foreach (IStorageItemWithPath item in items)
		{
			result = true;
			if (!ItemMissing(item))
			{
				return false;
			}
		}
		return result;
	}

	private static bool ItemExists(IStorageItemWithPath? item)
	{
		if (!HasPath(item))
		{
			return false;
		}
		return item.ItemType switch
		{
			FilesystemItemType.Directory => Directory.Exists(item.Path), 
			FilesystemItemType.File => File.Exists(item.Path), 
			_ => File.Exists(item.Path) || Directory.Exists(item.Path), 
		};
	}

	private static bool ItemMissing(IStorageItemWithPath? item)
	{
		if (HasPath(item))
		{
			return !ItemExists(item);
		}
		return false;
	}

	private static bool HasPath(IStorageItemWithPath? item)
	{
		if (item != null)
		{
			return !string.IsNullOrWhiteSpace(item.Path);
		}
		return false;
	}
}
