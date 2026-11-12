using System.Collections.Generic;
using System.IO;
using System.Linq;
using CommunityToolkit.Mvvm.DependencyInjection;
using Files.Data.Contracts;
using Files.Data.EventArguments;
using Files.Helpers.Shared;
using Files.Utils.Serialization;
using Files.Utils.Serialization.Implementation;

namespace Files.Services.Settings;

internal sealed class UserSettingsService : BaseJsonSettings, IUserSettingsService, IBaseSettingsService
{
	private IGeneralSettingsService _generalSettingsService;

	private IFoldersSettingsService _foldersSettingsService;

	private IAppearanceSettingsService _appearanceSettingsService;

	private ILayoutSettingsService _layoutSettingsService;

	public IGeneralSettingsService GeneralSettingsService => GetSettingsService(ref _generalSettingsService);

	public IFoldersSettingsService FoldersSettingsService => GetSettingsService(ref _foldersSettingsService);

	public IAppearanceSettingsService AppearanceSettingsService => GetSettingsService(ref _appearanceSettingsService);

	public ILayoutSettingsService LayoutSettingsService => GetSettingsService(ref _layoutSettingsService);

	public UserSettingsService()
	{
		SettingsSerializer = new DefaultSettingsSerializer();
		string filePath = Path.Combine(Path.Combine(PathHelpers.GetAppDataDirectory(), "cache"), "settings.json");
		Initialize(filePath);
		JsonSettingsSerializer = new DefaultJsonSettingsSerializer();
		JsonSettingsDatabase = new CachingJsonSettingsDatabase(SettingsSerializer, JsonSettingsSerializer);
	}

	public override object ExportSettings()
	{
		if (!(base.ExportSettings() is IDictionary<string, object> obj))
		{
			return string.Empty;
		}
		return JsonSettingsSerializer?.SerializeToJson(obj) ?? string.Empty;
	}

	public override bool ImportSettings(object import)
	{
		Dictionary<string, object> dictionary;
		if (import is string json)
		{
			dictionary = JsonSettingsSerializer?.DeserializeFromJson<Dictionary<string, object>>(json) ?? new Dictionary<string, object>();
		}
		else
		{
			dictionary = ((!(import is Dictionary<string, object> dictionary2)) ? new Dictionary<string, object>() : dictionary2);
		}
		Dictionary<string, object> dictionary3 = dictionary;
		if (!dictionary3.Any() || !base.ImportSettings(dictionary3))
		{
			return false;
		}
		foreach (KeyValuePair<string, object> item in dictionary3)
		{
			RaiseOnSettingChangedEvent(this, new SettingChangedEventArgs(item.Key, item.Value));
		}
		return true;
	}

	private TSettingsService GetSettingsService<TSettingsService>(ref TSettingsService settingsServiceMember) where TSettingsService : class, IBaseSettingsService
	{
		if (settingsServiceMember == null)
		{
			settingsServiceMember = Ioc.Default.GetRequiredService<TSettingsService>();
		}
		return settingsServiceMember;
	}
}
