using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
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
using CommunityToolkit.Mvvm.DependencyInjection;
using CompiledAvaloniaXaml;
using Files.Converters;
using Files.Data.Contexts;
using Files.Data.Contexts.ContentPage;
using Files.Data.Enums.Navigation;
using Files.Data.Items;
using Files.Services.App;
using Files.ViewModels.Shells;

namespace Files.UserControls.Sidebar;

public class SidebarView : UserControl
{
	[CompilerGenerated]
	private class XamlClosure_5
	{
		public static object Build_1(IServiceProvider P_0)
		{
			XamlIlContext.Context<SidebarView> context = CreateContext(P_0);
			return new FileIconConverter();
		}

		public static XamlIlContext.Context<SidebarView> CreateContext(IServiceProvider P_0)
		{
			XamlIlContext.Context<SidebarView> context = new XamlIlContext.Context<SidebarView>(P_0, new object[1] { _0021AvaloniaResources.NamespaceInfo_003A_002FUserControls_002FSidebar_002FSidebarView_002Eaxaml.Singleton }, "avares://XFolder/UserControls/Sidebar/SidebarView.axaml");
			if (P_0 != null)
			{
				object service = P_0.GetService(typeof(IRootObjectProvider));
				if (service != null)
				{
					service = ((IRootObjectProvider)service).RootObject;
					context.RootObject = (SidebarView)service;
				}
			}
			return context;
		}

		public static object Build_2(IServiceProvider P_0)
		{
			XamlIlContext.Context<SidebarView> context = CreateContext(P_0);
			return new FontSizeToIconSizeConverter();
		}

