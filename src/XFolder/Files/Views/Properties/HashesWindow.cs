using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Input.Platform;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Markup.Xaml;
using Avalonia.Markup.Xaml.MarkupExtensions;
using Avalonia.Markup.Xaml.Templates;
using Avalonia.Markup.Xaml.XamlIl.Runtime;
using Avalonia.Media;
using Avalonia.Platform.Storage;
using Avalonia.Threading;
using CompiledAvaloniaXaml;
using Files.Assets.Lang;
using Files.Data.Items;
using Files.Data.Models;
using Files.Services.App;
using Files.ViewModels.Properties;

namespace Files.Views.Properties;

public class HashesWindow : Window
{
	[CompilerGenerated]
	private class XamlClosure_9
	{
		public static object Build_1(IServiceProvider P_0)
		{
			XamlIlContext.Context<HashesWindow> context = CreateContext(P_0);
			context.IntermediateRoot = new Grid();
			object obj = context.IntermediateRoot;
			((ISupportInitialize)obj).BeginInit();
			Grid grid = (Grid)obj;
			context.PushParent(grid);
			grid.MinHeight = 48.0;
			ColumnDefinitions columnDefinitions = new ColumnDefinitions();
			columnDefinitions.Capacity = 3;
			columnDefinitions.Add(new ColumnDefinition(new GridLength(92.0, GridUnitType.Pixel)));
			columnDefinitions.Add(new ColumnDefinition(new GridLength(1.0, GridUnitType.Star)));
			columnDefinitions.Add(new ColumnDefinition(new GridLength(0.0, GridUnitType.Auto)));
			grid.ColumnDefinitions = columnDefinitions;
			grid.Margin = new Thickness(16.0, 0.0, 12.0, 0.0);
			Controls children = grid.Children;
			TextBlock textBlock2;
			TextBlock textBlock = (textBlock2 = new TextBlock());
			((ISupportInitialize)textBlock).BeginInit();
			children.Add(textBlock);
			TextBlock textBlock4;
			TextBlock textBlock3 = (textBlock4 = textBlock2);
			context.PushParent(textBlock4);
			TextBlock textBlock5 = textBlock4;
			Grid.SetColumn(textBlock5, 0);
			textBlock5.VerticalAlignment = VerticalAlignment.Center;
			textBlock5.FontWeight = FontWeight.DemiBold;
			StyledProperty<string?> textProperty = TextBlock.TextProperty;
			ReflectionBindingExtension reflectionBindingExtension = new ReflectionBindingExtension("Algorithm");
			context.ProvideTargetProperty = TextBlock.TextProperty;
			ReflectionBinding binding = reflectionBindingExtension.ProvideValue(context);
			context.ProvideTargetProperty = null;
			textBlock5.Bind(textProperty, binding);
			context.PopParent();
			((ISupportInitialize)textBlock3).EndInit();
			Controls children2 = grid.Children;
			StackPanel stackPanel2;
			StackPanel stackPanel = (stackPanel2 = new StackPanel());
			((ISupportInitialize)stackPanel).BeginInit();
			children2.Add(stackPanel);
			StackPanel stackPanel4;
			StackPanel stackPanel3 = (stackPanel4 = stackPanel2);
			context.PushParent(stackPanel4);
			Grid.SetColumn(stackPanel4, 1);
			stackPanel4.Orientation = Orientation.Horizontal;
			stackPanel4.VerticalAlignment = VerticalAlignment.Center;
			StyledProperty<bool> isVisibleProperty = Visual.IsVisibleProperty;
			ReflectionBindingExtension reflectionBindingExtension2 = new ReflectionBindingExtension("IsCalculating");
			context.ProvideTargetProperty = Visual.IsVisibleProperty;
			ReflectionBinding binding2 = reflectionBindingExtension2.ProvideValue(context);
			context.ProvideTargetProperty = null;
			stackPanel4.Bind(isVisibleProperty, binding2);
			Controls children3 = stackPanel4.Children;
			TextBlock textBlock7;
			TextBlock textBlock6 = (textBlock7 = new TextBlock());
			((ISupportInitialize)textBlock6).BeginInit();
			children3.Add(textBlock6);
			TextBlock textBlock8 = (textBlock4 = textBlock7);
			context.PushParent(textBlock4);
			TextBlock textBlock9 = textBlock4;
			textBlock9.VerticalAlignment = VerticalAlignment.Center;
			StyledProperty<string?> textProperty2 = TextBlock.TextProperty;
			ReflectionBindingExtension reflectionBindingExtension3 = new ReflectionBindingExtension("[Calculating]")
			{
				Source = LocalizationBindingSource.Instance
			};
			context.ProvideTargetProperty = TextBlock.TextProperty;
			ReflectionBinding binding3 = reflectionBindingExtension3.ProvideValue(context);
			context.ProvideTargetProperty = null;
			textBlock9.Bind(textProperty2, binding3);
			context.PopParent();
			((ISupportInitialize)textBlock8).EndInit();
			Controls children4 = stackPanel4.Children;
			ProgressBar progressBar2;
			ProgressBar progressBar = (progressBar2 = new ProgressBar());
			((ISupportInitialize)progressBar).BeginInit();
			children4.Add(progressBar);
			progressBar2.Width = 120.0;
			progressBar2.Margin = new Thickness(12.0, 0.0, 0.0, 0.0);
			progressBar2.VerticalAlignment = VerticalAlignment.Center;
			progressBar2.IsIndeterminate = true;
			((ISupportInitialize)progressBar2).EndInit();
			context.PopParent();
			((ISupportInitialize)stackPanel3).EndInit();
			Controls children5 = grid.Children;
			TextBlock textBlock11;
			TextBlock textBlock10 = (textBlock11 = new TextBlock());
			((ISupportInitialize)textBlock10).BeginInit();
			children5.Add(textBlock10);
			TextBlock textBlock12 = (textBlock4 = textBlock11);
			context.PushParent(textBlock4);
			TextBlock textBlock13 = textBlock4;
			Grid.SetColumn(textBlock13, 1);
			textBlock13.VerticalAlignment = VerticalAlignment.Center;
			StyledProperty<bool> isVisibleProperty2 = Visual.IsVisibleProperty;
			ReflectionBindingExtension reflectionBindingExtension4 = new ReflectionBindingExtension("!IsCalculating");
			context.ProvideTargetProperty = Visual.IsVisibleProperty;
			ReflectionBinding binding4 = reflectionBindingExtension4.ProvideValue(context);
			context.ProvideTargetProperty = null;
			textBlock13.Bind(isVisibleProperty2, binding4);
			StyledProperty<string?> textProperty3 = TextBlock.TextProperty;
			ReflectionBindingExtension reflectionBindingExtension5 = new ReflectionBindingExtension("DisplayHashValue");
			context.ProvideTargetProperty = TextBlock.TextProperty;
			ReflectionBinding binding5 = reflectionBindingExtension5.ProvideValue(context);
			context.ProvideTargetProperty = null;
			textBlock13.Bind(textProperty3, binding5);
			textBlock13.TextWrapping = TextWrapping.NoWrap;
			ReflectionBindingExtension reflectionBindingExtension6 = new ReflectionBindingExtension("HashValue");
			context.ProvideTargetProperty = ToolTip.TipProperty;
			ReflectionBinding reflectionBinding = reflectionBindingExtension6.ProvideValue(context);
			context.ProvideTargetProperty = null;
			XamlDynamicSetters._003C_003EXamlDynamicSetter_9(textBlock13, reflectionBinding);
			context.PopParent();
			((ISupportInitialize)textBlock12).EndInit();
			Controls children6 = grid.Children;
			Button button2;
			Button button = (button2 = new Button());
			((ISupportInitialize)button).BeginInit();
			children6.Add(button);
			Button button4;
			Button button3 = (button4 = button2);
			context.PushParent(button4);
			Grid.SetColumn(button4, 2);
			button4.Margin = new Thickness(8.0, 0.0, 0.0, 0.0);
			button4.Padding = new Thickness(10.0, 4.0, 10.0, 4.0);
			button4.VerticalAlignment = VerticalAlignment.Center;
			StyledProperty<bool> isVisibleProperty3 = Visual.IsVisibleProperty;
			ReflectionBindingExtension reflectionBindingExtension7 = new ReflectionBindingExtension("IsCalculated");
			context.ProvideTargetProperty = Visual.IsVisibleProperty;
			ReflectionBinding binding6 = reflectionBindingExtension7.ProvideValue(context);
			context.ProvideTargetProperty = null;
			button4.Bind(isVisibleProperty3, binding6);
			button4.AddHandler((RoutedEvent)Button.ClickEvent, (Delegate)new EventHandler<RoutedEventArgs>(context.RootObject.CopyHashButton_Click), RoutingStrategies.Direct | RoutingStrategies.Bubble, false);
			ReflectionBindingExtension reflectionBindingExtension8 = new ReflectionBindingExtension("[Copy]")
			{
				Source = LocalizationBindingSource.Instance
			};
			context.ProvideTargetProperty = ContentControl.ContentProperty;
			ReflectionBinding reflectionBinding2 = reflectionBindingExtension8.ProvideValue(context);
			context.ProvideTargetProperty = null;
			XamlDynamicSetters._003C_003EXamlDynamicSetter_8(button4, reflectionBinding2);
			context.PopParent();
			((ISupportInitialize)button3).EndInit();
			context.PopParent();
			((ISupportInitialize)obj).EndInit();
			return obj;
		}

