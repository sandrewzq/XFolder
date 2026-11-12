using System;
using System.ComponentModel;
using Files.Data.Contracts;
using Files.Utils.Serialization;

namespace Files.Services.Settings;

internal sealed class GeneralSettingsService : BaseObservableJsonSettings, IGeneralSettingsService, IBaseSettingsService, INotifyPropertyChanged
{
	public string AppLanguageCode
	{
		get
		{
			return Get(string.Empty, "AppLanguageCode") ?? string.Empty;
		}
		set
		{
			Set(value, "AppLanguageCode");
		}
	}

	public string NewTabDefaultPath
	{
		get
		{
			return Get(string.Empty, "NewTabDefaultPath") ?? string.Empty;
		}
		set
		{
			Set(value, "NewTabDefaultPath");
		}
	}

	public bool ContinueLastSessionOnStartUp
	{
		get
		{
			return Get(defaultValue: true, "ContinueLastSessionOnStartUp");
		}
		set
		{
			Set(value, "ContinueLastSessionOnStartUp");
		}
	}

	public bool OpenMainWindowMaximized
	{
		get
		{
			return Get(defaultValue: false, "OpenMainWindowMaximized");
		}
		set
		{
			Set(value, "OpenMainWindowMaximized");
		}
	}

	public int FontSize
	{
		get
		{
			return Math.Clamp(Get(13, "FontSize"), 9, 24);
		}
		set
		{
			Set(Math.Clamp(value, 9, 24), "FontSize");
		}
	}

	public GeneralSettingsService(ISettingsSharingContext settingsSharingContext)
	{
		RegisterSettingsContext(settingsSharingContext);
	}
}
