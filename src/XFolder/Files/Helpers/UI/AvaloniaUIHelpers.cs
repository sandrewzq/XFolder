using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Media;
using Avalonia.Styling;
using Avalonia.VisualTree;

namespace Files.Helpers.UI;

public class AvaloniaUIHelpers
{
	public static T? FindVisualChildByName<T>(Visual parent, string name) where T : Control
	{
		foreach (Visual visualDescendant in parent.GetVisualDescendants())
		{
			if (visualDescendant is T val && val.Name == name)
			{
				return val;
			}
		}
		return null;
	}

	public static bool IsScrollBar(object? source)
	{
		if (source is Visual visual)
		{
			if (!(visual is ScrollBar))
			{
				return visual.FindAncestorOfType<ScrollBar>() != null;
			}
			return true;
		}
		return false;
	}

	public static bool IsDataGridColumnHeader(object? source)
	{
		if (source is Visual visual)
		{
			if (!(visual is DataGridColumnHeader))
			{
				return visual.FindAncestorOfType<DataGridColumnHeader>() != null;
			}
			return true;
		}
		return false;
	}

	public static StreamGeometry? GetIconResource(string key)
	{
		if (string.IsNullOrEmpty(key))
		{
			return null;
		}
		if (!Avalonia.Application.Current.TryGetResource(key, ThemeVariant.Default, out object value))
		{
			return null;
		}
		return value as StreamGeometry;
	}
}
