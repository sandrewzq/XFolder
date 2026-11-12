using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Data.Converters;
using Avalonia.Input;
using Avalonia.Layout;
using Avalonia.Markup.Xaml.MarkupExtensions;
using Avalonia.Markup.Xaml.MarkupExtensions.CompiledBindings;
using Avalonia.Markup.Xaml.XamlIl.Runtime;
using Avalonia.Media;
using CompiledAvaloniaXaml;
using DialogHostAvalonia;
using Files.Data.Enums;
using Files.ViewModels.Dialogs;

namespace Files.Dialogs;

public class DynamicDialog : BaseUserDialog
{
	[GeneratedCode("Avalonia.Generators.NameGenerator.InitializeComponentCodeGenerator", "12.0.1.0")]
	internal Button PrimaryButton;

	[CompilerGenerated]
	private static Action<object> _0021XamlIlPopulateOverride;

	public DynamicDialogViewModel? ViewModel => DataContext as DynamicDialogViewModel;

	public DynamicDialog()
	{
		InitializeComponent();
		AttachedToVisualTree += OnAttachedToVisualTree;
	}

	public DynamicDialog(DynamicDialogViewModel viewModel)
		: this()
	{
		DataContext = viewModel;
	}

	private void OnAttachedToVisualTree(object? sender, VisualTreeAttachmentEventArgs e)
	{
		Button primaryButton = PrimaryButton;
		if (primaryButton != null && primaryButton.IsVisible)
		{
			PrimaryButton.Focus();
		}
	}

	protected override void OnKeyDown(KeyEventArgs e)
	{
		if (e.Key == Key.Return)
		{
			Button primaryButton = PrimaryButton;
			if (primaryButton != null && primaryButton.IsVisible)
			{
				ICommand command = PrimaryButton.Command;
				object commandParameter = PrimaryButton.CommandParameter;
				if (command != null && command.CanExecute(commandParameter))
				{
					command.Execute(commandParameter);
					e.Handled = true;
					return;
				}
			}
		}
		base.OnKeyDown(e);
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
		PrimaryButton = ((nameScope != null) ? nameScope.Find<Button>("PrimaryButton") : null);
	}

