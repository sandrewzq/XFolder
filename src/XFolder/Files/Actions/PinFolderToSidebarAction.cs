using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Input;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Messaging;
using Files.Assets.Lang;
using Files.Data.Commands.HotKey;
using Files.Data.Contracts;
using Files.Data.Items;
using Files.Data.Messages;
using Files.Helpers.Shared;

namespace Files.Actions;

internal sealed class PinFolderToSidebarAction : BaseTransferItemAction, IAction
{
	public string Label => Resources.PinFolderToSidebar;

	public string Description => Resources.PinFolderToSidebar;

	public string? Icon => "pin_regular";

	public HotKey HotKey => new HotKey(Key.None);

	bool IAction.IsExecutable => true;

	public Task ExecuteAsync(object? parameter = null)
	{
		string pathToPin = GetPathToPin(parameter);
		if (string.IsNullOrWhiteSpace(pathToPin))
		{
			return Task.CompletedTask;
		}
		IQuickAccessService requiredService = Ioc.Default.GetRequiredService<IQuickAccessService>();
		if (requiredService.IsPinnedToQuickAccess(pathToPin))
		{
			return Task.CompletedTask;
		}
		List<string> list = requiredService.LoadQuickAccessPaths()?.ToList() ?? new List<string>();
		list.Add(pathToPin);
		requiredService.SaveQuickAccessPaths(list);
		WeakReferenceMessenger.Default.Send(new QuickAccessChangedMessage());
		return Task.CompletedTask;
	}

	private string? GetPathToPin(object? parameter)
	{
		if (parameter is string text)
		{
			if (!IsPinnableFolderPath(text))
			{
				return null;
			}
			return text;
		}
		IReadOnlyList<ListedItem> selectedItems = ContentPageContext.SelectedItems;
		if (selectedItems == null || selectedItems.Count != 1)
		{
			return null;
		}
		ListedItem listedItem = selectedItems[0];
		if (!listedItem.IsFolder || string.IsNullOrWhiteSpace(listedItem.ItemPath))
		{
			return null;
		}
		if (!IsPinnableFolderPath(listedItem.ItemPath))
		{
			return null;
		}
		return listedItem.ItemPath;
	}

	private static bool IsPinnableFolderPath(string path)
	{
		if (string.IsNullOrWhiteSpace(path))
		{
			return false;
		}
		path = PathHelpers.NormalizePath(path);
		if (!PathHelpers.IsRootPath(path))
		{
			return Directory.Exists(path);
		}
		return false;
	}
}
