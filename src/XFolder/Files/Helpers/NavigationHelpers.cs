using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.DependencyInjection;
using Files.Data.Contexts;
using Files.Data.Contracts;
using Files.Data.Enums.Navigation;
using Files.Data.Items;
using Files.Data.Parameters;
using Files.Helpers.Shared;
using Files.Services.NativePlugin;
using Files.Utils.Storage.Helpers;
using Files.ViewModels.Items;

namespace Files.Helpers;

public static class NavigationHelpers
{
	public static string GetDefaultNewTabPath()
	{
		string newTabDefaultPath = Ioc.Default.GetRequiredService<IGeneralSettingsService>().NewTabDefaultPath;
		if (!string.IsNullOrWhiteSpace(newTabDefaultPath))
		{
			string text = PathHelpers.NormalizePath(newTabDefaultPath);
			if (!string.IsNullOrWhiteSpace(text))
			{
				if (text.Length <= 1)
				{
					return text;
				}
				return text.TrimEnd('/');
			}
		}
		string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
		if (!string.IsNullOrWhiteSpace(folderPath))
		{
			return folderPath.TrimEnd('/');
		}
		string folderPath2 = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
		if (!string.IsNullOrWhiteSpace(folderPath2))
		{
			return Path.Combine(folderPath2, "Desktop");
		}
		return "/";
	}

	public static async Task AddNewTabByPathAsync(string? path, bool switchToNewTab, int atIndex = -1)
	{
		AddNewTabByViewModel(TabItemViewModel.CreateNewTab(string.IsNullOrEmpty(path) ? GetDefaultNewTabPath() : path), switchToNewTab, atIndex);
	}

	public static async Task AddNewTabByParamAsync(NavigationParams param, bool switchToNewTab, int atIndex = -1)
	{
		AddNewTabByViewModel(TabItemViewModel.CreateNewTab(string.IsNullOrEmpty(param.NavPath) ? GetDefaultNewTabPath() : param.NavPath), switchToNewTab, atIndex);
	}

	private static void AddNewTabByViewModel(TabItemViewModel tabItem, bool switchToNewTab, int atIndex)
	{
		IMultiPanesContext requiredService = Ioc.Default.GetRequiredService<IMultiPanesContext>();
		tabItem.ParentShellViewModel = requiredService.ActivePaneViewModel;
		int index = ((atIndex == -1) ? requiredService.ActivePaneViewModel.TabItems.Count : atIndex);
		requiredService.ActivePaneViewModel.TabItems.Insert(index, tabItem);
		if (switchToNewTab)
		{
			requiredService.ActivePaneViewModel.SelectedTab = requiredService.ActivePaneViewModel.TabItems[index];
		}
	}

	public static void CloseSelectedTab()
	{
		IMultiPanesContext requiredService = Ioc.Default.GetRequiredService<IMultiPanesContext>();
		if (requiredService.ActivePaneViewModel?.SelectedTab != null)
		{
			requiredService.ActivePaneViewModel.CloseTab(requiredService.ActivePaneViewModel.SelectedTab);
		}
	}

	public static async Task OpenSelectedItemsAsync(bool openViaApplicationPicker = false)
	{
		List<ListedItem> list = Ioc.Default.GetRequiredService<IMultiPanesContext>().ActivePaneViewModel.SelectedTab.SelectedItems.ToList();
		if (list.Count == 0)
		{
			return;
		}
		List<ListedItem> folders = list.Where((ListedItem item) => item.IsFolder).ToList();
		List<ListedItem> files = list.Where((ListedItem item) => !item.IsFolder).ToList();
		if (folders.Count > 0)
		{
			await OpenDirectory(folders[0].ItemPath);
			for (int i = 1; i < folders.Count; i++)
			{
				await AddNewTabByPathAsync(folders[i].ItemPath, switchToNewTab: false);
			}
		}
		foreach (ListedItem item in files)
		{
			await OpenFile(item.ItemPath);
		}
	}

	public static async Task<bool> OpenPath(string path)
	{
		FilesystemResult filesystemResult = (FilesystemResult)false;
		if (Directory.Exists(path))
		{
			filesystemResult = await OpenDirectory(path);
		}
		else if (File.Exists(path))
		{
			filesystemResult = await OpenFile(path);
		}
		return filesystemResult;
	}

	private static async Task<FilesystemResult> OpenDirectory(string path)
	{
		return (FilesystemResult)Ioc.Default.GetRequiredService<IMultiPanesContext>().ActivePaneViewModel.SetWorkingDirectory(path, SetNavPathLocationEnum.OpenItemAction);
	}

	private static async Task<FilesystemResult> OpenFile(string path)
	{
		try
		{
			return (FilesystemResult)MacPluginInterop.OpenFileWithDefaultApp(path);
		}
		catch (Exception)
		{
			return (FilesystemResult)false;
		}
	}
}
