using System.Threading.Tasks;
using Avalonia.Input;
using Files.Assets.Lang;
using Files.Data.Commands.HotKey;
using Files.Extensions;

namespace Files.Actions;

internal sealed class DeleteItemAction : BaseDeleteAction, IAction
{
	public string Label => Resources.Delete;

	public string Description => Resources.DeleteItemDescription.GetLocalizedFormatResource(context.SelectedItems.Count);

	public string? Icon => "delete_regular";

	public HotKey HotKey => new HotKey(Key.Delete);

	public HotKey SecondHotKey => new HotKey(Key.D, KeyModifiers.Meta);

	public Task ExecuteAsync(object? parameter = null)
	{
		return DeleteItemsAsync(permanently: false);
	}
}
