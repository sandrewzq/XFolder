using System.ComponentModel;
using Files.Data.Contracts;
using Files.Data.Enums;
using Files.Utils.Serialization;

namespace Files.Services.Settings;

internal sealed class FoldersSettingsService : BaseObservableJsonSettings, IFoldersSettingsService, IBaseSettingsService, INotifyPropertyChanged
{
	public bool ShowHiddenItems
	{
		get
		{
			return Get(defaultValue: false, "ShowHiddenItems");
		}
		set
		{
			Set(value, "ShowHiddenItems");
		}
	}

	public bool ShowFileExtensions
	{
		get
		{
			return Get(defaultValue: true, "ShowFileExtensions");
		}
		set
		{
			Set(value, "ShowFileExtensions");
		}
	}

	public bool ShowThumbnails
	{
		get
		{
			return Get(defaultValue: true, "ShowThumbnails");
		}
		set
		{
			Set(value, "ShowThumbnails");
		}
	}

	public bool ScrollToPreviousFolderWhenNavigatingUp
	{
		get
		{
			return Get(defaultValue: true, "ScrollToPreviousFolderWhenNavigatingUp");
		}
		set
		{
			Set(value, "ScrollToPreviousFolderWhenNavigatingUp");
		}
	}

	public bool DoubleClickToGoUp
	{
		get
		{
			return Get(defaultValue: true, "DoubleClickToGoUp");
		}
		set
		{
			Set(value, "DoubleClickToGoUp");
		}
	}

	public DeleteConfirmationPolicies DeleteConfirmationPolicy
	{
		get
		{
			return (DeleteConfirmationPolicies)Get(0L, "DeleteConfirmationPolicy");
		}
		set
		{
			Set((long)value, "DeleteConfirmationPolicy");
		}
	}

	public bool ShowFileExtensionWarning
	{
		get
		{
			return Get(defaultValue: true, "ShowFileExtensionWarning");
		}
		set
		{
			Set(value, "ShowFileExtensionWarning");
		}
	}

	public FoldersSettingsService(ISettingsSharingContext settingsSharingContext)
	{
		RegisterSettingsContext(settingsSharingContext);
	}
}
