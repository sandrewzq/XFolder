using System;
using Avalonia.Controls;
using Files.Data.Items;

namespace Files.UserControls.BreadcrumbBar;

public sealed class BreadcrumbChevronClickedEventArgs(PathBoxItem item, Control targetControl) : EventArgs
{
	public PathBoxItem Item { get; } = item;

	public Control TargetControl { get; } = targetControl;
}
