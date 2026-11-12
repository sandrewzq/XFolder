using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Markup.Xaml.MarkupExtensions;
using Avalonia.Markup.Xaml.XamlIl.Runtime;
using Avalonia.Media;
using CompiledAvaloniaXaml;
using Files.Assets.Lang;
using Files.Helpers.Application;

namespace Files.Dialogs;

public class ExpiredVersionDialog : UserControl
{
	[GeneratedCode("Avalonia.Generators.NameGenerator.InitializeComponentCodeGenerator", "12.0.1.0")]
	internal TextBlock TitleTextBlock;

	[GeneratedCode("Avalonia.Generators.NameGenerator.InitializeComponentCodeGenerator", "12.0.1.0")]
	internal TextBlock MessageTextBlock;

	[GeneratedCode("Avalonia.Generators.NameGenerator.InitializeComponentCodeGenerator", "12.0.1.0")]
	internal TextBlock DownloadUrlTextBlock;

	[GeneratedCode("Avalonia.Generators.NameGenerator.InitializeComponentCodeGenerator", "12.0.1.0")]
	internal Button UpgradeButton;

	[CompilerGenerated]
	private static Action<object> _0021XamlIlPopulateOverride;

	public event EventHandler? UpgradeRequested;

	public ExpiredVersionDialog()
	{
		InitializeComponent();
		TitleTextBlock.Text = GetResource("VersionExpiredDialogTitle", "Version is outdated");
		MessageTextBlock.Text = GetResource("VersionExpiredDialogMessage", "This version is no longer supported. Please upgrade to the latest version.");
		DownloadUrlTextBlock.Text = "https://github.com/zebrapixel/XFolder/releases/latest";
		UpgradeButton.Content = GetResource("VersionExpiredDialogUpgradeButtonText", "Upgrade");
		AttachedToVisualTree += (object? _, VisualTreeAttachmentEventArgs _) =>
		{
			Focus();
			UpgradeButton.Focus();
		};
		AppLanguageHelper.LanguageChanged += OnLanguageChanged;
		DetachedFromVisualTree += (object? _, VisualTreeAttachmentEventArgs _) =>
		{
			AppLanguageHelper.LanguageChanged -= OnLanguageChanged;
		};
	}

	private void OnLanguageChanged(object? sender, EventArgs e)
	{
		TitleTextBlock.Text = GetResource("VersionExpiredDialogTitle", "Version is outdated");
		MessageTextBlock.Text = GetResource("VersionExpiredDialogMessage", "This version is no longer supported. Please upgrade to the latest version.");
		UpgradeButton.Content = GetResource("VersionExpiredDialogUpgradeButtonText", "Upgrade");
	}

	private void OnUpgradeButtonClick(object? sender, RoutedEventArgs e)
	{
		UpgradeRequested?.Invoke(this, EventArgs.Empty);
	}

	protected override void OnKeyDown(KeyEventArgs e)
	{
		if (e.Key == Key.Escape)
		{
			e.Handled = true;
		}
		else
		{
			base.OnKeyDown(e);
		}
	}

	private static string GetResource(string key, string fallback)
	{
		return Files.Assets.Lang.Resources.ResourceManager.GetString(key, Files.Assets.Lang.Resources.Culture) ?? fallback;
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
		TitleTextBlock = ((nameScope != null) ? nameScope.Find<TextBlock>("TitleTextBlock") : null);
		MessageTextBlock = ((nameScope != null) ? nameScope.Find<TextBlock>("MessageTextBlock") : null);
		DownloadUrlTextBlock = ((nameScope != null) ? nameScope.Find<TextBlock>("DownloadUrlTextBlock") : null);
		UpgradeButton = ((nameScope != null) ? nameScope.Find<Button>("UpgradeButton") : null);
	}

