using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Data.Converters;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Markup.Xaml;
using Avalonia.Markup.Xaml.Converters;
using Avalonia.Markup.Xaml.MarkupExtensions;
using Avalonia.Markup.Xaml.MarkupExtensions.CompiledBindings;
using Avalonia.Markup.Xaml.XamlIl.Runtime;
using Avalonia.Media;
using Avalonia.Media.Immutable;
using Avalonia.Threading;
using Avalonia.VisualTree;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Messaging;
using CompiledAvaloniaXaml;
using DialogHostAvalonia;
using Files.Assets.Lang;
using Files.Converters;
using Files.Data.Commands;
using Files.Data.Commands.HotKey;
using Files.Data.Commands.Manager;
using Files.Data.Contexts;
using Files.Data.Contexts.ContentPage;
using Files.Data.Contracts;
using Files.Data.Enums;
using Files.Data.Items;
using Files.Data.Messages;
using Files.Data.Models;
using Files.Helpers;
using Files.Helpers.Shared;
using Files.Services.App;
using Files.Services.NativePlugin;
using Files.UserControls.Sidebar;
using Files.ViewModels;
using Files.ViewModels.Layouts;
using Microsoft.Extensions.Logging;

namespace Files.Views;

public class MainWindow : Window, IRecipient<QuickAccessChangedMessage>
{
	[CompilerGenerated]
	private class XamlClosure_8
	{
		public static object Build_1(IServiceProvider P_0)
		{
			XamlIlContext.Context<MainWindow> context = CreateContext(P_0);
			return new IsPaneOpenToGridLengthConverter();
		}

		public static XamlIlContext.Context<MainWindow> CreateContext(IServiceProvider P_0)
		{
			XamlIlContext.Context<MainWindow> context = new XamlIlContext.Context<MainWindow>(P_0, new object[1] { _0021AvaloniaResources.NamespaceInfo_003A_002FViews_002FMainWindow_002Eaxaml.Singleton }, "avares://XFolder/Views/MainWindow.axaml");
			if (P_0 != null)
			{
				object service = P_0.GetService(typeof(IRootObjectProvider));
				if (service != null)
				{
					service = ((IRootObjectProvider)service).RootObject;
					context.RootObject = (MainWindow)service;
				}
			}
			return context;
		}
	}

	private const double MacTrafficLightSafeWidth = 92.0;

	private readonly ICommandManager Commands;

	private MainWindowViewModel _viewModel = new MainWindowViewModel();

	private readonly ILogger<MainWindow>? _logger;

	private readonly HashSet<GridSplitter> _draggingSplitters = new HashSet<GridSplitter>();

	private IQuickAccessService? _quickAccessService;

	private NativeMenu? _goMenu;

	private NativeMenu? _editMenu;

	private NativeMenu? _viewMenu;

	private NativeMenu? _layoutMenu;

	private NativeMenu? _sortByMenu;

	private readonly List<(CommandCodes CommandCode, NativeMenuItem MenuItem, EventHandler ClickHandler, EventHandler CanExecuteChangedHandler)> _commandMenuItemSubscriptions = new List<(CommandCodes, NativeMenuItem, EventHandler, EventHandler)>();

	private readonly List<(NativeMenuItem MenuItem, EventHandler ClickHandler)> _menuItemClickSubscriptions = new List<(NativeMenuItem, EventHandler)>();

	private NativeMenuItem? _navigateBackMenuItem;

	private NativeMenuItem? _navigateForwardMenuItem;

	private NativeMenuItem? _navigateUpMenuItem;

	private NativeMenuItem? _toggleSidebarMenuItem;

	private NativeMenuItem? _layoutDetailsMenuItem;

	private NativeMenuItem? _layoutGridMenuItem;

	private NativeMenuItem? _showHiddenItemsMenuItem;

	private NativeMenuItem? _showFileExtensionsMenuItem;

	private NativeMenuItem? _sortByNameMenuItem;

	private NativeMenuItem? _sortByDateModifiedMenuItem;

	private NativeMenuItem? _sortByDateCreatedMenuItem;

	private NativeMenuItem? _sortBySizeMenuItem;

	private NativeMenuItem? _sortByTypeMenuItem;

	private NativeMenuItem? _sortAscendingMenuItem;

	private NativeMenuItem? _sortDescendingMenuItem;

	private bool keyReleased = true;

	[GeneratedCode("Avalonia.Generators.NameGenerator.InitializeComponentCodeGenerator", "12.0.1.0")]
	internal Grid TopToolbarGrid;

	[CompilerGenerated]
	private static Action<object> _0021XamlIlPopulateOverride;

	public MainWindow()
	{
		InitializeComponent();
		if (!Design.IsDesignMode)
		{
			Commands = Ioc.Default.GetRequiredService<ICommandManager>();
			_quickAccessService = Ioc.Default.GetRequiredService<IQuickAccessService>();
			_logger = Ioc.Default.GetRequiredService<ILogger<MainWindow>>();
			if (Ioc.Default.GetRequiredService<IGeneralSettingsService>().OpenMainWindowMaximized)
			{
				WindowState = WindowState.Maximized;
			}
			DataContext = _viewModel;
			InitializeMenuGestures();
			InitializeGoMenuQuickAccessItems();
			WeakReferenceMessenger.Default.Register(this);
		}
		_viewModel.PanesView = new ShellPanesViewModel();
		KeyDown += MainWindow_KeyDown;
		AddHandler(InputElement.KeyUpEvent, MainWindow_KeyUp, RoutingStrategies.Tunnel, handledEventsToo: true);
		LostFocus += (object? sender, FocusChangedEventArgs e) =>
		{
			ResetHotKeyState();
		};
		GotFocus += (object? sender, FocusChangedEventArgs e) =>
		{
			ResetHotKeyState();
		};
		Activated += (object? sender, EventArgs e) =>
		{
			ResetHotKeyState();
		};
		Deactivated += (object? sender, EventArgs e) =>
		{
			ResetHotKeyState();
		};
		PropertyChanged += (object? _, AvaloniaPropertyChangedEventArgs e) =>
		{
			if (e.Property == Window.WindowStateProperty)
			{
				ResetHotKeyState();
				OnWindowStateChanged((WindowState)e.NewValue);
			}
		};
		Opened += MainWindow_Opened;
		Closed += MainWindow_Closed;
	}

	private async void MainWindow_Opened(object? sender, EventArgs e)
	{
		Opened -= MainWindow_Opened;
		RefreshCommandMenuItemStates();
		RefreshViewMenuCheckedStates();
		await Ioc.Default.GetRequiredService<DrivesViewModel>().InitializeAsync();
		if (!(await Ioc.Default.GetRequiredService<VersionExpirationService>().CheckExpirationAsync()))
		{
			await Ioc.Default.GetRequiredService<UpdateService>().CheckForUpdatesAsync();
		}
	}

	private void MainWindow_Closed(object? sender, EventArgs e)
	{
		UnsubscribeCommandMenuItems();
		WeakReferenceMessenger.Default.UnregisterAll(this);
	}

	private void OnWindowStateChanged(WindowState newState)
	{
		if (OperatingSystem.IsMacOS())
		{
			ColumnDefinition columnDefinition = TopToolbarGrid.ColumnDefinitions[0];
			if (newState == WindowState.FullScreen)
			{
				columnDefinition.Width = new GridLength(0.0);
				ExtendClientAreaToDecorationsHint = false;
			}
			else
			{
				ExtendClientAreaToDecorationsHint = true;
				columnDefinition.Width = new GridLength(92.0);
			}
		}
	}

	private void OnTitleBarDragAreaPointerPressed(object? sender, PointerPressedEventArgs e)
	{
		if (!e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
		{
			return;
		}
		if (e.ClickCount == 2)
		{
			ToggleWindowMaximize();
			e.Handled = true;
			return;
		}
		try
		{
			BeginMoveDrag(e);
			e.Handled = true;
		}
		catch (InvalidOperationException)
		{
		}
	}

	private void ToggleWindowMaximize()
	{
		if (WindowState == WindowState.Maximized)
		{
			WindowState = WindowState.Normal;
		}
		else if (WindowState == WindowState.Normal)
		{
			WindowState = WindowState.Maximized;
		}
	}

	private void OnSidebarSplitterPointerEntered(object? sender, PointerEventArgs e)
	{
		if (sender is GridSplitter splitter)
		{
			SetSidebarSplitterHighlight(splitter, isHighlighted: true);
		}
	}

	private void OnSidebarSplitterPointerExited(object? sender, PointerEventArgs e)
	{
		if (sender is GridSplitter gridSplitter && !_draggingSplitters.Contains(gridSplitter))
		{
			SetSidebarSplitterHighlight(gridSplitter, isHighlighted: false);
		}
	}

	private void OnSidebarSplitterDragStarted(object? sender, VectorEventArgs e)
	{
		if (sender is GridSplitter gridSplitter)
		{
			_draggingSplitters.Add(gridSplitter);
			SetSidebarSplitterHighlight(gridSplitter, isHighlighted: true);
		}
	}

	private void OnSidebarSplitterDragCompleted(object? sender, VectorEventArgs e)
	{
		if (sender is GridSplitter gridSplitter)
		{
			_draggingSplitters.Remove(gridSplitter);
			SetSidebarSplitterHighlight(gridSplitter, gridSplitter.IsPointerOver);
		}
	}

	private static void SetSidebarSplitterHighlight(GridSplitter splitter, bool isHighlighted)
	{
		splitter[!TemplatedControl.BackgroundProperty] = new DynamicResourceExtension(isHighlighted ? "SystemAccentColorBrush" : "TopToolbarBackground");
		splitter.Opacity = (isHighlighted ? 1.0 : 0.5);
	}

	private void InitializeMenuGestures()
	{
		NativeMenu menu = NativeMenu.GetMenu(this);
		if (menu != null)
		{
			InitializeFileMenuItems(menu);
			InitializeEditMenuItems(menu);
			InitializeViewMenuItems(menu);
			InitializeGoNavigationMenuItems();
			InitializeTabsMenuItems(menu);
			InitializeToolsMenuItems(menu);
			MacPluginInterop.InstallMainMenuKeyEquivalentPassthrough(GetTopLevelMenuTitles(menu), new string[1] { Files.Assets.Lang.Resources.Settings });
		}
	}

	private static string[] GetTopLevelMenuTitles(NativeMenu mainMenu)
	{
		return (from item in mainMenu.Items.OfType<NativeMenuItem>()
			select item.Header?.ToString() into title
			where !string.IsNullOrWhiteSpace(title)
			select (title)).ToArray();
	}

	private KeyGesture? GetCommandGesture(CommandCodes commandCode)
	{
		return ShortcutHelpers.GetPrimaryKeyGesture(Commands[commandCode].HotKeys);
	}

	private void InitializeFileMenuItems(NativeMenu mainMenu)
	{
		if (mainMenu.Items.Count < 1 || !(mainMenu.Items[0] is NativeMenuItem nativeMenuItem))
		{
			return;
		}
		NativeMenu menu = nativeMenuItem.Menu;
		if (menu == null)
		{
			return;
		}
		if (menu.Items[0] is NativeMenuItem nativeMenuItem2)
		{
			NativeMenu menu2 = nativeMenuItem2.Menu;
			if (menu2 != null && menu2.Items.Count >= 3)
			{
				InitializeCommandMenuItem(menu2.Items[0] as NativeMenuItem, CommandCodes.CreateNewFile);
				InitializeCommandMenuItem(menu2.Items[1] as NativeMenuItem, CommandCodes.CreateFolder);
				InitializeCommandMenuItem(menu2.Items[2] as NativeMenuItem, CommandCodes.CreateFolderWithSelection);
			}
		}
		InitializeCommandMenuItem(menu.Items[1] as NativeMenuItem, CommandCodes.OpenProperties);
		InitializeCommandMenuItem(menu.Items[3] as NativeMenuItem, CommandCodes.OpenItem);
		InitializeCommandMenuItem(menu.Items[4] as NativeMenuItem, CommandCodes.OpenInNewTab);
		InitializeCommandMenuItem(menu.Items[6] as NativeMenuItem, CommandCodes.OpenWorkspaceFromFile);
		InitializeCommandMenuItem(menu.Items[7] as NativeMenuItem, CommandCodes.SaveWorkspaceAs);
	}

	private void InitializeGoMenuQuickAccessItems()
	{
		_goMenu = TryGetGoMenu();
		RefreshGoMenuQuickAccessItems();
	}

	private void InitializeEditMenuItems(NativeMenu mainMenu)
	{
		if (mainMenu.Items.Count >= 2 && mainMenu.Items[1] is NativeMenuItem nativeMenuItem)
		{
			NativeMenu menu = nativeMenuItem.Menu;
			if (menu != null)
			{
				InitializeCommandMenuItem(menu.Items[0] as NativeMenuItem, CommandCodes.Undo);
				InitializeCommandMenuItem(menu.Items[1] as NativeMenuItem, CommandCodes.Redo);
				InitializeCommandMenuItem(menu.Items[3] as NativeMenuItem, CommandCodes.CutItem);
				InitializeCommandMenuItem(menu.Items[4] as NativeMenuItem, CommandCodes.CopyItem);
				InitializeCommandMenuItem(menu.Items[5] as NativeMenuItem, CommandCodes.PasteItem);
				InitializeCommandMenuItem(menu.Items[6] as NativeMenuItem, CommandCodes.Rename);
				InitializeCommandMenuItem(menu.Items[7] as NativeMenuItem, CommandCodes.DeleteItem);
				InitializeCommandMenuItem(menu.Items[8] as NativeMenuItem, CommandCodes.DeleteItemPermanently);
				InitializeCommandMenuItem(menu.Items[10] as NativeMenuItem, CommandCodes.SelectAll);
				InitializeCommandMenuItem(menu.Items[11] as NativeMenuItem, CommandCodes.InvertSelection);
				InitializeCommandMenuItem(menu.Items[12] as NativeMenuItem, CommandCodes.ClearSelection);
				InitializeCommandMenuItem(menu.Items[14] as NativeMenuItem, CommandCodes.CopyPath);
				InitializeCommandMenuItem(menu.Items[15] as NativeMenuItem, CommandCodes.CopyPathWithQuotes);
				InitializeCommandMenuItem(menu.Items[16] as NativeMenuItem, CommandCodes.CopyItemPath);
				InitializeCommandMenuItem(menu.Items[17] as NativeMenuItem, CommandCodes.CopyItemPathWithQuotes);
				InitializeCommandMenuItem(menu.Items[19] as NativeMenuItem, CommandCodes.EditPath);
				_editMenu = menu;
				_editMenu.NeedsUpdate -= EditMenu_NeedsUpdate;
				_editMenu.NeedsUpdate += EditMenu_NeedsUpdate;
				RemoveInjectedEditMenuItems();
			}
		}
	}

	private void EditMenu_NeedsUpdate(object? sender, EventArgs e)
	{
		RemoveInjectedEditMenuItems();
	}

	private static void RemoveInjectedEditMenuItems()
	{
		MacPluginInterop.RemoveNativeMenuItemsAfter(Files.Assets.Lang.Resources.Edit, Files.Assets.Lang.Resources.EditPath);
	}

	private void InitializeViewMenuItems(NativeMenu mainMenu)
	{
		if (mainMenu.Items.Count < 3 || !(mainMenu.Items[2] is NativeMenuItem nativeMenuItem))
		{
			return;
		}
		NativeMenu menu = nativeMenuItem.Menu;
		if (menu == null || menu.Items.Count < 7)
		{
			return;
		}
		InitializeCommandMenuItem(menu.Items[0] as NativeMenuItem, CommandCodes.ToggleFullScreen);
		InitializeCommandMenuItem(menu.Items[1] as NativeMenuItem, CommandCodes.ToggleSidebar);
		_toggleSidebarMenuItem = menu.Items[1] as NativeMenuItem;
		ConfigureToggleMenuItem(_toggleSidebarMenuItem);
		if (menu.Items[3] is NativeMenuItem nativeMenuItem2)
		{
			NativeMenu menu2 = nativeMenuItem2.Menu;
			if (menu2 != null && menu2.Items.Count >= 2)
			{
				InitializeCommandMenuItem(menu2.Items[0] as NativeMenuItem, CommandCodes.LayoutDetails);
				InitializeCommandMenuItem(menu2.Items[1] as NativeMenuItem, CommandCodes.LayoutGrid);
				_layoutMenu = menu2;
				_layoutDetailsMenuItem = menu2.Items[0] as NativeMenuItem;
				_layoutGridMenuItem = menu2.Items[1] as NativeMenuItem;
				ConfigureToggleMenuItem(_layoutDetailsMenuItem);
				ConfigureToggleMenuItem(_layoutGridMenuItem);
			}
		}
		if (menu.Items[4] is NativeMenuItem nativeMenuItem3)
		{
			NativeMenu menu3 = nativeMenuItem3.Menu;
			if (menu3 != null && menu3.Items.Count >= 8)
			{
				InitializeSortMenuItem(menu3.Items[0] as NativeMenuItem, SortOption.Name);
				InitializeSortMenuItem(menu3.Items[1] as NativeMenuItem, SortOption.DateModified);
				InitializeSortMenuItem(menu3.Items[2] as NativeMenuItem, SortOption.DateCreated);
				InitializeSortMenuItem(menu3.Items[3] as NativeMenuItem, SortOption.Size);
				InitializeSortMenuItem(menu3.Items[4] as NativeMenuItem, SortOption.FileType);
				InitializeSortDirectionMenuItem(menu3.Items[6] as NativeMenuItem, SortDirection.Ascending);
				InitializeSortDirectionMenuItem(menu3.Items[7] as NativeMenuItem, SortDirection.Descending);
				_sortByMenu = menu3;
				_sortByNameMenuItem = menu3.Items[0] as NativeMenuItem;
				_sortByDateModifiedMenuItem = menu3.Items[1] as NativeMenuItem;
				_sortByDateCreatedMenuItem = menu3.Items[2] as NativeMenuItem;
				_sortBySizeMenuItem = menu3.Items[3] as NativeMenuItem;
				_sortByTypeMenuItem = menu3.Items[4] as NativeMenuItem;
				_sortAscendingMenuItem = menu3.Items[6] as NativeMenuItem;
				_sortDescendingMenuItem = menu3.Items[7] as NativeMenuItem;
				ConfigureToggleMenuItem(_sortByNameMenuItem);
				ConfigureToggleMenuItem(_sortByDateModifiedMenuItem);
				ConfigureToggleMenuItem(_sortByDateCreatedMenuItem);
				ConfigureToggleMenuItem(_sortBySizeMenuItem);
				ConfigureToggleMenuItem(_sortByTypeMenuItem);
				ConfigureToggleMenuItem(_sortAscendingMenuItem);
				ConfigureToggleMenuItem(_sortDescendingMenuItem);
			}
		}
		InitializeCommandMenuItem(menu.Items[5] as NativeMenuItem, CommandCodes.ToggleShowHiddenItems);
		_showHiddenItemsMenuItem = menu.Items[5] as NativeMenuItem;
		ConfigureToggleMenuItem(_showHiddenItemsMenuItem);
		InitializeCommandMenuItem(menu.Items[6] as NativeMenuItem, CommandCodes.ToggleShowFileExtensions);
		_showFileExtensionsMenuItem = menu.Items[6] as NativeMenuItem;
		ConfigureToggleMenuItem(_showFileExtensionsMenuItem);
		InitializeCommandMenuItem(menu.Items[7] as NativeMenuItem, CommandCodes.RefreshItems);
		_viewMenu = menu;
		_viewMenu.NeedsUpdate -= ViewMenu_NeedsUpdate;
		_viewMenu.NeedsUpdate += ViewMenu_NeedsUpdate;
		if (_layoutMenu != null)
		{
			_layoutMenu.NeedsUpdate -= ViewMenu_NeedsUpdate;
			_layoutMenu.NeedsUpdate += ViewMenu_NeedsUpdate;
		}
		if (_sortByMenu != null)
		{
			_sortByMenu.NeedsUpdate -= ViewMenu_NeedsUpdate;
			_sortByMenu.NeedsUpdate += ViewMenu_NeedsUpdate;
		}
		RefreshViewMenuCheckedStates();
		RemoveInjectedViewMenuItems();
	}

	private void ViewMenu_NeedsUpdate(object? sender, EventArgs e)
	{
		RefreshCommandMenuItemStates();
		RefreshViewMenuCheckedStates();
		RemoveInjectedViewMenuItems();
	}

	private static void RemoveInjectedViewMenuItems()
	{
		MacPluginInterop.RemoveNativeMenuItemsAfter(Files.Assets.Lang.Resources.View, Files.Assets.Lang.Resources.Refresh);
	}

	private void InitializeGoNavigationMenuItems()
	{
		NativeMenu nativeMenu = _goMenu ?? (_goMenu = TryGetGoMenu());
		if (nativeMenu != null && nativeMenu.Items.Count >= 12)
		{
			_navigateBackMenuItem = nativeMenu.Items[0] as NativeMenuItem;
			_navigateForwardMenuItem = nativeMenu.Items[1] as NativeMenuItem;
			_navigateUpMenuItem = nativeMenu.Items[2] as NativeMenuItem;
			InitializeCommandMenuItem(_navigateBackMenuItem, CommandCodes.NavigateBack);
			InitializeCommandMenuItem(_navigateForwardMenuItem, CommandCodes.NavigateForward);
			InitializeCommandMenuItem(_navigateUpMenuItem, CommandCodes.NavigateUp);
			InitializeCommandMenuItem(nativeMenu.Items[4] as NativeMenuItem, CommandCodes.NavigateToApplications);
			InitializeCommandMenuItem(nativeMenu.Items[5] as NativeMenuItem, CommandCodes.NavigateToDesktop);
			InitializeCommandMenuItem(nativeMenu.Items[6] as NativeMenuItem, CommandCodes.NavigateToDocuments);
			InitializeCommandMenuItem(nativeMenu.Items[7] as NativeMenuItem, CommandCodes.NavigateToDownloads);
			InitializeCommandMenuItem(nativeMenu.Items[8] as NativeMenuItem, CommandCodes.NavigateToMusic);
			InitializeCommandMenuItem(nativeMenu.Items[9] as NativeMenuItem, CommandCodes.NavigateToMovies);
			InitializeCommandMenuItem(nativeMenu.Items[10] as NativeMenuItem, CommandCodes.NavigateToPictures);
			InitializeCommandMenuItem(nativeMenu.Items[11] as NativeMenuItem, CommandCodes.NavigateHome);
		}
	}

	private void InitializeTabsMenuItems(NativeMenu mainMenu)
	{
		if (mainMenu.Items.Count >= 5 && mainMenu.Items[4] is NativeMenuItem nativeMenuItem)
		{
			NativeMenu menu = nativeMenuItem.Menu;
			if (menu != null && menu.Items.Count >= 11)
			{
				InitializeCommandMenuItem(menu.Items[0] as NativeMenuItem, CommandCodes.NewTab);
				InitializeCommandMenuItem(menu.Items[1] as NativeMenuItem, CommandCodes.DuplicateSelectedTab);
				InitializeCommandMenuItem(menu.Items[2] as NativeMenuItem, CommandCodes.ReopenClosedTab);
				InitializeCommandMenuItem(menu.Items[4] as NativeMenuItem, CommandCodes.PreviousTab);
				InitializeCommandMenuItem(menu.Items[5] as NativeMenuItem, CommandCodes.NextTab);
				InitializeCommandMenuItem(menu.Items[7] as NativeMenuItem, CommandCodes.CloseSelectedTab);
				InitializeCommandMenuItem(menu.Items[8] as NativeMenuItem, CommandCodes.CloseOtherTabsSelected);
				InitializeCommandMenuItem(menu.Items[9] as NativeMenuItem, CommandCodes.CloseTabsToTheLeftSelected);
				InitializeCommandMenuItem(menu.Items[10] as NativeMenuItem, CommandCodes.CloseTabsToTheRightSelected);
			}
		}
	}

	private void InitializeToolsMenuItems(NativeMenu mainMenu)
	{
		if (mainMenu.Items.Count >= 6 && mainMenu.Items[5] is NativeMenuItem nativeMenuItem)
		{
			NativeMenu menu = nativeMenuItem.Menu;
			if (menu != null && menu.Items.Count >= 14)
			{
				InitializeCommandMenuItem(menu.Items[0] as NativeMenuItem, CommandCodes.CompressIntoTar);
				InitializeCommandMenuItem(menu.Items[1] as NativeMenuItem, CommandCodes.CompressIntoZip);
				InitializeCommandMenuItem(menu.Items[3] as NativeMenuItem, CommandCodes.ConvertToPng);
				InitializeCommandMenuItem(menu.Items[4] as NativeMenuItem, CommandCodes.ConvertToJpeg);
				InitializeCommandMenuItem(menu.Items[5] as NativeMenuItem, CommandCodes.ConvertToBmp);
				InitializeCommandMenuItem(menu.Items[7] as NativeMenuItem, CommandCodes.OpenHashes);
				InitializeCommandMenuItem(menu.Items[9] as NativeMenuItem, CommandCodes.OpenFinder);
				InitializeCommandMenuItem(menu.Items[10] as NativeMenuItem, CommandCodes.OpenTerminal);
				InitializeCommandMenuItem(menu.Items[12] as NativeMenuItem, CommandCodes.OpenSettingsFile);
				InitializeCommandMenuItem(menu.Items[13] as NativeMenuItem, CommandCodes.OpenAppSupportFolder);
			}
		}
	}

	private void InitializeCommandMenuItem(NativeMenuItem? menuItem, CommandCodes commandCode)
	{
		if (menuItem != null)
		{
			menuItem.Gesture = GetCommandGesture(commandCode);
			UpdateCommandMenuItemState(menuItem, commandCode);
			EventHandler eventHandler = (object? _, EventArgs _) =>
			{
				OnExcuteCommand(commandCode);
			};
			EventHandler eventHandler2 = (object? _, EventArgs _) =>
			{
				UpdateCommandMenuItemState(menuItem, commandCode);
			};
			menuItem.Click += eventHandler;
			Commands[commandCode].CanExecuteChanged += eventHandler2;
			_commandMenuItemSubscriptions.Add((commandCode, menuItem, eventHandler, eventHandler2));
		}
	}

	private static void ConfigureToggleMenuItem(NativeMenuItem? menuItem)
	{
		if (menuItem != null)
		{
			menuItem.ToggleType = MenuItemToggleType.CheckBox;
		}
	}

	private void RefreshViewMenuCheckedStates()
	{
		SetMenuItemChecked(_toggleSidebarMenuItem, Commands[CommandCodes.ToggleSidebar].IsOn);
		SetMenuItemChecked(_layoutDetailsMenuItem, Commands[CommandCodes.LayoutDetails].IsOn);
		SetMenuItemChecked(_layoutGridMenuItem, Commands[CommandCodes.LayoutGrid].IsOn);
		SetMenuItemChecked(_showHiddenItemsMenuItem, Commands[CommandCodes.ToggleShowHiddenItems].IsOn);
		SetMenuItemChecked(_showFileExtensionsMenuItem, Commands[CommandCodes.ToggleShowFileExtensions].IsOn);
		BaseLayoutViewModel? activeLayoutViewModel = GetActiveLayoutViewModel();
		SortOption sortOption = activeLayoutViewModel?.CurrentSortOption ?? SortOption.Name;
		SortDirection sortDirection = activeLayoutViewModel?.CurrentSortDirection ?? SortDirection.Ascending;
		SetMenuItemChecked(_sortByNameMenuItem, sortOption == SortOption.Name);
		SetMenuItemChecked(_sortByDateModifiedMenuItem, sortOption == SortOption.DateModified);
		SetMenuItemChecked(_sortByDateCreatedMenuItem, sortOption == SortOption.DateCreated);
		SetMenuItemChecked(_sortBySizeMenuItem, sortOption == SortOption.Size);
		SetMenuItemChecked(_sortByTypeMenuItem, sortOption == SortOption.FileType);
		SetMenuItemChecked(_sortAscendingMenuItem, sortDirection == SortDirection.Ascending);
		SetMenuItemChecked(_sortDescendingMenuItem, sortDirection == SortDirection.Descending);
	}

	private static void SetMenuItemChecked(NativeMenuItem? menuItem, bool isChecked)
	{
		if (menuItem != null)
		{
			menuItem.IsChecked = isChecked;
		}
	}

	private void InitializeSortMenuItem(NativeMenuItem? menuItem, SortOption sortOption)
	{
		InitializeMenuItemClick(menuItem, (object? _, EventArgs _) =>
		{
			ExecuteSortCommand(sortOption);
		});
	}

	private void InitializeSortDirectionMenuItem(NativeMenuItem? menuItem, SortDirection sortDirection)
	{
		InitializeMenuItemClick(menuItem, (object? _, EventArgs _) =>
		{
			ExecuteSortDirectionCommand(sortDirection);
		});
	}

	private void InitializeMenuItemClick(NativeMenuItem? menuItem, EventHandler clickHandler)
	{
		if (menuItem != null)
		{
			menuItem.Click += clickHandler;
			_menuItemClickSubscriptions.Add((menuItem, clickHandler));
		}
	}

	private void ExecuteSortCommand(SortOption sortOption)
	{
		BaseLayoutViewModel activeLayoutViewModel = GetActiveLayoutViewModel();
		if (activeLayoutViewModel != null && activeLayoutViewModel.SortItemsCommand.CanExecute(sortOption))
		{
			activeLayoutViewModel.SortItemsCommand.Execute(sortOption);
			RefreshViewMenuCheckedStates();
		}
	}

	private void ExecuteSortDirectionCommand(SortDirection sortDirection)
	{
		BaseLayoutViewModel activeLayoutViewModel = GetActiveLayoutViewModel();
		if (activeLayoutViewModel != null && activeLayoutViewModel.SortDirectionCommand.CanExecute(sortDirection))
		{
			activeLayoutViewModel.SortDirectionCommand.Execute(sortDirection);
			RefreshViewMenuCheckedStates();
		}
	}

	private static BaseLayoutViewModel? GetActiveLayoutViewModel()
	{
		return Ioc.Default.GetRequiredService<IMultitaskingContext>().SelectedTabViewModel?.LayoutViewModel;
	}

	private void UpdateCommandMenuItemState(NativeMenuItem menuItem, CommandCodes commandCode)
	{
		IRichCommand richCommand = Commands[commandCode];
		menuItem.Header = richCommand.Label;
		menuItem.IsEnabled = richCommand.IsExecutable;
	}

	private void RefreshCommandMenuItemStates()
	{
		foreach (var (commandCode, menuItem, _, _) in _commandMenuItemSubscriptions)
		{
			UpdateCommandMenuItemState(menuItem, commandCode);
		}
	}

	private void UnsubscribeCommandMenuItems()
	{
		if (Design.IsDesignMode)
		{
			return;
		}
		foreach (var (code, nativeMenuItem, value, value2) in _commandMenuItemSubscriptions)
		{
			nativeMenuItem.Click -= value;
			Commands[code].CanExecuteChanged -= value2;
		}
		_commandMenuItemSubscriptions.Clear();
		foreach (var (nativeMenuItem2, value3) in _menuItemClickSubscriptions)
		{
			nativeMenuItem2.Click -= value3;
		}
		_menuItemClickSubscriptions.Clear();
		if (_editMenu != null)
		{
			_editMenu.NeedsUpdate -= EditMenu_NeedsUpdate;
			_editMenu = null;
		}
		if (_viewMenu != null)
		{
			_viewMenu.NeedsUpdate -= ViewMenu_NeedsUpdate;
			_viewMenu = null;
		}
		if (_layoutMenu != null)
		{
			_layoutMenu.NeedsUpdate -= ViewMenu_NeedsUpdate;
			_layoutMenu = null;
		}
		if (_sortByMenu != null)
		{
			_sortByMenu.NeedsUpdate -= ViewMenu_NeedsUpdate;
			_sortByMenu = null;
		}
	}

	private NativeMenu? TryGetGoMenu()
	{
		NativeMenu menu = NativeMenu.GetMenu(this);
		if (menu != null && menu.Items.Count >= 4 && menu.Items[3] is NativeMenuItem nativeMenuItem)
		{
			NativeMenu menu2 = nativeMenuItem.Menu;
			if (menu2 != null)
			{
				return menu2;
			}
		}
		return null;
	}

	private void RefreshGoMenuQuickAccessItems()
	{
		NativeMenu nativeMenu = _goMenu ?? (_goMenu = TryGetGoMenu());
		if (nativeMenu == null || _quickAccessService == null)
		{
			return;
		}
		int goMenuDynamicDividerIndex = GetGoMenuDynamicDividerIndex(nativeMenu);
		if (goMenuDynamicDividerIndex < 0)
		{
			return;
		}
		while (nativeMenu.Items.Count > goMenuDynamicDividerIndex + 1)
		{
			nativeMenu.Items.RemoveAt(nativeMenu.Items.Count - 1);
		}
		foreach (string item in _quickAccessService.LoadCustomQuickAccessPaths())
		{
			string targetPath = item;
			NativeMenuItem nativeMenuItem = new NativeMenuItem(GetGoCustomLocationLabel(targetPath));
			nativeMenuItem.Click += (object? _, EventArgs _) =>
			{
				OpenGoLocationAsync(targetPath);
			};
			nativeMenu.Items.Add(nativeMenuItem);
		}
	}

	private static int GetGoMenuDynamicDividerIndex(NativeMenu goMenu)
	{
		for (int num = goMenu.Items.Count - 1; num >= 0; num--)
		{
			if (goMenu.Items[num] is NativeMenuItemSeparator)
			{
				return num;
			}
		}
		return -1;
	}

	private static string GetGoCustomLocationLabel(string path)
	{
		string text = PathHelpers.NormalizePath(path);
		if (string.IsNullOrWhiteSpace(text))
		{
			return path;
		}
		string text2 = ((text.Length > 1) ? text.TrimEnd('/') : text);
		if (text2 == "/")
		{
			return text2;
		}
		string fileName = Path.GetFileName(text2);
		if (!string.IsNullOrWhiteSpace(fileName))
		{
			return fileName;
		}
		return text2;
	}

	public void Receive(QuickAccessChangedMessage message)
	{
		Avalonia.Threading.Dispatcher.UIThread.Post(RefreshGoMenuQuickAccessItems);
	}

	private void OnCreateFolderMenuClick(object? sender, EventArgs e)
	{
		OnExcuteCommand(CommandCodes.CreateFolder);
	}

	private async void OnExcuteCommand(CommandCodes commandCode)
	{
		IRichCommand richCommand = Commands[commandCode];
		if (richCommand.IsExecutable)
		{
			await richCommand.ExecuteAsync();
			RefreshCommandMenuItemStates();
			RefreshViewMenuCheckedStates();
		}
	}

	private void OpenUrl(string url)
	{
		try
		{
			Process.Start(new ProcessStartInfo
			{
				FileName = url,
				UseShellExecute = true
			});
		}
		catch (Exception ex)
		{
			_logger?.LogError(ex, "Failed to open {url}: {ex.Message}", url, ex.Message);
		}
	}

	private void OnSubmitFeedbackClick(object? sender, EventArgs e)
	{
		OpenUrl("https://github.com/zebrapixel/XFolder/issues/new");
	}

	private void OnKeyboardShortcutsMenuClick(object? sender, EventArgs e)
	{
		try
		{
			string text = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", "KeyboardShortcuts.html");
			if (!File.Exists(text) && OperatingSystem.IsMacOS())
			{
				string text2 = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "Resources", "KeyboardShortcuts.html");
				if (File.Exists(text2))
				{
					text = text2;
				}
			}
			if (File.Exists(text))
			{
				Process.Start(new ProcessStartInfo
				{
					FileName = text,
					UseShellExecute = true
				});
			}
			else
			{
				_logger?.LogError("Keyboard shortcuts file not found: {htmlPath}", text);
			}
		}
		catch (Exception ex)
		{
			_logger?.LogError(ex, "Failed to open keyboard shortcuts: {ex.Message}", ex.Message);
		}
	}

