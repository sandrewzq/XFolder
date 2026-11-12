using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using CommunityToolkit.Mvvm.DependencyInjection;
using Files.Assets.Lang;
using Files.Data.Commands;
using Files.Data.Commands.Manager;
using Files.Data.Contracts;
using Files.Data.Enums;
using Files.Data.Items;
using Files.Data.Models;
using Files.Helpers.UI;
using Files.ViewModels.Items;
using Files.ViewModels.Layouts;

namespace Files.Data.Factories;

public static class ContentPageContextFlyoutFactory
{
	private static readonly IUserSettingsService UserSettingsService = Ioc.Default.GetRequiredService<IUserSettingsService>();

	private static readonly IModifiableCommandManager ModifiableCommands = Ioc.Default.GetRequiredService<IModifiableCommandManager>();

	private static readonly ICommandManager Commands = Ioc.Default.GetRequiredService<ICommandManager>();

	private static readonly IQuickAccessService QuickAccessService = Ioc.Default.GetRequiredService<IQuickAccessService>();

	public static List<ContextMenuFlyoutItemViewModel> GetItemContextCommandsWithoutShellItems(TabItemViewModel tabItemViewModel, List<ListedItem> selectedItems, BaseLayoutViewModel commandsViewModel, bool shiftPressed, bool isItemContextMenu)
	{
		List<ContextMenuFlyoutItemViewModel> baseItemMenuItems = GetBaseItemMenuItems(commandsViewModel, selectedItems, tabItemViewModel, isItemContextMenu);
		bool shiftPressed2 = shiftPressed;
		List<ContextMenuFlyoutItemViewModel> list = Filter(baseItemMenuItems, selectedItems, shiftPressed2, tabItemViewModel);
		AvoidDuplicateSeparators(list);
		return list;
	}

	public static void AvoidDuplicateSeparators(List<ContextMenuFlyoutItemViewModel> menuItems)
	{
		if (menuItems == null || menuItems.Count == 0)
		{
			return;
		}
		for (int i = 0; i < menuItems.Count; i++)
		{
			if (menuItems[i].ItemType == ContextMenuFlyoutItemType.Separator)
			{
				menuItems[i].ShowItem = false;
				continue;
			}
			AvoidDuplicateSeparators(menuItems[i].Items);
			break;
		}
		int num = menuItems.Count - 1;
		while (num >= 0 && menuItems[num].ItemType == ContextMenuFlyoutItemType.Separator)
		{
			menuItems[num].ShowItem = false;
			num--;
		}
		bool flag = false;
		for (int j = 0; j < menuItems.Count; j++)
		{
			if ((menuItems[j].ItemType == ContextMenuFlyoutItemType.Separator) & flag)
			{
				menuItems[j].ShowItem = false;
			}
			flag = menuItems[j].ItemType == ContextMenuFlyoutItemType.Separator;
			AvoidDuplicateSeparators(menuItems[j].Items);
		}
	}

	public static List<ContextMenuFlyoutItemViewModel> Filter(List<ContextMenuFlyoutItemViewModel> items, List<ListedItem> selectedItems, bool shiftPressed, TabItemViewModel tabItemViewModel, bool removeOverflowMenu = true)
	{
		items = items.Where((ContextMenuFlyoutItemViewModel x) => Check(x, tabItemViewModel, selectedItems)).ToList();
		items.ForEach((ContextMenuFlyoutItemViewModel x) =>
		{
			x.Items = x.Items?.Where((ContextMenuFlyoutItemViewModel y) => Check(y, tabItemViewModel, selectedItems)).ToList();
		});
		ContextMenuFlyoutItemViewModel contextMenuFlyoutItemViewModel = items.FirstOrDefault((ContextMenuFlyoutItemViewModel x) => x.ID == "ItemOverflow");
		if (contextMenuFlyoutItemViewModel != null)
		{
			if (!shiftPressed)
			{
				List<ContextMenuFlyoutItemViewModel> list = items.Where((ContextMenuFlyoutItemViewModel x) => x.ShowOnShift).ToList();
				if (contextMenuFlyoutItemViewModel.Items.Count != 0 && list.Count > 0 && contextMenuFlyoutItemViewModel.Items.Last().ItemType != ContextMenuFlyoutItemType.Separator)
				{
					contextMenuFlyoutItemViewModel.Items.Add(new ContextMenuFlyoutItemViewModel
					{
						ItemType = ContextMenuFlyoutItemType.Separator
					});
				}
				items = items.Except(list).ToList();
				contextMenuFlyoutItemViewModel.Items.AddRange(list);
			}
			if ((contextMenuFlyoutItemViewModel.Items.Count == 0) & removeOverflowMenu)
			{
				items.Remove(contextMenuFlyoutItemViewModel);
			}
		}
		return items;
	}

