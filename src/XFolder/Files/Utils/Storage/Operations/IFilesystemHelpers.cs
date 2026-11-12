using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Avalonia.Platform.Storage;
using Files.Data.Enums;
using Files.Data.Enums.Storage;
using Files.Data.Items;
using Files.Utils.Storage.Helpers;

namespace Files.Utils.Storage.Operations;

public interface IFilesystemHelpers : IDisposable
{
	Task<(ReturnResult, IStorageItem?)> CreateAsync(IStorageItemWithPath source, bool registerHistory);

	Task<ReturnResult> DeleteItemsAsync(IEnumerable<IStorageItemWithPath> source, DeleteConfirmationPolicies showDialog, bool permanently, bool registerHistory);

	Task<ReturnResult> DeleteItemAsync(IStorageItemWithPath source, DeleteConfirmationPolicies showDialog, bool permanently, bool registerHistory);

	Task<ReturnResult> DeleteItemsAsync(IEnumerable<ListedItem> source, DeleteConfirmationPolicies showDialog, bool permanently, bool registerHistory);

	Task<ReturnResult> DeleteItemAsync(ListedItem source, DeleteConfirmationPolicies showDialog, bool permanently, bool registerHistory);

	Task<ReturnResult> CopyItemsAsync(IEnumerable<ListedItem> source, IEnumerable<string> destination, bool showDialog, bool registerHistory);

	Task<ReturnResult> CopyItemAsync(ListedItem source, string destination, bool showDialog, bool registerHistory);

	Task<ReturnResult> CopyItemsAsync(IEnumerable<IStorageItemWithPath> source, IEnumerable<string> destination, bool showDialog, bool registerHistory);

	Task<ReturnResult> CopyItemAsync(IStorageItemWithPath source, string destination, bool showDialog, bool registerHistory);

	Task<(List<NameCollisionOption> collisions, bool cancelOperation)> GetCollision(IEnumerable<IStorageItemWithPath> source, IEnumerable<string> destination, bool showDialog);

	Task<ReturnResult> MoveItemsAsync(IEnumerable<ListedItem> source, IEnumerable<string> destination, bool showDialog, bool registerHistory);

	Task<ReturnResult> MoveItemAsync(ListedItem source, string destination, bool showDialog, bool registerHistory);

	Task<ReturnResult> MoveItemAsync(IStorageItemWithPath source, string destination, bool showDialog, bool registerHistory);

	Task<ReturnResult> MoveItemsAsync(IEnumerable<ListedItem> source, string targetDirectory, NameCollisionOption collision, bool showDialog, bool registerHistory = true);

	Task<ReturnResult> MoveItemsAsync(IEnumerable<IStorageItem> source, string targetDirectory, NameCollisionOption collision, bool showDialog, bool registerHistory = true);

	Task<ReturnResult> MoveItemsAsync(IEnumerable<IStorageItemWithPath> source, IEnumerable<string> destination, bool showDialog, bool registerHistory);

	Task<ReturnResult> RenameAsync(ListedItem source, string newName, NameCollisionOption collision, bool registerHistory, bool showExtensionDialog = true);

	Task<ReturnResult> RenameAsync(IStorageItemWithPath source, string newName, NameCollisionOption collision, bool registerHistory, bool showExtensionDialog = true);
}
