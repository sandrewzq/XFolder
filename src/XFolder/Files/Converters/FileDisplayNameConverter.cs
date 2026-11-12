using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Files.Services.NativePlugin;

namespace Files.Converters;

public class FileDisplayNameConverter : IValueConverter
{
	public static readonly FileDisplayNameConverter Instance = new FileDisplayNameConverter();

	private static string? s_rootVolumeName;

	public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
	{
		if (value is string text && text == "/")
		{
			if (s_rootVolumeName == null)
			{
				s_rootVolumeName = (OperatingSystem.IsMacOS() ? MacPluginInterop.GetVolumeName("/") : null);
			}
			return s_rootVolumeName ?? "Macintosh HD";
		}
		return value;
	}

	public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
	{
		throw new NotSupportedException();
	}
}
