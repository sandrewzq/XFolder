using System.ComponentModel;
using Files.Data.Contracts;
using Files.Utils.Serialization;

namespace Files.Services.Settings;

internal sealed class AppearanceSettingsService : BaseObservableJsonSettings, IAppearanceSettingsService, IBaseSettingsService, INotifyPropertyChanged
{
	public string AppThemeMode
	{
		get
		{
			return Get("Default", "AppThemeMode") ?? "Default";
		}
		set
		{
			Set(value, "AppThemeMode");
		}
	}

	public AppearanceSettingsService(ISettingsSharingContext settingsSharingContext)
	{
		RegisterSettingsContext(settingsSharingContext);
	}
}
