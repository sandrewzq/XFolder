using System;
using System.Globalization;
using System.IO;
using Avalonia.Data;
using Avalonia.Data.Converters;
using Avalonia.Media.Imaging;
using Avalonia.Platform;

namespace Files.Converters;

public class FileFolderIconConverter : IValueConverter
{
	public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
	{
		if (value is bool flag)
		{
			using Stream stream = AssetLoader.Open(new Uri("avares://XFolder/Assets/image/file.png"));
			using Stream stream2 = AssetLoader.Open(new Uri("avares://XFolder/Assets/image/folder.png"));
			Bitmap bitmap = new Bitmap(stream);
			Bitmap bitmap2 = new Bitmap(stream2);
			return flag ? bitmap2 : bitmap;
		}
		return null;
	}

	public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
	{
		return BindingOperations.DoNothing;
	}
}
