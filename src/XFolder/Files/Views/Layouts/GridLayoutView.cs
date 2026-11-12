using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using Avalonia;
using Avalonia.Animation;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Shapes;
using Avalonia.Data;
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
using CommunityToolkit.Mvvm.DependencyInjection;
using CompiledAvaloniaXaml;
using Files.Converters;
using Files.Data.Items;
using Files.Helpers.UI;
using Files.ViewModels.Layouts;
using Microsoft.Extensions.Logging;

namespace Files.Views.Layouts;

public class GridLayoutView : BaseLayoutView
{
	[CompilerGenerated]
	private class XamlClosure_7
	{
		public static object Build_1(IServiceProvider P_0)
		{
			XamlIlContext.Context<GridLayoutView> context = CreateContext(P_0);
			return new FileFolderIconConverter();
		}

		public static XamlIlContext.Context<GridLayoutView> CreateContext(IServiceProvider P_0)
		{
			XamlIlContext.Context<GridLayoutView> context = new XamlIlContext.Context<GridLayoutView>(P_0, new object[1] { _0021AvaloniaResources.NamespaceInfo_003A_002FViews_002FLayouts_002FGridLayoutView_002Eaxaml.Singleton }, "avares://XFolder/Views/Layouts/GridLayoutView.axaml");
			if (P_0 != null)
			{
				object service = P_0.GetService(typeof(IRootObjectProvider));
				if (service != null)
				{
					service = ((IRootObjectProvider)service).RootObject;
					context.RootObject = (GridLayoutView)service;
				}
			}
			return context;
		}

		public static object Build_2(IServiceProvider P_0)
		{
			XamlIlContext.Context<GridLayoutView> context = CreateContext(P_0);
			context.IntermediateRoot = new WrapPanel();
			object obj = context.IntermediateRoot;
			((ISupportInitialize)obj).BeginInit();
			((AvaloniaObject)obj).SetValue(WrapPanel.OrientationProperty, Orientation.Horizontal, BindingPriority.Template);
			((ISupportInitialize)obj).EndInit();
			return obj;
		}

