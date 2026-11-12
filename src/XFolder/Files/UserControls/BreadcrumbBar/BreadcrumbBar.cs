using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Data.Converters;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Markup.Xaml;
using Avalonia.Markup.Xaml.MarkupExtensions;
using Avalonia.Markup.Xaml.MarkupExtensions.CompiledBindings;
using Avalonia.Markup.Xaml.Templates;
using Avalonia.Markup.Xaml.XamlIl.Runtime;
using Avalonia.Media;
using Avalonia.Media.Immutable;
using Avalonia.Styling;
using Avalonia.Threading;
using Avalonia.VisualTree;
using CompiledAvaloniaXaml;
using Files.Converters;
using Files.Data.Items;

namespace Files.UserControls.BreadcrumbBar;

public class BreadcrumbBar : UserControl
{
	[CompilerGenerated]
	private class XamlClosure_3
	{
		public static object Build_1(IServiceProvider P_0)
		{
			XamlIlContext.Context<BreadcrumbBar> context = CreateContext(P_0);
			return new FileIconConverter();
		}

		public static XamlIlContext.Context<BreadcrumbBar> CreateContext(IServiceProvider P_0)
		{
			XamlIlContext.Context<BreadcrumbBar> context = new XamlIlContext.Context<BreadcrumbBar>(P_0, new object[1] { _0021AvaloniaResources.NamespaceInfo_003A_002FUserControls_002FBreadcrumbBar_002FBreadcrumbBar_002Eaxaml.Singleton }, "avares://XFolder/UserControls/BreadcrumbBar/BreadcrumbBar.axaml");
			if (P_0 != null)
			{
				object service = P_0.GetService(typeof(IRootObjectProvider));
				if (service != null)
				{
					service = ((IRootObjectProvider)service).RootObject;
					context.RootObject = (BreadcrumbBar)service;
				}
			}
			return context;
		}

		public static object Build_2(IServiceProvider P_0)
		{
			XamlIlContext.Context<BreadcrumbBar> context = CreateContext(P_0);
			return new FontSizeToIconSizeConverter();
		}

		public static object Build_3(IServiceProvider P_0)
		{
			XamlIlContext.Context<BreadcrumbBar> context = CreateContext(P_0);
			context.IntermediateRoot = new StackPanel();
			object obj = context.IntermediateRoot;
			((ISupportInitialize)obj).BeginInit();
			((AvaloniaObject)obj).SetValue(StackPanel.OrientationProperty, Orientation.Horizontal, BindingPriority.Template);
			((AvaloniaObject)obj).SetValue(StackPanel.SpacingProperty, 0.0, BindingPriority.Template);
			((ISupportInitialize)obj).EndInit();
			return obj;
		}

