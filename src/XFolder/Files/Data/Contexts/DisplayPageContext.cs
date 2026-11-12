using System;
using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using Files.Data.Enums;
using Files.Helpers;

namespace Files.Data.Contexts;

internal sealed class DisplayPageContext : ObservableObject, IDisplayPageContext, INotifyPropertyChanging, INotifyPropertyChanged
{
	private readonly IMultiPanesContext context = Ioc.Default.GetRequiredService<IMultiPanesContext>();

	private LayoutTypes _LayoutType;

	public LayoutTypes LayoutType
	{
		get
		{
			return _LayoutType;
		}
		set
		{
			LayoutPreferencesManager folderSettings = FolderSettings;
			if (folderSettings != null)
			{
				switch (value)
				{
				case LayoutTypes.Details:
					folderSettings.ToggleLayoutModeDetailsView();
					break;
				case LayoutTypes.Grid:
					folderSettings.ToggleLayoutModeGridView();
					break;
				}
			}
		}
	}

	private LayoutPreferencesManager? FolderSettings => context.ActivePane?.InstanceViewModel.FolderSettings;

	public DisplayPageContext()
	{
		context.ActivePaneChanging += Context_Changing;
		context.ActivePaneChanged += Context_Changed;
	}

	private void Context_Changing(object? sender, EventArgs e)
	{
		LayoutPreferencesManager folderSettings = FolderSettings;
		if (folderSettings != null)
		{
			folderSettings.PropertyChanged -= FolderSettings_PropertyChanged;
		}
		Update();
	}

	private void Context_Changed(object? sender, EventArgs e)
	{
		LayoutPreferencesManager folderSettings = FolderSettings;
		if (folderSettings != null)
		{
			folderSettings.PropertyChanged += FolderSettings_PropertyChanged;
		}
		Update();
	}

	private void FolderSettings_PropertyChanged(object? sender, PropertyChangedEventArgs e)
	{
		if (FolderSettings != null && e.PropertyName == "LayoutMode")
		{
			SetProperty(ref _LayoutType, GetLayoutType(), "LayoutType");
		}
	}

	private void Update()
	{
		if (FolderSettings == null)
		{
			SetProperty(ref _LayoutType, LayoutTypes.None, "LayoutType");
		}
		else
		{
			SetProperty(ref _LayoutType, GetLayoutType(), "LayoutType");
		}
	}

	private LayoutTypes GetLayoutType()
	{
		LayoutPreferencesManager folderSettings = FolderSettings;
		if (folderSettings == null)
		{
			return LayoutTypes.None;
		}
		return folderSettings.LayoutMode switch
		{
			FolderLayoutModes.DetailsView => LayoutTypes.Details, 
			FolderLayoutModes.ListView => LayoutTypes.List, 
			FolderLayoutModes.CardsView => LayoutTypes.Cards, 
			FolderLayoutModes.GridView => LayoutTypes.Grid, 
			FolderLayoutModes.ColumnView => LayoutTypes.Columns, 
			_ => throw new InvalidEnumArgumentException(), 
		};
	}
}
