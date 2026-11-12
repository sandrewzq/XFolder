using System.ComponentModel;

namespace Files.Data.Contracts;

public interface IAppearanceSettingsService : IBaseSettingsService, INotifyPropertyChanged
{
	string AppThemeMode { get; set; }
}
