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

internal sealed class CutItemAction : BaseTransferItemAction, IAction
{
	public string Label => Resources.Cut;

	public string Description => Resources.CutItemDescription.GetLocalizedFormatResource(ContentPageContext.SelectedItems.Count);

	public string? Icon => "cut_regular";

	public HotKey HotKey => new HotKey(Key.X, KeyModifiers.Meta);

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
		MacPluginInterop.SetClipboardFileList(list, isCut: true);
		App.AppModel.NotifyPasteAvailabilityChanged();
		ContentPageContext.ShellView?.InstanceViewModel?.SelectedTab?.LayoutViewModel?.RefreshItemsOpacityByClipboardCutStatus();
		return Task.CompletedTask;
	}
}