	[CompilerGenerated]
	private static void _0021XamlIlPopulate(IServiceProvider P_0, DynamicDialog P_1)
	{
		XamlIlContext.Context<DynamicDialog> context = new XamlIlContext.Context<DynamicDialog>(P_0, new object[1] { _0021AvaloniaResources.NamespaceInfo_003A_002FDialogs_002FDynamicDialog_002Eaxaml.Singleton }, "avares://XFolder/Dialogs/DynamicDialog.axaml")
		{
			RootObject = P_1,
			IntermediateRoot = P_1
		};
		((ISupportInitialize)P_1).BeginInit();
		context.PushParent(P_1);
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
		rowDefinitions.Add(new RowDefinition(new GridLength(1.0, GridUnitType.Star)));
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
		CompiledBindingExtension compiledBindingExtension = new CompiledBindingExtension(new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EViewModels_002EDialogs_002EDynamicDialogViewModel_002CXFolder_002ETitleText_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build());
		context.ProvideTargetProperty = TextBlock.TextProperty;
		CompiledBinding binding = compiledBindingExtension.ProvideValue(context);
		context.ProvideTargetProperty = null;
		textBlock5.Bind(textProperty, binding);
		StyledProperty<bool> isVisibleProperty = Visual.IsVisibleProperty;
		CompiledBindingExtension compiledBindingExtension2 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EViewModels_002EDialogs_002EDynamicDialogViewModel_002CXFolder_002ETitleText_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build())
		{
			Converter = StringConverters.IsNotNullOrEmpty
		};
		context.ProvideTargetProperty = Visual.IsVisibleProperty;
		CompiledBinding binding2 = compiledBindingExtension2.ProvideValue(context);
		context.ProvideTargetProperty = null;
		textBlock5.Bind(isVisibleProperty, binding2);
		textBlock5.FontSize = 20.0;
		textBlock5.FontWeight = FontWeight.DemiBold;
		textBlock5.Margin = new Thickness(0.0, 0.0, 0.0, 8.0);
		textBlock5.HorizontalAlignment = HorizontalAlignment.Center;
		context.PopParent();
		((ISupportInitialize)textBlock3).EndInit();
		Controls children2 = grid5.Children;
		Grid grid7;
		Grid grid6 = (grid7 = new Grid());
		((ISupportInitialize)grid6).BeginInit();
		children2.Add(grid6);
		Grid grid8 = (grid4 = grid7);
		context.PushParent(grid4);
		Grid grid9 = grid4;
		Grid.SetRow(grid9, 1);
		RowDefinitions rowDefinitions2 = new RowDefinitions();
		rowDefinitions2.Capacity = 2;
		rowDefinitions2.Add(new RowDefinition(new GridLength(0.0, GridUnitType.Auto)));
		rowDefinitions2.Add(new RowDefinition(new GridLength(1.0, GridUnitType.Star)));
		grid9.RowDefinitions = rowDefinitions2;
		Controls children3 = grid9.Children;
		TextBlock textBlock7;
		TextBlock textBlock6 = (textBlock7 = new TextBlock());
		((ISupportInitialize)textBlock6).BeginInit();
		children3.Add(textBlock6);
		TextBlock textBlock8 = (textBlock4 = textBlock7);
		context.PushParent(textBlock4);
		TextBlock textBlock9 = textBlock4;
		Grid.SetRow(textBlock9, 0);
		StyledProperty<string?> textProperty2 = TextBlock.TextProperty;
		CompiledBindingExtension compiledBindingExtension3 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EViewModels_002EDialogs_002EDynamicDialogViewModel_002CXFolder_002ESubtitleText_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build());
		context.ProvideTargetProperty = TextBlock.TextProperty;
		CompiledBinding binding3 = compiledBindingExtension3.ProvideValue(context);
		context.ProvideTargetProperty = null;
		textBlock9.Bind(textProperty2, binding3);
		StyledProperty<bool> isVisibleProperty2 = Visual.IsVisibleProperty;
		CompiledBindingExtension compiledBindingExtension4 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EViewModels_002EDialogs_002EDynamicDialogViewModel_002CXFolder_002ESubtitleText_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build())
		{
			Converter = StringConverters.IsNotNullOrEmpty
		};
		context.ProvideTargetProperty = Visual.IsVisibleProperty;
		CompiledBinding binding4 = compiledBindingExtension4.ProvideValue(context);
		context.ProvideTargetProperty = null;
		textBlock9.Bind(isVisibleProperty2, binding4);
		textBlock9.TextWrapping = TextWrapping.Wrap;
		textBlock9.Margin = new Thickness(0.0, 0.0, 0.0, 16.0);
		context.PopParent();
		((ISupportInitialize)textBlock8).EndInit();
		context.PopParent();
		((ISupportInitialize)grid8).EndInit();
		Controls children4 = grid5.Children;
		StackPanel stackPanel2;
		StackPanel stackPanel = (stackPanel2 = new StackPanel());
		((ISupportInitialize)stackPanel).BeginInit();
		children4.Add(stackPanel);
		StackPanel stackPanel4;
		StackPanel stackPanel3 = (stackPanel4 = stackPanel2);
		context.PushParent(stackPanel4);
		Grid.SetRow(stackPanel4, 2);
		stackPanel4.Orientation = Orientation.Horizontal;
		stackPanel4.HorizontalAlignment = HorizontalAlignment.Center;
		stackPanel4.Spacing = 8.0;
		Controls children5 = stackPanel4.Children;
		Button button2;
		Button button = (button2 = new Button());
		((ISupportInitialize)button).BeginInit();
		children5.Add(button);
		Button button4;
		Button button3 = (button4 = button2);
		context.PushParent(button4);
		Button button5 = button4;
		button5.Name = "PrimaryButton";
		object element = button5;
		context.AvaloniaNameScope.Register("PrimaryButton", element);
		button5.Classes.Add("accent");
		CompiledBindingExtension compiledBindingExtension5 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EViewModels_002EDialogs_002EDynamicDialogViewModel_002CXFolder_002EPrimaryButtonText_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build());
		context.ProvideTargetProperty = ContentControl.ContentProperty;
		CompiledBinding compiledBinding = compiledBindingExtension5.ProvideValue(context);
		context.ProvideTargetProperty = null;
		XamlDynamicSetters._003C_003EXamlDynamicSetter_2(button5, compiledBinding);
		StyledProperty<ICommand?> commandProperty = Button.CommandProperty;
		CompiledBindingExtension compiledBindingExtension6 = new CompiledBindingExtension
		{
			Path = new CompiledBindingPathBuilder().VisualAncestor(typeof(DialogHost), 0).Property(DialogHost.CloseDialogCommandProperty, PropertyInfoAccessorFactory.CreateAvaloniaPropertyAccessor).Build()
		};
		context.ProvideTargetProperty = Button.CommandProperty;
		CompiledBinding binding5 = compiledBindingExtension6.ProvideValue(context);
		context.ProvideTargetProperty = null;
		button5.Bind(commandProperty, binding5);
		button5.CommandParameter = DynamicDialogResult.Primary;
		StyledProperty<bool> isVisibleProperty3 = Visual.IsVisibleProperty;
		CompiledBindingExtension compiledBindingExtension7 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EViewModels_002EDialogs_002EDynamicDialogViewModel_002CXFolder_002EPrimaryButtonText_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build())
		{
			Converter = StringConverters.IsNotNullOrEmpty
		};
		context.ProvideTargetProperty = Visual.IsVisibleProperty;
		CompiledBinding binding6 = compiledBindingExtension7.ProvideValue(context);
		context.ProvideTargetProperty = null;
		button5.Bind(isVisibleProperty3, binding6);
		context.PopParent();
		((ISupportInitialize)button3).EndInit();
		Controls children6 = stackPanel4.Children;
		Button button7;
		Button button6 = (button7 = new Button());
		((ISupportInitialize)button6).BeginInit();
		children6.Add(button6);
		Button button8 = (button4 = button7);
		context.PushParent(button4);
		Button button9 = button4;
		CompiledBindingExtension compiledBindingExtension8 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EViewModels_002EDialogs_002EDynamicDialogViewModel_002CXFolder_002ESecondaryButtonText_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build());
		context.ProvideTargetProperty = ContentControl.ContentProperty;
		CompiledBinding compiledBinding2 = compiledBindingExtension8.ProvideValue(context);
		context.ProvideTargetProperty = null;
		XamlDynamicSetters._003C_003EXamlDynamicSetter_2(button9, compiledBinding2);
		StyledProperty<ICommand?> commandProperty2 = Button.CommandProperty;
		CompiledBindingExtension compiledBindingExtension9 = new CompiledBindingExtension
		{
			Path = new CompiledBindingPathBuilder().VisualAncestor(typeof(DialogHost), 0).Property(DialogHost.CloseDialogCommandProperty, PropertyInfoAccessorFactory.CreateAvaloniaPropertyAccessor).Build()
		};
		context.ProvideTargetProperty = Button.CommandProperty;
		CompiledBinding binding7 = compiledBindingExtension9.ProvideValue(context);
		context.ProvideTargetProperty = null;
		button9.Bind(commandProperty2, binding7);
		button9.CommandParameter = DynamicDialogResult.Secondary;
		StyledProperty<bool> isVisibleProperty4 = Visual.IsVisibleProperty;
		CompiledBindingExtension compiledBindingExtension10 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EViewModels_002EDialogs_002EDynamicDialogViewModel_002CXFolder_002ESecondaryButtonText_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build())
		{
			Converter = StringConverters.IsNotNullOrEmpty
		};
		context.ProvideTargetProperty = Visual.IsVisibleProperty;
		CompiledBinding binding8 = compiledBindingExtension10.ProvideValue(context);
		context.ProvideTargetProperty = null;
		button9.Bind(isVisibleProperty4, binding8);
		context.PopParent();
		((ISupportInitialize)button8).EndInit();
		Controls children7 = stackPanel4.Children;
		Button button11;
		Button button10 = (button11 = new Button());
		((ISupportInitialize)button10).BeginInit();
		children7.Add(button10);
		Button button12 = (button4 = button11);
		context.PushParent(button4);
		Button button13 = button4;
		CompiledBindingExtension compiledBindingExtension11 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EViewModels_002EDialogs_002EDynamicDialogViewModel_002CXFolder_002ECloseButtonText_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build());
		context.ProvideTargetProperty = ContentControl.ContentProperty;
		CompiledBinding compiledBinding3 = compiledBindingExtension11.ProvideValue(context);
		context.ProvideTargetProperty = null;
		XamlDynamicSetters._003C_003EXamlDynamicSetter_2(button13, compiledBinding3);
		StyledProperty<ICommand?> commandProperty3 = Button.CommandProperty;
		CompiledBindingExtension compiledBindingExtension12 = new CompiledBindingExtension
		{
			Path = new CompiledBindingPathBuilder().VisualAncestor(typeof(DialogHost), 0).Property(DialogHost.CloseDialogCommandProperty, PropertyInfoAccessorFactory.CreateAvaloniaPropertyAccessor).Build()
		};
		context.ProvideTargetProperty = Button.CommandProperty;
		CompiledBinding binding9 = compiledBindingExtension12.ProvideValue(context);
		context.ProvideTargetProperty = null;
		button13.Bind(commandProperty3, binding9);
		button13.CommandParameter = DynamicDialogResult.Cancel;
		StyledProperty<bool> isVisibleProperty5 = Visual.IsVisibleProperty;
		CompiledBindingExtension compiledBindingExtension13 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EViewModels_002EDialogs_002EDynamicDialogViewModel_002CXFolder_002ECloseButtonText_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build())
		{
			Converter = StringConverters.IsNotNullOrEmpty
		};
		context.ProvideTargetProperty = Visual.IsVisibleProperty;
		CompiledBinding binding10 = compiledBindingExtension13.ProvideValue(context);
		context.ProvideTargetProperty = null;
		button13.Bind(isVisibleProperty5, binding10);
		context.PopParent();
		((ISupportInitialize)button12).EndInit();
		context.PopParent();
		((ISupportInitialize)stackPanel3).EndInit();
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
	private static void _0021XamlIlPopulateTrampoline(DynamicDialog P_0)
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