		public static object Build_3(IServiceProvider P_0)
		{
			XamlIlContext.Context<SidebarView> context = CreateContext(P_0);
			context.IntermediateRoot = new StackPanel();
			object obj = context.IntermediateRoot;
			((ISupportInitialize)obj).BeginInit();
			StackPanel stackPanel = (StackPanel)obj;
			context.PushParent(stackPanel);
			stackPanel.Orientation = Orientation.Horizontal;
			StyledProperty<double> opacityProperty = Visual.OpacityProperty;
			CompiledBindingExtension compiledBindingExtension = new CompiledBindingExtension(new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EData_002EItems_002EFileTreeItem_002CXFolder_002EOpacity_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build());
			context.ProvideTargetProperty = Visual.OpacityProperty;
			CompiledBinding binding = compiledBindingExtension.ProvideValue(context);
			context.ProvideTargetProperty = null;
			stackPanel.Bind(opacityProperty, binding);
			Controls children = stackPanel.Children;
			Image image2;
			Image image = (image2 = new Image());
			((ISupportInitialize)image).BeginInit();
			children.Add(image);
			Image image4;
			Image image3 = (image4 = image2);
			context.PushParent(image4);
			image4.Margin = new Thickness(0.0, 0.0, 6.0, 0.0);
			image4.VerticalAlignment = VerticalAlignment.Center;
			StyledProperty<double> widthProperty = Layoutable.WidthProperty;
			CompiledBindingExtension compiledBindingExtension3;
			CompiledBindingExtension compiledBindingExtension2 = (compiledBindingExtension3 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Ancestor(typeof(TreeViewItem), 0).Property(TemplatedControl.FontSizeProperty, PropertyInfoAccessorFactory.CreateAvaloniaPropertyAccessor).Build()));
			context.PushParent(compiledBindingExtension3);
			CompiledBindingExtension compiledBindingExtension4 = compiledBindingExtension3;
			StaticResourceExtension staticResourceExtension = new StaticResourceExtension("FontSizeToIconSizeConverter");
			context.ProvideTargetProperty = XamlIlHelpers.Avalonia_002EData_002ECompiledBinding_002CAvalonia_002EBase_002EConverter_0021Property();
			object? obj2 = staticResourceExtension.ProvideValue(context);
			context.ProvideTargetProperty = null;
			compiledBindingExtension4.Converter = (IValueConverter)obj2;
			context.PopParent();
			context.ProvideTargetProperty = Layoutable.WidthProperty;
			CompiledBinding binding2 = compiledBindingExtension2.ProvideValue(context);
			context.ProvideTargetProperty = null;
			image4.Bind(widthProperty, binding2);
			StyledProperty<double> heightProperty = Layoutable.HeightProperty;
			CompiledBindingExtension compiledBindingExtension5 = (compiledBindingExtension3 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Ancestor(typeof(TreeViewItem), 0).Property(TemplatedControl.FontSizeProperty, PropertyInfoAccessorFactory.CreateAvaloniaPropertyAccessor).Build()));
			context.PushParent(compiledBindingExtension3);
			CompiledBindingExtension compiledBindingExtension6 = compiledBindingExtension3;
			StaticResourceExtension staticResourceExtension2 = new StaticResourceExtension("FontSizeToIconSizeConverter");
			context.ProvideTargetProperty = XamlIlHelpers.Avalonia_002EData_002ECompiledBinding_002CAvalonia_002EBase_002EConverter_0021Property();
			object? obj3 = staticResourceExtension2.ProvideValue(context);
			context.ProvideTargetProperty = null;
			compiledBindingExtension6.Converter = (IValueConverter)obj3;
			context.PopParent();
			context.ProvideTargetProperty = Layoutable.HeightProperty;
			CompiledBinding binding3 = compiledBindingExtension5.ProvideValue(context);
			context.ProvideTargetProperty = null;
			image4.Bind(heightProperty, binding3);
			StyledProperty<bool> isVisibleProperty = Visual.IsVisibleProperty;
			CompiledBindingExtension compiledBindingExtension7 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EData_002EItems_002EFileTreeItem_002CXFolder_002EPath_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build())
			{
				Converter = StringConverters.IsNotNullOrEmpty
			};
			context.ProvideTargetProperty = Visual.IsVisibleProperty;
			CompiledBinding binding4 = compiledBindingExtension7.ProvideValue(context);
			context.ProvideTargetProperty = null;
			image4.Bind(isVisibleProperty, binding4);
			StyledProperty<IImage?> sourceProperty = Image.SourceProperty;
			MultiBinding multiBinding;
			MultiBinding binding5 = (multiBinding = new MultiBinding());
			context.PushParent(multiBinding);
			multiBinding.Converter = SidebarViewModel.QuickAccessIconConverterInstance;
			IList<BindingBase> bindings = multiBinding.Bindings;
			CompiledBindingExtension compiledBindingExtension8 = new CompiledBindingExtension
			{
				Path = new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EData_002EItems_002EFileTreeItem_002CXFolder_002EIsDirectory_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build()
			};
			context.ProvideTargetProperty = XamlIlHelpers.Avalonia_002EData_002EMultiBinding_002CAvalonia_002EBase_002EBindings_0021Property();
			CompiledBinding item = compiledBindingExtension8.ProvideValue(context);
			context.ProvideTargetProperty = null;
			bindings.Add(item);
			IList<BindingBase> bindings2 = multiBinding.Bindings;
			CompiledBindingExtension compiledBindingExtension9 = new CompiledBindingExtension
			{
				Path = new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EData_002EItems_002EFileTreeItem_002CXFolder_002EPath_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build()
			};
			context.ProvideTargetProperty = XamlIlHelpers.Avalonia_002EData_002EMultiBinding_002CAvalonia_002EBase_002EBindings_0021Property();
			CompiledBinding item2 = compiledBindingExtension9.ProvideValue(context);
			context.ProvideTargetProperty = null;
			bindings2.Add(item2);
			context.PopParent();
			image4.Bind(sourceProperty, binding5);
			context.PopParent();
			((ISupportInitialize)image3).EndInit();
			Controls children2 = stackPanel.Children;
			TextBlock textBlock2;
			TextBlock textBlock = (textBlock2 = new TextBlock());
			((ISupportInitialize)textBlock).BeginInit();
			children2.Add(textBlock);
			TextBlock textBlock4;
			TextBlock textBlock3 = (textBlock4 = textBlock2);
			context.PushParent(textBlock4);
			StyledProperty<string?> textProperty = TextBlock.TextProperty;
			CompiledBindingExtension compiledBindingExtension10 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EData_002EItems_002EFileTreeItem_002CXFolder_002EName_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build());
			context.ProvideTargetProperty = TextBlock.TextProperty;
			CompiledBinding binding6 = compiledBindingExtension10.ProvideValue(context);
			context.ProvideTargetProperty = null;
			textBlock4.Bind(textProperty, binding6);
			textBlock4.VerticalAlignment = VerticalAlignment.Center;
			context.PopParent();
			((ISupportInitialize)textBlock3).EndInit();
			context.PopParent();
			((ISupportInitialize)obj).EndInit();
			return obj;
		}

		public static object Build_4(IServiceProvider P_0)
		{
			XamlIlContext.Context<SidebarView> context = CreateContext(P_0);
			context.IntermediateRoot = new Grid();
			object obj = context.IntermediateRoot;
			((ISupportInitialize)obj).BeginInit();
			Grid grid = (Grid)obj;
			context.PushParent(grid);
			ColumnDefinitions columnDefinitions = new ColumnDefinitions();
			columnDefinitions.Capacity = 2;
			columnDefinitions.Add(new ColumnDefinition(new GridLength(1.0, GridUnitType.Star)));
			columnDefinitions.Add(new ColumnDefinition(new GridLength(0.0, GridUnitType.Auto)));
			grid.ColumnDefinitions = columnDefinitions;
			TextBlock textBlock2;
			TextBlock textBlock = (textBlock2 = new TextBlock());
			((ISupportInitialize)textBlock).BeginInit();
			ToolTip.SetTip(grid, textBlock);
			TextBlock textBlock4;
			TextBlock textBlock3 = (textBlock4 = textBlock2);
			context.PushParent(textBlock4);
			TextBlock textBlock5 = textBlock4;
			StyledProperty<string?> textProperty = TextBlock.TextProperty;
			CompiledBindingExtension compiledBindingExtension = new CompiledBindingExtension(new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EData_002EItems_002ERemovableDriveTreeItem_002CXFolder_002EPath_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build());
			context.ProvideTargetProperty = TextBlock.TextProperty;
			CompiledBinding binding = compiledBindingExtension.ProvideValue(context);
			context.ProvideTargetProperty = null;
			textBlock5.Bind(textProperty, binding);
			context.PopParent();
			((ISupportInitialize)textBlock3).EndInit();
			Controls children = grid.Children;
			StackPanel stackPanel2;
			StackPanel stackPanel = (stackPanel2 = new StackPanel());
			((ISupportInitialize)stackPanel).BeginInit();
			children.Add(stackPanel);
			StackPanel stackPanel4;
			StackPanel stackPanel3 = (stackPanel4 = stackPanel2);
			context.PushParent(stackPanel4);
			Grid.SetColumn(stackPanel4, 0);
			stackPanel4.Orientation = Orientation.Horizontal;
			Controls children2 = stackPanel4.Children;
			Image image2;
			Image image = (image2 = new Image());
			((ISupportInitialize)image).BeginInit();
			children2.Add(image);
			Image image4;
			Image image3 = (image4 = image2);
			context.PushParent(image4);
			image4.Margin = new Thickness(0.0, 0.0, 6.0, 0.0);
			image4.VerticalAlignment = VerticalAlignment.Center;
			StyledProperty<double> widthProperty = Layoutable.WidthProperty;
			CompiledBindingExtension compiledBindingExtension3;
			CompiledBindingExtension compiledBindingExtension2 = (compiledBindingExtension3 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Ancestor(typeof(TreeViewItem), 0).Property(TemplatedControl.FontSizeProperty, PropertyInfoAccessorFactory.CreateAvaloniaPropertyAccessor).Build()));
			context.PushParent(compiledBindingExtension3);
			CompiledBindingExtension compiledBindingExtension4 = compiledBindingExtension3;
			StaticResourceExtension staticResourceExtension = new StaticResourceExtension("FontSizeToIconSizeConverter");
			context.ProvideTargetProperty = XamlIlHelpers.Avalonia_002EData_002ECompiledBinding_002CAvalonia_002EBase_002EConverter_0021Property();
			object? obj2 = staticResourceExtension.ProvideValue(context);
			context.ProvideTargetProperty = null;
			compiledBindingExtension4.Converter = (IValueConverter)obj2;
			context.PopParent();
			context.ProvideTargetProperty = Layoutable.WidthProperty;
			CompiledBinding binding2 = compiledBindingExtension2.ProvideValue(context);
			context.ProvideTargetProperty = null;
			image4.Bind(widthProperty, binding2);
			StyledProperty<double> heightProperty = Layoutable.HeightProperty;
			CompiledBindingExtension compiledBindingExtension5 = (compiledBindingExtension3 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Ancestor(typeof(TreeViewItem), 0).Property(TemplatedControl.FontSizeProperty, PropertyInfoAccessorFactory.CreateAvaloniaPropertyAccessor).Build()));
			context.PushParent(compiledBindingExtension3);
			CompiledBindingExtension compiledBindingExtension6 = compiledBindingExtension3;
			StaticResourceExtension staticResourceExtension2 = new StaticResourceExtension("FontSizeToIconSizeConverter");
			context.ProvideTargetProperty = XamlIlHelpers.Avalonia_002EData_002ECompiledBinding_002CAvalonia_002EBase_002EConverter_0021Property();
			object? obj3 = staticResourceExtension2.ProvideValue(context);
			context.ProvideTargetProperty = null;
			compiledBindingExtension6.Converter = (IValueConverter)obj3;
			context.PopParent();
			context.ProvideTargetProperty = Layoutable.HeightProperty;
			CompiledBinding binding3 = compiledBindingExtension5.ProvideValue(context);
			context.ProvideTargetProperty = null;
			image4.Bind(heightProperty, binding3);
			StyledProperty<bool> isVisibleProperty = Visual.IsVisibleProperty;
			CompiledBindingExtension compiledBindingExtension7 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EData_002EItems_002ERemovableDriveTreeItem_002CXFolder_002EPath_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build())
			{
				Converter = StringConverters.IsNotNullOrEmpty
			};
			context.ProvideTargetProperty = Visual.IsVisibleProperty;
			CompiledBinding binding4 = compiledBindingExtension7.ProvideValue(context);
			context.ProvideTargetProperty = null;
			image4.Bind(isVisibleProperty, binding4);
			StyledProperty<IImage?> sourceProperty = Image.SourceProperty;
			MultiBinding multiBinding;
			MultiBinding binding5 = (multiBinding = new MultiBinding());
			context.PushParent(multiBinding);
			multiBinding.Converter = SidebarViewModel.QuickAccessIconConverterInstance;
			IList<BindingBase> bindings = multiBinding.Bindings;
			CompiledBindingExtension compiledBindingExtension8 = new CompiledBindingExtension
			{
				Path = new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EData_002EItems_002ERemovableDriveTreeItem_002CXFolder_002EHasDrive_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build()
			};
			context.ProvideTargetProperty = XamlIlHelpers.Avalonia_002EData_002EMultiBinding_002CAvalonia_002EBase_002EBindings_0021Property();
			CompiledBinding item = compiledBindingExtension8.ProvideValue(context);
			context.ProvideTargetProperty = null;
			bindings.Add(item);
			IList<BindingBase> bindings2 = multiBinding.Bindings;
			CompiledBindingExtension compiledBindingExtension9 = new CompiledBindingExtension
			{
				Path = new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EData_002EItems_002ERemovableDriveTreeItem_002CXFolder_002EPath_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build()
			};
			context.ProvideTargetProperty = XamlIlHelpers.Avalonia_002EData_002EMultiBinding_002CAvalonia_002EBase_002EBindings_0021Property();
			CompiledBinding item2 = compiledBindingExtension9.ProvideValue(context);
			context.ProvideTargetProperty = null;
			bindings2.Add(item2);
			context.PopParent();
			image4.Bind(sourceProperty, binding5);
			context.PopParent();
			((ISupportInitialize)image3).EndInit();
			Controls children3 = stackPanel4.Children;
			TextBlock textBlock7;
			TextBlock textBlock6 = (textBlock7 = new TextBlock());
			((ISupportInitialize)textBlock6).BeginInit();
			children3.Add(textBlock6);
			TextBlock textBlock8 = (textBlock4 = textBlock7);
			context.PushParent(textBlock4);
			TextBlock textBlock9 = textBlock4;
			StyledProperty<string?> textProperty2 = TextBlock.TextProperty;
			CompiledBindingExtension compiledBindingExtension10 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EData_002EItems_002ERemovableDriveTreeItem_002CXFolder_002EName_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build());
			context.ProvideTargetProperty = TextBlock.TextProperty;
			CompiledBinding binding6 = compiledBindingExtension10.ProvideValue(context);
			context.ProvideTargetProperty = null;
			textBlock9.Bind(textProperty2, binding6);
			textBlock9.VerticalAlignment = VerticalAlignment.Center;
			context.PopParent();
			((ISupportInitialize)textBlock8).EndInit();
			context.PopParent();
			((ISupportInitialize)stackPanel3).EndInit();
			Controls children4 = grid.Children;
			Button button2;
			Button button = (button2 = new Button());
			((ISupportInitialize)button).BeginInit();
			children4.Add(button);
			Button button4;
			Button button3 = (button4 = button2);
			context.PushParent(button4);
			button4.Name = "EjectDriveButton";
			object element = button4;
			context.AvaloniaNameScope.Register("EjectDriveButton", element);
			Grid.SetColumn(button4, 1);
			button4.Margin = new Thickness(15.0, 0.0, 15.0, 0.0);
			button4.Padding = new Thickness(4.0, 4.0, 4.0, 4.0);
			button4.MinWidth = 0.0;
			button4.MinHeight = 0.0;
			button4.VerticalAlignment = VerticalAlignment.Center;
			button4.Background = new ImmutableSolidColorBrush(16777215u);
			button4.BorderBrush = new ImmutableSolidColorBrush(16777215u);
			StyledProperty<bool> isVisibleProperty2 = Visual.IsVisibleProperty;
			CompiledBindingExtension compiledBindingExtension11 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EData_002EItems_002ERemovableDriveTreeItem_002CXFolder_002EHasDrive_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build());
			context.ProvideTargetProperty = Visual.IsVisibleProperty;
			CompiledBinding binding7 = compiledBindingExtension11.ProvideValue(context);
			context.ProvideTargetProperty = null;
			button4.Bind(isVisibleProperty2, binding7);
			button4.AddHandler((RoutedEvent)Button.ClickEvent, (Delegate)new EventHandler<RoutedEventArgs>(context.RootObject.RemovableDriveEjectButton_OnClick), RoutingStrategies.Direct | RoutingStrategies.Bubble, false);
			TextBlock textBlock11;
			TextBlock textBlock10 = (textBlock11 = new TextBlock());
			((ISupportInitialize)textBlock10).BeginInit();
			ToolTip.SetTip(button4, textBlock10);
			TextBlock textBlock12 = (textBlock4 = textBlock11);
			context.PushParent(textBlock4);
			TextBlock textBlock13 = textBlock4;
			StyledProperty<string?> textProperty3 = TextBlock.TextProperty;
			CompiledBindingExtension compiledBindingExtension12 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EServices_002EApp_002ELocalizationBindingSource_002CXFolder_002EItem_005BEject_005D_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build())
			{
				Source = LocalizationBindingSource.Instance
			};
			context.ProvideTargetProperty = TextBlock.TextProperty;
			CompiledBinding binding8 = compiledBindingExtension12.ProvideValue(context);
			context.ProvideTargetProperty = null;
			textBlock13.Bind(textProperty3, binding8);
			context.PopParent();
			((ISupportInitialize)textBlock12).EndInit();
			PathIcon pathIcon2;
			PathIcon pathIcon = (pathIcon2 = new PathIcon());
			((ISupportInitialize)pathIcon).BeginInit();
			button4.Content = pathIcon;
			PathIcon pathIcon4;
			PathIcon pathIcon3 = (pathIcon4 = pathIcon2);
			context.PushParent(pathIcon4);
			StyledProperty<Geometry?> dataProperty = PathIcon.DataProperty;
			DynamicResourceExtension dynamicResourceExtension = new DynamicResourceExtension("eject_regular");
			context.ProvideTargetProperty = PathIcon.DataProperty;
			BindingBase binding9 = dynamicResourceExtension.ProvideValue(context);
			context.ProvideTargetProperty = null;
			pathIcon4.Bind(dataProperty, binding9);
			StyledProperty<IBrush?> foregroundProperty = TemplatedControl.ForegroundProperty;
			DynamicResourceExtension dynamicResourceExtension2 = new DynamicResourceExtension("SystemControlForegroundBaseMediumBrush");
			context.ProvideTargetProperty = TemplatedControl.ForegroundProperty;
			BindingBase binding10 = dynamicResourceExtension2.ProvideValue(context);
			context.ProvideTargetProperty = null;
			pathIcon4.Bind(foregroundProperty, binding10);
			StyledProperty<double> widthProperty2 = Layoutable.WidthProperty;
			CompiledBindingExtension compiledBindingExtension13 = (compiledBindingExtension3 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Ancestor(typeof(TreeViewItem), 0).Property(TemplatedControl.FontSizeProperty, PropertyInfoAccessorFactory.CreateAvaloniaPropertyAccessor).Build()));
			context.PushParent(compiledBindingExtension3);
			CompiledBindingExtension compiledBindingExtension14 = compiledBindingExtension3;
			StaticResourceExtension staticResourceExtension3 = new StaticResourceExtension("FontSizeToIconSizeConverter");
			context.ProvideTargetProperty = XamlIlHelpers.Avalonia_002EData_002ECompiledBinding_002CAvalonia_002EBase_002EConverter_0021Property();
			object? obj4 = staticResourceExtension3.ProvideValue(context);
			context.ProvideTargetProperty = null;
			compiledBindingExtension14.Converter = (IValueConverter)obj4;
			compiledBindingExtension14.ConverterParameter = "0.77";
			context.PopParent();
			context.ProvideTargetProperty = Layoutable.WidthProperty;
			CompiledBinding binding11 = compiledBindingExtension13.ProvideValue(context);
			context.ProvideTargetProperty = null;
			pathIcon4.Bind(widthProperty2, binding11);
			StyledProperty<double> heightProperty2 = Layoutable.HeightProperty;
			CompiledBindingExtension compiledBindingExtension15 = (compiledBindingExtension3 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Ancestor(typeof(TreeViewItem), 0).Property(TemplatedControl.FontSizeProperty, PropertyInfoAccessorFactory.CreateAvaloniaPropertyAccessor).Build()));
			context.PushParent(compiledBindingExtension3);
			CompiledBindingExtension compiledBindingExtension16 = compiledBindingExtension3;
			StaticResourceExtension staticResourceExtension4 = new StaticResourceExtension("FontSizeToIconSizeConverter");
			context.ProvideTargetProperty = XamlIlHelpers.Avalonia_002EData_002ECompiledBinding_002CAvalonia_002EBase_002EConverter_0021Property();
			object? obj5 = staticResourceExtension4.ProvideValue(context);
			context.ProvideTargetProperty = null;
			compiledBindingExtension16.Converter = (IValueConverter)obj5;
			compiledBindingExtension16.ConverterParameter = "0.77";
			context.PopParent();
			context.ProvideTargetProperty = Layoutable.HeightProperty;
			CompiledBinding binding12 = compiledBindingExtension15.ProvideValue(context);
			context.ProvideTargetProperty = null;
			pathIcon4.Bind(heightProperty2, binding12);
			context.PopParent();
			((ISupportInitialize)pathIcon3).EndInit();
			context.PopParent();
			((ISupportInitialize)button3).EndInit();
			context.PopParent();
			((ISupportInitialize)obj).EndInit();
			return obj;
		}

		public static object Build_5(IServiceProvider P_0)
		{
			XamlIlContext.Context<SidebarView> context = CreateContext(P_0);
			context.IntermediateRoot = new StackPanel();
			object obj = context.IntermediateRoot;
			((ISupportInitialize)obj).BeginInit();
			StackPanel stackPanel = (StackPanel)obj;
			context.PushParent(stackPanel);
			stackPanel.Orientation = Orientation.Horizontal;
			StyledProperty<double> opacityProperty = Visual.OpacityProperty;
			CompiledBindingExtension compiledBindingExtension = new CompiledBindingExtension(new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EData_002EItems_002EFileTreeItem_002CXFolder_002EOpacity_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build());
			context.ProvideTargetProperty = Visual.OpacityProperty;
			CompiledBinding binding = compiledBindingExtension.ProvideValue(context);
			context.ProvideTargetProperty = null;
			stackPanel.Bind(opacityProperty, binding);
			TextBlock textBlock2;
			TextBlock textBlock = (textBlock2 = new TextBlock());
			((ISupportInitialize)textBlock).BeginInit();
			ToolTip.SetTip(stackPanel, textBlock);
			TextBlock textBlock4;
			TextBlock textBlock3 = (textBlock4 = textBlock2);
			context.PushParent(textBlock4);
			TextBlock textBlock5 = textBlock4;
			StyledProperty<string?> textProperty = TextBlock.TextProperty;
			CompiledBindingExtension compiledBindingExtension2 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EData_002EItems_002EFileTreeItem_002CXFolder_002EPath_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build());
			context.ProvideTargetProperty = TextBlock.TextProperty;
			CompiledBinding binding2 = compiledBindingExtension2.ProvideValue(context);
			context.ProvideTargetProperty = null;
			textBlock5.Bind(textProperty, binding2);
			context.PopParent();
			((ISupportInitialize)textBlock3).EndInit();
			Controls children = stackPanel.Children;
			Image image2;
			Image image = (image2 = new Image());
			((ISupportInitialize)image).BeginInit();
			children.Add(image);
			Image image4;
			Image image3 = (image4 = image2);
			context.PushParent(image4);
			image4.Margin = new Thickness(0.0, 0.0, 6.0, 0.0);
			image4.VerticalAlignment = VerticalAlignment.Center;
			StyledProperty<double> widthProperty = Layoutable.WidthProperty;
			CompiledBindingExtension compiledBindingExtension4;
			CompiledBindingExtension compiledBindingExtension3 = (compiledBindingExtension4 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Ancestor(typeof(TreeViewItem), 0).Property(TemplatedControl.FontSizeProperty, PropertyInfoAccessorFactory.CreateAvaloniaPropertyAccessor).Build()));
			context.PushParent(compiledBindingExtension4);
			CompiledBindingExtension compiledBindingExtension5 = compiledBindingExtension4;
			StaticResourceExtension staticResourceExtension = new StaticResourceExtension("FontSizeToIconSizeConverter");
			context.ProvideTargetProperty = XamlIlHelpers.Avalonia_002EData_002ECompiledBinding_002CAvalonia_002EBase_002EConverter_0021Property();
			object? obj2 = staticResourceExtension.ProvideValue(context);
			context.ProvideTargetProperty = null;
			compiledBindingExtension5.Converter = (IValueConverter)obj2;
			context.PopParent();
			context.ProvideTargetProperty = Layoutable.WidthProperty;
			CompiledBinding binding3 = compiledBindingExtension3.ProvideValue(context);
			context.ProvideTargetProperty = null;
			image4.Bind(widthProperty, binding3);
			StyledProperty<double> heightProperty = Layoutable.HeightProperty;
			CompiledBindingExtension compiledBindingExtension6 = (compiledBindingExtension4 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Ancestor(typeof(TreeViewItem), 0).Property(TemplatedControl.FontSizeProperty, PropertyInfoAccessorFactory.CreateAvaloniaPropertyAccessor).Build()));
			context.PushParent(compiledBindingExtension4);
			CompiledBindingExtension compiledBindingExtension7 = compiledBindingExtension4;
			StaticResourceExtension staticResourceExtension2 = new StaticResourceExtension("FontSizeToIconSizeConverter");
			context.ProvideTargetProperty = XamlIlHelpers.Avalonia_002EData_002ECompiledBinding_002CAvalonia_002EBase_002EConverter_0021Property();
			object? obj3 = staticResourceExtension2.ProvideValue(context);
			context.ProvideTargetProperty = null;
			compiledBindingExtension7.Converter = (IValueConverter)obj3;
			context.PopParent();
			context.ProvideTargetProperty = Layoutable.HeightProperty;
			CompiledBinding binding4 = compiledBindingExtension6.ProvideValue(context);
			context.ProvideTargetProperty = null;
			image4.Bind(heightProperty, binding4);
			StyledProperty<IImage?> sourceProperty = Image.SourceProperty;
			MultiBinding multiBinding;
			MultiBinding binding5 = (multiBinding = new MultiBinding());
			context.PushParent(multiBinding);
			StaticResourceExtension staticResourceExtension3 = new StaticResourceExtension("FileIconConverter");
			context.ProvideTargetProperty = XamlIlHelpers.Avalonia_002EData_002EMultiBinding_002CAvalonia_002EBase_002EConverter_0021Property();
			object? obj4 = staticResourceExtension3.ProvideValue(context);
			context.ProvideTargetProperty = null;
			multiBinding.Converter = (IMultiValueConverter)obj4;
			IList<BindingBase> bindings = multiBinding.Bindings;
			CompiledBindingExtension compiledBindingExtension8 = new CompiledBindingExtension
			{
				Path = new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EData_002EItems_002EFileTreeItem_002CXFolder_002EIsDirectory_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build()
			};
			context.ProvideTargetProperty = XamlIlHelpers.Avalonia_002EData_002EMultiBinding_002CAvalonia_002EBase_002EBindings_0021Property();
			CompiledBinding item = compiledBindingExtension8.ProvideValue(context);
			context.ProvideTargetProperty = null;
			bindings.Add(item);
			IList<BindingBase> bindings2 = multiBinding.Bindings;
			CompiledBindingExtension compiledBindingExtension9 = new CompiledBindingExtension
			{
				Path = new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EData_002EItems_002EFileTreeItem_002CXFolder_002EIsExpanded_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build()
			};
			context.ProvideTargetProperty = XamlIlHelpers.Avalonia_002EData_002EMultiBinding_002CAvalonia_002EBase_002EBindings_0021Property();
			CompiledBinding item2 = compiledBindingExtension9.ProvideValue(context);
			context.ProvideTargetProperty = null;
			bindings2.Add(item2);
			IList<BindingBase> bindings3 = multiBinding.Bindings;
			CompiledBindingExtension compiledBindingExtension10 = new CompiledBindingExtension
			{
				Path = new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EData_002EItems_002EFileTreeItem_002CXFolder_002EPath_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build()
			};
			context.ProvideTargetProperty = XamlIlHelpers.Avalonia_002EData_002EMultiBinding_002CAvalonia_002EBase_002EBindings_0021Property();
			CompiledBinding item3 = compiledBindingExtension10.ProvideValue(context);
			context.ProvideTargetProperty = null;
			bindings3.Add(item3);
			context.PopParent();
			image4.Bind(sourceProperty, binding5);
			context.PopParent();
			((ISupportInitialize)image3).EndInit();
			Controls children2 = stackPanel.Children;
			TextBlock textBlock7;
			TextBlock textBlock6 = (textBlock7 = new TextBlock());
			((ISupportInitialize)textBlock6).BeginInit();
			children2.Add(textBlock6);
			TextBlock textBlock8 = (textBlock4 = textBlock7);
			context.PushParent(textBlock4);
			TextBlock textBlock9 = textBlock4;
			StyledProperty<string?> textProperty2 = TextBlock.TextProperty;
			CompiledBindingExtension compiledBindingExtension11 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EData_002EItems_002EFileTreeItem_002CXFolder_002EName_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build())
			{
				Converter = FileDisplayNameConverter.Instance
			};
			context.ProvideTargetProperty = TextBlock.TextProperty;
			CompiledBinding binding6 = compiledBindingExtension11.ProvideValue(context);
			context.ProvideTargetProperty = null;
			textBlock9.Bind(textProperty2, binding6);
			textBlock9.VerticalAlignment = VerticalAlignment.Center;
			context.PopParent();
			((ISupportInitialize)textBlock8).EndInit();
			context.PopParent();
			((ISupportInitialize)obj).EndInit();
			return obj;
		}
	}

	private readonly IContentPageContext contentPageContext;

	private readonly IMultiPanesContext multiPanesContext;

	private SidebarViewModel _viewModel = new SidebarViewModel();

	private string _searchText = string.Empty;

	private string _serachTextRawInput = string.Empty;

	private long _lastSearchTimeTick = DateTime.Now.Ticks;

	[GeneratedCode("Avalonia.Generators.NameGenerator.InitializeComponentCodeGenerator", "12.0.1.0")]
	internal TreeView quickAccessTreeView;

	[GeneratedCode("Avalonia.Generators.NameGenerator.InitializeComponentCodeGenerator", "12.0.1.0")]
	internal TreeView removableTreeView;

	[GeneratedCode("Avalonia.Generators.NameGenerator.InitializeComponentCodeGenerator", "12.0.1.0")]
	internal Button EjectDriveButton;

	[GeneratedCode("Avalonia.Generators.NameGenerator.InitializeComponentCodeGenerator", "12.0.1.0")]
	internal TreeView treeView;

	[CompilerGenerated]
	private static Action<object> _0021XamlIlPopulateOverride;

	public SidebarView()
	{
		InitializeComponent();
		if (!Design.IsDesignMode)
		{
			contentPageContext = Ioc.Default.GetRequiredService<IContentPageContext>();
			multiPanesContext = Ioc.Default.GetRequiredService<IMultiPanesContext>();
			DataContext = _viewModel;
			if (multiPanesContext != null)
			{
				multiPanesContext.SidebarView = this;
				multiPanesContext.ActivePaneChanged += Context_Changed;
			}
			AddHandler(InputElement.KeyDownEvent, OnSidebarKeyDown, RoutingStrategies.Bubble, handledEventsToo: true);
		}
	}

	private void Context_Changed(object? sender, EventArgs e)
	{
		if (multiPanesContext.ActivePaneViewModel is ModernShellViewModel modernShellViewModel)
		{
			RequestOpenDirectory(modernShellViewModel.WorkingDirectory);
		}
	}

	private void TreeView_OnTapped(object? sender, TappedEventArgs e)
	{
		if (e.Source is StyledElement { DataContext: FileTreeItem dataContext })
		{
			if (!IsExpandChevronTapped(e.Source))
			{
				RequestOpenDirectory(dataContext.Path);
			}
			FocusTree(treeView);
		}
	}

	private static void FocusTree(TreeView view)
	{
		// Navigation reloads the directory and may restore selection programmatically. Focus the
		// tree after that settles so Up/Down keep navigating the tree instead of the file list.
		Dispatcher.UIThread.Post(delegate
		{
			view.Focus();
		}, DispatcherPriority.Input);
	}

	private static bool IsExpandChevronTapped(object? source)
	{
		return IsNamedElementTapped(source, "PART_ExpandCollapseChevronContainer");
	}

	private static bool IsNamedElementTapped(object? source, string elementName)
	{
		Visual visual = source as Visual;
		if (visual == null)
		{
			return false;
		}
		while (visual != null)
		{
			StyledElement styledElement = visual;
			if (styledElement != null && styledElement.Name == elementName)
			{
				return true;
			}
			visual = visual.GetVisualParent();
		}
		return false;
	}

	private void QuickAccess_OnTapped(object? sender, TappedEventArgs e)
	{
		if (e.Source is StyledElement { DataContext: FileTreeItem dataContext } && !string.IsNullOrEmpty(dataContext.Path))
		{
			RequestOpenDirectory(dataContext.Path, isInit: false, ensureVisible: false);
			FocusTree(quickAccessTreeView);
		}
	}

	private void RemovableTreeView_OnTapped(object? sender, TappedEventArgs e)
	{
		if (!IsExpandChevronTapped(e.Source) && !IsNamedElementTapped(e.Source, "EjectDriveButton") && e.Source is StyledElement { DataContext: RemovableDriveTreeItem { HasDrive: not false } dataContext } && !string.IsNullOrEmpty(dataContext.Path))
		{
			RequestOpenDirectory(dataContext.Path, isInit: false, ensureVisible: false);
			FocusTree(removableTreeView);
		}
	}

	private void RemovableDriveEjectButton_OnClick(object? sender, RoutedEventArgs e)
	{
		if (sender is StyledElement { DataContext: RemovableDriveTreeItem dataContext })
		{
			_viewModel.EjectRemovableDrive(dataContext);
		}
		e.Handled = true;
	}

	public void RequestOpenDirectory(string path, bool isInit = false, bool ensureVisible = true)
	{
		if (!(path != _viewModel.SelectedPath))
		{
			return;
		}
		FileTreeItem fileTreeItem = _viewModel.OpenDirectory(path);
		if (fileTreeItem != null)
		{
			treeView.SelectedItem = null;
			if (ensureVisible)
			{
				fileTreeItem.IsSelected = true;
				_viewModel.EnsureTreeItemVisible(fileTreeItem);
			}
			if (!isInit)
			{
				multiPanesContext.ActivePaneViewModel.SetWorkingDirectory(path, SetNavPathLocationEnum.SideBar);
			}
		}
	}

	private void OnSidebarKeyDown(object? sender, KeyEventArgs e)
	{
		if (e.Handled && (e.KeyModifiers.HasFlag(KeyModifiers.Alt) || e.KeyModifiers.HasFlag(KeyModifiers.Control) || e.KeyModifiers.HasFlag(KeyModifiers.Meta)))
		{
			e.Handled = false;
		}
	}

	private void TreeView_OnTextInput(object? sender, TextInputEventArgs e)
	{
		if (DateTime.Now.Ticks - _lastSearchTimeTick > 5000000)
		{
			_searchText = string.Empty;
			_serachTextRawInput = string.Empty;
		}
		_lastSearchTimeTick = DateTime.Now.Ticks;
		_serachTextRawInput += e.Text.ToLower();
		if (_searchText.Length == 1)
		{
			if (e.Text.ToLower() != _searchText)
			{
				_searchText = _serachTextRawInput;
			}
		}
		else
		{
			_searchText += e.Text.ToLower();
		}
		IEnumerable<FileTreeItem> allVisibleFileItems = _viewModel.GetAllVisibleFileItems(treeView.ItemsSource as IEnumerable<FileTreeItem>);
		FileTreeItem fileTreeItem = null;
		if (treeView.SelectedItem == null)
		{
			fileTreeItem = allVisibleFileItems.FirstOrDefault((FileTreeItem item) => item.Name.ToLower().StartsWith(_searchText));
		}
		else
		{
			int count = 0;
			int num = 0;
			foreach (FileTreeItem item in allVisibleFileItems)
			{
				if (item.Path == (treeView.SelectedItem as FileTreeItem).Path)
				{
					count = (num + 1) % allVisibleFileItems.Count();
					break;
				}
				num++;
			}
			fileTreeItem = allVisibleFileItems.Skip(count).FirstOrDefault((FileTreeItem item) => item.Name.ToLower().StartsWith(_searchText));
			if (fileTreeItem == null)
			{
				fileTreeItem = allVisibleFileItems.Take(count).FirstOrDefault((FileTreeItem item) => item.Name.ToLower().StartsWith(_searchText));
			}
		}
		if (fileTreeItem != null)
		{
			treeView.SelectedItem = fileTreeItem;
			fileTreeItem.IsSelected = true;
			_viewModel.EnsureTreeItemVisible(fileTreeItem);
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
		quickAccessTreeView = ((nameScope != null) ? nameScope.Find<TreeView>("quickAccessTreeView") : null);
		removableTreeView = ((nameScope != null) ? nameScope.Find<TreeView>("removableTreeView") : null);
		EjectDriveButton = ((nameScope != null) ? nameScope.Find<Button>("EjectDriveButton") : null);
		treeView = ((nameScope != null) ? nameScope.Find<TreeView>("treeView") : null);
	}

	[CompilerGenerated]
	private unsafe static void _0021XamlIlPopulate(IServiceProvider P_0, SidebarView P_1)
	{
		XamlIlContext.Context<SidebarView> context = new XamlIlContext.Context<SidebarView>(P_0, new object[1] { _0021AvaloniaResources.NamespaceInfo_003A_002FUserControls_002FSidebar_002FSidebarView_002Eaxaml.Singleton }, "avares://XFolder/UserControls/Sidebar/SidebarView.axaml")
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
		((ResourceDictionary)P_1.Resources).AddDeferred((object)"FileIconConverter", XamlIlRuntimeHelpers.DeferredTransformationFactoryV3<object>((nint)(delegate*<IServiceProvider, object>)(&XamlClosure_5.Build_1), context));
		((ResourceDictionary)P_1.Resources).AddDeferred((object)"FontSizeToIconSizeConverter", XamlIlRuntimeHelpers.DeferredTransformationFactoryV3<object>((nint)(delegate*<IServiceProvider, object>)(&XamlClosure_5.Build_2), context));
		Styles styles = P_1.Styles;
		Style style = new Style();
		style.Selector = ((Selector?)null).OfType(typeof(TreeViewItem)).Template().OfType(typeof(Panel))
			.Name("PART_ExpandCollapseChevronContainer");
		Setter setter = new Setter();
		setter.Property = Layoutable.MarginProperty;
		setter.Value = new Thickness(0.0, 0.0, 2.0, 0.0);
		style.Add(setter);
		styles.Add(style);
		Styles styles2 = P_1.Styles;
		Style style2 = new Style();
		style2.Selector = ((Selector?)null).OfType(typeof(TreeViewItem)).Template().OfType(typeof(Border))
			.Name("PART_LayoutRoot");
		Setter setter2 = new Setter();
		setter2.Property = Border.CornerRadiusProperty;
		setter2.Value = new CornerRadius(6.0, 6.0, 6.0, 6.0);
		style2.Add(setter2);
		styles2.Add(style2);
		Styles styles3 = P_1.Styles;
		Style style3;
		Style item = (style3 = new Style());
		context.PushParent(style3);
		Style style4 = style3;
		style4.Selector = ((Selector?)null).OfType(typeof(TreeViewItem)).Class(":selected").Template()
			.OfType(typeof(Border))
			.Name("PART_LayoutRoot");
		Setter setter4;
		Setter setter3 = (setter4 = new Setter());
		context.PushParent(setter4);
		Setter setter5 = setter4;
		setter5.Property = Border.BackgroundProperty;
		DynamicResourceExtension dynamicResourceExtension = new DynamicResourceExtension("TreeViewItemSelectedBackgroundBrush");
		context.ProvideTargetProperty = XamlIlHelpers.Avalonia_002EStyling_002ESetter_002CAvalonia_002EBase_002EValue_0021Property();
		BindingBase value = dynamicResourceExtension.ProvideValue(context);
		context.ProvideTargetProperty = null;
		setter5.Value = value;
		context.PopParent();
		style4.Add(setter3);
		context.PopParent();
		styles3.Add(item);
		ScrollViewer scrollViewer2;
		ScrollViewer scrollViewer = (scrollViewer2 = new ScrollViewer());
		((ISupportInitialize)scrollViewer).BeginInit();
		P_1.Content = scrollViewer;
		ScrollViewer scrollViewer4;
		ScrollViewer scrollViewer3 = (scrollViewer4 = scrollViewer2);
		context.PushParent(scrollViewer4);
		StyledProperty<IBrush?> backgroundProperty = TemplatedControl.BackgroundProperty;
		DynamicResourceExtension dynamicResourceExtension2 = new DynamicResourceExtension("SidebarBackground");
		context.ProvideTargetProperty = TemplatedControl.BackgroundProperty;
		BindingBase binding = dynamicResourceExtension2.ProvideValue(context);
		context.ProvideTargetProperty = null;
		scrollViewer4.Bind(backgroundProperty, binding);
		scrollViewer4.HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled;
		scrollViewer4.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
		StackPanel stackPanel2;
		StackPanel stackPanel = (stackPanel2 = new StackPanel());
		((ISupportInitialize)stackPanel).BeginInit();
		scrollViewer4.Content = stackPanel;
		StackPanel stackPanel4;
		StackPanel stackPanel3 = (stackPanel4 = stackPanel2);
		context.PushParent(stackPanel4);
		Controls children = stackPanel4.Children;
		TreeView treeView2;
		TreeView treeView = (treeView2 = new TreeView());
		((ISupportInitialize)treeView).BeginInit();
		children.Add(treeView);
		TreeView treeView4;
		TreeView treeView3 = (treeView4 = treeView2);
		context.PushParent(treeView4);
		TreeView treeView5 = treeView4;
		treeView5.Name = "quickAccessTreeView";
		object element = treeView5;
		context.AvaloniaNameScope.Register("quickAccessTreeView", element);
		StyledProperty<IEnumerable?> itemsSourceProperty = ItemsControl.ItemsSourceProperty;
		CompiledBindingExtension compiledBindingExtension = new CompiledBindingExtension(new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EUserControls_002ESidebar_002ESidebarViewModel_002CXFolder_002EQuickAccessNodes_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build());
		context.ProvideTargetProperty = ItemsControl.ItemsSourceProperty;
		CompiledBinding binding2 = compiledBindingExtension.ProvideValue(context);
		context.ProvideTargetProperty = null;
		treeView5.Bind(itemsSourceProperty, binding2);
		treeView5.Background = new ImmutableSolidColorBrush(16777215u);
		ScrollViewer.SetHorizontalScrollBarVisibility(treeView5, ScrollBarVisibility.Disabled);
		ScrollViewer.SetVerticalScrollBarVisibility(treeView5, ScrollBarVisibility.Disabled);
		treeView5.SelectionMode = SelectionMode.Single;
		treeView5.AddHandler(InputElement.TappedEvent, context.RootObject.QuickAccess_OnTapped);
		treeView5.Resources.Add("TreeViewItemIndent", 0.0);
		Styles styles4 = treeView5.Styles;
		Style item2 = (style3 = new Style());
		context.PushParent(style3);
		Style style5 = style3;
		style5.Selector = ((Selector?)null).OfType(typeof(TreeViewItem));
		Setter setter6 = (setter4 = new Setter());
		context.PushParent(setter4);
		Setter setter7 = setter4;
		setter7.Property = TreeViewItem.IsExpandedProperty;
		CompiledBindingExtension compiledBindingExtension2 = new CompiledBindingExtension(new CompiledBindingPathBuilder().TypeCast<FileTreeItem>().Property(XamlIlHelpers.Files_002EData_002EItems_002EFileTreeItem_002CXFolder_002EIsExpanded_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build());
		context.ProvideTargetProperty = XamlIlHelpers.Avalonia_002EStyling_002ESetter_002CAvalonia_002EBase_002EValue_0021Property();
		CompiledBinding value2 = compiledBindingExtension2.ProvideValue(context);
		context.ProvideTargetProperty = null;
		setter7.Value = value2;
		context.PopParent();
		style5.Add(setter6);
		Setter setter8 = (setter4 = new Setter());
		context.PushParent(setter4);
		Setter setter9 = setter4;
		setter9.Property = TreeViewItem.IsSelectedProperty;
		CompiledBindingExtension compiledBindingExtension3 = new CompiledBindingExtension(new CompiledBindingPathBuilder().TypeCast<FileTreeItem>().Property(XamlIlHelpers.Files_002EData_002EItems_002EFileTreeItem_002CXFolder_002EIsSelected_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build());
		context.ProvideTargetProperty = XamlIlHelpers.Avalonia_002EStyling_002ESetter_002CAvalonia_002EBase_002EValue_0021Property();
		CompiledBinding value3 = compiledBindingExtension3.ProvideValue(context);
		context.ProvideTargetProperty = null;
		setter9.Value = value3;
		context.PopParent();
		style5.Add(setter8);
		Setter setter10 = new Setter();
		setter10.Property = TemplatedControl.PaddingProperty;
		setter10.Value = new Thickness(0.0, 0.0, 0.0, 0.0);
		style5.Add(setter10);
		Setter setter11 = (setter4 = new Setter());
		context.PushParent(setter4);
		Setter setter12 = setter4;
		setter12.Property = Layoutable.MinHeightProperty;
		CompiledBindingExtension compiledBindingExtension5;
		CompiledBindingExtension compiledBindingExtension4 = (compiledBindingExtension5 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Self().Property(TemplatedControl.FontSizeProperty, PropertyInfoAccessorFactory.CreateAvaloniaPropertyAccessor).Build()));
		context.PushParent(compiledBindingExtension5);
		CompiledBindingExtension compiledBindingExtension6 = compiledBindingExtension5;
		StaticResourceExtension staticResourceExtension = new StaticResourceExtension("FontSizeToIconSizeConverter");
		context.ProvideTargetProperty = XamlIlHelpers.Avalonia_002EData_002ECompiledBinding_002CAvalonia_002EBase_002EConverter_0021Property();
		object? obj = staticResourceExtension.ProvideValue(context);
		context.ProvideTargetProperty = null;
		compiledBindingExtension6.Converter = (IValueConverter)obj;
		compiledBindingExtension6.ConverterParameter = "1.62";
		context.PopParent();
		context.ProvideTargetProperty = XamlIlHelpers.Avalonia_002EStyling_002ESetter_002CAvalonia_002EBase_002EValue_0021Property();
		CompiledBinding value4 = compiledBindingExtension4.ProvideValue(context);
		context.ProvideTargetProperty = null;
		setter12.Value = value4;
		context.PopParent();
		style5.Add(setter11);
		Setter setter13 = new Setter();
		setter13.Property = Layoutable.MarginProperty;
		setter13.Value = new Thickness(0.0, 1.0, 0.0, 1.0);
		style5.Add(setter13);
		Setter setter14 = (setter4 = new Setter());
		context.PushParent(setter4);
		Setter setter15 = setter4;
		setter15.Property = TemplatedControl.FontSizeProperty;
		DynamicResourceExtension dynamicResourceExtension3 = new DynamicResourceExtension("GlobalFontSize");
		context.ProvideTargetProperty = XamlIlHelpers.Avalonia_002EStyling_002ESetter_002CAvalonia_002EBase_002EValue_0021Property();
		BindingBase value5 = dynamicResourceExtension3.ProvideValue(context);
		context.ProvideTargetProperty = null;
		setter15.Value = value5;
		context.PopParent();
		style5.Add(setter14);
		Setter setter16 = (setter4 = new Setter());
		context.PushParent(setter4);
		Setter setter17 = setter4;
		setter17.Property = Control.ContextMenuProperty;
		ContextMenu contextMenu2;
		ContextMenu contextMenu = (contextMenu2 = new ContextMenu());
		((ISupportInitialize)contextMenu).BeginInit();
		setter17.Value = contextMenu;
		ContextMenu contextMenu4;
		ContextMenu contextMenu3 = (contextMenu4 = contextMenu2);
		context.PushParent(contextMenu4);
		ItemCollection items = contextMenu4.Items;
		MenuItem menuItem2;
		MenuItem menuItem = (menuItem2 = new MenuItem());
		((ISupportInitialize)menuItem).BeginInit();
		items.Add(menuItem);
		MenuItem menuItem4;
		MenuItem menuItem3 = (menuItem4 = menuItem2);
		context.PushParent(menuItem4);
		ReflectionBindingExtension reflectionBindingExtension = new ReflectionBindingExtension("[UnpinFromSidebar]")
		{
			Source = LocalizationBindingSource.Instance
		};
		context.ProvideTargetProperty = HeaderedSelectingItemsControl.HeaderProperty;
		ReflectionBinding reflectionBinding = reflectionBindingExtension.ProvideValue(context);
		context.ProvideTargetProperty = null;
		XamlDynamicSetters._003C_003EXamlDynamicSetter_5(menuItem4, reflectionBinding);
		StyledProperty<ICommand?> commandProperty = MenuItem.CommandProperty;
		ReflectionBindingExtension reflectionBindingExtension2 = new ReflectionBindingExtension("$parent[UserControl].DataContext.RemoveFromSidebarCommand");
		context.ProvideTargetProperty = MenuItem.CommandProperty;
		ReflectionBinding binding3 = reflectionBindingExtension2.ProvideValue(context);
		context.ProvideTargetProperty = null;
		menuItem4.Bind(commandProperty, binding3);
		ReflectionBindingExtension reflectionBindingExtension3 = new ReflectionBindingExtension();
		context.ProvideTargetProperty = MenuItem.CommandParameterProperty;
		ReflectionBinding reflectionBinding2 = reflectionBindingExtension3.ProvideValue(context);
		context.ProvideTargetProperty = null;
		XamlDynamicSetters._003C_003EXamlDynamicSetter_6(menuItem4, reflectionBinding2);
		StyledProperty<bool> isVisibleProperty = Visual.IsVisibleProperty;
		ReflectionBindingExtension reflectionBindingExtension4 = new ReflectionBindingExtension("Path")
		{
			Converter = StringConverters.IsNotNullOrEmpty
		};
		context.ProvideTargetProperty = Visual.IsVisibleProperty;
		ReflectionBinding binding4 = reflectionBindingExtension4.ProvideValue(context);
		context.ProvideTargetProperty = null;
		menuItem4.Bind(isVisibleProperty, binding4);
		context.PopParent();
		((ISupportInitialize)menuItem3).EndInit();
		context.PopParent();
		((ISupportInitialize)contextMenu3).EndInit();
		context.PopParent();
		style5.Add(setter16);
		context.PopParent();
		styles4.Add(item2);
		Styles styles5 = treeView5.Styles;
		Style style6 = new Style();
		style6.Selector = ((Selector?)null).OfType(typeof(TreeViewItem)).Descendant().OfType(typeof(TreeViewItem));
		Setter setter18 = new Setter();
		setter18.Property = Layoutable.MarginProperty;
		setter18.Value = new Thickness(0.0, 0.0, 0.0, 0.0);
		style6.Add(setter18);
		styles5.Add(style6);
		TreeDataTemplate treeDataTemplate;
		TreeDataTemplate itemTemplate = (treeDataTemplate = new TreeDataTemplate());
		context.PushParent(treeDataTemplate);
		TreeDataTemplate treeDataTemplate2 = treeDataTemplate;
		CompiledBindingExtension compiledBindingExtension7 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EData_002EItems_002EFileTreeItem_002CXFolder_002EChildren_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build());
		context.ProvideTargetProperty = XamlIlHelpers.Avalonia_002EMarkup_002EXaml_002ETemplates_002ETreeDataTemplate_002CAvalonia_002EMarkup_002EXaml_002EItemsSource_0021Property();
		CompiledBinding itemsSource = compiledBindingExtension7.ProvideValue(context);
		context.ProvideTargetProperty = null;
		treeDataTemplate2.ItemsSource = itemsSource;
		treeDataTemplate2.Content = XamlIlRuntimeHelpers.DeferredTransformationFactoryV3<Control>((nint)(delegate*<IServiceProvider, object>)(&XamlClosure_5.Build_3), context);
		context.PopParent();
		treeView5.ItemTemplate = itemTemplate;
		context.PopParent();
		((ISupportInitialize)treeView3).EndInit();
		Controls children2 = stackPanel4.Children;
		TreeView treeView7;
		TreeView treeView6 = (treeView7 = new TreeView());
		((ISupportInitialize)treeView6).BeginInit();
		children2.Add(treeView6);
		TreeView treeView8 = (treeView4 = treeView7);
		context.PushParent(treeView4);
		TreeView treeView9 = treeView4;
		treeView9.Name = "removableTreeView";
		element = treeView9;
		context.AvaloniaNameScope.Register("removableTreeView", element);
		StyledProperty<IEnumerable?> itemsSourceProperty2 = ItemsControl.ItemsSourceProperty;
		CompiledBindingExtension compiledBindingExtension8 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EUserControls_002ESidebar_002ESidebarViewModel_002CXFolder_002ERemovableDriveNodes_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build());
		context.ProvideTargetProperty = ItemsControl.ItemsSourceProperty;
		CompiledBinding binding5 = compiledBindingExtension8.ProvideValue(context);
		context.ProvideTargetProperty = null;
		treeView9.Bind(itemsSourceProperty2, binding5);
		StyledProperty<bool> isVisibleProperty2 = Visual.IsVisibleProperty;
		CompiledBindingExtension compiledBindingExtension9 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EUserControls_002ESidebar_002ESidebarViewModel_002CXFolder_002EHasRemovableDrives_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build());
		context.ProvideTargetProperty = Visual.IsVisibleProperty;
		CompiledBinding binding6 = compiledBindingExtension9.ProvideValue(context);
		context.ProvideTargetProperty = null;
		treeView9.Bind(isVisibleProperty2, binding6);
		treeView9.SelectionMode = SelectionMode.Single;
		treeView9.Background = new ImmutableSolidColorBrush(16777215u);
		ScrollViewer.SetHorizontalScrollBarVisibility(treeView9, ScrollBarVisibility.Disabled);
		ScrollViewer.SetVerticalScrollBarVisibility(treeView9, ScrollBarVisibility.Disabled);
		treeView9.Margin = new Thickness(0.0, 6.0, 0.0, 6.0);
		treeView9.AddHandler(InputElement.TappedEvent, context.RootObject.RemovableTreeView_OnTapped);
		treeView9.Resources.Add("TreeViewItemIndent", 0.0);
		Styles styles6 = treeView9.Styles;
		Style item3 = (style3 = new Style());
		context.PushParent(style3);
		Style style7 = style3;
		style7.Selector = ((Selector?)null).OfType(typeof(TreeViewItem));
		Setter setter19 = (setter4 = new Setter());
		context.PushParent(setter4);
		Setter setter20 = setter4;
		setter20.Property = TreeViewItem.IsExpandedProperty;
		CompiledBindingExtension compiledBindingExtension10 = new CompiledBindingExtension(new CompiledBindingPathBuilder().TypeCast<RemovableDriveTreeItem>().Property(XamlIlHelpers.Files_002EData_002EItems_002ERemovableDriveTreeItem_002CXFolder_002EIsExpanded_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build());
		context.ProvideTargetProperty = XamlIlHelpers.Avalonia_002EStyling_002ESetter_002CAvalonia_002EBase_002EValue_0021Property();
		CompiledBinding value6 = compiledBindingExtension10.ProvideValue(context);
		context.ProvideTargetProperty = null;
		setter20.Value = value6;
		context.PopParent();
		style7.Add(setter19);
		Setter setter21 = (setter4 = new Setter());
		context.PushParent(setter4);
		Setter setter22 = setter4;
		setter22.Property = TreeViewItem.IsSelectedProperty;
		CompiledBindingExtension compiledBindingExtension11 = new CompiledBindingExtension(new CompiledBindingPathBuilder().TypeCast<RemovableDriveTreeItem>().Property(XamlIlHelpers.Files_002EData_002EItems_002ERemovableDriveTreeItem_002CXFolder_002EIsSelected_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build());
		context.ProvideTargetProperty = XamlIlHelpers.Avalonia_002EStyling_002ESetter_002CAvalonia_002EBase_002EValue_0021Property();
		CompiledBinding value7 = compiledBindingExtension11.ProvideValue(context);
		context.ProvideTargetProperty = null;
		setter22.Value = value7;
		context.PopParent();
		style7.Add(setter21);
		Setter setter23 = new Setter();
		setter23.Property = TemplatedControl.PaddingProperty;
		setter23.Value = new Thickness(0.0, 0.0, 0.0, 0.0);
		style7.Add(setter23);
		Setter setter24 = (setter4 = new Setter());
		context.PushParent(setter4);
		Setter setter25 = setter4;
		setter25.Property = Layoutable.MinHeightProperty;
		CompiledBindingExtension compiledBindingExtension12 = (compiledBindingExtension5 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Self().Property(TemplatedControl.FontSizeProperty, PropertyInfoAccessorFactory.CreateAvaloniaPropertyAccessor).Build()));
		context.PushParent(compiledBindingExtension5);
		CompiledBindingExtension compiledBindingExtension13 = compiledBindingExtension5;
		StaticResourceExtension staticResourceExtension2 = new StaticResourceExtension("FontSizeToIconSizeConverter");
		context.ProvideTargetProperty = XamlIlHelpers.Avalonia_002EData_002ECompiledBinding_002CAvalonia_002EBase_002EConverter_0021Property();
		object? obj2 = staticResourceExtension2.ProvideValue(context);
		context.ProvideTargetProperty = null;
		compiledBindingExtension13.Converter = (IValueConverter)obj2;
		compiledBindingExtension13.ConverterParameter = "1.62";
		context.PopParent();
		context.ProvideTargetProperty = XamlIlHelpers.Avalonia_002EStyling_002ESetter_002CAvalonia_002EBase_002EValue_0021Property();
		CompiledBinding value8 = compiledBindingExtension12.ProvideValue(context);
		context.ProvideTargetProperty = null;
		setter25.Value = value8;
		context.PopParent();
		style7.Add(setter24);
		Setter setter26 = new Setter();
		setter26.Property = Layoutable.MarginProperty;
		setter26.Value = new Thickness(0.0, 1.0, 0.0, 1.0);
		style7.Add(setter26);
		Setter setter27 = (setter4 = new Setter());
		context.PushParent(setter4);
		Setter setter28 = setter4;
		setter28.Property = TemplatedControl.FontSizeProperty;
		DynamicResourceExtension dynamicResourceExtension4 = new DynamicResourceExtension("GlobalFontSize");
		context.ProvideTargetProperty = XamlIlHelpers.Avalonia_002EStyling_002ESetter_002CAvalonia_002EBase_002EValue_0021Property();
		BindingBase value9 = dynamicResourceExtension4.ProvideValue(context);
		context.ProvideTargetProperty = null;
		setter28.Value = value9;
		context.PopParent();
		style7.Add(setter27);
		context.PopParent();
		styles6.Add(item3);
		TreeDataTemplate itemTemplate2 = (treeDataTemplate = new TreeDataTemplate());
		context.PushParent(treeDataTemplate);
		TreeDataTemplate treeDataTemplate3 = treeDataTemplate;
		treeDataTemplate3.DataType = typeof(RemovableDriveTreeItem);
		CompiledBindingExtension compiledBindingExtension14 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EData_002EItems_002ERemovableDriveTreeItem_002CXFolder_002EChildren_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build());
		context.ProvideTargetProperty = XamlIlHelpers.Avalonia_002EMarkup_002EXaml_002ETemplates_002ETreeDataTemplate_002CAvalonia_002EMarkup_002EXaml_002EItemsSource_0021Property();
		CompiledBinding itemsSource2 = compiledBindingExtension14.ProvideValue(context);
		context.ProvideTargetProperty = null;
		treeDataTemplate3.ItemsSource = itemsSource2;
		treeDataTemplate3.Content = XamlIlRuntimeHelpers.DeferredTransformationFactoryV3<Control>((nint)(delegate*<IServiceProvider, object>)(&XamlClosure_5.Build_4), context);
		context.PopParent();
		treeView9.ItemTemplate = itemTemplate2;
		context.PopParent();
		((ISupportInitialize)treeView8).EndInit();
		Controls children3 = stackPanel4.Children;
		TreeView treeView11;
		TreeView treeView10 = (treeView11 = new TreeView());
		((ISupportInitialize)treeView10).BeginInit();
		children3.Add(treeView10);
		TreeView treeView12 = (treeView4 = treeView11);
		context.PushParent(treeView4);
		TreeView treeView13 = treeView4;
		treeView13.Name = "treeView";
		element = treeView13;
		context.AvaloniaNameScope.Register("treeView", element);
		StyledProperty<IEnumerable?> itemsSourceProperty3 = ItemsControl.ItemsSourceProperty;
		CompiledBindingExtension compiledBindingExtension15 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EUserControls_002ESidebar_002ESidebarViewModel_002CXFolder_002EFilesystemNodes_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build());
		context.ProvideTargetProperty = ItemsControl.ItemsSourceProperty;
		CompiledBinding binding7 = compiledBindingExtension15.ProvideValue(context);
		context.ProvideTargetProperty = null;
		treeView13.Bind(itemsSourceProperty3, binding7);
		treeView13.SelectionMode = SelectionMode.Single;
		treeView13.AddHandler(InputElement.TextInputEvent, context.RootObject.TreeView_OnTextInput);
		treeView13.Background = new ImmutableSolidColorBrush(16777215u);
		ScrollViewer.SetHorizontalScrollBarVisibility(treeView13, ScrollBarVisibility.Disabled);
		ScrollViewer.SetVerticalScrollBarVisibility(treeView13, ScrollBarVisibility.Disabled);
		treeView13.Padding = new Thickness(0.0, 0.0, 0.0, 0.0);
		treeView13.AddHandler(InputElement.TappedEvent, context.RootObject.TreeView_OnTapped);
		treeView13.Resources.Add("TreeViewItemIndent", 10.0);
		Styles styles7 = treeView13.Styles;
		Style item4 = (style3 = new Style());
		context.PushParent(style3);
		Style style8 = style3;
		style8.Selector = ((Selector?)null).OfType(typeof(TreeViewItem));
		Setter setter29 = (setter4 = new Setter());
		context.PushParent(setter4);
		Setter setter30 = setter4;
		setter30.Property = TreeViewItem.IsExpandedProperty;
		CompiledBindingExtension compiledBindingExtension16 = new CompiledBindingExtension(new CompiledBindingPathBuilder().TypeCast<FileTreeItem>().Property(XamlIlHelpers.Files_002EData_002EItems_002EFileTreeItem_002CXFolder_002EIsExpanded_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build());
		context.ProvideTargetProperty = XamlIlHelpers.Avalonia_002EStyling_002ESetter_002CAvalonia_002EBase_002EValue_0021Property();
		CompiledBinding value10 = compiledBindingExtension16.ProvideValue(context);
		context.ProvideTargetProperty = null;
		setter30.Value = value10;
		context.PopParent();
		style8.Add(setter29);
		Setter setter31 = (setter4 = new Setter());
		context.PushParent(setter4);
		Setter setter32 = setter4;
		setter32.Property = TreeViewItem.IsSelectedProperty;
		CompiledBindingExtension compiledBindingExtension17 = new CompiledBindingExtension(new CompiledBindingPathBuilder().TypeCast<FileTreeItem>().Property(XamlIlHelpers.Files_002EData_002EItems_002EFileTreeItem_002CXFolder_002EIsSelected_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build());
		context.ProvideTargetProperty = XamlIlHelpers.Avalonia_002EStyling_002ESetter_002CAvalonia_002EBase_002EValue_0021Property();
		CompiledBinding value11 = compiledBindingExtension17.ProvideValue(context);
		context.ProvideTargetProperty = null;
		setter32.Value = value11;
		context.PopParent();
		style8.Add(setter31);
		Setter setter33 = new Setter();
		setter33.Property = TemplatedControl.PaddingProperty;
		setter33.Value = new Thickness(0.0, 0.0, 0.0, 0.0);
		style8.Add(setter33);
		Setter setter34 = (setter4 = new Setter());
		context.PushParent(setter4);
		Setter setter35 = setter4;
		setter35.Property = Layoutable.MinHeightProperty;
		CompiledBindingExtension compiledBindingExtension18 = (compiledBindingExtension5 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Self().Property(TemplatedControl.FontSizeProperty, PropertyInfoAccessorFactory.CreateAvaloniaPropertyAccessor).Build()));
		context.PushParent(compiledBindingExtension5);
		CompiledBindingExtension compiledBindingExtension19 = compiledBindingExtension5;
		StaticResourceExtension staticResourceExtension3 = new StaticResourceExtension("FontSizeToIconSizeConverter");
		context.ProvideTargetProperty = XamlIlHelpers.Avalonia_002EData_002ECompiledBinding_002CAvalonia_002EBase_002EConverter_0021Property();
		object? obj3 = staticResourceExtension3.ProvideValue(context);
		context.ProvideTargetProperty = null;
		compiledBindingExtension19.Converter = (IValueConverter)obj3;
		compiledBindingExtension19.ConverterParameter = "1.5";
		context.PopParent();
		context.ProvideTargetProperty = XamlIlHelpers.Avalonia_002EStyling_002ESetter_002CAvalonia_002EBase_002EValue_0021Property();
		CompiledBinding value12 = compiledBindingExtension18.ProvideValue(context);
		context.ProvideTargetProperty = null;
		setter35.Value = value12;
		context.PopParent();
		style8.Add(setter34);
		Setter setter36 = new Setter();
		setter36.Property = Layoutable.MarginProperty;
		setter36.Value = new Thickness(0.0, 1.0, 0.0, 1.0);
		style8.Add(setter36);
		Setter setter37 = (setter4 = new Setter());
		context.PushParent(setter4);
		Setter setter38 = setter4;
		setter38.Property = TemplatedControl.FontSizeProperty;
		DynamicResourceExtension dynamicResourceExtension5 = new DynamicResourceExtension("GlobalFontSize");
		context.ProvideTargetProperty = XamlIlHelpers.Avalonia_002EStyling_002ESetter_002CAvalonia_002EBase_002EValue_0021Property();
		BindingBase value13 = dynamicResourceExtension5.ProvideValue(context);
		context.ProvideTargetProperty = null;
		setter38.Value = value13;
		context.PopParent();
		style8.Add(setter37);
		context.PopParent();
		styles7.Add(item4);
		Styles styles8 = treeView13.Styles;
		Style style9 = new Style();
		style9.Selector = ((Selector?)null).OfType(typeof(TreeViewItem)).Descendant().OfType(typeof(TreeViewItem));
		Setter setter39 = new Setter();
		setter39.Property = Layoutable.MarginProperty;
		setter39.Value = new Thickness(0.0, 0.0, 0.0, 0.0);
		style9.Add(setter39);
		styles8.Add(style9);
		TreeDataTemplate itemTemplate3 = (treeDataTemplate = new TreeDataTemplate());
		context.PushParent(treeDataTemplate);
		TreeDataTemplate treeDataTemplate4 = treeDataTemplate;
		CompiledBindingExtension compiledBindingExtension20 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EData_002EItems_002EFileTreeItem_002CXFolder_002EChildren_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build());
		context.ProvideTargetProperty = XamlIlHelpers.Avalonia_002EMarkup_002EXaml_002ETemplates_002ETreeDataTemplate_002CAvalonia_002EMarkup_002EXaml_002EItemsSource_0021Property();
		CompiledBinding itemsSource3 = compiledBindingExtension20.ProvideValue(context);
		context.ProvideTargetProperty = null;
		treeDataTemplate4.ItemsSource = itemsSource3;
		treeDataTemplate4.Content = XamlIlRuntimeHelpers.DeferredTransformationFactoryV3<Control>((nint)(delegate*<IServiceProvider, object>)(&XamlClosure_5.Build_5), context);
		context.PopParent();
		treeView13.ItemTemplate = itemTemplate3;
		context.PopParent();
		((ISupportInitialize)treeView12).EndInit();
		context.PopParent();
		((ISupportInitialize)stackPanel3).EndInit();
		context.PopParent();
		((ISupportInitialize)scrollViewer3).EndInit();
		context.PopParent();
		((ISupportInitialize)P_1).EndInit();
		if (P_1 is StyledElement styled)
		{
			NameScope.SetNameScope(styled, context.AvaloniaNameScope);
		}
		context.AvaloniaNameScope.Complete();
	}

	[CompilerGenerated]
	private static void _0021XamlIlPopulateTrampoline(SidebarView P_0)
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
