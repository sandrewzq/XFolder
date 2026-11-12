using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Input;
using Files.Assets.Lang;
using Files.Data.Commands.HotKey;
using Files.Views;

namespace Files.Actions.Navigation.Open;

internal sealed class OpenSettingsAction : IAction
{
	public string Label => Resources.Settings;

	public string Description => Resources.OpenSettingsDescription;

	public string? Icon => "settings_regular";

	public HotKey HotKey => new HotKey(Key.OemComma, KeyModifiers.Meta);

	public bool IsExecutable => true;

	public Task ExecuteAsync(object? parameter = null)
	{
		if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime classicDesktopStyleApplicationLifetime)
		{
			Window mainWindow = classicDesktopStyleApplicationLifetime.MainWindow;
			if (mainWindow != null)
			{
				foreach (Window window in classicDesktopStyleApplicationLifetime.Windows)
				{
					if (window is SettingsWindow settingsWindow)
					{
						settingsWindow.Activate();
						return Task.CompletedTask;
					}
				}
				new SettingsWindow().ShowDialog(mainWindow);
			}
		}
		return Task.CompletedTask;
	}
}