		public static object Build_3(IServiceProvider P_0)
		{
			XamlIlContext.Context<GridLayoutView> context = CreateContext(P_0);
			context.IntermediateRoot = new StackPanel();
			object obj = context.IntermediateRoot;
			((ISupportInitialize)obj).BeginInit();
			StackPanel stackPanel = (StackPanel)obj;
			context.PushParent(stackPanel);
			stackPanel.Orientation = Orientation.Vertical;
			StyledProperty<Thickness> marginProperty = Layoutable.MarginProperty;
			CompiledBindingExtension compiledBindingExtension = new CompiledBindingExtension(new CompiledBindingPathBuilder().Ancestor(typeof(GridLayoutView), 0).Property(GridItemMarginProperty, PropertyInfoAccessorFactory.CreateAvaloniaPropertyAccessor).Build());
			context.ProvideTargetProperty = Layoutable.MarginProperty;
			CompiledBinding binding = compiledBindingExtension.ProvideValue(context);
			context.ProvideTargetProperty = null;
			stackPanel.Bind(marginProperty, binding);
			StyledProperty<double> widthProperty = Layoutable.WidthProperty;
			CompiledBindingExtension compiledBindingExtension2 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Ancestor(typeof(GridLayoutView), 0).Property(GridItemWidthProperty, PropertyInfoAccessorFactory.CreateAvaloniaPropertyAccessor).Build());
			context.ProvideTargetProperty = Layoutable.WidthProperty;
			CompiledBinding binding2 = compiledBindingExtension2.ProvideValue(context);
			context.ProvideTargetProperty = null;
			stackPanel.Bind(widthProperty, binding2);
			stackPanel.HorizontalAlignment = HorizontalAlignment.Center;
			StyledProperty<double> opacityProperty = Visual.OpacityProperty;
			CompiledBindingExtension compiledBindingExtension3 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EData_002EItems_002EListedItem_002CXFolder_002EOpacity_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build());
			context.ProvideTargetProperty = Visual.OpacityProperty;
			CompiledBinding binding3 = compiledBindingExtension3.ProvideValue(context);
			context.ProvideTargetProperty = null;
			stackPanel.Bind(opacityProperty, binding3);
			stackPanel.Background = new ImmutableSolidColorBrush(16777215u);
			CompiledBindingExtension compiledBindingExtension4 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EData_002EItems_002EListedItem_002CXFolder_002EItemTooltipText_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build());
			context.ProvideTargetProperty = ToolTip.TipProperty;
			CompiledBinding compiledBinding = compiledBindingExtension4.ProvideValue(context);
			context.ProvideTargetProperty = null;
			XamlDynamicSetters._003C_003EXamlDynamicSetter_4(stackPanel, compiledBinding);
			Controls children = stackPanel.Children;
			Image image2;
			Image image = (image2 = new Image());
			((ISupportInitialize)image).BeginInit();
			children.Add(image);
			Image image4;
			Image image3 = (image4 = image2);
			context.PushParent(image4);
			StyledProperty<IImage?> sourceProperty = Image.SourceProperty;
			CompiledBindingExtension compiledBindingExtension5 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EData_002EItems_002EListedItem_002CXFolder_002EThumbnail_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build());
			context.ProvideTargetProperty = Image.SourceProperty;
			CompiledBinding binding4 = compiledBindingExtension5.ProvideValue(context);
			context.ProvideTargetProperty = null;
			image4.Bind(sourceProperty, binding4);
			StyledProperty<double> widthProperty2 = Layoutable.WidthProperty;
			CompiledBindingExtension compiledBindingExtension6 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Ancestor(typeof(GridLayoutView), 0).Property(GridIconSizeProperty, PropertyInfoAccessorFactory.CreateAvaloniaPropertyAccessor).Build());
			context.ProvideTargetProperty = Layoutable.WidthProperty;
			CompiledBinding binding5 = compiledBindingExtension6.ProvideValue(context);
			context.ProvideTargetProperty = null;
			image4.Bind(widthProperty2, binding5);
			StyledProperty<double> heightProperty = Layoutable.HeightProperty;
			CompiledBindingExtension compiledBindingExtension7 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Ancestor(typeof(GridLayoutView), 0).Property(GridIconSizeProperty, PropertyInfoAccessorFactory.CreateAvaloniaPropertyAccessor).Build());
			context.ProvideTargetProperty = Layoutable.HeightProperty;
			CompiledBinding binding6 = compiledBindingExtension7.ProvideValue(context);
			context.ProvideTargetProperty = null;
			image4.Bind(heightProperty, binding6);
			image4.Margin = new Thickness(0.0, 0.0, 0.0, 8.0);
			Transitions transitions = new Transitions();
			DoubleTransition doubleTransition = new DoubleTransition();
			doubleTransition.Property = Visual.OpacityProperty;
			doubleTransition.Duration = TimeSpan.FromTicks(1500000L);
			transitions.Add(doubleTransition);
			image4.Transitions = transitions;
			context.PopParent();
			((ISupportInitialize)image3).EndInit();
			Controls children2 = stackPanel.Children;
			Grid grid2;
			Grid grid = (grid2 = new Grid());
			((ISupportInitialize)grid).BeginInit();
			children2.Add(grid);
			Grid grid4;
			Grid grid3 = (grid4 = grid2);
			context.PushParent(grid4);
			Controls children3 = grid4.Children;
			TextBlock textBlock2;
			TextBlock textBlock = (textBlock2 = new TextBlock());
			((ISupportInitialize)textBlock).BeginInit();
			children3.Add(textBlock);
			TextBlock textBlock4;
			TextBlock textBlock3 = (textBlock4 = textBlock2);
			context.PushParent(textBlock4);
			textBlock4.Name = "ItemName";
			object element = textBlock4;
			context.AvaloniaNameScope.Register("ItemName", element);
			StyledProperty<string?> textProperty = TextBlock.TextProperty;
			CompiledBindingExtension compiledBindingExtension8 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EData_002EItems_002EListedItem_002CXFolder_002EName_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build());
			context.ProvideTargetProperty = TextBlock.TextProperty;
			CompiledBinding binding7 = compiledBindingExtension8.ProvideValue(context);
			context.ProvideTargetProperty = null;
			textBlock4.Bind(textProperty, binding7);
			textBlock4.TextAlignment = TextAlignment.Center;
			textBlock4.TextWrapping = TextWrapping.Wrap;
			textBlock4.MaxLines = 2;
			textBlock4.TextTrimming = TextTrimming.PrefixCharacterEllipsis;
			StyledProperty<double> maxWidthProperty = Layoutable.MaxWidthProperty;
			CompiledBindingExtension compiledBindingExtension9 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Ancestor(typeof(GridLayoutView), 0).Property(GridTextMaxWidthProperty, PropertyInfoAccessorFactory.CreateAvaloniaPropertyAccessor).Build());
			context.ProvideTargetProperty = Layoutable.MaxWidthProperty;
			CompiledBinding binding8 = compiledBindingExtension9.ProvideValue(context);
			context.ProvideTargetProperty = null;
			textBlock4.Bind(maxWidthProperty, binding8);
			StyledProperty<double> fontSizeProperty = TextBlock.FontSizeProperty;
			CompiledBindingExtension compiledBindingExtension10 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Ancestor(typeof(GridLayoutView), 0).Property(GridTextFontSizeProperty, PropertyInfoAccessorFactory.CreateAvaloniaPropertyAccessor).Build());
			context.ProvideTargetProperty = TextBlock.FontSizeProperty;
			CompiledBinding binding9 = compiledBindingExtension10.ProvideValue(context);
			context.ProvideTargetProperty = null;
			textBlock4.Bind(fontSizeProperty, binding9);
			textBlock4.IsVisible = true;
			context.PopParent();
			((ISupportInitialize)textBlock3).EndInit();
			Controls children4 = grid4.Children;
			TextBox textBox2;
			TextBox textBox = (textBox2 = new TextBox());
			((ISupportInitialize)textBox).BeginInit();
			children4.Add(textBox);
			TextBox textBox4;
			TextBox textBox3 = (textBox4 = textBox2);
			context.PushParent(textBox4);
			textBox4.Name = "TextBoxItemName";
			element = textBox4;
			context.AvaloniaNameScope.Register("TextBoxItemName", element);
			textBox4.TextAlignment = TextAlignment.Center;
			textBox4.TextWrapping = TextWrapping.Wrap;
			StyledProperty<double> maxWidthProperty2 = Layoutable.MaxWidthProperty;
			CompiledBindingExtension compiledBindingExtension11 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Ancestor(typeof(GridLayoutView), 0).Property(GridTextMaxWidthProperty, PropertyInfoAccessorFactory.CreateAvaloniaPropertyAccessor).Build());
			context.ProvideTargetProperty = Layoutable.MaxWidthProperty;
			CompiledBinding binding10 = compiledBindingExtension11.ProvideValue(context);
			context.ProvideTargetProperty = null;
			textBox4.Bind(maxWidthProperty2, binding10);
			StyledProperty<double> fontSizeProperty2 = TemplatedControl.FontSizeProperty;
			CompiledBindingExtension compiledBindingExtension12 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Ancestor(typeof(GridLayoutView), 0).Property(GridTextFontSizeProperty, PropertyInfoAccessorFactory.CreateAvaloniaPropertyAccessor).Build());
			context.ProvideTargetProperty = TemplatedControl.FontSizeProperty;
			CompiledBinding binding11 = compiledBindingExtension12.ProvideValue(context);
			context.ProvideTargetProperty = null;
			textBox4.Bind(fontSizeProperty2, binding11);
			textBox4.AddHandler(InputElement.KeyDownEvent, context.RootObject.RenameTextBox_OnKeyDown);
			textBox4.AddHandler((RoutedEvent)InputElement.LostFocusEvent, (Delegate)new EventHandler<RoutedEventArgs>(context.RootObject.RenameTextBox_OnLostFocus), RoutingStrategies.Direct | RoutingStrategies.Bubble, false);
			textBox4.IsVisible = false;
			context.PopParent();
			((ISupportInitialize)textBox3).EndInit();
			context.PopParent();
			((ISupportInitialize)grid3).EndInit();
			context.PopParent();
			((ISupportInitialize)obj).EndInit();
			return obj;
		}
	}

	protected bool _isItemContextMenu;

	private readonly ILogger<GridLayoutView>? _logger;

	private const double DefaultGridIconSize = 64.0;

	private const double DefaultGridItemWidth = 96.0;

	private const double DefaultGridItemMargin = 12.0;

	private const double MinGridIconSize = 32.0;

	private const double MaxGridIconSize = 128.0;

	private const double GridIconSizeStep = 8.0;

	public static readonly StyledProperty<double> GridIconSizeProperty = AvaloniaProperty.Register<GridLayoutView, double>("GridIconSize", 64.0);

	public static readonly StyledProperty<double> GridItemWidthProperty = AvaloniaProperty.Register<GridLayoutView, double>("GridItemWidth", 96.0);

	public static readonly StyledProperty<double> GridTextMaxWidthProperty = AvaloniaProperty.Register<GridLayoutView, double>("GridTextMaxWidth", 96.0);

	public static readonly StyledProperty<double> GridTextFontSizeProperty = AvaloniaProperty.Register<GridLayoutView, double>("GridTextFontSize", 13.0);

	public static readonly StyledProperty<Thickness> GridItemMarginProperty = AvaloniaProperty.Register<GridLayoutView, Thickness>("GridItemMargin", new Thickness(12.0));

	[GeneratedCode("Avalonia.Generators.NameGenerator.InitializeComponentCodeGenerator", "12.0.1.0")]
	internal Grid rootGrid;

	[GeneratedCode("Avalonia.Generators.NameGenerator.InitializeComponentCodeGenerator", "12.0.1.0")]
	internal Canvas SelectionCanvas;

	[GeneratedCode("Avalonia.Generators.NameGenerator.InitializeComponentCodeGenerator", "12.0.1.0")]
	internal Rectangle SelectionRectangle;

	[GeneratedCode("Avalonia.Generators.NameGenerator.InitializeComponentCodeGenerator", "12.0.1.0")]
	internal ListBox ListedItemListBox;

	[CompilerGenerated]
	private static Action<object> _0021XamlIlPopulateOverride;

	public double GridIconSize
	{
		get
		{
			return GetValue(GridIconSizeProperty);
		}
		set
		{
			SetValue(GridIconSizeProperty, value);
		}
	}

	public double GridItemWidth
	{
		get
		{
			return GetValue(GridItemWidthProperty);
		}
		set
		{
			SetValue(GridItemWidthProperty, value);
		}
	}

	public double GridTextMaxWidth
	{
		get
		{
			return GetValue(GridTextMaxWidthProperty);
		}
		set
		{
			SetValue(GridTextMaxWidthProperty, value);
		}
	}

	public double GridTextFontSize
	{
		get
		{
			return GetValue(GridTextFontSizeProperty);
		}
		set
		{
			SetValue(GridTextFontSizeProperty, value);
		}
	}

	public Thickness GridItemMargin
	{
		get
		{
			return GetValue(GridItemMarginProperty);
		}
		set
		{
			SetValue(GridItemMarginProperty, value);
		}
	}

	protected override Canvas LayoutSelectionCanvas => SelectionCanvas;

	protected override Rectangle LayoutSelectionRectangle => SelectionRectangle;

	protected override IEnumerable<(ListedItem Item, Control Container)> GetVisibleItemsAndContainers()
	{
		if (!(DataContext is GridLayoutViewModel gridLayoutViewModel))
		{
			yield break;
		}
		foreach (ListedItem item in gridLayoutViewModel.Items.OfType<ListedItem>())
		{
			Control control = ListedItemListBox.ContainerFromItem(item);
			if (control != null)
			{
				yield return (Item: item, Container: control);
			}
		}
	}

	public override void FocusItemList()
	{
		ListedItemListBox.Focus();
	}

	protected override void ApplyMarqueeSelection(HashSet<ListedItem> itemsToSelect, HashSet<ListedItem> itemsToUnselect)
	{
		foreach (ListedItem item in itemsToUnselect)
		{
			ListedItemListBox.SelectedItems?.Remove(item);
		}
		foreach (ListedItem item2 in itemsToSelect)
		{
			ListedItemListBox.SelectedItems?.Add(item2);
		}
	}

	public GridLayoutView()
	{
		InitializeComponent();
		DataContextChanged += GridLayoutView_DataContextChanged;
		AddHandler(InputElement.PointerWheelChangedEvent, RootGrid_OnPointerWheelChanged, RoutingStrategies.Tunnel);
		if (!Design.IsDesignMode)
		{
			_logger = Ioc.Default.GetRequiredService<ILogger<GridLayoutView>>();
		}
		ApplyGridMetrics(64.0);
	}

	private void ListBox_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
	{
		if (sender is ListBox listBox && DataContext is GridLayoutViewModel)
		{
			if (listBox.SelectedItems != null)
			{
				SelectedItems = listBox.SelectedItems.OfType<ListedItem>().ToList();
			}
			// Programmatic selection changes (directory reload / selection restore) must not
			// steal keyboard focus from the sidebar tree. Focus is moved to the list only by
			// pointer input, pane switching, or when the view is loaded.
		}
	}

	private void ListBox_OnDoubleTapped(object? sender, TappedEventArgs e)
	{
		if (AvaloniaUIHelpers.IsScrollBar(e.Source))
		{
			e.Handled = true;
		}
		else if (GetItemFromSource(e.Source) != null)
		{
			OpenItem();
			e.Handled = true;
		}
	}

	protected override void OnLoaded(RoutedEventArgs e)
	{
		base.OnLoaded(e);
		ListedItemListBox.AddHandler(InputElement.PointerPressedEvent, ListBox_OnPointerPressed, RoutingStrategies.Bubble, handledEventsToo: true);
		ListedItemListBox.AddHandler(InputElement.PointerMovedEvent, ListBox_OnPointerMoved, RoutingStrategies.Bubble, handledEventsToo: true);
		ApplyGridMetrics(GetCurrentGridIconSize());
		ListedItemListBox.Focus();
	}

	protected override void OnKeyDown(KeyEventArgs e)
	{
		HandleArrowNavigation(e);
		if (!e.Handled)
		{
			base.OnKeyDown(e);
		}
	}

	protected override void OnUnloaded(RoutedEventArgs e)
	{
		ListedItemListBox.RemoveHandler(InputElement.PointerPressedEvent, ListBox_OnPointerPressed);
		ListedItemListBox.RemoveHandler(InputElement.PointerMovedEvent, ListBox_OnPointerMoved);
		base.OnUnloaded(e);
	}

	private void ListBox_OnPointerPressed(object? sender, PointerPressedEventArgs e)
	{
		ListedItemListBox.Focus();
		if (e.GetCurrentPoint(this).Properties.IsRightButtonPressed)
		{
			ListedItem itemFromSource = GetItemFromSource(e.Source);
			if (itemFromSource != null)
			{
				_isItemContextMenu = true;
				if (!SelectedItems.Contains(itemFromSource))
				{
					SelectItemInView(itemFromSource);
				}
			}
		}
		if (ShouldClearSelectionOnEmptyAreaPressed(e))
		{
			ListedItemListBox.SelectedItems?.Clear();
		}
		else
		{
			OnItemPointerPressed(sender, e);
		}
	}

	private void ListBox_OnPointerMoved(object? sender, PointerEventArgs e)
	{
		OnItemPointerMoved(sender, e);
	}

	private void RootGrid_OnPointerPressed(object? sender, PointerPressedEventArgs e)
	{
		HandleMarqueePointerPressed(e);
	}

	private void RootGrid_OnPointerMoved(object? sender, PointerEventArgs e)
	{
		HandleMarqueePointerMoved(e);
	}

	private void RootGrid_OnPointerReleased(object? sender, PointerReleasedEventArgs e)
	{
		HandleMarqueePointerReleased(e);
	}

	private void RootGrid_OnContextRequested(object? sender, ContextRequestedEventArgs e)
	{
		if (!_isItemContextMenu && ListedItemListBox?.SelectedItems != null)
		{
			FocusCurrentModernShellView();
			ListedItemListBox.SelectedItems.Clear();
		}
		ContextMenu contextMenu = CreateContextMenu(_isItemContextMenu);
		if (ListedItemListBox != null)
		{
			contextMenu.Open(ListedItemListBox);
		}
		_isItemContextMenu = false;
		e.Handled = true;
	}

	private void RootGrid_OnDoubleTapped(object? sender, TappedEventArgs e)
	{
		if (!e.Handled && ShouldNavigateUpOnEmptyAreaDoubleTapped(e))
		{
			NavigateUp();
			e.Handled = true;
		}
	}

	private void GridLayoutView_DataContextChanged(object? sender, EventArgs e)
	{
		ApplyGridMetrics(GetCurrentGridIconSize());
	}

	private void RootGrid_OnPointerWheelChanged(object? sender, PointerWheelEventArgs e)
	{
		if (e.KeyModifiers.HasFlag(KeyModifiers.Meta) && DataContext is GridLayoutViewModel { AssociatedInstance: not null } gridLayoutViewModel)
		{
			int num = Math.Sign(e.Delta.Y);
			if (num != 0)
			{
				double num2 = ClampGridIconSize(ClampGridIconSize(gridLayoutViewModel.AssociatedInstance.GridViewIconSize) + (double)num * 8.0);
				gridLayoutViewModel.AssociatedInstance.GridViewIconSize = num2;
				ApplyGridMetrics(num2);
				e.Handled = true;
			}
		}
	}

	private double GetCurrentGridIconSize()
	{
		if (DataContext is GridLayoutViewModel { AssociatedInstance: not null } gridLayoutViewModel)
		{
			return ClampGridIconSize(gridLayoutViewModel.AssociatedInstance.GridViewIconSize);
		}
		return 64.0;
	}

	private static double ClampGridIconSize(double iconSize)
	{
		return Math.Clamp(iconSize, 32.0, 128.0);
	}

	protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
	{
		base.OnPropertyChanged(change);
		if (change.Property == BaseLayoutView.BaseFontSizeProperty)
		{
			ApplyGridMetrics(GetCurrentGridIconSize());
		}
	}

	private void ApplyGridMetrics(double iconSize)
	{
		iconSize = ClampGridIconSize(iconSize);
		GridIconSize = iconSize;
		double num = Math.Max(72.0, Math.Round(iconSize * 1.5, 0));
		double gridTextFontSize = Math.Clamp(Math.Round(iconSize * (BaseFontSize / 64.0), 1), 9.0, 24.0);
		double uniformLength = Math.Clamp(Math.Round(iconSize * 0.1875, 1), 6.0, 24.0);
		GridItemWidth = num;
		GridTextMaxWidth = num;
		GridTextFontSize = gridTextFontSize;
		GridItemMargin = new Thickness(uniformLength);
	}

	public override void StartRenameItem()
	{
		RenamingItem = SelectedItem;
		if (RenamingItem != null && ListedItemListBox.ContainerFromItem(RenamingItem) is ListBoxItem container)
		{
			BeginInlineRenameInContainer(container);
		}
	}

	protected override void SelectItemInView(ListedItem item)
	{
		if (item != null)
		{
			ListedItemListBox.SelectedItems?.Clear();
			ListedItemListBox.SelectedItem = item;
			ListedItemListBox.ScrollIntoView(item);
		}
	}

	protected override void SelectItemsInView(IReadOnlyList<ListedItem> items)
	{
		if (ListedItemListBox.SelectedItems == null)
		{
			return;
		}
		ListedItemListBox.SelectedItems.Clear();
		foreach (ListedItem item in items)
		{
			ListedItemListBox.SelectedItems.Add(item);
		}
		if (items.Count > 0)
		{
			ListedItemListBox.ScrollIntoView(items[0]);
		}
	}

	public override void SelectAllItems()
	{
		ListedItemListBox.SelectAll();
	}

	public override void ClearSelection()
	{
		ListedItemListBox.SelectedItems?.Clear();
	}

	public override void InvertSelection()
	{
		if (!(DataContext is GridLayoutViewModel gridLayoutViewModel) || ListedItemListBox.SelectedItems == null)
		{
			return;
		}
		List<ListedItem> selectedItems = ListedItemListBox.SelectedItems.OfType<ListedItem>().ToList();
		List<ListedItem> list = gridLayoutViewModel.Items.Where((ListedItem item) => !selectedItems.Contains(item)).ToList();
		ListedItemListBox.SelectedItems.Clear();
		foreach (ListedItem item in list)
		{
			ListedItemListBox.SelectedItems.Add(item);
		}
	}

	private void HandleArrowNavigation(KeyEventArgs e)
	{
		if (!e.KeyModifiers.HasFlag(KeyModifiers.Meta))
		{
			switch (e.Key)
			{
			case Key.Left:
				NavigateToAdjacentItem(goNext: false);
				e.Handled = true;
				break;
			case Key.Right:
				NavigateToAdjacentItem(goNext: true);
				e.Handled = true;
				break;
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
		rootGrid = ((nameScope != null) ? nameScope.Find<Grid>("rootGrid") : null);
		SelectionCanvas = ((nameScope != null) ? nameScope.Find<Canvas>("SelectionCanvas") : null);
		SelectionRectangle = ((nameScope != null) ? nameScope.Find<Rectangle>("SelectionRectangle") : null);
		ListedItemListBox = ((nameScope != null) ? nameScope.Find<ListBox>("ListedItemListBox") : null);
	}

	[CompilerGenerated]
	private unsafe static void _0021XamlIlPopulate(IServiceProvider P_0, GridLayoutView P_1)
	{
		XamlIlContext.Context<GridLayoutView> context = new XamlIlContext.Context<GridLayoutView>(P_0, new object[1] { _0021AvaloniaResources.NamespaceInfo_003A_002FViews_002FLayouts_002FGridLayoutView_002Eaxaml.Singleton }, "avares://XFolder/Views/Layouts/GridLayoutView.axaml")
		{
			RootObject = P_1,
			IntermediateRoot = P_1
		};
		((ISupportInitialize)P_1).BeginInit();
		context.PushParent(P_1);
		StyledProperty<double> baseFontSizeProperty = BaseLayoutView.BaseFontSizeProperty;
		DynamicResourceExtension dynamicResourceExtension = new DynamicResourceExtension("GlobalFontSize");
		context.ProvideTargetProperty = BaseLayoutView.BaseFontSizeProperty;
		BindingBase binding = dynamicResourceExtension.ProvideValue(context);
		context.ProvideTargetProperty = null;
		P_1.Bind(baseFontSizeProperty, binding);
		P_1.Focusable = true;
		((ResourceDictionary)P_1.Resources).AddDeferred((object)"FileFolderIconConverter", XamlIlRuntimeHelpers.DeferredTransformationFactoryV3<object>((nint)(delegate*<IServiceProvider, object>)(&XamlClosure_7.Build_1), context));
		Grid grid2;
		Grid grid = (grid2 = new Grid());
		((ISupportInitialize)grid).BeginInit();
		P_1.Content = grid;
		Grid grid4;
		Grid grid3 = (grid4 = grid2);
		context.PushParent(grid4);
		grid4.Name = "rootGrid";
		object element = grid4;
		context.AvaloniaNameScope.Register("rootGrid", element);
		grid4.Background = new ImmutableSolidColorBrush(16777215u);
		grid4.AddHandler(InputElement.ContextRequestedEvent, context.RootObject.RootGrid_OnContextRequested);
		grid4.AddHandler(InputElement.DoubleTappedEvent, context.RootObject.RootGrid_OnDoubleTapped);
		grid4.AddHandler(InputElement.PointerPressedEvent, context.RootObject.RootGrid_OnPointerPressed);
		grid4.AddHandler(InputElement.PointerMovedEvent, context.RootObject.RootGrid_OnPointerMoved);
		grid4.AddHandler(InputElement.PointerReleasedEvent, context.RootObject.RootGrid_OnPointerReleased);
		Controls children = grid4.Children;
		Canvas canvas2;
		Canvas canvas = (canvas2 = new Canvas());
		((ISupportInitialize)canvas).BeginInit();
		children.Add(canvas);
		Canvas canvas4;
		Canvas canvas3 = (canvas4 = canvas2);
		context.PushParent(canvas4);
		canvas4.Name = "SelectionCanvas";
		element = canvas4;
		context.AvaloniaNameScope.Register("SelectionCanvas", element);
		canvas4.IsHitTestVisible = false;
		canvas4.ClipToBounds = true;
		canvas4.ZIndex = 1;
		Controls children2 = canvas4.Children;
		Rectangle rectangle2;
		Rectangle rectangle = (rectangle2 = new Rectangle());
		((ISupportInitialize)rectangle).BeginInit();
		children2.Add(rectangle);
		Rectangle rectangle4;
		Rectangle rectangle3 = (rectangle4 = rectangle2);
		context.PushParent(rectangle4);
		rectangle4.Name = "SelectionRectangle";
		element = rectangle4;
		context.AvaloniaNameScope.Register("SelectionRectangle", element);
		StyledProperty<IBrush?> fillProperty = Shape.FillProperty;
		DynamicResourceExtension dynamicResourceExtension2 = new DynamicResourceExtension("SystemAccentColorLight2");
		context.ProvideTargetProperty = Shape.FillProperty;
		BindingBase binding2 = dynamicResourceExtension2.ProvideValue(context);
		context.ProvideTargetProperty = null;
		rectangle4.Bind(fillProperty, binding2);
		rectangle4.Opacity = 0.4;
		StyledProperty<IBrush?> strokeProperty = Shape.StrokeProperty;
		DynamicResourceExtension dynamicResourceExtension3 = new DynamicResourceExtension("SystemAccentColor");
		context.ProvideTargetProperty = Shape.StrokeProperty;
		BindingBase binding3 = dynamicResourceExtension3.ProvideValue(context);
		context.ProvideTargetProperty = null;
		rectangle4.Bind(strokeProperty, binding3);
		rectangle4.StrokeThickness = 1.0;
		rectangle4.IsVisible = false;
		context.PopParent();
		((ISupportInitialize)rectangle3).EndInit();
		context.PopParent();
		((ISupportInitialize)canvas3).EndInit();
		Controls children3 = grid4.Children;
		ListBox listBox2;
		ListBox listBox = (listBox2 = new ListBox());
		((ISupportInitialize)listBox).BeginInit();
		children3.Add(listBox);
		ListBox listBox4;
		ListBox listBox3 = (listBox4 = listBox2);
		context.PushParent(listBox4);
		listBox4.Name = "ListedItemListBox";
		element = listBox4;
		context.AvaloniaNameScope.Register("ListedItemListBox", element);
		listBox4.Background = new ImmutableSolidColorBrush(16777215u);
		StyledProperty<IEnumerable?> itemsSourceProperty = ItemsControl.ItemsSourceProperty;
		CompiledBindingExtension compiledBindingExtension = new CompiledBindingExtension(new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EViewModels_002ELayouts_002EBaseLayoutViewModel_002CXFolder_002EItems_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build());
		context.ProvideTargetProperty = ItemsControl.ItemsSourceProperty;
		CompiledBinding binding4 = compiledBindingExtension.ProvideValue(context);
		context.ProvideTargetProperty = null;
		listBox4.Bind(itemsSourceProperty, binding4);
		listBox4.SelectionMode = SelectionMode.Multiple;
		listBox4.AddHandler(SelectingItemsControl.SelectionChangedEvent, context.RootObject.ListBox_OnSelectionChanged);
		listBox4.AddHandler(InputElement.DoubleTappedEvent, context.RootObject.ListBox_OnDoubleTapped);
		ScrollViewer.SetHorizontalScrollBarVisibility(listBox4, ScrollBarVisibility.Disabled);
		ScrollViewer.SetVerticalScrollBarVisibility(listBox4, ScrollBarVisibility.Auto);
		listBox4.ItemsPanel = new ItemsPanelTemplate
		{
			Content = XamlIlRuntimeHelpers.DeferredTransformationFactoryV3<Control>((nint)(delegate*<IServiceProvider, object>)(&XamlClosure_7.Build_2), context)
		};
		DataTemplate dataTemplate;
		DataTemplate itemTemplate = (dataTemplate = new DataTemplate());
		context.PushParent(dataTemplate);
		dataTemplate.Content = XamlIlRuntimeHelpers.DeferredTransformationFactoryV3<Control>((nint)(delegate*<IServiceProvider, object>)(&XamlClosure_7.Build_3), context);
		context.PopParent();
		listBox4.ItemTemplate = itemTemplate;
		Styles styles = listBox4.Styles;
		Style style = new Style();
		style.Selector = ((Selector?)null).OfType(typeof(ListBoxItem));
		Setter setter = new Setter();
		setter.Property = TemplatedControl.PaddingProperty;
		setter.Value = new Thickness(0.0, 0.0, 0.0, 0.0);
		style.Add(setter);
		Setter setter2 = new Setter();
		setter2.Property = TemplatedControl.BackgroundProperty;
		setter2.Value = new ImmutableSolidColorBrush(16777215u);
		style.Add(setter2);
		Setter setter3 = new Setter();
		setter3.Property = ContentControl.HorizontalContentAlignmentProperty;
		setter3.Value = HorizontalAlignment.Center;
		style.Add(setter3);
		styles.Add(style);
		context.PopParent();
		((ISupportInitialize)listBox3).EndInit();
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
	private static void _0021XamlIlPopulateTrampoline(GridLayoutView P_0)
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
