using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace Files.Converters;

public class FontSizeToIconSizeConverter : IValueConverter
{
	private const double DefaultScale = 1.3846153846153846;

	public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
	{
		double num2;
		if (value is double num)
		{
			num2 = num;
		}
		else if (value is int num3)
		{
			num2 = num3;
		}
		else
		{
			num2 = ((!(value is float num4)) ? 13.0 : ((double)num4));
		}
		double num5 = num2;
		double num6 = 1.3846153846153846;
		if (parameter is string s && double.TryParse(s, NumberStyles.Float, CultureInfo.InvariantCulture, out var result))
		{
			num6 = result;
		}
		return Math.Max(9.0, num5 * num6);
	}

	public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
	{
		throw new NotSupportedException();
	}
}
