using System;
using System.Collections.Generic;
using System.Globalization;
using Avalonia.Data.Converters;
using Files.Helpers;

namespace Files.Converters;

public class FileSizeConverter : IMultiValueConverter
{
	public object? Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
	{
		if (values == null || values.Count < 2)
		{
			return string.Empty;
		}
		bool flag = false;
		if (values[1] is bool flag2)
		{
			flag = flag2;
		}
		if (flag)
		{
			return string.Empty;
		}
		long bytes = 0L;
		long result;
		if (values[0] is long num)
		{
			bytes = num;
		}
		else if (values[0] is string s && long.TryParse(s, out result))
		{
			bytes = result;
		}
		return StringsHelper.FormatFileSize(bytes);
	}
}
