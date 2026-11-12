using System.Collections.Generic;
using System.Windows.Input;
using Avalonia.Input;
using Avalonia.Media;
using Files.Data.Enums;

namespace Files.Data.Models;

public sealed class ContextMenuFlyoutItemViewModel
{
	public bool ShowItem { get; set; } = true;

	public ICommand? Command { get; set; }

	public object? CommandParameter { get; set; }

	public string Text { get; set; } = string.Empty;

	public string? HotKeyText { get; set; }

	public StreamGeometry? Icon { get; set; }

	public KeyGesture? InputGesture { get; set; }

	public object? Tag { get; set; }

	public ContextMenuFlyoutItemType ItemType { get; set; }

	public List<ContextMenuFlyoutItemViewModel> Items { get; set; } = new List<ContextMenuFlyoutItemViewModel>();

	public bool ShowOnShift { get; set; }

	public bool SingleItemOnly { get; set; }

	public bool ShowInRecycleBin { get; set; }

	public bool ShowInSearchPage { get; set; }

	public bool ShowInFtpPage { get; set; }

	public bool ShowInZipPage { get; set; }

	public bool IsChecked { get; set; }

	public bool IsEnabled { get; set; } = true;

	public string ID { get; set; } = string.Empty;

	public string AccessKey { get; set; } = string.Empty;
}
