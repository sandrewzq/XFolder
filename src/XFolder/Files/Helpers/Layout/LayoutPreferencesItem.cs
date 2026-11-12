using System;
using CommunityToolkit.Mvvm.DependencyInjection;
using Files.Data.Contracts;
using Files.Data.Enums;

namespace Files.Helpers.Layout;

public sealed class LayoutPreferencesItem
{
	private IUserSettingsService UserSettingsService { get; } = Ioc.Default.GetRequiredService<IUserSettingsService>();

	public FolderLayoutModes LayoutMode { get; set; }

	public LayoutPreferencesItem()
	{
		LayoutMode = FolderLayoutModes.DetailsView;
	}

	public override bool Equals(object? obj)
	{
		if (obj == null)
		{
			return false;
		}
		if (obj == this)
		{
			return true;
		}
		if (obj is LayoutPreferencesItem layoutPreferencesItem)
		{
			return layoutPreferencesItem.LayoutMode == LayoutMode;
		}
		return base.Equals(obj);
	}

	public override int GetHashCode()
	{
		HashCode hashCode = default;
		hashCode.Add(LayoutMode);
		return hashCode.ToHashCode();
	}
}
