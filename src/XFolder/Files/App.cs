using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls.Documents;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Layout;
using Avalonia.Markup.Xaml;
using Avalonia.Markup.Xaml.MarkupExtensions;
using Avalonia.Markup.Xaml.XamlIl.Runtime;
using Avalonia.Media;
using Avalonia.Platform.Storage;
using Avalonia.Styling;
using Avalonia.Themes.Fluent;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.DependencyInjection;
using CompiledAvaloniaXaml;
using DialogHostAvalonia;
using Files.Assets.Lang;
using Files.Converters;
using Files.Data.Commands.Manager;
using Files.Data.Contexts;
using Files.Data.Contracts;
using Files.Data.Models;
using Files.Helpers;
using Files.Helpers.Application;
using Files.Helpers.Shared;
using Files.Services.App;
using Files.Utils.Storage.Helpers;
using Files.Utils.Storage.History;
using Files.ViewModels;
using Files.Views;
using Microsoft.Extensions.Logging;

namespace Files;

public class App : Application
{
	[CompilerGenerated]
	private class XamlClosure_1
	{
		public static object Build_1(IServiceProvider P_0)
		{
			XamlIlContext.Context<App> context = CreateContext(P_0);
			SolidColorBrush solidColorBrush;
			SolidColorBrush result = (solidColorBrush = new SolidColorBrush());
			context.PushParent(solidColorBrush);
			StaticResourceExtension staticResourceExtension = new StaticResourceExtension("SystemAccentColor");
			context.ProvideTargetProperty = SolidColorBrush.ColorProperty;
			object? obj = staticResourceExtension.ProvideValue(context);
			context.ProvideTargetProperty = null;
			XamlDynamicSetters._003C_003EXamlDynamicSetter_1(solidColorBrush, obj);
			context.PopParent();
			return result;
		}

		public static XamlIlContext.Context<App> CreateContext(IServiceProvider P_0)
		{
			XamlIlContext.Context<App> context = new XamlIlContext.Context<App>(P_0, new object[1] { _0021AvaloniaResources.NamespaceInfo_003A_002FApp_002Eaxaml.Singleton }, "avares://XFolder/App.axaml");
			if (P_0 != null)
			{
				object service = P_0.GetService(typeof(IRootObjectProvider));
				if (service != null)
				{
					service = ((IRootObjectProvider)service).RootObject;
					context.RootObject = (App)service;
				}
			}
			return context;
		}

		public static object Build_2(IServiceProvider P_0)
		{
			XamlIlContext.Context<App> context = CreateContext(P_0);
			return new SolidColorBrush
			{
				Color = Color.FromUInt32(4291618303u)
			};
		}

		public static object Build_3(IServiceProvider P_0)
		{
			XamlIlContext.Context<App> context = CreateContext(P_0);
			SolidColorBrush solidColorBrush;
			SolidColorBrush result = (solidColorBrush = new SolidColorBrush());
			context.PushParent(solidColorBrush);
			StaticResourceExtension staticResourceExtension = new StaticResourceExtension("DialogBackground");
			context.ProvideTargetProperty = SolidColorBrush.ColorProperty;
			object? obj = staticResourceExtension.ProvideValue(context);
			context.ProvideTargetProperty = null;
			XamlDynamicSetters._003C_003EXamlDynamicSetter_1(solidColorBrush, obj);
			context.PopParent();
			return result;
		}

		public static object Build_4(IServiceProvider P_0)
		{
			XamlIlContext.Context<App> context = CreateContext(P_0);
			SolidColorBrush solidColorBrush;
			SolidColorBrush result = (solidColorBrush = new SolidColorBrush());
			context.PushParent(solidColorBrush);
			StaticResourceExtension staticResourceExtension = new StaticResourceExtension("PaneActiveBackground");
			context.ProvideTargetProperty = SolidColorBrush.ColorProperty;
			object? obj = staticResourceExtension.ProvideValue(context);
			context.ProvideTargetProperty = null;
			XamlDynamicSetters._003C_003EXamlDynamicSetter_1(solidColorBrush, obj);
			context.PopParent();
			return result;
		}