		public static object Build_4(IServiceProvider P_0)
		{
			XamlIlContext.Context<BreadcrumbBar> context = CreateContext(P_0);
			context.IntermediateRoot = new StackPanel();
			object obj = context.IntermediateRoot;
			((ISupportInitialize)obj).BeginInit();
			StackPanel stackPanel = (StackPanel)obj;
			context.PushParent(stackPanel);
			StackPanel stackPanel2 = stackPanel;
			stackPanel2.Orientation = Orientation.Horizontal;
			Controls children = stackPanel2.Children;
			Button button2;
			Button button = (button2 = new Button());
			((ISupportInitialize)button).BeginInit();
			children.Add(button);
			Button button4;
			Button button3 = (button4 = button2);
			context.PushParent(button4);
			Button button5 = button4;
			button5.Classes.Add("BreadcrumbItem");
			CompiledBindingExtension compiledBindingExtension = new CompiledBindingExtension();
			context.ProvideTargetProperty = Control.TagProperty;
			CompiledBinding compiledBinding = compiledBindingExtension.ProvideValue(context);
			context.ProvideTargetProperty = null;
			XamlDynamicSetters._003C_003EXamlDynamicSetter_3(button5, compiledBinding);
			button5.AddHandler((RoutedEvent)Button.ClickEvent, (Delegate)new EventHandler<RoutedEventArgs>(context.RootObject.OnBreadcrumbItemClick), RoutingStrategies.Direct | RoutingStrategies.Bubble, false);
			button5.AddHandler(InputElement.PointerPressedEvent, context.RootObject.OnBreadcrumbItemPointerPressed);
			CompiledBindingExtension compiledBindingExtension2 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EData_002EItems_002EPathBoxItem_002CXFolder_002EPath_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build());
			context.ProvideTargetProperty = ToolTip.TipProperty;
			CompiledBinding compiledBinding2 = compiledBindingExtension2.ProvideValue(context);
			context.ProvideTargetProperty = null;
		XamlDynamicSetters._003C_003EXamlDynamicSetter_4(button5, compiledBinding2);
			StackPanel stackPanel4;
			StackPanel stackPanel3 = (stackPanel4 = new StackPanel());
			((ISupportInitialize)stackPanel3).BeginInit();
			button5.Content = stackPanel3;
			StackPanel stackPanel5 = (stackPanel = stackPanel4);
			context.PushParent(stackPanel);
			StackPanel stackPanel6 = stackPanel;
			stackPanel6.Orientation = Orientation.Horizontal;
			stackPanel6.Spacing = 6.0;
			Controls children2 = stackPanel6.Children;
			Image image2;
			Image image = (image2 = new Image());
			((ISupportInitialize)image).BeginInit();
			children2.Add(image);
			Image image4;
			Image image3 = (image4 = image2);
			context.PushParent(image4);
			StyledProperty<double> widthProperty = Layoutable.WidthProperty;
			CompiledBindingExtension compiledBindingExtension4;
			CompiledBindingExtension compiledBindingExtension3 = (compiledBindingExtension4 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Ancestor(typeof(Button), 0).Property(TemplatedControl.FontSizeProperty, PropertyInfoAccessorFactory.CreateAvaloniaPropertyAccessor).Build()));
			context.PushParent(compiledBindingExtension4);
			CompiledBindingExtension compiledBindingExtension5 = compiledBindingExtension4;
			StaticResourceExtension staticResourceExtension = new StaticResourceExtension("FontSizeToIconSizeConverter");
			context.ProvideTargetProperty = XamlIlHelpers.Avalonia_002EData_002ECompiledBinding_002CAvalonia_002EBase_002EConverter_0021Property();
			object? obj2 = staticResourceExtension.ProvideValue(context);
			context.ProvideTargetProperty = null;
			compiledBindingExtension5.Converter = (IValueConverter)obj2;
			compiledBindingExtension5.ConverterParameter = "1.23";
			context.PopParent();
			context.ProvideTargetProperty = Layoutable.WidthProperty;
			CompiledBinding binding = compiledBindingExtension3.ProvideValue(context);
			context.ProvideTargetProperty = null;
			image4.Bind(widthProperty, binding);
			StyledProperty<double> heightProperty = Layoutable.HeightProperty;
			CompiledBindingExtension compiledBindingExtension6 = (compiledBindingExtension4 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Ancestor(typeof(Button), 0).Property(TemplatedControl.FontSizeProperty, PropertyInfoAccessorFactory.CreateAvaloniaPropertyAccessor).Build()));
			context.PushParent(compiledBindingExtension4);
			CompiledBindingExtension compiledBindingExtension7 = compiledBindingExtension4;
			StaticResourceExtension staticResourceExtension2 = new StaticResourceExtension("FontSizeToIconSizeConverter");
			context.ProvideTargetProperty = XamlIlHelpers.Avalonia_002EData_002ECompiledBinding_002CAvalonia_002EBase_002EConverter_0021Property();
			object? obj3 = staticResourceExtension2.ProvideValue(context);
			context.ProvideTargetProperty = null;
			compiledBindingExtension7.Converter = (IValueConverter)obj3;
			compiledBindingExtension7.ConverterParameter = "1.23";
			context.PopParent();
			context.ProvideTargetProperty = Layoutable.HeightProperty;
			CompiledBinding binding2 = compiledBindingExtension6.ProvideValue(context);
			context.ProvideTargetProperty = null;
			image4.Bind(heightProperty, binding2);
			image4.VerticalAlignment = VerticalAlignment.Center;
			StyledProperty<bool> isVisibleProperty = Visual.IsVisibleProperty;
			CompiledBindingExtension compiledBindingExtension8 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EData_002EItems_002EPathBoxItem_002CXFolder_002EIsRootPath_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build());
			context.ProvideTargetProperty = Visual.IsVisibleProperty;
			CompiledBinding binding3 = compiledBindingExtension8.ProvideValue(context);
			context.ProvideTargetProperty = null;
			image4.Bind(isVisibleProperty, binding3);
			StyledProperty<IImage?> sourceProperty = Image.SourceProperty;
			CompiledBindingExtension compiledBindingExtension9 = (compiledBindingExtension4 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EData_002EItems_002EPathBoxItem_002CXFolder_002EPath_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build()));
			context.PushParent(compiledBindingExtension4);
			CompiledBindingExtension compiledBindingExtension10 = compiledBindingExtension4;
			StaticResourceExtension staticResourceExtension3 = new StaticResourceExtension("FileIconConverter");
			context.ProvideTargetProperty = XamlIlHelpers.Avalonia_002EData_002ECompiledBinding_002CAvalonia_002EBase_002EConverter_0021Property();
			object? obj4 = staticResourceExtension3.ProvideValue(context);
			context.ProvideTargetProperty = null;
			compiledBindingExtension10.Converter = (IValueConverter)obj4;
			context.PopParent();
			context.ProvideTargetProperty = Image.SourceProperty;
			CompiledBinding binding4 = compiledBindingExtension9.ProvideValue(context);
			context.ProvideTargetProperty = null;
			image4.Bind(sourceProperty, binding4);
			context.PopParent();
			((ISupportInitialize)image3).EndInit();
			Controls children3 = stackPanel6.Children;
			TextBlock textBlock2;
			TextBlock textBlock = (textBlock2 = new TextBlock());
			((ISupportInitialize)textBlock).BeginInit();
			children3.Add(textBlock);
			TextBlock textBlock4;
			TextBlock textBlock3 = (textBlock4 = textBlock2);
			context.PushParent(textBlock4);
			textBlock4.Name = "ItemText";
			object element = textBlock4;
			context.AvaloniaNameScope.Register("ItemText", element);
			StyledProperty<string?> textProperty = TextBlock.TextProperty;
			CompiledBindingExtension compiledBindingExtension11 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EData_002EItems_002EPathBoxItem_002CXFolder_002ETitle_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build());
			context.ProvideTargetProperty = TextBlock.TextProperty;
			CompiledBinding binding5 = compiledBindingExtension11.ProvideValue(context);
			context.ProvideTargetProperty = null;
			textBlock4.Bind(textProperty, binding5);
			StyledProperty<double> fontSizeProperty = TextBlock.FontSizeProperty;
			DynamicResourceExtension dynamicResourceExtension = new DynamicResourceExtension("GlobalFontSize");
			context.ProvideTargetProperty = TextBlock.FontSizeProperty;
			BindingBase binding6 = dynamicResourceExtension.ProvideValue(context);
			context.ProvideTargetProperty = null;
			textBlock4.Bind(fontSizeProperty, binding6);
			textBlock4.VerticalAlignment = VerticalAlignment.Center;
			textBlock4.TextTrimming = TextTrimming.CharacterEllipsis;
			StyledProperty<bool> isVisibleProperty2 = Visual.IsVisibleProperty;
			CompiledBindingExtension compiledBindingExtension12 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Not().Property(XamlIlHelpers.Files_002EData_002EItems_002EPathBoxItem_002CXFolder_002EIsRootPath_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build());
			context.ProvideTargetProperty = Visual.IsVisibleProperty;
			CompiledBinding binding7 = compiledBindingExtension12.ProvideValue(context);
			context.ProvideTargetProperty = null;
			textBlock4.Bind(isVisibleProperty2, binding7);
			context.PopParent();
			((ISupportInitialize)textBlock3).EndInit();
			context.PopParent();
			((ISupportInitialize)stackPanel5).EndInit();
			context.PopParent();
			((ISupportInitialize)button3).EndInit();
			Controls children4 = stackPanel2.Children;
			Button button7;
			Button button6 = (button7 = new Button());
			((ISupportInitialize)button6).BeginInit();
			children4.Add(button6);
			Button button8 = (button4 = button7);
			context.PushParent(button4);
			Button button9 = button4;
			button9.Classes.Add("ChevronButton");
			CompiledBindingExtension compiledBindingExtension13 = new CompiledBindingExtension();
			context.ProvideTargetProperty = Control.TagProperty;
			CompiledBinding compiledBinding3 = compiledBindingExtension13.ProvideValue(context);
			context.ProvideTargetProperty = null;
			XamlDynamicSetters._003C_003EXamlDynamicSetter_3(button9, compiledBinding3);
			button9.AddHandler((RoutedEvent)Button.ClickEvent, (Delegate)new EventHandler<RoutedEventArgs>(context.RootObject.OnChevronClick), RoutingStrategies.Direct | RoutingStrategies.Bubble, false);
			button9.AddHandler(InputElement.PointerPressedEvent, context.RootObject.OnChevronPointerPressed);
			CompiledBindingExtension compiledBindingExtension14 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EData_002EItems_002EPathBoxItem_002CXFolder_002EChevronToolTip_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build());
			context.ProvideTargetProperty = ToolTip.TipProperty;
			CompiledBinding compiledBinding4 = compiledBindingExtension14.ProvideValue(context);
			context.ProvideTargetProperty = null;
		XamlDynamicSetters._003C_003EXamlDynamicSetter_4(button9, compiledBinding4);
			PathIcon pathIcon2;
			PathIcon pathIcon = (pathIcon2 = new PathIcon());
			((ISupportInitialize)pathIcon).BeginInit();
			button9.Content = pathIcon;
			PathIcon pathIcon4;
			PathIcon pathIcon3 = (pathIcon4 = pathIcon2);
			context.PushParent(pathIcon4);
			pathIcon4.Data = Geometry.Parse("M10 6L8.59 7.41 13.17 12l-4.58 4.59L10 18l6-6z");
			StyledProperty<double> widthProperty2 = Layoutable.WidthProperty;
			CompiledBindingExtension compiledBindingExtension15 = (compiledBindingExtension4 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Ancestor(typeof(Button), 0).Property(TemplatedControl.FontSizeProperty, PropertyInfoAccessorFactory.CreateAvaloniaPropertyAccessor).Build()));
			context.PushParent(compiledBindingExtension4);
			CompiledBindingExtension compiledBindingExtension16 = compiledBindingExtension4;
			StaticResourceExtension staticResourceExtension4 = new StaticResourceExtension("FontSizeToIconSizeConverter");
			context.ProvideTargetProperty = XamlIlHelpers.Avalonia_002EData_002ECompiledBinding_002CAvalonia_002EBase_002EConverter_0021Property();
			object? obj5 = staticResourceExtension4.ProvideValue(context);
			context.ProvideTargetProperty = null;
			compiledBindingExtension16.Converter = (IValueConverter)obj5;
			compiledBindingExtension16.ConverterParameter = "0.62";
			context.PopParent();
			context.ProvideTargetProperty = Layoutable.WidthProperty;
			CompiledBinding binding8 = compiledBindingExtension15.ProvideValue(context);
			context.ProvideTargetProperty = null;
			pathIcon4.Bind(widthProperty2, binding8);
			StyledProperty<double> heightProperty2 = Layoutable.HeightProperty;
			CompiledBindingExtension compiledBindingExtension17 = (compiledBindingExtension4 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Ancestor(typeof(Button), 0).Property(TemplatedControl.FontSizeProperty, PropertyInfoAccessorFactory.CreateAvaloniaPropertyAccessor).Build()));
			context.PushParent(compiledBindingExtension4);
			CompiledBindingExtension compiledBindingExtension18 = compiledBindingExtension4;
			StaticResourceExtension staticResourceExtension5 = new StaticResourceExtension("FontSizeToIconSizeConverter");
			context.ProvideTargetProperty = XamlIlHelpers.Avalonia_002EData_002ECompiledBinding_002CAvalonia_002EBase_002EConverter_0021Property();
			object? obj6 = staticResourceExtension5.ProvideValue(context);
			context.ProvideTargetProperty = null;
			compiledBindingExtension18.Converter = (IValueConverter)obj6;
			compiledBindingExtension18.ConverterParameter = "0.62";
			context.PopParent();
			context.ProvideTargetProperty = Layoutable.HeightProperty;
			CompiledBinding binding9 = compiledBindingExtension17.ProvideValue(context);
			context.ProvideTargetProperty = null;
			pathIcon4.Bind(heightProperty2, binding9);
			context.PopParent();
			((ISupportInitialize)pathIcon3).EndInit();
			context.PopParent();
			((ISupportInitialize)button8).EndInit();
			context.PopParent();
			((ISupportInitialize)obj).EndInit();
			return obj;
		}
	}

	private const double MinItemWidth = 30.0;

	private const double MinMeaningfulTextCompressionWidth = 8.0;

	private const double BlankAreaMinWidth = 0.0;

	private const double PreferredBlankAreaWidth = 60.0;

	private const double ChevronWidth = 18.0;

	private const double ItemButtonPadding = 8.0;

	private const double RootIconWidth = 16.0;

	private const double TextWidthSafetyPadding = 2.0;

	public static readonly StyledProperty<ObservableCollection<PathBoxItem>> PathComponentsProperty = AvaloniaProperty.Register<BreadcrumbBar, ObservableCollection<PathBoxItem>>("PathComponents", new ObservableCollection<PathBoxItem>());

	[GeneratedCode("Avalonia.Generators.NameGenerator.InitializeComponentCodeGenerator", "12.0.1.0")]
	internal ItemsControl BreadcrumbItemsControl;

	[GeneratedCode("Avalonia.Generators.NameGenerator.InitializeComponentCodeGenerator", "12.0.1.0")]
	internal Border BlankClickArea;

	[CompilerGenerated]
	private static Action<object> _0021XamlIlPopulateOverride;

	public ObservableCollection<PathBoxItem> PathComponents
	{
		get
		{
			return GetValue(PathComponentsProperty);
		}
		set
		{
			SetValue(PathComponentsProperty, value);
		}
	}

	public event EventHandler<PathBoxItem>? ItemClicked;

	public event EventHandler<BreadcrumbChevronClickedEventArgs>? ChevronClicked;

	public event EventHandler? BlankAreaClicked;

	public event EventHandler<BreadcrumbContextRequestedEventArgs>? ItemContextRequested;

	public BreadcrumbBar()
	{
		InitializeComponent();
		PathComponentsProperty.Changed.AddClassHandler((BreadcrumbBar s, AvaloniaPropertyChangedEventArgs e) =>
		{
			s.OnPathComponentsChanged();
		});
	}

	protected override void OnSizeChanged(SizeChangedEventArgs e)
	{
		base.OnSizeChanged(e);
		UpdateItemWidths();
	}

	private void OnPathComponentsChanged()
	{
		Avalonia.Threading.Dispatcher.UIThread.Post(() =>
		{
			UpdateItemWidths();
		}, DispatcherPriority.Loaded);
	}

	private void UpdateItemWidths()
	{
		if (BreadcrumbItemsControl == null || PathComponents == null || PathComponents.Count == 0)
		{
			return;
		}
		double width = Bounds.Width;
		if (width <= 0.0)
		{
			return;
		}
		List<TextBlock> list = (from tb in BreadcrumbItemsControl.GetVisualDescendants().OfType<TextBlock>()
			where tb.Name == "ItemText" && tb.DataContext is PathBoxItem pathBoxItem && !pathBoxItem.IsRootPath
			select tb).ToList();
		if (list.Count == 0)
		{
			return;
		}
		int count;
		int num = (count = PathComponents.Count);
		int num2 = Enumerable.Count(PathComponents, (PathBoxItem item) => item.IsRootPath);
		double num3 = (double)num * 8.0 + (double)count * 18.0 + (double)num2 * 16.0;
		List<(TextBlock TextBlock, double DesiredWidth)> list2 = list.Select((TextBlock textBlock) => (TextBlock: textBlock, DesiredWidth: MeasureTextBlockDesiredWidth(textBlock))).ToList();
		double num4 = list2.Sum(((TextBlock TextBlock, double DesiredWidth) textBlock) => textBlock.DesiredWidth);
		double num5 = width - num3 - num4;
		double num6 = ((num5 > 0.0) ? Math.Min(60.0, num5) : 0.0);
		BlankClickArea.Width = num6;
		double availableWidth = Math.Max(0.0, width - num6 - num3);
		Dictionary<TextBlock, double> dictionary = CalculateTextBlockWidths(list2, availableWidth);
		foreach (TextBlock item in list)
		{
			item.MaxWidth = dictionary[item];
		}
	}

	private static double MeasureTextBlockDesiredWidth(TextBlock textBlock)
	{
		double maxWidth = textBlock.MaxWidth;
		try
		{
			textBlock.MaxWidth = double.PositiveInfinity;
			textBlock.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
			double width = textBlock.DesiredSize.Width;
			if (double.IsNaN(width) || double.IsInfinity(width) || width <= 0.0)
			{
				return 30.0;
			}
			return Math.Ceiling(width) + 2.0;
		}
		finally
		{
			textBlock.MaxWidth = maxWidth;
		}
	}

	private static Dictionary<TextBlock, double> CalculateTextBlockWidths(List<(TextBlock TextBlock, double DesiredWidth)> textBlocks, double availableWidth)
	{
		Dictionary<TextBlock, double> dictionary = new Dictionary<TextBlock, double>();
		if (availableWidth <= 0.0)
		{
			foreach (var textBlock in textBlocks)
			{
				dictionary[textBlock.TextBlock] = 30.0;
			}
			return dictionary;
		}
		if (textBlocks.Sum(((TextBlock TextBlock, double DesiredWidth) textBlock) => textBlock.DesiredWidth) <= availableWidth)
		{
			foreach (var textBlock2 in textBlocks)
			{
				dictionary[textBlock2.TextBlock] = textBlock2.DesiredWidth;
			}
			return dictionary;
		}
		List<(TextBlock, double)> list = textBlocks.OrderBy(((TextBlock TextBlock, double DesiredWidth) textBlock) => textBlock.DesiredWidth).ToList();
		double num = availableWidth;
		while (list.Count > 0)
		{
			double num2 = num / (double)list.Count;
			(TextBlock, double) tuple = list[0];
			double num3 = tuple.Item2 - num2;
			bool flag = num >= tuple.Item2 && num - tuple.Item2 >= (double)(list.Count - 1) * 30.0;
			if (tuple.Item2 > num2 && (num3 >= 8.0 || !flag))
			{
				break;
			}
			dictionary[tuple.Item1] = tuple.Item2;
			num -= tuple.Item2;
			list.RemoveAt(0);
		}
		if (list.Count == 0)
		{
			return dictionary;
		}
		double val = Math.Max(30.0, num / (double)list.Count);
		foreach (var item in list)
		{
			dictionary[item.Item1] = Math.Min(item.Item2, val);
		}
		return dictionary;
	}

	private void OnBreadcrumbItemClick(object? sender, RoutedEventArgs e)
	{
		if (sender is Button { Tag: PathBoxItem tag })
		{
			ItemClicked?.Invoke(this, tag);
		}
	}

	private void OnChevronClick(object? sender, RoutedEventArgs e)
	{
		if (sender is Button { Tag: PathBoxItem tag } button)
		{
			ChevronClicked?.Invoke(this, new BreadcrumbChevronClickedEventArgs(tag, button));
		}
	}

	private void OnBreadcrumbItemPointerPressed(object? sender, PointerPressedEventArgs e)
	{
		HandleContextRequestPointerPressed(sender, e);
	}

	private void OnChevronPointerPressed(object? sender, PointerPressedEventArgs e)
	{
		HandleContextRequestPointerPressed(sender, e);
	}

	private void OnBlankAreaPointerPressed(object? sender, PointerPressedEventArgs e)
	{
		if (e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
		{
			BlankAreaClicked?.Invoke(this, EventArgs.Empty);
			e.Handled = true;
		}
	}

	private void HandleContextRequestPointerPressed(object? sender, PointerPressedEventArgs e)
	{
		if (e.GetCurrentPoint(this).Properties.IsRightButtonPressed)
		{
			RaiseItemContextRequested(sender);
			e.Handled = true;
		}
	}

	private void RaiseItemContextRequested(object? sender)
	{
		if (sender is Control { Tag: PathBoxItem tag } control)
		{
			ItemContextRequested?.Invoke(this, new BreadcrumbContextRequestedEventArgs(tag, control));
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
		BreadcrumbItemsControl = ((nameScope != null) ? nameScope.Find<ItemsControl>("BreadcrumbItemsControl") : null);
		BlankClickArea = ((nameScope != null) ? nameScope.Find<Border>("BlankClickArea") : null);
	}

	[CompilerGenerated]
	private unsafe static void _0021XamlIlPopulate(IServiceProvider P_0, BreadcrumbBar P_1)
	{
		XamlIlContext.Context<BreadcrumbBar> context = new XamlIlContext.Context<BreadcrumbBar>(P_0, new object[1] { _0021AvaloniaResources.NamespaceInfo_003A_002FUserControls_002FBreadcrumbBar_002FBreadcrumbBar_002Eaxaml.Singleton }, "avares://XFolder/UserControls/BreadcrumbBar/BreadcrumbBar.axaml")
		{
			RootObject = P_1,
			IntermediateRoot = P_1
		};
		((ISupportInitialize)P_1).BeginInit();
		context.PushParent(P_1);
		if (P_1.Resources is ResourceDictionary resourceDictionary)
		{
			resourceDictionary.EnsureCapacity(resourceDictionary.Count + 2);
		}
		((ResourceDictionary)P_1.Resources).AddDeferred((object)"FileIconConverter", XamlIlRuntimeHelpers.DeferredTransformationFactoryV3<object>((nint)(delegate*<IServiceProvider, object>)(&XamlClosure_3.Build_1), context));
		((ResourceDictionary)P_1.Resources).AddDeferred((object)"FontSizeToIconSizeConverter", XamlIlRuntimeHelpers.DeferredTransformationFactoryV3<object>((nint)(delegate*<IServiceProvider, object>)(&XamlClosure_3.Build_2), context));
		Styles styles = P_1.Styles;
		Style style;
		Style item = (style = new Style());
		context.PushParent(style);
		Style style2 = style;
		style2.Selector = ((Selector?)null).OfType(typeof(Button)).Class("BreadcrumbItem");
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
		setter3.Value = new Thickness(4.0, 2.0, 4.0, 2.0);
		style2.Add(setter3);
		Setter setter4 = new Setter();
		setter4.Property = TemplatedControl.CornerRadiusProperty;
		setter4.Value = new CornerRadius(4.0, 4.0, 4.0, 4.0);
		style2.Add(setter4);
		Setter setter5 = new Setter();
		setter5.Property = InputElement.CursorProperty;
		setter5.Value = new Cursor(StandardCursorType.Hand);
		style2.Add(setter5);
		Setter setter6 = new Setter();
		setter6.Property = Layoutable.MinWidthProperty;
		setter6.Value = 0.0;
		style2.Add(setter6);
		Setter setter7 = new Setter();
		setter7.Property = Layoutable.MinHeightProperty;
		setter7.Value = 0.0;
		style2.Add(setter7);
		Setter setter9;
		Setter setter8 = (setter9 = new Setter());
		context.PushParent(setter9);
		Setter setter10 = setter9;
		setter10.Property = TemplatedControl.FontSizeProperty;
		DynamicResourceExtension dynamicResourceExtension = new DynamicResourceExtension("GlobalFontSize");
		context.ProvideTargetProperty = XamlIlHelpers.Avalonia_002EStyling_002ESetter_002CAvalonia_002EBase_002EValue_0021Property();
		BindingBase value = dynamicResourceExtension.ProvideValue(context);
		context.ProvideTargetProperty = null;
		setter10.Value = value;
		context.PopParent();
		style2.Add(setter8);
		context.PopParent();
		styles.Add(item);
		Styles styles2 = P_1.Styles;
		Style item2 = (style = new Style());
		context.PushParent(style);
		Style style3 = style;
		style3.Selector = ((Selector?)null).OfType(typeof(Button)).Class("BreadcrumbItem").Class(":pointerover")
			.Template()
			.OfType(typeof(ContentPresenter));
		Setter setter11 = (setter9 = new Setter());
		context.PushParent(setter9);
		Setter setter12 = setter9;
		setter12.Property = ContentPresenter.BackgroundProperty;
		DynamicResourceExtension dynamicResourceExtension2 = new DynamicResourceExtension("BreadcrumbItemPointerOverBackground");
		context.ProvideTargetProperty = XamlIlHelpers.Avalonia_002EStyling_002ESetter_002CAvalonia_002EBase_002EValue_0021Property();
		BindingBase value2 = dynamicResourceExtension2.ProvideValue(context);
		context.ProvideTargetProperty = null;
		setter12.Value = value2;
		context.PopParent();
		style3.Add(setter11);
		context.PopParent();
		styles2.Add(item2);
		Styles styles3 = P_1.Styles;
		Style item3 = (style = new Style());
		context.PushParent(style);
		Style style4 = style;
		style4.Selector = ((Selector?)null).OfType(typeof(Button)).Class("BreadcrumbItem").Class(":pressed")
			.Template()
			.OfType(typeof(ContentPresenter));
		Setter setter13 = (setter9 = new Setter());
		context.PushParent(setter9);
		Setter setter14 = setter9;
		setter14.Property = ContentPresenter.BackgroundProperty;
		DynamicResourceExtension dynamicResourceExtension3 = new DynamicResourceExtension("BreadcrumbItemPressedBackground");
		context.ProvideTargetProperty = XamlIlHelpers.Avalonia_002EStyling_002ESetter_002CAvalonia_002EBase_002EValue_0021Property();
		BindingBase value3 = dynamicResourceExtension3.ProvideValue(context);
		context.ProvideTargetProperty = null;
		setter14.Value = value3;
		context.PopParent();
		style4.Add(setter13);
		context.PopParent();
		styles3.Add(item3);
		Styles styles4 = P_1.Styles;
		Style item4 = (style = new Style());
		context.PushParent(style);
		Style style5 = style;
		style5.Selector = ((Selector?)null).OfType(typeof(Button)).Class("ChevronButton");
		Setter setter15 = new Setter();
		setter15.Property = TemplatedControl.BackgroundProperty;
		setter15.Value = new ImmutableSolidColorBrush(16777215u);
		style5.Add(setter15);
		Setter setter16 = new Setter();
		setter16.Property = TemplatedControl.BorderThicknessProperty;
		setter16.Value = new Thickness(0.0, 0.0, 0.0, 0.0);
		style5.Add(setter16);
		Setter setter17 = new Setter();
		setter17.Property = TemplatedControl.PaddingProperty;
		setter17.Value = new Thickness(2.0, 2.0, 2.0, 2.0);
		style5.Add(setter17);
		Setter setter18 = new Setter();
		setter18.Property = TemplatedControl.CornerRadiusProperty;
		setter18.Value = new CornerRadius(4.0, 4.0, 4.0, 4.0);
		style5.Add(setter18);
		Setter setter19 = new Setter();
		setter19.Property = InputElement.CursorProperty;
		setter19.Value = new Cursor(StandardCursorType.Hand);
		style5.Add(setter19);
		Setter setter20 = new Setter();
		setter20.Property = Layoutable.MinWidthProperty;
		setter20.Value = 0.0;
		style5.Add(setter20);
		Setter setter21 = new Setter();
		setter21.Property = Layoutable.MinHeightProperty;
		setter21.Value = 0.0;
		style5.Add(setter21);
		Setter setter22 = new Setter();
		setter22.Property = Visual.OpacityProperty;
		setter22.Value = 0.6;
		style5.Add(setter22);
		Setter setter23 = (setter9 = new Setter());
		context.PushParent(setter9);
		Setter setter24 = setter9;
		setter24.Property = TemplatedControl.FontSizeProperty;
		DynamicResourceExtension dynamicResourceExtension4 = new DynamicResourceExtension("GlobalFontSize");
		context.ProvideTargetProperty = XamlIlHelpers.Avalonia_002EStyling_002ESetter_002CAvalonia_002EBase_002EValue_0021Property();
		BindingBase value4 = dynamicResourceExtension4.ProvideValue(context);
		context.ProvideTargetProperty = null;
		setter24.Value = value4;
		context.PopParent();
		style5.Add(setter23);
		context.PopParent();
		styles4.Add(item4);
		Styles styles5 = P_1.Styles;
		Style style6 = new Style();
		style6.Selector = ((Selector?)null).OfType(typeof(Button)).Class("ChevronButton").Class(":pointerover");
		Setter setter25 = new Setter();
		setter25.Property = Visual.OpacityProperty;
		setter25.Value = 1.0;
		style6.Add(setter25);
		styles5.Add(style6);
		Styles styles6 = P_1.Styles;
		Style item5 = (style = new Style());
		context.PushParent(style);
		Style style7 = style;
		style7.Selector = ((Selector?)null).OfType(typeof(Button)).Class("ChevronButton").Class(":pointerover")
			.Template()
			.OfType(typeof(ContentPresenter));
		Setter setter26 = (setter9 = new Setter());
		context.PushParent(setter9);
		Setter setter27 = setter9;
		setter27.Property = ContentPresenter.BackgroundProperty;
		DynamicResourceExtension dynamicResourceExtension5 = new DynamicResourceExtension("BreadcrumbItemPointerOverBackground");
		context.ProvideTargetProperty = XamlIlHelpers.Avalonia_002EStyling_002ESetter_002CAvalonia_002EBase_002EValue_0021Property();
		BindingBase value5 = dynamicResourceExtension5.ProvideValue(context);
		context.ProvideTargetProperty = null;
		setter27.Value = value5;
		context.PopParent();
		style7.Add(setter26);
		context.PopParent();
		styles6.Add(item5);
		Grid grid2;
		Grid grid = (grid2 = new Grid());
		((ISupportInitialize)grid).BeginInit();
		P_1.Content = grid;
		Grid grid4;
		Grid grid3 = (grid4 = grid2);
		context.PushParent(grid4);
		grid4.ColumnDefinitions.Add(new ColumnDefinition
		{
			Width = new GridLength(1.0, GridUnitType.Star)
		});
		grid4.ColumnDefinitions.Add(new ColumnDefinition
		{
			Width = new GridLength(0.0, GridUnitType.Auto)
		});
		Controls children = grid4.Children;
		ItemsControl itemsControl2;
		ItemsControl itemsControl = (itemsControl2 = new ItemsControl());
		((ISupportInitialize)itemsControl).BeginInit();
		children.Add(itemsControl);
		ItemsControl itemsControl4;
		ItemsControl itemsControl3 = (itemsControl4 = itemsControl2);
		context.PushParent(itemsControl4);
		itemsControl4.Name = "BreadcrumbItemsControl";
		object element = itemsControl4;
		context.AvaloniaNameScope.Register("BreadcrumbItemsControl", element);
		Grid.SetColumn(itemsControl4, 0);
		StyledProperty<IEnumerable?> itemsSourceProperty = ItemsControl.ItemsSourceProperty;
		CompiledBindingExtension compiledBindingExtension = new CompiledBindingExtension(new CompiledBindingPathBuilder().VisualAncestor(typeof(BreadcrumbBar), 0).Property(PathComponentsProperty, PropertyInfoAccessorFactory.CreateAvaloniaPropertyAccessor).Build());
		context.ProvideTargetProperty = ItemsControl.ItemsSourceProperty;
		CompiledBinding binding = compiledBindingExtension.ProvideValue(context);
		context.ProvideTargetProperty = null;
		itemsControl4.Bind(itemsSourceProperty, binding);
		itemsControl4.HorizontalAlignment = HorizontalAlignment.Stretch;
		itemsControl4.ClipToBounds = true;
		itemsControl4.ItemsPanel = new ItemsPanelTemplate
		{
			Content = XamlIlRuntimeHelpers.DeferredTransformationFactoryV3<Control>((nint)(delegate*<IServiceProvider, object>)(&XamlClosure_3.Build_3), context)
		};
		DataTemplate dataTemplate;
		DataTemplate itemTemplate = (dataTemplate = new DataTemplate());
		context.PushParent(dataTemplate);
		dataTemplate.DataType = typeof(PathBoxItem);
		dataTemplate.Content = XamlIlRuntimeHelpers.DeferredTransformationFactoryV3<Control>((nint)(delegate*<IServiceProvider, object>)(&XamlClosure_3.Build_4), context);
		context.PopParent();
		itemsControl4.ItemTemplate = itemTemplate;
		context.PopParent();
		((ISupportInitialize)itemsControl3).EndInit();
		Controls children2 = grid4.Children;
		Border border2;
		Border border = (border2 = new Border());
		((ISupportInitialize)border).BeginInit();
		children2.Add(border);
		border2.Name = "BlankClickArea";
		element = border2;
		context.AvaloniaNameScope.Register("BlankClickArea", element);
		Grid.SetColumn(border2, 1);
		border2.Background = new ImmutableSolidColorBrush(16777215u);
		border2.MinWidth = 0.0;
		border2.HorizontalAlignment = HorizontalAlignment.Stretch;
		border2.AddHandler(InputElement.PointerPressedEvent, context.RootObject.OnBlankAreaPointerPressed);
		border2.Cursor = Avalonia.Input.Cursor.Parse("IBeam");
		((ISupportInitialize)border2).EndInit();
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
	private static void _0021XamlIlPopulateTrampoline(BreadcrumbBar P_0)
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
