using System;
using System.Collections.Generic;
using System.Globalization;
using Avalonia.Data.Converters;
using Files.Extensions;

namespace Files.Converters;

public class LocalizedCountTextConverter : IMultiValueConverter
{
	public object? Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
	{
		if (values == null || values.Count < 3)
		{
			return string.Empty;
		}
		if (!TryGetCount(values[0], out var count))
		{
			return string.Empty;
		}
		string text = values[1] as string;
		string text2 = values[2] as string;
		if (values.Count >= 5 && TryGetCount(values[3], out var count2) && count2 > 0)
		{
			string text3 = values[4] as string;
			if (!string.IsNullOrWhiteSpace(text3))
			{
				return text3.GetLocalizedFormatResource(count2, count);
			}
		}
		string text4 = ((count == 1) ? text : text2);
		if (string.IsNullOrWhiteSpace(text4))
		{
			return string.Empty;
		}
		return text4.GetLocalizedFormatResource(count);
	}

	private static bool TryGetCount(object? value, out int count)
	{
		if (!(value is int num))
		{
			if (!(value is long num2))
			{
				if (value is string s && int.TryParse(s, out var result))
				{
					count = result;
					return true;
				}
			}
			else
			{
				long num3 = num2;
				if (num3 >= int.MinValue && num3 <= int.MaxValue)
				{
					count = (int)num3;
					return true;
				}
			}
			count = 0;
			return false;
		}
		int num4 = num;
		count = num4;
		return true;
	}
}
