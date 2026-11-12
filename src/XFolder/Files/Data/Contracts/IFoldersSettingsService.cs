using System.ComponentModel;
using Files.Data.Enums;

namespace Files.Data.Contracts;

public interface IFoldersSettingsService : IBaseSettingsService, INotifyPropertyChanged
{
	bool ShowHiddenItems { get; set; }

	bool ShowFileExtensions { get; set; }

	bool ShowThumbnails { get; set; }

	bool ScrollToPreviousFolderWhenNavigatingUp { get; set; }

	bool DoubleClickToGoUp { get; set; }

	DeleteConfirmationPolicies DeleteConfirmationPolicy { get; set; }

	bool ShowFileExtensionWarning { get; set; }
}