		public static XamlIlContext.Context<HashesWindow> CreateContext(IServiceProvider P_0)
		{
			XamlIlContext.Context<HashesWindow> context = new XamlIlContext.Context<HashesWindow>(P_0, new object[1] { _0021AvaloniaResources.NamespaceInfo_003A_002FViews_002FProperties_002FHashesWindow_002Eaxaml.Singleton }, "avares://XFolder/Views/Properties/HashesWindow.axaml");
			if (P_0 != null)
			{
				object service = P_0.GetService(typeof(IRootObjectProvider));
				if (service != null)
				{
					service = ((IRootObjectProvider)service).RootObject;
					context.RootObject = (HashesWindow)service;
				}
			}
			return context;
		}
	}

	[GeneratedCode("Avalonia.Generators.NameGenerator.InitializeComponentCodeGenerator", "12.0.1.0")]
	internal TextBox HashInputTextBox;

	[CompilerGenerated]
	private static Action<object> _0021XamlIlPopulateOverride;

	public HashesWindow()
	{
		InitializeComponent();
		Opened += HashesWindow_Opened;
	}

	public HashesWindow(ListedItem item)
		: this()
	{
		DataContext = new HashesViewModel(item);
	}

	protected override void OnClosed(EventArgs e)
	{
		Opened -= HashesWindow_Opened;
		if (DataContext is IDisposable disposable)
		{
			disposable.Dispose();
		}
		base.OnClosed(e);
	}

