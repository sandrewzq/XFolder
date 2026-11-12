using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Markup.Xaml.MarkupExtensions;
using Avalonia.Markup.Xaml.MarkupExtensions.CompiledBindings;
using Avalonia.Markup.Xaml.XamlIl.Runtime;
using CompiledAvaloniaXaml;
using DialogHostAvalonia;
using Files.Data.Enums;
using Files.Services.App;
using Files.ViewModels.Dialogs;

namespace Files.Dialogs;

public class CreateItemDialog : BaseUserDialog, IDialog<CreateItemDialogViewModel>
{
	[GeneratedCode("Avalonia.Generators.NameGenerator.InitializeComponentCodeGenerator", "12.0.1.0")]
	internal TextBox InputTextBox;

	[GeneratedCode("Avalonia.Generators.NameGenerator.InitializeComponentCodeGenerator", "12.0.1.0")]
	internal Button OkButton;

	[CompilerGenerated]
	private static Action<object> _0021XamlIlPopulateOverride;

	public CreateItemDialogViewModel? ViewModel
	{
		get
		{
			return DataContext as CreateItemDialogViewModel;
		}
		set
		{
			DataContext = value;
		}
	}

	public CreateItemDialog()
	{
		InitializeComponent();
		Loaded += (object? s, RoutedEventArgs e) =>
		{
			InputTextBox.Focus();
		};
	}

