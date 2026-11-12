using System;
using System.Collections.Generic;
using System.Linq;
using Files.Data.Items;
using Files.Helpers.Shared;
using Files.Services.NativePlugin;
using Files.ViewModels.Layouts;

namespace Files.Helpers.UI;

public static class ClipboardCutOpacityHelper
{
	public static void RefreshByLayout(BaseLayoutViewModel? layoutViewModel)
	{
		if (layoutViewModel?.Items != null && layoutViewModel.Items.Count != 0)
		{
			RefreshItems(layoutViewModel.Items);
		}
	}

	public static void RefreshItems(IEnumerable<ListedItem>? items)
	{
		if (items == null)
		{
			return;
		}
		HashSet<string> cutPaths = GetCutPaths();
		foreach (ListedItem item in items)
		{
			if (cutPaths != null && cutPaths.Contains(PathHelpers.NormalizePath(item.ItemPath)))
			{
				item.Opacity = 0.45;
			}
			else
			{
				item.Opacity = (item.IsHiddenItem ? 0.45 : 1.0);
			}
		}
	}

	private static HashSet<string>? GetCutPaths()
	{
		if (!MacPluginInterop.ClipboardIsCutMode())
		{
			return null;
		}
		List<string> clipboardFileList = MacPluginInterop.GetClipboardFileList();
		if (clipboardFileList == null || clipboardFileList.Count == 0)
		{
			return null;
		}
		return clipboardFileList.Where((string path) => !string.IsNullOrWhiteSpace(path)).Select(PathHelpers.NormalizePath).ToHashSet(StringComparer.OrdinalIgnoreCase);
	}
}
