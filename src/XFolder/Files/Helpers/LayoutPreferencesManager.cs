using System;
using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;
using Files.Data.Enums;
using Files.Data.EventArguments;
using Files.Helpers.Layout;
using Files.Views.Layouts;

namespace Files.Helpers;

public sealed class LayoutPreferencesManager : ObservableObject
{
	private bool _IsLayoutModeChanging;

	private LayoutPreferencesItem? _LayoutPreferencesItem;

	public FolderLayoutModes LayoutMode
	{
		get
		{
			return LayoutPreferencesItem.LayoutMode;
		}
		set
		{
			if (SetProperty((LayoutPreferencesItem item) => item.LayoutMode, (LayoutPreferencesItem item) =>
			{
				item.LayoutMode = value;
			}, "LayoutMode"))
			{
				LayoutPreferencesUpdateRequired?.Invoke(this, new LayoutPreferenceEventArgs(LayoutPreferencesItem));
			}
		}
	}

	public bool IsLayoutModeChanging
	{
		get
		{
			return _IsLayoutModeChanging;
		}
		set
		{
			SetProperty(ref _IsLayoutModeChanging, value, "IsLayoutModeChanging");
		}
	}

	public LayoutPreferencesItem LayoutPreferencesItem
	{
		get
		{
			return _LayoutPreferencesItem;
		}
		private set
		{
			if (SetProperty(ref _LayoutPreferencesItem, value, "LayoutPreferencesItem"))
			{
				OnPropertyChanged("LayoutMode");
			}
		}
	}

	public event EventHandler<LayoutPreferenceEventArgs>? LayoutPreferencesUpdateRequired;

	public event EventHandler<LayoutModeEventArgs>? LayoutModeChangeRequested;

	public LayoutPreferencesManager()
	{
		LayoutPreferencesItem = new LayoutPreferencesItem();
	}

	public Type GetCurrentLayoutType()
	{
		FolderLayoutModes layoutMode = LayoutPreferencesItem.LayoutMode;
		return layoutMode switch
		{
			FolderLayoutModes.DetailsView => typeof(DetailsLayoutView), 
			FolderLayoutModes.GridView => typeof(GridLayoutView), 
		};
	}

	public void ToggleLayoutModeGridView()
	{
		LayoutMode = FolderLayoutModes.GridView;
		LayoutModeChangeRequested?.Invoke(this, new LayoutModeEventArgs(FolderLayoutModes.GridView));
	}

	public void ToggleLayoutModeDetailsView()
	{
		LayoutMode = FolderLayoutModes.DetailsView;
		LayoutModeChangeRequested?.Invoke(this, new LayoutModeEventArgs(FolderLayoutModes.DetailsView));
	}

	public static void SetDefaultLayoutPreferences()
	{
	}

	private bool SetProperty<TValue>(Func<LayoutPreferencesItem, TValue> prop, Action<LayoutPreferencesItem> update, string propertyName)
	{
		TValue y = prop(LayoutPreferencesItem);
		update(LayoutPreferencesItem);
		if (EqualityComparer<TValue>.Default.Equals(prop(LayoutPreferencesItem), y))
		{
			return false;
		}
		OnPropertyChanged(propertyName);
		return true;
	}
}
