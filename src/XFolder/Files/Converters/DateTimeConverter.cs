using System;
using System.Globalization;
using Avalonia.Data;
using Avalonia.Data.Converters;

namespace Files.Converters;

public class DateTimeConverter : IValueConverter
{
	public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
	{
		if (value is DateTimeOffset dateTimeOffset)
		{
			return dateTimeOffset.ToLocalTime().ToString("yyyy-MM-dd HH:mm");
		}
		if (value is DateTime dateTime)
		{
			return dateTime.ToString("yyyy-MM-dd HH:mm");
		}
		return string.Empty;
	}

	public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
	{
		return BindingOperations.DoNothing;
	}
}
