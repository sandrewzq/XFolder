using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using Avalonia;
using Avalonia.Animation;
using Avalonia.Controls;
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
using Avalonia.Markup.Xaml.Templates;
using Avalonia.Markup.Xaml.XamlIl.Runtime;
using Avalonia.Media;
using Avalonia.Media.Immutable;
using Avalonia.Styling;
using Avalonia.VisualTree;
using CommunityToolkit.Mvvm.DependencyInjection;
using CompiledAvaloniaXaml;
using Files.Converters;
using Files.Data.Enums;
using Files.Data.Items;
using Files.Helpers.UI;
using Files.Services.App;
using Files.ViewModels.Layouts;
using Microsoft.Extensions.Logging;

namespace Files.Views.Layouts;

public class DetailsLayoutView : BaseLayoutView
{
	[CompilerGenerated]
	private class XamlClosure_6
	{
		public static object Build_1(IServiceProvider P_0)
		{
			XamlIlContext.Context<DetailsLayoutView> context = CreateContext(P_0);
			return new FileFolderIconConverter();
		}

		public static XamlIlContext.Context<DetailsLayoutView> CreateContext(IServiceProvider P_0)
		{
			XamlIlContext.Context<DetailsLayoutView> context = new XamlIlContext.Context<DetailsLayoutView>(P_0, new object[1] { _0021AvaloniaResources.NamespaceInfo_003A_002FViews_002FLayouts_002FDetailsLayoutView_002Eaxaml.Singleton }, "avares://XFolder/Views/Layouts/DetailsLayoutView.axaml");
			if (P_0 != null)
			{
				object service = P_0.GetService(typeof(IRootObjectProvider));
				if (service != null)
				{
					service = ((IRootObjectProvider)service).RootObject;
					context.RootObject = (DetailsLayoutView)service;
				}
			}
			return context;
		}

		public static object Build_2(IServiceProvider P_0)
		{
			XamlIlContext.Context<DetailsLayoutView> context = CreateContext(P_0);
			return new FontSizeToIconSizeConverter();
		}