	protected override void OnKeyDown(KeyEventArgs e)
	{
		if (e.Key == Key.Return)
		{
			Button okButton = OkButton;
			if (okButton != null && okButton.IsEnabled)
			{
				ICommand command = OkButton.Command;
				object commandParameter = OkButton.CommandParameter;
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
		InputTextBox = ((nameScope != null) ? nameScope.Find<TextBox>("InputTextBox") : null);
		OkButton = ((nameScope != null) ? nameScope.Find<Button>("OkButton") : null);
	}

	[CompilerGenerated]
	private static void _0021XamlIlPopulate(IServiceProvider P_0, CreateItemDialog P_1)
	{
		XamlIlContext.Context<CreateItemDialog> context = new XamlIlContext.Context<CreateItemDialog>(P_0, new object[1] { _0021AvaloniaResources.NamespaceInfo_003A_002FDialogs_002FCreateItemDialog_002Eaxaml.Singleton }, "avares://XFolder/Dialogs/CreateItemDialog.axaml")
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
		rowDefinitions.Capacity = 2;
		rowDefinitions.Add(new RowDefinition(new GridLength(0.0, GridUnitType.Auto)));
		rowDefinitions.Add(new RowDefinition(new GridLength(0.0, GridUnitType.Auto)));
		grid5.RowDefinitions = rowDefinitions;
		grid5.Margin = new Thickness(5.0, 5.0, 5.0, 5.0);
		Controls children = grid5.Children;
		Grid grid7;
		Grid grid6 = (grid7 = new Grid());
		((ISupportInitialize)grid6).BeginInit();
		children.Add(grid6);
		Grid grid8 = (grid4 = grid7);
		context.PushParent(grid4);
		Grid grid9 = grid4;
		Grid.SetRow(grid9, 0);
		grid9.MinWidth = 200.0;
		Controls children2 = grid9.Children;
		TextBox textBox2;
		TextBox textBox = (textBox2 = new TextBox());
		((ISupportInitialize)textBox).BeginInit();
		children2.Add(textBox);
		TextBox textBox4;
		TextBox textBox3 = (textBox4 = textBox2);
		context.PushParent(textBox4);
		textBox4.Name = "InputTextBox";
		object element = textBox4;
		context.AvaloniaNameScope.Register("InputTextBox", element);
		StyledProperty<string?> placeholderTextProperty = TextBox.PlaceholderTextProperty;
		CompiledBindingExtension compiledBindingExtension = new CompiledBindingExtension(new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EServices_002EApp_002ELocalizationBindingSource_002CXFolder_002EItem_005BEnterAnItemName_005D_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build())
		{
			Source = LocalizationBindingSource.Instance
		};
		context.ProvideTargetProperty = TextBox.PlaceholderTextProperty;
		CompiledBinding binding = compiledBindingExtension.ProvideValue(context);
		context.ProvideTargetProperty = null;
		textBox4.Bind(placeholderTextProperty, binding);
		StyledProperty<string?> textProperty = TextBox.TextProperty;
		CompiledBindingExtension compiledBindingExtension2 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EViewModels_002EDialogs_002ECreateItemDialogViewModel_002CXFolder_002EItemName_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build());
		context.ProvideTargetProperty = TextBox.TextProperty;
		CompiledBinding binding2 = compiledBindingExtension2.ProvideValue(context);
		context.ProvideTargetProperty = null;
		textBox4.Bind(textProperty, binding2);
		TextBlock textBlock2;
		TextBlock textBlock = (textBlock2 = new TextBlock());
		((ISupportInitialize)textBlock).BeginInit();
		ToolTip.SetTip(textBox4, textBlock);
		TextBlock textBlock4;
		TextBlock textBlock3 = (textBlock4 = textBlock2);
		context.PushParent(textBlock4);
		StyledProperty<string?> textProperty2 = TextBlock.TextProperty;
		CompiledBindingExtension compiledBindingExtension3 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EServices_002EApp_002ELocalizationBindingSource_002CXFolder_002EItem_005BInvalidFilename_Text_005D_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build())
		{
			Source = LocalizationBindingSource.Instance
		};
		context.ProvideTargetProperty = TextBlock.TextProperty;
		CompiledBinding binding3 = compiledBindingExtension3.ProvideValue(context);
		context.ProvideTargetProperty = null;
		textBlock4.Bind(textProperty2, binding3);
		StyledProperty<bool> isVisibleProperty = Visual.IsVisibleProperty;
		CompiledBindingExtension compiledBindingExtension4 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Not().Property(XamlIlHelpers.Files_002EViewModels_002EDialogs_002ECreateItemDialogViewModel_002CXFolder_002EIsNameValid_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build());
		context.ProvideTargetProperty = Visual.IsVisibleProperty;
		CompiledBinding binding4 = compiledBindingExtension4.ProvideValue(context);
		context.ProvideTargetProperty = null;
		textBlock4.Bind(isVisibleProperty, binding4);
		context.PopParent();
		((ISupportInitialize)textBlock3).EndInit();
		context.PopParent();
		((ISupportInitialize)textBox3).EndInit();
		context.PopParent();
		((ISupportInitialize)grid8).EndInit();
		Controls children3 = grid5.Children;
		StackPanel stackPanel2;
		StackPanel stackPanel = (stackPanel2 = new StackPanel());
		((ISupportInitialize)stackPanel).BeginInit();
		children3.Add(stackPanel);
		StackPanel stackPanel4;
		StackPanel stackPanel3 = (stackPanel4 = stackPanel2);
		context.PushParent(stackPanel4);
		Grid.SetRow(stackPanel4, 1);
		stackPanel4.Orientation = Orientation.Horizontal;
		stackPanel4.HorizontalAlignment = HorizontalAlignment.Center;
		stackPanel4.Spacing = 8.0;
		stackPanel4.Margin = new Thickness(0.0, 20.0, 0.0, 0.0);
		Controls children4 = stackPanel4.Children;
		Button button2;
		Button button = (button2 = new Button());
		((ISupportInitialize)button).BeginInit();
		children4.Add(button);
		Button button4;
		Button button3 = (button4 = button2);
		context.PushParent(button4);
		Button button5 = button4;
		button5.Name = "OkButton";
		element = button5;
		context.AvaloniaNameScope.Register("OkButton", element);
		button5.Classes.Add("accent");
		CompiledBindingExtension compiledBindingExtension5 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EServices_002EApp_002ELocalizationBindingSource_002CXFolder_002EItem_005BOK_005D_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build())
		{
			Source = LocalizationBindingSource.Instance
		};
		context.ProvideTargetProperty = ContentControl.ContentProperty;
		CompiledBinding compiledBinding = compiledBindingExtension5.ProvideValue(context);
		context.ProvideTargetProperty = null;
		XamlDynamicSetters._003C_003EXamlDynamicSetter_2(button5, compiledBinding);
		StyledProperty<bool> isEnabledProperty = InputElement.IsEnabledProperty;
		CompiledBindingExtension compiledBindingExtension6 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EViewModels_002EDialogs_002ECreateItemDialogViewModel_002CXFolder_002EIsNameValid_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build());
		context.ProvideTargetProperty = InputElement.IsEnabledProperty;
		CompiledBinding binding5 = compiledBindingExtension6.ProvideValue(context);
		context.ProvideTargetProperty = null;
		button5.Bind(isEnabledProperty, binding5);
		StyledProperty<ICommand?> commandProperty = Button.CommandProperty;
		CompiledBindingExtension compiledBindingExtension7 = new CompiledBindingExtension
		{
			Path = new CompiledBindingPathBuilder().VisualAncestor(typeof(DialogHost), 0).Property(DialogHost.CloseDialogCommandProperty, PropertyInfoAccessorFactory.CreateAvaloniaPropertyAccessor).Build()
		};
		context.ProvideTargetProperty = Button.CommandProperty;
		CompiledBinding binding6 = compiledBindingExtension7.ProvideValue(context);
		context.ProvideTargetProperty = null;
		button5.Bind(commandProperty, binding6);
		button5.CommandParameter = DynamicDialogResult.Primary;
		context.PopParent();
		((ISupportInitialize)button3).EndInit();
		Controls children5 = stackPanel4.Children;
		Button button7;
		Button button6 = (button7 = new Button());
		((ISupportInitialize)button6).BeginInit();
		children5.Add(button6);
		Button button8 = (button4 = button7);
		context.PushParent(button4);
		Button button9 = button4;
		CompiledBindingExtension compiledBindingExtension8 = new CompiledBindingExtension(new CompiledBindingPathBuilder().Property(XamlIlHelpers.Files_002EServices_002EApp_002ELocalizationBindingSource_002CXFolder_002EItem_005BCancel_005D_0021Property(), PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build())
		{
			Source = LocalizationBindingSource.Instance
		};
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
		button9.CommandParameter = DynamicDialogResult.Cancel;
		context.PopParent();
		((ISupportInitialize)button8).EndInit();
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
	private static void _0021XamlIlPopulateTrampoline(CreateItemDialog P_0)
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