		public static object Build_5(IServiceProvider P_0)
		{
			XamlIlContext.Context<App> context = CreateContext(P_0);
			return new SolidColorBrush
			{
				Color = Color.FromUInt32(4294375674u)
			};
		}

		public static object Build_6(IServiceProvider P_0)
		{
			XamlIlContext.Context<App> context = CreateContext(P_0);
			SolidColorBrush solidColorBrush;
			SolidColorBrush result = (solidColorBrush = new SolidColorBrush());
			context.PushParent(solidColorBrush);
			StaticResourceExtension staticResourceExtension = new StaticResourceExtension("SystemAccentColor");
			context.ProvideTargetProperty = SolidColorBrush.ColorProperty;
			object? obj = staticResourceExtension.ProvideValue(context);
			context.ProvideTargetProperty = null;
			XamlDynamicSetters._003C_003EXamlDynamicSetter_1(solidColorBrush, obj);
			context.PopParent();
			return result;
		}

		public static object Build_7(IServiceProvider P_0)
		{
			XamlIlContext.Context<App> context = CreateContext(P_0);
			return new SolidColorBrush
			{
				Color = Color.FromUInt32(4278469257u)
			};
		}

		public static object Build_8(IServiceProvider P_0)
		{
			XamlIlContext.Context<App> context = CreateContext(P_0);
			SolidColorBrush solidColorBrush;
			SolidColorBrush result = (solidColorBrush = new SolidColorBrush());
			context.PushParent(solidColorBrush);
			StaticResourceExtension staticResourceExtension = new StaticResourceExtension("DialogBackground");
			context.ProvideTargetProperty = SolidColorBrush.ColorProperty;
			object? obj = staticResourceExtension.ProvideValue(context);
			context.ProvideTargetProperty = null;
			XamlDynamicSetters._003C_003EXamlDynamicSetter_1(solidColorBrush, obj);
			context.PopParent();
			return result;
		}

		public static object Build_9(IServiceProvider P_0)
		{
			XamlIlContext.Context<App> context = CreateContext(P_0);
			SolidColorBrush solidColorBrush;
			SolidColorBrush result = (solidColorBrush = new SolidColorBrush());
			context.PushParent(solidColorBrush);
			StaticResourceExtension staticResourceExtension = new StaticResourceExtension("PaneActiveBackground");
			context.ProvideTargetProperty = SolidColorBrush.ColorProperty;
			object? obj = staticResourceExtension.ProvideValue(context);
			context.ProvideTargetProperty = null;
			XamlDynamicSetters._003C_003EXamlDynamicSetter_1(solidColorBrush, obj);
			context.PopParent();
			return result;
		}

		public static object Build_10(IServiceProvider P_0)
		{
			XamlIlContext.Context<App> context = CreateContext(P_0);
			return new SolidColorBrush
			{
				Color = Color.FromUInt32(4281020470u)
			};
		}

		public static object Build_11(IServiceProvider P_0)
		{
			XamlIlContext.Context<App> context = CreateContext(P_0);
			return new SolidColorBrush
			{
				Color = Color.FromUInt32(4287995301u)
			};
		}

		public static object Build_12(IServiceProvider P_0)
		{
			XamlIlContext.Context<App> context = CreateContext(P_0);
			return new SolidColorBrush
			{
				Color = Color.FromUInt32(4280823348u)
			};
		}

		public static object Build_13(IServiceProvider P_0)
		{
			XamlIlContext.Context<App> context = CreateContext(P_0);
			return new SolidColorBrush
			{
				Color = Color.FromUInt32(4282666326u)
			};
		}

		public static object Build_14(IServiceProvider P_0)
		{
			XamlIlContext.Context<App> context = CreateContext(P_0);
			return new SolidColorBrush
			{
				Color = Color.FromUInt32(16777215u)
			};
		}

		public static object Build_15(IServiceProvider P_0)
		{
			XamlIlContext.Context<App> context = CreateContext(P_0);
			return new LocalizedCountTextConverter();
		}

		public static object Build_16(IServiceProvider P_0)
		{
			XamlIlContext.Context<App> context = CreateContext(P_0);
			return new LayoutModeToVMConverter();
		}

		public static object Build_17(IServiceProvider P_0)
		{
			XamlIlContext.Context<App> context = CreateContext(P_0);
			return new PaneBackgroundConverter();
		}
	}

