using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Avalonia;
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
using CommunityToolkit.Mvvm.DependencyInjection;
using CompiledAvaloniaXaml;
using Files.Converters;
using Files.Data.Commands;
using Files.Data.Commands.Manager;
using Files.Data.Contexts;
using Files.Data.Enums;
using Files.Data.EventArguments;
using Files.Helpers.Application;
using Files.Helpers.Shared;
using Files.Services.App;
using Files.UserControls.NavigationToolbar;
using Files.ViewModels.Items;
using Files.ViewModels.Shells;
using Files.Views.Layouts;

namespace Files.Views.Shells;

public class ModernShellView : BaseShellView
{
	[CompilerGenerated]
	private class XamlClosure_11
	{
		public static object Build_1(IServiceProvider P_0)
		{
			XamlIlContext.Context<ModernShellView> context = CreateContext(P_0);
			return new FontSizeToIconSizeConverter();
		}

		public static XamlIlContext.Context<ModernShellView> CreateContext(IServiceProvider P_0)
		{
			XamlIlContext.Context<ModernShellView> context = new XamlIlContext.Context<ModernShellView>(P_0, new object[1] { _0021AvaloniaResources.NamespaceInfo_003A_002FViews_002FShells_002FModernShellView_002Eaxaml.Singleton }, "avares://XFolder/Views/Shells/ModernShellView.axaml");
			if (P_0 != null)
			{
				object service = P_0.GetService(typeof(IRootObjectProvider));
				if (service != null)
				{
					service = ((IRootObjectProvider)service).RootObject;
					context.RootObject = (ModernShellView)service;
				}
			}
			return context;
		}

		public static object Build_2(IServiceProvider P_0)
		{
			XamlIlContext.Context<ModernShellView> context = CreateContext(P_0);
			context.IntermediateRoot = new UniformGrid();
			object obj = context.IntermediateRoot;
			((ISupportInitialize)obj).BeginInit();
			((AvaloniaObject)obj).SetValue(UniformGrid.RowsProperty, 1, BindingPriority.Template);
			((AvaloniaObject)obj).SetValue(UniformGrid.ColumnsProperty, 0, BindingPriority.Template);
			((ISupportInitialize)obj).EndInit();
			return obj;
		}

