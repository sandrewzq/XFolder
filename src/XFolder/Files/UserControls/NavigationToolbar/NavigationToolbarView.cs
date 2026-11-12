using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Shapes;
using Avalonia.Data;
using Avalonia.Data.Converters;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Markup.Xaml;
using Avalonia.Markup.Xaml.MarkupExtensions;
using Avalonia.Markup.Xaml.MarkupExtensions.CompiledBindings;
using Avalonia.Markup.Xaml.XamlIl.Runtime;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Media.Immutable;
using Avalonia.Platform;
using Avalonia.Styling;
using Avalonia.Threading;
using Avalonia.VisualTree;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using CompiledAvaloniaXaml;
using Files.Assets.Lang;
using Files.Converters;
using Files.Data.Commands.Manager;
using Files.Data.Contexts;
using Files.Data.Contracts;
using Files.Data.Enums;
using Files.Data.Enums.Navigation;
using Files.Data.Items;
using Files.Data.Models;
using Files.Helpers.ContextFlyouts;
using Files.Helpers.Shared;
using Files.UserControls.BreadcrumbBar;

namespace Files.UserControls.NavigationToolbar;

public class NavigationToolbarView : UserControl
{
	private sealed class BreadcrumbDirectoryMenuState(string path)
	{
		public string Path { get; } = path;

		public bool ChildrenLoading { get; set; }

		public bool ChildrenLoaded { get; set; }

		public bool HasChildren { get; set; }
	}

	[CompilerGenerated]
	private class XamlClosure_4
	{
		public static object Build_1(IServiceProvider P_0)
		{
			XamlIlContext.Context<NavigationToolbarView> context = CreateContext(P_0);
			return new FontSizeToIconSizeConverter();
		}

		public static XamlIlContext.Context<NavigationToolbarView> CreateContext(IServiceProvider P_0)
		{
			XamlIlContext.Context<NavigationToolbarView> context = new XamlIlContext.Context<NavigationToolbarView>(P_0, new object[1] { _0021AvaloniaResources.NamespaceInfo_003A_002FUserControls_002FNavigationToolbar_002FNavigationToolbarView_002Eaxaml.Singleton }, "avares://XFolder/UserControls/NavigationToolbar/NavigationToolbarView.axaml");
			if (P_0 != null)
			{
				object service = P_0.GetService(typeof(IRootObjectProvider));
				if (service != null)
				{
					service = ((IRootObjectProvider)service).RootObject;
					context.RootObject = (NavigationToolbarView)service;
				}
			}
			return context;
		}
	}

	private static readonly ICommandManager Commands = Ioc.Default.GetRequiredService<ICommandManager>();

	private static Bitmap? s_breadcrumbDirectoryIcon;

	private ContextMenu? _activeBreadcrumbContextMenu;

	private ContextMenu? _activeBreadcrumbDirectoryMenu;

	private NavigationToolbarViewModel? _vm;

	[GeneratedCode("Avalonia.Generators.NameGenerator.InitializeComponentCodeGenerator", "12.0.1.0")]
	internal Border BreadcrumbContainer;

	[GeneratedCode("Avalonia.Generators.NameGenerator.InitializeComponentCodeGenerator", "12.0.1.0")]
	internal Files.UserControls.BreadcrumbBar.BreadcrumbBar PathBreadcrumb;

	[GeneratedCode("Avalonia.Generators.NameGenerator.InitializeComponentCodeGenerator", "12.0.1.0")]
	internal TextBox tbWorkingDirectory;

	[CompilerGenerated]
	private static Action<object> _0021XamlIlPopulateOverride;

	public NavigationToolbarViewModel? ViewModel
	{
		get
		{
			if (_vm == null)
			{
				_vm = DataContext as NavigationToolbarViewModel;
			}
			return _vm;
		}
	}

	public NavigationToolbarView()
	{
		InitializeComponent();
		DataContextChanged += NavigationToolbarView_DataContextChanged;
		DetachedFromVisualTree += NavigationToolbarView_DetachedFromVisualTree;
		UpdateViewModelSubscription(null, DataContext as NavigationToolbarViewModel);
	}

	private void NavigationToolbarView_DataContextChanged(object? sender, EventArgs e)
	{
		NavigationToolbarViewModel vm = _vm;
		_vm = DataContext as NavigationToolbarViewModel;
		UpdateViewModelSubscription(vm, _vm);
	}

	private void NavigationToolbarView_DetachedFromVisualTree(object? sender, VisualTreeAttachmentEventArgs e)
	{
		UpdateViewModelSubscription(_vm, null);
		CloseBreadcrumbContextMenu();
		CloseBreadcrumbDirectoryMenu();
	}

	private void UpdateViewModelSubscription(NavigationToolbarViewModel? oldViewModel, NavigationToolbarViewModel? newViewModel)
	{
		if (oldViewModel != newViewModel)
		{
			if (oldViewModel != null)
			{
				oldViewModel.PathModeRequested -= ViewModel_PathModeRequested;
			}
			if (newViewModel != null)
			{
				newViewModel.PathModeRequested += ViewModel_PathModeRequested;
			}
		}
	}

	private void ViewModel_PathModeRequested(object? sender, EventArgs e)
	{
		SwitchToPathMode();
	}

	private void SwitchToPathMode()
	{
		if (ViewModel != null)
		{
			ViewModel.IsFocused = true;
			ViewModel.Path2Go = string.Empty;
			Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(() =>
			{
				tbWorkingDirectory.Focus();
				tbWorkingDirectory.SelectAll();
			}, DispatcherPriority.Background);
		}
	}

	private void OnGoClick(object? sender, TappedEventArgs e)
	{
		string newPath = string.Empty;
		if (!ViewModel.IsFocused && !string.IsNullOrEmpty(ViewModel.Path2Go))
		{
			newPath = PathHelpers.NormalizePath(ViewModel.Path2Go);
			ViewModel.Path2Go = string.Empty;
		}
		tbWorkingDirectory.Text = ViewModel.PathControlDisplayText;
		Ioc.Default.GetRequiredService<IMultiPanesContext>().ActivePaneViewModel?.SetWorkingDirectory(newPath, SetNavPathLocationEnum.EnterAddress);
	}

	private void TbWorkingDirectory_OnGotFocus(object? sender, FocusChangedEventArgs e)
	{
		TextBox textBox = sender as TextBox;
		if (textBox != null)
		{
			ViewModel.IsFocused = true;
			ViewModel.Path2Go = string.Empty;
			Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(() =>
			{
				textBox.SelectAll();
			}, DispatcherPriority.Background);
		}
	}

	private void TbWorkingDirectory_OnLostFocus(object? sender, RoutedEventArgs e)
	{
		if (sender is TextBox textBox)
		{
			ViewModel.IsFocused = false;
			ViewModel.Path2Go = textBox.Text;
			textBox.Text = ViewModel?.PathControlDisplayText ?? string.Empty;
		}
	}

	private void TbWorkingDirectory_OnKeyDown(object? sender, KeyEventArgs e)
	{
		if (e.Key == Key.Return)
		{
			if (sender is TextBox { Text: var text } textBox)
			{
				string text2 = text?.Trim() ?? string.Empty;
				if (ViewModel != null)
				{
					ViewModel.IsFocused = false;
					ViewModel.Path2Go = text2;
				}
				(textBox.Parent as Control)?.Focus();
				if (!string.IsNullOrEmpty(text2))
				{
					string newPath = PathHelpers.NormalizePath(text2);
					Ioc.Default.GetRequiredService<IMultiPanesContext>().ActivePaneViewModel?.SetWorkingDirectory(newPath, SetNavPathLocationEnum.EnterAddress);
				}
			}
			e.Handled = true;
		}
		else if (e.Key == Key.Escape)
		{
			if (sender is TextBox textBox2)
			{
				ViewModel.IsFocused = false;
				ViewModel.Path2Go = string.Empty;
				textBox2.Text = ViewModel?.PathControlDisplayText ?? string.Empty;
				(textBox2.Parent as Control)?.Focus();
			}
			e.Handled = true;
		}
	}