	private IAppThemeService? _appThemeService;

	private ICommandManager? _commands;

	private NativeMenuItem? _aboutMenuItem;

	private NativeMenuItem? _settingsMenuItem;

	[CompilerGenerated]
	private static Action<object> _0021XamlIlPopulateOverride;

	public static StorageHistoryWrapper HistoryWrapper { get; private set; }

	public static AppModel AppModel { get; private set; }

	public static ILogger Logger { get; private set; }

	public override void Initialize()
	{
		_0021XamlIlPopulateTrampoline(this);
		Logger = Ioc.Default.GetRequiredService<ILogger<App>>();
		AppModel = Ioc.Default.GetRequiredService<AppModel>();
		HistoryWrapper = Ioc.Default.GetRequiredService<StorageHistoryWrapper>();
		_appThemeService = Ioc.Default.GetRequiredService<IAppThemeService>();
		_commands = Ioc.Default.GetRequiredService<ICommandManager>();
		_appThemeService.ApplySavedTheme();
		IGeneralSettingsService requiredService = Ioc.Default.GetRequiredService<IGeneralSettingsService>();
		ApplyGlobalFontSize(requiredService.FontSize);
		requiredService.PropertyChanged += GeneralSettingsServiceOnPropertyChanged;
		AppLanguageHelper.Initialize(requiredService);
		AppLanguageHelper.ApplySavedLanguage();
		LocalizationBindingSource.Instance.Initialize();
		InitializeApplicationMenu();
		AppLanguageHelper.LanguageChanged += (object? _, EventArgs _) =>
		{
			UpdateApplicationMenuLocalization();
		};
	}

	private void GeneralSettingsServiceOnPropertyChanged(object? sender, PropertyChangedEventArgs e)
	{
		if (e.PropertyName == "FontSize" && sender is IGeneralSettingsService generalSettingsService)
		{
			ApplyGlobalFontSize(generalSettingsService.FontSize);
		}
	}

	private void ApplyGlobalFontSize(int fontSize)
	{
		int clamped = Math.Clamp(fontSize, 9, 24);
		Avalonia.Threading.Dispatcher.UIThread.Post(() =>
		{
			Resources["GlobalFontSize"] = (double)clamped;
		});
	}

	private void InitializeApplicationMenu()
	{
		_aboutMenuItem = new NativeMenuItem(Files.Assets.Lang.Resources.About);
		_aboutMenuItem.Click += OnAboutMenuClick;
		_settingsMenuItem = new NativeMenuItem(Files.Assets.Lang.Resources.Settings);
		_settingsMenuItem.Gesture = GetCommandGesture(CommandCodes.OpenSettings);
		_settingsMenuItem.Click += OnOpenSettingsClick;
		NativeMenu.SetMenu(this, new NativeMenu
		{
			Items = 
			{
				(NativeMenuItemBase)_aboutMenuItem,
				(NativeMenuItemBase)_settingsMenuItem
			}
		});
	}

	private void UpdateApplicationMenuLocalization()
	{
		if (_aboutMenuItem != null)
		{
			_aboutMenuItem.Header = Files.Assets.Lang.Resources.About;
		}
		if (_settingsMenuItem != null)
		{
			_settingsMenuItem.Header = Files.Assets.Lang.Resources.Settings;
		}
	}

	private KeyGesture? GetCommandGesture(CommandCodes commandCode)
	{
		if (_commands == null)
		{
			return null;
		}
		return ShortcutHelpers.GetPrimaryKeyGesture(_commands[commandCode].HotKeys);
	}

