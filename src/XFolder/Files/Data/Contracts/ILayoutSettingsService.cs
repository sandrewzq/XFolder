using System.ComponentModel;
using Files.Data.Enums;

namespace Files.Data.Contracts;

public interface ILayoutSettingsService : IBaseSettingsService, INotifyPropertyChanged
{
	FolderLayoutModes DefaultLayoutMode { get; set; }

	SortOption DefaultSortOption { get; set; }

	SortDirection DefaultSortDirection { get; set; }
}
