using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Input;
using Avalonia.Input.Platform;
using Files.Assets.Lang;
using Files.Data.Commands.HotKey;
using Files.Data.Items;
using Files.Extensions;

namespace Files.Actions;

internal sealed class CopyItemPathAction : BaseTransferItemAction, IAction
{
	public string Label => Resources.CopyItemPath;

	public string Description => Resources.CopyItemPathDescription;

	public HotKey HotKey => new HotKey(Key.C, KeyModifiers.Shift | KeyModifiers.Meta);

	public async Task ExecuteAsync(object? parameter = null)
	{
		List<string> list = (from item in ContentPageContext.SelectedItems
			select item.ItemPath into path
			where !string.IsNullOrWhiteSpace(path)
			select path).ToList();
		if (list.Count != 0)
		{
			IClipboard clipboard = Application.Current.GetTopLevel()?.Clipboard;
			if (clipboard != null)
			{
				await clipboard.SetTextAsync(string.Join(Environment.NewLine, list));
			}
		}
	}
}