	[CompilerGenerated]
	private static void _0021XamlIlPopulate(IServiceProvider P_0, ExpiredVersionDialog P_1)
	{
		XamlIlContext.Context<ExpiredVersionDialog> context = new XamlIlContext.Context<ExpiredVersionDialog>(P_0, new object[1] { _0021AvaloniaResources.NamespaceInfo_003A_002FDialogs_002FExpiredVersionDialog_002Eaxaml.Singleton }, "avares://XFolder/Dialogs/ExpiredVersionDialog.axaml")
		{
			RootObject = P_1,
			IntermediateRoot = P_1
		};
		((ISupportInitialize)P_1).BeginInit();
		context.PushParent(P_1);
		P_1.Focusable = true;
		Border border2;
		Border border = (border2 = new Border());
		((ISupportInitialize)border).BeginInit();
		P_1.Content = border;
		Border border4;
		Border border3 = (border4 = border2);
		context.PushParent(border4);
		border4.Width = 420.0;
		border4.Padding = new Thickness(24.0, 24.0, 24.0, 24.0);
		border4.CornerRadius = new CornerRadius(12.0, 12.0, 12.0, 12.0);
		StyledProperty<IBrush?> backgroundProperty = Border.BackgroundProperty;
		DynamicResourceExtension dynamicResourceExtension = new DynamicResourceExtension("DialogBackgroundBrush");
		context.ProvideTargetProperty = Border.BackgroundProperty;
		BindingBase binding = dynamicResourceExtension.ProvideValue(context);
		context.ProvideTargetProperty = null;
		border4.Bind(backgroundProperty, binding);
		StackPanel stackPanel2;
		StackPanel stackPanel = (stackPanel2 = new StackPanel());
		((ISupportInitialize)stackPanel).BeginInit();
		border4.Child = stackPanel;
		StackPanel stackPanel4;
		StackPanel stackPanel3 = (stackPanel4 = stackPanel2);
		context.PushParent(stackPanel4);
		stackPanel4.Spacing = 16.0;
		Controls children = stackPanel4.Children;
		TextBlock textBlock2;
		TextBlock textBlock = (textBlock2 = new TextBlock());
		((ISupportInitialize)textBlock).BeginInit();
		children.Add(textBlock);
		textBlock2.Name = "TitleTextBlock";
		object element = textBlock2;
		context.AvaloniaNameScope.Register("TitleTextBlock", element);
		textBlock2.FontSize = 20.0;
		textBlock2.FontWeight = FontWeight.DemiBold;
		textBlock2.HorizontalAlignment = HorizontalAlignment.Center;
		((ISupportInitialize)textBlock2).EndInit();
		Controls children2 = stackPanel4.Children;
		TextBlock textBlock4;
		TextBlock textBlock3 = (textBlock4 = new TextBlock());
		((ISupportInitialize)textBlock3).BeginInit();
		children2.Add(textBlock3);
		textBlock4.Name = "MessageTextBlock";
		element = textBlock4;
		context.AvaloniaNameScope.Register("MessageTextBlock", element);
		textBlock4.TextWrapping = TextWrapping.Wrap;
		textBlock4.TextAlignment = TextAlignment.Center;
		((ISupportInitialize)textBlock4).EndInit();
		Controls children3 = stackPanel4.Children;
		TextBlock textBlock6;
		TextBlock textBlock5 = (textBlock6 = new TextBlock());
		((ISupportInitialize)textBlock5).BeginInit();
		children3.Add(textBlock5);
		TextBlock textBlock8;
		TextBlock textBlock7 = (textBlock8 = textBlock6);
		context.PushParent(textBlock8);
		textBlock8.Name = "DownloadUrlTextBlock";
		element = textBlock8;
		context.AvaloniaNameScope.Register("DownloadUrlTextBlock", element);
		textBlock8.TextWrapping = TextWrapping.Wrap;
		textBlock8.TextAlignment = TextAlignment.Center;
		StyledProperty<IBrush?> foregroundProperty = TextBlock.ForegroundProperty;
		DynamicResourceExtension dynamicResourceExtension2 = new DynamicResourceExtension("SystemAccentColorBrush");
		context.ProvideTargetProperty = TextBlock.ForegroundProperty;
		BindingBase binding2 = dynamicResourceExtension2.ProvideValue(context);
		context.ProvideTargetProperty = null;
		textBlock8.Bind(foregroundProperty, binding2);
		context.PopParent();
		((ISupportInitialize)textBlock7).EndInit();
		Controls children4 = stackPanel4.Children;
		Button button2;
		Button button = (button2 = new Button());
		((ISupportInitialize)button).BeginInit();
		children4.Add(button);
		button2.Name = "UpgradeButton";
		element = button2;
		context.AvaloniaNameScope.Register("UpgradeButton", element);
		button2.Classes.Add("accent");
		button2.HorizontalAlignment = HorizontalAlignment.Center;
		button2.AddHandler((RoutedEvent)Button.ClickEvent, (Delegate)new EventHandler<RoutedEventArgs>(context.RootObject.OnUpgradeButtonClick), RoutingStrategies.Direct | RoutingStrategies.Bubble, false);
		((ISupportInitialize)button2).EndInit();
		context.PopParent();
		((ISupportInitialize)stackPanel3).EndInit();
		context.PopParent();
		((ISupportInitialize)border3).EndInit();
		context.PopParent();
		((ISupportInitialize)P_1).EndInit();
		if (P_1 is StyledElement styled)
		{
			NameScope.SetNameScope(styled, context.AvaloniaNameScope);
		}
		context.AvaloniaNameScope.Complete();
	}

	[CompilerGenerated]
	private static void _0021XamlIlPopulateTrampoline(ExpiredVersionDialog P_0)
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
