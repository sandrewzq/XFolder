using System;
using System.Collections.Generic;
using System.Globalization;
using Avalonia.Controls;
using Avalonia.Data.Converters;
using Files.Data.Enums;
using Files.ViewModels.Items;

namespace Files.Converters;

public class LayoutModeToVMConverter : IMultiValueConverter
{
	public object? Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
	{
		if (values.Count == 4 && values[0] is FolderLayoutModes && values[1] is string && values[2] is TabItemViewModel tabItemViewModel && values[3] is ContentControl owner)
		{
			return tabItemViewModel.GetOrCreateLayoutView(owner);
		}
		return null;
	}
}