	private void BreadcrumbContainer_OnPointerPressed(object? sender, PointerPressedEventArgs e)
	{
		if (e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
		{
			SwitchToPathMode();
			e.Handled = true;
		}
	}

	private void PathBreadcrumb_OnItemClicked(object? sender, PathBoxItem item)
	{
		if (item != null && item.Path != null)
		{
			Ioc.Default.GetRequiredService<IMultiPanesContext>().ActivePaneViewModel?.SetWorkingDirectory(item.Path, SetNavPathLocationEnum.BreadcrumbClick);
		}
	}

	private void PathBreadcrumb_OnItemContextRequested(object? sender, BreadcrumbContextRequestedEventArgs e)
	{
		if (!string.IsNullOrWhiteSpace(e.Item.Path))
		{
			List<ContextMenuFlyoutItemViewModel> list = BuildBreadcrumbContextMenuItems(e.Item);
			if (list.Count != 0)
			{
				CloseBreadcrumbContextMenu();
				CloseBreadcrumbDirectoryMenu();
				ContextMenu contextMenu = (_activeBreadcrumbContextMenu = new ContextMenu
				{
					ItemsSource = ContextFlyoutModelToElementHelper.ToContextMenuItemsSource(list)
				});
				contextMenu.Closed += BreadcrumbContextMenu_OnClosed;
				contextMenu.Open(e.TargetControl);
			}
		}
	}

	private async void PathBreadcrumb_OnChevronClicked(object? sender, BreadcrumbChevronClickedEventArgs e)
	{
		if (!string.IsNullOrWhiteSpace(e.Item.Path))
		{
			List<MenuItem> list = await BuildBreadcrumbDirectoryMenuItemsAsync(e.Item.Path);
			if (list.Count != 0)
			{
				CloseBreadcrumbContextMenu();
				CloseBreadcrumbDirectoryMenu();
				ContextMenu contextMenu = (_activeBreadcrumbDirectoryMenu = new ContextMenu
				{
					ItemsSource = list
				});
				contextMenu.Closed += BreadcrumbDirectoryMenu_OnClosed;
				contextMenu.Open(e.TargetControl);
				PreloadChildrenForMenuItemsAsync(list);
			}
		}
	}

	private async Task<List<MenuItem>> BuildBreadcrumbDirectoryMenuItemsAsync(string path)
	{
		return (await Task.Run(() => GetSubDirectories(path))).Select(CreateBreadcrumbDirectoryMenuItem).ToList();
	}

	private MenuItem CreateBreadcrumbDirectoryMenuItem(DirectoryInfo directory)
	{
		bool isHidden = PathHelpers.IsHiddenDirectory(directory);
		MenuItem menuItem = new MenuItem();
		menuItem.Header = CreateBreadcrumbDirectoryHeader(directory.Name, IsBreadcrumbDirectoryInCurrentPathChain(directory.FullName), isHidden);
		menuItem.Icon = CreateBreadcrumbDirectoryIcon(isHidden);
		menuItem.Tag = new BreadcrumbDirectoryMenuState(directory.FullName);
		menuItem.AddHandler(InputElement.PointerReleasedEvent, BreadcrumbDirectoryMenuItem_OnPointerReleased, RoutingStrategies.Tunnel, handledEventsToo: true);
		menuItem.PointerEntered += BreadcrumbDirectoryMenuItem_OnPointerEntered;
		return menuItem;
	}

	private static object CreateBreadcrumbDirectoryHeader(string name, bool isCurrentPathAncestor, bool isHidden)
	{
		if (isCurrentPathAncestor | isHidden)
		{
			TextBlock textBlock = new TextBlock
			{
				Text = name,
				FontWeight = (isCurrentPathAncestor ? FontWeight.Bold : FontWeight.Normal)
			};
			if (isHidden)
			{
				textBlock.Opacity = 0.45;
			}
			return textBlock;
		}
		return name;
	}

	private bool IsBreadcrumbDirectoryInCurrentPathChain(string directoryPath)
	{
		string text = ViewModel?.PathControlDisplayText;
		if (string.IsNullOrWhiteSpace(text))
		{
			return false;
		}
		return PathHelpers.IsPathInCurrentPathChain(directoryPath, text);
	}

	private static Image CreateBreadcrumbDirectoryIcon(bool isHidden)
	{
		if (s_breadcrumbDirectoryIcon == null)
		{
			using Stream stream = AssetLoader.Open(new Uri("avares://XFolder/Assets/image/folder.png"));
			s_breadcrumbDirectoryIcon = new Bitmap(stream);
		}
		Image image = new Image
		{
			Source = s_breadcrumbDirectoryIcon,
			Width = 16.0,
			Height = 16.0
		};
		if (isHidden)
		{
			image.Opacity = 0.45;
		}
		return image;
	}

	private async void BreadcrumbDirectoryMenuItem_OnPointerEntered(object? sender, PointerEventArgs e)
	{
		if (!(sender is MenuItem menuItem))
		{
			return;
		}
		object tag = menuItem.Tag;
		BreadcrumbDirectoryMenuState state = tag as BreadcrumbDirectoryMenuState;
		if (state == null)
		{
			return;
		}
		if (state.ChildrenLoaded)
		{
			if (state.HasChildren)
			{
				menuItem.IsSubMenuOpen = true;
				PreloadChildrenForMenuItemsAsync(GetBreadcrumbSubMenuItems(menuItem));
			}
		}
		else
		{
			if (state.ChildrenLoading)
			{
				return;
			}
			await Task.Delay(150);
			if (!menuItem.IsPointerOver || state.ChildrenLoaded || state.ChildrenLoading)
			{
				return;
			}
			state.ChildrenLoading = true;
			List<DirectoryInfo> list = await Task.Run(() => GetSubDirectories(state.Path));
			state.ChildrenLoading = false;
			state.ChildrenLoaded = true;
			state.HasChildren = list.Count > 0;
			if (state.HasChildren)
			{
				List<MenuItem> menuItems = (List<MenuItem>)(menuItem.ItemsSource = CreateBreadcrumbSubMenuItems(list));
				if (menuItem.IsPointerOver)
				{
					menuItem.IsSubMenuOpen = true;
				}
				PreloadChildrenForMenuItemsAsync(menuItems);
			}
		}
	}

	private void BreadcrumbDirectoryMenuItem_OnPointerReleased(object? sender, PointerReleasedEventArgs e)
	{
		if (sender is MenuItem { Tag: BreadcrumbDirectoryMenuState tag } menuItem && e.InitialPressMouseButton == MouseButton.Left && IsBreadcrumbMenuEventFromCurrentItem(menuItem, e.Source))
		{
			e.Handled = true;
			CloseBreadcrumbDirectoryMenu();
			NavigateToBreadcrumbPath(tag.Path);
		}
	}

	private List<MenuItem> CreateBreadcrumbSubMenuItems(List<DirectoryInfo> childDirectories)
	{
		return childDirectories.Select(CreateBreadcrumbDirectoryMenuItem).ToList();
	}

	private async Task PreloadChildrenForMenuItemsAsync(List<MenuItem> menuItems)
	{
		await Task.WhenAll(((IEnumerable<MenuItem>)menuItems).Select((Func<MenuItem, Task>)(async (MenuItem mi) =>
		{
			object tag = mi.Tag;
			BreadcrumbDirectoryMenuState s = tag as BreadcrumbDirectoryMenuState;
			if (s != null && !s.ChildrenLoaded && !s.ChildrenLoading)
			{
				s.ChildrenLoading = true;
				List<DirectoryInfo> list = await Task.Run(() => GetSubDirectories(s.Path));
				s.ChildrenLoading = false;
				s.ChildrenLoaded = true;
				s.HasChildren = list.Count > 0;
				if (s.HasChildren)
				{
					mi.ItemsSource = CreateBreadcrumbSubMenuItems(list);
				}
			}
		})));
	}

	private static List<MenuItem> GetBreadcrumbSubMenuItems(MenuItem menuItem)
	{
		if (!(menuItem.ItemsSource is IEnumerable<MenuItem> source))
		{
			return new List<MenuItem>();
		}
		return source.ToList();
	}

	private static List<DirectoryInfo> GetSubDirectories(string path)
	{
		try
		{
			bool showHiddenItems = Ioc.Default.GetRequiredService<IFoldersSettingsService>().ShowHiddenItems;
			EnumerationOptions enumerationOptions = new EnumerationOptions
			{
				IgnoreInaccessible = true,
				AttributesToSkip = (showHiddenItems ? FileAttributes.System : (FileAttributes.Hidden | FileAttributes.System))
			};
			return (from d in Directory.EnumerateDirectories(path, "*", enumerationOptions)
				select new DirectoryInfo(d)).ToList();
		}
		catch
		{
			return new List<DirectoryInfo>();
		}
	}

	private static void NavigateToBreadcrumbPath(string path)
	{
		Ioc.Default.GetRequiredService<IMultiPanesContext>().ActivePaneViewModel?.SetWorkingDirectory(path, SetNavPathLocationEnum.BreadcrumbClick);
	}

	private void PathBreadcrumb_OnBlankAreaClicked(object? sender, EventArgs e)
	{
		SwitchToPathMode();
	}

	private List<ContextMenuFlyoutItemViewModel> BuildBreadcrumbContextMenuItems(PathBoxItem item)
	{
		List<ContextMenuFlyoutItemViewModel> list = new List<ContextMenuFlyoutItemViewModel>();
		bool flag = !string.IsNullOrWhiteSpace(item.Path);
		bool flag2 = CanPinBreadcrumbPath(item.Path);
		if (flag)
		{
			list.Add(new ContextMenuFlyoutItemViewModel
			{
				Text = Files.Assets.Lang.Resources.CopyPath,
				Command = new AsyncRelayCommand(() => ExecuteBreadcrumbCommandAsync(CommandCodes.CopyPath, item.Path))
			});
			list.Add(new ContextMenuFlyoutItemViewModel
			{
				Text = Files.Assets.Lang.Resources.CopyPathWithQuotes,
				Command = new AsyncRelayCommand(() => ExecuteBreadcrumbCommandAsync(CommandCodes.CopyPathWithQuotes, item.Path))
			});
			list.Add(CreateSeparatorMenuItem());
			if (flag2)
			{
				list.Add(new ContextMenuFlyoutItemViewModel
				{
					Text = Commands[CommandCodes.PinToSidebar].Label,
					Command = Commands[CommandCodes.PinToSidebar],
					CommandParameter = item.Path
				});
			}
			list.Add(new ContextMenuFlyoutItemViewModel
			{
				Text = Files.Assets.Lang.Resources.OpenFinder,
				Command = new AsyncRelayCommand(() => ExecuteBreadcrumbCommandAsync(CommandCodes.OpenFinder, item.Path))
			});
			list.Add(new ContextMenuFlyoutItemViewModel
			{
				Text = Files.Assets.Lang.Resources.OpenTerminal,
				Command = new AsyncRelayCommand(() => ExecuteBreadcrumbCommandAsync(CommandCodes.OpenTerminal, item.Path))
			});
			list.Add(CreateSeparatorMenuItem());
		}
		list.Add(new ContextMenuFlyoutItemViewModel
		{
			Text = Files.Assets.Lang.Resources.EditPath,
			Command = new RelayCommand(SwitchToPathMode)
		});
		return list;
	}

	private static Task ExecuteBreadcrumbCommandAsync(CommandCodes commandCode, string? path)
	{
		if (string.IsNullOrWhiteSpace(path))
		{
			return Task.CompletedTask;
		}
		return Commands[commandCode].ExecuteAsync(path);
	}

	private static bool CanPinBreadcrumbPath(string? path)
	{
		if (string.IsNullOrWhiteSpace(path))
		{
			return false;
		}
		path = PathHelpers.NormalizePath(path);
		if (PathHelpers.IsRootPath(path) || !Directory.Exists(path))
		{
			return false;
		}
		return !Ioc.Default.GetRequiredService<IQuickAccessService>().IsPinnedToQuickAccess(path);
	}

	private static ContextMenuFlyoutItemViewModel CreateSeparatorMenuItem()
	{
		return new ContextMenuFlyoutItemViewModel
		{
			ItemType = ContextMenuFlyoutItemType.Separator
		};
	}

	private void BreadcrumbContextMenu_OnClosed(object? sender, RoutedEventArgs e)
	{
		if (sender is ContextMenu contextMenu)
		{
			contextMenu.Closed -= BreadcrumbContextMenu_OnClosed;
		}
		if (_activeBreadcrumbContextMenu == sender)
		{
			_activeBreadcrumbContextMenu = null;
		}
	}

	private void CloseBreadcrumbContextMenu()
	{
		if (_activeBreadcrumbContextMenu != null)
		{
			ContextMenu? activeBreadcrumbContextMenu = _activeBreadcrumbContextMenu;
			_activeBreadcrumbContextMenu = null;
			activeBreadcrumbContextMenu.Closed -= BreadcrumbContextMenu_OnClosed;
			activeBreadcrumbContextMenu.Close();
		}
	}

	private void BreadcrumbDirectoryMenu_OnClosed(object? sender, RoutedEventArgs e)
	{
		if (sender is ContextMenu contextMenu)
		{
			contextMenu.Closed -= BreadcrumbDirectoryMenu_OnClosed;
		}
		if (_activeBreadcrumbDirectoryMenu == sender)
		{
			_activeBreadcrumbDirectoryMenu = null;
		}
	}

	private void CloseBreadcrumbDirectoryMenu()
	{
		if (_activeBreadcrumbDirectoryMenu != null)
		{
			ContextMenu? activeBreadcrumbDirectoryMenu = _activeBreadcrumbDirectoryMenu;
			_activeBreadcrumbDirectoryMenu = null;
			activeBreadcrumbDirectoryMenu.Closed -= BreadcrumbDirectoryMenu_OnClosed;
			activeBreadcrumbDirectoryMenu.Close();
		}
	}

	private static bool IsBreadcrumbMenuEventFromCurrentItem(MenuItem menuItem, object? source)
	{
		if (!(source is Visual visual))
		{
			return false;
		}
		return ((visual as MenuItem) ?? visual.FindAncestorOfType<MenuItem>()) == menuItem;
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
		BreadcrumbContainer = ((nameScope != null) ? nameScope.Find<Border>("BreadcrumbContainer") : null);
		PathBreadcrumb = ((nameScope != null) ? nameScope.Find<Files.UserControls.BreadcrumbBar.BreadcrumbBar>("PathBreadcrumb") : null);
		tbWorkingDirectory = ((nameScope != null) ? nameScope.Find<TextBox>("tbWorkingDirectory") : null);
	}

	[CompilerGenerated]
	private unsafe static void _0021XamlIlPopulate(IServiceProvider P_0, NavigationToolbarView P_1)
	{
		XamlIlContext.Context<NavigationToolbarView> context = new XamlIlContext.Context<NavigationToolbarView>(P_0, new object[1] { _0021AvaloniaResources.NamespaceInfo_003A_002FUserControls_002FNavigationToolbar_002FNavigationToolbarView_002Eaxaml.Singleton }, "avares://XFolder/UserControls/NavigationToolbar/NavigationToolbarView.axaml")
		{
			RootObject = P_1,
			IntermediateRoot = P_1
		};
		((ISupportInitialize)P_1).BeginInit();
		context.PushParent(P_1);
		((ResourceDictionary)P_1.Resources).AddDeferred((object)"FontSizeToIconSizeConverter", XamlIlRuntimeHelpers.DeferredTransformationFactoryV3<object>((nint)(delegate*<IServiceProvider, object>)(&XamlClosure_4.Build_1), context));
		P_1.DataContext = new NavigationToolbarViewModel();
		Styles styles = P_1.Styles;
		Style style;
		Style item = (style = new Style());
		context.PushParent(style);
		Style style2 = style;
		style2.Selector = ((Selector?)null).OfType(typeof(Button)).Class("NavButton");
		Setter setter = new Setter();
		setter.Property = TemplatedControl.BackgroundProperty;
		setter.Value = new ImmutableSolidColorBrush(16777215u);
		style2.Add(setter);
		Setter setter2 = new Setter();
		setter2.Property = TemplatedControl.BorderThicknessProperty;
		setter2.Value = new Thickness(0.0, 0.0, 0.0, 0.0);
		style2.Add(setter2);
		Setter setter3 = new Setter();
		setter3.Property = TemplatedControl.PaddingProperty;
		setter3.Value = new Thickness(0.0, 0.0, 0.0, 0.0);
		style2.Add(setter3);
		Setter setter5;
		Setter setter4 = (setter5 = new Setter());
		context.PushParent(setter5);
		Setter setter6 = setter5;
		setter6.Property = TemplatedControl.ForegroundProperty;
		DynamicResourceExtension dynamicResourceExtension = new DynamicResourceExtension("AddressBarForeground");
		context.ProvideTargetProperty = XamlIlHelpers.Avalonia_002EStyling_002ESetter_002CAvalonia_002EBase_002EValue_0021Property();
		BindingBase value = dynamicResourceExtension.ProvideValue(context);
		context.ProvideTargetProperty = null;
		setter6.Value = value;
		context.PopParent();
		style2.Add(setter4);
		Setter setter7 = new Setter();
		setter7.Property = ContentControl.HorizontalContentAlignmentProperty;
		setter7.Value = HorizontalAlignment.Center;
		style2.Add(setter7);
		Setter setter8 = new Setter();
		setter8.Property = ContentControl.VerticalContentAlignmentProperty;
		setter8.Value = VerticalAlignment.Center;
		style2.Add(setter8);
		Setter setter9 = (setter5 = new Setter());
		context.PushParent(setter5);
		Setter setter10 = setter5;
		setter10.Property = TemplatedControl.FontSizeProperty;
		DynamicResourceExtension dynamicResourceExtension2 = new DynamicResourceExtension("GlobalFontSize");
		context.ProvideTargetProperty = XamlIlHelpers.Avalonia_002EStyling_002ESetter_002CAvalonia_002EBase_002EValue_0021Property();
		BindingBase value2 = dynamicResourceExtension2.ProvideValue(context);
		context.ProvideTargetProperty = null;
		setter10.Value = value2;
		context.PopParent();
		style2.Add(setter9);
		context.PopParent();
		styles.Add(item);
		Styles styles2 = P_1.Styles;
		Style style3 = new Style();
		style3.Selector = ((Selector?)null).OfType(typeof(Button)).Class("NavButton").Class(":disabled");
		Setter setter11 = new Setter();
		setter11.Property = TemplatedControl.BackgroundProperty;
		setter11.Value = new ImmutableSolidColorBrush(16777215u);
		style3.Add(setter11);
		Setter setter12 = new Setter();
		setter12.Property = Visual.OpacityProperty;
		setter12.Value = 1.0;
		style3.Add(setter12);
		styles2.Add(style3);
		Styles styles3 = P_1.Styles;
		Style style4 = new Style();
		style4.Selector = ((Selector?)null).OfType(typeof(Button)).Class("NavButton").Class(":disabled")
			.Template()
			.OfType(typeof(ContentPresenter));
		Setter setter13 = new Setter();
		setter13.Property = ContentPresenter.BackgroundProperty;
		setter13.Value = new ImmutableSolidColorBrush(16777215u);
		style4.Add(setter13);
		Setter setter14 = new Setter();
		setter14.Property = ContentPresenter.BorderBrushProperty;
		setter14.Value = new ImmutableSolidColorBrush(16777215u);
		style4.Add(setter14);
		Setter setter15 = new Setter();
		setter15.Property = Visual.OpacityProperty;
		setter15.Value = 1.0;
		style4.Add(setter15);
		styles3.Add(style4);
		Styles styles4 = P_1.Styles;
		Style style5 = new Style();
		style5.Selector = ((Selector?)null).OfType(typeof(Button)).Class("NavButton").Class(":disabled")
			.Class(":pointerover")
			.Template()
			.OfType(typeof(ContentPresenter));
		Setter setter16 = new Setter();
		setter16.Property = ContentPresenter.BackgroundProperty;
		setter16.Value = new ImmutableSolidColorBrush(16777215u);
		style5.Add(setter16);
		Setter setter17 = new Setter();
		setter17.Property = ContentPresenter.BorderBrushProperty;
		setter17.Value = new ImmutableSolidColorBrush(16777215u);
		style5.Add(setter17);
		Setter setter18 = new Setter();
		setter18.Property = Visual.OpacityProperty;
		setter18.Value = 1.0;
		style5.Add(setter18);
		styles4.Add(style5);
		Styles styles5 = P_1.Styles;
		Style item2 = (style = new Style());
		context.PushParent(style);
		Style style6 = style;
		style6.Selector = ((Selector?)null).OfType(typeof(Button)).Class("NavButton").Class(":pointerover")
			.Template()
			.OfType(typeof(ContentPresenter));
		Setter setter19 = (setter5 = new Setter());
		context.PushParent(setter5);
		Setter setter20 = setter5;
		setter20.Property = ContentPresenter.BackgroundProperty;
		DynamicResourceExtension dynamicResourceExtension3 = new DynamicResourceExtension("BreadcrumbItemPointerOverBackground");
		context.ProvideTargetProperty = XamlIlHelpers.Avalonia_002EStyling_002ESetter_002CAvalonia_002EBase_002EValue_0021Property();
		BindingBase value3 = dynamicResourceExtension3.ProvideValue(context);
		context.ProvideTargetProperty = null;
		setter20.Value = value3;
		context.PopParent();
		style6.Add(setter19);
		context.PopParent();
		styles5.Add(item2);
		Styles styles6 = P_1.Styles;
		Style item3 = (style = new Style());
		context.PushParent(style);
		Style style7 = style;
		style7.Selector = ((Selector?)null).OfType(typeof(Button)).Class("NavButton").Class(":pressed")
			.Template()
			.OfType(typeof(ContentPresenter));
		Setter setter21 = (setter5 = new Setter());
		context.PushParent(setter5);
		Setter setter22 = setter5;
		setter22.Property = ContentPresenter.BackgroundProperty;
		DynamicResourceExtension dynamicResourceExtension4 = new DynamicResourceExtension("BreadcrumbItemPressedBackground");
		context.ProvideTargetProperty = XamlIlHelpers.Avalonia_002EStyling_002ESetter_002CAvalonia_002EBase_002EValue_0021Property();
		BindingBase value4 = dynamicResourceExtension4.ProvideValue(context);
		context.ProvideTargetProperty = null;
		setter22.Value = value4;
		context.PopParent();
		style7.Add(setter21);
		context.PopParent();
		styles6.Add(item3);
		Styles styles7 = P_1.Styles;
		Style item4 = (style = new Style());
		context.PushParent(style);
		Style style8 = style;
		style8.Selector = ((Selector?)null).OfType(typeof(Button)).Class("NavButton").Class(":disabled")
			.Descendant()
			.OfType(typeof(Avalonia.Controls.Shapes.Path))
			.Class("NavChevron");
		Setter setter23 = (setter5 = new Setter());
		context.PushParent(setter5);
		Setter setter24 = setter5;
		setter24.Property = Shape.StrokeProperty;
		DynamicResourceExtension dynamicResourceExtension5 = new DynamicResourceExtension("AddressBarForeground");
		context.ProvideTargetProperty = XamlIlHelpers.Avalonia_002EStyling_002ESetter_002CAvalonia_002EBase_002EValue_0021Property();
		BindingBase value5 = dynamicResourceExtension5.ProvideValue(context);
		context.ProvideTargetProperty = null;
		setter24.Value = value5;
		context.PopParent();
		style8.Add(setter23);
		Setter setter25 = new Setter();
		setter25.Property = Visual.OpacityProperty;
		setter25.Value = 0.25;
		style8.Add(setter25);
		context.PopParent();
		styles7.Add(item4);
		Styles styles8 = P_1.Styles;
		Style item5 = (style = new Style());
		context.PushParent(style);
		Style style9 = style;
		style9.Selector = ((Selector?)null).OfType(typeof(Avalonia.Controls.Shapes.Path)).Class("NavChevron");
		Setter setter26 = new Setter();
		setter26.Property = Layoutable.WidthProperty;
		setter26.Value = 14.0;
		style9.Add(setter26);
		Setter setter27 = new Setter();
		setter27.Property = Layoutable.HeightProperty;
		setter27.Value = 14.0;
		style9.Add(setter27);
		Setter setter28 = new Setter();
		setter28.Property = Shape.StretchProperty;
		setter28.Value = Stretch.None;
		style9.Add(setter28);
		Setter setter29 = (setter5 = new Setter());
		context.PushParent(setter5);
		Setter setter30 = setter5;
		setter30.Property = Shape.StrokeProperty;
		DynamicResourceExtension dynamicResourceExtension6 = new DynamicResourceExtension("AddressBarForeground");
		context.ProvideTargetProperty = XamlIlHelpers.Avalonia_002EStyling_002ESetter_002CAvalonia_002EBase_002EValue_0021Property();
		BindingBase value6 = dynamicResourceExtension6.ProvideValue(context);
		context.ProvideTargetProperty = null;
		setter30.Value = value6;
		context.PopParent();
		style9.Add(setter29);
		Setter setter31 = new Setter();
		setter31.Property = Shape.StrokeThicknessProperty;
		setter31.Value = 1.6;
		style9.Add(setter31);
		Setter setter32 = new Setter();
		setter32.Property = Shape.StrokeLineCapProperty;
		setter32.Value = PenLineCap.Round;
		style9.Add(setter32);
		Setter setter33 = new Setter();
		setter33.Property = Shape.StrokeJoinProperty;
		setter33.Value = PenLineJoin.Round;
		style9.Add(setter33);
		Setter setter34 = new Setter();
		setter34.Property = Shape.FillProperty;
		setter34.Value = new ImmutableSolidColorBrush(16777215u);
		style9.Add(setter34);
		Setter setter35 = new Setter();
		setter35.Property = Layoutable.HorizontalAlignmentProperty;
		setter35.Value = HorizontalAlignment.Center;
		style9.Add(setter35);
		Setter setter36 = new Setter();
		setter36.Property = Layoutable.VerticalAlignmentProperty;
		setter36.Value = VerticalAlignment.Center;
		style9.Add(setter36);
		context.PopParent();
		styles8.Add(item5);
		Grid grid2;
		Grid grid = (grid2 = new Grid());
		((ISupportInitialize)grid).BeginInit();
		P_1.Content = grid;
		Grid grid4;
		Grid grid3 = (grid4 = grid2);
		context.PushParent(grid4);
		Grid grid5 = grid4;
		grid5.Background = new ImmutableSolidColorBrush(16777215u);
		grid5.Margin = new Thickness(12.0, 6.0, 12.0, 6.0);
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
			Width = new GridLength(12.0, GridUnitType.Pixel)
		});
		Controls children = grid5.Children;
		StackPanel stackPanel2;
		StackPanel stackPanel = (stackPanel2 = new StackPanel());
		((ISupportInitialize)stackPanel).BeginInit();
		children.Add(stackPanel);
		StackPanel stackPanel4;
		StackPanel stackPanel3 = (stackPanel4 = stackPanel2);
		context.PushParent(stackPanel4);
		Grid.SetColumn(stackPanel4, 0);
		stackPanel4.Orientation = Orientation.Horizontal;
		stackPanel4.Spacing = 2.0;
		stackPanel4.Margin = new Thickness(0.0, 0.0, 12.0, 0.0);
		stackPanel4.VerticalAlignment = VerticalAlignment.Center;
		Controls children2 = stackPanel4.Children;
		Button button2;
		Button button = (button2 = new Button());
		((ISupportInitialize)button).BeginInit();
		children2.Add(button);
		Button button4;
		Button button3 = (button4 = button2);
		context.PushParent(button4);
		Button button5 = button4;
		button5.Classes.Add("NavButton");
		StyledProperty<double> widthProperty = Layoutable.WidthProperty;
		CompiledBindingExtension compiledBindingExtension2;
		CompiledBindingExtension compiledBindingExtension = (compiledBindingExtension2 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Self().Property(TemplatedControl.FontSizeProperty, PropertyInfoAccessorFactory.CreateAvaloniaPropertyAccessor).Build()));
		context.PushParent(compiledBindingExtension2);
		CompiledBindingExtension compiledBindingExtension3 = compiledBindingExtension2;
		StaticResourceExtension staticResourceExtension = new StaticResourceExtension("FontSizeToIconSizeConverter");
		context.ProvideTargetProperty = XamlIlHelpers.Avalonia_002EData_002ECompiledBinding_002CAvalonia_002EBase_002EConverter_0021Property();
		object? obj = staticResourceExtension.ProvideValue(context);
		context.ProvideTargetProperty = null;
		compiledBindingExtension3.Converter = (IValueConverter)obj;
		compiledBindingExtension3.ConverterParameter = "2.31";
		context.PopParent();
		context.ProvideTargetProperty = Layoutable.WidthProperty;
		CompiledBinding binding = compiledBindingExtension.ProvideValue(context);
		context.ProvideTargetProperty = null;
		button5.Bind(widthProperty, binding);
		StyledProperty<double> heightProperty = Layoutable.HeightProperty;
		CompiledBindingExtension compiledBindingExtension4 = (compiledBindingExtension2 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Self().Property(TemplatedControl.FontSizeProperty, PropertyInfoAccessorFactory.CreateAvaloniaPropertyAccessor).Build()));
		context.PushParent(compiledBindingExtension2);
		CompiledBindingExtension compiledBindingExtension5 = compiledBindingExtension2;
		StaticResourceExtension staticResourceExtension2 = new StaticResourceExtension("FontSizeToIconSizeConverter");
		context.ProvideTargetProperty = XamlIlHelpers.Avalonia_002EData_002ECompiledBinding_002CAvalonia_002EBase_002EConverter_0021Property();
		object? obj2 = staticResourceExtension2.ProvideValue(context);
		context.ProvideTargetProperty = null;
		compiledBindingExtension5.Converter = (IValueConverter)obj2;
		compiledBindingExtension5.ConverterParameter = "2.0";
		context.PopParent();
		context.ProvideTargetProperty = Layoutable.HeightProperty;
		CompiledBinding binding2 = compiledBindingExtension4.ProvideValue(context);
		context.ProvideTargetProperty = null;
		button5.Bind(heightProperty, binding2);
		button5.CornerRadius = new CornerRadius(6.0, 6.0, 6.0, 6.0);
		StyledProperty<ICommand?> commandProperty = Button.CommandProperty;
		CompiledBindingExtension compiledBindingExtension6 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EUserControls_002ENavigationToolbar_002ENavigationToolbarViewModel_002CXFolder_002EGoBackCommand_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build());
		context.ProvideTargetProperty = Button.CommandProperty;
		CompiledBinding binding3 = compiledBindingExtension6.ProvideValue(context);
		context.ProvideTargetProperty = null;
		button5.Bind(commandProperty, binding3);
		StyledProperty<bool> isEnabledProperty = InputElement.IsEnabledProperty;
		CompiledBindingExtension compiledBindingExtension7 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EUserControls_002ENavigationToolbar_002ENavigationToolbarViewModel_002CXFolder_002ECanGoBack_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build());
		context.ProvideTargetProperty = InputElement.IsEnabledProperty;
		CompiledBinding binding4 = compiledBindingExtension7.ProvideValue(context);
		context.ProvideTargetProperty = null;
		button5.Bind(isEnabledProperty, binding4);
		ToolTip.SetTip(button5, "Back");
		Viewbox viewbox2;
		Viewbox viewbox = (viewbox2 = new Viewbox());
		((ISupportInitialize)viewbox).BeginInit();
		button5.Content = viewbox;
		Viewbox viewbox4;
		Viewbox viewbox3 = (viewbox4 = viewbox2);
		context.PushParent(viewbox4);
		Viewbox viewbox5 = viewbox4;
		StyledProperty<double> widthProperty2 = Layoutable.WidthProperty;
		CompiledBindingExtension compiledBindingExtension8 = (compiledBindingExtension2 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Ancestor(typeof(Button), 0).Property(TemplatedControl.FontSizeProperty, PropertyInfoAccessorFactory.CreateAvaloniaPropertyAccessor).Build()));
		context.PushParent(compiledBindingExtension2);
		CompiledBindingExtension compiledBindingExtension9 = compiledBindingExtension2;
		StaticResourceExtension staticResourceExtension3 = new StaticResourceExtension("FontSizeToIconSizeConverter");
		context.ProvideTargetProperty = XamlIlHelpers.Avalonia_002EData_002ECompiledBinding_002CAvalonia_002EBase_002EConverter_0021Property();
		object? obj3 = staticResourceExtension3.ProvideValue(context);
		context.ProvideTargetProperty = null;
		compiledBindingExtension9.Converter = (IValueConverter)obj3;
		compiledBindingExtension9.ConverterParameter = "1";
		context.PopParent();
		context.ProvideTargetProperty = Layoutable.WidthProperty;
		CompiledBinding binding5 = compiledBindingExtension8.ProvideValue(context);
		context.ProvideTargetProperty = null;
		viewbox5.Bind(widthProperty2, binding5);
		StyledProperty<double> heightProperty2 = Layoutable.HeightProperty;
		CompiledBindingExtension compiledBindingExtension10 = (compiledBindingExtension2 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Ancestor(typeof(Button), 0).Property(TemplatedControl.FontSizeProperty, PropertyInfoAccessorFactory.CreateAvaloniaPropertyAccessor).Build()));
		context.PushParent(compiledBindingExtension2);
		CompiledBindingExtension compiledBindingExtension11 = compiledBindingExtension2;
		StaticResourceExtension staticResourceExtension4 = new StaticResourceExtension("FontSizeToIconSizeConverter");
		context.ProvideTargetProperty = XamlIlHelpers.Avalonia_002EData_002ECompiledBinding_002CAvalonia_002EBase_002EConverter_0021Property();
		object? obj4 = staticResourceExtension4.ProvideValue(context);
		context.ProvideTargetProperty = null;
		compiledBindingExtension11.Converter = (IValueConverter)obj4;
		compiledBindingExtension11.ConverterParameter = "1";
		context.PopParent();
		context.ProvideTargetProperty = Layoutable.HeightProperty;
		CompiledBinding binding6 = compiledBindingExtension10.ProvideValue(context);
		context.ProvideTargetProperty = null;
		viewbox5.Bind(heightProperty2, binding6);
		Avalonia.Controls.Shapes.Path path2;
		Avalonia.Controls.Shapes.Path path = (path2 = new Avalonia.Controls.Shapes.Path());
		((ISupportInitialize)path).BeginInit();
		viewbox5.Child = path;
		path2.Classes.Add("NavChevron");
		path2.Data = Geometry.Parse("M 9 3 L 5 7 L 9 11");
		((ISupportInitialize)path2).EndInit();
		context.PopParent();
		((ISupportInitialize)viewbox3).EndInit();
		context.PopParent();
		((ISupportInitialize)button3).EndInit();
		Controls children3 = stackPanel4.Children;
		Button button7;
		Button button6 = (button7 = new Button());
		((ISupportInitialize)button6).BeginInit();
		children3.Add(button6);
		Button button8 = (button4 = button7);
		context.PushParent(button4);
		Button button9 = button4;
		button9.Classes.Add("NavButton");
		StyledProperty<double> widthProperty3 = Layoutable.WidthProperty;
		CompiledBindingExtension compiledBindingExtension12 = (compiledBindingExtension2 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Self().Property(TemplatedControl.FontSizeProperty, PropertyInfoAccessorFactory.CreateAvaloniaPropertyAccessor).Build()));
		context.PushParent(compiledBindingExtension2);
		CompiledBindingExtension compiledBindingExtension13 = compiledBindingExtension2;
		StaticResourceExtension staticResourceExtension5 = new StaticResourceExtension("FontSizeToIconSizeConverter");
		context.ProvideTargetProperty = XamlIlHelpers.Avalonia_002EData_002ECompiledBinding_002CAvalonia_002EBase_002EConverter_0021Property();
		object? obj5 = staticResourceExtension5.ProvideValue(context);
		context.ProvideTargetProperty = null;
		compiledBindingExtension13.Converter = (IValueConverter)obj5;
		compiledBindingExtension13.ConverterParameter = "2.31";
		context.PopParent();
		context.ProvideTargetProperty = Layoutable.WidthProperty;
		CompiledBinding binding7 = compiledBindingExtension12.ProvideValue(context);
		context.ProvideTargetProperty = null;
		button9.Bind(widthProperty3, binding7);
		StyledProperty<double> heightProperty3 = Layoutable.HeightProperty;
		CompiledBindingExtension compiledBindingExtension14 = (compiledBindingExtension2 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Self().Property(TemplatedControl.FontSizeProperty, PropertyInfoAccessorFactory.CreateAvaloniaPropertyAccessor).Build()));
		context.PushParent(compiledBindingExtension2);
		CompiledBindingExtension compiledBindingExtension15 = compiledBindingExtension2;
		StaticResourceExtension staticResourceExtension6 = new StaticResourceExtension("FontSizeToIconSizeConverter");
		context.ProvideTargetProperty = XamlIlHelpers.Avalonia_002EData_002ECompiledBinding_002CAvalonia_002EBase_002EConverter_0021Property();
		object? obj6 = staticResourceExtension6.ProvideValue(context);
		context.ProvideTargetProperty = null;
		compiledBindingExtension15.Converter = (IValueConverter)obj6;
		compiledBindingExtension15.ConverterParameter = "2.0";
		context.PopParent();
		context.ProvideTargetProperty = Layoutable.HeightProperty;
		CompiledBinding binding8 = compiledBindingExtension14.ProvideValue(context);
		context.ProvideTargetProperty = null;
		button9.Bind(heightProperty3, binding8);
		button9.CornerRadius = new CornerRadius(6.0, 6.0, 6.0, 6.0);
		StyledProperty<ICommand?> commandProperty2 = Button.CommandProperty;
		CompiledBindingExtension compiledBindingExtension16 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EUserControls_002ENavigationToolbar_002ENavigationToolbarViewModel_002CXFolder_002EGoForwardCommand_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build());
		context.ProvideTargetProperty = Button.CommandProperty;
		CompiledBinding binding9 = compiledBindingExtension16.ProvideValue(context);
		context.ProvideTargetProperty = null;
		button9.Bind(commandProperty2, binding9);
		StyledProperty<bool> isEnabledProperty2 = InputElement.IsEnabledProperty;
		CompiledBindingExtension compiledBindingExtension17 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EUserControls_002ENavigationToolbar_002ENavigationToolbarViewModel_002CXFolder_002ECanGoForward_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build());
		context.ProvideTargetProperty = InputElement.IsEnabledProperty;
		CompiledBinding binding10 = compiledBindingExtension17.ProvideValue(context);
		context.ProvideTargetProperty = null;
		button9.Bind(isEnabledProperty2, binding10);
		ToolTip.SetTip(button9, "Forward");
		Viewbox viewbox7;
		Viewbox viewbox6 = (viewbox7 = new Viewbox());
		((ISupportInitialize)viewbox6).BeginInit();
		button9.Content = viewbox6;
		Viewbox viewbox8 = (viewbox4 = viewbox7);
		context.PushParent(viewbox4);
		Viewbox viewbox9 = viewbox4;
		StyledProperty<double> widthProperty4 = Layoutable.WidthProperty;
		CompiledBindingExtension compiledBindingExtension18 = (compiledBindingExtension2 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Ancestor(typeof(Button), 0).Property(TemplatedControl.FontSizeProperty, PropertyInfoAccessorFactory.CreateAvaloniaPropertyAccessor).Build()));
		context.PushParent(compiledBindingExtension2);
		CompiledBindingExtension compiledBindingExtension19 = compiledBindingExtension2;
		StaticResourceExtension staticResourceExtension7 = new StaticResourceExtension("FontSizeToIconSizeConverter");
		context.ProvideTargetProperty = XamlIlHelpers.Avalonia_002EData_002ECompiledBinding_002CAvalonia_002EBase_002EConverter_0021Property();
		object? obj7 = staticResourceExtension7.ProvideValue(context);
		context.ProvideTargetProperty = null;
		compiledBindingExtension19.Converter = (IValueConverter)obj7;
		compiledBindingExtension19.ConverterParameter = "1";
		context.PopParent();
		context.ProvideTargetProperty = Layoutable.WidthProperty;
		CompiledBinding binding11 = compiledBindingExtension18.ProvideValue(context);
		context.ProvideTargetProperty = null;
		viewbox9.Bind(widthProperty4, binding11);
		StyledProperty<double> heightProperty4 = Layoutable.HeightProperty;
		CompiledBindingExtension compiledBindingExtension20 = (compiledBindingExtension2 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Ancestor(typeof(Button), 0).Property(TemplatedControl.FontSizeProperty, PropertyInfoAccessorFactory.CreateAvaloniaPropertyAccessor).Build()));
		context.PushParent(compiledBindingExtension2);
		CompiledBindingExtension compiledBindingExtension21 = compiledBindingExtension2;
		StaticResourceExtension staticResourceExtension8 = new StaticResourceExtension("FontSizeToIconSizeConverter");
		context.ProvideTargetProperty = XamlIlHelpers.Avalonia_002EData_002ECompiledBinding_002CAvalonia_002EBase_002EConverter_0021Property();
		object? obj8 = staticResourceExtension8.ProvideValue(context);
		context.ProvideTargetProperty = null;
		compiledBindingExtension21.Converter = (IValueConverter)obj8;
		compiledBindingExtension21.ConverterParameter = "1";
		context.PopParent();
		context.ProvideTargetProperty = Layoutable.HeightProperty;
		CompiledBinding binding12 = compiledBindingExtension20.ProvideValue(context);
		context.ProvideTargetProperty = null;
		viewbox9.Bind(heightProperty4, binding12);
		Avalonia.Controls.Shapes.Path path4;
		Avalonia.Controls.Shapes.Path path3 = (path4 = new Avalonia.Controls.Shapes.Path());
		((ISupportInitialize)path3).BeginInit();
		viewbox9.Child = path3;
		path4.Classes.Add("NavChevron");
		path4.Data = Geometry.Parse("M 5 3 L 9 7 L 5 11");
		((ISupportInitialize)path4).EndInit();
		context.PopParent();
		((ISupportInitialize)viewbox8).EndInit();
		context.PopParent();
		((ISupportInitialize)button8).EndInit();
		Controls children4 = stackPanel4.Children;
		Button button11;
		Button button10 = (button11 = new Button());
		((ISupportInitialize)button10).BeginInit();
		children4.Add(button10);
		Button button12 = (button4 = button11);
		context.PushParent(button4);
		Button button13 = button4;
		button13.Classes.Add("NavButton");
		StyledProperty<double> widthProperty5 = Layoutable.WidthProperty;
		CompiledBindingExtension compiledBindingExtension22 = (compiledBindingExtension2 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Self().Property(TemplatedControl.FontSizeProperty, PropertyInfoAccessorFactory.CreateAvaloniaPropertyAccessor).Build()));
		context.PushParent(compiledBindingExtension2);
		CompiledBindingExtension compiledBindingExtension23 = compiledBindingExtension2;
		StaticResourceExtension staticResourceExtension9 = new StaticResourceExtension("FontSizeToIconSizeConverter");
		context.ProvideTargetProperty = XamlIlHelpers.Avalonia_002EData_002ECompiledBinding_002CAvalonia_002EBase_002EConverter_0021Property();
		object? obj9 = staticResourceExtension9.ProvideValue(context);
		context.ProvideTargetProperty = null;
		compiledBindingExtension23.Converter = (IValueConverter)obj9;
		compiledBindingExtension23.ConverterParameter = "2.31";
		context.PopParent();
		context.ProvideTargetProperty = Layoutable.WidthProperty;
		CompiledBinding binding13 = compiledBindingExtension22.ProvideValue(context);
		context.ProvideTargetProperty = null;
		button13.Bind(widthProperty5, binding13);
		StyledProperty<double> heightProperty5 = Layoutable.HeightProperty;
		CompiledBindingExtension compiledBindingExtension24 = (compiledBindingExtension2 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Self().Property(TemplatedControl.FontSizeProperty, PropertyInfoAccessorFactory.CreateAvaloniaPropertyAccessor).Build()));
		context.PushParent(compiledBindingExtension2);
		CompiledBindingExtension compiledBindingExtension25 = compiledBindingExtension2;
		StaticResourceExtension staticResourceExtension10 = new StaticResourceExtension("FontSizeToIconSizeConverter");
		context.ProvideTargetProperty = XamlIlHelpers.Avalonia_002EData_002ECompiledBinding_002CAvalonia_002EBase_002EConverter_0021Property();
		object? obj10 = staticResourceExtension10.ProvideValue(context);
		context.ProvideTargetProperty = null;
		compiledBindingExtension25.Converter = (IValueConverter)obj10;
		compiledBindingExtension25.ConverterParameter = "2.0";
		context.PopParent();
		context.ProvideTargetProperty = Layoutable.HeightProperty;
		CompiledBinding binding14 = compiledBindingExtension24.ProvideValue(context);
		context.ProvideTargetProperty = null;
		button13.Bind(heightProperty5, binding14);
		button13.CornerRadius = new CornerRadius(6.0, 6.0, 6.0, 6.0);
		StyledProperty<ICommand?> commandProperty3 = Button.CommandProperty;
		CompiledBindingExtension compiledBindingExtension26 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EUserControls_002ENavigationToolbar_002ENavigationToolbarViewModel_002CXFolder_002ENavigateToParentCommand_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build());
		context.ProvideTargetProperty = Button.CommandProperty;
		CompiledBinding binding15 = compiledBindingExtension26.ProvideValue(context);
		context.ProvideTargetProperty = null;
		button13.Bind(commandProperty3, binding15);
		StyledProperty<bool> isEnabledProperty3 = InputElement.IsEnabledProperty;
		CompiledBindingExtension compiledBindingExtension27 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EUserControls_002ENavigationToolbar_002ENavigationToolbarViewModel_002CXFolder_002ECanNavigateToParent_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build());
		context.ProvideTargetProperty = InputElement.IsEnabledProperty;
		CompiledBinding binding16 = compiledBindingExtension27.ProvideValue(context);
		context.ProvideTargetProperty = null;
		button13.Bind(isEnabledProperty3, binding16);
		ToolTip.SetTip(button13, "Up");
		Viewbox viewbox11;
		Viewbox viewbox10 = (viewbox11 = new Viewbox());
		((ISupportInitialize)viewbox10).BeginInit();
		button13.Content = viewbox10;
		Viewbox viewbox12 = (viewbox4 = viewbox11);
		context.PushParent(viewbox4);
		Viewbox viewbox13 = viewbox4;
		StyledProperty<double> widthProperty6 = Layoutable.WidthProperty;
		CompiledBindingExtension compiledBindingExtension28 = (compiledBindingExtension2 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Ancestor(typeof(Button), 0).Property(TemplatedControl.FontSizeProperty, PropertyInfoAccessorFactory.CreateAvaloniaPropertyAccessor).Build()));
		context.PushParent(compiledBindingExtension2);
		CompiledBindingExtension compiledBindingExtension29 = compiledBindingExtension2;
		StaticResourceExtension staticResourceExtension11 = new StaticResourceExtension("FontSizeToIconSizeConverter");
		context.ProvideTargetProperty = XamlIlHelpers.Avalonia_002EData_002ECompiledBinding_002CAvalonia_002EBase_002EConverter_0021Property();
		object? obj11 = staticResourceExtension11.ProvideValue(context);
		context.ProvideTargetProperty = null;
		compiledBindingExtension29.Converter = (IValueConverter)obj11;
		compiledBindingExtension29.ConverterParameter = "1";
		context.PopParent();
		context.ProvideTargetProperty = Layoutable.WidthProperty;
		CompiledBinding binding17 = compiledBindingExtension28.ProvideValue(context);
		context.ProvideTargetProperty = null;
		viewbox13.Bind(widthProperty6, binding17);
		StyledProperty<double> heightProperty6 = Layoutable.HeightProperty;
		CompiledBindingExtension compiledBindingExtension30 = (compiledBindingExtension2 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Ancestor(typeof(Button), 0).Property(TemplatedControl.FontSizeProperty, PropertyInfoAccessorFactory.CreateAvaloniaPropertyAccessor).Build()));
		context.PushParent(compiledBindingExtension2);
		CompiledBindingExtension compiledBindingExtension31 = compiledBindingExtension2;
		StaticResourceExtension staticResourceExtension12 = new StaticResourceExtension("FontSizeToIconSizeConverter");
		context.ProvideTargetProperty = XamlIlHelpers.Avalonia_002EData_002ECompiledBinding_002CAvalonia_002EBase_002EConverter_0021Property();
		object? obj12 = staticResourceExtension12.ProvideValue(context);
		context.ProvideTargetProperty = null;
		compiledBindingExtension31.Converter = (IValueConverter)obj12;
		compiledBindingExtension31.ConverterParameter = "1";
		context.PopParent();
		context.ProvideTargetProperty = Layoutable.HeightProperty;
		CompiledBinding binding18 = compiledBindingExtension30.ProvideValue(context);
		context.ProvideTargetProperty = null;
		viewbox13.Bind(heightProperty6, binding18);
		Avalonia.Controls.Shapes.Path path6;
		Avalonia.Controls.Shapes.Path path5 = (path6 = new Avalonia.Controls.Shapes.Path());
		((ISupportInitialize)path5).BeginInit();
		viewbox13.Child = path5;
		path6.Classes.Add("NavChevron");
		path6.Data = Geometry.Parse("M 3 9 L 7 5 L 11 9");
		((ISupportInitialize)path6).EndInit();
		context.PopParent();
		((ISupportInitialize)viewbox12).EndInit();
		context.PopParent();
		((ISupportInitialize)button12).EndInit();
		context.PopParent();
		((ISupportInitialize)stackPanel3).EndInit();
		Controls children5 = grid5.Children;
		Grid grid7;
		Grid grid6 = (grid7 = new Grid());
		((ISupportInitialize)grid6).BeginInit();
		children5.Add(grid6);
		Grid grid8 = (grid4 = grid7);
		context.PushParent(grid4);
		Grid grid9 = grid4;
		Grid.SetColumn(grid9, 1);
		grid9.HorizontalAlignment = HorizontalAlignment.Stretch;
		grid9.VerticalAlignment = VerticalAlignment.Center;
		StyledProperty<double> heightProperty7 = Layoutable.HeightProperty;
		CompiledBindingExtension compiledBindingExtension32 = (compiledBindingExtension2 = new CompiledBindingExtension(new CompiledBindingPathBuilder().ElementName(context.AvaloniaNameScope, "tbWorkingDirectory").Property(TemplatedControl.FontSizeProperty, PropertyInfoAccessorFactory.CreateAvaloniaPropertyAccessor).Build()));
		context.PushParent(compiledBindingExtension2);
		CompiledBindingExtension compiledBindingExtension33 = compiledBindingExtension2;
		StaticResourceExtension staticResourceExtension13 = new StaticResourceExtension("FontSizeToIconSizeConverter");
		context.ProvideTargetProperty = XamlIlHelpers.Avalonia_002EData_002ECompiledBinding_002CAvalonia_002EBase_002EConverter_0021Property();
		object? obj13 = staticResourceExtension13.ProvideValue(context);
		context.ProvideTargetProperty = null;
		compiledBindingExtension33.Converter = (IValueConverter)obj13;
		compiledBindingExtension33.ConverterParameter = "2";
		context.PopParent();
		context.ProvideTargetProperty = Layoutable.HeightProperty;
		CompiledBinding binding19 = compiledBindingExtension32.ProvideValue(context);
		context.ProvideTargetProperty = null;
		grid9.Bind(heightProperty7, binding19);
		Controls children6 = grid9.Children;
		Border border2;
		Border border = (border2 = new Border());
		((ISupportInitialize)border).BeginInit();
		children6.Add(border);
		Border border4;
		Border border3 = (border4 = border2);
		context.PushParent(border4);
		border4.Name = "BreadcrumbContainer";
		object element = border4;
		context.AvaloniaNameScope.Register("BreadcrumbContainer", element);
		StyledProperty<IBrush?> backgroundProperty = Border.BackgroundProperty;
		DynamicResourceExtension dynamicResourceExtension7 = new DynamicResourceExtension("AddressBarBackground");
		context.ProvideTargetProperty = Border.BackgroundProperty;
		BindingBase binding20 = dynamicResourceExtension7.ProvideValue(context);
		context.ProvideTargetProperty = null;
		border4.Bind(backgroundProperty, binding20);
		StyledProperty<IBrush?> borderBrushProperty = Border.BorderBrushProperty;
		DynamicResourceExtension dynamicResourceExtension8 = new DynamicResourceExtension("AddressBarBorder");
		context.ProvideTargetProperty = Border.BorderBrushProperty;
		BindingBase binding21 = dynamicResourceExtension8.ProvideValue(context);
		context.ProvideTargetProperty = null;
		border4.Bind(borderBrushProperty, binding21);
		border4.BorderThickness = new Thickness(1.0, 1.0, 1.0, 1.0);
		border4.CornerRadius = new CornerRadius(14.0, 14.0, 14.0, 14.0);
		border4.Padding = new Thickness(8.0, 0.0, 8.0, 0.0);
		StyledProperty<bool> isVisibleProperty = Visual.IsVisibleProperty;
		CompiledBindingExtension compiledBindingExtension34 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Not().Property(XamlIlHelpers.Files_002EUserControls_002ENavigationToolbar_002ENavigationToolbarViewModel_002CXFolder_002EIsFocused_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build());
		context.ProvideTargetProperty = Visual.IsVisibleProperty;
		CompiledBinding binding22 = compiledBindingExtension34.ProvideValue(context);
		context.ProvideTargetProperty = null;
		border4.Bind(isVisibleProperty, binding22);
		border4.AddHandler(InputElement.PointerPressedEvent, context.RootObject.BreadcrumbContainer_OnPointerPressed);
		Files.UserControls.BreadcrumbBar.BreadcrumbBar breadcrumbBar2;
		Files.UserControls.BreadcrumbBar.BreadcrumbBar breadcrumbBar = (breadcrumbBar2 = new Files.UserControls.BreadcrumbBar.BreadcrumbBar());
		((ISupportInitialize)breadcrumbBar).BeginInit();
		border4.Child = breadcrumbBar;
		Files.UserControls.BreadcrumbBar.BreadcrumbBar breadcrumbBar4;
		Files.UserControls.BreadcrumbBar.BreadcrumbBar breadcrumbBar3 = (breadcrumbBar4 = breadcrumbBar2);
		context.PushParent(breadcrumbBar4);
		breadcrumbBar4.Name = "PathBreadcrumb";
		element = breadcrumbBar4;
		context.AvaloniaNameScope.Register("PathBreadcrumb", element);
		StyledProperty<ObservableCollection<PathBoxItem>> pathComponentsProperty = Files.UserControls.BreadcrumbBar.BreadcrumbBar.PathComponentsProperty;
		CompiledBindingExtension compiledBindingExtension35 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EUserControls_002ENavigationToolbar_002ENavigationToolbarViewModel_002CXFolder_002EPathComponents_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build());
		context.ProvideTargetProperty = Files.UserControls.BreadcrumbBar.BreadcrumbBar.PathComponentsProperty;
		CompiledBinding binding23 = compiledBindingExtension35.ProvideValue(context);
		context.ProvideTargetProperty = null;
		breadcrumbBar4.Bind(pathComponentsProperty, binding23);
		breadcrumbBar4.ItemClicked += context.RootObject.PathBreadcrumb_OnItemClicked;
		breadcrumbBar4.ItemContextRequested += context.RootObject.PathBreadcrumb_OnItemContextRequested;
		breadcrumbBar4.ChevronClicked += context.RootObject.PathBreadcrumb_OnChevronClicked;
		breadcrumbBar4.BlankAreaClicked += context.RootObject.PathBreadcrumb_OnBlankAreaClicked;
		breadcrumbBar4.VerticalAlignment = VerticalAlignment.Center;
		breadcrumbBar4.HorizontalAlignment = HorizontalAlignment.Stretch;
		context.PopParent();
		((ISupportInitialize)breadcrumbBar3).EndInit();
		context.PopParent();
		((ISupportInitialize)border3).EndInit();
		Controls children7 = grid9.Children;
		TextBox textBox2;
		TextBox textBox = (textBox2 = new TextBox());
		((ISupportInitialize)textBox).BeginInit();
		children7.Add(textBox);
		TextBox textBox4;
		TextBox textBox3 = (textBox4 = textBox2);
		context.PushParent(textBox4);
		textBox4.Name = "tbWorkingDirectory";
		element = textBox4;
		context.AvaloniaNameScope.Register("tbWorkingDirectory", element);
		textBox4.HorizontalAlignment = HorizontalAlignment.Stretch;
		textBox4.VerticalAlignment = VerticalAlignment.Stretch;
		textBox4.CornerRadius = new CornerRadius(14.0, 14.0, 14.0, 14.0);
		StyledProperty<double> fontSizeProperty = TemplatedControl.FontSizeProperty;
		DynamicResourceExtension dynamicResourceExtension9 = new DynamicResourceExtension("GlobalFontSize");
		context.ProvideTargetProperty = TemplatedControl.FontSizeProperty;
		BindingBase binding24 = dynamicResourceExtension9.ProvideValue(context);
		context.ProvideTargetProperty = null;
		textBox4.Bind(fontSizeProperty, binding24);
		textBox4.BorderThickness = new Thickness(1.0, 1.0, 1.0, 1.0);
		textBox4.Padding = new Thickness(12.0, 0.0, 12.0, 0.0);
		textBox4.VerticalContentAlignment = VerticalAlignment.Center;
		StyledProperty<string?> textProperty = TextBox.TextProperty;
		CompiledBindingExtension compiledBindingExtension36 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EUserControls_002ENavigationToolbar_002ENavigationToolbarViewModel_002CXFolder_002EPathControlDisplayText_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build())
		{
			Mode = BindingMode.OneWay
		};
		context.ProvideTargetProperty = TextBox.TextProperty;
		CompiledBinding binding25 = compiledBindingExtension36.ProvideValue(context);
		context.ProvideTargetProperty = null;
		textBox4.Bind(textProperty, binding25);
		StyledProperty<bool> isVisibleProperty2 = Visual.IsVisibleProperty;
		CompiledBindingExtension compiledBindingExtension37 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EUserControls_002ENavigationToolbar_002ENavigationToolbarViewModel_002CXFolder_002EIsFocused_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build());
		context.ProvideTargetProperty = Visual.IsVisibleProperty;
		CompiledBinding binding26 = compiledBindingExtension37.ProvideValue(context);
		context.ProvideTargetProperty = null;
		textBox4.Bind(isVisibleProperty2, binding26);
		textBox4.AddHandler(InputElement.GotFocusEvent, context.RootObject.TbWorkingDirectory_OnGotFocus);
		textBox4.AddHandler((RoutedEvent)InputElement.LostFocusEvent, (Delegate)new EventHandler<RoutedEventArgs>(context.RootObject.TbWorkingDirectory_OnLostFocus), RoutingStrategies.Direct | RoutingStrategies.Bubble, false);
		textBox4.AddHandler(InputElement.KeyDownEvent, context.RootObject.TbWorkingDirectory_OnKeyDown);
		Styles styles9 = textBox4.Styles;
		Style item6 = (style = new Style());
		context.PushParent(style);
		Style style10 = style;
		style10.Selector = ((Selector?)null).OfType(typeof(TextBox));
		Setter setter37 = (setter5 = new Setter());
		context.PushParent(setter5);
		Setter setter38 = setter5;
		setter38.Property = TemplatedControl.BackgroundProperty;
		DynamicResourceExtension dynamicResourceExtension10 = new DynamicResourceExtension("AddressBarBackground");
		context.ProvideTargetProperty = XamlIlHelpers.Avalonia_002EStyling_002ESetter_002CAvalonia_002EBase_002EValue_0021Property();
		BindingBase value7 = dynamicResourceExtension10.ProvideValue(context);
		context.ProvideTargetProperty = null;
		setter38.Value = value7;
		context.PopParent();
		style10.Add(setter37);
		Setter setter39 = (setter5 = new Setter());
		context.PushParent(setter5);
		Setter setter40 = setter5;
		setter40.Property = TemplatedControl.BorderBrushProperty;
		DynamicResourceExtension dynamicResourceExtension11 = new DynamicResourceExtension("AddressBarBorder");
		context.ProvideTargetProperty = XamlIlHelpers.Avalonia_002EStyling_002ESetter_002CAvalonia_002EBase_002EValue_0021Property();
		BindingBase value8 = dynamicResourceExtension11.ProvideValue(context);
		context.ProvideTargetProperty = null;
		setter40.Value = value8;
		context.PopParent();
		style10.Add(setter39);
		Setter setter41 = (setter5 = new Setter());
		context.PushParent(setter5);
		Setter setter42 = setter5;
		setter42.Property = TemplatedControl.ForegroundProperty;
		DynamicResourceExtension dynamicResourceExtension12 = new DynamicResourceExtension("AddressBarForeground");
		context.ProvideTargetProperty = XamlIlHelpers.Avalonia_002EStyling_002ESetter_002CAvalonia_002EBase_002EValue_0021Property();
		BindingBase value9 = dynamicResourceExtension12.ProvideValue(context);
		context.ProvideTargetProperty = null;
		setter42.Value = value9;
		context.PopParent();
		style10.Add(setter41);
		Setter setter43 = (setter5 = new Setter());
		context.PushParent(setter5);
		Setter setter44 = setter5;
		setter44.Property = TextBox.SelectionBrushProperty;
		DynamicResourceExtension dynamicResourceExtension13 = new DynamicResourceExtension("SystemAccentColor");
		context.ProvideTargetProperty = XamlIlHelpers.Avalonia_002EStyling_002ESetter_002CAvalonia_002EBase_002EValue_0021Property();
		BindingBase value10 = dynamicResourceExtension13.ProvideValue(context);
		context.ProvideTargetProperty = null;
		setter44.Value = value10;
		context.PopParent();
		style10.Add(setter43);
		Setter setter45 = (setter5 = new Setter());
		context.PushParent(setter5);
		Setter setter46 = setter5;
		setter46.Property = TextBox.SelectionForegroundBrushProperty;
		DynamicResourceExtension dynamicResourceExtension14 = new DynamicResourceExtension("SystemBaseHighColor");
		context.ProvideTargetProperty = XamlIlHelpers.Avalonia_002EStyling_002ESetter_002CAvalonia_002EBase_002EValue_0021Property();
		BindingBase value11 = dynamicResourceExtension14.ProvideValue(context);
		context.ProvideTargetProperty = null;
		setter46.Value = value11;
		context.PopParent();
		style10.Add(setter45);
		context.PopParent();
		styles9.Add(item6);
		Styles styles10 = textBox4.Styles;
		Style item7 = (style = new Style());
		context.PushParent(style);
		Style style11 = style;
		style11.Selector = ((Selector?)null).OfType(typeof(TextBox)).Class(":focus").Template()
			.OfType(typeof(Border))
			.Name("PART_BorderElement");
		Setter setter47 = (setter5 = new Setter());
		context.PushParent(setter5);
		Setter setter48 = setter5;
		setter48.Property = Border.BackgroundProperty;
		DynamicResourceExtension dynamicResourceExtension15 = new DynamicResourceExtension("AddressBarBackground");
		context.ProvideTargetProperty = XamlIlHelpers.Avalonia_002EStyling_002ESetter_002CAvalonia_002EBase_002EValue_0021Property();
		BindingBase value12 = dynamicResourceExtension15.ProvideValue(context);
		context.ProvideTargetProperty = null;
		setter48.Value = value12;
		context.PopParent();
		style11.Add(setter47);
		Setter setter49 = (setter5 = new Setter());
		context.PushParent(setter5);
		Setter setter50 = setter5;
		setter50.Property = Border.BorderBrushProperty;
		DynamicResourceExtension dynamicResourceExtension16 = new DynamicResourceExtension("SystemAccentColor");
		context.ProvideTargetProperty = XamlIlHelpers.Avalonia_002EStyling_002ESetter_002CAvalonia_002EBase_002EValue_0021Property();
		BindingBase value13 = dynamicResourceExtension16.ProvideValue(context);
		context.ProvideTargetProperty = null;
		setter50.Value = value13;
		context.PopParent();
		style11.Add(setter49);
		Setter setter51 = new Setter();
		setter51.Property = Border.BorderThicknessProperty;
		setter51.Value = new Thickness(1.0, 1.0, 1.0, 1.0);
		style11.Add(setter51);
		context.PopParent();
		styles10.Add(item7);
		context.PopParent();
		((ISupportInitialize)textBox3).EndInit();
		context.PopParent();
		((ISupportInitialize)grid8).EndInit();
		context.PopParent();
		((ISupportInitialize)grid3).EndInit();
		context.PopParent();
		((ISupportInitialize)P_1).EndInit();
		if (P_1 is StyledElement styled)
		{
			NameScope.SetNameScope(styled, context.AvaloniaNameScope);
		}
		context.AvaloniaNameScope.Complete();
	}

	[CompilerGenerated]
	private static void _0021XamlIlPopulateTrampoline(NavigationToolbarView P_0)
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
