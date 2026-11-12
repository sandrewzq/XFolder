using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Styling;
using Files.Assets.Lang;
using Files.Data.Contracts;
using Files.Data.Models;

namespace Files.Services.App;

public sealed class AppThemeService : IAppThemeService
{
	private readonly IAppearanceSettingsService _appearanceSettingsService;

	public string CurrentThemeCode { get; private set; } = string.Empty;

	public IReadOnlyList<AppThemeItem> SupportedThemes { get; } = new _003C_003Ez__ReadOnlyArray<AppThemeItem>(new AppThemeItem[3]
	{
		new AppThemeItem
		{
			Code = "auto",
			DisplayName = Resources.Auto
		},
		new AppThemeItem
		{
			Code = "light",
			DisplayName = Resources.LightTheme
		},
		new AppThemeItem
		{
			Code = "dark",
			DisplayName = Resources.DarkTheme
		}
	});

	public event EventHandler? ThemeChanged;

	public AppThemeService(IAppearanceSettingsService appearanceSettingsService)
	{
		_appearanceSettingsService = appearanceSettingsService;
	}

	public void ApplySavedTheme()
	{
		string themeCode = ReadSavedThemeCodeFromSettings();
		ApplyTheme(themeCode, saveToDisk: false, notify: false);
	}

	public bool TryChangeTheme(string? themeCode)
	{
		if (string.IsNullOrWhiteSpace(themeCode))
		{
			return false;
		}
		string text = NormalizeThemeCode(themeCode);
		if (CurrentThemeCode.Equals(text, StringComparison.OrdinalIgnoreCase))
		{
			return false;
		}
		ApplyTheme(text, saveToDisk: true, notify: true);
		return true;
	}

	private void ApplyTheme(string? themeCode, bool saveToDisk, bool notify)
	{
		string text = NormalizeThemeCode(themeCode);
		ThemeVariant requestedThemeVariant = ToRequestedThemeVariant(text);
		if (Application.Current != null)
		{
			Application.Current.RequestedThemeVariant = requestedThemeVariant;
		}
		CurrentThemeCode = text;
		if (saveToDisk)
		{
			_appearanceSettingsService.AppThemeMode = text;
		}
		if (notify)
		{
			ThemeChanged?.Invoke(this, EventArgs.Empty);
		}
	}

	private string ReadSavedThemeCodeFromSettings()
	{
		return NormalizeThemeCode(_appearanceSettingsService.AppThemeMode);
	}

	private string NormalizeThemeCode(string? themeCode)
	{
		if (themeCode != null && themeCode.Equals("light", StringComparison.OrdinalIgnoreCase))
		{
			return "light";
		}
		if (themeCode != null && themeCode.Equals("dark", StringComparison.OrdinalIgnoreCase))
		{
			return "dark";
		}
		return GetDefaultThemeCode();
	}

	private static ThemeVariant ToRequestedThemeVariant(string normalizedCode)
	{
		if (!(normalizedCode == "light"))
		{
			if (normalizedCode == "dark")
			{
				return ThemeVariant.Dark;
			}
			return ThemeVariant.Default;
		}
		return ThemeVariant.Light;
	}

	private string GetDefaultThemeCode()
	{
		return "auto";
	}
}