	private static bool Check(ContextMenuFlyoutItemViewModel item, TabItemViewModel tabItemViewModel, List<ListedItem> selectedItems)
	{
		if ((item.ShowInRecycleBin || !tabItemViewModel.IsPageTypeRecycleBin) && (item.ShowInZipPage || !tabItemViewModel.IsPageTypeZipFolder) && (!item.SingleItemOnly || selectedItems.Count == 1))
		{
			return item.ShowItem;
		}
		return false;
	}

	public static List<ContextMenuFlyoutItemViewModel> GetBaseItemMenuItems(BaseLayoutViewModel commandsViewModel, List<ListedItem> selectedItems, TabItemViewModel tabItemViewModel, bool isItemContextMenu)
	{
		bool flag = isItemContextMenu;
		bool flag2 = flag && selectedItems.Count == 1 && selectedItems[0].IsFolder;
		bool isVisible = flag && selectedItems.Count == 1 && !selectedItems[0].IsFolder && File.Exists(selectedItems[0].ItemPath);
		bool flag3 = flag2 && QuickAccessService.IsPinnedToQuickAccess(selectedItems[0].ToStorageItemWithPath().Path);
		bool isExecutable = Commands[CommandCodes.ConvertToPng].IsExecutable;
		bool isExecutable2 = Commands[CommandCodes.ConvertToJpeg].IsExecutable;
		bool isExecutable3 = Commands[CommandCodes.ConvertToBmp].IsExecutable;
		bool isExecutable4 = Commands[CommandCodes.RotateLeft].IsExecutable;
		bool isExecutable5 = Commands[CommandCodes.RotateRight].IsExecutable;
		bool showItem = isExecutable4 | isExecutable5;
		bool showItem2 = isExecutable | isExecutable2 | isExecutable3;
		List<ContextMenuFlyoutItemViewModel> list = new List<ContextMenuFlyoutItemViewModel>();
		ContextMenuFlyoutItemViewModel contextMenuFlyoutItemViewModel = new ContextMenuFlyoutItemViewModel
		{
			Text = Resources.Layout,
			ShowItem = !flag,
			ShowInRecycleBin = true,
			ShowInSearchPage = true,
			ShowInFtpPage = true,
			ShowInZipPage = true
		};
		int num = 2;
		List<ContextMenuFlyoutItemViewModel> list2 = new List<ContextMenuFlyoutItemViewModel>(num);
		CollectionsMarshal.SetCount(list2, num);
		Span<ContextMenuFlyoutItemViewModel> span = CollectionsMarshal.AsSpan(list2);
		span[0] = new ContextMenuFlyoutItemViewModelBuilder(Commands[CommandCodes.LayoutDetails])
		{
			IsToggle = true
		}.Build();
		span[1] = new ContextMenuFlyoutItemViewModelBuilder(Commands[CommandCodes.LayoutGrid])
		{
			IsToggle = true
		}.Build();
		contextMenuFlyoutItemViewModel.Items = list2;
		list.Add(contextMenuFlyoutItemViewModel);
		list.Add(new ContextMenuFlyoutItemViewModel
		{
			Text = Resources.SortBy,
			ShowItem = !flag,
			ShowInRecycleBin = true,
			ShowInSearchPage = true,
			ShowInFtpPage = true,
			ShowInZipPage = true,
			Items = GetSortByItems(commandsViewModel),
			Icon = AvaloniaUIHelpers.GetIconResource("arrow_sort_regular")
		});
		list.Add(new ContextMenuFlyoutItemViewModel
		{
			ItemType = ContextMenuFlyoutItemType.Separator,
			ShowInRecycleBin = true,
			ShowInSearchPage = true,
			ShowInFtpPage = true,
			ShowInZipPage = true
		});
		list.Add(new ContextMenuFlyoutItemViewModelBuilder(Commands[CommandCodes.OpenItem])
		{
			IsVisible = (flag && selectedItems.Count == 1)
		}.Build());
		list.Add(new ContextMenuFlyoutItemViewModelBuilder(Commands[CommandCodes.OpenInNewTab])
		{
			IsVisible = (flag && selectedItems.Count > 0 && selectedItems.All((ListedItem i) => i.IsFolder))
		}.Build());
		list.Add(new ContextMenuFlyoutItemViewModelBuilder(Commands[CommandCodes.PinToSidebar])
		{
			IsVisible = (flag2 && !flag3)
		}.Build());
		ContextMenuFlyoutItemViewModel contextMenuFlyoutItemViewModel2 = new ContextMenuFlyoutItemViewModel
		{
			Text = Resources.Compress,
			ShowItem = flag
		};
		num = 2;
		List<ContextMenuFlyoutItemViewModel> list3 = new List<ContextMenuFlyoutItemViewModel>(num);
		CollectionsMarshal.SetCount(list3, num);
		Span<ContextMenuFlyoutItemViewModel> span2 = CollectionsMarshal.AsSpan(list3);
		span2[0] = new ContextMenuFlyoutItemViewModelBuilder(Commands[CommandCodes.CompressIntoZip])
		{
			IsVisible = flag
		}.Build();
		span2[1] = new ContextMenuFlyoutItemViewModelBuilder(Commands[CommandCodes.CompressIntoTar])
		{
			IsVisible = flag
		}.Build();
		contextMenuFlyoutItemViewModel2.Items = list3;
		list.Add(contextMenuFlyoutItemViewModel2);
		ContextMenuFlyoutItemViewModel contextMenuFlyoutItemViewModel3 = new ContextMenuFlyoutItemViewModel
		{
			Text = Resources.Rotate,
			ShowItem = showItem
		};
		num = 2;
		List<ContextMenuFlyoutItemViewModel> list4 = new List<ContextMenuFlyoutItemViewModel>(num);
		CollectionsMarshal.SetCount(list4, num);
		Span<ContextMenuFlyoutItemViewModel> span3 = CollectionsMarshal.AsSpan(list4);
		span3[0] = new ContextMenuFlyoutItemViewModelBuilder(Commands[CommandCodes.RotateLeft])
		{
			IsVisible = isExecutable4
		}.Build();
		span3[1] = new ContextMenuFlyoutItemViewModelBuilder(Commands[CommandCodes.RotateRight])
		{
			IsVisible = isExecutable5
		}.Build();
		contextMenuFlyoutItemViewModel3.Items = list4;
		list.Add(contextMenuFlyoutItemViewModel3);
		ContextMenuFlyoutItemViewModel contextMenuFlyoutItemViewModel4 = new ContextMenuFlyoutItemViewModel
		{
			Text = Resources.Convert,
			ShowItem = showItem2
		};
		num = 3;
		List<ContextMenuFlyoutItemViewModel> list5 = new List<ContextMenuFlyoutItemViewModel>(num);
		CollectionsMarshal.SetCount(list5, num);
		Span<ContextMenuFlyoutItemViewModel> span4 = CollectionsMarshal.AsSpan(list5);
		span4[0] = new ContextMenuFlyoutItemViewModelBuilder(Commands[CommandCodes.ConvertToPng])
		{
			IsVisible = isExecutable
		}.Build();
		span4[1] = new ContextMenuFlyoutItemViewModelBuilder(Commands[CommandCodes.ConvertToJpeg])
		{
			IsVisible = isExecutable2
		}.Build();
		span4[2] = new ContextMenuFlyoutItemViewModelBuilder(Commands[CommandCodes.ConvertToBmp])
		{
			IsVisible = isExecutable3
		}.Build();
		contextMenuFlyoutItemViewModel4.Items = list5;
		list.Add(contextMenuFlyoutItemViewModel4);
		list.Add(new ContextMenuFlyoutItemViewModelBuilder(Commands[CommandCodes.OpenHashes])
		{
			IsVisible = isVisible
		}.Build());
		list.Add(new ContextMenuFlyoutItemViewModelBuilder(Commands[CommandCodes.OpenProperties])
		{
			IsVisible = flag,
			AccessKey = "I"
		}.Build());
		list.Add(new ContextMenuFlyoutItemViewModelBuilder(Commands[CommandCodes.Rename])
		{
			AccessKey = "M"
		}.Build());
		list.Add(new ContextMenuFlyoutItemViewModelBuilder(Commands[CommandCodes.CutItem])
		{
			AccessKey = "X"
		}.Build());
		list.Add(new ContextMenuFlyoutItemViewModelBuilder(Commands[CommandCodes.CopyItem])
		{
			AccessKey = "C"
		}.Build());
		list.Add(new ContextMenuFlyoutItemViewModelBuilder(Commands[CommandCodes.CopyItemPath]).Build());
		list.Add(new ContextMenuFlyoutItemViewModelBuilder(Commands[CommandCodes.CopyItemPathWithQuotes]).Build());
		list.Add(new ContextMenuFlyoutItemViewModelBuilder(Commands[CommandCodes.CreateFolderWithSelection])
		{
			IsVisible = flag
		}.Build());
		list.Add(new ContextMenuFlyoutItemViewModelBuilder(Commands[CommandCodes.CopyPath])
		{
			IsVisible = !flag
		}.Build());
		list.Add(new ContextMenuFlyoutItemViewModelBuilder(Commands[CommandCodes.CopyPathWithQuotes])
		{
			IsVisible = !flag
		}.Build());
		list.Add(new ContextMenuFlyoutItemViewModelBuilder(Commands[CommandCodes.PasteItem])
		{
			AccessKey = "V"
		}.Build());
		list.Add(new ContextMenuFlyoutItemViewModelBuilder(Commands[CommandCodes.PasteItemToSelection]).Build());
		list.Add(new ContextMenuFlyoutItemViewModelBuilder(Commands[CommandCodes.DeleteItem])
		{
			IsVisible = flag
		}.Build());
		list.Add(new ContextMenuFlyoutItemViewModelBuilder(Commands[CommandCodes.DeleteItemPermanently])
		{
			IsVisible = flag
		}.Build());
		list.Add(new ContextMenuFlyoutItemViewModel
		{
			ItemType = ContextMenuFlyoutItemType.Separator,
			ShowInFtpPage = true,
			ShowInZipPage = true,
			ShowItem = !flag
		});
		list.Add(new ContextMenuFlyoutItemViewModel
		{
			Text = Resources.Add,
			Items = GetNewItemItems(commandsViewModel, tabItemViewModel.CanCreateFileInPage),
			ShowItem = !flag,
			ShowInFtpPage = true
		});
		list.Add(new ContextMenuFlyoutItemViewModel
		{
			ItemType = ContextMenuFlyoutItemType.Separator,
			ShowItem = ((!flag && Commands[CommandCodes.OpenFinder].IsExecutable) || (!flag && Commands[CommandCodes.OpenTerminal].IsExecutable)),
			ShowOnShift = true
		});
		list.Add(new ContextMenuFlyoutItemViewModelBuilder(Commands[CommandCodes.OpenFinder])
		{
			IsVisible = (!flag && Commands[CommandCodes.OpenFinder].IsExecutable),
			ShowOnShift = true
		}.Build());
		list.Add(new ContextMenuFlyoutItemViewModelBuilder(Commands[CommandCodes.OpenTerminal])
		{
			IsVisible = (!flag && Commands[CommandCodes.OpenTerminal].IsExecutable),
			ShowOnShift = true
		}.Build());
		list.Add(new ContextMenuFlyoutItemViewModel
		{
			Text = Resources.MoreOptions,
			Items = new List<ContextMenuFlyoutItemViewModel>(),
			ID = "ItemOverflow",
			Tag = "ItemOverflow",
			IsEnabled = true
		});
		return list.Where((ContextMenuFlyoutItemViewModel x) => x.ShowItem).ToList();
	}

