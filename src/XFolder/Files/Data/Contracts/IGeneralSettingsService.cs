using System.ComponentModel;

namespace Files.Data.Contracts;

public interface IGeneralSettingsService : IBaseSettingsService, INotifyPropertyChanged
{
	string AppLanguageCode { get; set; }

	string NewTabDefaultPath { get; set; }

	bool ContinueLastSessionOnStartUp { get; set; }

	bool OpenMainWindowMaximized { get; set; }

	int FontSize { get; set; }
}
