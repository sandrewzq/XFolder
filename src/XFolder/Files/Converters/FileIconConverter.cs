using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Avalonia.Data;
using Avalonia.Data.Converters;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Files.Services.NativePlugin;

namespace Files.Converters;

public class FileIconConverter : IValueConverter, IMultiValueConverter
{
	private static Bitmap? s_fileIcon;

	private static Bitmap? s_folderCollapsedIcon;

	private static Bitmap? s_folderExpandedIcon;

	private static Bitmap? s_diskIcon;

	public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
	{
		if (value is string text)
		{
			if (!(text == "/"))
			{
				return GetFolderCollapsedIcon();
			}
			return GetDiskIcon();
		}
		if (value is bool)
		{
			if (!(bool)value)
			{
				return GetFileIcon();
			}
			return GetFolderCollapsedIcon();
		}
		return null;
	}

	public object? Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
	{
		if (values.Count == 3 && values[0] is bool flag && values[1] is bool flag2 && values[2] is string text)
		{
			if (text == "/")
			{
				return GetDiskIcon();
			}
			if (!flag)
			{
				return GetFileIcon();
			}
			if (!flag2)
			{
				return GetFolderCollapsedIcon();
			}
			return GetFolderExpandedIcon();
		}
		return null;
	}

	public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
	{
		return BindingOperations.DoNothing;
	}

	private static Bitmap GetFileIcon()
	{
		if (s_fileIcon != null)
		{
			return s_fileIcon;
		}
		using Stream stream = AssetLoader.Open(new Uri("avares://XFolder/Assets/image/file.png"));
		s_fileIcon = new Bitmap(stream);
		return s_fileIcon;
	}

	private static Bitmap GetFolderCollapsedIcon()
	{
		if (s_folderCollapsedIcon != null)
		{
			return s_folderCollapsedIcon;
		}
		using Stream stream = AssetLoader.Open(new Uri("avares://XFolder/Assets/image/folder.png"));
		s_folderCollapsedIcon = new Bitmap(stream);
		return s_folderCollapsedIcon;
	}

	private static Bitmap GetFolderExpandedIcon()
	{
		if (s_folderExpandedIcon != null)
		{
			return s_folderExpandedIcon;
		}
		using Stream stream = AssetLoader.Open(new Uri("avares://XFolder/Assets/image/folder-open.png"));
		s_folderExpandedIcon = new Bitmap(stream);
		return s_folderExpandedIcon;
	}

	private static Bitmap GetDiskIcon()
	{
		if (s_diskIcon != null)
		{
			return s_diskIcon;
		}
		s_diskIcon = (OperatingSystem.IsMacOS() ? MacPluginInterop.GetSidebarIconBitmapByPath("/") : null);
		if (s_diskIcon == null)
		{
			s_diskIcon = GetFolderCollapsedIcon();
		}
		return s_diskIcon;
	}
}