	private static async Task OpenGoLocationAsync(string? path)
	{
		if (!string.IsNullOrWhiteSpace(path) && Ioc.Default.GetRequiredService<IMultiPanesContext>().ActivePaneViewModel != null)
		{
			await NavigationHelpers.OpenPath(path);
		}
	}

	public void OpenWorkspaceFromFile(string? filePath)
	{
		if (!string.IsNullOrWhiteSpace(filePath))
		{
			IRichCommand richCommand = Commands[CommandCodes.OpenWorkspaceFromFile];
			if (richCommand.IsExecutable)
			{
				richCommand.ExecuteAsync(filePath);
			}
		}
	}

	private void ResetHotKeyState()
	{
		keyReleased = true;
	}

	private async void MainWindow_KeyDown(object? sender, KeyEventArgs e)
	{
		if (DialogHost.IsDialogOpen("MainDialogHost") || IsEditableTextInputSource(e.Source))
		{
			return;
		}
		Key key = e.Key;
		if ((uint)(key - 70) <= 1u || (uint)(key - 116) <= 5u)
		{
			return;
		}
		HotKey hotKey = new HotKey(e.Key, e.KeyModifiers);
		IRichCommand command = Commands[hotKey];
		if (command.Code != CommandCodes.None && keyReleased)
		{
			keyReleased = false;
			e.Handled = command.IsExecutable;
			try
			{
				await command.ExecuteAsync();
			}
			catch (Exception exception)
			{
				_logger?.LogError(exception, "Failed to execute hotkey command {CommandCode} for {HotKey}", command.Code, hotKey.RawLabel);
			}
		}
	}

	private static bool IsEditableTextInputSource(object? source)
	{
		for (Visual visual = source as Visual; visual != null; visual = visual.GetVisualParent())
		{
			if (visual is TextBox)
			{
				return true;
			}
		}
		return false;
	}

	private void MainWindow_KeyUp(object? sender, KeyEventArgs e)
	{
		ResetHotKeyState();
	}

	private void OnTogglePaneClick(object sender, RoutedEventArgs e)
	{
		OnExcuteCommand(CommandCodes.ToggleSidebar);
	}

	private async void OnRunActionCommand(object? sender, RoutedEventArgs e)
	{
		if (sender is Control { Tag: string tag })
		{
			IRichCommand richCommand = Commands[tag];
			if (richCommand.IsExecutable)
			{
				await richCommand.ExecuteAsync();
			}
		}
	}

	private void OnTestButtonClick(object? sender, RoutedEventArgs e)
	{
		ListedItem listedItem = (Ioc.Default.GetRequiredService<IContentPageContext>().ShellView?.InstanceViewModel)?.CurrentFolder;
		if (listedItem != null)
		{
			_logger?.LogInformation("test:{Name}", listedItem.Name);
		}
	}

	[GeneratedCode("Avalonia.Generators.NameGenerator.InitializeComponentCodeGenerator", "12.0.1.0")]
	[ExcludeFromCodeCoverage]
	public void InitializeComponent(bool loadXaml = true)
	{
		if (loadXaml)
		{
			_0021XamlIlPopulateTrampoline(this);
		}
		INameScope nameScope = this.FindNameScope();
		TopToolbarGrid = ((nameScope != null) ? nameScope.Find<Grid>("TopToolbarGrid") : null);
	}

