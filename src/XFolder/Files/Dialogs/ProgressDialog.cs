using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Layout;
using Avalonia.Markup.Xaml;
using Avalonia.Markup.Xaml.MarkupExtensions;
using Avalonia.Markup.Xaml.MarkupExtensions.CompiledBindings;
using Avalonia.Markup.Xaml.XamlIl.Runtime;
using Avalonia.Media;
using CompiledAvaloniaXaml;
using DialogHostAvalonia;
using Files.Data.Enums;
using Files.Services.App;
using Files.ViewModels.Dialogs;

namespace Files.Dialogs;

public class ProgressDialog : BaseUserDialog
{
	[CompilerGenerated]
	private static Action<object> _0021XamlIlPopulateOverride;

	public ProgressDialogViewModel ViewModel => (ProgressDialogViewModel)DataContext;

	public ProgressDialog()
	{
		InitializeComponent();
	}

	public ProgressDialog(ProgressDialogViewModel viewModel)
		: this()
	{
		DataContext = viewModel;
	}

	[GeneratedCode("Avalonia.Generators.NameGenerator.InitializeComponentCodeGenerator", "12.0.1.0")]
	[ExcludeFromCodeCoverage]
	public void InitializeComponent(bool loadXaml = true)
	{
		if (loadXaml)
		{
			_0021XamlIlPopulateTrampoline(this);
		}
	}

