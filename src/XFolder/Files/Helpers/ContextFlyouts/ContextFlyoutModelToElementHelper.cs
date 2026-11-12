using System.Collections.Generic;
using Avalonia.Controls;
using Files.Data.Enums;
using Files.Data.Models;

namespace Files.Helpers.ContextFlyouts;

public static class ContextFlyoutModelToElementHelper
{
	public static List<object> ToContextMenuItemsSource(List<ContextMenuFlyoutItemViewModel> items)
	{
		if (items == null || items.Count == 0)
		{
			return null;
		}
		List<object> list = new List<object>();
		foreach (ContextMenuFlyoutItemViewModel item in items)
		{
			object obj = CreateMenuItem(item);
			if (obj != null)
			{
				list.Add(obj);
			}
		}
		return list;
	}

	public static object CreateMenuItem(ContextMenuFlyoutItemViewModel vm)
	{
		if (!vm.ShowItem)
		{
			return null;
		}
		if (vm.ItemType == ContextMenuFlyoutItemType.Separator)
		{
			return new Separator();
		}
		MenuItem menuItem = new MenuItem
		{
			Header = vm.Text,
			InputGesture = vm.InputGesture,
			Command = vm.Command,
			CommandParameter = vm.CommandParameter,
			IsEnabled = vm.IsEnabled,
			IsChecked = vm.IsChecked
		};
		if (vm.Icon != null)
		{
			menuItem.Icon = new PathIcon
			{
				Data = vm.Icon
			};
		}
		if (vm.ItemType == ContextMenuFlyoutItemType.Toggle)
		{
			menuItem.ToggleType = MenuItemToggleType.CheckBox;
		}
		menuItem.ItemsSource = ToContextMenuItemsSource(vm.Items);
		return menuItem;
	}
}