		public static object Build_3(IServiceProvider P_0)
		{
			XamlIlContext.Context<ModernShellView> context = CreateContext(P_0);
			context.IntermediateRoot = new Grid();
			object obj = context.IntermediateRoot;
			((ISupportInitialize)obj).BeginInit();
			Grid grid = (Grid)obj;
			context.PushParent(grid);
			grid.Background = new ImmutableSolidColorBrush(16777215u);
			grid.Margin = new Thickness(0.0, 0.0, 0.0, 0.0);
			grid.AddHandler(InputElement.PointerPressedEvent, context.RootObject.TabHeader_OnPointerPressed);
			ContextMenu contextMenu2;
			ContextMenu contextMenu = (contextMenu2 = new ContextMenu());
			((ISupportInitialize)contextMenu).BeginInit();
			grid.ContextMenu = contextMenu;
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
			MenuItem menuItem5 = menuItem4;
			CompiledBindingExtension compiledBindingExtension = new CompiledBindingExtension(new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EServices_002EApp_002ELocalizationBindingSource_002CXFolder_002EItem_005BNewTab_005D_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build())
			{
				Source = LocalizationBindingSource.Instance
			};
			context.ProvideTargetProperty = HeaderedSelectingItemsControl.HeaderProperty;
			CompiledBinding compiledBinding = compiledBindingExtension.ProvideValue(context);
			context.ProvideTargetProperty = null;
			XamlDynamicSetters._003C_003EXamlDynamicSetter_13(menuItem5, compiledBinding);
			StyledProperty<KeyGesture?> inputGestureProperty = MenuItem.InputGestureProperty;
			CompiledBindingExtension compiledBindingExtension2 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EViewModels_002EItems_002ETabItemViewModel_002CXFolder_002ENewTabInputGesture_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build());
			context.ProvideTargetProperty = MenuItem.InputGestureProperty;
			CompiledBinding binding = compiledBindingExtension2.ProvideValue(context);
			context.ProvideTargetProperty = null;
			menuItem5.Bind(inputGestureProperty, binding);
			StyledProperty<ICommand?> commandProperty = MenuItem.CommandProperty;
			CompiledBindingExtension compiledBindingExtension3 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EViewModels_002EItems_002ETabItemViewModel_002CXFolder_002ENewTabCommand_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build());
			context.ProvideTargetProperty = MenuItem.CommandProperty;
			CompiledBinding binding2 = compiledBindingExtension3.ProvideValue(context);
			context.ProvideTargetProperty = null;
			menuItem5.Bind(commandProperty, binding2);
			Viewbox viewbox2;
			Viewbox viewbox = (viewbox2 = new Viewbox());
			((ISupportInitialize)viewbox).BeginInit();
			menuItem5.Icon = viewbox;
			Viewbox viewbox4;
			Viewbox viewbox3 = (viewbox4 = viewbox2);
			context.PushParent(viewbox4);
			Viewbox viewbox5 = viewbox4;
			viewbox5.Width = 16.0;
			viewbox5.Height = 16.0;
			viewbox5.Stretch = Stretch.Uniform;
			Path path2;
			Path path = (path2 = new Path());
			((ISupportInitialize)path).BeginInit();
			viewbox5.Child = path;
			Path path4;
			Path path3 = (path4 = path2);
			context.PushParent(path4);
			Path path5 = path4;
			StyledProperty<Geometry?> dataProperty = Path.DataProperty;
			DynamicResourceExtension dynamicResourceExtension = new DynamicResourceExtension("tab_new_regular");
			context.ProvideTargetProperty = Path.DataProperty;
			BindingBase binding3 = dynamicResourceExtension.ProvideValue(context);
			context.ProvideTargetProperty = null;
			path5.Bind(dataProperty, binding3);
			StyledProperty<IBrush?> fillProperty = Shape.FillProperty;
			CompiledBindingExtension compiledBindingExtension4 = new CompiledBindingExtension(new CompiledBindingPathBuilder().VisualAncestor(typeof(MenuItem), 0).Property(TemplatedControl.ForegroundProperty, PropertyInfoAccessorFactory.CreateAvaloniaPropertyAccessor).Build());
			context.ProvideTargetProperty = Shape.FillProperty;
			CompiledBinding binding4 = compiledBindingExtension4.ProvideValue(context);
			context.ProvideTargetProperty = null;
			path5.Bind(fillProperty, binding4);
			context.PopParent();
			((ISupportInitialize)path3).EndInit();
			context.PopParent();
			((ISupportInitialize)viewbox3).EndInit();
			context.PopParent();
			((ISupportInitialize)menuItem3).EndInit();
			ItemCollection items2 = contextMenu4.Items;
			MenuItem menuItem7;
			MenuItem menuItem6 = (menuItem7 = new MenuItem());
			((ISupportInitialize)menuItem6).BeginInit();
			items2.Add(menuItem6);
			MenuItem menuItem8 = (menuItem4 = menuItem7);
			context.PushParent(menuItem4);
			MenuItem menuItem9 = menuItem4;
			CompiledBindingExtension compiledBindingExtension5 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EServices_002EApp_002ELocalizationBindingSource_002CXFolder_002EItem_005BDuplicateTab_005D_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build())
			{
				Source = LocalizationBindingSource.Instance
			};
			context.ProvideTargetProperty = HeaderedSelectingItemsControl.HeaderProperty;
			CompiledBinding compiledBinding2 = compiledBindingExtension5.ProvideValue(context);
			context.ProvideTargetProperty = null;
			XamlDynamicSetters._003C_003EXamlDynamicSetter_13(menuItem9, compiledBinding2);
			StyledProperty<KeyGesture?> inputGestureProperty2 = MenuItem.InputGestureProperty;
			CompiledBindingExtension compiledBindingExtension6 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EViewModels_002EItems_002ETabItemViewModel_002CXFolder_002EDuplicateTabInputGesture_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build());
			context.ProvideTargetProperty = MenuItem.InputGestureProperty;
			CompiledBinding binding5 = compiledBindingExtension6.ProvideValue(context);
			context.ProvideTargetProperty = null;
			menuItem9.Bind(inputGestureProperty2, binding5);
			StyledProperty<ICommand?> commandProperty2 = MenuItem.CommandProperty;
			CompiledBindingExtension compiledBindingExtension7 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EViewModels_002EItems_002ETabItemViewModel_002CXFolder_002EDuplicateTabCommand_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build());
			context.ProvideTargetProperty = MenuItem.CommandProperty;
			CompiledBinding binding6 = compiledBindingExtension7.ProvideValue(context);
			context.ProvideTargetProperty = null;
			menuItem9.Bind(commandProperty2, binding6);
			Viewbox viewbox7;
			Viewbox viewbox6 = (viewbox7 = new Viewbox());
			((ISupportInitialize)viewbox6).BeginInit();
			menuItem9.Icon = viewbox6;
			Viewbox viewbox8 = (viewbox4 = viewbox7);
			context.PushParent(viewbox4);
			Viewbox viewbox9 = viewbox4;
			viewbox9.Width = 16.0;
			viewbox9.Height = 16.0;
			viewbox9.Stretch = Stretch.Uniform;
			Path path7;
			Path path6 = (path7 = new Path());
			((ISupportInitialize)path6).BeginInit();
			viewbox9.Child = path6;
			Path path8 = (path4 = path7);
			context.PushParent(path4);
			Path path9 = path4;
			StyledProperty<Geometry?> dataProperty2 = Path.DataProperty;
			DynamicResourceExtension dynamicResourceExtension2 = new DynamicResourceExtension("tab_desktop_copy_regular");
			context.ProvideTargetProperty = Path.DataProperty;
			BindingBase binding7 = dynamicResourceExtension2.ProvideValue(context);
			context.ProvideTargetProperty = null;
			path9.Bind(dataProperty2, binding7);
			StyledProperty<IBrush?> fillProperty2 = Shape.FillProperty;
			CompiledBindingExtension compiledBindingExtension8 = new CompiledBindingExtension(new CompiledBindingPathBuilder().VisualAncestor(typeof(MenuItem), 0).Property(TemplatedControl.ForegroundProperty, PropertyInfoAccessorFactory.CreateAvaloniaPropertyAccessor).Build());
			context.ProvideTargetProperty = Shape.FillProperty;
			CompiledBinding binding8 = compiledBindingExtension8.ProvideValue(context);
			context.ProvideTargetProperty = null;
			path9.Bind(fillProperty2, binding8);
			context.PopParent();
			((ISupportInitialize)path8).EndInit();
			context.PopParent();
			((ISupportInitialize)viewbox8).EndInit();
			context.PopParent();
			((ISupportInitialize)menuItem8).EndInit();
			ItemCollection items3 = contextMenu4.Items;
			MenuItem menuItem11;
			MenuItem menuItem10 = (menuItem11 = new MenuItem());
			((ISupportInitialize)menuItem10).BeginInit();
			items3.Add(menuItem10);
			MenuItem menuItem12 = (menuItem4 = menuItem11);
			context.PushParent(menuItem4);
			MenuItem menuItem13 = menuItem4;
			CompiledBindingExtension compiledBindingExtension9 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EServices_002EApp_002ELocalizationBindingSource_002CXFolder_002EItem_005BReopenClosedTab_005D_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build())
			{
				Source = LocalizationBindingSource.Instance
			};
			context.ProvideTargetProperty = HeaderedSelectingItemsControl.HeaderProperty;
			CompiledBinding compiledBinding3 = compiledBindingExtension9.ProvideValue(context);
			context.ProvideTargetProperty = null;
			XamlDynamicSetters._003C_003EXamlDynamicSetter_13(menuItem13, compiledBinding3);
			StyledProperty<KeyGesture?> inputGestureProperty3 = MenuItem.InputGestureProperty;
			CompiledBindingExtension compiledBindingExtension10 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EViewModels_002EItems_002ETabItemViewModel_002CXFolder_002EReopenClosedTabInputGesture_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build());
			context.ProvideTargetProperty = MenuItem.InputGestureProperty;
			CompiledBinding binding9 = compiledBindingExtension10.ProvideValue(context);
			context.ProvideTargetProperty = null;
			menuItem13.Bind(inputGestureProperty3, binding9);
			StyledProperty<ICommand?> commandProperty3 = MenuItem.CommandProperty;
			CompiledBindingExtension compiledBindingExtension11 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EViewModels_002EItems_002ETabItemViewModel_002CXFolder_002EReopenClosedTabCommand_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build());
			context.ProvideTargetProperty = MenuItem.CommandProperty;
			CompiledBinding binding10 = compiledBindingExtension11.ProvideValue(context);
			context.ProvideTargetProperty = null;
			menuItem13.Bind(commandProperty3, binding10);
			context.PopParent();
			((ISupportInitialize)menuItem12).EndInit();
			ItemCollection items4 = contextMenu4.Items;
			Separator separator2;
			Separator separator = (separator2 = new Separator());
			((ISupportInitialize)separator).BeginInit();
			items4.Add(separator);
			((ISupportInitialize)separator2).EndInit();
			ItemCollection items5 = contextMenu4.Items;
			MenuItem menuItem15;
			MenuItem menuItem14 = (menuItem15 = new MenuItem());
			((ISupportInitialize)menuItem14).BeginInit();
			items5.Add(menuItem14);
			MenuItem menuItem16 = (menuItem4 = menuItem15);
			context.PushParent(menuItem4);
			MenuItem menuItem17 = menuItem4;
			CompiledBindingExtension compiledBindingExtension12 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EServices_002EApp_002ELocalizationBindingSource_002CXFolder_002EItem_005BCloseTab_005D_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build())
			{
				Source = LocalizationBindingSource.Instance
			};
			context.ProvideTargetProperty = HeaderedSelectingItemsControl.HeaderProperty;
			CompiledBinding compiledBinding4 = compiledBindingExtension12.ProvideValue(context);
			context.ProvideTargetProperty = null;
			XamlDynamicSetters._003C_003EXamlDynamicSetter_13(menuItem17, compiledBinding4);
			StyledProperty<KeyGesture?> inputGestureProperty4 = MenuItem.InputGestureProperty;
			CompiledBindingExtension compiledBindingExtension13 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EViewModels_002EItems_002ETabItemViewModel_002CXFolder_002ECloseTabInputGesture_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build());
			context.ProvideTargetProperty = MenuItem.InputGestureProperty;
			CompiledBinding binding11 = compiledBindingExtension13.ProvideValue(context);
			context.ProvideTargetProperty = null;
			menuItem17.Bind(inputGestureProperty4, binding11);
			StyledProperty<ICommand?> commandProperty4 = MenuItem.CommandProperty;
			CompiledBindingExtension compiledBindingExtension14 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EViewModels_002EItems_002ETabItemViewModel_002CXFolder_002ECloseTabCommand_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build());
			context.ProvideTargetProperty = MenuItem.CommandProperty;
			CompiledBinding binding12 = compiledBindingExtension14.ProvideValue(context);
			context.ProvideTargetProperty = null;
			menuItem17.Bind(commandProperty4, binding12);
			Viewbox viewbox11;
			Viewbox viewbox10 = (viewbox11 = new Viewbox());
			((ISupportInitialize)viewbox10).BeginInit();
			menuItem17.Icon = viewbox10;
			Viewbox viewbox12 = (viewbox4 = viewbox11);
			context.PushParent(viewbox4);
			Viewbox viewbox13 = viewbox4;
			viewbox13.Width = 16.0;
			viewbox13.Height = 16.0;
			viewbox13.Stretch = Stretch.Uniform;
			Path path11;
			Path path10 = (path11 = new Path());
			((ISupportInitialize)path10).BeginInit();
			viewbox13.Child = path10;
			Path path12 = (path4 = path11);
			context.PushParent(path4);
			Path path13 = path4;
			StyledProperty<Geometry?> dataProperty3 = Path.DataProperty;
			DynamicResourceExtension dynamicResourceExtension3 = new DynamicResourceExtension("close_regular");
			context.ProvideTargetProperty = Path.DataProperty;
			BindingBase binding13 = dynamicResourceExtension3.ProvideValue(context);
			context.ProvideTargetProperty = null;
			path13.Bind(dataProperty3, binding13);
			StyledProperty<IBrush?> fillProperty3 = Shape.FillProperty;
			CompiledBindingExtension compiledBindingExtension15 = new CompiledBindingExtension(new CompiledBindingPathBuilder().VisualAncestor(typeof(MenuItem), 0).Property(TemplatedControl.ForegroundProperty, PropertyInfoAccessorFactory.CreateAvaloniaPropertyAccessor).Build());
			context.ProvideTargetProperty = Shape.FillProperty;
			CompiledBinding binding14 = compiledBindingExtension15.ProvideValue(context);
			context.ProvideTargetProperty = null;
			path13.Bind(fillProperty3, binding14);
			context.PopParent();
			((ISupportInitialize)path12).EndInit();
			context.PopParent();
			((ISupportInitialize)viewbox12).EndInit();
			context.PopParent();
			((ISupportInitialize)menuItem16).EndInit();
			ItemCollection items6 = contextMenu4.Items;
			MenuItem menuItem19;
			MenuItem menuItem18 = (menuItem19 = new MenuItem());
			((ISupportInitialize)menuItem18).BeginInit();
			items6.Add(menuItem18);
			MenuItem menuItem20 = (menuItem4 = menuItem19);
			context.PushParent(menuItem4);
			MenuItem menuItem21 = menuItem4;
			CompiledBindingExtension compiledBindingExtension16 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EServices_002EApp_002ELocalizationBindingSource_002CXFolder_002EItem_005BCloseOtherTabs_005D_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build())
			{
				Source = LocalizationBindingSource.Instance
			};
			context.ProvideTargetProperty = HeaderedSelectingItemsControl.HeaderProperty;
			CompiledBinding compiledBinding5 = compiledBindingExtension16.ProvideValue(context);
			context.ProvideTargetProperty = null;
			XamlDynamicSetters._003C_003EXamlDynamicSetter_13(menuItem21, compiledBinding5);
			StyledProperty<KeyGesture?> inputGestureProperty5 = MenuItem.InputGestureProperty;
			CompiledBindingExtension compiledBindingExtension17 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EViewModels_002EItems_002ETabItemViewModel_002CXFolder_002ECloseOtherTabsInputGesture_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build());
			context.ProvideTargetProperty = MenuItem.InputGestureProperty;
			CompiledBinding binding15 = compiledBindingExtension17.ProvideValue(context);
			context.ProvideTargetProperty = null;
			menuItem21.Bind(inputGestureProperty5, binding15);
			StyledProperty<ICommand?> commandProperty5 = MenuItem.CommandProperty;
			CompiledBindingExtension compiledBindingExtension18 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EViewModels_002EItems_002ETabItemViewModel_002CXFolder_002ECloseOtherTabsCommand_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build());
			context.ProvideTargetProperty = MenuItem.CommandProperty;
			CompiledBinding binding16 = compiledBindingExtension18.ProvideValue(context);
			context.ProvideTargetProperty = null;
			menuItem21.Bind(commandProperty5, binding16);
			context.PopParent();
			((ISupportInitialize)menuItem20).EndInit();
			ItemCollection items7 = contextMenu4.Items;
			MenuItem menuItem23;
			MenuItem menuItem22 = (menuItem23 = new MenuItem());
			((ISupportInitialize)menuItem22).BeginInit();
			items7.Add(menuItem22);
			MenuItem menuItem24 = (menuItem4 = menuItem23);
			context.PushParent(menuItem4);
			MenuItem menuItem25 = menuItem4;
			CompiledBindingExtension compiledBindingExtension19 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EServices_002EApp_002ELocalizationBindingSource_002CXFolder_002EItem_005BCloseTabsToTheLeft_005D_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build())
			{
				Source = LocalizationBindingSource.Instance
			};
			context.ProvideTargetProperty = HeaderedSelectingItemsControl.HeaderProperty;
			CompiledBinding compiledBinding6 = compiledBindingExtension19.ProvideValue(context);
			context.ProvideTargetProperty = null;
			XamlDynamicSetters._003C_003EXamlDynamicSetter_13(menuItem25, compiledBinding6);
			StyledProperty<KeyGesture?> inputGestureProperty6 = MenuItem.InputGestureProperty;
			CompiledBindingExtension compiledBindingExtension20 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EViewModels_002EItems_002ETabItemViewModel_002CXFolder_002ECloseTabsToTheLeftInputGesture_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build());
			context.ProvideTargetProperty = MenuItem.InputGestureProperty;
			CompiledBinding binding17 = compiledBindingExtension20.ProvideValue(context);
			context.ProvideTargetProperty = null;
			menuItem25.Bind(inputGestureProperty6, binding17);
			StyledProperty<ICommand?> commandProperty6 = MenuItem.CommandProperty;
			CompiledBindingExtension compiledBindingExtension21 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EViewModels_002EItems_002ETabItemViewModel_002CXFolder_002ECloseTabsToTheLeftCommand_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build());
			context.ProvideTargetProperty = MenuItem.CommandProperty;
			CompiledBinding binding18 = compiledBindingExtension21.ProvideValue(context);
			context.ProvideTargetProperty = null;
			menuItem25.Bind(commandProperty6, binding18);
			context.PopParent();
			((ISupportInitialize)menuItem24).EndInit();
			ItemCollection items8 = contextMenu4.Items;
			MenuItem menuItem27;
			MenuItem menuItem26 = (menuItem27 = new MenuItem());
			((ISupportInitialize)menuItem26).BeginInit();
			items8.Add(menuItem26);
			MenuItem menuItem28 = (menuItem4 = menuItem27);
			context.PushParent(menuItem4);
			MenuItem menuItem29 = menuItem4;
			CompiledBindingExtension compiledBindingExtension22 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EServices_002EApp_002ELocalizationBindingSource_002CXFolder_002EItem_005BCloseTabsToTheRight_005D_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build())
			{
				Source = LocalizationBindingSource.Instance
			};
			context.ProvideTargetProperty = HeaderedSelectingItemsControl.HeaderProperty;
			CompiledBinding compiledBinding7 = compiledBindingExtension22.ProvideValue(context);
			context.ProvideTargetProperty = null;
			XamlDynamicSetters._003C_003EXamlDynamicSetter_13(menuItem29, compiledBinding7);
			StyledProperty<KeyGesture?> inputGestureProperty7 = MenuItem.InputGestureProperty;
			CompiledBindingExtension compiledBindingExtension23 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EViewModels_002EItems_002ETabItemViewModel_002CXFolder_002ECloseTabsToTheRightInputGesture_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build());
			context.ProvideTargetProperty = MenuItem.InputGestureProperty;
			CompiledBinding binding19 = compiledBindingExtension23.ProvideValue(context);
			context.ProvideTargetProperty = null;
			menuItem29.Bind(inputGestureProperty7, binding19);
			StyledProperty<ICommand?> commandProperty7 = MenuItem.CommandProperty;
			CompiledBindingExtension compiledBindingExtension24 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EViewModels_002EItems_002ETabItemViewModel_002CXFolder_002ECloseTabsToTheRightCommand_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build());
			context.ProvideTargetProperty = MenuItem.CommandProperty;
			CompiledBinding binding20 = compiledBindingExtension24.ProvideValue(context);
			context.ProvideTargetProperty = null;
			menuItem29.Bind(commandProperty7, binding20);
			context.PopParent();
			((ISupportInitialize)menuItem28).EndInit();
			context.PopParent();
			((ISupportInitialize)contextMenu3).EndInit();
			Controls children = grid.Children;
			TextBlock textBlock2;
			TextBlock textBlock = (textBlock2 = new TextBlock());
			((ISupportInitialize)textBlock).BeginInit();
			children.Add(textBlock);
			TextBlock textBlock4;
			TextBlock textBlock3 = (textBlock4 = textBlock2);
			context.PushParent(textBlock4);
			TextBlock textBlock5 = textBlock4;
			StyledProperty<string?> textProperty = TextBlock.TextProperty;
			CompiledBindingExtension compiledBindingExtension25 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EViewModels_002EItems_002ETabItemViewModel_002CXFolder_002ETitle_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build());
			context.ProvideTargetProperty = TextBlock.TextProperty;
			CompiledBinding binding21 = compiledBindingExtension25.ProvideValue(context);
			context.ProvideTargetProperty = null;
			textBlock5.Bind(textProperty, binding21);
			textBlock5.HorizontalAlignment = HorizontalAlignment.Center;
			textBlock5.VerticalAlignment = VerticalAlignment.Center;
			textBlock5.TextTrimming = TextTrimming.CharacterEllipsis;
			textBlock5.Margin = new Thickness(24.0, 0.0, 24.0, 0.0);
			context.PopParent();
			((ISupportInitialize)textBlock3).EndInit();
			Controls children2 = grid.Children;
			Button button2;
			Button button = (button2 = new Button());
			((ISupportInitialize)button).BeginInit();
			children2.Add(button);
			Button button4;
			Button button3 = (button4 = button2);
			context.PushParent(button4);
			button4.Classes.Add("CloseTabButton");
			StyledProperty<ICommand?> commandProperty8 = Button.CommandProperty;
			CompiledBindingExtension compiledBindingExtension26 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EViewModels_002EItems_002ETabItemViewModel_002CXFolder_002ECloseTabCommand_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build());
			context.ProvideTargetProperty = Button.CommandProperty;
			CompiledBinding binding22 = compiledBindingExtension26.ProvideValue(context);
			context.ProvideTargetProperty = null;
			button4.Bind(commandProperty8, binding22);
			StyledProperty<double> widthProperty = Layoutable.WidthProperty;
			CompiledBindingExtension compiledBindingExtension28;
			CompiledBindingExtension compiledBindingExtension27 = (compiledBindingExtension28 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Ancestor(typeof(TabItem), 0).Property(TemplatedControl.FontSizeProperty, PropertyInfoAccessorFactory.CreateAvaloniaPropertyAccessor).Build()));
			context.PushParent(compiledBindingExtension28);
			CompiledBindingExtension compiledBindingExtension29 = compiledBindingExtension28;
			StaticResourceExtension staticResourceExtension = new StaticResourceExtension("FontSizeToIconSizeConverter");
			context.ProvideTargetProperty = XamlIlHelpers.Avalonia_002EData_002ECompiledBinding_002CAvalonia_002EBase_002EConverter_0021Property();
			object? obj2 = staticResourceExtension.ProvideValue(context);
			context.ProvideTargetProperty = null;
			compiledBindingExtension29.Converter = (IValueConverter)obj2;
			compiledBindingExtension29.ConverterParameter = "1.23";
			context.PopParent();
			context.ProvideTargetProperty = Layoutable.WidthProperty;
			CompiledBinding binding23 = compiledBindingExtension27.ProvideValue(context);
			context.ProvideTargetProperty = null;
			button4.Bind(widthProperty, binding23);
			StyledProperty<double> heightProperty = Layoutable.HeightProperty;
			CompiledBindingExtension compiledBindingExtension30 = (compiledBindingExtension28 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Ancestor(typeof(TabItem), 0).Property(TemplatedControl.FontSizeProperty, PropertyInfoAccessorFactory.CreateAvaloniaPropertyAccessor).Build()));
			context.PushParent(compiledBindingExtension28);
			CompiledBindingExtension compiledBindingExtension31 = compiledBindingExtension28;
			StaticResourceExtension staticResourceExtension2 = new StaticResourceExtension("FontSizeToIconSizeConverter");
			context.ProvideTargetProperty = XamlIlHelpers.Avalonia_002EData_002ECompiledBinding_002CAvalonia_002EBase_002EConverter_0021Property();
			object? obj3 = staticResourceExtension2.ProvideValue(context);
			context.ProvideTargetProperty = null;
			compiledBindingExtension31.Converter = (IValueConverter)obj3;
			compiledBindingExtension31.ConverterParameter = "1.23";
			context.PopParent();
			context.ProvideTargetProperty = Layoutable.HeightProperty;
			CompiledBinding binding24 = compiledBindingExtension30.ProvideValue(context);
			context.ProvideTargetProperty = null;
			button4.Bind(heightProperty, binding24);
			button4.Padding = new Thickness(0.0, 0.0, 0.0, 0.0);
			button4.Margin = new Thickness(0.0, 0.0, 6.0, 0.0);
			button4.CornerRadius = new CornerRadius(4.0, 4.0, 4.0, 4.0);
			button4.Background = new ImmutableSolidColorBrush(16777215u);
			button4.BorderThickness = new Thickness(0.0, 0.0, 0.0, 0.0);
			button4.HorizontalAlignment = HorizontalAlignment.Right;
			button4.VerticalAlignment = VerticalAlignment.Center;
			button4.HorizontalContentAlignment = HorizontalAlignment.Center;
			button4.VerticalContentAlignment = VerticalAlignment.Center;
			Styles styles = button4.Styles;
			Style style;
			Style item = (style = new Style());
			context.PushParent(style);
			style.Selector = ((Selector?)null).OfType(typeof(Button)).Class(":pointerover");
			Setter setter2;
			Setter setter = (setter2 = new Setter());
			context.PushParent(setter2);
			setter2.Property = TemplatedControl.BackgroundProperty;
			DynamicResourceExtension dynamicResourceExtension4 = new DynamicResourceExtension("TabCloseButtonPointerOverBackground");
			context.ProvideTargetProperty = XamlIlHelpers.Avalonia_002EStyling_002ESetter_002CAvalonia_002EBase_002EValue_0021Property();
			BindingBase value = dynamicResourceExtension4.ProvideValue(context);
			context.ProvideTargetProperty = null;
			setter2.Value = value;
			context.PopParent();
			style.Add(setter);
			context.PopParent();
			styles.Add(item);
			TextBlock textBlock7;
			TextBlock textBlock6 = (textBlock7 = new TextBlock());
			((ISupportInitialize)textBlock6).BeginInit();
			button4.Content = textBlock6;
			TextBlock textBlock8 = (textBlock4 = textBlock7);
			context.PushParent(textBlock4);
			TextBlock textBlock9 = textBlock4;
			textBlock9.Text = "×";
			StyledProperty<double> fontSizeProperty = TextBlock.FontSizeProperty;
			CompiledBindingExtension compiledBindingExtension32 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Ancestor(typeof(TabItem), 0).Property(TemplatedControl.FontSizeProperty, PropertyInfoAccessorFactory.CreateAvaloniaPropertyAccessor).Build());
			context.ProvideTargetProperty = TextBlock.FontSizeProperty;
			CompiledBinding binding25 = compiledBindingExtension32.ProvideValue(context);
			context.ProvideTargetProperty = null;
			textBlock9.Bind(fontSizeProperty, binding25);
			textBlock9.Margin = new Thickness(0.0, -2.0, 0.0, 0.0);
			textBlock9.HorizontalAlignment = HorizontalAlignment.Center;
			textBlock9.VerticalAlignment = VerticalAlignment.Center;
			context.PopParent();
			((ISupportInitialize)textBlock8).EndInit();
			context.PopParent();
			((ISupportInitialize)button3).EndInit();
			Controls children3 = grid.Children;
			Rectangle rectangle2;
			Rectangle rectangle = (rectangle2 = new Rectangle());
			((ISupportInitialize)rectangle).BeginInit();
			children3.Add(rectangle);
			Rectangle rectangle4;
			Rectangle rectangle3 = (rectangle4 = rectangle2);
			context.PushParent(rectangle4);
			rectangle4.Width = 1.0;
			StyledProperty<double> heightProperty2 = Layoutable.HeightProperty;
			CompiledBindingExtension compiledBindingExtension33 = (compiledBindingExtension28 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Ancestor(typeof(TabItem), 0).Property(TemplatedControl.FontSizeProperty, PropertyInfoAccessorFactory.CreateAvaloniaPropertyAccessor).Build()));
			context.PushParent(compiledBindingExtension28);
			CompiledBindingExtension compiledBindingExtension34 = compiledBindingExtension28;
			StaticResourceExtension staticResourceExtension3 = new StaticResourceExtension("FontSizeToIconSizeConverter");
			context.ProvideTargetProperty = XamlIlHelpers.Avalonia_002EData_002ECompiledBinding_002CAvalonia_002EBase_002EConverter_0021Property();
			object? obj4 = staticResourceExtension3.ProvideValue(context);
			context.ProvideTargetProperty = null;
			compiledBindingExtension34.Converter = (IValueConverter)obj4;
			compiledBindingExtension34.ConverterParameter = "1.23";
			context.PopParent();
			context.ProvideTargetProperty = Layoutable.HeightProperty;
			CompiledBinding binding26 = compiledBindingExtension33.ProvideValue(context);
			context.ProvideTargetProperty = null;
			rectangle4.Bind(heightProperty2, binding26);
			StyledProperty<IBrush?> fillProperty4 = Shape.FillProperty;
			DynamicResourceExtension dynamicResourceExtension5 = new DynamicResourceExtension("TabHeaderBorder");
			context.ProvideTargetProperty = Shape.FillProperty;
			BindingBase binding27 = dynamicResourceExtension5.ProvideValue(context);
			context.ProvideTargetProperty = null;
			rectangle4.Bind(fillProperty4, binding27);
			rectangle4.HorizontalAlignment = HorizontalAlignment.Right;
			rectangle4.VerticalAlignment = VerticalAlignment.Center;
			StyledProperty<bool> isVisibleProperty = Visual.IsVisibleProperty;
			CompiledBindingExtension compiledBindingExtension35 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Not().Ancestor(typeof(TabItem), 0).Property(TabItem.IsSelectedProperty, PropertyInfoAccessorFactory.CreateAvaloniaPropertyAccessor)
				.Build());
			context.ProvideTargetProperty = Visual.IsVisibleProperty;
			CompiledBinding binding28 = compiledBindingExtension35.ProvideValue(context);
			context.ProvideTargetProperty = null;
			rectangle4.Bind(isVisibleProperty, binding28);
			context.PopParent();
			((ISupportInitialize)rectangle3).EndInit();
			context.PopParent();
			((ISupportInitialize)obj).EndInit();
			return obj;
		}

		public static object Build_4(IServiceProvider P_0)
		{
			XamlIlContext.Context<ModernShellView> context = CreateContext(P_0);
			context.IntermediateRoot = new Grid();
			object obj = context.IntermediateRoot;
			((ISupportInitialize)obj).BeginInit();
			Grid grid = (Grid)obj;
			context.PushParent(grid);
			Controls children = grid.Children;
			ContentControl contentControl2;
			ContentControl contentControl = (contentControl2 = new ContentControl());
			((ISupportInitialize)contentControl).BeginInit();
			children.Add(contentControl);
			ContentControl contentControl4;
			ContentControl contentControl3 = (contentControl4 = contentControl2);
			context.PushParent(contentControl4);
			contentControl4.Name = "LayoutHost";
			object element = contentControl4;
			context.AvaloniaNameScope.Register("LayoutHost", element);
			MultiBinding multiBinding2;
			MultiBinding multiBinding = (multiBinding2 = new MultiBinding());
			context.PushParent(multiBinding2);
			StaticResourceExtension staticResourceExtension = new StaticResourceExtension("LayoutModeToVMConverter");
			context.ProvideTargetProperty = XamlIlHelpers.Avalonia_002EData_002EMultiBinding_002CAvalonia_002EBase_002EConverter_0021Property();
			object? obj2 = staticResourceExtension.ProvideValue(context);
			context.ProvideTargetProperty = null;
			multiBinding2.Converter = (IMultiValueConverter)obj2;
			IList<BindingBase> bindings = multiBinding2.Bindings;
			CompiledBindingExtension compiledBindingExtension = new CompiledBindingExtension
			{
				Path = new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EViewModels_002EItems_002ETabItemViewModel_002CXFolder_002EFolderLayoutMode_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build()
			};
			context.ProvideTargetProperty = XamlIlHelpers.Avalonia_002EData_002EMultiBinding_002CAvalonia_002EBase_002EBindings_0021Property();
			CompiledBinding item = compiledBindingExtension.ProvideValue(context);
			context.ProvideTargetProperty = null;
			bindings.Add(item);
			IList<BindingBase> bindings2 = multiBinding2.Bindings;
			CompiledBindingExtension compiledBindingExtension2 = new CompiledBindingExtension
			{
				Path = new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EViewModels_002EItems_002ETabItemViewModel_002CXFolder_002ENavPath_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build()
			};
			context.ProvideTargetProperty = XamlIlHelpers.Avalonia_002EData_002EMultiBinding_002CAvalonia_002EBase_002EBindings_0021Property();
			CompiledBinding item2 = compiledBindingExtension2.ProvideValue(context);
			context.ProvideTargetProperty = null;
			bindings2.Add(item2);
			IList<BindingBase> bindings3 = multiBinding2.Bindings;
			CompiledBindingExtension compiledBindingExtension3 = new CompiledBindingExtension
			{
				Path = new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EViewModels_002EItems_002ETabItemViewModel_002CXFolder_002EMySelf_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build()
			};
			context.ProvideTargetProperty = XamlIlHelpers.Avalonia_002EData_002EMultiBinding_002CAvalonia_002EBase_002EBindings_0021Property();
			CompiledBinding item3 = compiledBindingExtension3.ProvideValue(context);
			context.ProvideTargetProperty = null;
			bindings3.Add(item3);
			IList<BindingBase> bindings4 = multiBinding2.Bindings;
			CompiledBindingExtension compiledBindingExtension4 = new CompiledBindingExtension(new CompiledBindingPathBuilder().ElementName(context.AvaloniaNameScope, "LayoutHost").Build());
			context.ProvideTargetProperty = XamlIlHelpers.Avalonia_002EData_002EMultiBinding_002CAvalonia_002EBase_002EBindings_0021Property();
			CompiledBinding item4 = compiledBindingExtension4.ProvideValue(context);
			context.ProvideTargetProperty = null;
			bindings4.Add(item4);
			context.PopParent();
			XamlDynamicSetters._003C_003EXamlDynamicSetter_14(contentControl4, multiBinding);
			context.PopParent();
			((ISupportInitialize)contentControl3).EndInit();
			context.PopParent();
			((ISupportInitialize)obj).EndInit();
			return obj;
		}
	}

	private readonly IMultiPanesContext multiPanesContext = Ioc.Default.GetRequiredService<IMultiPanesContext>();

	private readonly ICommandManager commandManager = Ioc.Default.GetRequiredService<ICommandManager>();

	private IRichCommand? _detailsLayoutCommand;

	private IRichCommand? _gridLayoutCommand;

	private TabItemViewModel? _observedSelectedTab;

	private ModernShellViewModel? _vm;

	private Border? _inactivePaneHeader;

	private Border? _activePaneHeader;

	private TextBlock? _inactivePaneHeaderText;

	private TextBlock? _activePaneHeaderText;

	private int _paneNumber = 1;

	private bool _showPaneHeader;

	private readonly List<Control> _paneContentControls = new List<Control>();

	[GeneratedCode("Avalonia.Generators.NameGenerator.InitializeComponentCodeGenerator", "12.0.1.0")]
	internal Grid RootGrid;

	[GeneratedCode("Avalonia.Generators.NameGenerator.InitializeComponentCodeGenerator", "12.0.1.0")]
	internal Button DetailsLayoutButton;

	[GeneratedCode("Avalonia.Generators.NameGenerator.InitializeComponentCodeGenerator", "12.0.1.0")]
	internal Button GridLayoutButton;

	[CompilerGenerated]
	private static Action<object> _0021XamlIlPopulateOverride;

	public ModernShellViewModel? ViewModel
	{
		get
		{
			if (_vm == null)
			{
				_vm = DataContext as ModernShellViewModel;
			}
			return _vm;
		}
	}

	public ModernShellView()
	{
		InitializeComponent();
		AddPaneActivityIndicator();
		Focusable = true;
		IsHitTestVisible = true;
		Loaded += ModernShellView_Loaded;
		AppLanguageHelper.LanguageChanged += AppLanguageHelper_LanguageChanged;
	}

	public int PaneNumber
	{
		get => _paneNumber;
		set
		{
			int paneNumber = Math.Max(1, value);
			if (_paneNumber == paneNumber)
			{
				return;
			}
			_paneNumber = paneNumber;
			UpdatePaneHeaderText();
		}
	}

	public bool ShowPaneHeader
	{
		get => _showPaneHeader;
		set
		{
			if (_showPaneHeader == value)
			{
				return;
			}
			_showPaneHeader = value;
			UpdatePaneActivityIndicator();
		}
	}

	private void AddPaneActivityIndicator()
	{
		RootGrid.RowDefinitions.Insert(0, new RowDefinition
		{
			Height = GridLength.Auto
		});
		foreach (Control child in RootGrid.Children)
		{
			Grid.SetRow(child, Grid.GetRow(child) + 1);
			_paneContentControls.Add(child);
		}

		Border paneBoundary = new Border
		{
			BorderThickness = new Thickness(1.0),
			CornerRadius = new CornerRadius(3.0),
			IsHitTestVisible = false,
			Margin = new Thickness(0.5),
			Opacity = 0.7
		};
		Grid.SetRowSpan(paneBoundary, RootGrid.RowDefinitions.Count);
		paneBoundary[!Border.BorderBrushProperty] = new DynamicResourceExtension("GridSplitterBackground");
		RootGrid.Children.Add(paneBoundary);

		_inactivePaneHeaderText = new TextBlock
		{
			FontSize = 11.0,
			FontWeight = FontWeight.SemiBold,
			Margin = new Thickness(10.0, 0.0, 0.0, 0.0),
			VerticalAlignment = VerticalAlignment.Center
		};
		_inactivePaneHeaderText[!TextBlock.ForegroundProperty] = new DynamicResourceExtension("SystemControlForegroundBaseMediumBrush");

		_inactivePaneHeader = new Border
		{
			IsHitTestVisible = false,
			BorderThickness = new Thickness(0.0, 0.0, 0.0, 1.0),
			Height = 24.0,
			IsVisible = false
		};
		_inactivePaneHeader[!Border.BackgroundProperty] = new DynamicResourceExtension("TopToolbarBackground");
		_inactivePaneHeader[!Border.BorderBrushProperty] = new DynamicResourceExtension("GridSplitterBackground");
		_inactivePaneHeader.Child = _inactivePaneHeaderText;
		Grid.SetRow(_inactivePaneHeader, 0);
		RootGrid.Children.Add(_inactivePaneHeader);

		_activePaneHeaderText = new TextBlock
		{
			FontSize = 11.0,
			FontWeight = FontWeight.SemiBold,
			Foreground = Brushes.White,
			Margin = new Thickness(10.0, 0.0, 0.0, 0.0),
			VerticalAlignment = VerticalAlignment.Center
		};

		_activePaneHeader = new Border
		{
			Background = new SolidColorBrush(Color.Parse("#0F766E")),
			Height = 24.0,
			IsHitTestVisible = false,
			IsVisible = false,
			Child = _activePaneHeaderText
		};
		Grid.SetRow(_activePaneHeader, 0);
		RootGrid.Children.Add(_activePaneHeader);

		UpdatePaneHeaderText();
	}

	private void ModernShellView_Loaded(object? sender, RoutedEventArgs e)
	{
		InstanceViewModel.FolderSettings.LayoutModeChangeRequested -= BaseFolderSettings_LayoutModeChangeRequested;
		InstanceViewModel.FolderSettings.LayoutModeChangeRequested += BaseFolderSettings_LayoutModeChangeRequested;
		if (ViewModel != null)
		{
			ViewModel.PropertyChanged -= ViewModelPropertyChanged;
			ViewModel.PropertyChanged += ViewModelPropertyChanged;
			ObserveSelectedTab(ViewModel.SelectedTab);
		}
		UpdatePaneActivityIndicator();
		InitializeLayoutCommands();
		UpdateLayoutButtonsState();
		UpdateLayoutButtonsToolTips();
	}

	private void ViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
	{
		if (e.PropertyName == "SelectedTab")
		{
			ObserveSelectedTab(ViewModel?.SelectedTab);
			UpdateTabInfo();
		}
		else if (e.PropertyName == "WorkingDirectory" && ViewModel?.SelectedTab != null && !string.IsNullOrEmpty(ViewModel.WorkingDirectory))
		{
			ViewModel.SelectedTab.NavPath = ViewModel.WorkingDirectory;
			multiPanesContext.SidebarView?.RequestOpenDirectory(ViewModel.WorkingDirectory);
		}
		else if (e.PropertyName == nameof(ModernShellViewModel.IsCurrentInstance))
		{
			UpdatePaneActivityIndicator();
		}
	}

	private void UpdatePaneActivityIndicator()
	{
		bool isActive = ViewModel?.IsCurrentInstance == true;
		double contentOpacity = (_showPaneHeader && !isActive) ? 0.42 : 1.0;
		foreach (Control contentControl in _paneContentControls)
		{
			contentControl.Opacity = contentOpacity;
		}
		if (_inactivePaneHeader != null)
		{
			_inactivePaneHeader.IsVisible = _showPaneHeader && !isActive;
		}
		if (_activePaneHeader != null)
		{
			_activePaneHeader.IsVisible = _showPaneHeader && isActive;
		}
	}

	private void UpdatePaneHeaderText()
	{
		if (_inactivePaneHeaderText != null)
		{
			_inactivePaneHeaderText.Text = $"PANE {_paneNumber}";
		}
		if (_activePaneHeaderText != null)
		{
			_activePaneHeaderText.Text = $"CURRENT  ·  PANE {_paneNumber}";
		}
	}

	public override void Dispose()
	{
		base.Dispose();
		AppLanguageHelper.LanguageChanged -= AppLanguageHelper_LanguageChanged;
		if (InstanceViewModel?.FolderSettings != null)
		{
			InstanceViewModel.FolderSettings.LayoutModeChangeRequested -= BaseFolderSettings_LayoutModeChangeRequested;
		}
		if (ViewModel != null)
		{
			ViewModel.PropertyChanged -= ViewModelPropertyChanged;
		}
		if (_observedSelectedTab != null)
		{
			_observedSelectedTab.PropertyChanged -= SelectedTab_PropertyChanged;
		}
		if (_detailsLayoutCommand != null)
		{
			_detailsLayoutCommand.PropertyChanged -= LayoutCommand_PropertyChanged;
		}
		if (_gridLayoutCommand != null)
		{
			_gridLayoutCommand.PropertyChanged -= LayoutCommand_PropertyChanged;
		}
	}

	protected virtual void BaseFolderSettings_LayoutModeChangeRequested(object? sender, LayoutModeEventArgs e)
	{
		Type currentLayoutType = InstanceViewModel.FolderSettings.GetCurrentLayoutType();
		SwitchLayout(currentLayoutType);
	}

	private void UpdateTabInfo()
	{
		if (ViewModel?.SelectedTab != null)
		{
			int value = ViewModel.TabItems.IndexOf(ViewModel.SelectedTab);
			Console.WriteLine($"当前选中的标签：{ViewModel.SelectedTab.Title}，索引：{value}");
		}
	}

	private void InitializeLayoutCommands()
	{
		if (_detailsLayoutCommand == null)
		{
			_detailsLayoutCommand = commandManager[CommandCodes.LayoutDetails];
		}
		if (_gridLayoutCommand == null)
		{
			_gridLayoutCommand = commandManager[CommandCodes.LayoutGrid];
		}
		_detailsLayoutCommand.PropertyChanged -= LayoutCommand_PropertyChanged;
		_detailsLayoutCommand.PropertyChanged += LayoutCommand_PropertyChanged;
		_gridLayoutCommand.PropertyChanged -= LayoutCommand_PropertyChanged;
		_gridLayoutCommand.PropertyChanged += LayoutCommand_PropertyChanged;
	}

	private void ObserveSelectedTab(TabItemViewModel? selectedTab)
	{
		if (_observedSelectedTab == selectedTab)
		{
			UpdateLayoutButtonsState();
			return;
		}
		if (_observedSelectedTab != null)
		{
			_observedSelectedTab.PropertyChanged -= SelectedTab_PropertyChanged;
		}
		_observedSelectedTab = selectedTab;
		if (_observedSelectedTab != null)
		{
			_observedSelectedTab.PropertyChanged += SelectedTab_PropertyChanged;
		}
		UpdateLayoutButtonsState();
	}

	private void SelectedTab_PropertyChanged(object? sender, PropertyChangedEventArgs e)
	{
		if (e.PropertyName == "FolderLayoutMode")
		{
			UpdateLayoutButtonsState();
		}
	}

	private void LayoutCommand_PropertyChanged(object? sender, PropertyChangedEventArgs e)
	{
		string propertyName = e.PropertyName;
		if ((propertyName == "HotKeys" || propertyName == "Description") ? true : false)
		{
			UpdateLayoutButtonsToolTips();
		}
	}

	private void AppLanguageHelper_LanguageChanged(object? sender, EventArgs e)
	{
		UpdateLayoutButtonsToolTips();
	}

	private static string? GetLayoutButtonToolTip(IRichCommand? command)
	{
		if (command == null)
		{
			return null;
		}
		string primaryMacDisplayText = ShortcutHelpers.GetPrimaryMacDisplayText(command.HotKeys);
		if (string.IsNullOrWhiteSpace(primaryMacDisplayText))
		{
			return command.Description;
		}
		return command.Description + "\n" + primaryMacDisplayText;
	}

	private void UpdateLayoutButtonsState()
	{
		FolderLayoutModes? folderLayoutModes = ViewModel?.SelectedTab?.FolderLayoutMode;
		DetailsLayoutButton.Classes.Set("selected", folderLayoutModes == FolderLayoutModes.DetailsView);
		GridLayoutButton.Classes.Set("selected", folderLayoutModes == FolderLayoutModes.GridView);
	}

	private void UpdateLayoutButtonsToolTips()
	{
		ToolTip.SetTip(DetailsLayoutButton, GetLayoutButtonToolTip(_detailsLayoutCommand));
		ToolTip.SetTip(GridLayoutButton, GetLayoutButtonToolTip(_gridLayoutCommand));
	}

	private void SwitchLayout(Type layoutType)
	{
		if (InstanceViewModel is ModernShellViewModel modernShellViewModel)
		{
			if (layoutType == typeof(DetailsLayoutView))
			{
				modernShellViewModel.SelectedTab.FolderLayoutMode = FolderLayoutModes.DetailsView;
			}
			else if (layoutType == typeof(GridLayoutView))
			{
				modernShellViewModel.SelectedTab.FolderLayoutMode = FolderLayoutModes.GridView;
			}
		}
	}

	private async void OnRunActionCommand(object? sender, RoutedEventArgs e)
	{
		if (sender is Control { Tag: string tag })
		{
			IRichCommand richCommand = Ioc.Default.GetRequiredService<ICommandManager>()[tag];
			if (richCommand.IsExecutable)
			{
				await richCommand.ExecuteAsync();
			}
		}
	}

	private void TabHeader_OnPointerPressed(object? sender, PointerPressedEventArgs e)
	{
		if (e.GetCurrentPoint(null).Properties.IsMiddleButtonPressed && sender is Control { DataContext: TabItemViewModel dataContext })
		{
			dataContext.CloseTabCommand.Execute(null);
			e.Handled = true;
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
		RootGrid = ((nameScope != null) ? nameScope.Find<Grid>("RootGrid") : null);
		DetailsLayoutButton = ((nameScope != null) ? nameScope.Find<Button>("DetailsLayoutButton") : null);
		GridLayoutButton = ((nameScope != null) ? nameScope.Find<Button>("GridLayoutButton") : null);
	}

	[CompilerGenerated]
	private unsafe static void _0021XamlIlPopulate(IServiceProvider P_0, ModernShellView P_1)
	{
		XamlIlContext.Context<ModernShellView> context = new XamlIlContext.Context<ModernShellView>(P_0, new object[1] { _0021AvaloniaResources.NamespaceInfo_003A_002FViews_002FShells_002FModernShellView_002Eaxaml.Singleton }, "avares://XFolder/Views/Shells/ModernShellView.axaml")
		{
			RootObject = P_1,
			IntermediateRoot = P_1
		};
		((ISupportInitialize)P_1).BeginInit();
		context.PushParent(P_1);
		((ResourceDictionary)P_1.Resources).AddDeferred((object)"FontSizeToIconSizeConverter", XamlIlRuntimeHelpers.DeferredTransformationFactoryV3<object>((nint)(delegate*<IServiceProvider, object>)(&XamlClosure_11.Build_1), context));
		Grid grid2;
		Grid grid = (grid2 = new Grid());
		((ISupportInitialize)grid).BeginInit();
		P_1.Content = grid;
		Grid grid4;
		Grid grid3 = (grid4 = grid2);
		context.PushParent(grid4);
		Grid grid5 = grid4;
		grid5.Name = "RootGrid";
		object element = grid5;
		context.AvaloniaNameScope.Register("RootGrid", element);
		StyledProperty<IBrush?> backgroundProperty = Panel.BackgroundProperty;
		MultiBinding multiBinding;
		MultiBinding binding = (multiBinding = new MultiBinding());
		context.PushParent(multiBinding);
		MultiBinding multiBinding2 = multiBinding;
		StaticResourceExtension staticResourceExtension = new StaticResourceExtension("PaneBackgroundConverter");
		context.ProvideTargetProperty = XamlIlHelpers.Avalonia_002EData_002EMultiBinding_002CAvalonia_002EBase_002EConverter_0021Property();
		object? obj = staticResourceExtension.ProvideValue(context);
		context.ProvideTargetProperty = null;
		multiBinding2.Converter = (IMultiValueConverter)obj;
		IList<BindingBase> bindings = multiBinding2.Bindings;
		CompiledBindingExtension compiledBindingExtension = new CompiledBindingExtension
		{
			Path = new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EViewModels_002EShells_002EModernShellViewModel_002CXFolder_002EIsCurrentInstance_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build()
		};
		context.ProvideTargetProperty = XamlIlHelpers.Avalonia_002EData_002EMultiBinding_002CAvalonia_002EBase_002EBindings_0021Property();
		CompiledBinding item = compiledBindingExtension.ProvideValue(context);
		context.ProvideTargetProperty = null;
		bindings.Add(item);
		IList<BindingBase> bindings2 = multiBinding2.Bindings;
		CompiledBindingExtension compiledBindingExtension2 = new CompiledBindingExtension
		{
			Path = new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EViewModels_002EShells_002EModernShellViewModel_002CXFolder_002ECurrentThemeVariant_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build()
		};
		context.ProvideTargetProperty = XamlIlHelpers.Avalonia_002EData_002EMultiBinding_002CAvalonia_002EBase_002EBindings_0021Property();
		CompiledBinding item2 = compiledBindingExtension2.ProvideValue(context);
		context.ProvideTargetProperty = null;
		bindings2.Add(item2);
		context.PopParent();
		grid5.Bind(backgroundProperty, binding);
		grid5.RowDefinitions.Add(new RowDefinition
		{
			Height = new GridLength(0.0, GridUnitType.Auto)
		});
		grid5.RowDefinitions.Add(new RowDefinition
		{
			Height = new GridLength(1.0, GridUnitType.Star)
		});
		grid5.RowDefinitions.Add(new RowDefinition
		{
			Height = new GridLength(0.0, GridUnitType.Auto)
		});
		grid5.RowDefinitions.Add(new RowDefinition
		{
			Height = new GridLength(0.0, GridUnitType.Auto)
		});
		Controls children = grid5.Children;
		NavigationToolbarView navigationToolbarView2;
		NavigationToolbarView navigationToolbarView = (navigationToolbarView2 = new NavigationToolbarView());
		((ISupportInitialize)navigationToolbarView).BeginInit();
		children.Add(navigationToolbarView);
		NavigationToolbarView navigationToolbarView4;
		NavigationToolbarView navigationToolbarView3 = (navigationToolbarView4 = navigationToolbarView2);
		context.PushParent(navigationToolbarView4);
		Grid.SetRow(navigationToolbarView4, 0);
		CompiledBindingExtension compiledBindingExtension3 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EViewModels_002EShells_002EBaseShellViewModel_002CXFolder_002ENavigationToolbarViewModel_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build());
		context.ProvideTargetProperty = StyledElement.DataContextProperty;
		CompiledBinding compiledBinding = compiledBindingExtension3.ProvideValue(context);
		context.ProvideTargetProperty = null;
		XamlDynamicSetters._003C_003EXamlDynamicSetter_11(navigationToolbarView4, compiledBinding);
		context.PopParent();
		((ISupportInitialize)navigationToolbarView3).EndInit();
		Controls children2 = grid5.Children;
		Grid grid7;
		Grid grid6 = (grid7 = new Grid());
		((ISupportInitialize)grid6).BeginInit();
		children2.Add(grid6);
		Grid grid8 = (grid4 = grid7);
		context.PushParent(grid4);
		Grid grid9 = grid4;
		Grid.SetRow(grid9, 1);
		Controls children3 = grid9.Children;
		TabControl tabControl2;
		TabControl tabControl = (tabControl2 = new TabControl());
		((ISupportInitialize)tabControl).BeginInit();
		children3.Add(tabControl);
		TabControl tabControl4;
		TabControl tabControl3 = (tabControl4 = tabControl2);
		context.PushParent(tabControl4);
		StyledProperty<IEnumerable?> itemsSourceProperty = ItemsControl.ItemsSourceProperty;
		CompiledBindingExtension compiledBindingExtension4 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EViewModels_002EShells_002EBaseShellViewModel_002CXFolder_002ETabItems_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build());
		context.ProvideTargetProperty = ItemsControl.ItemsSourceProperty;
		CompiledBinding binding2 = compiledBindingExtension4.ProvideValue(context);
		context.ProvideTargetProperty = null;
		tabControl4.Bind(itemsSourceProperty, binding2);
		CompiledBindingExtension compiledBindingExtension5 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EViewModels_002EShells_002EBaseShellViewModel_002CXFolder_002ESelectedTab_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build());
		context.ProvideTargetProperty = SelectingItemsControl.SelectedItemProperty;
		CompiledBinding compiledBinding2 = compiledBindingExtension5.ProvideValue(context);
		context.ProvideTargetProperty = null;
		XamlDynamicSetters._003C_003EXamlDynamicSetter_12(tabControl4, compiledBinding2);
		tabControl4.Padding = new Thickness(0.0, 0.0, 0.0, 0.0);
		tabControl4.Margin = new Thickness(0.0, 0.0, 0.0, 0.0);
		Styles styles = tabControl4.Styles;
		Style style;
		Style item3 = (style = new Style());
		context.PushParent(style);
		Style style2 = style;
		style2.Selector = ((Selector?)null).OfType(typeof(TabItem));
		Setter setter2;
		Setter setter = (setter2 = new Setter());
		context.PushParent(setter2);
		Setter setter3 = setter2;
		setter3.Property = Layoutable.MinHeightProperty;
		CompiledBindingExtension compiledBindingExtension7;
		CompiledBindingExtension compiledBindingExtension6 = (compiledBindingExtension7 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Self().Property(TemplatedControl.FontSizeProperty, PropertyInfoAccessorFactory.CreateAvaloniaPropertyAccessor).Build()));
		context.PushParent(compiledBindingExtension7);
		StaticResourceExtension staticResourceExtension2 = new StaticResourceExtension("FontSizeToIconSizeConverter");
		context.ProvideTargetProperty = XamlIlHelpers.Avalonia_002EData_002ECompiledBinding_002CAvalonia_002EBase_002EConverter_0021Property();
		object? obj2 = staticResourceExtension2.ProvideValue(context);
		context.ProvideTargetProperty = null;
		compiledBindingExtension7.Converter = (IValueConverter)obj2;
		compiledBindingExtension7.ConverterParameter = "2";
		context.PopParent();
		context.ProvideTargetProperty = XamlIlHelpers.Avalonia_002EStyling_002ESetter_002CAvalonia_002EBase_002EValue_0021Property();
		CompiledBinding value = compiledBindingExtension6.ProvideValue(context);
		context.ProvideTargetProperty = null;
		setter3.Value = value;
		context.PopParent();
		style2.Add(setter);
		Setter setter4 = new Setter();
		setter4.Property = Layoutable.MarginProperty;
		setter4.Value = new Thickness(0.0, 0.0, 0.0, 0.0);
		style2.Add(setter4);
		Setter setter5 = new Setter();
		setter5.Property = TemplatedControl.PaddingProperty;
		setter5.Value = new Thickness(0.0, 0.0, 0.0, 0.0);
		style2.Add(setter5);
		Setter setter6 = new Setter();
		setter6.Property = TemplatedControl.CornerRadiusProperty;
		setter6.Value = new CornerRadius(0.0, 0.0, 0.0, 0.0);
		style2.Add(setter6);
		Setter setter7 = new Setter();
		setter7.Property = TemplatedControl.BorderThicknessProperty;
		setter7.Value = new Thickness(0.0, 0.0, 0.0, 0.0);
		style2.Add(setter7);
		Setter setter8 = (setter2 = new Setter());
		context.PushParent(setter2);
		Setter setter9 = setter2;
		setter9.Property = TemplatedControl.BackgroundProperty;
		DynamicResourceExtension dynamicResourceExtension = new DynamicResourceExtension("TabHeaderBackground");
		context.ProvideTargetProperty = XamlIlHelpers.Avalonia_002EStyling_002ESetter_002CAvalonia_002EBase_002EValue_0021Property();
		BindingBase value2 = dynamicResourceExtension.ProvideValue(context);
		context.ProvideTargetProperty = null;
		setter9.Value = value2;
		context.PopParent();
		style2.Add(setter8);
		Setter setter10 = (setter2 = new Setter());
		context.PushParent(setter2);
		Setter setter11 = setter2;
		setter11.Property = TemplatedControl.ForegroundProperty;
		DynamicResourceExtension dynamicResourceExtension2 = new DynamicResourceExtension("TabHeaderForeground");
		context.ProvideTargetProperty = XamlIlHelpers.Avalonia_002EStyling_002ESetter_002CAvalonia_002EBase_002EValue_0021Property();
		BindingBase value3 = dynamicResourceExtension2.ProvideValue(context);
		context.ProvideTargetProperty = null;
		setter11.Value = value3;
		context.PopParent();
		style2.Add(setter10);
		Setter setter12 = (setter2 = new Setter());
		context.PushParent(setter2);
		Setter setter13 = setter2;
		setter13.Property = TemplatedControl.FontSizeProperty;
		DynamicResourceExtension dynamicResourceExtension3 = new DynamicResourceExtension("GlobalFontSize");
		context.ProvideTargetProperty = XamlIlHelpers.Avalonia_002EStyling_002ESetter_002CAvalonia_002EBase_002EValue_0021Property();
		BindingBase value4 = dynamicResourceExtension3.ProvideValue(context);
		context.ProvideTargetProperty = null;
		setter13.Value = value4;
		context.PopParent();
		style2.Add(setter12);
		context.PopParent();
		styles.Add(item3);
		Styles styles2 = tabControl4.Styles;
		Style style3 = new Style();
		style3.Selector = ((Selector?)null).OfType(typeof(TabItem)).Class(":selected").Template()
			.OfType(typeof(Border))
			.Name("PART_SelectedPipe");
		Setter setter14 = new Setter();
		setter14.Property = Layoutable.VerticalAlignmentProperty;
		setter14.Value = VerticalAlignment.Bottom;
		style3.Add(setter14);
		Setter setter15 = new Setter();
		setter15.Property = Layoutable.MarginProperty;
		setter15.Value = new Thickness(0.0, 0.0, 0.0, 0.0);
		style3.Add(setter15);
		Setter setter16 = new Setter();
		setter16.Property = Layoutable.HeightProperty;
		setter16.Value = 2.0;
		style3.Add(setter16);
		styles2.Add(style3);
		Styles styles3 = tabControl4.Styles;
		Style item4 = (style = new Style());
		context.PushParent(style);
		Style style4 = style;
		style4.Selector = ((Selector?)null).OfType(typeof(TabItem)).Class(":selected");
		Setter setter17 = (setter2 = new Setter());
		context.PushParent(setter2);
		Setter setter18 = setter2;
		setter18.Property = TemplatedControl.BackgroundProperty;
		DynamicResourceExtension dynamicResourceExtension4 = new DynamicResourceExtension("TabHeaderSelectedBackground");
		context.ProvideTargetProperty = XamlIlHelpers.Avalonia_002EStyling_002ESetter_002CAvalonia_002EBase_002EValue_0021Property();
		BindingBase value5 = dynamicResourceExtension4.ProvideValue(context);
		context.ProvideTargetProperty = null;
		setter18.Value = value5;
		context.PopParent();
		style4.Add(setter17);
		Setter setter19 = (setter2 = new Setter());
		context.PushParent(setter2);
		Setter setter20 = setter2;
		setter20.Property = TemplatedControl.ForegroundProperty;
		DynamicResourceExtension dynamicResourceExtension5 = new DynamicResourceExtension("TabHeaderSelectedForeground");
		context.ProvideTargetProperty = XamlIlHelpers.Avalonia_002EStyling_002ESetter_002CAvalonia_002EBase_002EValue_0021Property();
		BindingBase value6 = dynamicResourceExtension5.ProvideValue(context);
		context.ProvideTargetProperty = null;
		setter20.Value = value6;
		context.PopParent();
		style4.Add(setter19);
		context.PopParent();
		styles3.Add(item4);
		Styles styles4 = tabControl4.Styles;
		Style item5 = (style = new Style());
		context.PushParent(style);
		Style style5 = style;
		style5.Selector = ((Selector?)null).OfType(typeof(TabItem)).Class(":pointerover");
		Setter setter21 = (setter2 = new Setter());
		context.PushParent(setter2);
		Setter setter22 = setter2;
		setter22.Property = TemplatedControl.BackgroundProperty;
		DynamicResourceExtension dynamicResourceExtension6 = new DynamicResourceExtension("TabHeaderPointerOverBackground");
		context.ProvideTargetProperty = XamlIlHelpers.Avalonia_002EStyling_002ESetter_002CAvalonia_002EBase_002EValue_0021Property();
		BindingBase value7 = dynamicResourceExtension6.ProvideValue(context);
		context.ProvideTargetProperty = null;
		setter22.Value = value7;
		context.PopParent();
		style5.Add(setter21);
		Setter setter23 = (setter2 = new Setter());
		context.PushParent(setter2);
		Setter setter24 = setter2;
		setter24.Property = TemplatedControl.ForegroundProperty;
		DynamicResourceExtension dynamicResourceExtension7 = new DynamicResourceExtension("TabHeaderPointerOverForeground");
		context.ProvideTargetProperty = XamlIlHelpers.Avalonia_002EStyling_002ESetter_002CAvalonia_002EBase_002EValue_0021Property();
		BindingBase value8 = dynamicResourceExtension7.ProvideValue(context);
		context.ProvideTargetProperty = null;
		setter24.Value = value8;
		context.PopParent();
		style5.Add(setter23);
		context.PopParent();
		styles4.Add(item5);
		Styles styles5 = tabControl4.Styles;
		Style style6 = new Style();
		style6.Selector = ((Selector?)null).OfType(typeof(TabItem)).Descendant().OfType(typeof(Button))
			.Class("CloseTabButton");
		Setter setter25 = new Setter();
		setter25.Property = Visual.OpacityProperty;
		setter25.Value = 0.0;
		style6.Add(setter25);
		Setter setter26 = new Setter();
		setter26.Property = InputElement.IsHitTestVisibleProperty;
		setter26.Value = false;
		style6.Add(setter26);
		styles5.Add(style6);
		Styles styles6 = tabControl4.Styles;
		Style style7 = new Style();
		style7.Selector = ((Selector?)null).OfType(typeof(TabItem)).Class(":pointerover").Descendant()
			.OfType(typeof(Button))
			.Class("CloseTabButton");
		Setter setter27 = new Setter();
		setter27.Property = Visual.OpacityProperty;
		setter27.Value = 1.0;
		style7.Add(setter27);
		Setter setter28 = new Setter();
		setter28.Property = InputElement.IsHitTestVisibleProperty;
		setter28.Value = true;
		style7.Add(setter28);
		styles6.Add(style7);
		tabControl4.ItemsPanel = new ItemsPanelTemplate
		{
			Content = XamlIlRuntimeHelpers.DeferredTransformationFactoryV3<Control>((nint)(delegate*<IServiceProvider, object>)(&XamlClosure_11.Build_2), context)
		};
		DataTemplate dataTemplate;
		DataTemplate itemTemplate = (dataTemplate = new DataTemplate());
		context.PushParent(dataTemplate);
		DataTemplate dataTemplate2 = dataTemplate;
		dataTemplate2.DataType = typeof(TabItemViewModel);
		dataTemplate2.Content = XamlIlRuntimeHelpers.DeferredTransformationFactoryV3<Control>((nint)(delegate*<IServiceProvider, object>)(&XamlClosure_11.Build_3), context);
		context.PopParent();
		tabControl4.ItemTemplate = itemTemplate;
		DataTemplate contentTemplate = (dataTemplate = new DataTemplate());
		context.PushParent(dataTemplate);
		DataTemplate dataTemplate3 = dataTemplate;
		dataTemplate3.DataType = typeof(TabItemViewModel);
		dataTemplate3.Content = XamlIlRuntimeHelpers.DeferredTransformationFactoryV3<Control>((nint)(delegate*<IServiceProvider, object>)(&XamlClosure_11.Build_4), context);
		context.PopParent();
		tabControl4.ContentTemplate = contentTemplate;
		context.PopParent();
		((ISupportInitialize)tabControl3).EndInit();
		context.PopParent();
		((ISupportInitialize)grid8).EndInit();
		Controls children4 = grid5.Children;
		Border border2;
		Border border = (border2 = new Border());
		((ISupportInitialize)border).BeginInit();
		children4.Add(border);
		Border border4;
		Border border3 = (border4 = border2);
		context.PushParent(border4);
		Grid.SetRow(border4, 2);
		border4.Height = 1.0;
		StyledProperty<IBrush?> backgroundProperty2 = Border.BackgroundProperty;
		DynamicResourceExtension dynamicResourceExtension8 = new DynamicResourceExtension("TopToolbarBackground");
		context.ProvideTargetProperty = Border.BackgroundProperty;
		BindingBase binding3 = dynamicResourceExtension8.ProvideValue(context);
		context.ProvideTargetProperty = null;
		border4.Bind(backgroundProperty2, binding3);
		border4.Opacity = 0.2;
		context.PopParent();
		((ISupportInitialize)border3).EndInit();
		Controls children5 = grid5.Children;
		Grid grid11;
		Grid grid10 = (grid11 = new Grid());
		((ISupportInitialize)grid10).BeginInit();
		children5.Add(grid10);
		Grid grid12 = (grid4 = grid11);
		context.PushParent(grid4);
		Grid grid13 = grid4;
		Grid.SetRow(grid13, 3);
		grid13.Height = 20.0;
		grid13.ColumnDefinitions.Add(new ColumnDefinition
		{
			Width = new GridLength(1.0, GridUnitType.Star)
		});
		grid13.ColumnDefinitions.Add(new ColumnDefinition
		{
			Width = new GridLength(0.0, GridUnitType.Auto)
		});
		grid13.ColumnDefinitions.Add(new ColumnDefinition
		{
			Width = new GridLength(0.0, GridUnitType.Auto)
		});
		grid13.ColumnDefinitions.Add(new ColumnDefinition
		{
			Width = new GridLength(0.0, GridUnitType.Auto)
		});
		Controls children6 = grid13.Children;
		TextBlock textBlock2;
		TextBlock textBlock = (textBlock2 = new TextBlock());
		((ISupportInitialize)textBlock).BeginInit();
		children6.Add(textBlock);
		TextBlock textBlock4;
		TextBlock textBlock3 = (textBlock4 = textBlock2);
		context.PushParent(textBlock4);
		TextBlock textBlock5 = textBlock4;
		Grid.SetColumn(textBlock5, 0);
		textBlock5.Margin = new Thickness(8.0, 0.0, 0.0, 0.0);
		textBlock5.VerticalAlignment = VerticalAlignment.Center;
		textBlock5.FontSize = 12.0;
		StyledProperty<IBrush?> foregroundProperty = TextBlock.ForegroundProperty;
		DynamicResourceExtension dynamicResourceExtension9 = new DynamicResourceExtension("SystemControlForegroundBaseMediumBrush");
		context.ProvideTargetProperty = TextBlock.ForegroundProperty;
		BindingBase binding4 = dynamicResourceExtension9.ProvideValue(context);
		context.ProvideTargetProperty = null;
		textBlock5.Bind(foregroundProperty, binding4);
		textBlock5.TextTrimming = TextTrimming.CharacterEllipsis;
		StyledProperty<string?> textProperty = TextBlock.TextProperty;
		MultiBinding binding5 = (multiBinding = new MultiBinding());
		context.PushParent(multiBinding);
		MultiBinding multiBinding3 = multiBinding;
		StaticResourceExtension staticResourceExtension3 = new StaticResourceExtension("LocalizedCountTextConverter");
		context.ProvideTargetProperty = XamlIlHelpers.Avalonia_002EData_002EMultiBinding_002CAvalonia_002EBase_002EConverter_0021Property();
		object? obj3 = staticResourceExtension3.ProvideValue(context);
		context.ProvideTargetProperty = null;
		multiBinding3.Converter = (IMultiValueConverter)obj3;
		IList<BindingBase> bindings3 = multiBinding3.Bindings;
		CompiledBindingExtension compiledBindingExtension8 = new CompiledBindingExtension
		{
			Path = new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EViewModels_002EShells_002EBaseShellViewModel_002CXFolder_002ESelectedTab_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Property(XamlIlHelpers.Files_002EViewModels_002EItems_002ETabItemViewModel_002CXFolder_002EItemCount_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build()
		};
		context.ProvideTargetProperty = XamlIlHelpers.Avalonia_002EData_002EMultiBinding_002CAvalonia_002EBase_002EBindings_0021Property();
		CompiledBinding item6 = compiledBindingExtension8.ProvideValue(context);
		context.ProvideTargetProperty = null;
		bindings3.Add(item6);
		IList<BindingBase> bindings4 = multiBinding3.Bindings;
		CompiledBindingExtension compiledBindingExtension9 = new CompiledBindingExtension();
		compiledBindingExtension9.Path = new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EServices_002EApp_002ELocalizationBindingSource_002CXFolder_002EItem_005BGroupItemsCount_Singular_005D_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build();
		compiledBindingExtension9.Source = LocalizationBindingSource.Instance;
		context.ProvideTargetProperty = XamlIlHelpers.Avalonia_002EData_002EMultiBinding_002CAvalonia_002EBase_002EBindings_0021Property();
		CompiledBinding item7 = compiledBindingExtension9.ProvideValue(context);
		context.ProvideTargetProperty = null;
		bindings4.Add(item7);
		IList<BindingBase> bindings5 = multiBinding3.Bindings;
		CompiledBindingExtension compiledBindingExtension10 = new CompiledBindingExtension();
		compiledBindingExtension10.Path = new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EServices_002EApp_002ELocalizationBindingSource_002CXFolder_002EItem_005BGroupItemsCount_Plural_005D_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build();
		compiledBindingExtension10.Source = LocalizationBindingSource.Instance;
		context.ProvideTargetProperty = XamlIlHelpers.Avalonia_002EData_002EMultiBinding_002CAvalonia_002EBase_002EBindings_0021Property();
		CompiledBinding item8 = compiledBindingExtension10.ProvideValue(context);
		context.ProvideTargetProperty = null;
		bindings5.Add(item8);
		IList<BindingBase> bindings6 = multiBinding3.Bindings;
		CompiledBindingExtension compiledBindingExtension11 = new CompiledBindingExtension
		{
			Path = new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EViewModels_002EShells_002EBaseShellViewModel_002CXFolder_002ESelectedTab_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Property(XamlIlHelpers.Files_002EViewModels_002EItems_002ETabItemViewModel_002CXFolder_002ESelectedItems_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Property(XamlIlHelpers.System_002ECollections_002EObjectModel_002ECollection_00601_002CSystem_002ERuntime_002ECount_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor)
				.Build()
		};
		context.ProvideTargetProperty = XamlIlHelpers.Avalonia_002EData_002EMultiBinding_002CAvalonia_002EBase_002EBindings_0021Property();
		CompiledBinding item9 = compiledBindingExtension11.ProvideValue(context);
		context.ProvideTargetProperty = null;
		bindings6.Add(item9);
		IList<BindingBase> bindings7 = multiBinding3.Bindings;
		CompiledBindingExtension compiledBindingExtension12 = new CompiledBindingExtension();
		compiledBindingExtension12.Path = new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EServices_002EApp_002ELocalizationBindingSource_002CXFolder_002EItem_005BItemsSelectedCountFormat_005D_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build();
		compiledBindingExtension12.Source = LocalizationBindingSource.Instance;
		context.ProvideTargetProperty = XamlIlHelpers.Avalonia_002EData_002EMultiBinding_002CAvalonia_002EBase_002EBindings_0021Property();
		CompiledBinding item10 = compiledBindingExtension12.ProvideValue(context);
		context.ProvideTargetProperty = null;
		bindings7.Add(item10);
		context.PopParent();
		textBlock5.Bind(textProperty, binding5);
		context.PopParent();
		((ISupportInitialize)textBlock3).EndInit();
		Controls children7 = grid13.Children;
		TextBlock textBlock7;
		TextBlock textBlock6 = (textBlock7 = new TextBlock());
		((ISupportInitialize)textBlock6).BeginInit();
		children7.Add(textBlock6);
		TextBlock textBlock8 = (textBlock4 = textBlock7);
		context.PushParent(textBlock4);
		TextBlock textBlock9 = textBlock4;
		Grid.SetColumn(textBlock9, 1);
		textBlock9.Margin = new Thickness(0.0, 0.0, 8.0, 0.0);
		textBlock9.VerticalAlignment = VerticalAlignment.Center;
		textBlock9.FontSize = 12.0;
		StyledProperty<IBrush?> foregroundProperty2 = TextBlock.ForegroundProperty;
		DynamicResourceExtension dynamicResourceExtension10 = new DynamicResourceExtension("SystemControlForegroundBaseMediumBrush");
		context.ProvideTargetProperty = TextBlock.ForegroundProperty;
		BindingBase binding6 = dynamicResourceExtension10.ProvideValue(context);
		context.ProvideTargetProperty = null;
		textBlock9.Bind(foregroundProperty2, binding6);
		StyledProperty<bool> isVisibleProperty = Visual.IsVisibleProperty;
		CompiledBindingExtension compiledBindingExtension13 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EViewModels_002EShells_002EBaseShellViewModel_002CXFolder_002ESelectedTab_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Property(XamlIlHelpers.Files_002EViewModels_002EItems_002ETabItemViewModel_002CXFolder_002EHasSelectedFileImageSizeDisplay_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build());
		context.ProvideTargetProperty = Visual.IsVisibleProperty;
		CompiledBinding binding7 = compiledBindingExtension13.ProvideValue(context);
		context.ProvideTargetProperty = null;
		textBlock9.Bind(isVisibleProperty, binding7);
		StyledProperty<string?> textProperty2 = TextBlock.TextProperty;
		CompiledBindingExtension compiledBindingExtension14 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EViewModels_002EShells_002EBaseShellViewModel_002CXFolder_002ESelectedTab_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Property(XamlIlHelpers.Files_002EViewModels_002EItems_002ETabItemViewModel_002CXFolder_002ESelectedFileImageSizeDisplay_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build());
		context.ProvideTargetProperty = TextBlock.TextProperty;
		CompiledBinding binding8 = compiledBindingExtension14.ProvideValue(context);
		context.ProvideTargetProperty = null;
		textBlock9.Bind(textProperty2, binding8);
		context.PopParent();
		((ISupportInitialize)textBlock8).EndInit();
		Controls children8 = grid13.Children;
		TextBlock textBlock11;
		TextBlock textBlock10 = (textBlock11 = new TextBlock());
		((ISupportInitialize)textBlock10).BeginInit();
		children8.Add(textBlock10);
		TextBlock textBlock12 = (textBlock4 = textBlock11);
		context.PushParent(textBlock4);
		TextBlock textBlock13 = textBlock4;
		Grid.SetColumn(textBlock13, 2);
		textBlock13.Margin = new Thickness(0.0, 0.0, 8.0, 0.0);
		textBlock13.VerticalAlignment = VerticalAlignment.Center;
		textBlock13.FontSize = 12.0;
		StyledProperty<IBrush?> foregroundProperty3 = TextBlock.ForegroundProperty;
		DynamicResourceExtension dynamicResourceExtension11 = new DynamicResourceExtension("SystemControlForegroundBaseMediumBrush");
		context.ProvideTargetProperty = TextBlock.ForegroundProperty;
		BindingBase binding9 = dynamicResourceExtension11.ProvideValue(context);
		context.ProvideTargetProperty = null;
		textBlock13.Bind(foregroundProperty3, binding9);
		StyledProperty<bool> isVisibleProperty2 = Visual.IsVisibleProperty;
		CompiledBindingExtension compiledBindingExtension15 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EViewModels_002EShells_002EBaseShellViewModel_002CXFolder_002ESelectedTab_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Property(XamlIlHelpers.Files_002EViewModels_002EItems_002ETabItemViewModel_002CXFolder_002EHasSelectedFileSizeDisplay_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build());
		context.ProvideTargetProperty = Visual.IsVisibleProperty;
		CompiledBinding binding10 = compiledBindingExtension15.ProvideValue(context);
		context.ProvideTargetProperty = null;
		textBlock13.Bind(isVisibleProperty2, binding10);
		StyledProperty<string?> textProperty3 = TextBlock.TextProperty;
		CompiledBindingExtension compiledBindingExtension16 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EViewModels_002EShells_002EBaseShellViewModel_002CXFolder_002ESelectedTab_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Property(XamlIlHelpers.Files_002EViewModels_002EItems_002ETabItemViewModel_002CXFolder_002ESelectedFileSizeDisplay_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build());
		context.ProvideTargetProperty = TextBlock.TextProperty;
		CompiledBinding binding11 = compiledBindingExtension16.ProvideValue(context);
		context.ProvideTargetProperty = null;
		textBlock13.Bind(textProperty3, binding11);
		context.PopParent();
		((ISupportInitialize)textBlock12).EndInit();
		Controls children9 = grid13.Children;
		StackPanel stackPanel2;
		StackPanel stackPanel = (stackPanel2 = new StackPanel());
		((ISupportInitialize)stackPanel).BeginInit();
		children9.Add(stackPanel);
		StackPanel stackPanel4;
		StackPanel stackPanel3 = (stackPanel4 = stackPanel2);
		context.PushParent(stackPanel4);
		Grid.SetColumn(stackPanel4, 3);
		stackPanel4.Orientation = Orientation.Horizontal;
		stackPanel4.HorizontalAlignment = HorizontalAlignment.Right;
		stackPanel4.VerticalAlignment = VerticalAlignment.Center;
		stackPanel4.Margin = new Thickness(0.0, 0.0, 0.0, 0.0);
		Styles styles7 = stackPanel4.Styles;
		Style style8 = new Style();
		style8.Selector = ((Selector?)null).OfType(typeof(Button)).Class("layout-toggle");
		Setter setter29 = new Setter();
		setter29.Property = TemplatedControl.BackgroundProperty;
		setter29.Value = new ImmutableSolidColorBrush(16777215u);
		style8.Add(setter29);
		Setter setter30 = new Setter();
		setter30.Property = TemplatedControl.BorderThicknessProperty;
		setter30.Value = new Thickness(1.0, 1.0, 1.0, 1.0);
		style8.Add(setter30);
		Setter setter31 = new Setter();
		setter31.Property = TemplatedControl.BorderBrushProperty;
		setter31.Value = new ImmutableSolidColorBrush(16777215u);
		style8.Add(setter31);
		Setter setter32 = new Setter();
		setter32.Property = TemplatedControl.CornerRadiusProperty;
		setter32.Value = new CornerRadius(4.0, 4.0, 4.0, 4.0);
		style8.Add(setter32);
		styles7.Add(style8);
		Styles styles8 = stackPanel4.Styles;
		Style item11 = (style = new Style());
		context.PushParent(style);
		Style style9 = style;
		style9.Selector = ((Selector?)null).OfType(typeof(Button)).Class("layout-toggle").Class(":pointerover");
		Setter setter33 = (setter2 = new Setter());
		context.PushParent(setter2);
		Setter setter34 = setter2;
		setter34.Property = TemplatedControl.BackgroundProperty;
		DynamicResourceExtension dynamicResourceExtension12 = new DynamicResourceExtension("ButtonBackgroundPointerOver");
		context.ProvideTargetProperty = XamlIlHelpers.Avalonia_002EStyling_002ESetter_002CAvalonia_002EBase_002EValue_0021Property();
		BindingBase value9 = dynamicResourceExtension12.ProvideValue(context);
		context.ProvideTargetProperty = null;
		setter34.Value = value9;
		context.PopParent();
		style9.Add(setter33);
		context.PopParent();
		styles8.Add(item11);
		Styles styles9 = stackPanel4.Styles;
		Style item12 = (style = new Style());
		context.PushParent(style);
		Style style10 = style;
		style10.Selector = ((Selector?)null).OfType(typeof(Button)).Class("layout-toggle").Class("selected");
		Setter setter35 = (setter2 = new Setter());
		context.PushParent(setter2);
		Setter setter36 = setter2;
		setter36.Property = TemplatedControl.BackgroundProperty;
		DynamicResourceExtension dynamicResourceExtension13 = new DynamicResourceExtension("TreeViewItemSelectedBackgroundBrush");
		context.ProvideTargetProperty = XamlIlHelpers.Avalonia_002EStyling_002ESetter_002CAvalonia_002EBase_002EValue_0021Property();
		BindingBase value10 = dynamicResourceExtension13.ProvideValue(context);
		context.ProvideTargetProperty = null;
		setter36.Value = value10;
		context.PopParent();
		style10.Add(setter35);
		Setter setter37 = (setter2 = new Setter());
		context.PushParent(setter2);
		Setter setter38 = setter2;
		setter38.Property = TemplatedControl.BorderBrushProperty;
		DynamicResourceExtension dynamicResourceExtension14 = new DynamicResourceExtension("SystemControlForegroundBaseMediumBrush");
		context.ProvideTargetProperty = XamlIlHelpers.Avalonia_002EStyling_002ESetter_002CAvalonia_002EBase_002EValue_0021Property();
		BindingBase value11 = dynamicResourceExtension14.ProvideValue(context);
		context.ProvideTargetProperty = null;
		setter38.Value = value11;
		context.PopParent();
		style10.Add(setter37);
		context.PopParent();
		styles9.Add(item12);
		Styles styles10 = stackPanel4.Styles;
		Style item13 = (style = new Style());
		context.PushParent(style);
		Style style11 = style;
		style11.Selector = ((Selector?)null).OfType(typeof(Button)).Class("layout-toggle").Class("selected")
			.Class(":pointerover");
		Setter setter39 = (setter2 = new Setter());
		context.PushParent(setter2);
		Setter setter40 = setter2;
		setter40.Property = TemplatedControl.BackgroundProperty;
		DynamicResourceExtension dynamicResourceExtension15 = new DynamicResourceExtension("TreeViewItemSelectedBackgroundBrush");
		context.ProvideTargetProperty = XamlIlHelpers.Avalonia_002EStyling_002ESetter_002CAvalonia_002EBase_002EValue_0021Property();
		BindingBase value12 = dynamicResourceExtension15.ProvideValue(context);
		context.ProvideTargetProperty = null;
		setter40.Value = value12;
		context.PopParent();
		style11.Add(setter39);
		context.PopParent();
		styles10.Add(item13);
		Styles styles11 = stackPanel4.Styles;
		Style item14 = (style = new Style());
		context.PushParent(style);
		Style style12 = style;
		style12.Selector = ((Selector?)null).OfType(typeof(Button)).Class("layout-toggle").Class("selected")
			.Descendant()
			.OfType(typeof(PathIcon));
		Setter setter41 = (setter2 = new Setter());
		context.PushParent(setter2);
		Setter setter42 = setter2;
		setter42.Property = TemplatedControl.ForegroundProperty;
		DynamicResourceExtension dynamicResourceExtension16 = new DynamicResourceExtension("SystemControlForegroundBaseMediumBrush");
		context.ProvideTargetProperty = XamlIlHelpers.Avalonia_002EStyling_002ESetter_002CAvalonia_002EBase_002EValue_0021Property();
		BindingBase value13 = dynamicResourceExtension16.ProvideValue(context);
		context.ProvideTargetProperty = null;
		setter42.Value = value13;
		context.PopParent();
		style12.Add(setter41);
		Setter setter43 = new Setter();
		setter43.Property = Visual.OpacityProperty;
		setter43.Value = 1.0;
		style12.Add(setter43);
		context.PopParent();
		styles11.Add(item14);
		Controls children10 = stackPanel4.Children;
		Button button2;
		Button button = (button2 = new Button());
		((ISupportInitialize)button).BeginInit();
		children10.Add(button);
		Button button4;
		Button button3 = (button4 = button2);
		context.PushParent(button4);
		Button button5 = button4;
		button5.Name = "DetailsLayoutButton";
		element = button5;
		context.AvaloniaNameScope.Register("DetailsLayoutButton", element);
		button5.Classes.Add("layout-toggle");
		button5.Width = 18.0;
		button5.Height = 18.0;
		button5.Padding = new Thickness(0.0, 0.0, 0.0, 0.0);
		button5.Margin = new Thickness(0.0, 0.0, 0.0, 0.0);
		button5.Tag = "LayoutDetails";
		button5.AddHandler((RoutedEvent)Button.ClickEvent, (Delegate)new EventHandler<RoutedEventArgs>(context.RootObject.OnRunActionCommand), RoutingStrategies.Direct | RoutingStrategies.Bubble, false);
		PathIcon pathIcon2;
		PathIcon pathIcon = (pathIcon2 = new PathIcon());
		((ISupportInitialize)pathIcon).BeginInit();
		button5.Content = pathIcon;
		PathIcon pathIcon4;
		PathIcon pathIcon3 = (pathIcon4 = pathIcon2);
		context.PushParent(pathIcon4);
		PathIcon pathIcon5 = pathIcon4;
		StaticResourceExtension staticResourceExtension4 = new StaticResourceExtension("text_column_one_regular");
		context.ProvideTargetProperty = PathIcon.DataProperty;
		object? obj4 = staticResourceExtension4.ProvideValue(context);
		context.ProvideTargetProperty = null;
		XamlDynamicSetters._003C_003EXamlDynamicSetter_15(pathIcon5, obj4);
		pathIcon5.Width = 12.0;
		pathIcon5.Height = 12.0;
		StyledProperty<IBrush?> foregroundProperty4 = TemplatedControl.ForegroundProperty;
		DynamicResourceExtension dynamicResourceExtension17 = new DynamicResourceExtension("SystemControlForegroundBaseMediumBrush");
		context.ProvideTargetProperty = TemplatedControl.ForegroundProperty;
		BindingBase binding12 = dynamicResourceExtension17.ProvideValue(context);
		context.ProvideTargetProperty = null;
		pathIcon5.Bind(foregroundProperty4, binding12);
		pathIcon5.Opacity = 0.85;
		context.PopParent();
		((ISupportInitialize)pathIcon3).EndInit();
		context.PopParent();
		((ISupportInitialize)button3).EndInit();
		Controls children11 = stackPanel4.Children;
		Button button7;
		Button button6 = (button7 = new Button());
		((ISupportInitialize)button6).BeginInit();
		children11.Add(button6);
		Button button8 = (button4 = button7);
		context.PushParent(button4);
		Button button9 = button4;
		button9.Name = "GridLayoutButton";
		element = button9;
		context.AvaloniaNameScope.Register("GridLayoutButton", element);
		button9.Classes.Add("layout-toggle");
		button9.Width = 18.0;
		button9.Height = 18.0;
		button9.Padding = new Thickness(0.0, 0.0, 0.0, 0.0);
		button9.Margin = new Thickness(2.0, 0.0, 4.0, 0.0);
		button9.Tag = "LayoutGrid";
		button9.AddHandler((RoutedEvent)Button.ClickEvent, (Delegate)new EventHandler<RoutedEventArgs>(context.RootObject.OnRunActionCommand), RoutingStrategies.Direct | RoutingStrategies.Bubble, false);
		PathIcon pathIcon7;
		PathIcon pathIcon6 = (pathIcon7 = new PathIcon());
		((ISupportInitialize)pathIcon6).BeginInit();
		button9.Content = pathIcon6;
		PathIcon pathIcon8 = (pathIcon4 = pathIcon7);
		context.PushParent(pathIcon4);
		PathIcon pathIcon9 = pathIcon4;
		StaticResourceExtension staticResourceExtension5 = new StaticResourceExtension("grid_regular");
		context.ProvideTargetProperty = PathIcon.DataProperty;
		object? obj5 = staticResourceExtension5.ProvideValue(context);
		context.ProvideTargetProperty = null;
		XamlDynamicSetters._003C_003EXamlDynamicSetter_15(pathIcon9, obj5);
		pathIcon9.Width = 12.0;
		pathIcon9.Height = 12.0;
		StyledProperty<IBrush?> foregroundProperty5 = TemplatedControl.ForegroundProperty;
		DynamicResourceExtension dynamicResourceExtension18 = new DynamicResourceExtension("SystemControlForegroundBaseMediumBrush");
		context.ProvideTargetProperty = TemplatedControl.ForegroundProperty;
		BindingBase binding13 = dynamicResourceExtension18.ProvideValue(context);
		context.ProvideTargetProperty = null;
		pathIcon9.Bind(foregroundProperty5, binding13);
		pathIcon9.Opacity = 0.85;
		context.PopParent();
		((ISupportInitialize)pathIcon8).EndInit();
		context.PopParent();
		((ISupportInitialize)button8).EndInit();
		context.PopParent();
		((ISupportInitialize)stackPanel3).EndInit();
		context.PopParent();
		((ISupportInitialize)grid12).EndInit();
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
	private static void _0021XamlIlPopulateTrampoline(ModernShellView P_0)
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
