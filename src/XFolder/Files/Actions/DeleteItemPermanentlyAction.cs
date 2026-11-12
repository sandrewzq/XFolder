using System.Threading.Tasks;
using Avalonia.Input;
using Files.Assets.Lang;
using Files.Data.Commands.HotKey;
using Files.Extensions;

namespace Files.Actions;

internal sealed class DeleteItemPermanentlyAction : BaseDeleteAction, IAction
{
	public string Label => Resources.DeletePermanently;

	public string Description => Resources.DeleteItemPermanentlyDescription.GetLocalizedFormatResource(context.SelectedItems.Count);

	public string? Icon => "delete_forever_regular";

	public HotKey HotKey => new HotKey(Key.Delete, KeyModifiers.Shift);

	public Task ExecuteAsync(object? parameter = null)
	{
		return DeleteItemsAsync(permanently: true);
	}
}
