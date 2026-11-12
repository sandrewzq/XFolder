using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;

namespace Files.Extensions;

public static class ApplicationExtensions
{
	public static TopLevel? GetTopLevel(this Application? app)
	{
		if (app?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime classicDesktopStyleApplicationLifetime)
		{
			return classicDesktopStyleApplicationLifetime.MainWindow;
		}
		if (app?.ApplicationLifetime is ISingleViewApplicationLifetime singleViewApplicationLifetime)
		{
			if (singleViewApplicationLifetime.MainView != null)
			{
				return TopLevel.GetTopLevel(singleViewApplicationLifetime.MainView);
			}
			return null;
		}
		return null;
	}
}