		public static object Build_3(IServiceProvider P_0)
		{
			XamlIlContext.Context<DetailsLayoutView> context = CreateContext(P_0);
			context.IntermediateRoot = new Image();
			object obj = context.IntermediateRoot;
			((ISupportInitialize)obj).BeginInit();
			Image image = (Image)obj;
			context.PushParent(image);
			StyledProperty<IImage?> sourceProperty = Image.SourceProperty;
			CompiledBindingExtension compiledBindingExtension = new CompiledBindingExtension(new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EData_002EItems_002EListedItem_002CXFolder_002EThumbnail_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build());
			context.ProvideTargetProperty = Image.SourceProperty;
			CompiledBinding binding = compiledBindingExtension.ProvideValue(context);
			context.ProvideTargetProperty = null;
			image.Bind(sourceProperty, binding);
			StyledProperty<double> widthProperty = Layoutable.WidthProperty;
			CompiledBindingExtension compiledBindingExtension3;
			CompiledBindingExtension compiledBindingExtension2 = (compiledBindingExtension3 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Ancestor(typeof(DataGrid), 0).Property(TemplatedControl.FontSizeProperty, PropertyInfoAccessorFactory.CreateAvaloniaPropertyAccessor).Build()));
			context.PushParent(compiledBindingExtension3);
			CompiledBindingExtension compiledBindingExtension4 = compiledBindingExtension3;
			StaticResourceExtension staticResourceExtension = new StaticResourceExtension("FontSizeToIconSizeConverter");
			context.ProvideTargetProperty = XamlIlHelpers.Avalonia_002EData_002ECompiledBinding_002CAvalonia_002EBase_002EConverter_0021Property();
			object? obj2 = staticResourceExtension.ProvideValue(context);
			context.ProvideTargetProperty = null;
			compiledBindingExtension4.Converter = (IValueConverter)obj2;
			compiledBindingExtension4.ConverterParameter = "1.85";
			context.PopParent();
			context.ProvideTargetProperty = Layoutable.WidthProperty;
			CompiledBinding binding2 = compiledBindingExtension2.ProvideValue(context);
			context.ProvideTargetProperty = null;
			image.Bind(widthProperty, binding2);
			StyledProperty<double> heightProperty = Layoutable.HeightProperty;
			CompiledBindingExtension compiledBindingExtension5 = (compiledBindingExtension3 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Ancestor(typeof(DataGrid), 0).Property(TemplatedControl.FontSizeProperty, PropertyInfoAccessorFactory.CreateAvaloniaPropertyAccessor).Build()));
			context.PushParent(compiledBindingExtension3);
			CompiledBindingExtension compiledBindingExtension6 = compiledBindingExtension3;
			StaticResourceExtension staticResourceExtension2 = new StaticResourceExtension("FontSizeToIconSizeConverter");
			context.ProvideTargetProperty = XamlIlHelpers.Avalonia_002EData_002ECompiledBinding_002CAvalonia_002EBase_002EConverter_0021Property();
			object? obj3 = staticResourceExtension2.ProvideValue(context);
			context.ProvideTargetProperty = null;
			compiledBindingExtension6.Converter = (IValueConverter)obj3;
			compiledBindingExtension6.ConverterParameter = "1.85";
			context.PopParent();
			context.ProvideTargetProperty = Layoutable.HeightProperty;
			CompiledBinding binding3 = compiledBindingExtension5.ProvideValue(context);
			context.ProvideTargetProperty = null;
			image.Bind(heightProperty, binding3);
			image.Margin = new Thickness(16.0, 4.0, 16.0, 4.0);
			StyledProperty<double> opacityProperty = Visual.OpacityProperty;
			CompiledBindingExtension compiledBindingExtension7 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EData_002EItems_002EListedItem_002CXFolder_002EOpacity_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build());
			context.ProvideTargetProperty = Visual.OpacityProperty;
			CompiledBinding binding4 = compiledBindingExtension7.ProvideValue(context);
			context.ProvideTargetProperty = null;
			image.Bind(opacityProperty, binding4);
			Transitions transitions = new Transitions();
			DoubleTransition doubleTransition = new DoubleTransition();
			doubleTransition.Property = Visual.OpacityProperty;
			doubleTransition.Duration = TimeSpan.FromTicks(1500000L);
			transitions.Add(doubleTransition);
			image.Transitions = transitions;
			context.PopParent();
			((ISupportInitialize)obj).EndInit();
			return obj;
		}

		public static object Build_4(IServiceProvider P_0)
		{
			XamlIlContext.Context<DetailsLayoutView> context = CreateContext(P_0);
			context.IntermediateRoot = new Grid();
			object obj = context.IntermediateRoot;
			((ISupportInitialize)obj).BeginInit();
			Grid grid = (Grid)obj;
			context.PushParent(grid);
			StyledProperty<double> opacityProperty = Visual.OpacityProperty;
			CompiledBindingExtension compiledBindingExtension = new CompiledBindingExtension(new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EData_002EItems_002EListedItem_002CXFolder_002EOpacity_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build());
			context.ProvideTargetProperty = Visual.OpacityProperty;
			CompiledBinding binding = compiledBindingExtension.ProvideValue(context);
			context.ProvideTargetProperty = null;
			grid.Bind(opacityProperty, binding);
			grid.HorizontalAlignment = HorizontalAlignment.Stretch;
			Controls children = grid.Children;
			TextBlock textBlock2;
			TextBlock textBlock = (textBlock2 = new TextBlock());
			((ISupportInitialize)textBlock).BeginInit();
			children.Add(textBlock);
			TextBlock textBlock4;
			TextBlock textBlock3 = (textBlock4 = textBlock2);
			context.PushParent(textBlock4);
			textBlock4.Name = "ItemName";
			object element = textBlock4;
			context.AvaloniaNameScope.Register("ItemName", element);
			StyledProperty<string?> textProperty = TextBlock.TextProperty;
			CompiledBindingExtension compiledBindingExtension2 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EData_002EItems_002EListedItem_002CXFolder_002EName_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build());
			context.ProvideTargetProperty = TextBlock.TextProperty;
			CompiledBinding binding2 = compiledBindingExtension2.ProvideValue(context);
			context.ProvideTargetProperty = null;
			textBlock4.Bind(textProperty, binding2);
			textBlock4.TextWrapping = TextWrapping.NoWrap;
			textBlock4.TextTrimming = TextTrimming.CharacterEllipsis;
			textBlock4.IsVisible = true;
			textBlock4.HorizontalAlignment = HorizontalAlignment.Stretch;
			textBlock4.VerticalAlignment = VerticalAlignment.Center;
			textBlock4.Margin = new Thickness(0.0, 0.0, 0.0, 0.0);
			context.PopParent();
			((ISupportInitialize)textBlock3).EndInit();
			Controls children2 = grid.Children;
			TextBox textBox2;
			TextBox textBox = (textBox2 = new TextBox());
			((ISupportInitialize)textBox).BeginInit();
			children2.Add(textBox);
			textBox2.Name = "TextBoxItemName";
			element = textBox2;
			context.AvaloniaNameScope.Register("TextBoxItemName", element);
			textBox2.AddHandler(InputElement.KeyDownEvent, context.RootObject.RenameTextBox_OnKeyDown);
			textBox2.AddHandler((RoutedEvent)InputElement.LostFocusEvent, (Delegate)new EventHandler<RoutedEventArgs>(context.RootObject.RenameTextBox_OnLostFocus), RoutingStrategies.Direct | RoutingStrategies.Bubble, false);
			textBox2.IsVisible = false;
			textBox2.HorizontalAlignment = HorizontalAlignment.Stretch;
			textBox2.VerticalAlignment = VerticalAlignment.Center;
			((ISupportInitialize)textBox2).EndInit();
			context.PopParent();
			((ISupportInitialize)obj).EndInit();
			return obj;
		}

		public static object Build_5(IServiceProvider P_0)
		{
			XamlIlContext.Context<DetailsLayoutView> context = CreateContext(P_0);
			context.IntermediateRoot = new TextBlock();
			object obj = context.IntermediateRoot;
			((ISupportInitialize)obj).BeginInit();
			TextBlock textBlock = (TextBlock)obj;
			context.PushParent(textBlock);
			textBlock.VerticalAlignment = VerticalAlignment.Center;
			StyledProperty<string?> textProperty = TextBlock.TextProperty;
			CompiledBindingExtension compiledBindingExtension = new CompiledBindingExtension(new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EData_002EItems_002EListedItem_002CXFolder_002EFileSizeDisplay_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build());
			context.ProvideTargetProperty = TextBlock.TextProperty;
			CompiledBinding binding = compiledBindingExtension.ProvideValue(context);
			context.ProvideTargetProperty = null;
			textBlock.Bind(textProperty, binding);
			StyledProperty<double> opacityProperty = Visual.OpacityProperty;
			CompiledBindingExtension compiledBindingExtension2 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EData_002EItems_002EListedItem_002CXFolder_002EOpacity_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build());
			context.ProvideTargetProperty = Visual.OpacityProperty;
			CompiledBinding binding2 = compiledBindingExtension2.ProvideValue(context);
			context.ProvideTargetProperty = null;
			textBlock.Bind(opacityProperty, binding2);
			context.PopParent();
			((ISupportInitialize)obj).EndInit();
			return obj;
		}

		public static object Build_6(IServiceProvider P_0)
		{
			XamlIlContext.Context<DetailsLayoutView> context = CreateContext(P_0);
			context.IntermediateRoot = new TextBlock();
			object obj = context.IntermediateRoot;
			((ISupportInitialize)obj).BeginInit();
			TextBlock textBlock = (TextBlock)obj;
			context.PushParent(textBlock);
			textBlock.VerticalAlignment = VerticalAlignment.Center;
			StyledProperty<string?> textProperty = TextBlock.TextProperty;
			CompiledBindingExtension compiledBindingExtension = new CompiledBindingExtension(new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EData_002EItems_002EListedItem_002CXFolder_002EDisplayItemType_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build());
			context.ProvideTargetProperty = TextBlock.TextProperty;
			CompiledBinding binding = compiledBindingExtension.ProvideValue(context);
			context.ProvideTargetProperty = null;
			textBlock.Bind(textProperty, binding);
			textBlock.TextWrapping = TextWrapping.NoWrap;
			textBlock.TextTrimming = TextTrimming.CharacterEllipsis;
			StyledProperty<double> opacityProperty = Visual.OpacityProperty;
			CompiledBindingExtension compiledBindingExtension2 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EData_002EItems_002EListedItem_002CXFolder_002EOpacity_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build());
			context.ProvideTargetProperty = Visual.OpacityProperty;
			CompiledBinding binding2 = compiledBindingExtension2.ProvideValue(context);
			context.ProvideTargetProperty = null;
			textBlock.Bind(opacityProperty, binding2);
			context.PopParent();
			((ISupportInitialize)obj).EndInit();
			return obj;
		}

		public static object Build_7(IServiceProvider P_0)
		{
			XamlIlContext.Context<DetailsLayoutView> context = CreateContext(P_0);
			context.IntermediateRoot = new TextBlock();
			object obj = context.IntermediateRoot;
			((ISupportInitialize)obj).BeginInit();
			TextBlock textBlock = (TextBlock)obj;
			context.PushParent(textBlock);
			textBlock.VerticalAlignment = VerticalAlignment.Center;
			StyledProperty<string?> textProperty = TextBlock.TextProperty;
			CompiledBindingExtension compiledBindingExtension = new CompiledBindingExtension(new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EData_002EItems_002EListedItem_002CXFolder_002EItemDateModified_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build());
			context.ProvideTargetProperty = TextBlock.TextProperty;
			CompiledBinding binding = compiledBindingExtension.ProvideValue(context);
			context.ProvideTargetProperty = null;
			textBlock.Bind(textProperty, binding);
			StyledProperty<double> opacityProperty = Visual.OpacityProperty;
			CompiledBindingExtension compiledBindingExtension2 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EData_002EItems_002EListedItem_002CXFolder_002EOpacity_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build());
			context.ProvideTargetProperty = Visual.OpacityProperty;
			CompiledBinding binding2 = compiledBindingExtension2.ProvideValue(context);
			context.ProvideTargetProperty = null;
			textBlock.Bind(opacityProperty, binding2);
			context.PopParent();
			((ISupportInitialize)obj).EndInit();
			return obj;
		}
	}

	private readonly ILogger<DetailsLayoutView>? _logger;

	[GeneratedCode("Avalonia.Generators.NameGenerator.InitializeComponentCodeGenerator", "12.0.1.0")]
	internal Grid rootGrid;

	[GeneratedCode("Avalonia.Generators.NameGenerator.InitializeComponentCodeGenerator", "12.0.1.0")]
	internal Canvas SelectionCanvas;

	[GeneratedCode("Avalonia.Generators.NameGenerator.InitializeComponentCodeGenerator", "12.0.1.0")]
	internal Rectangle SelectionRectangle;

	[GeneratedCode("Avalonia.Generators.NameGenerator.InitializeComponentCodeGenerator", "12.0.1.0")]
	internal DataGrid listedItemList;

	[CompilerGenerated]
	private static Action<object> _0021XamlIlPopulateOverride;

	protected override Canvas LayoutSelectionCanvas => SelectionCanvas;

	protected override Rectangle LayoutSelectionRectangle => SelectionRectangle;

	public DetailsLayoutView()
	{
		InitializeComponent();
		if (!Design.IsDesignMode)
		{
			_logger = Ioc.Default.GetRequiredService<ILogger<DetailsLayoutView>>();
		}
	}

	protected override IEnumerable<(ListedItem Item, Control Container)> GetVisibleItemsAndContainers()
	{
		foreach (DataGridRow item2 in listedItemList.GetVisualDescendants().OfType<DataGridRow>())
		{
			if (item2.DataContext is ListedItem item)
			{
				yield return (Item: item, Container: item2);
			}
		}
	}

	public override void FocusItemList()
	{
		listedItemList.Focus();
	}

	protected override void ApplyMarqueeSelection(HashSet<ListedItem> itemsToSelect, HashSet<ListedItem> itemsToUnselect)
	{
		foreach (ListedItem item in itemsToUnselect)
		{
			listedItemList.SelectedItems?.Remove(item);
		}
		foreach (ListedItem item2 in itemsToSelect)
		{
			listedItemList.SelectedItems?.Add(item2);
		}
	}

	private void DataGrid_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
	{
		if (sender is DataGrid dataGrid && DataContext is DetailsLayoutViewModel)
		{
			SelectedItems = dataGrid.SelectedItems.OfType<ListedItem>().ToList();
			// Programmatic selection changes (directory reload / selection restore) must not
			// steal keyboard focus from the sidebar tree. Focus is moved to the list only by
			// pointer input, pane switching, or when the view is loaded.
		}
	}

	private void DataGrid_OnDoubleTapped(object? sender, TappedEventArgs e)
	{
		if (AvaloniaUIHelpers.IsDataGridColumnHeader(e.Source) || AvaloniaUIHelpers.IsScrollBar(e.Source))
		{
			e.Handled = true;
		}
		else if (GetItemFromSource(e.Source) != null)
		{
			OpenItem();
			e.Handled = true;
		}
	}

	private void DataGrid_OnSorting(object? sender, DataGridColumnEventArgs e)
	{
		if (DataContext is DetailsLayoutViewModel)
		{
			string sortMemberPath = e.Column.SortMemberPath;
			if (!string.IsNullOrWhiteSpace(sortMemberPath))
			{
				e.Handled = true;
				SortOption sortOption = BaseLayoutViewModel.GetSortOption(sortMemberPath);
				SortDirection sortDirection = ((HasCustomSortState() && GetCurrentSortOption() == sortOption && GetCurrentSortDirection() == SortDirection.Ascending) ? SortDirection.Descending : SortDirection.Ascending);
				UpdateCurrentSortState(sortOption, sortDirection);
			}
		}
	}

	protected override void OnLoaded(RoutedEventArgs e)
	{
		base.OnLoaded(e);
		listedItemList.AddHandler(InputElement.PointerPressedEvent, DataGrid_OnPointerPressed, RoutingStrategies.Bubble, handledEventsToo: true);
		listedItemList.AddHandler(InputElement.PointerMovedEvent, DataGrid_OnPointerMoved, RoutingStrategies.Bubble, handledEventsToo: true);
		listedItemList.Focus();
	}

	protected override void OnUnloaded(RoutedEventArgs e)
	{
		listedItemList.RemoveHandler(InputElement.PointerPressedEvent, DataGrid_OnPointerPressed);
		listedItemList.RemoveHandler(InputElement.PointerMovedEvent, DataGrid_OnPointerMoved);
		base.OnUnloaded(e);
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

	private void RootGrid_OnDoubleTapped(object? sender, TappedEventArgs e)
	{
		if (!e.Handled && ShouldNavigateUpOnEmptyAreaDoubleTapped(e))
		{
			NavigateUp();
			e.Handled = true;
		}
	}

	private void DataGrid_OnPointerPressed(object? sender, PointerPressedEventArgs e)
	{
		listedItemList.Focus();
		if (ShouldClearSelectionOnEmptyAreaPressed(e))
		{
			listedItemList.SelectedItems.Clear();
		}
		else
		{
			OnItemPointerPressed(sender, e);
		}
	}

	private void DataGrid_OnPointerMoved(object? sender, PointerEventArgs e)
	{
		OnItemPointerMoved(sender, e);
	}

	protected override void OnKeyDown(KeyEventArgs e)
	{
		HandleArrowNavigation(e);
		if (!e.Handled)
		{
			base.OnKeyDown(e);
		}
	}

	private void RootGrid_OnContextRequested(object? sender, ContextRequestedEventArgs e)
	{
		if (AvaloniaUIHelpers.IsDataGridColumnHeader(e.Source))
		{
			e.Handled = true;
			return;
		}
		FocusCurrentModernShellView();
		listedItemList.SelectedItems.Clear();
		CreateContextMenu(isItemContextMenu: false).Open(listedItemList);
		e.Handled = true;
	}

	private void ItemList_OnContextRequested(object? sender, ContextRequestedEventArgs e)
	{
		if (AvaloniaUIHelpers.IsDataGridColumnHeader(e.Source))
		{
			e.Handled = true;
			return;
		}
		CreateContextMenu(isItemContextMenu: true).Open(listedItemList);
		e.Handled = true;
	}

	public override void StartRenameItem()
	{
		RenamingItem = SelectedItem;
		if (RenamingItem != null)
		{
			DataGridRow dataGridRow = listedItemList.GetVisualDescendants().OfType<DataGridRow>().FirstOrDefault((DataGridRow r) => r.DataContext == RenamingItem);
			if (dataGridRow != null)
			{
				BeginInlineRenameInContainer(dataGridRow);
			}
		}
	}

	protected override void SelectItemInView(ListedItem item)
	{
		if (item != null)
		{
			listedItemList.SelectedItems.Clear();
			listedItemList.SelectedItem = item;
			listedItemList.ScrollIntoView(item, null);
		}
	}

	protected override void SelectItemsInView(IReadOnlyList<ListedItem> items)
	{
		listedItemList.SelectedItems.Clear();
		foreach (ListedItem item in items)
		{
			listedItemList.SelectedItems.Add(item);
		}
		if (items.Count > 0)
		{
			listedItemList.ScrollIntoView(items[0], null);
		}
	}

	public override void SelectAllItems()
	{
		listedItemList.SelectAll();
	}

	public override void ClearSelection()
	{
		listedItemList.SelectedItems.Clear();
	}

	public override void InvertSelection()
	{
		if (!(DataContext is DetailsLayoutViewModel detailsLayoutViewModel))
		{
			return;
		}
		List<ListedItem> selectedItems = listedItemList.SelectedItems.OfType<ListedItem>().ToList();
		List<ListedItem> list = detailsLayoutViewModel.Items.Where((ListedItem item) => !selectedItems.Contains(item)).ToList();
		listedItemList.SelectedItems.Clear();
		foreach (ListedItem item in list)
		{
			listedItemList.SelectedItems.Add(item);
		}
	}

	private void HandleArrowNavigation(KeyEventArgs e)
	{
		switch (e.Key)
		{
		case Key.Up:
			NavigateToAdjacentItem(goNext: false);
			e.Handled = true;
			break;
		case Key.Down:
			NavigateToAdjacentItem(goNext: true);
			e.Handled = true;
			break;
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
		listedItemList = ((nameScope != null) ? nameScope.Find<DataGrid>("listedItemList") : null);
	}

	[CompilerGenerated]
	private unsafe static void _0021XamlIlPopulate(IServiceProvider P_0, DetailsLayoutView P_1)
	{
		XamlIlContext.Context<DetailsLayoutView> context = new XamlIlContext.Context<DetailsLayoutView>(P_0, new object[1] { _0021AvaloniaResources.NamespaceInfo_003A_002FViews_002FLayouts_002FDetailsLayoutView_002Eaxaml.Singleton }, "avares://XFolder/Views/Layouts/DetailsLayoutView.axaml")
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
		StyledProperty<double> baseFontSizeProperty = BaseLayoutView.BaseFontSizeProperty;
		DynamicResourceExtension dynamicResourceExtension = new DynamicResourceExtension("GlobalFontSize");
		context.ProvideTargetProperty = BaseLayoutView.BaseFontSizeProperty;
		BindingBase binding = dynamicResourceExtension.ProvideValue(context);
		context.ProvideTargetProperty = null;
		P_1.Bind(baseFontSizeProperty, binding);
		P_1.Focusable = true;
		((ResourceDictionary)P_1.Resources).AddDeferred((object)"FileFolderIconConverter", XamlIlRuntimeHelpers.DeferredTransformationFactoryV3<object>((nint)(delegate*<IServiceProvider, object>)(&XamlClosure_6.Build_1), context));
		((ResourceDictionary)P_1.Resources).AddDeferred((object)"FontSizeToIconSizeConverter", XamlIlRuntimeHelpers.DeferredTransformationFactoryV3<object>((nint)(delegate*<IServiceProvider, object>)(&XamlClosure_6.Build_2), context));
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
		DataGrid dataGrid2;
		DataGrid dataGrid = (dataGrid2 = new DataGrid());
		((ISupportInitialize)dataGrid).BeginInit();
		children3.Add(dataGrid);
		DataGrid dataGrid4;
		DataGrid dataGrid3 = (dataGrid4 = dataGrid2);
		context.PushParent(dataGrid4);
		dataGrid4.Name = "listedItemList";
		element = dataGrid4;
		context.AvaloniaNameScope.Register("listedItemList", element);
		StyledProperty<IEnumerable> itemsSourceProperty = DataGrid.ItemsSourceProperty;
		CompiledBindingExtension compiledBindingExtension = new CompiledBindingExtension(new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EViewModels_002ELayouts_002EBaseLayoutViewModel_002CXFolder_002EItems_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build());
		context.ProvideTargetProperty = DataGrid.ItemsSourceProperty;
		CompiledBinding binding4 = compiledBindingExtension.ProvideValue(context);
		context.ProvideTargetProperty = null;
		dataGrid4.Bind(itemsSourceProperty, binding4);
		dataGrid4.AutoGenerateColumns = false;
		dataGrid4.Margin = new Thickness(0.0, 0.0, 0.0, 0.0);
		dataGrid4.IsReadOnly = true;
		StyledProperty<double> fontSizeProperty = TemplatedControl.FontSizeProperty;
		CompiledBindingExtension compiledBindingExtension2 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Ancestor(typeof(DetailsLayoutView), 0).Property(BaseLayoutView.BaseFontSizeProperty, PropertyInfoAccessorFactory.CreateAvaloniaPropertyAccessor).Build());
		context.ProvideTargetProperty = TemplatedControl.FontSizeProperty;
		CompiledBinding binding5 = compiledBindingExtension2.ProvideValue(context);
		context.ProvideTargetProperty = null;
		dataGrid4.Bind(fontSizeProperty, binding5);
		dataGrid4.CanUserResizeColumns = true;
		dataGrid4.CanUserReorderColumns = true;
		dataGrid4.GridLinesVisibility = DataGridGridLinesVisibility.None;
		dataGrid4.AddHandler(DataGrid.SelectionChangedEvent, context.RootObject.DataGrid_OnSelectionChanged);
		dataGrid4.Sorting += context.RootObject.DataGrid_OnSorting;
		dataGrid4.AddHandler(InputElement.DoubleTappedEvent, context.RootObject.DataGrid_OnDoubleTapped);
		dataGrid4.AddHandler(InputElement.ContextRequestedEvent, context.RootObject.ItemList_OnContextRequested);
		dataGrid4.SelectionMode = DataGridSelectionMode.Extended;
		Styles styles = dataGrid4.Styles;
		Style style;
		Style item = (style = new Style());
		context.PushParent(style);
		Style style2 = style;
		style2.Selector = ((Selector?)null).OfType(typeof(DataGridRow));
		Setter setter2;
		Setter setter = (setter2 = new Setter());
		context.PushParent(setter2);
		Setter setter3 = setter2;
		setter3.Property = TemplatedControl.BackgroundProperty;
		DynamicResourceExtension dynamicResourceExtension4 = new DynamicResourceExtension("DetailsListRowBackgroundBrush");
		context.ProvideTargetProperty = XamlIlHelpers.Avalonia_002EStyling_002ESetter_002CAvalonia_002EBase_002EValue_0021Property();
		BindingBase value = dynamicResourceExtension4.ProvideValue(context);
		context.ProvideTargetProperty = null;
		setter3.Value = value;
		context.PopParent();
		style2.Add(setter);
		Setter setter4 = (setter2 = new Setter());
		context.PushParent(setter2);
		Setter setter5 = setter2;
		setter5.Property = Layoutable.MinHeightProperty;
		CompiledBindingExtension compiledBindingExtension4;
		CompiledBindingExtension compiledBindingExtension3 = (compiledBindingExtension4 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Ancestor(typeof(DataGrid), 0).Property(TemplatedControl.FontSizeProperty, PropertyInfoAccessorFactory.CreateAvaloniaPropertyAccessor).Build()));
		context.PushParent(compiledBindingExtension4);
		StaticResourceExtension staticResourceExtension = new StaticResourceExtension("FontSizeToIconSizeConverter");
		context.ProvideTargetProperty = XamlIlHelpers.Avalonia_002EData_002ECompiledBinding_002CAvalonia_002EBase_002EConverter_0021Property();
		object? obj = staticResourceExtension.ProvideValue(context);
		context.ProvideTargetProperty = null;
		compiledBindingExtension4.Converter = (IValueConverter)obj;
		compiledBindingExtension4.ConverterParameter = "2.46";
		context.PopParent();
		context.ProvideTargetProperty = XamlIlHelpers.Avalonia_002EStyling_002ESetter_002CAvalonia_002EBase_002EValue_0021Property();
		CompiledBinding value2 = compiledBindingExtension3.ProvideValue(context);
		context.ProvideTargetProperty = null;
		setter5.Value = value2;
		context.PopParent();
		style2.Add(setter4);
		context.PopParent();
		styles.Add(item);
		Styles styles2 = dataGrid4.Styles;
		Style item2 = (style = new Style());
		context.PushParent(style);
		Style style3 = style;
		style3.Selector = ((Selector?)null).OfType(typeof(DataGridRow)).NthChild(2, 0);
		Setter setter6 = (setter2 = new Setter());
		context.PushParent(setter2);
		Setter setter7 = setter2;
		setter7.Property = TemplatedControl.BackgroundProperty;
		DynamicResourceExtension dynamicResourceExtension5 = new DynamicResourceExtension("DetailsListAlternatingRowBackgroundBrush");
		context.ProvideTargetProperty = XamlIlHelpers.Avalonia_002EStyling_002ESetter_002CAvalonia_002EBase_002EValue_0021Property();
		BindingBase value3 = dynamicResourceExtension5.ProvideValue(context);
		context.ProvideTargetProperty = null;
		setter7.Value = value3;
		context.PopParent();
		style3.Add(setter6);
		context.PopParent();
		styles2.Add(item2);
		Styles styles3 = dataGrid4.Styles;
		Style style4 = new Style();
		style4.Selector = ((Selector?)null).OfType(typeof(DataGridCell));
		Setter setter8 = new Setter();
		setter8.Property = ContentControl.HorizontalContentAlignmentProperty;
		setter8.Value = HorizontalAlignment.Stretch;
		style4.Add(setter8);
		styles3.Add(style4);
		Styles styles4 = dataGrid4.Styles;
		Style style5 = new Style();
		style5.Selector = ((Selector?)null).OfType(typeof(DataGridCell)).Class(":current").Template()
			.OfType(typeof(Grid))
			.Name("FocusVisual");
		Setter setter9 = new Setter();
		setter9.Property = Visual.IsVisibleProperty;
		setter9.Value = false;
		style5.Add(setter9);
		styles4.Add(style5);
		ObservableCollection<DataGridColumn> columns = dataGrid4.Columns;
		DataGridTemplateColumn dataGridTemplateColumn;
		DataGridTemplateColumn item3 = (dataGridTemplateColumn = new DataGridTemplateColumn());
		context.PushParent(dataGridTemplateColumn);
		DataGridTemplateColumn dataGridTemplateColumn2 = dataGridTemplateColumn;
		dataGridTemplateColumn2.Width = (DataGridLength)new DataGridLengthConverter().ConvertFrom(context, CultureInfo.InvariantCulture, "56");
		DataTemplate dataTemplate;
		DataTemplate cellTemplate = (dataTemplate = new DataTemplate());
		context.PushParent(dataTemplate);
		dataTemplate.Content = XamlIlRuntimeHelpers.DeferredTransformationFactoryV3<Control>((nint)(delegate*<IServiceProvider, object>)(&XamlClosure_6.Build_3), context);
		context.PopParent();
		dataGridTemplateColumn2.CellTemplate = cellTemplate;
		context.PopParent();
		columns.Add(item3);
		ObservableCollection<DataGridColumn> columns2 = dataGrid4.Columns;
		DataGridTemplateColumn item4 = (dataGridTemplateColumn = new DataGridTemplateColumn());
		context.PushParent(dataGridTemplateColumn);
		DataGridTemplateColumn dataGridTemplateColumn3 = dataGridTemplateColumn;
		CompiledBindingExtension compiledBindingExtension5 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EServices_002EApp_002ELocalizationBindingSource_002CXFolder_002EItem_005BName_005D_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build())
		{
			Source = LocalizationBindingSource.Instance
		};
		context.ProvideTargetProperty = DataGridColumn.HeaderProperty;
		CompiledBinding compiledBinding = compiledBindingExtension5.ProvideValue(context);
		context.ProvideTargetProperty = null;
		XamlDynamicSetters._003C_003EXamlDynamicSetter_7(dataGridTemplateColumn3, compiledBinding);
		dataGridTemplateColumn3.Width = (DataGridLength)new DataGridLengthConverter().ConvertFrom(context, CultureInfo.InvariantCulture, "2*");
		dataGridTemplateColumn3.CanUserResize = true;
		dataGridTemplateColumn3.SortMemberPath = "Name";
		DataTemplate cellTemplate2 = (dataTemplate = new DataTemplate());
		context.PushParent(dataTemplate);
		dataTemplate.Content = XamlIlRuntimeHelpers.DeferredTransformationFactoryV3<Control>((nint)(delegate*<IServiceProvider, object>)(&XamlClosure_6.Build_4), context);
		context.PopParent();
		dataGridTemplateColumn3.CellTemplate = cellTemplate2;
		context.PopParent();
		columns2.Add(item4);
		ObservableCollection<DataGridColumn> columns3 = dataGrid4.Columns;
		DataGridTemplateColumn item5 = (dataGridTemplateColumn = new DataGridTemplateColumn());
		context.PushParent(dataGridTemplateColumn);
		DataGridTemplateColumn dataGridTemplateColumn4 = dataGridTemplateColumn;
		CompiledBindingExtension compiledBindingExtension6 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EServices_002EApp_002ELocalizationBindingSource_002CXFolder_002EItem_005BSize_005D_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build())
		{
			Source = LocalizationBindingSource.Instance
		};
		context.ProvideTargetProperty = DataGridColumn.HeaderProperty;
		CompiledBinding compiledBinding2 = compiledBindingExtension6.ProvideValue(context);
		context.ProvideTargetProperty = null;
		XamlDynamicSetters._003C_003EXamlDynamicSetter_7(dataGridTemplateColumn4, compiledBinding2);
		dataGridTemplateColumn4.Width = (DataGridLength)new DataGridLengthConverter().ConvertFrom(context, CultureInfo.InvariantCulture, "*");
		dataGridTemplateColumn4.CanUserResize = true;
		dataGridTemplateColumn4.SortMemberPath = "FileSizeBytes";
		DataTemplate cellTemplate3 = (dataTemplate = new DataTemplate());
		context.PushParent(dataTemplate);
		dataTemplate.Content = XamlIlRuntimeHelpers.DeferredTransformationFactoryV3<Control>((nint)(delegate*<IServiceProvider, object>)(&XamlClosure_6.Build_5), context);
		context.PopParent();
		dataGridTemplateColumn4.CellTemplate = cellTemplate3;
		context.PopParent();
		columns3.Add(item5);
		ObservableCollection<DataGridColumn> columns4 = dataGrid4.Columns;
		DataGridTemplateColumn item6 = (dataGridTemplateColumn = new DataGridTemplateColumn());
		context.PushParent(dataGridTemplateColumn);
		DataGridTemplateColumn dataGridTemplateColumn5 = dataGridTemplateColumn;
		CompiledBindingExtension compiledBindingExtension7 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EServices_002EApp_002ELocalizationBindingSource_002CXFolder_002EItem_005BType_005D_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build())
		{
			Source = LocalizationBindingSource.Instance
		};
		context.ProvideTargetProperty = DataGridColumn.HeaderProperty;
		CompiledBinding compiledBinding3 = compiledBindingExtension7.ProvideValue(context);
		context.ProvideTargetProperty = null;
		XamlDynamicSetters._003C_003EXamlDynamicSetter_7(dataGridTemplateColumn5, compiledBinding3);
		dataGridTemplateColumn5.Width = (DataGridLength)new DataGridLengthConverter().ConvertFrom(context, CultureInfo.InvariantCulture, "*");
		dataGridTemplateColumn5.CanUserResize = true;
		dataGridTemplateColumn5.SortMemberPath = "DisplayItemType";
		DataTemplate cellTemplate4 = (dataTemplate = new DataTemplate());
		context.PushParent(dataTemplate);
		dataTemplate.Content = XamlIlRuntimeHelpers.DeferredTransformationFactoryV3<Control>((nint)(delegate*<IServiceProvider, object>)(&XamlClosure_6.Build_6), context);
		context.PopParent();
		dataGridTemplateColumn5.CellTemplate = cellTemplate4;
		context.PopParent();
		columns4.Add(item6);
		ObservableCollection<DataGridColumn> columns5 = dataGrid4.Columns;
		DataGridTemplateColumn item7 = (dataGridTemplateColumn = new DataGridTemplateColumn());
		context.PushParent(dataGridTemplateColumn);
		DataGridTemplateColumn dataGridTemplateColumn6 = dataGridTemplateColumn;
		CompiledBindingExtension compiledBindingExtension8 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EServices_002EApp_002ELocalizationBindingSource_002CXFolder_002EItem_005BPropertyDateModified_005D_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build())
		{
			Source = LocalizationBindingSource.Instance
		};
		context.ProvideTargetProperty = DataGridColumn.HeaderProperty;
		CompiledBinding compiledBinding4 = compiledBindingExtension8.ProvideValue(context);
		context.ProvideTargetProperty = null;
		XamlDynamicSetters._003C_003EXamlDynamicSetter_7(dataGridTemplateColumn6, compiledBinding4);
		dataGridTemplateColumn6.Width = (DataGridLength)new DataGridLengthConverter().ConvertFrom(context, CultureInfo.InvariantCulture, "1.5*");
		dataGridTemplateColumn6.CanUserResize = true;
		dataGridTemplateColumn6.SortMemberPath = "ItemDateModified";
		DataTemplate cellTemplate5 = (dataTemplate = new DataTemplate());
		context.PushParent(dataTemplate);
		dataTemplate.Content = XamlIlRuntimeHelpers.DeferredTransformationFactoryV3<Control>((nint)(delegate*<IServiceProvider, object>)(&XamlClosure_6.Build_7), context);
		context.PopParent();
		dataGridTemplateColumn6.CellTemplate = cellTemplate5;
		context.PopParent();
		columns5.Add(item7);
		context.PopParent();
		((ISupportInitialize)dataGrid3).EndInit();
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
	private static void _0021XamlIlPopulateTrampoline(DetailsLayoutView P_0)
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
