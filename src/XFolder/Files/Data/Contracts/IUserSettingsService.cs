using System;
using Files.Data.EventArguments;

namespace Files.Data.Contracts;

public interface IUserSettingsService : IBaseSettingsService
{
	IGeneralSettingsService GeneralSettingsService { get; }

	IFoldersSettingsService FoldersSettingsService { get; }

	IAppearanceSettingsService AppearanceSettingsService { get; }

	ILayoutSettingsService LayoutSettingsService { get; }

	event EventHandler<SettingChangedEventArgs> OnSettingChangedEvent;

	bool ImportSettings(object import);

	object ExportSettings();
}
