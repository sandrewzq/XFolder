using System;
using System.Collections.Generic;
using System.Globalization;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Data.Converters;
using Avalonia.Media;

namespace Files.Converters;

public class PaneBackgroundConverter : IMultiValueConverter
{
	public object? Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
	{
		bool flag = default;
		int num;
		if (values.Count > 0)
		{
			object obj = values[0];
			if (obj is bool)
			{
				flag = (bool)obj;
				num = 1;
			}
			else
			{
				num = 0;
			}
		}
		else
		{
			num = 0;
		}
		string key = ((((uint)num & (flag ? 1u : 0u)) != 0) ? "PaneActiveBackground" : "PaneInactiveBackground");
		IImmutableSolidColorBrush transparent = Brushes.Transparent;
		Application current = Application.Current;
		if (current == null)
		{
			return transparent;
		}
		if (current.TryFindResource(key, current.ActualThemeVariant, out object value) || current.TryFindResource(key, null, out value))
		{
			if (value is IBrush result)
			{
				return result;
			}
			if (value is Color color)
			{
				return new SolidColorBrush(color);
			}
		}
		return transparent;
	}
}
