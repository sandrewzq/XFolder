using System;
using System.Globalization;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Data.Converters;

namespace Files.Converters;

public class IsPaneOpenToGridLengthConverter : IValueConverter
{
	public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
	{
		if (value is bool)
		{
			return ((bool)value) ? new GridLength(1.0, GridUnitType.Auto) : new GridLength(0.0);
		}
		return new GridLength(0.0);
	}

	public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
	{
		return BindingOperations.DoNothing;
	}
}