	public override void OnFrameworkInitializationCompleted()
	{
		if (Design.IsDesignMode)
		{
			if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime classicDesktopStyleApplicationLifetime)
			{
				classicDesktopStyleApplicationLifetime.MainWindow = new MainWindow
				{
					DataContext = new MainWindowViewModel()
				};
			}
		}
		else if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime classicDesktopStyleApplicationLifetime2)
		{
			FileThumbnailHelper.EnsureDefaultIconsPreloadedAsync();
			classicDesktopStyleApplicationLifetime2.MainWindow = new MainWindow();
			classicDesktopStyleApplicationLifetime2.MainWindow.Closed += Window_Closed;
			classicDesktopStyleApplicationLifetime2.MainWindow.Activated += Window_Activated;
			if (Application.Current?.TryGetFeature(typeof(IActivatableLifetime)) is IActivatableLifetime activatableLifetime)
			{
				activatableLifetime.Activated += OnAppActivated;
			}
		}
		base.OnFrameworkInitializationCompleted();
	}

	private void Window_Closed(object? sender, EventArgs e)
	{
		Ioc.Default.GetService<IMultiPanesContext>()?.PanesView?.SaveState();
		FileThumbnailHelper.SavePersistentCache();
	}

	private async void Window_Activated(object? sender, EventArgs e)
	{
	}

	private void OnAppActivated(object? sender, ActivatedEventArgs e)
	{
		if (!(e is FileActivatedEventArgs e2))
		{
			return;
		}
		foreach (IStorageItem file in e2.Files)
		{
			string localPath = file.Path.LocalPath;
			if (FileExtensionHelpers.IsValidXFolderWorkspaceFile(localPath))
			{
				TryOpenWorkspace(localPath);
				break;
			}
		}
	}

	private void TryOpenWorkspace(string? filePath)
	{
		if (!(ApplicationLifetime is IClassicDesktopStyleApplicationLifetime classicDesktopStyleApplicationLifetime))
		{
			return;
		}
		Window mainWindow = classicDesktopStyleApplicationLifetime.MainWindow;
		MainWindow mainWindow2 = mainWindow as MainWindow;
		if (mainWindow2 != null)
		{
			Avalonia.Threading.Dispatcher.UIThread.Post(() =>
			{
				mainWindow2.OpenWorkspaceFromFile(filePath);
			});
		}
	}

	private void OnOpenSettingsClick(object? sender, EventArgs e)
	{
		if (!(ApplicationLifetime is IClassicDesktopStyleApplicationLifetime classicDesktopStyleApplicationLifetime))
		{
			return;
		}
		foreach (Window window in classicDesktopStyleApplicationLifetime.Windows)
		{
			if (window is SettingsWindow settingsWindow)
			{
				settingsWindow.Activate();
				return;
			}
		}
		SettingsWindow settingsWindow2 = new SettingsWindow();
		Window mainWindow = classicDesktopStyleApplicationLifetime.MainWindow;
		if (mainWindow != null)
		{
			settingsWindow2.ShowDialog(mainWindow);
		}
		else
		{
			settingsWindow2.Show();
		}
	}

	private async void OnAboutMenuClick(object? sender, EventArgs e)
	{
		try
		{
			Assembly? element = Assembly.GetEntryAssembly() ?? Assembly.GetExecutingAssembly();
			string text = element.GetCustomAttribute<AssemblyProductAttribute>()?.Product;
			string text2 = element.GetCustomAttribute<AssemblyFileVersionAttribute>()?.Version;
			await DialogDisplayHelper.ShowTipAsync(Files.Assets.Lang.Resources.About, text + " " + text2);
		}
		catch (Exception)
		{
		}
	}

	[CompilerGenerated]
	private unsafe static void _0021XamlIlPopulate(IServiceProvider P_0, App P_1)
	{
		XamlIlContext.Context<App> context = new XamlIlContext.Context<App>(P_0, new object[1] { _0021AvaloniaResources.NamespaceInfo_003A_002FApp_002Eaxaml.Singleton }, "avares://XFolder/App.axaml")
		{
			RootObject = P_1,
			IntermediateRoot = P_1
		};
		App app2;
		App app = (app2 = P_1);
		context.PushParent(app2);
		app2.RequestedThemeVariant = ThemeVariant.Default;
		app2.Name = "XFolder";
		app2.DataTemplates.Add(new ViewLocator());
		app2.Styles.Add(new FluentTheme(context));
		IServiceProvider dataGridThemeContext = XamlIlRuntimeHelpers.CreateRootServiceProviderV3(context);
		Type? dataGridResourcesType = typeof(Avalonia.Controls.DataGrid).Assembly.GetType("CompiledAvaloniaXaml.!AvaloniaResources");
		MethodInfo? buildDataGridTheme = dataGridResourcesType?.GetMethod("Build:/Themes/Fluent.xaml");
		if (buildDataGridTheme?.Invoke(null, new object[] { dataGridThemeContext }) is Styles dataGridStyles)
		{
			app2.Styles.Add(dataGridStyles);
		}
		app2.Styles.Add(new DialogHostStyles());
		Styles styles = app2.Styles;
		Style style;
		Style item = (style = new Style());
		context.PushParent(style);
		style.Selector = ((Selector?)null).Is(typeof(Control));
		Setter setter2;
		Setter setter = (setter2 = new Setter());
		context.PushParent(setter2);
		setter2.Property = TextElement.FontSizeProperty;
		DynamicResourceExtension dynamicResourceExtension = new DynamicResourceExtension("GlobalFontSize");
		context.ProvideTargetProperty = XamlIlHelpers.Avalonia_002EStyling_002ESetter_002CAvalonia_002EBase_002EValue_0021Property();
		BindingBase value = dynamicResourceExtension.ProvideValue(context);
		context.ProvideTargetProperty = null;
		setter2.Value = value;
		context.PopParent();
		style.Add(setter);
		context.PopParent();
		styles.Add(item);
		Styles styles2 = app2.Styles;
		Style style2 = new Style();
		style2.Selector = ((Selector?)null).OfType(typeof(TreeViewItem)).Template().OfType(typeof(Border))
			.Name("PART_SelectionIndicator");
		Setter setter3 = new Setter();
		setter3.Property = Visual.IsVisibleProperty;
		setter3.Value = false;
		style2.Add(setter3);
		styles2.Add(style2);
		Styles styles3 = app2.Styles;
		Style style3 = new Style();
		style3.Selector = ((Selector?)null).OfType(typeof(TreeViewItem));
		Setter setter4 = new Setter();
		setter4.Property = Layoutable.MarginProperty;
		setter4.Value = new Thickness(4.0, 1.0, 4.0, 1.0);
		style3.Add(setter4);
		styles3.Add(style3);
		ResourceDictionary resourceDictionary;
		ResourceDictionary resources = (resourceDictionary = new ResourceDictionary());
		context.PushParent(resourceDictionary);
		ResourceDictionary resourceDictionary2 = resourceDictionary;
		if (resourceDictionary2 is ResourceDictionary resourceDictionary3)
		{
			resourceDictionary3.EnsureCapacity(resourceDictionary3.Count + 9);
		}
		resourceDictionary2.MergedDictionaries.Add(_0021AvaloniaResources.Build_003A_002FAssets_002Ficons_002Eaxaml(XamlIlRuntimeHelpers.CreateRootServiceProviderV3(context)));
		IDictionary<ThemeVariant, IThemeVariantProvider> themeDictionaries = resourceDictionary2.ThemeDictionaries;
		ThemeVariant light = ThemeVariant.Light;
		ResourceDictionary value2 = (resourceDictionary = new ResourceDictionary());
		context.PushParent(resourceDictionary);
		ResourceDictionary resourceDictionary4 = resourceDictionary;
		if (resourceDictionary4 is ResourceDictionary resourceDictionary5)
		{
			resourceDictionary5.EnsureCapacity(resourceDictionary5.Count + 29);
		}
		((IThemeVariantProvider)resourceDictionary4).Key = ThemeVariant.Light;
		((IDictionary<object, object>)resourceDictionary4).Add((object)"SystemAccentColor", (object)Color.FromUInt32(4278413014u));
		((IDictionary<object, object>)resourceDictionary4).Add((object)"SystemAccentColorDark1", (object)Color.FromUInt32(4278213829u));
		((IDictionary<object, object>)resourceDictionary4).Add((object)"SystemAccentColorDark2", (object)Color.FromUInt32(4278469257u));
		((IDictionary<object, object>)resourceDictionary4).Add((object)"SystemAccentColorLight1", (object)Color.FromUInt32(4280387839u));
		((IDictionary<object, object>)resourceDictionary4).Add((object)"SystemAccentColorLight2", (object)Color.FromUInt32(4292603391u));
		resourceDictionary4.AddDeferred("SystemAccentColorBrush", XamlIlRuntimeHelpers.DeferredTransformationFactoryV3<object>((nint)(delegate*<IServiceProvider, object>)(&XamlClosure_1.Build_1), context));
		resourceDictionary4.AddDeferred("TreeViewItemSelectedBackgroundBrush", XamlIlRuntimeHelpers.DeferredTransformationFactoryV3<object>((nint)(delegate*<IServiceProvider, object>)(&XamlClosure_1.Build_2), context));
		((IDictionary<object, object>)resourceDictionary4).Add((object)"TabHeaderBorder", (object)Color.FromUInt32(4292994280u));
		((IDictionary<object, object>)resourceDictionary4).Add((object)"TabHeaderBackground", (object)Color.FromUInt32(4294375674u));
		((IDictionary<object, object>)resourceDictionary4).Add((object)"TabHeaderForeground", (object)Color.FromUInt32(4285166461u));
		((IDictionary<object, object>)resourceDictionary4).Add((object)"TabHeaderSelectedBackground", (object)Color.FromUInt32(uint.MaxValue));
		((IDictionary<object, object>)resourceDictionary4).Add((object)"TabHeaderSelectedForeground", (object)Color.FromUInt32(4281284157u));
		((IDictionary<object, object>)resourceDictionary4).Add((object)"TabHeaderPointerOverBackground", (object)Color.FromUInt32(uint.MaxValue));
		((IDictionary<object, object>)resourceDictionary4).Add((object)"TabHeaderPointerOverForeground", (object)Color.FromUInt32(4281284157u));
		((IDictionary<object, object>)resourceDictionary4).Add((object)"SidebarBackground", (object)Color.FromUInt32(4294375674u));
		((IDictionary<object, object>)resourceDictionary4).Add((object)"TopToolbarBackground", (object)Color.FromUInt32(4294375674u));
		((IDictionary<object, object>)resourceDictionary4).Add((object)"DialogBackground", (object)Color.FromUInt32(uint.MaxValue));
		((IDictionary<object, object>)resourceDictionary4).Add((object)"PaneActiveBackground", (object)Color.FromUInt32(uint.MaxValue));
		((IDictionary<object, object>)resourceDictionary4).Add((object)"PaneInactiveBackground", (object)Color.FromUInt32(4293455345u));
		((IDictionary<object, object>)resourceDictionary4).Add((object)"AddressBarBackground", (object)Color.FromUInt32(4294638588u));
		((IDictionary<object, object>)resourceDictionary4).Add((object)"AddressBarBorder", (object)Color.FromUInt32(4292994280u));
		((IDictionary<object, object>)resourceDictionary4).Add((object)"AddressBarForeground", (object)Color.FromUInt32(4281284157u));
		((IDictionary<object, object>)resourceDictionary4).Add((object)"BreadcrumbItemPointerOverBackground", (object)Color.FromUInt32(4293652724u));
		((IDictionary<object, object>)resourceDictionary4).Add((object)"BreadcrumbItemPressedBackground", (object)Color.FromUInt32(4293060073u));
		((IDictionary<object, object>)resourceDictionary4).Add((object)"BreadcrumbChevronForeground", (object)Color.FromUInt32(4285166461u));
		((IDictionary<object, object>)resourceDictionary4).Add((object)"GridSplitterBackground", (object)Color.FromUInt32(4292994280u));
		resourceDictionary4.AddDeferred("DialogBackgroundBrush", XamlIlRuntimeHelpers.DeferredTransformationFactoryV3<object>((nint)(delegate*<IServiceProvider, object>)(&XamlClosure_1.Build_3), context));
		resourceDictionary4.AddDeferred("PaneActiveBackgroundBrush", XamlIlRuntimeHelpers.DeferredTransformationFactoryV3<object>((nint)(delegate*<IServiceProvider, object>)(&XamlClosure_1.Build_4), context));
		resourceDictionary4.AddDeferred("DetailsListAlternatingRowBackgroundBrush", XamlIlRuntimeHelpers.DeferredTransformationFactoryV3<object>((nint)(delegate*<IServiceProvider, object>)(&XamlClosure_1.Build_5), context));
		context.PopParent();
		themeDictionaries.Add(light, value2);
		IDictionary<ThemeVariant, IThemeVariantProvider> themeDictionaries2 = resourceDictionary2.ThemeDictionaries;
		ThemeVariant dark = ThemeVariant.Dark;
		ResourceDictionary value3 = (resourceDictionary = new ResourceDictionary());
		context.PushParent(resourceDictionary);
		ResourceDictionary resourceDictionary6 = resourceDictionary;
		if (resourceDictionary6 is ResourceDictionary resourceDictionary7)
		{
			resourceDictionary7.EnsureCapacity(resourceDictionary7.Count + 33);
		}
		((IThemeVariantProvider)resourceDictionary6).Key = ThemeVariant.Dark;
		((IDictionary<object, object>)resourceDictionary6).Add((object)"SystemAccentColor", (object)Color.FromUInt32(4286167295u));
		((IDictionary<object, object>)resourceDictionary6).Add((object)"SystemAccentColorDark1", (object)Color.FromUInt32(4280387839u));
		((IDictionary<object, object>)resourceDictionary6).Add((object)"SystemAccentColorDark2", (object)Color.FromUInt32(4278413014u));
		((IDictionary<object, object>)resourceDictionary6).Add((object)"SystemAccentColorLight1", (object)Color.FromUInt32(4291355135u));
		((IDictionary<object, object>)resourceDictionary6).Add((object)"SystemAccentColorLight2", (object)Color.FromUInt32(4292603391u));
		resourceDictionary6.AddDeferred("SystemAccentColorBrush", XamlIlRuntimeHelpers.DeferredTransformationFactoryV3<object>((nint)(delegate*<IServiceProvider, object>)(&XamlClosure_1.Build_6), context));
		resourceDictionary6.AddDeferred("TreeViewItemSelectedBackgroundBrush", XamlIlRuntimeHelpers.DeferredTransformationFactoryV3<object>((nint)(delegate*<IServiceProvider, object>)(&XamlClosure_1.Build_7), context));
		((IDictionary<object, object>)resourceDictionary6).Add((object)"TabHeaderBorder", (object)Color.FromUInt32(4279967523u));
		((IDictionary<object, object>)resourceDictionary6).Add((object)"TabHeaderBackground", (object)Color.FromUInt32(4280230952u));
		((IDictionary<object, object>)resourceDictionary6).Add((object)"TabHeaderForeground", (object)Color.FromUInt32(4287995301u));
		((IDictionary<object, object>)resourceDictionary6).Add((object)"TabHeaderSelectedBackground", (object)Color.FromUInt32(4280559918u));
		((IDictionary<object, object>)resourceDictionary6).Add((object)"TabHeaderSelectedForeground", (object)Color.FromUInt32(4292994280u));
		((IDictionary<object, object>)resourceDictionary6).Add((object)"TabHeaderPointerOverBackground", (object)Color.FromUInt32(4280559918u));
		((IDictionary<object, object>)resourceDictionary6).Add((object)"TabHeaderPointerOverForeground", (object)Color.FromUInt32(4291941850u));
		((IDictionary<object, object>)resourceDictionary6).Add((object)"SidebarBackground", (object)Color.FromUInt32(4280230952u));
		((IDictionary<object, object>)resourceDictionary6).Add((object)"TopToolbarBackground", (object)Color.FromUInt32(4280230952u));
		((IDictionary<object, object>)resourceDictionary6).Add((object)"DialogBackground", (object)Color.FromUInt32(4280559918u));
		((IDictionary<object, object>)resourceDictionary6).Add((object)"PaneActiveBackground", (object)Color.FromUInt32(4280559918u));
		((IDictionary<object, object>)resourceDictionary6).Add((object)"PaneInactiveBackground", (object)Color.FromUInt32(4279704351u));
		((IDictionary<object, object>)resourceDictionary6).Add((object)"AddressBarBackground", (object)Color.FromUInt32(4281284157u));
		((IDictionary<object, object>)resourceDictionary6).Add((object)"AddressBarBorder", (object)Color.FromUInt32(4279967523u));
		((IDictionary<object, object>)resourceDictionary6).Add((object)"AddressBarForeground", (object)Color.FromUInt32(4292994280u));
		((IDictionary<object, object>)resourceDictionary6).Add((object)"BreadcrumbItemPointerOverBackground", (object)Color.FromUInt32(4280823348u));
		((IDictionary<object, object>)resourceDictionary6).Add((object)"BreadcrumbItemPressedBackground", (object)Color.FromUInt32(4281942346u));
		((IDictionary<object, object>)resourceDictionary6).Add((object)"BreadcrumbChevronForeground", (object)Color.FromUInt32(4287995301u));
		((IDictionary<object, object>)resourceDictionary6).Add((object)"GridSplitterBackground", (object)Color.FromUInt32(4279967523u));
		resourceDictionary6.AddDeferred("DialogBackgroundBrush", XamlIlRuntimeHelpers.DeferredTransformationFactoryV3<object>((nint)(delegate*<IServiceProvider, object>)(&XamlClosure_1.Build_8), context));
		resourceDictionary6.AddDeferred("PaneActiveBackgroundBrush", XamlIlRuntimeHelpers.DeferredTransformationFactoryV3<object>((nint)(delegate*<IServiceProvider, object>)(&XamlClosure_1.Build_9), context));
		resourceDictionary6.AddDeferred("DetailsListAlternatingRowBackgroundBrush", XamlIlRuntimeHelpers.DeferredTransformationFactoryV3<object>((nint)(delegate*<IServiceProvider, object>)(&XamlClosure_1.Build_10), context));
		resourceDictionary6.AddDeferred("SystemControlForegroundBaseMediumBrush", XamlIlRuntimeHelpers.DeferredTransformationFactoryV3<object>((nint)(delegate*<IServiceProvider, object>)(&XamlClosure_1.Build_11), context));
		((IDictionary<object, object>)resourceDictionary6).Add((object)"SystemBaseHighColor", (object)Color.FromUInt32(4292994280u));
		resourceDictionary6.AddDeferred("ButtonBackgroundPointerOver", XamlIlRuntimeHelpers.DeferredTransformationFactoryV3<object>((nint)(delegate*<IServiceProvider, object>)(&XamlClosure_1.Build_12), context));
		resourceDictionary6.AddDeferred("TabCloseButtonPointerOverBackground", XamlIlRuntimeHelpers.DeferredTransformationFactoryV3<object>((nint)(delegate*<IServiceProvider, object>)(&XamlClosure_1.Build_13), context));
		context.PopParent();
		themeDictionaries2.Add(dark, value3);
		((IDictionary<object, object>)resourceDictionary2).Add((object)"ControlCornerRadius", (object)new CornerRadius(8.0, 8.0, 8.0, 8.0));
		((IDictionary<object, object>)resourceDictionary2).Add((object)"OverlayCornerRadius", (object)new CornerRadius(12.0, 12.0, 12.0, 12.0));
		((IDictionary<object, object>)resourceDictionary2).Add((object)"TextControlCornerRadius", (object)new CornerRadius(8.0, 8.0, 8.0, 8.0));
		((IDictionary<object, object>)resourceDictionary2).Add((object)"GlobalFontSize", (object)14.0);
		((IDictionary<object, object>)resourceDictionary2).Add((object)"TreeViewItemSelectionIndicatorWidth", (object)0.0);
		resourceDictionary2.AddDeferred("DetailsListRowBackgroundBrush", XamlIlRuntimeHelpers.DeferredTransformationFactoryV3<object>((nint)(delegate*<IServiceProvider, object>)(&XamlClosure_1.Build_14), context));
		resourceDictionary2.AddDeferred("LocalizedCountTextConverter", XamlIlRuntimeHelpers.DeferredTransformationFactoryV3<object>((nint)(delegate*<IServiceProvider, object>)(&XamlClosure_1.Build_15), context));
		resourceDictionary2.AddDeferred("LayoutModeToVMConverter", XamlIlRuntimeHelpers.DeferredTransformationFactoryV3<object>((nint)(delegate*<IServiceProvider, object>)(&XamlClosure_1.Build_16), context));
		resourceDictionary2.AddDeferred("PaneBackgroundConverter", XamlIlRuntimeHelpers.DeferredTransformationFactoryV3<object>((nint)(delegate*<IServiceProvider, object>)(&XamlClosure_1.Build_17), context));
		context.PopParent();
		app2.Resources = resources;
		context.PopParent();
		context.AvaloniaNameScope.Complete();
	}

	[CompilerGenerated]
	private static void _0021XamlIlPopulateTrampoline(App P_0)
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
