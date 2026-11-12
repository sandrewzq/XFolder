using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.DependencyInjection;
using Files.Assets.Lang;
using Files.Data.Contracts;
using Files.Data.Enums;
using Files.Data.Enums.Storage;
using Files.Data.Items;
using Files.Extensions;
using Files.Helpers;
using Files.Helpers.Shared;
using Files.Utils.Storage.Helpers;
using Files.Utils.Storage.History;

namespace Files.Utils.Storage.Operations;

public sealed class FilesystemHelpers : IFilesystemHelpers, IDisposable
{
	private IShellView associatedInstance;

	private FilesystemOperations filesystemOperations;

	private readonly IFoldersSettingsService foldersSettingsService;

	public FilesystemHelpers(IShellView associatedInstance)
	{
		this.associatedInstance = associatedInstance;
		filesystemOperations = new FilesystemOperations(this.associatedInstance);
		foldersSettingsService = Ioc.Default.GetRequiredService<IFoldersSettingsService>();
	}

	public async Task<(ReturnResult, IStorageItem?)> CreateAsync(IStorageItemWithPath source, bool registerHistory)
	{
		if (string.IsNullOrWhiteSpace(source.Path))
		{
			return (ReturnResult.BadArgumentException, null);
		}
		if (Directory.Exists(source.Path) || File.Exists(source.Path))
		{
			return (ReturnResult.AlreadyExists, null);
		}
		(IStorageHistory, IStorageItem) result = await filesystemOperations.CreateAsync(source);
		if (registerHistory && result.Item1 != null)
		{
			App.HistoryWrapper.AddHistory(result.Item1);
		}
		await Task.Yield();
		return ((result.Item1 != null) ? ReturnResult.Success : ReturnResult.Failed, result.Item2);
	}

	public async Task<ReturnResult> DeleteItemsAsync(IEnumerable<IStorageItemWithPath> source, DeleteConfirmationPolicies showDialog, bool permanently, bool registerHistory)
	{
		List<IStorageItemWithPath> itemsList = source.ToList();
		if (!itemsList.Any())
		{
			return ReturnResult.Success;
		}
		bool flag = false;
		switch (showDialog)
		{
		case DeleteConfirmationPolicies.Always:
			flag = true;
			break;
		case DeleteConfirmationPolicies.PermanentOnly:
			flag = permanently;
			break;
		case DeleteConfirmationPolicies.Never:
			flag = false;
			break;
		}
		if (flag)
		{
			string message = (permanently ? Resources.DeleteItemDescription.GetLocalizedFormatResource(itemsList.Count) : Resources.MoveItemsToTrashDescription.GetLocalizedFormatResource(itemsList.Count));
			string primaryText = (permanently ? Resources.DeletePermanently : Resources.Delete);
			string cancel = Resources.Cancel;
			if (!(await DialogDisplayHelper.ShowAskDialogAsync("", message, primaryText, cancel)))
			{
				return ReturnResult.Cancelled;
			}
		}
		IStorageHistory history = await filesystemOperations.DeleteItemsAsync(itemsList, permanently);
		await Task.Yield();
		if (!permanently & registerHistory)
		{
			App.HistoryWrapper.AddHistory(history);
		}
		return ReturnResult.Success;
	}

	public Task<ReturnResult> DeleteItemAsync(IStorageItemWithPath source, DeleteConfirmationPolicies showDialog, bool permanently, bool registerHistory)
	{
		return DeleteItemsAsync(source.CreateEnumerable(), showDialog, permanently, registerHistory);
	}

	public Task<ReturnResult> DeleteItemsAsync(IEnumerable<ListedItem> source, DeleteConfirmationPolicies showDialog, bool permanently, bool registerHistory)
	{
		return DeleteItemsAsync(source.Select((ListedItem item) => item.ToStorageItemWithPath()), showDialog, permanently, registerHistory);
	}

	public Task<ReturnResult> DeleteItemAsync(ListedItem source, DeleteConfirmationPolicies showDialog, bool permanently, bool registerHistory)
	{
		return DeleteItemAsync(source.ToStorageItemWithPath(), showDialog, permanently, registerHistory);
	}

	public Task<ReturnResult> CopyItemsAsync(IEnumerable<ListedItem> source, IEnumerable<string> destination, bool showDialog, bool registerHistory)
	{
		return CopyItemsAsync(source.Select((ListedItem item) => item.ToStorageItemWithPath()), destination, showDialog, registerHistory);
	}

	public Task<ReturnResult> CopyItemAsync(ListedItem source, string destination, bool showDialog, bool registerHistory)
	{
		return CopyItemAsync(source.ToStorageItemWithPath(), destination, showDialog, registerHistory);
	}

	public async Task<ReturnResult> CopyItemsAsync(IEnumerable<IStorageItemWithPath> source, IEnumerable<string> destination, bool showDialog, bool registerHistory)
	{
		(List<NameCollisionOption>, bool) tuple = await GetCollision(source, destination, showDialog);
		var (collisions, _) = tuple;
		if (tuple.Item2)
		{
			return ReturnResult.Cancelled;
		}
		IStorageHistory history = await filesystemOperations.CopyItemsAsync(source.ToList(), destination.ToList(), collisions);
		await Task.Yield();
		if (registerHistory && history != null)
		{
			App.HistoryWrapper.AddHistory(history);
		}
		return (history != null) ? ReturnResult.Success : ReturnResult.Failed;
	}