	private void HashesWindow_Opened(object? sender, EventArgs e)
	{
		Avalonia.Threading.Dispatcher.UIThread.Post(() =>
		{
			Activate();
			Focus();
			HashInputTextBox.Focus();
		}, DispatcherPriority.Background);
	}

	protected override void OnKeyDown(KeyEventArgs e)
	{
		if (e.Key == Key.Escape)
		{
			Close();
			e.Handled = true;
		}
		else
		{
			base.OnKeyDown(e);
		}
	}

	private async void CompareFileButton_Click(object? sender, RoutedEventArgs e)
	{
		object dataContext = DataContext;
		if (dataContext is HashesViewModel viewModel)
		{
			IReadOnlyList<IStorageFile> readOnlyList = await StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
			{
				Title = Files.Assets.Lang.Resources.CompareFile,
				AllowMultiple = false,
				FileTypeFilter = new _003C_003Ez__ReadOnlySingleElementList<FilePickerFileType>(FilePickerFileTypes.All)
			});
			if (readOnlyList.Count != 0)
			{
				await viewModel.CompareFileAsync(readOnlyList[0].Path.LocalPath);
			}
		}
	}

	private async void CopyHashButton_Click(object? sender, RoutedEventArgs e)
	{
		if (sender is Button { DataContext: HashInfoItem dataContext } && !string.IsNullOrWhiteSpace(dataContext.HashValue))
		{
			IClipboard clipboard = TopLevel.GetTopLevel(this)?.Clipboard;
			if (clipboard != null)
			{
				await clipboard.SetTextAsync(dataContext.HashValue);
			}
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
		HashInputTextBox = ((nameScope != null) ? nameScope.Find<TextBox>("HashInputTextBox") : null);
	}

	[CompilerGenerated]
	private unsafe static void _0021XamlIlPopulate(IServiceProvider P_0, HashesWindow P_1)
	{
		XamlIlContext.Context<HashesWindow> context = new XamlIlContext.Context<HashesWindow>(P_0, new object[1] { _0021AvaloniaResources.NamespaceInfo_003A_002FViews_002FProperties_002FHashesWindow_002Eaxaml.Singleton }, "avares://XFolder/Views/Properties/HashesWindow.axaml")
		{
			RootObject = P_1,
			IntermediateRoot = P_1
		};
		((ISupportInitialize)P_1).BeginInit();
		context.PushParent(P_1);
		P_1.Width = 760.0;
		P_1.Height = 520.0;
		P_1.MinWidth = 620.0;
		P_1.MinHeight = 420.0;
		P_1.CanResize = true;
		P_1.WindowStartupLocation = WindowStartupLocation.CenterOwner;
		P_1.WindowDecorations = WindowDecorations.Full;
		StyledProperty<string?> titleProperty = Window.TitleProperty;
		ReflectionBindingExtension reflectionBindingExtension = new ReflectionBindingExtension("[Hashes]")
		{
			Source = LocalizationBindingSource.Instance
		};
		context.ProvideTargetProperty = Window.TitleProperty;
		ReflectionBinding binding = reflectionBindingExtension.ProvideValue(context);
		context.ProvideTargetProperty = null;
		P_1.Bind(titleProperty, binding);
		StyledProperty<IBrush?> backgroundProperty = TemplatedControl.BackgroundProperty;
		DynamicResourceExtension dynamicResourceExtension = new DynamicResourceExtension("DialogBackgroundBrush");
		context.ProvideTargetProperty = TemplatedControl.BackgroundProperty;
		BindingBase binding2 = dynamicResourceExtension.ProvideValue(context);
		context.ProvideTargetProperty = null;
		P_1.Bind(backgroundProperty, binding2);
		Grid grid2;
		Grid grid = (grid2 = new Grid());
		((ISupportInitialize)grid).BeginInit();
		P_1.Content = grid;
		Grid grid4;
		Grid grid3 = (grid4 = grid2);
		context.PushParent(grid4);
		Grid grid5 = grid4;
		RowDefinitions rowDefinitions = new RowDefinitions();
		rowDefinitions.Capacity = 3;
		rowDefinitions.Add(new RowDefinition(new GridLength(0.0, GridUnitType.Auto)));
		rowDefinitions.Add(new RowDefinition(new GridLength(0.0, GridUnitType.Auto)));
		rowDefinitions.Add(new RowDefinition(new GridLength(1.0, GridUnitType.Star)));
		grid5.RowDefinitions = rowDefinitions;
		grid5.Margin = new Thickness(20.0, 20.0, 20.0, 20.0);
		Controls children = grid5.Children;
		StackPanel stackPanel2;
		StackPanel stackPanel = (stackPanel2 = new StackPanel());
		((ISupportInitialize)stackPanel).BeginInit();
		children.Add(stackPanel);
		StackPanel stackPanel4;
		StackPanel stackPanel3 = (stackPanel4 = stackPanel2);
		context.PushParent(stackPanel4);
		Grid.SetRow(stackPanel4, 0);
		stackPanel4.Spacing = 12.0;
		Controls children2 = stackPanel4.Children;
		TextBlock textBlock2;
		TextBlock textBlock = (textBlock2 = new TextBlock());
		((ISupportInitialize)textBlock).BeginInit();
		children2.Add(textBlock);
		TextBlock textBlock4;
		TextBlock textBlock3 = (textBlock4 = textBlock2);
		context.PushParent(textBlock4);
		TextBlock textBlock5 = textBlock4;
		StyledProperty<string?> textProperty = TextBlock.TextProperty;
		ReflectionBindingExtension reflectionBindingExtension2 = new ReflectionBindingExtension("ItemName");
		context.ProvideTargetProperty = TextBlock.TextProperty;
		ReflectionBinding binding3 = reflectionBindingExtension2.ProvideValue(context);
		context.ProvideTargetProperty = null;
		textBlock5.Bind(textProperty, binding3);
		textBlock5.FontSize = 20.0;
		textBlock5.FontWeight = FontWeight.DemiBold;
		textBlock5.TextTrimming = TextTrimming.CharacterEllipsis;
		context.PopParent();
		((ISupportInitialize)textBlock3).EndInit();
		Controls children3 = stackPanel4.Children;
		Border border2;
		Border border = (border2 = new Border());
		((ISupportInitialize)border).BeginInit();
		children3.Add(border);
		Border border4;
		Border border3 = (border4 = border2);
		context.PushParent(border4);
		Border border5 = border4;
		border5.Padding = new Thickness(12.0, 12.0, 12.0, 12.0);
		StyledProperty<IBrush?> backgroundProperty2 = Border.BackgroundProperty;
		DynamicResourceExtension dynamicResourceExtension2 = new DynamicResourceExtension("PaneActiveBackgroundBrush");
		context.ProvideTargetProperty = Border.BackgroundProperty;
		BindingBase binding4 = dynamicResourceExtension2.ProvideValue(context);
		context.ProvideTargetProperty = null;
		border5.Bind(backgroundProperty2, binding4);
		StyledProperty<IBrush?> borderBrushProperty = Border.BorderBrushProperty;
		DynamicResourceExtension dynamicResourceExtension3 = new DynamicResourceExtension("TopToolbarBackground");
		context.ProvideTargetProperty = Border.BorderBrushProperty;
		BindingBase binding5 = dynamicResourceExtension3.ProvideValue(context);
		context.ProvideTargetProperty = null;
		border5.Bind(borderBrushProperty, binding5);
		border5.BorderThickness = new Thickness(1.0, 1.0, 1.0, 1.0);
		border5.CornerRadius = new CornerRadius(8.0, 8.0, 8.0, 8.0);
		Grid grid7;
		Grid grid6 = (grid7 = new Grid());
		((ISupportInitialize)grid6).BeginInit();
		border5.Child = grid6;
		Grid grid8 = (grid4 = grid7);
		context.PushParent(grid4);
		Grid grid9 = grid4;
		ColumnDefinitions columnDefinitions = new ColumnDefinitions();
		columnDefinitions.Capacity = 2;
		columnDefinitions.Add(new ColumnDefinition(new GridLength(1.0, GridUnitType.Star)));
		columnDefinitions.Add(new ColumnDefinition(new GridLength(0.0, GridUnitType.Auto)));
		grid9.ColumnDefinitions = columnDefinitions;
		Controls children4 = grid9.Children;
		TextBox textBox2;
		TextBox textBox = (textBox2 = new TextBox());
		((ISupportInitialize)textBox).BeginInit();
		children4.Add(textBox);
		TextBox textBox4;
		TextBox textBox3 = (textBox4 = textBox2);
		context.PushParent(textBox4);
		Grid.SetColumn(textBox4, 0);
		textBox4.Name = "HashInputTextBox";
		object element = textBox4;
		context.AvaloniaNameScope.Register("HashInputTextBox", element);
		StyledProperty<string?> placeholderTextProperty = TextBox.PlaceholderTextProperty;
		ReflectionBindingExtension reflectionBindingExtension3 = new ReflectionBindingExtension("[EnterHashToCompare]")
		{
			Source = LocalizationBindingSource.Instance
		};
		context.ProvideTargetProperty = TextBox.PlaceholderTextProperty;
		ReflectionBinding binding6 = reflectionBindingExtension3.ProvideValue(context);
		context.ProvideTargetProperty = null;
		textBox4.Bind(placeholderTextProperty, binding6);
		StyledProperty<string?> textProperty2 = TextBox.TextProperty;
		ReflectionBindingExtension reflectionBindingExtension4 = new ReflectionBindingExtension("HashInput")
		{
			UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
		};
		context.ProvideTargetProperty = TextBox.TextProperty;
		ReflectionBinding binding7 = reflectionBindingExtension4.ProvideValue(context);
		context.ProvideTargetProperty = null;
		textBox4.Bind(textProperty2, binding7);
		context.PopParent();
		((ISupportInitialize)textBox3).EndInit();
		Controls children5 = grid9.Children;
		Button button2;
		Button button = (button2 = new Button());
		((ISupportInitialize)button).BeginInit();
		children5.Add(button);
		Button button4;
		Button button3 = (button4 = button2);
		context.PushParent(button4);
		Grid.SetColumn(button4, 1);
		button4.Margin = new Thickness(8.0, 0.0, 0.0, 0.0);
		button4.VerticalAlignment = VerticalAlignment.Center;
		button4.AddHandler((RoutedEvent)Button.ClickEvent, (Delegate)new EventHandler<RoutedEventArgs>(context.RootObject.CompareFileButton_Click), RoutingStrategies.Direct | RoutingStrategies.Bubble, false);
		ReflectionBindingExtension reflectionBindingExtension5 = new ReflectionBindingExtension("[CompareFile]")
		{
			Source = LocalizationBindingSource.Instance
		};
		context.ProvideTargetProperty = ContentControl.ContentProperty;
		ReflectionBinding reflectionBinding = reflectionBindingExtension5.ProvideValue(context);
		context.ProvideTargetProperty = null;
		XamlDynamicSetters._003C_003EXamlDynamicSetter_8(button4, reflectionBinding);
		context.PopParent();
		((ISupportInitialize)button3).EndInit();
		context.PopParent();
		((ISupportInitialize)grid8).EndInit();
		context.PopParent();
		((ISupportInitialize)border3).EndInit();
		context.PopParent();
		((ISupportInitialize)stackPanel3).EndInit();
		Controls children6 = grid5.Children;
		Border border7;
		Border border6 = (border7 = new Border());
		((ISupportInitialize)border6).BeginInit();
		children6.Add(border6);
		Border border8 = (border4 = border7);
		context.PushParent(border4);
		Border border9 = border4;
		Grid.SetRow(border9, 1);
		border9.Margin = new Thickness(0.0, 12.0, 0.0, 0.0);
		border9.Padding = new Thickness(12.0, 12.0, 12.0, 12.0);
		StyledProperty<IBrush?> backgroundProperty3 = Border.BackgroundProperty;
		DynamicResourceExtension dynamicResourceExtension4 = new DynamicResourceExtension("PaneActiveBackgroundBrush");
		context.ProvideTargetProperty = Border.BackgroundProperty;
		BindingBase binding8 = dynamicResourceExtension4.ProvideValue(context);
		context.ProvideTargetProperty = null;
		border9.Bind(backgroundProperty3, binding8);
		StyledProperty<IBrush?> borderBrushProperty2 = Border.BorderBrushProperty;
		DynamicResourceExtension dynamicResourceExtension5 = new DynamicResourceExtension("TopToolbarBackground");
		context.ProvideTargetProperty = Border.BorderBrushProperty;
		BindingBase binding9 = dynamicResourceExtension5.ProvideValue(context);
		context.ProvideTargetProperty = null;
		border9.Bind(borderBrushProperty2, binding9);
		border9.BorderThickness = new Thickness(1.0, 1.0, 1.0, 1.0);
		border9.CornerRadius = new CornerRadius(8.0, 8.0, 8.0, 8.0);
		StyledProperty<bool> isVisibleProperty = Visual.IsVisibleProperty;
		ReflectionBindingExtension reflectionBindingExtension6 = new ReflectionBindingExtension("IsInfoBarOpen");
		context.ProvideTargetProperty = Visual.IsVisibleProperty;
		ReflectionBinding binding10 = reflectionBindingExtension6.ProvideValue(context);
		context.ProvideTargetProperty = null;
		border9.Bind(isVisibleProperty, binding10);
		TextBlock textBlock7;
		TextBlock textBlock6 = (textBlock7 = new TextBlock());
		((ISupportInitialize)textBlock6).BeginInit();
		border9.Child = textBlock6;
		TextBlock textBlock8 = (textBlock4 = textBlock7);
		context.PushParent(textBlock4);
		TextBlock textBlock9 = textBlock4;
		StyledProperty<string?> textProperty3 = TextBlock.TextProperty;
		ReflectionBindingExtension reflectionBindingExtension7 = new ReflectionBindingExtension("InfoBarTitle");
		context.ProvideTargetProperty = TextBlock.TextProperty;
		ReflectionBinding binding11 = reflectionBindingExtension7.ProvideValue(context);
		context.ProvideTargetProperty = null;
		textBlock9.Bind(textProperty3, binding11);
		context.PopParent();
		((ISupportInitialize)textBlock8).EndInit();
		context.PopParent();
		((ISupportInitialize)border8).EndInit();
		Controls children7 = grid5.Children;
		Border border11;
		Border border10 = (border11 = new Border());
		((ISupportInitialize)border10).BeginInit();
		children7.Add(border10);
		Border border12 = (border4 = border11);
		context.PushParent(border4);
		Border border13 = border4;
		Grid.SetRow(border13, 2);
		border13.Margin = new Thickness(0.0, 12.0, 0.0, 0.0);
		StyledProperty<IBrush?> backgroundProperty4 = Border.BackgroundProperty;
		DynamicResourceExtension dynamicResourceExtension6 = new DynamicResourceExtension("PaneActiveBackgroundBrush");
		context.ProvideTargetProperty = Border.BackgroundProperty;
		BindingBase binding12 = dynamicResourceExtension6.ProvideValue(context);
		context.ProvideTargetProperty = null;
		border13.Bind(backgroundProperty4, binding12);
		StyledProperty<IBrush?> borderBrushProperty3 = Border.BorderBrushProperty;
		DynamicResourceExtension dynamicResourceExtension7 = new DynamicResourceExtension("TopToolbarBackground");
		context.ProvideTargetProperty = Border.BorderBrushProperty;
		BindingBase binding13 = dynamicResourceExtension7.ProvideValue(context);
		context.ProvideTargetProperty = null;
		border13.Bind(borderBrushProperty3, binding13);
		border13.BorderThickness = new Thickness(1.0, 1.0, 1.0, 1.0);
		border13.CornerRadius = new CornerRadius(8.0, 8.0, 8.0, 8.0);
		border13.ClipToBounds = true;
		Grid grid11;
		Grid grid10 = (grid11 = new Grid());
		((ISupportInitialize)grid10).BeginInit();
		border13.Child = grid10;
		Grid grid12 = (grid4 = grid11);
		context.PushParent(grid4);
		Grid grid13 = grid4;
		RowDefinitions rowDefinitions2 = new RowDefinitions();
		rowDefinitions2.Capacity = 2;
		rowDefinitions2.Add(new RowDefinition(new GridLength(0.0, GridUnitType.Auto)));
		rowDefinitions2.Add(new RowDefinition(new GridLength(1.0, GridUnitType.Star)));
		grid13.RowDefinitions = rowDefinitions2;
		Controls children8 = grid13.Children;
		Grid grid15;
		Grid grid14 = (grid15 = new Grid());
		((ISupportInitialize)grid14).BeginInit();
		children8.Add(grid14);
		Grid grid16 = (grid4 = grid15);
		context.PushParent(grid4);
		Grid grid17 = grid4;
		Grid.SetRow(grid17, 0);
		grid17.Height = 44.0;
		ColumnDefinitions columnDefinitions2 = new ColumnDefinitions();
		columnDefinitions2.Capacity = 3;
		columnDefinitions2.Add(new ColumnDefinition(new GridLength(92.0, GridUnitType.Pixel)));
		columnDefinitions2.Add(new ColumnDefinition(new GridLength(1.0, GridUnitType.Star)));
		columnDefinitions2.Add(new ColumnDefinition(new GridLength(0.0, GridUnitType.Auto)));
		grid17.ColumnDefinitions = columnDefinitions2;
		grid17.Margin = new Thickness(16.0, 0.0, 12.0, 0.0);
		Controls children9 = grid17.Children;
		TextBlock textBlock11;
		TextBlock textBlock10 = (textBlock11 = new TextBlock());
		((ISupportInitialize)textBlock10).BeginInit();
		children9.Add(textBlock10);
		TextBlock textBlock12 = (textBlock4 = textBlock11);
		context.PushParent(textBlock4);
		TextBlock textBlock13 = textBlock4;
		Grid.SetColumn(textBlock13, 0);
		textBlock13.VerticalAlignment = VerticalAlignment.Center;
		textBlock13.FontWeight = FontWeight.DemiBold;
		StyledProperty<string?> textProperty4 = TextBlock.TextProperty;
		ReflectionBindingExtension reflectionBindingExtension8 = new ReflectionBindingExtension("[Algorithm]")
		{
			Source = LocalizationBindingSource.Instance
		};
		context.ProvideTargetProperty = TextBlock.TextProperty;
		ReflectionBinding binding14 = reflectionBindingExtension8.ProvideValue(context);
		context.ProvideTargetProperty = null;
		textBlock13.Bind(textProperty4, binding14);
		context.PopParent();
		((ISupportInitialize)textBlock12).EndInit();
		Controls children10 = grid17.Children;
		TextBlock textBlock15;
		TextBlock textBlock14 = (textBlock15 = new TextBlock());
		((ISupportInitialize)textBlock14).BeginInit();
		children10.Add(textBlock14);
		TextBlock textBlock16 = (textBlock4 = textBlock15);
		context.PushParent(textBlock4);
		TextBlock textBlock17 = textBlock4;
		Grid.SetColumn(textBlock17, 1);
		textBlock17.VerticalAlignment = VerticalAlignment.Center;
		textBlock17.FontWeight = FontWeight.DemiBold;
		StyledProperty<string?> textProperty5 = TextBlock.TextProperty;
		ReflectionBindingExtension reflectionBindingExtension9 = new ReflectionBindingExtension("[HashValue]")
		{
			Source = LocalizationBindingSource.Instance
		};
		context.ProvideTargetProperty = TextBlock.TextProperty;
		ReflectionBinding binding15 = reflectionBindingExtension9.ProvideValue(context);
		context.ProvideTargetProperty = null;
		textBlock17.Bind(textProperty5, binding15);
		context.PopParent();
		((ISupportInitialize)textBlock16).EndInit();
		context.PopParent();
		((ISupportInitialize)grid16).EndInit();
		Controls children11 = grid13.Children;
		ScrollViewer scrollViewer2;
		ScrollViewer scrollViewer = (scrollViewer2 = new ScrollViewer());
		((ISupportInitialize)scrollViewer).BeginInit();
		children11.Add(scrollViewer);
		ScrollViewer scrollViewer4;
		ScrollViewer scrollViewer3 = (scrollViewer4 = scrollViewer2);
		context.PushParent(scrollViewer4);
		Grid.SetRow(scrollViewer4, 1);
		scrollViewer4.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
		ItemsControl itemsControl2;
		ItemsControl itemsControl = (itemsControl2 = new ItemsControl());
		((ISupportInitialize)itemsControl).BeginInit();
		scrollViewer4.Content = itemsControl;
		ItemsControl itemsControl4;
		ItemsControl itemsControl3 = (itemsControl4 = itemsControl2);
		context.PushParent(itemsControl4);
		StyledProperty<IEnumerable?> itemsSourceProperty = ItemsControl.ItemsSourceProperty;
		ReflectionBindingExtension reflectionBindingExtension10 = new ReflectionBindingExtension("Hashes");
		context.ProvideTargetProperty = ItemsControl.ItemsSourceProperty;
		ReflectionBinding binding16 = reflectionBindingExtension10.ProvideValue(context);
		context.ProvideTargetProperty = null;
		itemsControl4.Bind(itemsSourceProperty, binding16);
		DataTemplate dataTemplate;
		DataTemplate itemTemplate = (dataTemplate = new DataTemplate());
		context.PushParent(dataTemplate);
		dataTemplate.Content = XamlIlRuntimeHelpers.DeferredTransformationFactoryV3<Control>((nint)(delegate*<IServiceProvider, object>)(&XamlClosure_9.Build_1), context);
		context.PopParent();
		itemsControl4.ItemTemplate = itemTemplate;
		context.PopParent();
		((ISupportInitialize)itemsControl3).EndInit();
		context.PopParent();
		((ISupportInitialize)scrollViewer3).EndInit();
		context.PopParent();
		((ISupportInitialize)grid12).EndInit();
		context.PopParent();
		((ISupportInitialize)border12).EndInit();
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
	private static void _0021XamlIlPopulateTrampoline(HashesWindow P_0)
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