	[CompilerGenerated]
	private unsafe static void _0021XamlIlPopulate(IServiceProvider P_0, MainWindow P_1)
	{
		XamlIlContext.Context<MainWindow> context = new XamlIlContext.Context<MainWindow>(P_0, new object[1] { _0021AvaloniaResources.NamespaceInfo_003A_002FViews_002FMainWindow_002Eaxaml.Singleton }, "avares://XFolder/Views/MainWindow.axaml")
		{
			RootObject = P_1,
			IntermediateRoot = P_1
		};
		((ISupportInitialize)P_1).BeginInit();
		context.PushParent(P_1);
		P_1.Icon = (WindowIcon)new IconTypeConverter().ConvertFrom(context, CultureInfo.InvariantCulture, "/Assets/avalonia-logo.ico");
		P_1.TransparencyLevelHint = new WindowTransparencyLevel[1] { WindowTransparencyLevel.AcrylicBlur };
		P_1.Background = new ImmutableSolidColorBrush(16777215u);
		P_1.WindowDecorations = WindowDecorations.Full;
		P_1.ExtendClientAreaToDecorationsHint = true;
		P_1.ExtendClientAreaTitleBarHeightHint = 44.0;
		P_1.CanResize = true;
		P_1.Width = 1200.0;
		P_1.Height = 800.0;
		P_1.MinWidth = 600.0;
		P_1.MinHeight = 400.0;
		P_1.Title = "";
		NativeMenu nativeMenu;
		NativeMenu menu = (nativeMenu = new NativeMenu());
		context.PushParent(nativeMenu);
		NativeMenu nativeMenu2 = nativeMenu;
		IList<NativeMenuItemBase> items = nativeMenu2.Items;
		NativeMenuItem nativeMenuItem;
		NativeMenuItem item = (nativeMenuItem = new NativeMenuItem());
		context.PushParent(nativeMenuItem);
		NativeMenuItem nativeMenuItem2 = nativeMenuItem;
		StyledProperty<string?> headerProperty = NativeMenuItem.HeaderProperty;
		CompiledBindingExtension compiledBindingExtension = new CompiledBindingExtension(new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EServices_002EApp_002ELocalizationBindingSource_002CXFolder_002EItem_005BFile_005D_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build())
		{
			Source = LocalizationBindingSource.Instance
		};
		context.ProvideTargetProperty = NativeMenuItem.HeaderProperty;
		CompiledBinding binding = compiledBindingExtension.ProvideValue(context);
		context.ProvideTargetProperty = null;
		nativeMenuItem2.Bind(headerProperty, binding);
		NativeMenu menu2 = (nativeMenu = new NativeMenu());
		context.PushParent(nativeMenu);
		NativeMenu nativeMenu3 = nativeMenu;
		IList<NativeMenuItemBase> items2 = nativeMenu3.Items;
		NativeMenuItem item2 = (nativeMenuItem = new NativeMenuItem());
		context.PushParent(nativeMenuItem);
		NativeMenuItem nativeMenuItem3 = nativeMenuItem;
		StyledProperty<string?> headerProperty2 = NativeMenuItem.HeaderProperty;
		CompiledBindingExtension compiledBindingExtension2 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EServices_002EApp_002ELocalizationBindingSource_002CXFolder_002EItem_005BBaseLayoutContextFlyoutNew_002ELabel_005D_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build())
		{
			Source = LocalizationBindingSource.Instance
		};
		context.ProvideTargetProperty = NativeMenuItem.HeaderProperty;
		CompiledBinding binding2 = compiledBindingExtension2.ProvideValue(context);
		context.ProvideTargetProperty = null;
		nativeMenuItem3.Bind(headerProperty2, binding2);
		NativeMenu menu3 = (nativeMenu = new NativeMenu());
		context.PushParent(nativeMenu);
		NativeMenu nativeMenu4 = nativeMenu;
		IList<NativeMenuItemBase> items3 = nativeMenu4.Items;
		NativeMenuItem item3 = (nativeMenuItem = new NativeMenuItem());
		context.PushParent(nativeMenuItem);
		NativeMenuItem nativeMenuItem4 = nativeMenuItem;
		StyledProperty<string?> headerProperty3 = NativeMenuItem.HeaderProperty;
		CompiledBindingExtension compiledBindingExtension3 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EServices_002EApp_002ELocalizationBindingSource_002CXFolder_002EItem_005BFile_005D_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build())
		{
			Source = LocalizationBindingSource.Instance
		};
		context.ProvideTargetProperty = NativeMenuItem.HeaderProperty;
		CompiledBinding binding3 = compiledBindingExtension3.ProvideValue(context);
		context.ProvideTargetProperty = null;
		nativeMenuItem4.Bind(headerProperty3, binding3);
		context.PopParent();
		items3.Add(item3);
		IList<NativeMenuItemBase> items4 = nativeMenu4.Items;
		NativeMenuItem item4 = (nativeMenuItem = new NativeMenuItem());
		context.PushParent(nativeMenuItem);
		NativeMenuItem nativeMenuItem5 = nativeMenuItem;
		StyledProperty<string?> headerProperty4 = NativeMenuItem.HeaderProperty;
		CompiledBindingExtension compiledBindingExtension4 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EServices_002EApp_002ELocalizationBindingSource_002CXFolder_002EItem_005BFolder_005D_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build())
		{
			Source = LocalizationBindingSource.Instance
		};
		context.ProvideTargetProperty = NativeMenuItem.HeaderProperty;
		CompiledBinding binding4 = compiledBindingExtension4.ProvideValue(context);
		context.ProvideTargetProperty = null;
		nativeMenuItem5.Bind(headerProperty4, binding4);
		context.PopParent();
		items4.Add(item4);
		IList<NativeMenuItemBase> items5 = nativeMenu4.Items;
		NativeMenuItem item5 = (nativeMenuItem = new NativeMenuItem());
		context.PushParent(nativeMenuItem);
		NativeMenuItem nativeMenuItem6 = nativeMenuItem;
		StyledProperty<string?> headerProperty5 = NativeMenuItem.HeaderProperty;
		CompiledBindingExtension compiledBindingExtension5 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EServices_002EApp_002ELocalizationBindingSource_002CXFolder_002EItem_005BCreateFolderWithSelection_005D_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build())
		{
			Source = LocalizationBindingSource.Instance
		};
		context.ProvideTargetProperty = NativeMenuItem.HeaderProperty;
		CompiledBinding binding5 = compiledBindingExtension5.ProvideValue(context);
		context.ProvideTargetProperty = null;
		nativeMenuItem6.Bind(headerProperty5, binding5);
		context.PopParent();
		items5.Add(item5);
		context.PopParent();
		nativeMenuItem3.Menu = menu3;
		context.PopParent();
		items2.Add(item2);
		IList<NativeMenuItemBase> items6 = nativeMenu3.Items;
		NativeMenuItem item6 = (nativeMenuItem = new NativeMenuItem());
		context.PushParent(nativeMenuItem);
		NativeMenuItem nativeMenuItem7 = nativeMenuItem;
		StyledProperty<string?> headerProperty6 = NativeMenuItem.HeaderProperty;
		CompiledBindingExtension compiledBindingExtension6 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EServices_002EApp_002ELocalizationBindingSource_002CXFolder_002EItem_005BShowInfoInFinder_005D_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build())
		{
			Source = LocalizationBindingSource.Instance
		};
		context.ProvideTargetProperty = NativeMenuItem.HeaderProperty;
		CompiledBinding binding6 = compiledBindingExtension6.ProvideValue(context);
		context.ProvideTargetProperty = null;
		nativeMenuItem7.Bind(headerProperty6, binding6);
		context.PopParent();
		items6.Add(item6);
		nativeMenu3.Items.Add(new NativeMenuItemSeparator());
		IList<NativeMenuItemBase> items7 = nativeMenu3.Items;
		NativeMenuItem item7 = (nativeMenuItem = new NativeMenuItem());
		context.PushParent(nativeMenuItem);
		NativeMenuItem nativeMenuItem8 = nativeMenuItem;
		StyledProperty<string?> headerProperty7 = NativeMenuItem.HeaderProperty;
		CompiledBindingExtension compiledBindingExtension7 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EServices_002EApp_002ELocalizationBindingSource_002CXFolder_002EItem_005BOpen_005D_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build())
		{
			Source = LocalizationBindingSource.Instance
		};
		context.ProvideTargetProperty = NativeMenuItem.HeaderProperty;
		CompiledBinding binding7 = compiledBindingExtension7.ProvideValue(context);
		context.ProvideTargetProperty = null;
		nativeMenuItem8.Bind(headerProperty7, binding7);
		context.PopParent();
		items7.Add(item7);
		IList<NativeMenuItemBase> items8 = nativeMenu3.Items;
		NativeMenuItem item8 = (nativeMenuItem = new NativeMenuItem());
		context.PushParent(nativeMenuItem);
		NativeMenuItem nativeMenuItem9 = nativeMenuItem;
		StyledProperty<string?> headerProperty8 = NativeMenuItem.HeaderProperty;
		CompiledBindingExtension compiledBindingExtension8 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EServices_002EApp_002ELocalizationBindingSource_002CXFolder_002EItem_005BOpenInNewTab_005D_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build())
		{
			Source = LocalizationBindingSource.Instance
		};
		context.ProvideTargetProperty = NativeMenuItem.HeaderProperty;
		CompiledBinding binding8 = compiledBindingExtension8.ProvideValue(context);
		context.ProvideTargetProperty = null;
		nativeMenuItem9.Bind(headerProperty8, binding8);
		context.PopParent();
		items8.Add(item8);
		nativeMenu3.Items.Add(new NativeMenuItemSeparator());
		IList<NativeMenuItemBase> items9 = nativeMenu3.Items;
		NativeMenuItem item9 = (nativeMenuItem = new NativeMenuItem());
		context.PushParent(nativeMenuItem);
		NativeMenuItem nativeMenuItem10 = nativeMenuItem;
		StyledProperty<string?> headerProperty9 = NativeMenuItem.HeaderProperty;
		CompiledBindingExtension compiledBindingExtension9 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EServices_002EApp_002ELocalizationBindingSource_002CXFolder_002EItem_005BOpenWorkspaceFromFile_005D_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build())
		{
			Source = LocalizationBindingSource.Instance
		};
		context.ProvideTargetProperty = NativeMenuItem.HeaderProperty;
		CompiledBinding binding9 = compiledBindingExtension9.ProvideValue(context);
		context.ProvideTargetProperty = null;
		nativeMenuItem10.Bind(headerProperty9, binding9);
		context.PopParent();
		items9.Add(item9);
		IList<NativeMenuItemBase> items10 = nativeMenu3.Items;
		NativeMenuItem item10 = (nativeMenuItem = new NativeMenuItem());
		context.PushParent(nativeMenuItem);
		NativeMenuItem nativeMenuItem11 = nativeMenuItem;
		StyledProperty<string?> headerProperty10 = NativeMenuItem.HeaderProperty;
		CompiledBindingExtension compiledBindingExtension10 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EServices_002EApp_002ELocalizationBindingSource_002CXFolder_002EItem_005BSaveWorkspaceAs_005D_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build())
		{
			Source = LocalizationBindingSource.Instance
		};
		context.ProvideTargetProperty = NativeMenuItem.HeaderProperty;
		CompiledBinding binding10 = compiledBindingExtension10.ProvideValue(context);
		context.ProvideTargetProperty = null;
		nativeMenuItem11.Bind(headerProperty10, binding10);
		context.PopParent();
		items10.Add(item10);
		context.PopParent();
		nativeMenuItem2.Menu = menu2;
		context.PopParent();
		items.Add(item);
		IList<NativeMenuItemBase> items11 = nativeMenu2.Items;
		NativeMenuItem item11 = (nativeMenuItem = new NativeMenuItem());
		context.PushParent(nativeMenuItem);
		NativeMenuItem nativeMenuItem12 = nativeMenuItem;
		StyledProperty<string?> headerProperty11 = NativeMenuItem.HeaderProperty;
		CompiledBindingExtension compiledBindingExtension11 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EServices_002EApp_002ELocalizationBindingSource_002CXFolder_002EItem_005BEdit_005D_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build())
		{
			Source = LocalizationBindingSource.Instance
		};
		context.ProvideTargetProperty = NativeMenuItem.HeaderProperty;
		CompiledBinding binding11 = compiledBindingExtension11.ProvideValue(context);
		context.ProvideTargetProperty = null;
		nativeMenuItem12.Bind(headerProperty11, binding11);
		NativeMenu menu4 = (nativeMenu = new NativeMenu());
		context.PushParent(nativeMenu);
		NativeMenu nativeMenu5 = nativeMenu;
		IList<NativeMenuItemBase> items12 = nativeMenu5.Items;
		NativeMenuItem item12 = (nativeMenuItem = new NativeMenuItem());
		context.PushParent(nativeMenuItem);
		NativeMenuItem nativeMenuItem13 = nativeMenuItem;
		StyledProperty<string?> headerProperty12 = NativeMenuItem.HeaderProperty;
		CompiledBindingExtension compiledBindingExtension12 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EServices_002EApp_002ELocalizationBindingSource_002CXFolder_002EItem_005BUndo_005D_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build())
		{
			Source = LocalizationBindingSource.Instance
		};
		context.ProvideTargetProperty = NativeMenuItem.HeaderProperty;
		CompiledBinding binding12 = compiledBindingExtension12.ProvideValue(context);
		context.ProvideTargetProperty = null;
		nativeMenuItem13.Bind(headerProperty12, binding12);
		context.PopParent();
		items12.Add(item12);
		IList<NativeMenuItemBase> items13 = nativeMenu5.Items;
		NativeMenuItem item13 = (nativeMenuItem = new NativeMenuItem());
		context.PushParent(nativeMenuItem);
		NativeMenuItem nativeMenuItem14 = nativeMenuItem;
		StyledProperty<string?> headerProperty13 = NativeMenuItem.HeaderProperty;
		CompiledBindingExtension compiledBindingExtension13 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EServices_002EApp_002ELocalizationBindingSource_002CXFolder_002EItem_005BRedo_005D_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build())
		{
			Source = LocalizationBindingSource.Instance
		};
		context.ProvideTargetProperty = NativeMenuItem.HeaderProperty;
		CompiledBinding binding13 = compiledBindingExtension13.ProvideValue(context);
		context.ProvideTargetProperty = null;
		nativeMenuItem14.Bind(headerProperty13, binding13);
		context.PopParent();
		items13.Add(item13);
		nativeMenu5.Items.Add(new NativeMenuItemSeparator());
		IList<NativeMenuItemBase> items14 = nativeMenu5.Items;
		NativeMenuItem item14 = (nativeMenuItem = new NativeMenuItem());
		context.PushParent(nativeMenuItem);
		NativeMenuItem nativeMenuItem15 = nativeMenuItem;
		StyledProperty<string?> headerProperty14 = NativeMenuItem.HeaderProperty;
		CompiledBindingExtension compiledBindingExtension14 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EServices_002EApp_002ELocalizationBindingSource_002CXFolder_002EItem_005BCut_005D_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build())
		{
			Source = LocalizationBindingSource.Instance
		};
		context.ProvideTargetProperty = NativeMenuItem.HeaderProperty;
		CompiledBinding binding14 = compiledBindingExtension14.ProvideValue(context);
		context.ProvideTargetProperty = null;
		nativeMenuItem15.Bind(headerProperty14, binding14);
		context.PopParent();
		items14.Add(item14);
		IList<NativeMenuItemBase> items15 = nativeMenu5.Items;
		NativeMenuItem item15 = (nativeMenuItem = new NativeMenuItem());
		context.PushParent(nativeMenuItem);
		NativeMenuItem nativeMenuItem16 = nativeMenuItem;
		StyledProperty<string?> headerProperty15 = NativeMenuItem.HeaderProperty;
		CompiledBindingExtension compiledBindingExtension15 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EServices_002EApp_002ELocalizationBindingSource_002CXFolder_002EItem_005BCopy_005D_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build())
		{
			Source = LocalizationBindingSource.Instance
		};
		context.ProvideTargetProperty = NativeMenuItem.HeaderProperty;
		CompiledBinding binding15 = compiledBindingExtension15.ProvideValue(context);
		context.ProvideTargetProperty = null;
		nativeMenuItem16.Bind(headerProperty15, binding15);
		context.PopParent();
		items15.Add(item15);
		IList<NativeMenuItemBase> items16 = nativeMenu5.Items;
		NativeMenuItem item16 = (nativeMenuItem = new NativeMenuItem());
		context.PushParent(nativeMenuItem);
		NativeMenuItem nativeMenuItem17 = nativeMenuItem;
		StyledProperty<string?> headerProperty16 = NativeMenuItem.HeaderProperty;
		CompiledBindingExtension compiledBindingExtension16 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EServices_002EApp_002ELocalizationBindingSource_002CXFolder_002EItem_005BPaste_005D_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build())
		{
			Source = LocalizationBindingSource.Instance
		};
		context.ProvideTargetProperty = NativeMenuItem.HeaderProperty;
		CompiledBinding binding16 = compiledBindingExtension16.ProvideValue(context);
		context.ProvideTargetProperty = null;
		nativeMenuItem17.Bind(headerProperty16, binding16);
		context.PopParent();
		items16.Add(item16);
		IList<NativeMenuItemBase> items17 = nativeMenu5.Items;
		NativeMenuItem item17 = (nativeMenuItem = new NativeMenuItem());
		context.PushParent(nativeMenuItem);
		NativeMenuItem nativeMenuItem18 = nativeMenuItem;
		StyledProperty<string?> headerProperty17 = NativeMenuItem.HeaderProperty;
		CompiledBindingExtension compiledBindingExtension17 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EServices_002EApp_002ELocalizationBindingSource_002CXFolder_002EItem_005BRename_005D_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build())
		{
			Source = LocalizationBindingSource.Instance
		};
		context.ProvideTargetProperty = NativeMenuItem.HeaderProperty;
		CompiledBinding binding17 = compiledBindingExtension17.ProvideValue(context);
		context.ProvideTargetProperty = null;
		nativeMenuItem18.Bind(headerProperty17, binding17);
		context.PopParent();
		items17.Add(item17);
		IList<NativeMenuItemBase> items18 = nativeMenu5.Items;
		NativeMenuItem item18 = (nativeMenuItem = new NativeMenuItem());
		context.PushParent(nativeMenuItem);
		NativeMenuItem nativeMenuItem19 = nativeMenuItem;
		StyledProperty<string?> headerProperty18 = NativeMenuItem.HeaderProperty;
		CompiledBindingExtension compiledBindingExtension18 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EServices_002EApp_002ELocalizationBindingSource_002CXFolder_002EItem_005BDelete_005D_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build())
		{
			Source = LocalizationBindingSource.Instance
		};
		context.ProvideTargetProperty = NativeMenuItem.HeaderProperty;
		CompiledBinding binding18 = compiledBindingExtension18.ProvideValue(context);
		context.ProvideTargetProperty = null;
		nativeMenuItem19.Bind(headerProperty18, binding18);
		context.PopParent();
		items18.Add(item18);
		IList<NativeMenuItemBase> items19 = nativeMenu5.Items;
		NativeMenuItem item19 = (nativeMenuItem = new NativeMenuItem());
		context.PushParent(nativeMenuItem);
		NativeMenuItem nativeMenuItem20 = nativeMenuItem;
		StyledProperty<string?> headerProperty19 = NativeMenuItem.HeaderProperty;
		CompiledBindingExtension compiledBindingExtension19 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EServices_002EApp_002ELocalizationBindingSource_002CXFolder_002EItem_005BDeletePermanently_005D_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build())
		{
			Source = LocalizationBindingSource.Instance
		};
		context.ProvideTargetProperty = NativeMenuItem.HeaderProperty;
		CompiledBinding binding19 = compiledBindingExtension19.ProvideValue(context);
		context.ProvideTargetProperty = null;
		nativeMenuItem20.Bind(headerProperty19, binding19);
		context.PopParent();
		items19.Add(item19);
		nativeMenu5.Items.Add(new NativeMenuItemSeparator());
		IList<NativeMenuItemBase> items20 = nativeMenu5.Items;
		NativeMenuItem item20 = (nativeMenuItem = new NativeMenuItem());
		context.PushParent(nativeMenuItem);
		NativeMenuItem nativeMenuItem21 = nativeMenuItem;
		StyledProperty<string?> headerProperty20 = NativeMenuItem.HeaderProperty;
		CompiledBindingExtension compiledBindingExtension20 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EServices_002EApp_002ELocalizationBindingSource_002CXFolder_002EItem_005BSelectAll_005D_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build())
		{
			Source = LocalizationBindingSource.Instance
		};
		context.ProvideTargetProperty = NativeMenuItem.HeaderProperty;
		CompiledBinding binding20 = compiledBindingExtension20.ProvideValue(context);
		context.ProvideTargetProperty = null;
		nativeMenuItem21.Bind(headerProperty20, binding20);
		context.PopParent();
		items20.Add(item20);
		IList<NativeMenuItemBase> items21 = nativeMenu5.Items;
		NativeMenuItem item21 = (nativeMenuItem = new NativeMenuItem());
		context.PushParent(nativeMenuItem);
		NativeMenuItem nativeMenuItem22 = nativeMenuItem;
		StyledProperty<string?> headerProperty21 = NativeMenuItem.HeaderProperty;
		CompiledBindingExtension compiledBindingExtension21 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EServices_002EApp_002ELocalizationBindingSource_002CXFolder_002EItem_005BInvertSelection_005D_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build())
		{
			Source = LocalizationBindingSource.Instance
		};
		context.ProvideTargetProperty = NativeMenuItem.HeaderProperty;
		CompiledBinding binding21 = compiledBindingExtension21.ProvideValue(context);
		context.ProvideTargetProperty = null;
		nativeMenuItem22.Bind(headerProperty21, binding21);
		context.PopParent();
		items21.Add(item21);
		IList<NativeMenuItemBase> items22 = nativeMenu5.Items;
		NativeMenuItem item22 = (nativeMenuItem = new NativeMenuItem());
		context.PushParent(nativeMenuItem);
		NativeMenuItem nativeMenuItem23 = nativeMenuItem;
		StyledProperty<string?> headerProperty22 = NativeMenuItem.HeaderProperty;
		CompiledBindingExtension compiledBindingExtension22 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EServices_002EApp_002ELocalizationBindingSource_002CXFolder_002EItem_005BClearSelection_005D_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build())
		{
			Source = LocalizationBindingSource.Instance
		};
		context.ProvideTargetProperty = NativeMenuItem.HeaderProperty;
		CompiledBinding binding22 = compiledBindingExtension22.ProvideValue(context);
		context.ProvideTargetProperty = null;
		nativeMenuItem23.Bind(headerProperty22, binding22);
		context.PopParent();
		items22.Add(item22);
		nativeMenu5.Items.Add(new NativeMenuItemSeparator());
		IList<NativeMenuItemBase> items23 = nativeMenu5.Items;
		NativeMenuItem item23 = (nativeMenuItem = new NativeMenuItem());
		context.PushParent(nativeMenuItem);
		NativeMenuItem nativeMenuItem24 = nativeMenuItem;
		StyledProperty<string?> headerProperty23 = NativeMenuItem.HeaderProperty;
		CompiledBindingExtension compiledBindingExtension23 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EServices_002EApp_002ELocalizationBindingSource_002CXFolder_002EItem_005BCopyPath_005D_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build())
		{
			Source = LocalizationBindingSource.Instance
		};
		context.ProvideTargetProperty = NativeMenuItem.HeaderProperty;
		CompiledBinding binding23 = compiledBindingExtension23.ProvideValue(context);
		context.ProvideTargetProperty = null;
		nativeMenuItem24.Bind(headerProperty23, binding23);
		context.PopParent();
		items23.Add(item23);
		IList<NativeMenuItemBase> items24 = nativeMenu5.Items;
		NativeMenuItem item24 = (nativeMenuItem = new NativeMenuItem());
		context.PushParent(nativeMenuItem);
		NativeMenuItem nativeMenuItem25 = nativeMenuItem;
		StyledProperty<string?> headerProperty24 = NativeMenuItem.HeaderProperty;
		CompiledBindingExtension compiledBindingExtension24 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EServices_002EApp_002ELocalizationBindingSource_002CXFolder_002EItem_005BCopyPathWithQuotes_005D_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build())
		{
			Source = LocalizationBindingSource.Instance
		};
		context.ProvideTargetProperty = NativeMenuItem.HeaderProperty;
		CompiledBinding binding24 = compiledBindingExtension24.ProvideValue(context);
		context.ProvideTargetProperty = null;
		nativeMenuItem25.Bind(headerProperty24, binding24);
		context.PopParent();
		items24.Add(item24);
		IList<NativeMenuItemBase> items25 = nativeMenu5.Items;
		NativeMenuItem item25 = (nativeMenuItem = new NativeMenuItem());
		context.PushParent(nativeMenuItem);
		NativeMenuItem nativeMenuItem26 = nativeMenuItem;
		StyledProperty<string?> headerProperty25 = NativeMenuItem.HeaderProperty;
		CompiledBindingExtension compiledBindingExtension25 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EServices_002EApp_002ELocalizationBindingSource_002CXFolder_002EItem_005BCopyItemPath_005D_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build())
		{
			Source = LocalizationBindingSource.Instance
		};
		context.ProvideTargetProperty = NativeMenuItem.HeaderProperty;
		CompiledBinding binding25 = compiledBindingExtension25.ProvideValue(context);
		context.ProvideTargetProperty = null;
		nativeMenuItem26.Bind(headerProperty25, binding25);
		context.PopParent();
		items25.Add(item25);
		IList<NativeMenuItemBase> items26 = nativeMenu5.Items;
		NativeMenuItem item26 = (nativeMenuItem = new NativeMenuItem());
		context.PushParent(nativeMenuItem);
		NativeMenuItem nativeMenuItem27 = nativeMenuItem;
		StyledProperty<string?> headerProperty26 = NativeMenuItem.HeaderProperty;
		CompiledBindingExtension compiledBindingExtension26 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EServices_002EApp_002ELocalizationBindingSource_002CXFolder_002EItem_005BCopyItemPathWithQuotes_005D_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build())
		{
			Source = LocalizationBindingSource.Instance
		};
		context.ProvideTargetProperty = NativeMenuItem.HeaderProperty;
		CompiledBinding binding26 = compiledBindingExtension26.ProvideValue(context);
		context.ProvideTargetProperty = null;
		nativeMenuItem27.Bind(headerProperty26, binding26);
		context.PopParent();
		items26.Add(item26);
		nativeMenu5.Items.Add(new NativeMenuItemSeparator());
		IList<NativeMenuItemBase> items27 = nativeMenu5.Items;
		NativeMenuItem item27 = (nativeMenuItem = new NativeMenuItem());
		context.PushParent(nativeMenuItem);
		NativeMenuItem nativeMenuItem28 = nativeMenuItem;
		StyledProperty<string?> headerProperty27 = NativeMenuItem.HeaderProperty;
		CompiledBindingExtension compiledBindingExtension27 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EServices_002EApp_002ELocalizationBindingSource_002CXFolder_002EItem_005BEditPath_005D_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build())
		{
			Source = LocalizationBindingSource.Instance
		};
		context.ProvideTargetProperty = NativeMenuItem.HeaderProperty;
		CompiledBinding binding27 = compiledBindingExtension27.ProvideValue(context);
		context.ProvideTargetProperty = null;
		nativeMenuItem28.Bind(headerProperty27, binding27);
		context.PopParent();
		items27.Add(item27);
		context.PopParent();
		nativeMenuItem12.Menu = menu4;
		context.PopParent();
		items11.Add(item11);
		IList<NativeMenuItemBase> items28 = nativeMenu2.Items;
		NativeMenuItem item28 = (nativeMenuItem = new NativeMenuItem());
		context.PushParent(nativeMenuItem);
		NativeMenuItem nativeMenuItem29 = nativeMenuItem;
		StyledProperty<string?> headerProperty28 = NativeMenuItem.HeaderProperty;
		CompiledBindingExtension compiledBindingExtension28 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EServices_002EApp_002ELocalizationBindingSource_002CXFolder_002EItem_005BView_005D_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build())
		{
			Source = LocalizationBindingSource.Instance
		};
		context.ProvideTargetProperty = NativeMenuItem.HeaderProperty;
		CompiledBinding binding28 = compiledBindingExtension28.ProvideValue(context);
		context.ProvideTargetProperty = null;
		nativeMenuItem29.Bind(headerProperty28, binding28);
		NativeMenu menu5 = (nativeMenu = new NativeMenu());
		context.PushParent(nativeMenu);
		NativeMenu nativeMenu6 = nativeMenu;
		IList<NativeMenuItemBase> items29 = nativeMenu6.Items;
		NativeMenuItem item29 = (nativeMenuItem = new NativeMenuItem());
		context.PushParent(nativeMenuItem);
		NativeMenuItem nativeMenuItem30 = nativeMenuItem;
		StyledProperty<string?> headerProperty29 = NativeMenuItem.HeaderProperty;
		CompiledBindingExtension compiledBindingExtension29 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EServices_002EApp_002ELocalizationBindingSource_002CXFolder_002EItem_005BFullScreen_005D_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build())
		{
			Source = LocalizationBindingSource.Instance
		};
		context.ProvideTargetProperty = NativeMenuItem.HeaderProperty;
		CompiledBinding binding29 = compiledBindingExtension29.ProvideValue(context);
		context.ProvideTargetProperty = null;
		nativeMenuItem30.Bind(headerProperty29, binding29);
		context.PopParent();
		items29.Add(item29);
		IList<NativeMenuItemBase> items30 = nativeMenu6.Items;
		NativeMenuItem item30 = (nativeMenuItem = new NativeMenuItem());
		context.PushParent(nativeMenuItem);
		NativeMenuItem nativeMenuItem31 = nativeMenuItem;
		StyledProperty<string?> headerProperty30 = NativeMenuItem.HeaderProperty;
		CompiledBindingExtension compiledBindingExtension30 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EServices_002EApp_002ELocalizationBindingSource_002CXFolder_002EItem_005BToggleSidebar_005D_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build())
		{
			Source = LocalizationBindingSource.Instance
		};
		context.ProvideTargetProperty = NativeMenuItem.HeaderProperty;
		CompiledBinding binding30 = compiledBindingExtension30.ProvideValue(context);
		context.ProvideTargetProperty = null;
		nativeMenuItem31.Bind(headerProperty30, binding30);
		context.PopParent();
		items30.Add(item30);
		nativeMenu6.Items.Add(new NativeMenuItemSeparator());
		IList<NativeMenuItemBase> items31 = nativeMenu6.Items;
		NativeMenuItem item31 = (nativeMenuItem = new NativeMenuItem());
		context.PushParent(nativeMenuItem);
		NativeMenuItem nativeMenuItem32 = nativeMenuItem;
		StyledProperty<string?> headerProperty31 = NativeMenuItem.HeaderProperty;
		CompiledBindingExtension compiledBindingExtension31 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EServices_002EApp_002ELocalizationBindingSource_002CXFolder_002EItem_005BLayout_005D_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build())
		{
			Source = LocalizationBindingSource.Instance
		};
		context.ProvideTargetProperty = NativeMenuItem.HeaderProperty;
		CompiledBinding binding31 = compiledBindingExtension31.ProvideValue(context);
		context.ProvideTargetProperty = null;
		nativeMenuItem32.Bind(headerProperty31, binding31);
		NativeMenu menu6 = (nativeMenu = new NativeMenu());
		context.PushParent(nativeMenu);
		NativeMenu nativeMenu7 = nativeMenu;
		IList<NativeMenuItemBase> items32 = nativeMenu7.Items;
		NativeMenuItem item32 = (nativeMenuItem = new NativeMenuItem());
		context.PushParent(nativeMenuItem);
		NativeMenuItem nativeMenuItem33 = nativeMenuItem;
		StyledProperty<string?> headerProperty32 = NativeMenuItem.HeaderProperty;
		CompiledBindingExtension compiledBindingExtension32 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EServices_002EApp_002ELocalizationBindingSource_002CXFolder_002EItem_005BDetails_005D_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build())
		{
			Source = LocalizationBindingSource.Instance
		};
		context.ProvideTargetProperty = NativeMenuItem.HeaderProperty;
		CompiledBinding binding32 = compiledBindingExtension32.ProvideValue(context);
		context.ProvideTargetProperty = null;
		nativeMenuItem33.Bind(headerProperty32, binding32);
		context.PopParent();
		items32.Add(item32);
		IList<NativeMenuItemBase> items33 = nativeMenu7.Items;
		NativeMenuItem item33 = (nativeMenuItem = new NativeMenuItem());
		context.PushParent(nativeMenuItem);
		NativeMenuItem nativeMenuItem34 = nativeMenuItem;
		StyledProperty<string?> headerProperty33 = NativeMenuItem.HeaderProperty;
		CompiledBindingExtension compiledBindingExtension33 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EServices_002EApp_002ELocalizationBindingSource_002CXFolder_002EItem_005BGrid_005D_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build())
		{
			Source = LocalizationBindingSource.Instance
		};
		context.ProvideTargetProperty = NativeMenuItem.HeaderProperty;
		CompiledBinding binding33 = compiledBindingExtension33.ProvideValue(context);
		context.ProvideTargetProperty = null;
		nativeMenuItem34.Bind(headerProperty33, binding33);
		context.PopParent();
		items33.Add(item33);
		context.PopParent();
		nativeMenuItem32.Menu = menu6;
		context.PopParent();
		items31.Add(item31);
		IList<NativeMenuItemBase> items34 = nativeMenu6.Items;
		NativeMenuItem item34 = (nativeMenuItem = new NativeMenuItem());
		context.PushParent(nativeMenuItem);
		NativeMenuItem nativeMenuItem35 = nativeMenuItem;
		StyledProperty<string?> headerProperty34 = NativeMenuItem.HeaderProperty;
		CompiledBindingExtension compiledBindingExtension34 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EServices_002EApp_002ELocalizationBindingSource_002CXFolder_002EItem_005BSortBy_005D_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build())
		{
			Source = LocalizationBindingSource.Instance
		};
		context.ProvideTargetProperty = NativeMenuItem.HeaderProperty;
		CompiledBinding binding34 = compiledBindingExtension34.ProvideValue(context);
		context.ProvideTargetProperty = null;
		nativeMenuItem35.Bind(headerProperty34, binding34);
		NativeMenu menu7 = (nativeMenu = new NativeMenu());
		context.PushParent(nativeMenu);
		NativeMenu nativeMenu8 = nativeMenu;
		IList<NativeMenuItemBase> items35 = nativeMenu8.Items;
		NativeMenuItem item35 = (nativeMenuItem = new NativeMenuItem());
		context.PushParent(nativeMenuItem);
		NativeMenuItem nativeMenuItem36 = nativeMenuItem;
		StyledProperty<string?> headerProperty35 = NativeMenuItem.HeaderProperty;
		CompiledBindingExtension compiledBindingExtension35 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EServices_002EApp_002ELocalizationBindingSource_002CXFolder_002EItem_005BName_005D_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build())
		{
			Source = LocalizationBindingSource.Instance
		};
		context.ProvideTargetProperty = NativeMenuItem.HeaderProperty;
		CompiledBinding binding35 = compiledBindingExtension35.ProvideValue(context);
		context.ProvideTargetProperty = null;
		nativeMenuItem36.Bind(headerProperty35, binding35);
		context.PopParent();
		items35.Add(item35);
		IList<NativeMenuItemBase> items36 = nativeMenu8.Items;
		NativeMenuItem item36 = (nativeMenuItem = new NativeMenuItem());
		context.PushParent(nativeMenuItem);
		NativeMenuItem nativeMenuItem37 = nativeMenuItem;
		StyledProperty<string?> headerProperty36 = NativeMenuItem.HeaderProperty;
		CompiledBindingExtension compiledBindingExtension36 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EServices_002EApp_002ELocalizationBindingSource_002CXFolder_002EItem_005BDateModifiedLowerCase_005D_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build())
		{
			Source = LocalizationBindingSource.Instance
		};
		context.ProvideTargetProperty = NativeMenuItem.HeaderProperty;
		CompiledBinding binding36 = compiledBindingExtension36.ProvideValue(context);
		context.ProvideTargetProperty = null;
		nativeMenuItem37.Bind(headerProperty36, binding36);
		context.PopParent();
		items36.Add(item36);
		IList<NativeMenuItemBase> items37 = nativeMenu8.Items;
		NativeMenuItem item37 = (nativeMenuItem = new NativeMenuItem());
		context.PushParent(nativeMenuItem);
		NativeMenuItem nativeMenuItem38 = nativeMenuItem;
		StyledProperty<string?> headerProperty37 = NativeMenuItem.HeaderProperty;
		CompiledBindingExtension compiledBindingExtension37 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EServices_002EApp_002ELocalizationBindingSource_002CXFolder_002EItem_005BDateCreated_005D_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build())
		{
			Source = LocalizationBindingSource.Instance
		};
		context.ProvideTargetProperty = NativeMenuItem.HeaderProperty;
		CompiledBinding binding37 = compiledBindingExtension37.ProvideValue(context);
		context.ProvideTargetProperty = null;
		nativeMenuItem38.Bind(headerProperty37, binding37);
		context.PopParent();
		items37.Add(item37);
		IList<NativeMenuItemBase> items38 = nativeMenu8.Items;
		NativeMenuItem item38 = (nativeMenuItem = new NativeMenuItem());
		context.PushParent(nativeMenuItem);
		NativeMenuItem nativeMenuItem39 = nativeMenuItem;
		StyledProperty<string?> headerProperty38 = NativeMenuItem.HeaderProperty;
		CompiledBindingExtension compiledBindingExtension38 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EServices_002EApp_002ELocalizationBindingSource_002CXFolder_002EItem_005BSize_005D_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build())
		{
			Source = LocalizationBindingSource.Instance
		};
		context.ProvideTargetProperty = NativeMenuItem.HeaderProperty;
		CompiledBinding binding38 = compiledBindingExtension38.ProvideValue(context);
		context.ProvideTargetProperty = null;
		nativeMenuItem39.Bind(headerProperty38, binding38);
		context.PopParent();
		items38.Add(item38);
		IList<NativeMenuItemBase> items39 = nativeMenu8.Items;
		NativeMenuItem item39 = (nativeMenuItem = new NativeMenuItem());
		context.PushParent(nativeMenuItem);
		NativeMenuItem nativeMenuItem40 = nativeMenuItem;
		StyledProperty<string?> headerProperty39 = NativeMenuItem.HeaderProperty;
		CompiledBindingExtension compiledBindingExtension39 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EServices_002EApp_002ELocalizationBindingSource_002CXFolder_002EItem_005BType_005D_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build())
		{
			Source = LocalizationBindingSource.Instance
		};
		context.ProvideTargetProperty = NativeMenuItem.HeaderProperty;
		CompiledBinding binding39 = compiledBindingExtension39.ProvideValue(context);
		context.ProvideTargetProperty = null;
		nativeMenuItem40.Bind(headerProperty39, binding39);
		context.PopParent();
		items39.Add(item39);
		nativeMenu8.Items.Add(new NativeMenuItemSeparator());
		IList<NativeMenuItemBase> items40 = nativeMenu8.Items;
		NativeMenuItem item40 = (nativeMenuItem = new NativeMenuItem());
		context.PushParent(nativeMenuItem);
		NativeMenuItem nativeMenuItem41 = nativeMenuItem;
		StyledProperty<string?> headerProperty40 = NativeMenuItem.HeaderProperty;
		CompiledBindingExtension compiledBindingExtension40 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EServices_002EApp_002ELocalizationBindingSource_002CXFolder_002EItem_005BAscending_005D_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build())
		{
			Source = LocalizationBindingSource.Instance
		};
		context.ProvideTargetProperty = NativeMenuItem.HeaderProperty;
		CompiledBinding binding40 = compiledBindingExtension40.ProvideValue(context);
		context.ProvideTargetProperty = null;
		nativeMenuItem41.Bind(headerProperty40, binding40);
		context.PopParent();
		items40.Add(item40);
		IList<NativeMenuItemBase> items41 = nativeMenu8.Items;
		NativeMenuItem item41 = (nativeMenuItem = new NativeMenuItem());
		context.PushParent(nativeMenuItem);
		NativeMenuItem nativeMenuItem42 = nativeMenuItem;
		StyledProperty<string?> headerProperty41 = NativeMenuItem.HeaderProperty;
		CompiledBindingExtension compiledBindingExtension41 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EServices_002EApp_002ELocalizationBindingSource_002CXFolder_002EItem_005BDescending_005D_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build())
		{
			Source = LocalizationBindingSource.Instance
		};
		context.ProvideTargetProperty = NativeMenuItem.HeaderProperty;
		CompiledBinding binding41 = compiledBindingExtension41.ProvideValue(context);
		context.ProvideTargetProperty = null;
		nativeMenuItem42.Bind(headerProperty41, binding41);
		context.PopParent();
		items41.Add(item41);
		context.PopParent();
		nativeMenuItem35.Menu = menu7;
		context.PopParent();
		items34.Add(item34);
		IList<NativeMenuItemBase> items42 = nativeMenu6.Items;
		NativeMenuItem item42 = (nativeMenuItem = new NativeMenuItem());
		context.PushParent(nativeMenuItem);
		NativeMenuItem nativeMenuItem43 = nativeMenuItem;
		StyledProperty<string?> headerProperty42 = NativeMenuItem.HeaderProperty;
		CompiledBindingExtension compiledBindingExtension42 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EServices_002EApp_002ELocalizationBindingSource_002CXFolder_002EItem_005BShowHiddenItems_005D_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build())
		{
			Source = LocalizationBindingSource.Instance
		};
		context.ProvideTargetProperty = NativeMenuItem.HeaderProperty;
		CompiledBinding binding42 = compiledBindingExtension42.ProvideValue(context);
		context.ProvideTargetProperty = null;
		nativeMenuItem43.Bind(headerProperty42, binding42);
		context.PopParent();
		items42.Add(item42);
		IList<NativeMenuItemBase> items43 = nativeMenu6.Items;
		NativeMenuItem item43 = (nativeMenuItem = new NativeMenuItem());
		context.PushParent(nativeMenuItem);
		NativeMenuItem nativeMenuItem44 = nativeMenuItem;
		StyledProperty<string?> headerProperty43 = NativeMenuItem.HeaderProperty;
		CompiledBindingExtension compiledBindingExtension43 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EServices_002EApp_002ELocalizationBindingSource_002CXFolder_002EItem_005BShowFileExtensions_005D_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build())
		{
			Source = LocalizationBindingSource.Instance
		};
		context.ProvideTargetProperty = NativeMenuItem.HeaderProperty;
		CompiledBinding binding43 = compiledBindingExtension43.ProvideValue(context);
		context.ProvideTargetProperty = null;
		nativeMenuItem44.Bind(headerProperty43, binding43);
		context.PopParent();
		items43.Add(item43);
		IList<NativeMenuItemBase> items44 = nativeMenu6.Items;
		NativeMenuItem item44 = (nativeMenuItem = new NativeMenuItem());
		context.PushParent(nativeMenuItem);
		NativeMenuItem nativeMenuItem45 = nativeMenuItem;
		StyledProperty<string?> headerProperty44 = NativeMenuItem.HeaderProperty;
		CompiledBindingExtension compiledBindingExtension44 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EServices_002EApp_002ELocalizationBindingSource_002CXFolder_002EItem_005BRefresh_005D_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build())
		{
			Source = LocalizationBindingSource.Instance
		};
		context.ProvideTargetProperty = NativeMenuItem.HeaderProperty;
		CompiledBinding binding44 = compiledBindingExtension44.ProvideValue(context);
		context.ProvideTargetProperty = null;
		nativeMenuItem45.Bind(headerProperty44, binding44);
		context.PopParent();
		items44.Add(item44);
		context.PopParent();
		nativeMenuItem29.Menu = menu5;
		context.PopParent();
		items28.Add(item28);
		IList<NativeMenuItemBase> items45 = nativeMenu2.Items;
		NativeMenuItem item45 = (nativeMenuItem = new NativeMenuItem());
		context.PushParent(nativeMenuItem);
		NativeMenuItem nativeMenuItem46 = nativeMenuItem;
		StyledProperty<string?> headerProperty45 = NativeMenuItem.HeaderProperty;
		CompiledBindingExtension compiledBindingExtension45 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EServices_002EApp_002ELocalizationBindingSource_002CXFolder_002EItem_005BGo_005D_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build())
		{
			Source = LocalizationBindingSource.Instance
		};
		context.ProvideTargetProperty = NativeMenuItem.HeaderProperty;
		CompiledBinding binding45 = compiledBindingExtension45.ProvideValue(context);
		context.ProvideTargetProperty = null;
		nativeMenuItem46.Bind(headerProperty45, binding45);
		NativeMenu menu8 = (nativeMenu = new NativeMenu());
		context.PushParent(nativeMenu);
		NativeMenu nativeMenu9 = nativeMenu;
		IList<NativeMenuItemBase> items46 = nativeMenu9.Items;
		NativeMenuItem item46 = (nativeMenuItem = new NativeMenuItem());
		context.PushParent(nativeMenuItem);
		NativeMenuItem nativeMenuItem47 = nativeMenuItem;
		StyledProperty<string?> headerProperty46 = NativeMenuItem.HeaderProperty;
		CompiledBindingExtension compiledBindingExtension46 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EServices_002EApp_002ELocalizationBindingSource_002CXFolder_002EItem_005BBack_005D_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build())
		{
			Source = LocalizationBindingSource.Instance
		};
		context.ProvideTargetProperty = NativeMenuItem.HeaderProperty;
		CompiledBinding binding46 = compiledBindingExtension46.ProvideValue(context);
		context.ProvideTargetProperty = null;
		nativeMenuItem47.Bind(headerProperty46, binding46);
		context.PopParent();
		items46.Add(item46);
		IList<NativeMenuItemBase> items47 = nativeMenu9.Items;
		NativeMenuItem item47 = (nativeMenuItem = new NativeMenuItem());
		context.PushParent(nativeMenuItem);
		NativeMenuItem nativeMenuItem48 = nativeMenuItem;
		StyledProperty<string?> headerProperty47 = NativeMenuItem.HeaderProperty;
		CompiledBindingExtension compiledBindingExtension47 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EServices_002EApp_002ELocalizationBindingSource_002CXFolder_002EItem_005BForward_005D_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build())
		{
			Source = LocalizationBindingSource.Instance
		};
		context.ProvideTargetProperty = NativeMenuItem.HeaderProperty;
		CompiledBinding binding47 = compiledBindingExtension47.ProvideValue(context);
		context.ProvideTargetProperty = null;
		nativeMenuItem48.Bind(headerProperty47, binding47);
		context.PopParent();
		items47.Add(item47);
		IList<NativeMenuItemBase> items48 = nativeMenu9.Items;
		NativeMenuItem item48 = (nativeMenuItem = new NativeMenuItem());
		context.PushParent(nativeMenuItem);
		NativeMenuItem nativeMenuItem49 = nativeMenuItem;
		StyledProperty<string?> headerProperty48 = NativeMenuItem.HeaderProperty;
		CompiledBindingExtension compiledBindingExtension48 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EServices_002EApp_002ELocalizationBindingSource_002CXFolder_002EItem_005BUp_005D_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build())
		{
			Source = LocalizationBindingSource.Instance
		};
		context.ProvideTargetProperty = NativeMenuItem.HeaderProperty;
		CompiledBinding binding48 = compiledBindingExtension48.ProvideValue(context);
		context.ProvideTargetProperty = null;
		nativeMenuItem49.Bind(headerProperty48, binding48);
		context.PopParent();
		items48.Add(item48);
		nativeMenu9.Items.Add(new NativeMenuItemSeparator());
		IList<NativeMenuItemBase> items49 = nativeMenu9.Items;
		NativeMenuItem item49 = (nativeMenuItem = new NativeMenuItem());
		context.PushParent(nativeMenuItem);
		NativeMenuItem nativeMenuItem50 = nativeMenuItem;
		StyledProperty<string?> headerProperty49 = NativeMenuItem.HeaderProperty;
		CompiledBindingExtension compiledBindingExtension49 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EServices_002EApp_002ELocalizationBindingSource_002CXFolder_002EItem_005BApplications_005D_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build())
		{
			Source = LocalizationBindingSource.Instance
		};
		context.ProvideTargetProperty = NativeMenuItem.HeaderProperty;
		CompiledBinding binding49 = compiledBindingExtension49.ProvideValue(context);
		context.ProvideTargetProperty = null;
		nativeMenuItem50.Bind(headerProperty49, binding49);
		context.PopParent();
		items49.Add(item49);
		IList<NativeMenuItemBase> items50 = nativeMenu9.Items;
		NativeMenuItem item50 = (nativeMenuItem = new NativeMenuItem());
		context.PushParent(nativeMenuItem);
		NativeMenuItem nativeMenuItem51 = nativeMenuItem;
		StyledProperty<string?> headerProperty50 = NativeMenuItem.HeaderProperty;
		CompiledBindingExtension compiledBindingExtension50 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EServices_002EApp_002ELocalizationBindingSource_002CXFolder_002EItem_005BDesktop_005D_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build())
		{
			Source = LocalizationBindingSource.Instance
		};
		context.ProvideTargetProperty = NativeMenuItem.HeaderProperty;
		CompiledBinding binding50 = compiledBindingExtension50.ProvideValue(context);
		context.ProvideTargetProperty = null;
		nativeMenuItem51.Bind(headerProperty50, binding50);
		context.PopParent();
		items50.Add(item50);
		IList<NativeMenuItemBase> items51 = nativeMenu9.Items;
		NativeMenuItem item51 = (nativeMenuItem = new NativeMenuItem());
		context.PushParent(nativeMenuItem);
		NativeMenuItem nativeMenuItem52 = nativeMenuItem;
		StyledProperty<string?> headerProperty51 = NativeMenuItem.HeaderProperty;
		CompiledBindingExtension compiledBindingExtension51 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EServices_002EApp_002ELocalizationBindingSource_002CXFolder_002EItem_005BDocuments_005D_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build())
		{
			Source = LocalizationBindingSource.Instance
		};
		context.ProvideTargetProperty = NativeMenuItem.HeaderProperty;
		CompiledBinding binding51 = compiledBindingExtension51.ProvideValue(context);
		context.ProvideTargetProperty = null;
		nativeMenuItem52.Bind(headerProperty51, binding51);
		context.PopParent();
		items51.Add(item51);
		IList<NativeMenuItemBase> items52 = nativeMenu9.Items;
		NativeMenuItem item52 = (nativeMenuItem = new NativeMenuItem());
		context.PushParent(nativeMenuItem);
		NativeMenuItem nativeMenuItem53 = nativeMenuItem;
		StyledProperty<string?> headerProperty52 = NativeMenuItem.HeaderProperty;
		CompiledBindingExtension compiledBindingExtension52 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EServices_002EApp_002ELocalizationBindingSource_002CXFolder_002EItem_005BDownloads_005D_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build())
		{
			Source = LocalizationBindingSource.Instance
		};
		context.ProvideTargetProperty = NativeMenuItem.HeaderProperty;
		CompiledBinding binding52 = compiledBindingExtension52.ProvideValue(context);
		context.ProvideTargetProperty = null;
		nativeMenuItem53.Bind(headerProperty52, binding52);
		context.PopParent();
		items52.Add(item52);
		IList<NativeMenuItemBase> items53 = nativeMenu9.Items;
		NativeMenuItem item53 = (nativeMenuItem = new NativeMenuItem());
		context.PushParent(nativeMenuItem);
		NativeMenuItem nativeMenuItem54 = nativeMenuItem;
		StyledProperty<string?> headerProperty53 = NativeMenuItem.HeaderProperty;
		CompiledBindingExtension compiledBindingExtension53 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EServices_002EApp_002ELocalizationBindingSource_002CXFolder_002EItem_005BMusic_005D_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build())
		{
			Source = LocalizationBindingSource.Instance
		};
		context.ProvideTargetProperty = NativeMenuItem.HeaderProperty;
		CompiledBinding binding53 = compiledBindingExtension53.ProvideValue(context);
		context.ProvideTargetProperty = null;
		nativeMenuItem54.Bind(headerProperty53, binding53);
		context.PopParent();
		items53.Add(item53);
		IList<NativeMenuItemBase> items54 = nativeMenu9.Items;
		NativeMenuItem item54 = (nativeMenuItem = new NativeMenuItem());
		context.PushParent(nativeMenuItem);
		NativeMenuItem nativeMenuItem55 = nativeMenuItem;
		StyledProperty<string?> headerProperty54 = NativeMenuItem.HeaderProperty;
		CompiledBindingExtension compiledBindingExtension54 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EServices_002EApp_002ELocalizationBindingSource_002CXFolder_002EItem_005BMovies_005D_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build())
		{
			Source = LocalizationBindingSource.Instance
		};
		context.ProvideTargetProperty = NativeMenuItem.HeaderProperty;
		CompiledBinding binding54 = compiledBindingExtension54.ProvideValue(context);
		context.ProvideTargetProperty = null;
		nativeMenuItem55.Bind(headerProperty54, binding54);
		context.PopParent();
		items54.Add(item54);
		IList<NativeMenuItemBase> items55 = nativeMenu9.Items;
		NativeMenuItem item55 = (nativeMenuItem = new NativeMenuItem());
		context.PushParent(nativeMenuItem);
		NativeMenuItem nativeMenuItem56 = nativeMenuItem;
		StyledProperty<string?> headerProperty55 = NativeMenuItem.HeaderProperty;
		CompiledBindingExtension compiledBindingExtension55 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EServices_002EApp_002ELocalizationBindingSource_002CXFolder_002EItem_005BPictures_005D_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build())
		{
			Source = LocalizationBindingSource.Instance
		};
		context.ProvideTargetProperty = NativeMenuItem.HeaderProperty;
		CompiledBinding binding55 = compiledBindingExtension55.ProvideValue(context);
		context.ProvideTargetProperty = null;
		nativeMenuItem56.Bind(headerProperty55, binding55);
		context.PopParent();
		items55.Add(item55);
		IList<NativeMenuItemBase> items56 = nativeMenu9.Items;
		NativeMenuItem item56 = (nativeMenuItem = new NativeMenuItem());
		context.PushParent(nativeMenuItem);
		NativeMenuItem nativeMenuItem57 = nativeMenuItem;
		StyledProperty<string?> headerProperty56 = NativeMenuItem.HeaderProperty;
		CompiledBindingExtension compiledBindingExtension56 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EServices_002EApp_002ELocalizationBindingSource_002CXFolder_002EItem_005BHome_005D_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build())
		{
			Source = LocalizationBindingSource.Instance
		};
		context.ProvideTargetProperty = NativeMenuItem.HeaderProperty;
		CompiledBinding binding56 = compiledBindingExtension56.ProvideValue(context);
		context.ProvideTargetProperty = null;
		nativeMenuItem57.Bind(headerProperty56, binding56);
		context.PopParent();
		items56.Add(item56);
		nativeMenu9.Items.Add(new NativeMenuItemSeparator());
		context.PopParent();
		nativeMenuItem46.Menu = menu8;
		context.PopParent();
		items45.Add(item45);
		IList<NativeMenuItemBase> items57 = nativeMenu2.Items;
		NativeMenuItem item57 = (nativeMenuItem = new NativeMenuItem());
		context.PushParent(nativeMenuItem);
		NativeMenuItem nativeMenuItem58 = nativeMenuItem;
		StyledProperty<string?> headerProperty57 = NativeMenuItem.HeaderProperty;
		CompiledBindingExtension compiledBindingExtension57 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EServices_002EApp_002ELocalizationBindingSource_002CXFolder_002EItem_005BTabs_005D_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build())
		{
			Source = LocalizationBindingSource.Instance
		};
		context.ProvideTargetProperty = NativeMenuItem.HeaderProperty;
		CompiledBinding binding57 = compiledBindingExtension57.ProvideValue(context);
		context.ProvideTargetProperty = null;
		nativeMenuItem58.Bind(headerProperty57, binding57);
		NativeMenu menu9 = (nativeMenu = new NativeMenu());
		context.PushParent(nativeMenu);
		NativeMenu nativeMenu10 = nativeMenu;
		IList<NativeMenuItemBase> items58 = nativeMenu10.Items;
		NativeMenuItem item58 = (nativeMenuItem = new NativeMenuItem());
		context.PushParent(nativeMenuItem);
		NativeMenuItem nativeMenuItem59 = nativeMenuItem;
		StyledProperty<string?> headerProperty58 = NativeMenuItem.HeaderProperty;
		CompiledBindingExtension compiledBindingExtension58 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EServices_002EApp_002ELocalizationBindingSource_002CXFolder_002EItem_005BNewTab_005D_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build())
		{
			Source = LocalizationBindingSource.Instance
		};
		context.ProvideTargetProperty = NativeMenuItem.HeaderProperty;
		CompiledBinding binding58 = compiledBindingExtension58.ProvideValue(context);
		context.ProvideTargetProperty = null;
		nativeMenuItem59.Bind(headerProperty58, binding58);
		context.PopParent();
		items58.Add(item58);
		IList<NativeMenuItemBase> items59 = nativeMenu10.Items;
		NativeMenuItem item59 = (nativeMenuItem = new NativeMenuItem());
		context.PushParent(nativeMenuItem);
		NativeMenuItem nativeMenuItem60 = nativeMenuItem;
		StyledProperty<string?> headerProperty59 = NativeMenuItem.HeaderProperty;
		CompiledBindingExtension compiledBindingExtension59 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EServices_002EApp_002ELocalizationBindingSource_002CXFolder_002EItem_005BDuplicateTab_005D_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build())
		{
			Source = LocalizationBindingSource.Instance
		};
		context.ProvideTargetProperty = NativeMenuItem.HeaderProperty;
		CompiledBinding binding59 = compiledBindingExtension59.ProvideValue(context);
		context.ProvideTargetProperty = null;
		nativeMenuItem60.Bind(headerProperty59, binding59);
		context.PopParent();
		items59.Add(item59);
		IList<NativeMenuItemBase> items60 = nativeMenu10.Items;
		NativeMenuItem item60 = (nativeMenuItem = new NativeMenuItem());
		context.PushParent(nativeMenuItem);
		NativeMenuItem nativeMenuItem61 = nativeMenuItem;
		StyledProperty<string?> headerProperty60 = NativeMenuItem.HeaderProperty;
		CompiledBindingExtension compiledBindingExtension60 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EServices_002EApp_002ELocalizationBindingSource_002CXFolder_002EItem_005BReopenClosedTab_005D_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build())
		{
			Source = LocalizationBindingSource.Instance
		};
		context.ProvideTargetProperty = NativeMenuItem.HeaderProperty;
		CompiledBinding binding60 = compiledBindingExtension60.ProvideValue(context);
		context.ProvideTargetProperty = null;
		nativeMenuItem61.Bind(headerProperty60, binding60);
		context.PopParent();
		items60.Add(item60);
		nativeMenu10.Items.Add(new NativeMenuItemSeparator());
		IList<NativeMenuItemBase> items61 = nativeMenu10.Items;
		NativeMenuItem item61 = (nativeMenuItem = new NativeMenuItem());
		context.PushParent(nativeMenuItem);
		NativeMenuItem nativeMenuItem62 = nativeMenuItem;
		StyledProperty<string?> headerProperty61 = NativeMenuItem.HeaderProperty;
		CompiledBindingExtension compiledBindingExtension61 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EServices_002EApp_002ELocalizationBindingSource_002CXFolder_002EItem_005BPreviousTab_005D_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build())
		{
			Source = LocalizationBindingSource.Instance
		};
		context.ProvideTargetProperty = NativeMenuItem.HeaderProperty;
		CompiledBinding binding61 = compiledBindingExtension61.ProvideValue(context);
		context.ProvideTargetProperty = null;
		nativeMenuItem62.Bind(headerProperty61, binding61);
		context.PopParent();
		items61.Add(item61);
		IList<NativeMenuItemBase> items62 = nativeMenu10.Items;
		NativeMenuItem item62 = (nativeMenuItem = new NativeMenuItem());
		context.PushParent(nativeMenuItem);
		NativeMenuItem nativeMenuItem63 = nativeMenuItem;
		StyledProperty<string?> headerProperty62 = NativeMenuItem.HeaderProperty;
		CompiledBindingExtension compiledBindingExtension62 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EServices_002EApp_002ELocalizationBindingSource_002CXFolder_002EItem_005BNextTab_005D_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build())
		{
			Source = LocalizationBindingSource.Instance
		};
		context.ProvideTargetProperty = NativeMenuItem.HeaderProperty;
		CompiledBinding binding62 = compiledBindingExtension62.ProvideValue(context);
		context.ProvideTargetProperty = null;
		nativeMenuItem63.Bind(headerProperty62, binding62);
		context.PopParent();
		items62.Add(item62);
		nativeMenu10.Items.Add(new NativeMenuItemSeparator());
		IList<NativeMenuItemBase> items63 = nativeMenu10.Items;
		NativeMenuItem item63 = (nativeMenuItem = new NativeMenuItem());
		context.PushParent(nativeMenuItem);
		NativeMenuItem nativeMenuItem64 = nativeMenuItem;
		StyledProperty<string?> headerProperty63 = NativeMenuItem.HeaderProperty;
		CompiledBindingExtension compiledBindingExtension63 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EServices_002EApp_002ELocalizationBindingSource_002CXFolder_002EItem_005BCloseTab_005D_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build())
		{
			Source = LocalizationBindingSource.Instance
		};
		context.ProvideTargetProperty = NativeMenuItem.HeaderProperty;
		CompiledBinding binding63 = compiledBindingExtension63.ProvideValue(context);
		context.ProvideTargetProperty = null;
		nativeMenuItem64.Bind(headerProperty63, binding63);
		context.PopParent();
		items63.Add(item63);
		IList<NativeMenuItemBase> items64 = nativeMenu10.Items;
		NativeMenuItem item64 = (nativeMenuItem = new NativeMenuItem());
		context.PushParent(nativeMenuItem);
		NativeMenuItem nativeMenuItem65 = nativeMenuItem;
		StyledProperty<string?> headerProperty64 = NativeMenuItem.HeaderProperty;
		CompiledBindingExtension compiledBindingExtension64 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EServices_002EApp_002ELocalizationBindingSource_002CXFolder_002EItem_005BCloseOtherTabs_005D_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build())
		{
			Source = LocalizationBindingSource.Instance
		};
		context.ProvideTargetProperty = NativeMenuItem.HeaderProperty;
		CompiledBinding binding64 = compiledBindingExtension64.ProvideValue(context);
		context.ProvideTargetProperty = null;
		nativeMenuItem65.Bind(headerProperty64, binding64);
		context.PopParent();
		items64.Add(item64);
		IList<NativeMenuItemBase> items65 = nativeMenu10.Items;
		NativeMenuItem item65 = (nativeMenuItem = new NativeMenuItem());
		context.PushParent(nativeMenuItem);
		NativeMenuItem nativeMenuItem66 = nativeMenuItem;
		StyledProperty<string?> headerProperty65 = NativeMenuItem.HeaderProperty;
		CompiledBindingExtension compiledBindingExtension65 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EServices_002EApp_002ELocalizationBindingSource_002CXFolder_002EItem_005BCloseTabsToTheLeft_005D_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build())
		{
			Source = LocalizationBindingSource.Instance
		};
		context.ProvideTargetProperty = NativeMenuItem.HeaderProperty;
		CompiledBinding binding65 = compiledBindingExtension65.ProvideValue(context);
		context.ProvideTargetProperty = null;
		nativeMenuItem66.Bind(headerProperty65, binding65);
		context.PopParent();
		items65.Add(item65);
		IList<NativeMenuItemBase> items66 = nativeMenu10.Items;
		NativeMenuItem item66 = (nativeMenuItem = new NativeMenuItem());
		context.PushParent(nativeMenuItem);
		NativeMenuItem nativeMenuItem67 = nativeMenuItem;
		StyledProperty<string?> headerProperty66 = NativeMenuItem.HeaderProperty;
		CompiledBindingExtension compiledBindingExtension66 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EServices_002EApp_002ELocalizationBindingSource_002CXFolder_002EItem_005BCloseTabsToTheRight_005D_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build())
		{
			Source = LocalizationBindingSource.Instance
		};
		context.ProvideTargetProperty = NativeMenuItem.HeaderProperty;
		CompiledBinding binding66 = compiledBindingExtension66.ProvideValue(context);
		context.ProvideTargetProperty = null;
		nativeMenuItem67.Bind(headerProperty66, binding66);
		context.PopParent();
		items66.Add(item66);
		context.PopParent();
		nativeMenuItem58.Menu = menu9;
		context.PopParent();
		items57.Add(item57);
		IList<NativeMenuItemBase> items67 = nativeMenu2.Items;
		NativeMenuItem item67 = (nativeMenuItem = new NativeMenuItem());
		context.PushParent(nativeMenuItem);
		NativeMenuItem nativeMenuItem68 = nativeMenuItem;
		StyledProperty<string?> headerProperty67 = NativeMenuItem.HeaderProperty;
		CompiledBindingExtension compiledBindingExtension67 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EServices_002EApp_002ELocalizationBindingSource_002CXFolder_002EItem_005BTools_005D_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build())
		{
			Source = LocalizationBindingSource.Instance
		};
		context.ProvideTargetProperty = NativeMenuItem.HeaderProperty;
		CompiledBinding binding67 = compiledBindingExtension67.ProvideValue(context);
		context.ProvideTargetProperty = null;
		nativeMenuItem68.Bind(headerProperty67, binding67);
		NativeMenu menu10 = (nativeMenu = new NativeMenu());
		context.PushParent(nativeMenu);
		NativeMenu nativeMenu11 = nativeMenu;
		nativeMenu11.Items.Add(new NativeMenuItem
		{
			Header = "Compress into tar"
		});
		nativeMenu11.Items.Add(new NativeMenuItem
		{
			Header = "Compress into zip"
		});
		nativeMenu11.Items.Add(new NativeMenuItemSeparator());
		IList<NativeMenuItemBase> items68 = nativeMenu11.Items;
		NativeMenuItem item68 = (nativeMenuItem = new NativeMenuItem());
		context.PushParent(nativeMenuItem);
		NativeMenuItem nativeMenuItem69 = nativeMenuItem;
		StyledProperty<string?> headerProperty68 = NativeMenuItem.HeaderProperty;
		CompiledBindingExtension compiledBindingExtension68 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EServices_002EApp_002ELocalizationBindingSource_002CXFolder_002EItem_005BConvertToPng_005D_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build())
		{
			Source = LocalizationBindingSource.Instance
		};
		context.ProvideTargetProperty = NativeMenuItem.HeaderProperty;
		CompiledBinding binding68 = compiledBindingExtension68.ProvideValue(context);
		context.ProvideTargetProperty = null;
		nativeMenuItem69.Bind(headerProperty68, binding68);
		context.PopParent();
		items68.Add(item68);
		IList<NativeMenuItemBase> items69 = nativeMenu11.Items;
		NativeMenuItem item69 = (nativeMenuItem = new NativeMenuItem());
		context.PushParent(nativeMenuItem);
		NativeMenuItem nativeMenuItem70 = nativeMenuItem;
		StyledProperty<string?> headerProperty69 = NativeMenuItem.HeaderProperty;
		CompiledBindingExtension compiledBindingExtension69 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EServices_002EApp_002ELocalizationBindingSource_002CXFolder_002EItem_005BConvertToJpeg_005D_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build())
		{
			Source = LocalizationBindingSource.Instance
		};
		context.ProvideTargetProperty = NativeMenuItem.HeaderProperty;
		CompiledBinding binding69 = compiledBindingExtension69.ProvideValue(context);
		context.ProvideTargetProperty = null;
		nativeMenuItem70.Bind(headerProperty69, binding69);
		context.PopParent();
		items69.Add(item69);
		IList<NativeMenuItemBase> items70 = nativeMenu11.Items;
		NativeMenuItem item70 = (nativeMenuItem = new NativeMenuItem());
		context.PushParent(nativeMenuItem);
		NativeMenuItem nativeMenuItem71 = nativeMenuItem;
		StyledProperty<string?> headerProperty70 = NativeMenuItem.HeaderProperty;
		CompiledBindingExtension compiledBindingExtension70 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EServices_002EApp_002ELocalizationBindingSource_002CXFolder_002EItem_005BConvertToBmp_005D_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build())
		{
			Source = LocalizationBindingSource.Instance
		};
		context.ProvideTargetProperty = NativeMenuItem.HeaderProperty;
		CompiledBinding binding70 = compiledBindingExtension70.ProvideValue(context);
		context.ProvideTargetProperty = null;
		nativeMenuItem71.Bind(headerProperty70, binding70);
		context.PopParent();
		items70.Add(item70);
		nativeMenu11.Items.Add(new NativeMenuItemSeparator());
		IList<NativeMenuItemBase> items71 = nativeMenu11.Items;
		NativeMenuItem item71 = (nativeMenuItem = new NativeMenuItem());
		context.PushParent(nativeMenuItem);
		NativeMenuItem nativeMenuItem72 = nativeMenuItem;
		StyledProperty<string?> headerProperty71 = NativeMenuItem.HeaderProperty;
		CompiledBindingExtension compiledBindingExtension71 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EServices_002EApp_002ELocalizationBindingSource_002CXFolder_002EItem_005BHashes_005D_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build())
		{
			Source = LocalizationBindingSource.Instance
		};
		context.ProvideTargetProperty = NativeMenuItem.HeaderProperty;
		CompiledBinding binding71 = compiledBindingExtension71.ProvideValue(context);
		context.ProvideTargetProperty = null;
		nativeMenuItem72.Bind(headerProperty71, binding71);
		context.PopParent();
		items71.Add(item71);
		nativeMenu11.Items.Add(new NativeMenuItemSeparator());
		IList<NativeMenuItemBase> items72 = nativeMenu11.Items;
		NativeMenuItem item72 = (nativeMenuItem = new NativeMenuItem());
		context.PushParent(nativeMenuItem);
		NativeMenuItem nativeMenuItem73 = nativeMenuItem;
		StyledProperty<string?> headerProperty72 = NativeMenuItem.HeaderProperty;
		CompiledBindingExtension compiledBindingExtension72 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EServices_002EApp_002ELocalizationBindingSource_002CXFolder_002EItem_005BOpenFinder_005D_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build())
		{
			Source = LocalizationBindingSource.Instance
		};
		context.ProvideTargetProperty = NativeMenuItem.HeaderProperty;
		CompiledBinding binding72 = compiledBindingExtension72.ProvideValue(context);
		context.ProvideTargetProperty = null;
		nativeMenuItem73.Bind(headerProperty72, binding72);
		context.PopParent();
		items72.Add(item72);
		IList<NativeMenuItemBase> items73 = nativeMenu11.Items;
		NativeMenuItem item73 = (nativeMenuItem = new NativeMenuItem());
		context.PushParent(nativeMenuItem);
		NativeMenuItem nativeMenuItem74 = nativeMenuItem;
		StyledProperty<string?> headerProperty73 = NativeMenuItem.HeaderProperty;
		CompiledBindingExtension compiledBindingExtension73 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EServices_002EApp_002ELocalizationBindingSource_002CXFolder_002EItem_005BOpenTerminal_005D_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build())
		{
			Source = LocalizationBindingSource.Instance
		};
		context.ProvideTargetProperty = NativeMenuItem.HeaderProperty;
		CompiledBinding binding73 = compiledBindingExtension73.ProvideValue(context);
		context.ProvideTargetProperty = null;
		nativeMenuItem74.Bind(headerProperty73, binding73);
		context.PopParent();
		items73.Add(item73);
		nativeMenu11.Items.Add(new NativeMenuItemSeparator());
		IList<NativeMenuItemBase> items74 = nativeMenu11.Items;
		NativeMenuItem item74 = (nativeMenuItem = new NativeMenuItem());
		context.PushParent(nativeMenuItem);
		NativeMenuItem nativeMenuItem75 = nativeMenuItem;
		StyledProperty<string?> headerProperty74 = NativeMenuItem.HeaderProperty;
		CompiledBindingExtension compiledBindingExtension74 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EServices_002EApp_002ELocalizationBindingSource_002CXFolder_002EItem_005BEditSettingsFile_005D_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build())
		{
			Source = LocalizationBindingSource.Instance
		};
		context.ProvideTargetProperty = NativeMenuItem.HeaderProperty;
		CompiledBinding binding74 = compiledBindingExtension74.ProvideValue(context);
		context.ProvideTargetProperty = null;
		nativeMenuItem75.Bind(headerProperty74, binding74);
		context.PopParent();
		items74.Add(item74);
		IList<NativeMenuItemBase> items75 = nativeMenu11.Items;
		NativeMenuItem item75 = (nativeMenuItem = new NativeMenuItem());
		context.PushParent(nativeMenuItem);
		NativeMenuItem nativeMenuItem76 = nativeMenuItem;
		StyledProperty<string?> headerProperty75 = NativeMenuItem.HeaderProperty;
		CompiledBindingExtension compiledBindingExtension75 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EServices_002EApp_002ELocalizationBindingSource_002CXFolder_002EItem_005BOpenAppSupportFolder_005D_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build())
		{
			Source = LocalizationBindingSource.Instance
		};
		context.ProvideTargetProperty = NativeMenuItem.HeaderProperty;
		CompiledBinding binding75 = compiledBindingExtension75.ProvideValue(context);
		context.ProvideTargetProperty = null;
		nativeMenuItem76.Bind(headerProperty75, binding75);
		context.PopParent();
		items75.Add(item75);
		context.PopParent();
		nativeMenuItem68.Menu = menu10;
		context.PopParent();
		items67.Add(item67);
		IList<NativeMenuItemBase> items76 = nativeMenu2.Items;
		NativeMenuItem item76 = (nativeMenuItem = new NativeMenuItem());
		context.PushParent(nativeMenuItem);
		NativeMenuItem nativeMenuItem77 = nativeMenuItem;
		StyledProperty<string?> headerProperty76 = NativeMenuItem.HeaderProperty;
		CompiledBindingExtension compiledBindingExtension76 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EServices_002EApp_002ELocalizationBindingSource_002CXFolder_002EItem_005BHelp_005D_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build())
		{
			Source = LocalizationBindingSource.Instance
		};
		context.ProvideTargetProperty = NativeMenuItem.HeaderProperty;
		CompiledBinding binding76 = compiledBindingExtension76.ProvideValue(context);
		context.ProvideTargetProperty = null;
		nativeMenuItem77.Bind(headerProperty76, binding76);
		NativeMenu menu11 = (nativeMenu = new NativeMenu());
		context.PushParent(nativeMenu);
		NativeMenu nativeMenu12 = nativeMenu;
		IList<NativeMenuItemBase> items77 = nativeMenu12.Items;
		NativeMenuItem item77 = (nativeMenuItem = new NativeMenuItem());
		context.PushParent(nativeMenuItem);
		NativeMenuItem nativeMenuItem78 = nativeMenuItem;
		StyledProperty<string?> headerProperty77 = NativeMenuItem.HeaderProperty;
		CompiledBindingExtension compiledBindingExtension77 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EServices_002EApp_002ELocalizationBindingSource_002CXFolder_002EItem_005BKeyboardShortcuts_005D_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build())
		{
			Source = LocalizationBindingSource.Instance
		};
		context.ProvideTargetProperty = NativeMenuItem.HeaderProperty;
		CompiledBinding binding77 = compiledBindingExtension77.ProvideValue(context);
		context.ProvideTargetProperty = null;
		nativeMenuItem78.Bind(headerProperty77, binding77);
		nativeMenuItem78.Click += context.RootObject.OnKeyboardShortcutsMenuClick;
		context.PopParent();
		items77.Add(item77);
		IList<NativeMenuItemBase> items78 = nativeMenu12.Items;
		NativeMenuItem item78 = (nativeMenuItem = new NativeMenuItem());
		context.PushParent(nativeMenuItem);
		NativeMenuItem nativeMenuItem79 = nativeMenuItem;
		StyledProperty<string?> headerProperty78 = NativeMenuItem.HeaderProperty;
		CompiledBindingExtension compiledBindingExtension78 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EServices_002EApp_002ELocalizationBindingSource_002CXFolder_002EItem_005BSubmitFeedback_005D_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build())
		{
			Source = LocalizationBindingSource.Instance
		};
		context.ProvideTargetProperty = NativeMenuItem.HeaderProperty;
		CompiledBinding binding78 = compiledBindingExtension78.ProvideValue(context);
		context.ProvideTargetProperty = null;
		nativeMenuItem79.Bind(headerProperty78, binding78);
		nativeMenuItem79.Click += context.RootObject.OnSubmitFeedbackClick;
		context.PopParent();
		items78.Add(item78);
		context.PopParent();
		nativeMenuItem77.Menu = menu11;
		context.PopParent();
		items76.Add(item76);
		context.PopParent();
		NativeMenu.SetMenu(P_1, menu);
		((ResourceDictionary)P_1.Resources).AddDeferred((object)"IsPaneOpenToGridLengthConverter", XamlIlRuntimeHelpers.DeferredTransformationFactoryV3<object>((nint)(delegate*<IServiceProvider, object>)(&XamlClosure_8.Build_1), context));
		DialogHost dialogHost2;
		DialogHost dialogHost = (dialogHost2 = new DialogHost());
		((ISupportInitialize)dialogHost).BeginInit();
		P_1.Content = dialogHost;
		DialogHost dialogHost4;
		DialogHost dialogHost3 = (dialogHost4 = dialogHost2);
		context.PushParent(dialogHost4);
		dialogHost4.Identifier = "MainDialogHost";
		dialogHost4.DialogMargin = new Thickness(16.0, 16.0, 16.0, 16.0);
		StyledProperty<IBrush?> backgroundProperty = TemplatedControl.BackgroundProperty;
		DynamicResourceExtension dynamicResourceExtension = new DynamicResourceExtension("DialogBackground");
		context.ProvideTargetProperty = TemplatedControl.BackgroundProperty;
		BindingBase binding79 = dynamicResourceExtension.ProvideValue(context);
		context.ProvideTargetProperty = null;
		dialogHost4.Bind(backgroundProperty, binding79);
		DialogHostStyle.SetCornerRadius(dialogHost4, new CornerRadius(8.0, 8.0, 8.0, 8.0));
		DockPanel dockPanel2;
		DockPanel dockPanel = (dockPanel2 = new DockPanel());
		((ISupportInitialize)dockPanel).BeginInit();
		dialogHost4.Content = dockPanel;
		DockPanel dockPanel4;
		DockPanel dockPanel3 = (dockPanel4 = dockPanel2);
		context.PushParent(dockPanel4);
		StyledProperty<IBrush?> backgroundProperty2 = Panel.BackgroundProperty;
		DynamicResourceExtension dynamicResourceExtension2 = new DynamicResourceExtension("TopToolbarBackground");
		context.ProvideTargetProperty = Panel.BackgroundProperty;
		BindingBase binding80 = dynamicResourceExtension2.ProvideValue(context);
		context.ProvideTargetProperty = null;
		dockPanel4.Bind(backgroundProperty2, binding80);
		Controls children = dockPanel4.Children;
		Grid grid2;
		Grid grid = (grid2 = new Grid());
		((ISupportInitialize)grid).BeginInit();
		children.Add(grid);
		Grid grid4;
		Grid grid3 = (grid4 = grid2);
		context.PushParent(grid4);
		Grid grid5 = grid4;
		grid5.Name = "TopToolbarGrid";
		object element = grid5;
		context.AvaloniaNameScope.Register("TopToolbarGrid", element);
		DockPanel.SetDock(grid5, Dock.Top);
		grid5.Height = 44.0;
		grid5.MinHeight = 44.0;
		StyledProperty<IBrush?> backgroundProperty3 = Panel.BackgroundProperty;
		DynamicResourceExtension dynamicResourceExtension3 = new DynamicResourceExtension("TopToolbarBackground");
		context.ProvideTargetProperty = Panel.BackgroundProperty;
		BindingBase binding81 = dynamicResourceExtension3.ProvideValue(context);
		context.ProvideTargetProperty = null;
		grid5.Bind(backgroundProperty3, binding81);
		grid5.ColumnDefinitions.Add(new ColumnDefinition
		{
			Width = new GridLength(92.0, GridUnitType.Pixel)
		});
		grid5.ColumnDefinitions.Add(new ColumnDefinition
		{
			Width = new GridLength(0.0, GridUnitType.Auto)
		});
		grid5.ColumnDefinitions.Add(new ColumnDefinition
		{
			Width = new GridLength(0.0, GridUnitType.Auto)
		});
		grid5.ColumnDefinitions.Add(new ColumnDefinition
		{
			Width = new GridLength(1.0, GridUnitType.Star)
		});
		grid5.ColumnDefinitions.Add(new ColumnDefinition
		{
			Width = new GridLength(0.0, GridUnitType.Auto)
		});
		Controls children2 = grid5.Children;
		Border border2;
		Border border = (border2 = new Border());
		((ISupportInitialize)border).BeginInit();
		children2.Add(border);
		Grid.SetColumn(border2, 0);
		border2.Height = 44.0;
		border2.Background = new ImmutableSolidColorBrush(16777215u);
		border2.AddHandler(InputElement.PointerPressedEvent, context.RootObject.OnTitleBarDragAreaPointerPressed);
		((ISupportInitialize)border2).EndInit();
		Controls children3 = grid5.Children;
		Border border5;
		Border border4 = (border5 = new Border());
		((ISupportInitialize)border4).BeginInit();
		children3.Add(border4);
		Grid.SetColumn(border5, 3);
		border5.Background = new ImmutableSolidColorBrush(16777215u);
		border5.AddHandler(InputElement.PointerPressedEvent, context.RootObject.OnTitleBarDragAreaPointerPressed);
		((ISupportInitialize)border5).EndInit();
		Controls children4 = grid5.Children;
		StackPanel stackPanel2;
		StackPanel stackPanel = (stackPanel2 = new StackPanel());
		((ISupportInitialize)stackPanel).BeginInit();
		children4.Add(stackPanel);
		StackPanel stackPanel4;
		StackPanel stackPanel3 = (stackPanel4 = stackPanel2);
		context.PushParent(stackPanel4);
		Grid.SetColumn(stackPanel4, 4);
		stackPanel4.Orientation = Orientation.Horizontal;
		stackPanel4.Margin = new Thickness(10.0, 0.0, 10.0, 0.0);
		stackPanel4.VerticalAlignment = VerticalAlignment.Center;
		Controls children5 = stackPanel4.Children;
		Button button2;
		Button button = (button2 = new Button());
		((ISupportInitialize)button).BeginInit();
		children5.Add(button);
		Button button4;
		Button button3 = (button4 = button2);
		context.PushParent(button4);
		Button button5 = button4;
		button5.Width = 28.0;
		button5.Height = 26.0;
		button5.Margin = new Thickness(1.0, 1.0, 1.0, 1.0);
		button5.HorizontalContentAlignment = HorizontalAlignment.Center;
		button5.AddHandler((RoutedEvent)Button.ClickEvent, (Delegate)new EventHandler<RoutedEventArgs>(context.RootObject.OnTogglePaneClick), RoutingStrategies.Direct | RoutingStrategies.Bubble, false);
		ToolTip.SetTip(button5, "Toggle Sidebar");
		Grid grid7;
		Grid grid6 = (grid7 = new Grid());
		((ISupportInitialize)grid6).BeginInit();
		button5.Content = grid6;
		Grid grid8 = (grid4 = grid7);
		context.PushParent(grid4);
		Grid grid9 = grid4;
		grid9.Width = 14.0;
		grid9.Height = 14.0;
		grid9.IsHitTestVisible = false;
		grid9.ColumnDefinitions.Add(new ColumnDefinition
		{
			Width = new GridLength(1.0, GridUnitType.Star)
		});
		grid9.ColumnDefinitions.Add(new ColumnDefinition
		{
			Width = new GridLength(2.0, GridUnitType.Star)
		});
		Controls children6 = grid9.Children;
		Border border7;
		Border border6 = (border7 = new Border());
		((ISupportInitialize)border6).BeginInit();
		children6.Add(border6);
		Border border3;
		Border border8 = (border3 = border7);
		context.PushParent(border3);
		Border border9 = border3;
		Grid.SetColumn(border9, 0);
		StyledProperty<IBrush?> borderBrushProperty = Border.BorderBrushProperty;
		DynamicResourceExtension dynamicResourceExtension4 = new DynamicResourceExtension("SystemControlForegroundBaseMediumBrush");
		context.ProvideTargetProperty = Border.BorderBrushProperty;
		BindingBase binding82 = dynamicResourceExtension4.ProvideValue(context);
		context.ProvideTargetProperty = null;
		border9.Bind(borderBrushProperty, binding82);
		border9.BorderThickness = new Thickness(1.0, 1.0, 1.0, 1.0);
		context.PopParent();
		((ISupportInitialize)border8).EndInit();
		Controls children7 = grid9.Children;
		Border border11;
		Border border10 = (border11 = new Border());
		((ISupportInitialize)border10).BeginInit();
		children7.Add(border10);
		Border border12 = (border3 = border11);
		context.PushParent(border3);
		Border border13 = border3;
		Grid.SetColumn(border13, 1);
		StyledProperty<IBrush?> borderBrushProperty2 = Border.BorderBrushProperty;
		DynamicResourceExtension dynamicResourceExtension5 = new DynamicResourceExtension("SystemControlForegroundBaseMediumBrush");
		context.ProvideTargetProperty = Border.BorderBrushProperty;
		BindingBase binding83 = dynamicResourceExtension5.ProvideValue(context);
		context.ProvideTargetProperty = null;
		border13.Bind(borderBrushProperty2, binding83);
		border13.BorderThickness = new Thickness(1.0, 1.0, 1.0, 1.0);
		context.PopParent();
		((ISupportInitialize)border12).EndInit();
		context.PopParent();
		((ISupportInitialize)grid8).EndInit();
		context.PopParent();
		((ISupportInitialize)button3).EndInit();
		Controls children8 = stackPanel4.Children;
		Border border15;
		Border border14 = (border15 = new Border());
		((ISupportInitialize)border14).BeginInit();
		children8.Add(border14);
		Border border16 = (border3 = border15);
		context.PushParent(border3);
		Border border17 = border3;
		border17.Width = 1.0;
		border17.Height = 16.0;
		StyledProperty<IBrush?> backgroundProperty4 = Border.BackgroundProperty;
		DynamicResourceExtension dynamicResourceExtension6 = new DynamicResourceExtension("SystemControlForegroundBaseMediumBrush");
		context.ProvideTargetProperty = Border.BackgroundProperty;
		BindingBase binding84 = dynamicResourceExtension6.ProvideValue(context);
		context.ProvideTargetProperty = null;
		border17.Bind(backgroundProperty4, binding84);
		border17.Margin = new Thickness(8.0, 0.0, 8.0, 0.0);
		border17.Opacity = 0.3;
		context.PopParent();
		((ISupportInitialize)border16).EndInit();
		Controls children9 = stackPanel4.Children;
		Button button7;
		Button button6 = (button7 = new Button());
		((ISupportInitialize)button6).BeginInit();
		children9.Add(button6);
		Button button8 = (button4 = button7);
		context.PushParent(button4);
		Button button9 = button4;
		button9.Width = 28.0;
		button9.Height = 26.0;
		button9.Margin = new Thickness(1.0, 1.0, 1.0, 1.0);
		button9.Tag = "ArrangePanesReset";
		button9.AddHandler((RoutedEvent)Button.ClickEvent, (Delegate)new EventHandler<RoutedEventArgs>(context.RootObject.OnRunActionCommand), RoutingStrategies.Direct | RoutingStrategies.Bubble, false);
		Grid grid11;
		Grid grid10 = (grid11 = new Grid());
		((ISupportInitialize)grid10).BeginInit();
		button9.Content = grid10;
		Grid grid12 = (grid4 = grid11);
		context.PushParent(grid4);
		Grid grid13 = grid4;
		grid13.Width = 14.0;
		grid13.Height = 14.0;
		grid13.IsHitTestVisible = false;
		Controls children10 = grid13.Children;
		Border border19;
		Border border18 = (border19 = new Border());
		((ISupportInitialize)border18).BeginInit();
		children10.Add(border18);
		Border border20 = (border3 = border19);
		context.PushParent(border3);
		Border border21 = border3;
		StyledProperty<IBrush?> borderBrushProperty3 = Border.BorderBrushProperty;
		DynamicResourceExtension dynamicResourceExtension7 = new DynamicResourceExtension("SystemControlForegroundBaseMediumBrush");
		context.ProvideTargetProperty = Border.BorderBrushProperty;
		BindingBase binding85 = dynamicResourceExtension7.ProvideValue(context);
		context.ProvideTargetProperty = null;
		border21.Bind(borderBrushProperty3, binding85);
		border21.BorderThickness = new Thickness(1.0, 1.0, 1.0, 1.0);
		context.PopParent();
		((ISupportInitialize)border20).EndInit();
		context.PopParent();
		((ISupportInitialize)grid12).EndInit();
		context.PopParent();
		((ISupportInitialize)button8).EndInit();
		Controls children11 = stackPanel4.Children;
		Button button11;
		Button button10 = (button11 = new Button());
		((ISupportInitialize)button10).BeginInit();
		children11.Add(button10);
		Button button12 = (button4 = button11);
		context.PushParent(button4);
		Button button13 = button4;
		button13.Width = 28.0;
		button13.Height = 26.0;
		button13.Margin = new Thickness(1.0, 1.0, 1.0, 1.0);
		button13.Tag = "ArrangePanesHorizontally";
		button13.AddHandler((RoutedEvent)Button.ClickEvent, (Delegate)new EventHandler<RoutedEventArgs>(context.RootObject.OnRunActionCommand), RoutingStrategies.Direct | RoutingStrategies.Bubble, false);
		Grid grid15;
		Grid grid14 = (grid15 = new Grid());
		((ISupportInitialize)grid14).BeginInit();
		button13.Content = grid14;
		Grid grid16 = (grid4 = grid15);
		context.PushParent(grid4);
		Grid grid17 = grid4;
		grid17.Width = 14.0;
		grid17.Height = 14.0;
		grid17.IsHitTestVisible = false;
		grid17.ColumnDefinitions.Add(new ColumnDefinition
		{
			Width = new GridLength(1.0, GridUnitType.Star)
		});
		grid17.ColumnDefinitions.Add(new ColumnDefinition
		{
			Width = new GridLength(1.0, GridUnitType.Star)
		});
		Controls children12 = grid17.Children;
		Border border23;
		Border border22 = (border23 = new Border());
		((ISupportInitialize)border22).BeginInit();
		children12.Add(border22);
		Border border24 = (border3 = border23);
		context.PushParent(border3);
		Border border25 = border3;
		Grid.SetColumn(border25, 0);
		StyledProperty<IBrush?> borderBrushProperty4 = Border.BorderBrushProperty;
		DynamicResourceExtension dynamicResourceExtension8 = new DynamicResourceExtension("SystemControlForegroundBaseMediumBrush");
		context.ProvideTargetProperty = Border.BorderBrushProperty;
		BindingBase binding86 = dynamicResourceExtension8.ProvideValue(context);
		context.ProvideTargetProperty = null;
		border25.Bind(borderBrushProperty4, binding86);
		border25.BorderThickness = new Thickness(1.0, 1.0, 1.0, 1.0);
		context.PopParent();
		((ISupportInitialize)border24).EndInit();
		Controls children13 = grid17.Children;
		Border border27;
		Border border26 = (border27 = new Border());
		((ISupportInitialize)border26).BeginInit();
		children13.Add(border26);
		Border border28 = (border3 = border27);
		context.PushParent(border3);
		Border border29 = border3;
		Grid.SetColumn(border29, 1);
		StyledProperty<IBrush?> borderBrushProperty5 = Border.BorderBrushProperty;
		DynamicResourceExtension dynamicResourceExtension9 = new DynamicResourceExtension("SystemControlForegroundBaseMediumBrush");
		context.ProvideTargetProperty = Border.BorderBrushProperty;
		BindingBase binding87 = dynamicResourceExtension9.ProvideValue(context);
		context.ProvideTargetProperty = null;
		border29.Bind(borderBrushProperty5, binding87);
		border29.BorderThickness = new Thickness(1.0, 1.0, 1.0, 1.0);
		context.PopParent();
		((ISupportInitialize)border28).EndInit();
		context.PopParent();
		((ISupportInitialize)grid16).EndInit();
		context.PopParent();
		((ISupportInitialize)button12).EndInit();
		Controls children14 = stackPanel4.Children;
		Button button15;
		Button button14 = (button15 = new Button());
		((ISupportInitialize)button14).BeginInit();
		children14.Add(button14);
		Button button16 = (button4 = button15);
		context.PushParent(button4);
		Button button17 = button4;
		button17.Width = 28.0;
		button17.Height = 26.0;
		button17.Margin = new Thickness(1.0, 1.0, 1.0, 1.0);
		button17.Tag = "ArrangePanesVertically";
		button17.AddHandler((RoutedEvent)Button.ClickEvent, (Delegate)new EventHandler<RoutedEventArgs>(context.RootObject.OnRunActionCommand), RoutingStrategies.Direct | RoutingStrategies.Bubble, false);
		Grid grid19;
		Grid grid18 = (grid19 = new Grid());
		((ISupportInitialize)grid18).BeginInit();
		button17.Content = grid18;
		Grid grid20 = (grid4 = grid19);
		context.PushParent(grid4);
		Grid grid21 = grid4;
		grid21.Width = 14.0;
		grid21.Height = 14.0;
		grid21.IsHitTestVisible = false;
		grid21.RowDefinitions.Add(new RowDefinition
		{
			Height = new GridLength(1.0, GridUnitType.Star)
		});
		grid21.RowDefinitions.Add(new RowDefinition
		{
			Height = new GridLength(1.0, GridUnitType.Star)
		});
		Controls children15 = grid21.Children;
		Border border31;
		Border border30 = (border31 = new Border());
		((ISupportInitialize)border30).BeginInit();
		children15.Add(border30);
		Border border32 = (border3 = border31);
		context.PushParent(border3);
		Border border33 = border3;
		Grid.SetRow(border33, 0);
		StyledProperty<IBrush?> borderBrushProperty6 = Border.BorderBrushProperty;
		DynamicResourceExtension dynamicResourceExtension10 = new DynamicResourceExtension("SystemControlForegroundBaseMediumBrush");
		context.ProvideTargetProperty = Border.BorderBrushProperty;
		BindingBase binding88 = dynamicResourceExtension10.ProvideValue(context);
		context.ProvideTargetProperty = null;
		border33.Bind(borderBrushProperty6, binding88);
		border33.BorderThickness = new Thickness(1.0, 1.0, 1.0, 1.0);
		context.PopParent();
		((ISupportInitialize)border32).EndInit();
		Controls children16 = grid21.Children;
		Border border35;
		Border border34 = (border35 = new Border());
		((ISupportInitialize)border34).BeginInit();
		children16.Add(border34);
		Border border36 = (border3 = border35);
		context.PushParent(border3);
		Border border37 = border3;
		Grid.SetRow(border37, 1);
		StyledProperty<IBrush?> borderBrushProperty7 = Border.BorderBrushProperty;
		DynamicResourceExtension dynamicResourceExtension11 = new DynamicResourceExtension("SystemControlForegroundBaseMediumBrush");
		context.ProvideTargetProperty = Border.BorderBrushProperty;
		BindingBase binding89 = dynamicResourceExtension11.ProvideValue(context);
		context.ProvideTargetProperty = null;
		border37.Bind(borderBrushProperty7, binding89);
		border37.BorderThickness = new Thickness(1.0, 1.0, 1.0, 1.0);
		context.PopParent();
		((ISupportInitialize)border36).EndInit();
		context.PopParent();
		((ISupportInitialize)grid20).EndInit();
		context.PopParent();
		((ISupportInitialize)button16).EndInit();
		Controls children17 = stackPanel4.Children;
		Button button19;
		Button button18 = (button19 = new Button());
		((ISupportInitialize)button18).BeginInit();
		children17.Add(button18);
		Button button20 = (button4 = button19);
		context.PushParent(button4);
		Button button21 = button4;
		button21.Width = 28.0;
		button21.Height = 26.0;
		button21.Margin = new Thickness(1.0, 1.0, 1.0, 1.0);
		button21.Tag = "ArrangePanesQuad";
		button21.AddHandler((RoutedEvent)Button.ClickEvent, (Delegate)new EventHandler<RoutedEventArgs>(context.RootObject.OnRunActionCommand), RoutingStrategies.Direct | RoutingStrategies.Bubble, false);
		Grid grid23;
		Grid grid22 = (grid23 = new Grid());
		((ISupportInitialize)grid22).BeginInit();
		button21.Content = grid22;
		Grid grid24 = (grid4 = grid23);
		context.PushParent(grid4);
		Grid grid25 = grid4;
		grid25.Width = 14.0;
		grid25.Height = 14.0;
		grid25.IsHitTestVisible = false;
		grid25.RowDefinitions.Add(new RowDefinition
		{
			Height = new GridLength(1.0, GridUnitType.Star)
		});
		grid25.RowDefinitions.Add(new RowDefinition
		{
			Height = new GridLength(1.0, GridUnitType.Star)
		});
		grid25.ColumnDefinitions.Add(new ColumnDefinition
		{
			Width = new GridLength(1.0, GridUnitType.Star)
		});
		grid25.ColumnDefinitions.Add(new ColumnDefinition
		{
			Width = new GridLength(1.0, GridUnitType.Star)
		});
		Controls children18 = grid25.Children;
		Border border39;
		Border border38 = (border39 = new Border());
		((ISupportInitialize)border38).BeginInit();
		children18.Add(border38);
		Border border40 = (border3 = border39);
		context.PushParent(border3);
		Border border41 = border3;
		Grid.SetRow(border41, 0);
		Grid.SetColumn(border41, 0);
		StyledProperty<IBrush?> borderBrushProperty8 = Border.BorderBrushProperty;
		DynamicResourceExtension dynamicResourceExtension12 = new DynamicResourceExtension("SystemControlForegroundBaseMediumBrush");
		context.ProvideTargetProperty = Border.BorderBrushProperty;
		BindingBase binding90 = dynamicResourceExtension12.ProvideValue(context);
		context.ProvideTargetProperty = null;
		border41.Bind(borderBrushProperty8, binding90);
		border41.BorderThickness = new Thickness(1.0, 1.0, 1.0, 1.0);
		context.PopParent();
		((ISupportInitialize)border40).EndInit();
		Controls children19 = grid25.Children;
		Border border43;
		Border border42 = (border43 = new Border());
		((ISupportInitialize)border42).BeginInit();
		children19.Add(border42);
		Border border44 = (border3 = border43);
		context.PushParent(border3);
		Border border45 = border3;
		Grid.SetRow(border45, 0);
		Grid.SetColumn(border45, 1);
		StyledProperty<IBrush?> borderBrushProperty9 = Border.BorderBrushProperty;
		DynamicResourceExtension dynamicResourceExtension13 = new DynamicResourceExtension("SystemControlForegroundBaseMediumBrush");
		context.ProvideTargetProperty = Border.BorderBrushProperty;
		BindingBase binding91 = dynamicResourceExtension13.ProvideValue(context);
		context.ProvideTargetProperty = null;
		border45.Bind(borderBrushProperty9, binding91);
		border45.BorderThickness = new Thickness(1.0, 1.0, 1.0, 1.0);
		context.PopParent();
		((ISupportInitialize)border44).EndInit();
		Controls children20 = grid25.Children;
		Border border47;
		Border border46 = (border47 = new Border());
		((ISupportInitialize)border46).BeginInit();
		children20.Add(border46);
		Border border48 = (border3 = border47);
		context.PushParent(border3);
		Border border49 = border3;
		Grid.SetRow(border49, 1);
		Grid.SetColumn(border49, 0);
		StyledProperty<IBrush?> borderBrushProperty10 = Border.BorderBrushProperty;
		DynamicResourceExtension dynamicResourceExtension14 = new DynamicResourceExtension("SystemControlForegroundBaseMediumBrush");
		context.ProvideTargetProperty = Border.BorderBrushProperty;
		BindingBase binding92 = dynamicResourceExtension14.ProvideValue(context);
		context.ProvideTargetProperty = null;
		border49.Bind(borderBrushProperty10, binding92);
		border49.BorderThickness = new Thickness(1.0, 1.0, 1.0, 1.0);
		context.PopParent();
		((ISupportInitialize)border48).EndInit();
		Controls children21 = grid25.Children;
		Border border51;
		Border border50 = (border51 = new Border());
		((ISupportInitialize)border50).BeginInit();
		children21.Add(border50);
		Border border52 = (border3 = border51);
		context.PushParent(border3);
		Border border53 = border3;
		Grid.SetRow(border53, 1);
		Grid.SetColumn(border53, 1);
		StyledProperty<IBrush?> borderBrushProperty11 = Border.BorderBrushProperty;
		DynamicResourceExtension dynamicResourceExtension15 = new DynamicResourceExtension("SystemControlForegroundBaseMediumBrush");
		context.ProvideTargetProperty = Border.BorderBrushProperty;
		BindingBase binding93 = dynamicResourceExtension15.ProvideValue(context);
		context.ProvideTargetProperty = null;
		border53.Bind(borderBrushProperty11, binding93);
		border53.BorderThickness = new Thickness(1.0, 1.0, 1.0, 1.0);
		context.PopParent();
		((ISupportInitialize)border52).EndInit();
		context.PopParent();
		((ISupportInitialize)grid24).EndInit();
		context.PopParent();
		((ISupportInitialize)button20).EndInit();
		Controls children22 = stackPanel4.Children;
		Button button23;
		Button button22 = (button23 = new Button());
		((ISupportInitialize)button22).BeginInit();
		children22.Add(button22);
		Button button24 = (button4 = button23);
		context.PushParent(button4);
		Button button25 = button4;
		button25.Width = 28.0;
		button25.Height = 26.0;
		button25.Margin = new Thickness(1.0, 1.0, 1.0, 1.0);
		button25.Tag = "ArrangePanesTopTwoBottom";
		button25.AddHandler((RoutedEvent)Button.ClickEvent, (Delegate)new EventHandler<RoutedEventArgs>(context.RootObject.OnRunActionCommand), RoutingStrategies.Direct | RoutingStrategies.Bubble, false);
		Grid grid27;
		Grid grid26 = (grid27 = new Grid());
		((ISupportInitialize)grid26).BeginInit();
		button25.Content = grid26;
		Grid grid28 = (grid4 = grid27);
		context.PushParent(grid4);
		Grid grid29 = grid4;
		grid29.Width = 14.0;
		grid29.Height = 14.0;
		grid29.IsHitTestVisible = false;
		grid29.RowDefinitions.Add(new RowDefinition
		{
			Height = new GridLength(1.0, GridUnitType.Star)
		});
		grid29.RowDefinitions.Add(new RowDefinition
		{
			Height = new GridLength(1.0, GridUnitType.Star)
		});
		grid29.ColumnDefinitions.Add(new ColumnDefinition
		{
			Width = new GridLength(1.0, GridUnitType.Star)
		});
		grid29.ColumnDefinitions.Add(new ColumnDefinition
		{
			Width = new GridLength(1.0, GridUnitType.Star)
		});
		Controls children23 = grid29.Children;
		Border border55;
		Border border54 = (border55 = new Border());
		((ISupportInitialize)border54).BeginInit();
		children23.Add(border54);
		Border border56 = (border3 = border55);
		context.PushParent(border3);
		Border border57 = border3;
		Grid.SetRow(border57, 0);
		Grid.SetColumnSpan(border57, 2);
		StyledProperty<IBrush?> borderBrushProperty12 = Border.BorderBrushProperty;
		DynamicResourceExtension dynamicResourceExtension16 = new DynamicResourceExtension("SystemControlForegroundBaseMediumBrush");
		context.ProvideTargetProperty = Border.BorderBrushProperty;
		BindingBase binding94 = dynamicResourceExtension16.ProvideValue(context);
		context.ProvideTargetProperty = null;
		border57.Bind(borderBrushProperty12, binding94);
		border57.BorderThickness = new Thickness(1.0, 1.0, 1.0, 1.0);
		context.PopParent();
		((ISupportInitialize)border56).EndInit();
		Controls children24 = grid29.Children;
		Border border59;
		Border border58 = (border59 = new Border());
		((ISupportInitialize)border58).BeginInit();
		children24.Add(border58);
		Border border60 = (border3 = border59);
		context.PushParent(border3);
		Border border61 = border3;
		Grid.SetRow(border61, 1);
		Grid.SetColumn(border61, 0);
		StyledProperty<IBrush?> borderBrushProperty13 = Border.BorderBrushProperty;
		DynamicResourceExtension dynamicResourceExtension17 = new DynamicResourceExtension("SystemControlForegroundBaseMediumBrush");
		context.ProvideTargetProperty = Border.BorderBrushProperty;
		BindingBase binding95 = dynamicResourceExtension17.ProvideValue(context);
		context.ProvideTargetProperty = null;
		border61.Bind(borderBrushProperty13, binding95);
		border61.BorderThickness = new Thickness(1.0, 1.0, 1.0, 1.0);
		context.PopParent();
		((ISupportInitialize)border60).EndInit();
		Controls children25 = grid29.Children;
		Border border63;
		Border border62 = (border63 = new Border());
		((ISupportInitialize)border62).BeginInit();
		children25.Add(border62);
		Border border64 = (border3 = border63);
		context.PushParent(border3);
		Border border65 = border3;
		Grid.SetRow(border65, 1);
		Grid.SetColumn(border65, 1);
		StyledProperty<IBrush?> borderBrushProperty14 = Border.BorderBrushProperty;
		DynamicResourceExtension dynamicResourceExtension18 = new DynamicResourceExtension("SystemControlForegroundBaseMediumBrush");
		context.ProvideTargetProperty = Border.BorderBrushProperty;
		BindingBase binding96 = dynamicResourceExtension18.ProvideValue(context);
		context.ProvideTargetProperty = null;
		border65.Bind(borderBrushProperty14, binding96);
		border65.BorderThickness = new Thickness(1.0, 1.0, 1.0, 1.0);
		context.PopParent();
		((ISupportInitialize)border64).EndInit();
		context.PopParent();
		((ISupportInitialize)grid28).EndInit();
		context.PopParent();
		((ISupportInitialize)button24).EndInit();
		Controls children26 = stackPanel4.Children;
		Button button27;
		Button button26 = (button27 = new Button());
		((ISupportInitialize)button26).BeginInit();
		children26.Add(button26);
		Button button28 = (button4 = button27);
		context.PushParent(button4);
		Button button29 = button4;
		button29.Width = 28.0;
		button29.Height = 26.0;
		button29.Margin = new Thickness(1.0, 1.0, 1.0, 1.0);
		button29.Tag = "ArrangePanesTwoTopBottom";
		button29.AddHandler((RoutedEvent)Button.ClickEvent, (Delegate)new EventHandler<RoutedEventArgs>(context.RootObject.OnRunActionCommand), RoutingStrategies.Direct | RoutingStrategies.Bubble, false);
		Grid grid31;
		Grid grid30 = (grid31 = new Grid());
		((ISupportInitialize)grid30).BeginInit();
		button29.Content = grid30;
		Grid grid32 = (grid4 = grid31);
		context.PushParent(grid4);
		Grid grid33 = grid4;
		grid33.Width = 14.0;
		grid33.Height = 14.0;
		grid33.IsHitTestVisible = false;
		grid33.RowDefinitions.Add(new RowDefinition
		{
			Height = new GridLength(1.0, GridUnitType.Star)
		});
		grid33.RowDefinitions.Add(new RowDefinition
		{
			Height = new GridLength(1.0, GridUnitType.Star)
		});
		grid33.ColumnDefinitions.Add(new ColumnDefinition
		{
			Width = new GridLength(1.0, GridUnitType.Star)
		});
		grid33.ColumnDefinitions.Add(new ColumnDefinition
		{
			Width = new GridLength(1.0, GridUnitType.Star)
		});
		Controls children27 = grid33.Children;
		Border border67;
		Border border66 = (border67 = new Border());
		((ISupportInitialize)border66).BeginInit();
		children27.Add(border66);
		Border border68 = (border3 = border67);
		context.PushParent(border3);
		Border border69 = border3;
		Grid.SetRow(border69, 0);
		Grid.SetColumn(border69, 0);
		StyledProperty<IBrush?> borderBrushProperty15 = Border.BorderBrushProperty;
		DynamicResourceExtension dynamicResourceExtension19 = new DynamicResourceExtension("SystemControlForegroundBaseMediumBrush");
		context.ProvideTargetProperty = Border.BorderBrushProperty;
		BindingBase binding97 = dynamicResourceExtension19.ProvideValue(context);
		context.ProvideTargetProperty = null;
		border69.Bind(borderBrushProperty15, binding97);
		border69.BorderThickness = new Thickness(1.0, 1.0, 1.0, 1.0);
		context.PopParent();
		((ISupportInitialize)border68).EndInit();
		Controls children28 = grid33.Children;
		Border border71;
		Border border70 = (border71 = new Border());
		((ISupportInitialize)border70).BeginInit();
		children28.Add(border70);
		Border border72 = (border3 = border71);
		context.PushParent(border3);
		Border border73 = border3;
		Grid.SetRow(border73, 0);
		Grid.SetColumn(border73, 1);
		StyledProperty<IBrush?> borderBrushProperty16 = Border.BorderBrushProperty;
		DynamicResourceExtension dynamicResourceExtension20 = new DynamicResourceExtension("SystemControlForegroundBaseMediumBrush");
		context.ProvideTargetProperty = Border.BorderBrushProperty;
		BindingBase binding98 = dynamicResourceExtension20.ProvideValue(context);
		context.ProvideTargetProperty = null;
		border73.Bind(borderBrushProperty16, binding98);
		border73.BorderThickness = new Thickness(1.0, 1.0, 1.0, 1.0);
		context.PopParent();
		((ISupportInitialize)border72).EndInit();
		Controls children29 = grid33.Children;
		Border border75;
		Border border74 = (border75 = new Border());
		((ISupportInitialize)border74).BeginInit();
		children29.Add(border74);
		Border border76 = (border3 = border75);
		context.PushParent(border3);
		Border border77 = border3;
		Grid.SetRow(border77, 1);
		Grid.SetColumnSpan(border77, 2);
		StyledProperty<IBrush?> borderBrushProperty17 = Border.BorderBrushProperty;
		DynamicResourceExtension dynamicResourceExtension21 = new DynamicResourceExtension("SystemControlForegroundBaseMediumBrush");
		context.ProvideTargetProperty = Border.BorderBrushProperty;
		BindingBase binding99 = dynamicResourceExtension21.ProvideValue(context);
		context.ProvideTargetProperty = null;
		border77.Bind(borderBrushProperty17, binding99);
		border77.BorderThickness = new Thickness(1.0, 1.0, 1.0, 1.0);
		context.PopParent();
		((ISupportInitialize)border76).EndInit();
		context.PopParent();
		((ISupportInitialize)grid32).EndInit();
		context.PopParent();
		((ISupportInitialize)button28).EndInit();
		Controls children30 = stackPanel4.Children;
		Button button31;
		Button button30 = (button31 = new Button());
		((ISupportInitialize)button30).BeginInit();
		children30.Add(button30);
		Button button32 = (button4 = button31);
		context.PushParent(button4);
		Button button33 = button4;
		button33.Width = 28.0;
		button33.Height = 26.0;
		button33.Margin = new Thickness(1.0, 1.0, 1.0, 1.0);
		button33.Tag = "ArrangePanesLeftTwoRight";
		button33.AddHandler((RoutedEvent)Button.ClickEvent, (Delegate)new EventHandler<RoutedEventArgs>(context.RootObject.OnRunActionCommand), RoutingStrategies.Direct | RoutingStrategies.Bubble, false);
		Grid grid35;
		Grid grid34 = (grid35 = new Grid());
		((ISupportInitialize)grid34).BeginInit();
		button33.Content = grid34;
		Grid grid36 = (grid4 = grid35);
		context.PushParent(grid4);
		Grid grid37 = grid4;
		grid37.Width = 14.0;
		grid37.Height = 14.0;
		grid37.IsHitTestVisible = false;
		grid37.ColumnDefinitions.Add(new ColumnDefinition
		{
			Width = new GridLength(1.0, GridUnitType.Star)
		});
		grid37.ColumnDefinitions.Add(new ColumnDefinition
		{
			Width = new GridLength(1.0, GridUnitType.Star)
		});
		grid37.RowDefinitions.Add(new RowDefinition
		{
			Height = new GridLength(1.0, GridUnitType.Star)
		});
		grid37.RowDefinitions.Add(new RowDefinition
		{
			Height = new GridLength(1.0, GridUnitType.Star)
		});
		Controls children31 = grid37.Children;
		Border border79;
		Border border78 = (border79 = new Border());
		((ISupportInitialize)border78).BeginInit();
		children31.Add(border78);
		Border border80 = (border3 = border79);
		context.PushParent(border3);
		Border border81 = border3;
		Grid.SetColumn(border81, 0);
		Grid.SetRowSpan(border81, 2);
		StyledProperty<IBrush?> borderBrushProperty18 = Border.BorderBrushProperty;
		DynamicResourceExtension dynamicResourceExtension22 = new DynamicResourceExtension("SystemControlForegroundBaseMediumBrush");
		context.ProvideTargetProperty = Border.BorderBrushProperty;
		BindingBase binding100 = dynamicResourceExtension22.ProvideValue(context);
		context.ProvideTargetProperty = null;
		border81.Bind(borderBrushProperty18, binding100);
		border81.BorderThickness = new Thickness(1.0, 1.0, 1.0, 1.0);
		context.PopParent();
		((ISupportInitialize)border80).EndInit();
		Controls children32 = grid37.Children;
		Border border83;
		Border border82 = (border83 = new Border());
		((ISupportInitialize)border82).BeginInit();
		children32.Add(border82);
		Border border84 = (border3 = border83);
		context.PushParent(border3);
		Border border85 = border3;
		Grid.SetColumn(border85, 1);
		Grid.SetRow(border85, 0);
		StyledProperty<IBrush?> borderBrushProperty19 = Border.BorderBrushProperty;
		DynamicResourceExtension dynamicResourceExtension23 = new DynamicResourceExtension("SystemControlForegroundBaseMediumBrush");
		context.ProvideTargetProperty = Border.BorderBrushProperty;
		BindingBase binding101 = dynamicResourceExtension23.ProvideValue(context);
		context.ProvideTargetProperty = null;
		border85.Bind(borderBrushProperty19, binding101);
		border85.BorderThickness = new Thickness(1.0, 1.0, 1.0, 1.0);
		context.PopParent();
		((ISupportInitialize)border84).EndInit();
		Controls children33 = grid37.Children;
		Border border87;
		Border border86 = (border87 = new Border());
		((ISupportInitialize)border86).BeginInit();
		children33.Add(border86);
		Border border88 = (border3 = border87);
		context.PushParent(border3);
		Border border89 = border3;
		Grid.SetColumn(border89, 1);
		Grid.SetRow(border89, 1);
		StyledProperty<IBrush?> borderBrushProperty20 = Border.BorderBrushProperty;
		DynamicResourceExtension dynamicResourceExtension24 = new DynamicResourceExtension("SystemControlForegroundBaseMediumBrush");
		context.ProvideTargetProperty = Border.BorderBrushProperty;
		BindingBase binding102 = dynamicResourceExtension24.ProvideValue(context);
		context.ProvideTargetProperty = null;
		border89.Bind(borderBrushProperty20, binding102);
		border89.BorderThickness = new Thickness(1.0, 1.0, 1.0, 1.0);
		context.PopParent();
		((ISupportInitialize)border88).EndInit();
		context.PopParent();
		((ISupportInitialize)grid36).EndInit();
		context.PopParent();
		((ISupportInitialize)button32).EndInit();
		Controls children34 = stackPanel4.Children;
		Button button35;
		Button button34 = (button35 = new Button());
		((ISupportInitialize)button34).BeginInit();
		children34.Add(button34);
		Button button36 = (button4 = button35);
		context.PushParent(button4);
		Button button37 = button4;
		button37.Width = 28.0;
		button37.Height = 26.0;
		button37.Margin = new Thickness(1.0, 1.0, 1.0, 1.0);
		button37.Tag = "ArrangePanesTwoLeftRight";
		button37.AddHandler((RoutedEvent)Button.ClickEvent, (Delegate)new EventHandler<RoutedEventArgs>(context.RootObject.OnRunActionCommand), RoutingStrategies.Direct | RoutingStrategies.Bubble, false);
		Grid grid39;
		Grid grid38 = (grid39 = new Grid());
		((ISupportInitialize)grid38).BeginInit();
		button37.Content = grid38;
		Grid grid40 = (grid4 = grid39);
		context.PushParent(grid4);
		Grid grid41 = grid4;
		grid41.Width = 14.0;
		grid41.Height = 14.0;
		grid41.IsHitTestVisible = false;
		grid41.ColumnDefinitions.Add(new ColumnDefinition
		{
			Width = new GridLength(1.0, GridUnitType.Star)
		});
		grid41.ColumnDefinitions.Add(new ColumnDefinition
		{
			Width = new GridLength(1.0, GridUnitType.Star)
		});
		grid41.RowDefinitions.Add(new RowDefinition
		{
			Height = new GridLength(1.0, GridUnitType.Star)
		});
		grid41.RowDefinitions.Add(new RowDefinition
		{
			Height = new GridLength(1.0, GridUnitType.Star)
		});
		Controls children35 = grid41.Children;
		Border border91;
		Border border90 = (border91 = new Border());
		((ISupportInitialize)border90).BeginInit();
		children35.Add(border90);
		Border border92 = (border3 = border91);
		context.PushParent(border3);
		Border border93 = border3;
		Grid.SetColumn(border93, 0);
		Grid.SetRow(border93, 0);
		StyledProperty<IBrush?> borderBrushProperty21 = Border.BorderBrushProperty;
		DynamicResourceExtension dynamicResourceExtension25 = new DynamicResourceExtension("SystemControlForegroundBaseMediumBrush");
		context.ProvideTargetProperty = Border.BorderBrushProperty;
		BindingBase binding103 = dynamicResourceExtension25.ProvideValue(context);
		context.ProvideTargetProperty = null;
		border93.Bind(borderBrushProperty21, binding103);
		border93.BorderThickness = new Thickness(1.0, 1.0, 1.0, 1.0);
		context.PopParent();
		((ISupportInitialize)border92).EndInit();
		Controls children36 = grid41.Children;
		Border border95;
		Border border94 = (border95 = new Border());
		((ISupportInitialize)border94).BeginInit();
		children36.Add(border94);
		Border border96 = (border3 = border95);
		context.PushParent(border3);
		Border border97 = border3;
		Grid.SetColumn(border97, 0);
		Grid.SetRow(border97, 1);
		StyledProperty<IBrush?> borderBrushProperty22 = Border.BorderBrushProperty;
		DynamicResourceExtension dynamicResourceExtension26 = new DynamicResourceExtension("SystemControlForegroundBaseMediumBrush");
		context.ProvideTargetProperty = Border.BorderBrushProperty;
		BindingBase binding104 = dynamicResourceExtension26.ProvideValue(context);
		context.ProvideTargetProperty = null;
		border97.Bind(borderBrushProperty22, binding104);
		border97.BorderThickness = new Thickness(1.0, 1.0, 1.0, 1.0);
		context.PopParent();
		((ISupportInitialize)border96).EndInit();
		Controls children37 = grid41.Children;
		Border border99;
		Border border98 = (border99 = new Border());
		((ISupportInitialize)border98).BeginInit();
		children37.Add(border98);
		Border border100 = (border3 = border99);
		context.PushParent(border3);
		Border border101 = border3;
		Grid.SetColumn(border101, 1);
		Grid.SetRowSpan(border101, 2);
		StyledProperty<IBrush?> borderBrushProperty23 = Border.BorderBrushProperty;
		DynamicResourceExtension dynamicResourceExtension27 = new DynamicResourceExtension("SystemControlForegroundBaseMediumBrush");
		context.ProvideTargetProperty = Border.BorderBrushProperty;
		BindingBase binding105 = dynamicResourceExtension27.ProvideValue(context);
		context.ProvideTargetProperty = null;
		border101.Bind(borderBrushProperty23, binding105);
		border101.BorderThickness = new Thickness(1.0, 1.0, 1.0, 1.0);
		context.PopParent();
		((ISupportInitialize)border100).EndInit();
		context.PopParent();
		((ISupportInitialize)grid40).EndInit();
		context.PopParent();
		((ISupportInitialize)button36).EndInit();
		Controls children38 = stackPanel4.Children;
		Button button39;
		Button button38 = (button39 = new Button());
		((ISupportInitialize)button38).BeginInit();
		children38.Add(button38);
		Button button40 = (button4 = button39);
		context.PushParent(button4);
		Button button41 = button4;
		button41.Width = 28.0;
		button41.Height = 26.0;
		button41.Margin = new Thickness(1.0, 1.0, 1.0, 1.0);
		button41.Tag = "ArrangePanesThreeColumns";
		button41.AddHandler((RoutedEvent)Button.ClickEvent, (Delegate)new EventHandler<RoutedEventArgs>(context.RootObject.OnRunActionCommand), RoutingStrategies.Direct | RoutingStrategies.Bubble, false);
		Grid grid43;
		Grid grid42 = (grid43 = new Grid());
		((ISupportInitialize)grid42).BeginInit();
		button41.Content = grid42;
		Grid grid44 = (grid4 = grid43);
		context.PushParent(grid4);
		Grid grid45 = grid4;
		grid45.Width = 14.0;
		grid45.Height = 14.0;
		grid45.IsHitTestVisible = false;
		grid45.ColumnDefinitions.Add(new ColumnDefinition
		{
			Width = new GridLength(1.0, GridUnitType.Star)
		});
		grid45.ColumnDefinitions.Add(new ColumnDefinition
		{
			Width = new GridLength(1.0, GridUnitType.Star)
		});
		grid45.ColumnDefinitions.Add(new ColumnDefinition
		{
			Width = new GridLength(1.0, GridUnitType.Star)
		});
		Controls children39 = grid45.Children;
		Border border103;
		Border border102 = (border103 = new Border());
		((ISupportInitialize)border102).BeginInit();
		children39.Add(border102);
		Border border104 = (border3 = border103);
		context.PushParent(border3);
		Border border105 = border3;
		Grid.SetColumn(border105, 0);
		StyledProperty<IBrush?> borderBrushProperty24 = Border.BorderBrushProperty;
		DynamicResourceExtension dynamicResourceExtension28 = new DynamicResourceExtension("SystemControlForegroundBaseMediumBrush");
		context.ProvideTargetProperty = Border.BorderBrushProperty;
		BindingBase binding106 = dynamicResourceExtension28.ProvideValue(context);
		context.ProvideTargetProperty = null;
		border105.Bind(borderBrushProperty24, binding106);
		border105.BorderThickness = new Thickness(1.0, 1.0, 1.0, 1.0);
		context.PopParent();
		((ISupportInitialize)border104).EndInit();
		Controls children40 = grid45.Children;
		Border border107;
		Border border106 = (border107 = new Border());
		((ISupportInitialize)border106).BeginInit();
		children40.Add(border106);
		Border border108 = (border3 = border107);
		context.PushParent(border3);
		Border border109 = border3;
		Grid.SetColumn(border109, 1);
		StyledProperty<IBrush?> borderBrushProperty25 = Border.BorderBrushProperty;
		DynamicResourceExtension dynamicResourceExtension29 = new DynamicResourceExtension("SystemControlForegroundBaseMediumBrush");
		context.ProvideTargetProperty = Border.BorderBrushProperty;
		BindingBase binding107 = dynamicResourceExtension29.ProvideValue(context);
		context.ProvideTargetProperty = null;
		border109.Bind(borderBrushProperty25, binding107);
		border109.BorderThickness = new Thickness(1.0, 1.0, 1.0, 1.0);
		context.PopParent();
		((ISupportInitialize)border108).EndInit();
		Controls children41 = grid45.Children;
		Border border111;
		Border border110 = (border111 = new Border());
		((ISupportInitialize)border110).BeginInit();
		children41.Add(border110);
		Border border112 = (border3 = border111);
		context.PushParent(border3);
		Border border113 = border3;
		Grid.SetColumn(border113, 2);
		StyledProperty<IBrush?> borderBrushProperty26 = Border.BorderBrushProperty;
		DynamicResourceExtension dynamicResourceExtension30 = new DynamicResourceExtension("SystemControlForegroundBaseMediumBrush");
		context.ProvideTargetProperty = Border.BorderBrushProperty;
		BindingBase binding108 = dynamicResourceExtension30.ProvideValue(context);
		context.ProvideTargetProperty = null;
		border113.Bind(borderBrushProperty26, binding108);
		border113.BorderThickness = new Thickness(1.0, 1.0, 1.0, 1.0);
		context.PopParent();
		((ISupportInitialize)border112).EndInit();
		context.PopParent();
		((ISupportInitialize)grid44).EndInit();
		context.PopParent();
		((ISupportInitialize)button40).EndInit();
		Controls children42 = stackPanel4.Children;
		Button button43;
		Button button42 = (button43 = new Button());
		((ISupportInitialize)button42).BeginInit();
		children42.Add(button42);
		Button button44 = (button4 = button43);
		context.PushParent(button4);
		Button button45 = button4;
		button45.Width = 28.0;
		button45.Height = 26.0;
		button45.Margin = new Thickness(1.0, 1.0, 1.0, 1.0);
		button45.Tag = "ArrangePanesThreeRows";
		button45.AddHandler((RoutedEvent)Button.ClickEvent, (Delegate)new EventHandler<RoutedEventArgs>(context.RootObject.OnRunActionCommand), RoutingStrategies.Direct | RoutingStrategies.Bubble, false);
		Grid grid47;
		Grid grid46 = (grid47 = new Grid());
		((ISupportInitialize)grid46).BeginInit();
		button45.Content = grid46;
		Grid grid48 = (grid4 = grid47);
		context.PushParent(grid4);
		Grid grid49 = grid4;
		grid49.Width = 14.0;
		grid49.Height = 14.0;
		grid49.IsHitTestVisible = false;
		grid49.RowDefinitions.Add(new RowDefinition
		{
			Height = new GridLength(1.0, GridUnitType.Star)
		});
		grid49.RowDefinitions.Add(new RowDefinition
		{
			Height = new GridLength(1.0, GridUnitType.Star)
		});
		grid49.RowDefinitions.Add(new RowDefinition
		{
			Height = new GridLength(1.0, GridUnitType.Star)
		});
		Controls children43 = grid49.Children;
		Border border115;
		Border border114 = (border115 = new Border());
		((ISupportInitialize)border114).BeginInit();
		children43.Add(border114);
		Border border116 = (border3 = border115);
		context.PushParent(border3);
		Border border117 = border3;
		Grid.SetRow(border117, 0);
		StyledProperty<IBrush?> borderBrushProperty27 = Border.BorderBrushProperty;
		DynamicResourceExtension dynamicResourceExtension31 = new DynamicResourceExtension("SystemControlForegroundBaseMediumBrush");
		context.ProvideTargetProperty = Border.BorderBrushProperty;
		BindingBase binding109 = dynamicResourceExtension31.ProvideValue(context);
		context.ProvideTargetProperty = null;
		border117.Bind(borderBrushProperty27, binding109);
		border117.BorderThickness = new Thickness(1.0, 1.0, 1.0, 1.0);
		context.PopParent();
		((ISupportInitialize)border116).EndInit();
		Controls children44 = grid49.Children;
		Border border119;
		Border border118 = (border119 = new Border());
		((ISupportInitialize)border118).BeginInit();
		children44.Add(border118);
		Border border120 = (border3 = border119);
		context.PushParent(border3);
		Border border121 = border3;
		Grid.SetRow(border121, 1);
		StyledProperty<IBrush?> borderBrushProperty28 = Border.BorderBrushProperty;
		DynamicResourceExtension dynamicResourceExtension32 = new DynamicResourceExtension("SystemControlForegroundBaseMediumBrush");
		context.ProvideTargetProperty = Border.BorderBrushProperty;
		BindingBase binding110 = dynamicResourceExtension32.ProvideValue(context);
		context.ProvideTargetProperty = null;
		border121.Bind(borderBrushProperty28, binding110);
		border121.BorderThickness = new Thickness(1.0, 1.0, 1.0, 1.0);
		context.PopParent();
		((ISupportInitialize)border120).EndInit();
		Controls children45 = grid49.Children;
		Border border123;
		Border border122 = (border123 = new Border());
		((ISupportInitialize)border122).BeginInit();
		children45.Add(border122);
		Border border124 = (border3 = border123);
		context.PushParent(border3);
		Border border125 = border3;
		Grid.SetRow(border125, 2);
		StyledProperty<IBrush?> borderBrushProperty29 = Border.BorderBrushProperty;
		DynamicResourceExtension dynamicResourceExtension33 = new DynamicResourceExtension("SystemControlForegroundBaseMediumBrush");
		context.ProvideTargetProperty = Border.BorderBrushProperty;
		BindingBase binding111 = dynamicResourceExtension33.ProvideValue(context);
		context.ProvideTargetProperty = null;
		border125.Bind(borderBrushProperty29, binding111);
		border125.BorderThickness = new Thickness(1.0, 1.0, 1.0, 1.0);
		context.PopParent();
		((ISupportInitialize)border124).EndInit();
		context.PopParent();
		((ISupportInitialize)grid48).EndInit();
		context.PopParent();
		((ISupportInitialize)button44).EndInit();
		Controls children46 = stackPanel4.Children;
		Button button47;
		Button button46 = (button47 = new Button());
		((ISupportInitialize)button46).BeginInit();
		children46.Add(button46);
		Button button48 = (button4 = button47);
		context.PushParent(button4);
		Button button49 = button4;
		button49.Width = 28.0;
		button49.Height = 26.0;
		button49.Margin = new Thickness(1.0, 1.0, 1.0, 1.0);
		button49.Tag = "ArrangePanesThreeColumnsWeighted";
		button49.AddHandler((RoutedEvent)Button.ClickEvent, (Delegate)new EventHandler<RoutedEventArgs>(context.RootObject.OnRunActionCommand), RoutingStrategies.Direct | RoutingStrategies.Bubble, false);
		Grid grid51;
		Grid grid50 = (grid51 = new Grid());
		((ISupportInitialize)grid50).BeginInit();
		button49.Content = grid50;
		Grid grid52 = (grid4 = grid51);
		context.PushParent(grid4);
		Grid grid53 = grid4;
		grid53.Width = 14.0;
		grid53.Height = 14.0;
		grid53.IsHitTestVisible = false;
		grid53.ColumnDefinitions.Add(new ColumnDefinition
		{
			Width = new GridLength(1.0, GridUnitType.Star)
		});
		grid53.ColumnDefinitions.Add(new ColumnDefinition
		{
			Width = new GridLength(1.0, GridUnitType.Star)
		});
		grid53.ColumnDefinitions.Add(new ColumnDefinition
		{
			Width = new GridLength(2.0, GridUnitType.Star)
		});
		Controls children47 = grid53.Children;
		Border border127;
		Border border126 = (border127 = new Border());
		((ISupportInitialize)border126).BeginInit();
		children47.Add(border126);
		Border border128 = (border3 = border127);
		context.PushParent(border3);
		Border border129 = border3;
		Grid.SetColumn(border129, 0);
		StyledProperty<IBrush?> borderBrushProperty30 = Border.BorderBrushProperty;
		DynamicResourceExtension dynamicResourceExtension34 = new DynamicResourceExtension("SystemControlForegroundBaseMediumBrush");
		context.ProvideTargetProperty = Border.BorderBrushProperty;
		BindingBase binding112 = dynamicResourceExtension34.ProvideValue(context);
		context.ProvideTargetProperty = null;
		border129.Bind(borderBrushProperty30, binding112);
		border129.BorderThickness = new Thickness(1.0, 1.0, 1.0, 1.0);
		context.PopParent();
		((ISupportInitialize)border128).EndInit();
		Controls children48 = grid53.Children;
		Border border131;
		Border border130 = (border131 = new Border());
		((ISupportInitialize)border130).BeginInit();
		children48.Add(border130);
		Border border132 = (border3 = border131);
		context.PushParent(border3);
		Border border133 = border3;
		Grid.SetColumn(border133, 1);
		StyledProperty<IBrush?> borderBrushProperty31 = Border.BorderBrushProperty;
		DynamicResourceExtension dynamicResourceExtension35 = new DynamicResourceExtension("SystemControlForegroundBaseMediumBrush");
		context.ProvideTargetProperty = Border.BorderBrushProperty;
		BindingBase binding113 = dynamicResourceExtension35.ProvideValue(context);
		context.ProvideTargetProperty = null;
		border133.Bind(borderBrushProperty31, binding113);
		border133.BorderThickness = new Thickness(1.0, 1.0, 1.0, 1.0);
		context.PopParent();
		((ISupportInitialize)border132).EndInit();
		Controls children49 = grid53.Children;
		Border border135;
		Border border134 = (border135 = new Border());
		((ISupportInitialize)border134).BeginInit();
		children49.Add(border134);
		Border border136 = (border3 = border135);
		context.PushParent(border3);
		Border border137 = border3;
		Grid.SetColumn(border137, 2);
		StyledProperty<IBrush?> borderBrushProperty32 = Border.BorderBrushProperty;
		DynamicResourceExtension dynamicResourceExtension36 = new DynamicResourceExtension("SystemControlForegroundBaseMediumBrush");
		context.ProvideTargetProperty = Border.BorderBrushProperty;
		BindingBase binding114 = dynamicResourceExtension36.ProvideValue(context);
		context.ProvideTargetProperty = null;
		border137.Bind(borderBrushProperty32, binding114);
		border137.BorderThickness = new Thickness(1.0, 1.0, 1.0, 1.0);
		context.PopParent();
		((ISupportInitialize)border136).EndInit();
		context.PopParent();
		((ISupportInitialize)grid52).EndInit();
		context.PopParent();
		((ISupportInitialize)button48).EndInit();
		Controls children50 = stackPanel4.Children;
		Button button51;
		Button button50 = (button51 = new Button());
		((ISupportInitialize)button50).BeginInit();
		children50.Add(button50);
		Button button52 = (button4 = button51);
		context.PushParent(button4);
		Button button53 = button4;
		button53.Width = 28.0;
		button53.Height = 26.0;
		button53.Margin = new Thickness(1.0, 1.0, 1.0, 1.0);
		button53.Tag = "ArrangePanesThreeRowsWeighted";
		button53.AddHandler((RoutedEvent)Button.ClickEvent, (Delegate)new EventHandler<RoutedEventArgs>(context.RootObject.OnRunActionCommand), RoutingStrategies.Direct | RoutingStrategies.Bubble, false);
		Grid grid55;
		Grid grid54 = (grid55 = new Grid());
		((ISupportInitialize)grid54).BeginInit();
		button53.Content = grid54;
		Grid grid56 = (grid4 = grid55);
		context.PushParent(grid4);
		Grid grid57 = grid4;
		grid57.Width = 14.0;
		grid57.Height = 14.0;
		grid57.IsHitTestVisible = false;
		grid57.RowDefinitions.Add(new RowDefinition
		{
			Height = new GridLength(1.0, GridUnitType.Star)
		});
		grid57.RowDefinitions.Add(new RowDefinition
		{
			Height = new GridLength(1.0, GridUnitType.Star)
		});
		grid57.RowDefinitions.Add(new RowDefinition
		{
			Height = new GridLength(2.0, GridUnitType.Star)
		});
		Controls children51 = grid57.Children;
		Border border139;
		Border border138 = (border139 = new Border());
		((ISupportInitialize)border138).BeginInit();
		children51.Add(border138);
		Border border140 = (border3 = border139);
		context.PushParent(border3);
		Border border141 = border3;
		Grid.SetRow(border141, 0);
		StyledProperty<IBrush?> borderBrushProperty33 = Border.BorderBrushProperty;
		DynamicResourceExtension dynamicResourceExtension37 = new DynamicResourceExtension("SystemControlForegroundBaseMediumBrush");
		context.ProvideTargetProperty = Border.BorderBrushProperty;
		BindingBase binding115 = dynamicResourceExtension37.ProvideValue(context);
		context.ProvideTargetProperty = null;
		border141.Bind(borderBrushProperty33, binding115);
		border141.BorderThickness = new Thickness(1.0, 1.0, 1.0, 1.0);
		context.PopParent();
		((ISupportInitialize)border140).EndInit();
		Controls children52 = grid57.Children;
		Border border143;
		Border border142 = (border143 = new Border());
		((ISupportInitialize)border142).BeginInit();
		children52.Add(border142);
		Border border144 = (border3 = border143);
		context.PushParent(border3);
		Border border145 = border3;
		Grid.SetRow(border145, 1);
		StyledProperty<IBrush?> borderBrushProperty34 = Border.BorderBrushProperty;
		DynamicResourceExtension dynamicResourceExtension38 = new DynamicResourceExtension("SystemControlForegroundBaseMediumBrush");
		context.ProvideTargetProperty = Border.BorderBrushProperty;
		BindingBase binding116 = dynamicResourceExtension38.ProvideValue(context);
		context.ProvideTargetProperty = null;
		border145.Bind(borderBrushProperty34, binding116);
		border145.BorderThickness = new Thickness(1.0, 1.0, 1.0, 1.0);
		context.PopParent();
		((ISupportInitialize)border144).EndInit();
		Controls children53 = grid57.Children;
		Border border147;
		Border border146 = (border147 = new Border());
		((ISupportInitialize)border146).BeginInit();
		children53.Add(border146);
		Border border148 = (border3 = border147);
		context.PushParent(border3);
		Border border149 = border3;
		Grid.SetRow(border149, 2);
		StyledProperty<IBrush?> borderBrushProperty35 = Border.BorderBrushProperty;
		DynamicResourceExtension dynamicResourceExtension39 = new DynamicResourceExtension("SystemControlForegroundBaseMediumBrush");
		context.ProvideTargetProperty = Border.BorderBrushProperty;
		BindingBase binding117 = dynamicResourceExtension39.ProvideValue(context);
		context.ProvideTargetProperty = null;
		border149.Bind(borderBrushProperty35, binding117);
		border149.BorderThickness = new Thickness(1.0, 1.0, 1.0, 1.0);
		context.PopParent();
		((ISupportInitialize)border148).EndInit();
		context.PopParent();
		((ISupportInitialize)grid56).EndInit();
		context.PopParent();
		((ISupportInitialize)button52).EndInit();
		Controls children54 = stackPanel4.Children;
		Button button55;
		Button button54 = (button55 = new Button());
		((ISupportInitialize)button54).BeginInit();
		children54.Add(button54);
		Button button56 = (button4 = button55);
		context.PushParent(button4);
		Button button57 = button4;
		button57.Width = 28.0;
		button57.Height = 26.0;
		button57.Margin = new Thickness(1.0, 1.0, 1.0, 1.0);
		button57.Tag = "ArrangePanesFourColumns";
		button57.AddHandler((RoutedEvent)Button.ClickEvent, (Delegate)new EventHandler<RoutedEventArgs>(context.RootObject.OnRunActionCommand), RoutingStrategies.Direct | RoutingStrategies.Bubble, false);
		Grid grid59;
		Grid grid58 = (grid59 = new Grid());
		((ISupportInitialize)grid58).BeginInit();
		button57.Content = grid58;
		Grid grid60 = (grid4 = grid59);
		context.PushParent(grid4);
		Grid grid61 = grid4;
		grid61.Width = 14.0;
		grid61.Height = 14.0;
		grid61.IsHitTestVisible = false;
		grid61.ColumnDefinitions.Add(new ColumnDefinition
		{
			Width = new GridLength(1.0, GridUnitType.Star)
		});
		grid61.ColumnDefinitions.Add(new ColumnDefinition
		{
			Width = new GridLength(1.0, GridUnitType.Star)
		});
		grid61.ColumnDefinitions.Add(new ColumnDefinition
		{
			Width = new GridLength(1.0, GridUnitType.Star)
		});
		grid61.ColumnDefinitions.Add(new ColumnDefinition
		{
			Width = new GridLength(1.0, GridUnitType.Star)
		});
		Controls children55 = grid61.Children;
		Border border151;
		Border border150 = (border151 = new Border());
		((ISupportInitialize)border150).BeginInit();
		children55.Add(border150);
		Border border152 = (border3 = border151);
		context.PushParent(border3);
		Border border153 = border3;
		Grid.SetColumn(border153, 0);
		StyledProperty<IBrush?> borderBrushProperty36 = Border.BorderBrushProperty;
		DynamicResourceExtension dynamicResourceExtension40 = new DynamicResourceExtension("SystemControlForegroundBaseMediumBrush");
		context.ProvideTargetProperty = Border.BorderBrushProperty;
		BindingBase binding118 = dynamicResourceExtension40.ProvideValue(context);
		context.ProvideTargetProperty = null;
		border153.Bind(borderBrushProperty36, binding118);
		border153.BorderThickness = new Thickness(1.0, 1.0, 1.0, 1.0);
		context.PopParent();
		((ISupportInitialize)border152).EndInit();
		Controls children56 = grid61.Children;
		Border border155;
		Border border154 = (border155 = new Border());
		((ISupportInitialize)border154).BeginInit();
		children56.Add(border154);
		Border border156 = (border3 = border155);
		context.PushParent(border3);
		Border border157 = border3;
		Grid.SetColumn(border157, 1);
		StyledProperty<IBrush?> borderBrushProperty37 = Border.BorderBrushProperty;
		DynamicResourceExtension dynamicResourceExtension41 = new DynamicResourceExtension("SystemControlForegroundBaseMediumBrush");
		context.ProvideTargetProperty = Border.BorderBrushProperty;
		BindingBase binding119 = dynamicResourceExtension41.ProvideValue(context);
		context.ProvideTargetProperty = null;
		border157.Bind(borderBrushProperty37, binding119);
		border157.BorderThickness = new Thickness(1.0, 1.0, 1.0, 1.0);
		context.PopParent();
		((ISupportInitialize)border156).EndInit();
		Controls children57 = grid61.Children;
		Border border159;
		Border border158 = (border159 = new Border());
		((ISupportInitialize)border158).BeginInit();
		children57.Add(border158);
		Border border160 = (border3 = border159);
		context.PushParent(border3);
		Border border161 = border3;
		Grid.SetColumn(border161, 2);
		StyledProperty<IBrush?> borderBrushProperty38 = Border.BorderBrushProperty;
		DynamicResourceExtension dynamicResourceExtension42 = new DynamicResourceExtension("SystemControlForegroundBaseMediumBrush");
		context.ProvideTargetProperty = Border.BorderBrushProperty;
		BindingBase binding120 = dynamicResourceExtension42.ProvideValue(context);
		context.ProvideTargetProperty = null;
		border161.Bind(borderBrushProperty38, binding120);
		border161.BorderThickness = new Thickness(1.0, 1.0, 1.0, 1.0);
		context.PopParent();
		((ISupportInitialize)border160).EndInit();
		Controls children58 = grid61.Children;
		Border border163;
		Border border162 = (border163 = new Border());
		((ISupportInitialize)border162).BeginInit();
		children58.Add(border162);
		Border border164 = (border3 = border163);
		context.PushParent(border3);
		Border border165 = border3;
		Grid.SetColumn(border165, 3);
		StyledProperty<IBrush?> borderBrushProperty39 = Border.BorderBrushProperty;
		DynamicResourceExtension dynamicResourceExtension43 = new DynamicResourceExtension("SystemControlForegroundBaseMediumBrush");
		context.ProvideTargetProperty = Border.BorderBrushProperty;
		BindingBase binding121 = dynamicResourceExtension43.ProvideValue(context);
		context.ProvideTargetProperty = null;
		border165.Bind(borderBrushProperty39, binding121);
		border165.BorderThickness = new Thickness(1.0, 1.0, 1.0, 1.0);
		context.PopParent();
		((ISupportInitialize)border164).EndInit();
		context.PopParent();
		((ISupportInitialize)grid60).EndInit();
		context.PopParent();
		((ISupportInitialize)button56).EndInit();
		Controls children59 = stackPanel4.Children;
		Button button59;
		Button button58 = (button59 = new Button());
		((ISupportInitialize)button58).BeginInit();
		children59.Add(button58);
		Button button60 = (button4 = button59);
		context.PushParent(button4);
		Button button61 = button4;
		button61.Width = 28.0;
		button61.Height = 26.0;
		button61.Margin = new Thickness(1.0, 1.0, 1.0, 1.0);
		button61.Tag = "ArrangePanesFourRows";
		button61.AddHandler((RoutedEvent)Button.ClickEvent, (Delegate)new EventHandler<RoutedEventArgs>(context.RootObject.OnRunActionCommand), RoutingStrategies.Direct | RoutingStrategies.Bubble, false);
		Grid grid63;
		Grid grid62 = (grid63 = new Grid());
		((ISupportInitialize)grid62).BeginInit();
		button61.Content = grid62;
		Grid grid64 = (grid4 = grid63);
		context.PushParent(grid4);
		Grid grid65 = grid4;
		grid65.Width = 14.0;
		grid65.Height = 14.0;
		grid65.IsHitTestVisible = false;
		grid65.RowDefinitions.Add(new RowDefinition
		{
			Height = new GridLength(1.0, GridUnitType.Star)
		});
		grid65.RowDefinitions.Add(new RowDefinition
		{
			Height = new GridLength(1.0, GridUnitType.Star)
		});
		grid65.RowDefinitions.Add(new RowDefinition
		{
			Height = new GridLength(1.0, GridUnitType.Star)
		});
		grid65.RowDefinitions.Add(new RowDefinition
		{
			Height = new GridLength(1.0, GridUnitType.Star)
		});
		Controls children60 = grid65.Children;
		Border border167;
		Border border166 = (border167 = new Border());
		((ISupportInitialize)border166).BeginInit();
		children60.Add(border166);
		Border border168 = (border3 = border167);
		context.PushParent(border3);
		Border border169 = border3;
		Grid.SetRow(border169, 0);
		StyledProperty<IBrush?> borderBrushProperty40 = Border.BorderBrushProperty;
		DynamicResourceExtension dynamicResourceExtension44 = new DynamicResourceExtension("SystemControlForegroundBaseMediumBrush");
		context.ProvideTargetProperty = Border.BorderBrushProperty;
		BindingBase binding122 = dynamicResourceExtension44.ProvideValue(context);
		context.ProvideTargetProperty = null;
		border169.Bind(borderBrushProperty40, binding122);
		border169.BorderThickness = new Thickness(1.0, 1.0, 1.0, 1.0);
		context.PopParent();
		((ISupportInitialize)border168).EndInit();
		Controls children61 = grid65.Children;
		Border border171;
		Border border170 = (border171 = new Border());
		((ISupportInitialize)border170).BeginInit();
		children61.Add(border170);
		Border border172 = (border3 = border171);
		context.PushParent(border3);
		Border border173 = border3;
		Grid.SetRow(border173, 1);
		StyledProperty<IBrush?> borderBrushProperty41 = Border.BorderBrushProperty;
		DynamicResourceExtension dynamicResourceExtension45 = new DynamicResourceExtension("SystemControlForegroundBaseMediumBrush");
		context.ProvideTargetProperty = Border.BorderBrushProperty;
		BindingBase binding123 = dynamicResourceExtension45.ProvideValue(context);
		context.ProvideTargetProperty = null;
		border173.Bind(borderBrushProperty41, binding123);
		border173.BorderThickness = new Thickness(1.0, 1.0, 1.0, 1.0);
		context.PopParent();
		((ISupportInitialize)border172).EndInit();
		Controls children62 = grid65.Children;
		Border border175;
		Border border174 = (border175 = new Border());
		((ISupportInitialize)border174).BeginInit();
		children62.Add(border174);
		Border border176 = (border3 = border175);
		context.PushParent(border3);
		Border border177 = border3;
		Grid.SetRow(border177, 2);
		StyledProperty<IBrush?> borderBrushProperty42 = Border.BorderBrushProperty;
		DynamicResourceExtension dynamicResourceExtension46 = new DynamicResourceExtension("SystemControlForegroundBaseMediumBrush");
		context.ProvideTargetProperty = Border.BorderBrushProperty;
		BindingBase binding124 = dynamicResourceExtension46.ProvideValue(context);
		context.ProvideTargetProperty = null;
		border177.Bind(borderBrushProperty42, binding124);
		border177.BorderThickness = new Thickness(1.0, 1.0, 1.0, 1.0);
		context.PopParent();
		((ISupportInitialize)border176).EndInit();
		Controls children63 = grid65.Children;
		Border border179;
		Border border178 = (border179 = new Border());
		((ISupportInitialize)border178).BeginInit();
		children63.Add(border178);
		Border border180 = (border3 = border179);
		context.PushParent(border3);
		Border border181 = border3;
		Grid.SetRow(border181, 3);
		StyledProperty<IBrush?> borderBrushProperty43 = Border.BorderBrushProperty;
		DynamicResourceExtension dynamicResourceExtension47 = new DynamicResourceExtension("SystemControlForegroundBaseMediumBrush");
		context.ProvideTargetProperty = Border.BorderBrushProperty;
		BindingBase binding125 = dynamicResourceExtension47.ProvideValue(context);
		context.ProvideTargetProperty = null;
		border181.Bind(borderBrushProperty43, binding125);
		border181.BorderThickness = new Thickness(1.0, 1.0, 1.0, 1.0);
		context.PopParent();
		((ISupportInitialize)border180).EndInit();
		context.PopParent();
		((ISupportInitialize)grid64).EndInit();
		context.PopParent();
		((ISupportInitialize)button60).EndInit();
		context.PopParent();
		((ISupportInitialize)stackPanel3).EndInit();
		context.PopParent();
		((ISupportInitialize)grid3).EndInit();
		Controls children64 = dockPanel4.Children;
		Grid grid67;
		Grid grid66 = (grid67 = new Grid());
		((ISupportInitialize)grid66).BeginInit();
		children64.Add(grid66);
		Grid grid68 = (grid4 = grid67);
		context.PushParent(grid4);
		Grid grid69 = grid4;
		ColumnDefinitions columnDefinitions = grid69.ColumnDefinitions;
		ColumnDefinition columnDefinition;
		ColumnDefinition item79 = (columnDefinition = new ColumnDefinition());
		context.PushParent(columnDefinition);
		ColumnDefinition columnDefinition2 = columnDefinition;
		StyledProperty<GridLength> widthProperty = ColumnDefinition.WidthProperty;
		CompiledBindingExtension compiledBindingExtension79 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EViewModels_002EMainWindowViewModel_002CXFolder_002ESidebarGridLength_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build())
		{
			Mode = BindingMode.TwoWay
		};
		context.ProvideTargetProperty = ColumnDefinition.WidthProperty;
		CompiledBinding binding126 = compiledBindingExtension79.ProvideValue(context);
		context.ProvideTargetProperty = null;
		columnDefinition2.Bind(widthProperty, binding126);
		context.PopParent();
		columnDefinitions.Add(item79);
		ColumnDefinitions columnDefinitions2 = grid69.ColumnDefinitions;
		ColumnDefinition item80 = (columnDefinition = new ColumnDefinition());
		context.PushParent(columnDefinition);
		ColumnDefinition columnDefinition3 = columnDefinition;
		StyledProperty<GridLength> widthProperty2 = ColumnDefinition.WidthProperty;
		CompiledBindingExtension compiledBindingExtension81;
		CompiledBindingExtension compiledBindingExtension80 = (compiledBindingExtension81 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EViewModels_002EMainWindowViewModel_002CXFolder_002EIsPaneOpen_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build()));
		context.PushParent(compiledBindingExtension81);
		StaticResourceExtension staticResourceExtension = new StaticResourceExtension("IsPaneOpenToGridLengthConverter");
		context.ProvideTargetProperty = XamlIlHelpers.Avalonia_002EData_002ECompiledBinding_002CAvalonia_002EBase_002EConverter_0021Property();
		object? obj = staticResourceExtension.ProvideValue(context);
		context.ProvideTargetProperty = null;
		compiledBindingExtension81.Converter = (IValueConverter)obj;
		context.PopParent();
		context.ProvideTargetProperty = ColumnDefinition.WidthProperty;
		CompiledBinding binding127 = compiledBindingExtension80.ProvideValue(context);
		context.ProvideTargetProperty = null;
		columnDefinition3.Bind(widthProperty2, binding127);
		context.PopParent();
		columnDefinitions2.Add(item80);
		grid69.ColumnDefinitions.Add(new ColumnDefinition
		{
			Width = new GridLength(1.0, GridUnitType.Star)
		});
		Controls children65 = grid69.Children;
		Grid grid71;
		Grid grid70 = (grid71 = new Grid());
		((ISupportInitialize)grid70).BeginInit();
		children65.Add(grid70);
		Grid grid72 = (grid4 = grid71);
		context.PushParent(grid4);
		Grid grid73 = grid4;
		Grid.SetColumn(grid73, 0);
		StyledProperty<bool> isVisibleProperty = Visual.IsVisibleProperty;
		CompiledBindingExtension compiledBindingExtension82 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EViewModels_002EMainWindowViewModel_002CXFolder_002EIsPaneOpen_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build());
		context.ProvideTargetProperty = Visual.IsVisibleProperty;
		CompiledBinding binding128 = compiledBindingExtension82.ProvideValue(context);
		context.ProvideTargetProperty = null;
		grid73.Bind(isVisibleProperty, binding128);
		Controls children66 = grid73.Children;
		SidebarView sidebarView2;
		SidebarView sidebarView = (sidebarView2 = new SidebarView());
		((ISupportInitialize)sidebarView).BeginInit();
		children66.Add(sidebarView);
		((ISupportInitialize)sidebarView2).EndInit();
		context.PopParent();
		((ISupportInitialize)grid72).EndInit();
		Controls children67 = grid69.Children;
		GridSplitter gridSplitter2;
		GridSplitter gridSplitter = (gridSplitter2 = new GridSplitter());
		((ISupportInitialize)gridSplitter).BeginInit();
		children67.Add(gridSplitter);
		GridSplitter gridSplitter4;
		GridSplitter gridSplitter3 = (gridSplitter4 = gridSplitter2);
		context.PushParent(gridSplitter4);
		Grid.SetColumn(gridSplitter4, 1);
		gridSplitter4.HorizontalAlignment = HorizontalAlignment.Stretch;
		gridSplitter4.VerticalAlignment = VerticalAlignment.Stretch;
		StyledProperty<IBrush?> backgroundProperty5 = TemplatedControl.BackgroundProperty;
		DynamicResourceExtension dynamicResourceExtension48 = new DynamicResourceExtension("TopToolbarBackground");
		context.ProvideTargetProperty = TemplatedControl.BackgroundProperty;
		BindingBase binding129 = dynamicResourceExtension48.ProvideValue(context);
		context.ProvideTargetProperty = null;
		gridSplitter4.Bind(backgroundProperty5, binding129);
		gridSplitter4.Width = 1.0;
		gridSplitter4.Opacity = 0.5;
		gridSplitter4.AddHandler(InputElement.PointerEnteredEvent, context.RootObject.OnSidebarSplitterPointerEntered);
		gridSplitter4.AddHandler(InputElement.PointerExitedEvent, context.RootObject.OnSidebarSplitterPointerExited);
		gridSplitter4.AddHandler(Thumb.DragStartedEvent, context.RootObject.OnSidebarSplitterDragStarted);
		gridSplitter4.AddHandler(Thumb.DragCompletedEvent, context.RootObject.OnSidebarSplitterDragCompleted);
		StyledProperty<bool> isVisibleProperty2 = Visual.IsVisibleProperty;
		CompiledBindingExtension compiledBindingExtension83 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EViewModels_002EMainWindowViewModel_002CXFolder_002EIsPaneOpen_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build());
		context.ProvideTargetProperty = Visual.IsVisibleProperty;
		CompiledBinding binding130 = compiledBindingExtension83.ProvideValue(context);
		context.ProvideTargetProperty = null;
		gridSplitter4.Bind(isVisibleProperty2, binding130);
		context.PopParent();
		((ISupportInitialize)gridSplitter3).EndInit();
		Controls children68 = grid69.Children;
		Grid grid75;
		Grid grid74 = (grid75 = new Grid());
		((ISupportInitialize)grid74).BeginInit();
		children68.Add(grid74);
		Grid grid76 = (grid4 = grid75);
		context.PushParent(grid4);
		Grid grid77 = grid4;
		Grid.SetColumn(grid77, 2);
		grid77.Margin = new Thickness(1.0, 1.0, 1.0, 1.0);
		Controls children69 = grid77.Children;
		TransitioningContentControl transitioningContentControl2;
		TransitioningContentControl transitioningContentControl = (transitioningContentControl2 = new TransitioningContentControl());
		((ISupportInitialize)transitioningContentControl).BeginInit();
		children69.Add(transitioningContentControl);
		TransitioningContentControl transitioningContentControl4;
		TransitioningContentControl transitioningContentControl3 = (transitioningContentControl4 = transitioningContentControl2);
		context.PushParent(transitioningContentControl4);
		CompiledBindingExtension compiledBindingExtension84 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EViewModels_002EMainWindowViewModel_002CXFolder_002EPanesView_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build());
		context.ProvideTargetProperty = ContentControl.ContentProperty;
		CompiledBinding compiledBinding = compiledBindingExtension84.ProvideValue(context);
		context.ProvideTargetProperty = null;
		XamlDynamicSetters._003C_003EXamlDynamicSetter_2(transitioningContentControl4, compiledBinding);
		context.PopParent();
		((ISupportInitialize)transitioningContentControl3).EndInit();
		context.PopParent();
		((ISupportInitialize)grid76).EndInit();
		context.PopParent();
		((ISupportInitialize)grid68).EndInit();
		context.PopParent();
		((ISupportInitialize)dockPanel3).EndInit();
		context.PopParent();
		((ISupportInitialize)dialogHost3).EndInit();
		context.PopParent();
		((ISupportInitialize)P_1).EndInit();
		if (P_1 is StyledElement styled)
		{
			NameScope.SetNameScope(styled, context.AvaloniaNameScope);
		}
		context.AvaloniaNameScope.Complete();
	}

	[CompilerGenerated]
	private static void _0021XamlIlPopulateTrampoline(MainWindow P_0)
	{
		if (_0021XamlIlPopulateOverride != null)
		{
			_0021XamlIlPopulateOverride(P_0);
		}
		else
		{
			_0021XamlIlPopulate(XamlIlRuntimeHelpers.CreateRootServiceProviderV3(null), P_0);
		}
	}
}
