using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Input;
using Files.Assets.Lang;
using Files.Data.Commands.HotKey;
using Files.Data.Items;
using Files.Extensions;
using Files.Services.NativePlugin;

namespace Files.Actions;

internal sealed class CopyItemAction : BaseTransferItemAction, IAction
{
	public string Label => Resources.Copy;

	public string Description => Resources.CopyItemDescription.GetLocalizedFormatResource(ContentPageContext.SelectedItems.Count);

	public string? Icon => "copy_regular";

	public HotKey HotKey => new HotKey(Key.C, KeyModifiers.Meta);

	public Task ExecuteAsync(object? parameter = null)
	{
		IReadOnlyList<ListedItem> selectedItems = ContentPageContext.SelectedItems;
		if (selectedItems == null || selectedItems.Count == 0)
		{
			return Task.CompletedTask;
		}
		List<string> list = (from i in selectedItems
			select i.ItemPath into p
			where !string.IsNullOrWhiteSpace(p)
			select p).ToList();
		if (list.Count == 0)
		{
			return Task.CompletedTask;
		}
		MacPluginInterop.SetClipboardFileList(list);
		App.AppModel.NotifyPasteAvailabilityChanged();
		ContentPageContext.ShellView?.InstanceViewModel?.SelectedTab?.LayoutViewModel?.RefreshItemsOpacityByClipboardCutStatus();
		return Task.CompletedTask;
	}
}