	public static List<ContextMenuFlyoutItemViewModel> GetNewItemItems(BaseLayoutViewModel commandsViewModel, bool canCreateFileInPage)
	{
		return new List<ContextMenuFlyoutItemViewModel>
		{
			new ContextMenuFlyoutItemViewModelBuilder(Commands[CommandCodes.CreateFolder]).Build(),
			new ContextMenuFlyoutItemViewModelBuilder(Commands[CommandCodes.CreateNewFile]).Build()
		};
	}

	private static List<ContextMenuFlyoutItemViewModel> GetSortByItems(BaseLayoutViewModel commandsViewModel)
	{
		return new List<ContextMenuFlyoutItemViewModel>
		{
			CreateSortByItem(commandsViewModel, Resources.Name, SortOption.Name),
			CreateSortByItem(commandsViewModel, Resources.DateModifiedLowerCase, SortOption.DateModified),
			CreateSortByItem(commandsViewModel, Resources.DateCreated, SortOption.DateCreated),
			CreateSortByItem(commandsViewModel, Resources.Size, SortOption.Size),
			CreateSortByItem(commandsViewModel, Resources.Type, SortOption.FileType),
			new ContextMenuFlyoutItemViewModel
			{
				ItemType = ContextMenuFlyoutItemType.Separator,
				ShowInRecycleBin = true,
				ShowInSearchPage = true,
				ShowInFtpPage = true,
				ShowInZipPage = true
			},
			CreateSortDirectionItem(commandsViewModel, Resources.Ascending, SortDirection.Ascending),
			CreateSortDirectionItem(commandsViewModel, Resources.Descending, SortDirection.Descending)
		};
	}

	private static ContextMenuFlyoutItemViewModel CreateSortDirectionItem(BaseLayoutViewModel commandsViewModel, string text, SortDirection sortDirection)
	{
		return new ContextMenuFlyoutItemViewModel
		{
			Text = text,
			ItemType = ContextMenuFlyoutItemType.Toggle,
			Command = commandsViewModel.SortDirectionCommand,
			CommandParameter = sortDirection,
			IsChecked = (commandsViewModel.CurrentSortDirection == sortDirection),
			ShowInRecycleBin = true,
			ShowInSearchPage = true,
			ShowInFtpPage = true,
			ShowInZipPage = true
		};
	}

	private static ContextMenuFlyoutItemViewModel CreateSortByItem(BaseLayoutViewModel commandsViewModel, string text, SortOption sortOption)
	{
		return new ContextMenuFlyoutItemViewModel
		{
			Text = text,
			ItemType = ContextMenuFlyoutItemType.Toggle,
			Command = commandsViewModel.SortItemsCommand,
			CommandParameter = sortOption,
			IsChecked = (commandsViewModel.CurrentSortOption == sortOption),
			ShowInRecycleBin = true,
			ShowInSearchPage = true,
			ShowInFtpPage = true,
			ShowInZipPage = true
		};
	}
}
