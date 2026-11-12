using System.ComponentModel;
using Files.Data.Contracts;
using Files.Data.Enums;
using Files.Utils.Serialization;

namespace Files.Services.Settings;

internal sealed class LayoutSettingsService : BaseObservableJsonSettings, ILayoutSettingsService, IBaseSettingsService, INotifyPropertyChanged
{
	public FolderLayoutModes DefaultLayoutMode
	{
		get
		{
			return (FolderLayoutModes)Get(0L, "DefaultLayoutMode");
		}
		set
		{
			Set((long)value, "DefaultLayoutMode");
		}
	}

	public SortOption DefaultSortOption
	{
		get
		{
			return (SortOption)Get(0L, "DefaultSortOption");
		}
		set
		{
			Set((long)value, "DefaultSortOption");
		}
	}

	public SortDirection DefaultSortDirection
	{
		get
		{
			return (SortDirection)Get(0L, "DefaultSortDirection");
		}
		set
		{
			Set((long)value, "DefaultSortDirection");
		}
	}

	public LayoutSettingsService(ISettingsSharingContext settingsSharingContext)
	{
		RegisterSettingsContext(settingsSharingContext);
	}
}
