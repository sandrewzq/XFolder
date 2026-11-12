using System;
using System.Collections.Generic;
using System.Globalization;
using Files.Assets.Lang;
using Files.Data.Contracts;
using Files.Data.Models;

namespace Files.Helpers.Application;

public static class AppLanguageHelper
{
	private static IGeneralSettingsService? _generalSettingsService;

	private static bool _initialized;

	public static IReadOnlyList<AppLanguageItem> SupportedLanguages { get; } = new _003C_003Ez__ReadOnlyArray<AppLanguageItem>(new AppLanguageItem[2]
	{
		new AppLanguageItem
		{
			Code = "zh-Hans",
			DisplayName = "简体中文"
		},
		new AppLanguageItem
		{
			Code = "en",
			DisplayName = "English"
		}
	});

	public static string CurrentLanguageCode { get; private set; } = string.Empty;

	public static event EventHandler? LanguageChanged;

	public static void Initialize(IGeneralSettingsService generalSettingsService)
	{
		_generalSettingsService = generalSettingsService;
		_initialized = true;
	}

	public static void ApplySavedLanguage()
	{
		ApplyLanguage(NormalizeLanguageCode(EnsureInitialized().AppLanguageCode), saveToSettings: false, notify: false);
	}

	public static bool TryChange(string? languageCode)
	{
		if (string.IsNullOrWhiteSpace(languageCode))
		{
			return false;
		}
		string text = NormalizeLanguageCode(languageCode);
		if (CurrentLanguageCode.Equals(text, StringComparison.OrdinalIgnoreCase))
		{
			return false;
		}
		ApplyLanguage(text, saveToSettings: true, notify: true);
		return true;
	}

	private static void ApplyLanguage(string languageCode, bool saveToSettings, bool notify)
	{
		Resources.Culture = (CultureInfo.DefaultThreadCurrentUICulture = (CultureInfo.DefaultThreadCurrentCulture = (CultureInfo.CurrentUICulture = (CultureInfo.CurrentCulture = new CultureInfo(languageCode)))));
		CurrentLanguageCode = languageCode;
		if (saveToSettings)
		{
			EnsureInitialized().AppLanguageCode = languageCode;
		}
		if (notify)
		{
			LanguageChanged?.Invoke(null, EventArgs.Empty);
		}
	}

	private static string NormalizeLanguageCode(string? languageCode)
	{
		if (string.IsNullOrWhiteSpace(languageCode))
		{
			return GetDefaultLanguageCode();
		}
		if (languageCode.StartsWith("zh", StringComparison.OrdinalIgnoreCase))
		{
			return "zh-Hans";
		}
		return "en";
	}

	private static string GetDefaultLanguageCode()
	{
		if (!CultureInfo.CurrentUICulture.Name.StartsWith("zh", StringComparison.OrdinalIgnoreCase))
		{
			return "en";
		}
		return "zh-Hans";
	}

	private static IGeneralSettingsService EnsureInitialized()
	{
		if (!_initialized || _generalSettingsService == null)
		{
			throw new InvalidOperationException("AppLanguageHelper is not initialized.");
		}
		return _generalSettingsService;
	}
}