	public async Task<ReturnResult> CopyItemsAsync(IEnumerable<IStorageItemWithPath> source, IEnumerable<string> destination, IEnumerable<NameCollisionOption> collisions, bool registerHistory, IProgress<FileOperationProgress>? progress, CancellationToken cancellationToken)
	{
		try
		{
			IStorageHistory storageHistory = await filesystemOperations.CopyItemsAsync(source.ToList(), destination.ToList(), collisions.ToList(), progress, cancellationToken);
			if (registerHistory && storageHistory != null)
			{
				App.HistoryWrapper.AddHistory(storageHistory);
			}
			return (storageHistory != null) ? ReturnResult.Success : ReturnResult.Failed;
		}
		catch (OperationCanceledException)
		{
			return ReturnResult.Cancelled;
		}
	}

	public Task<ReturnResult> CopyItemAsync(IStorageItemWithPath source, string destination, bool showDialog, bool registerHistory)
	{
		return CopyItemsAsync(source.CreateEnumerable(), destination.CreateEnumerable(), showDialog, registerHistory);
	}

	public async Task<(List<NameCollisionOption> collisions, bool cancelOperation)> GetCollision(IEnumerable<IStorageItemWithPath> source, IEnumerable<string> destination, bool showDialog)
	{
		List<NameCollisionOption> collisions = new List<NameCollisionOption>();
		List<IStorageItemWithPath> srcList = source.ToList();
		List<string> dstList = destination.ToList();
		for (int i = 0; i < srcList.Count; i++)
		{
			if (srcList[i].Path == dstList[i])
			{
				collisions.Add(NameCollisionOption.GenerateUniqueName);
				continue;
			}
			string path = dstList[i];
			if (File.Exists(path) || Directory.Exists(path))
			{
				if (showDialog)
				{
					string fileName = Path.GetFileName(path);
					switch (await DialogDisplayHelper.ShowOptionsDialogAsync(Resources.ItemAlreadyExistsDialogTitle, string.Format(Resources.ItemAlreadyExistsDialogContent, fileName), Resources.GenerateNewName, Resources.ItemAlreadyExistsDialogSecondaryButtonText, Resources.Cancel))
					{
					case DynamicDialogResult.Primary:
						collisions.Add(NameCollisionOption.GenerateUniqueName);
						break;
					case DynamicDialogResult.Secondary:
						collisions.Add(NameCollisionOption.ReplaceExisting);
						break;
					default:
						return (collisions: collisions, cancelOperation: true);
					}
				}
				else
				{
					collisions.Add(NameCollisionOption.GenerateUniqueName);
				}
			}
			else
			{
				collisions.Add(NameCollisionOption.GenerateUniqueName);
			}
		}
		return (collisions: collisions, cancelOperation: false);
	}

	public async Task<ReturnResult> RenameAsync(IStorageItemWithPath source, string newName, NameCollisionOption collision, bool registerHistory, bool showExtensionDialog = true)
	{
		if (!IsValidForFilename(newName))
		{
			await DialogDisplayHelper.ShowTipAsync(Resources.ErrorDialogThisActionCannotBeDone, Resources.ErrorDialogNameNotAllowed);
			return ReturnResult.Failed;
		}
		if ((source.ItemType == FilesystemItemType.File || FileKindHelper.IsMacOSApp(source.Path)) && showExtensionDialog && foldersSettingsService.ShowFileExtensionWarning && Path.GetExtension(source.Path) != Path.GetExtension(newName) && !(await DialogDisplayHelper.ShowAskDialogAsync(Resources.Rename, Resources.RenameFileDialog_Text, Resources.Yes, Resources.No)))
		{
			return ReturnResult.Cancelled;
		}
		ReturnResult returnStatus;
		try
		{
			string directoryName = Path.GetDirectoryName(source.Path);
			string path = Path.Combine(directoryName, newName);
			if ((source.ItemType == FilesystemItemType.File) ? File.Exists(path) : Directory.Exists(path))
			{
				switch (collision)
				{
				case NameCollisionOption.FailIfExists:
					returnStatus = ReturnResult.AlreadyExists;
					await Task.Yield();
					return returnStatus;
				case NameCollisionOption.GenerateUniqueName:
				{
					(string name, string path) tuple = GenerateUniqueName(directoryName, newName, source.ItemType == FilesystemItemType.File);
					var (text, _) = tuple;
					_ = tuple.path;
					newName = text;
					break;
				}
				}
			}
			IStorageHistory storageHistory = await filesystemOperations.RenameAsync(source, newName);
			if (registerHistory && storageHistory != null)
			{
				App.HistoryWrapper.AddHistory(storageHistory);
			}
			returnStatus = ((storageHistory != null) ? ReturnResult.Success : ReturnResult.Failed);
		}
		catch (UnauthorizedAccessException)
		{
			returnStatus = ReturnResult.AccessUnauthorized;
		}
		catch (Exception)
		{
			returnStatus = ReturnResult.Failed;
		}
		await Task.Yield();
		return returnStatus;
	}

