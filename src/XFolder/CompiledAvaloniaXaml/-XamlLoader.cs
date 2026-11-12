using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Avalonia.Markup.Xaml.XamlIl.Runtime;
using Files;
using Files.Dialogs;
using Files.UserControls.BreadcrumbBar;
using Files.UserControls.NavigationToolbar;
using Files.UserControls.Sidebar;
using Files.Views;
using Files.Views.Layouts;
using Files.Views.Properties;
using Files.Views.Shells;

namespace CompiledAvaloniaXaml;

[EditorBrowsable(EditorBrowsableState.Never)]
[CompilerGenerated]
public class _0021XamlLoader
{
	public static object TryLoad(IServiceProvider P_0, string P_1)
	{
		if (string.Equals(P_1, "avares://XFolder/App.axaml", StringComparison.OrdinalIgnoreCase))
		{
			return new App();
		}
		if (string.Equals(P_1, "avares://XFolder/Assets/icons.axaml", StringComparison.OrdinalIgnoreCase))
		{
			return _0021AvaloniaResources.Build_003A_002FAssets_002Ficons_002Eaxaml(XamlIlRuntimeHelpers.CreateRootServiceProviderV3(P_0));
		}
		if (string.Equals(P_1, "avares://XFolder/Dialogs/BulkRenameDialog.axaml", StringComparison.OrdinalIgnoreCase))
		{
			return new BulkRenameDialog();
		}
		if (string.Equals(P_1, "avares://XFolder/Dialogs/CreateItemDialog.axaml", StringComparison.OrdinalIgnoreCase))
		{
			return new CreateItemDialog();
		}
		if (string.Equals(P_1, "avares://XFolder/Dialogs/DynamicDialog.axaml", StringComparison.OrdinalIgnoreCase))
		{
			return new DynamicDialog();
		}
		if (string.Equals(P_1, "avares://XFolder/Dialogs/ExpiredVersionDialog.axaml", StringComparison.OrdinalIgnoreCase))
		{
			return new ExpiredVersionDialog();
		}
		if (string.Equals(P_1, "avares://XFolder/Dialogs/ProgressDialog.axaml", StringComparison.OrdinalIgnoreCase))
		{
			return new ProgressDialog();
		}
		if (string.Equals(P_1, "avares://XFolder/UserControls/BreadcrumbBar/BreadcrumbBar.axaml", StringComparison.OrdinalIgnoreCase))
		{
			return new BreadcrumbBar();
		}
		if (string.Equals(P_1, "avares://XFolder/UserControls/NavigationToolbar/NavigationToolbarView.axaml", StringComparison.OrdinalIgnoreCase))
		{
			return new NavigationToolbarView();
		}
		if (string.Equals(P_1, "avares://XFolder/UserControls/Sidebar/SidebarView.axaml", StringComparison.OrdinalIgnoreCase))
		{
			return new SidebarView();
		}
		if (string.Equals(P_1, "avares://XFolder/Views/Layouts/DetailsLayoutView.axaml", StringComparison.OrdinalIgnoreCase))
		{
			return new DetailsLayoutView();
		}
		if (string.Equals(P_1, "avares://XFolder/Views/Layouts/GridLayoutView.axaml", StringComparison.OrdinalIgnoreCase))
		{
			return new GridLayoutView();
		}
		if (string.Equals(P_1, "avares://XFolder/Views/MainWindow.axaml", StringComparison.OrdinalIgnoreCase))
		{
			return new MainWindow();
		}
		if (string.Equals(P_1, "avares://XFolder/Views/Properties/HashesWindow.axaml", StringComparison.OrdinalIgnoreCase))
		{
			return new HashesWindow();
		}
		if (string.Equals(P_1, "avares://XFolder/Views/SettingsWindow.axaml", StringComparison.OrdinalIgnoreCase))
		{
			return new SettingsWindow();
		}
		if (string.Equals(P_1, "avares://XFolder/Views/ShellPanesView.axaml", StringComparison.OrdinalIgnoreCase))
		{
			return new ShellPanesView();
		}
		if (string.Equals(P_1, "avares://XFolder/Views/Shells/ModernShellView.axaml", StringComparison.OrdinalIgnoreCase))
		{
			return new ModernShellView
			{
				PaneHolder = null!
			};
		}
		return null;
	}

	public static object TryLoad(string P_0)
	{
		return TryLoad(null, P_0);
	}
}