	[CompilerGenerated]
	private static void _0021XamlIlPopulate(IServiceProvider P_0, ProgressDialog P_1)
	{
		XamlIlContext.Context<ProgressDialog> context = new XamlIlContext.Context<ProgressDialog>(P_0, new object[1] { _0021AvaloniaResources.NamespaceInfo_003A_002FDialogs_002FProgressDialog_002Eaxaml.Singleton }, "avares://XFolder/Dialogs/ProgressDialog.axaml")
		{
			RootObject = P_1,
			IntermediateRoot = P_1
		};
		((ISupportInitialize)P_1).BeginInit();
		context.PushParent(P_1);
		P_1.Width = 540.0;
		P_1.MinWidth = 560.0;
		Grid grid2;
		Grid grid = (grid2 = new Grid());
		((ISupportInitialize)grid).BeginInit();
		P_1.Content = grid;
		Grid grid4;
		Grid grid3 = (grid4 = grid2);
		context.PushParent(grid4);
		Grid grid5 = grid4;
		RowDefinitions rowDefinitions = new RowDefinitions();
		rowDefinitions.Capacity = 4;
		rowDefinitions.Add(new RowDefinition(new GridLength(0.0, GridUnitType.Auto)));
		rowDefinitions.Add(new RowDefinition(new GridLength(0.0, GridUnitType.Auto)));
		rowDefinitions.Add(new RowDefinition(new GridLength(0.0, GridUnitType.Auto)));
		rowDefinitions.Add(new RowDefinition(new GridLength(0.0, GridUnitType.Auto)));
		grid5.RowDefinitions = rowDefinitions;
		grid5.Margin = new Thickness(16.0, 16.0, 16.0, 16.0);
		Controls children = grid5.Children;
		TextBlock textBlock2;
		TextBlock textBlock = (textBlock2 = new TextBlock());
		((ISupportInitialize)textBlock).BeginInit();
		children.Add(textBlock);
		TextBlock textBlock4;
		TextBlock textBlock3 = (textBlock4 = textBlock2);
		context.PushParent(textBlock4);
		TextBlock textBlock5 = textBlock4;
		Grid.SetRow(textBlock5, 0);
		StyledProperty<string?> textProperty = TextBlock.TextProperty;
		CompiledBindingExtension compiledBindingExtension = new CompiledBindingExtension(new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EViewModels_002EDialogs_002EProgressDialogViewModel_002CXFolder_002ETitleText_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build());
		context.ProvideTargetProperty = TextBlock.TextProperty;
		CompiledBinding binding = compiledBindingExtension.ProvideValue(context);
		context.ProvideTargetProperty = null;
		textBlock5.Bind(textProperty, binding);
		textBlock5.FontSize = 20.0;
		textBlock5.FontWeight = FontWeight.DemiBold;
		textBlock5.HorizontalAlignment = HorizontalAlignment.Center;
		context.PopParent();
		((ISupportInitialize)textBlock3).EndInit();
		Controls children2 = grid5.Children;
		TextBlock textBlock7;
		TextBlock textBlock6 = (textBlock7 = new TextBlock());
		((ISupportInitialize)textBlock6).BeginInit();
		children2.Add(textBlock6);
		TextBlock textBlock8 = (textBlock4 = textBlock7);
		context.PushParent(textBlock4);
		TextBlock textBlock9 = textBlock4;
		Grid.SetRow(textBlock9, 1);
		StyledProperty<string?> textProperty2 = TextBlock.TextProperty;
		CompiledBindingExtension compiledBindingExtension2 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EViewModels_002EDialogs_002EProgressDialogViewModel_002CXFolder_002ESubtitleText_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build());
		context.ProvideTargetProperty = TextBlock.TextProperty;
		CompiledBinding binding2 = compiledBindingExtension2.ProvideValue(context);
		context.ProvideTargetProperty = null;
		textBlock9.Bind(textProperty2, binding2);
		textBlock9.TextWrapping = TextWrapping.NoWrap;
		textBlock9.TextTrimming = TextTrimming.CharacterEllipsis;
		textBlock9.FontFamily = new FontFamily(((IUriContext)context).BaseUri, "Consolas, Menlo, Monaco, Courier New, monospace");
		textBlock9.Margin = new Thickness(0.0, 8.0, 0.0, 0.0);
		context.PopParent();
		((ISupportInitialize)textBlock8).EndInit();
		Controls children3 = grid5.Children;
		TextBlock textBlock11;
		TextBlock textBlock10 = (textBlock11 = new TextBlock());
		((ISupportInitialize)textBlock10).BeginInit();
		children3.Add(textBlock10);
		TextBlock textBlock12 = (textBlock4 = textBlock11);
		context.PushParent(textBlock4);
		TextBlock textBlock13 = textBlock4;
		Grid.SetRow(textBlock13, 2);
		StyledProperty<string?> textProperty3 = TextBlock.TextProperty;
		CompiledBindingExtension compiledBindingExtension3 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EViewModels_002EDialogs_002EProgressDialogViewModel_002CXFolder_002ECurrentItemText_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build());
		context.ProvideTargetProperty = TextBlock.TextProperty;
		CompiledBinding binding3 = compiledBindingExtension3.ProvideValue(context);
		context.ProvideTargetProperty = null;
		textBlock13.Bind(textProperty3, binding3);
		textBlock13.TextWrapping = TextWrapping.NoWrap;
		textBlock13.TextTrimming = TextTrimming.CharacterEllipsis;
		textBlock13.FontFamily = new FontFamily(((IUriContext)context).BaseUri, "Consolas, Menlo, Monaco, Courier New, monospace");
		textBlock13.Margin = new Thickness(0.0, 8.0, 0.0, 0.0);
		context.PopParent();
		((ISupportInitialize)textBlock12).EndInit();
		Controls children4 = grid5.Children;
		Grid grid7;
		Grid grid6 = (grid7 = new Grid());
		((ISupportInitialize)grid6).BeginInit();
		children4.Add(grid6);
		Grid grid8 = (grid4 = grid7);
		context.PushParent(grid4);
		Grid grid9 = grid4;
		Grid.SetRow(grid9, 3);
		RowDefinitions rowDefinitions2 = new RowDefinitions();
		rowDefinitions2.Capacity = 2;
		rowDefinitions2.Add(new RowDefinition(new GridLength(0.0, GridUnitType.Auto)));
		rowDefinitions2.Add(new RowDefinition(new GridLength(0.0, GridUnitType.Auto)));
		grid9.RowDefinitions = rowDefinitions2;
		grid9.Margin = new Thickness(0.0, 12.0, 0.0, 0.0);
		Controls children5 = grid9.Children;
		ProgressBar progressBar2;
		ProgressBar progressBar = (progressBar2 = new ProgressBar());
		((ISupportInitialize)progressBar).BeginInit();
		children5.Add(progressBar);
		ProgressBar progressBar4;
		ProgressBar progressBar3 = (progressBar4 = progressBar2);
		context.PushParent(progressBar4);
		Grid.SetRow(progressBar4, 0);
		progressBar4.Minimum = 0.0;
		progressBar4.Maximum = 100.0;
		StyledProperty<double> valueProperty = RangeBase.ValueProperty;
		CompiledBindingExtension compiledBindingExtension4 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EViewModels_002EDialogs_002EProgressDialogViewModel_002CXFolder_002EProgressValue_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build());
		context.ProvideTargetProperty = RangeBase.ValueProperty;
		CompiledBinding binding4 = compiledBindingExtension4.ProvideValue(context);
		context.ProvideTargetProperty = null;
		progressBar4.Bind(valueProperty, binding4);
		StyledProperty<bool> isIndeterminateProperty = ProgressBar.IsIndeterminateProperty;
		CompiledBindingExtension compiledBindingExtension5 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EViewModels_002EDialogs_002EProgressDialogViewModel_002CXFolder_002EIsIndeterminate_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build());
		context.ProvideTargetProperty = ProgressBar.IsIndeterminateProperty;
		CompiledBinding binding5 = compiledBindingExtension5.ProvideValue(context);
		context.ProvideTargetProperty = null;
		progressBar4.Bind(isIndeterminateProperty, binding5);
		progressBar4.Height = 6.0;
		context.PopParent();
		((ISupportInitialize)progressBar3).EndInit();
		Controls children6 = grid9.Children;
		StackPanel stackPanel2;
		StackPanel stackPanel = (stackPanel2 = new StackPanel());
		((ISupportInitialize)stackPanel).BeginInit();
		children6.Add(stackPanel);
		StackPanel stackPanel4;
		StackPanel stackPanel3 = (stackPanel4 = stackPanel2);
		context.PushParent(stackPanel4);
		Grid.SetRow(stackPanel4, 1);
		stackPanel4.Orientation = Orientation.Horizontal;
		stackPanel4.HorizontalAlignment = HorizontalAlignment.Center;
		stackPanel4.Spacing = 8.0;
		stackPanel4.Margin = new Thickness(0.0, 30.0, 0.0, 0.0);
		Controls children7 = stackPanel4.Children;
		Button button2;
		Button button = (button2 = new Button());
		((ISupportInitialize)button).BeginInit();
		children7.Add(button);
		Button button4;
		Button button3 = (button4 = button2);
		context.PushParent(button4);
		CompiledBindingExtension compiledBindingExtension6 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EServices_002EApp_002ELocalizationBindingSource_002CXFolder_002EItem_005BCancel_005D_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build())
		{
			Source = LocalizationBindingSource.Instance
		};
		context.ProvideTargetProperty = ContentControl.ContentProperty;
		CompiledBinding compiledBinding = compiledBindingExtension6.ProvideValue(context);
		context.ProvideTargetProperty = null;
		XamlDynamicSetters._003C_003EXamlDynamicSetter_2(button4, compiledBinding);
		StyledProperty<ICommand?> commandProperty = Button.CommandProperty;
		CompiledBindingExtension compiledBindingExtension7 = new CompiledBindingExtension
		{
			Path = new CompiledBindingPathBuilder().VisualAncestor(typeof(DialogHost), 0).Property(DialogHost.CloseDialogCommandProperty, PropertyInfoAccessorFactory.CreateAvaloniaPropertyAccessor).Build()
		};
		context.ProvideTargetProperty = Button.CommandProperty;
		CompiledBinding binding6 = compiledBindingExtension7.ProvideValue(context);
		context.ProvideTargetProperty = null;
		button4.Bind(commandProperty, binding6);
		button4.CommandParameter = DynamicDialogResult.Cancel;
		context.PopParent();
		((ISupportInitialize)button3).EndInit();
		context.PopParent();
		((ISupportInitialize)stackPanel3).EndInit();
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
	private static void _0021XamlIlPopulateTrampoline(ProgressDialog P_0)
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
