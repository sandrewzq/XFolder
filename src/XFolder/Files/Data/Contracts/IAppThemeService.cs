using System;

namespace Files.Data.Contracts;

public interface IAppThemeService
{
	string CurrentThemeCode { get; }

	event EventHandler? ThemeChanged;

	void ApplySavedTheme();

	bool TryChangeTheme(string? themeCode);
}