	public Task<ReturnResult> RenameAsync(ListedItem source, string newName, NameCollisionOption collision, bool registerHistory, bool showExtensionDialog = true)
	{
		return RenameAsync(source.ToStorageItemWithPath(), newName, collision, registerHistory, showExtensionDialog);
	}

	public Task<ReturnResult> MoveItemsAsync(IEnumerable<ListedItem> source, IEnumerable<string> destination, bool showDialog, bool registerHistory)
	{
		return MoveItemsAsync(source.Select((ListedItem item) => item.ToStorageItemWithPath()), destination, showDialog, registerHistory);
	}

	public Task<ReturnResult> MoveItemAsync(ListedItem source, string destination, bool showDialog, bool registerHistory)
	{
		return MoveItemAsync(source.ToStorageItemWithPath(), destination, showDialog, registerHistory);
	}

	public async Task<ReturnResult> MoveItemsAsync(IEnumerable<IStorageItemWithPath> source, IEnumerable<string> destination, bool showDialog, bool registerHistory)
	{
		(List<NameCollisionOption>, bool) tuple = await GetCollision(source, destination, showDialog);
		var (collisions, _) = tuple;
		if (tuple.Item2)
		{
			return ReturnResult.Cancelled;
		}
		IStorageHistory history = await filesystemOperations.MoveItemsAsync(source.ToList(), destination.ToList(), collisions);
		await Task.Yield();
		if (registerHistory && history != null)
		{
			App.HistoryWrapper.AddHistory(history);
		}
		return (history != null) ? ReturnResult.Success : ReturnResult.Failed;
	}

	public async Task<ReturnResult> MoveItemsAsync(IEnumerable<IStorageItemWithPath> source, IEnumerable<string> destination, IEnumerable<NameCollisionOption> collisions, bool registerHistory, IProgress<FileOperationProgress>? progress, CancellationToken cancellationToken)
	{
		try
		{
			IStorageHistory storageHistory = await filesystemOperations.MoveItemsAsync(source.ToList(), destination.ToList(), collisions.ToList(), progress, cancellationToken);
			if (registerHistory && storageHistory != null)
			{
				App.HistoryWrapper.AddHistory(storageHistory);
			}
			return (storageHistory != null) ? ReturnResult.Success : ReturnResult.Failed;
		}
		catch (OperationCanceledException)
		{
			return ReturnResult.Cancelled;
		}
	}

	public Task<ReturnResult> MoveItemAsync(IStorageItemWithPath source, string destination, bool showDialog, bool registerHistory)
	{
		return MoveItemsAsync(source.CreateEnumerable(), destination.CreateEnumerable(), showDialog, registerHistory);
	}

	public async Task<ReturnResult> MoveItemsAsync(IEnumerable<ListedItem> source, string targetDirectory, NameCollisionOption collision, bool showDialog, bool registerHistory = true)
	{
		IList<IStorageItemWithPath> source2 = await source.Select((ListedItem s) => s.ToStorageItemWithPath()).ToListAsync();
		List<string> destination = source2.Select((IStorageItemWithPath i) => Path.Combine(targetDirectory, i.Name)).ToList();
		return await MoveItemsAsync(source2, destination, showDialog, registerHistory);
	}

	public async Task<ReturnResult> MoveItemsAsync(IEnumerable<IStorageItem> source, string targetDirectory, NameCollisionOption collision, bool showDialog, bool registerHistory = true)
	{
		IList<IStorageItemWithPath> source2 = await (from s in source
			select StorageHelpers.FromPathAndType(s.Path.LocalPath) into item
			where item != null
			select item).ToListAsync();
		List<string> destination = source2.Select((IStorageItemWithPath i) => Path.Combine(targetDirectory, i.Name)).ToList();
		return await MoveItemsAsync(source2, destination, showDialog, registerHistory);
	}

	public static bool IsValidForFilename(string name)
	{
		if (string.IsNullOrWhiteSpace(name))
		{
			return false;
		}
		char[] anyOf = new char[1] { ':' };
		if (name.IndexOfAny(anyOf) >= 0)
		{
			return false;
		}
		if (name.Length > 255)
		{
			return false;
		}
		if (name.StartsWith("."))
		{
			return false;
		}
		return true;
	}

	public static (string name, string path) GenerateUniqueName(string directory, string desiredName, bool isFile)
	{
		string value;
		string value2;
		if (isFile)
		{
			value = Path.GetFileNameWithoutExtension(desiredName);
			value2 = Path.GetExtension(desiredName);
		}
		else
		{
			value = desiredName;
			value2 = string.Empty;
		}
		int num = 2;
		string text;
		string text2;
		do
		{
			text = $"{value} ({num}){value2}";
			text2 = Path.Combine(directory, text);
			num++;
		}
		while (isFile ? File.Exists(text2) : Directory.Exists(text2));
		return (name: text, path: text2);
	}

	public void Dispose()
	{
	}
}
