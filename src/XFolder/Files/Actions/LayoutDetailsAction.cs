using Avalonia.Input;
using Files.Assets.Lang;
using Files.Data.Commands.HotKey;
using Files.Data.Contexts;

namespace Files.Actions;

internal sealed class LayoutDetailsAction : ToggleLayoutAction
{
	protected override LayoutTypes LayoutType => LayoutTypes.Details;

	public override string Label => Resources.Details;

	public override string Description => Resources.LayoutDetailsDescription;

	public string? Icon => "text_column_one_regular";

	public override HotKey HotKey => new HotKey(Key.D1, KeyModifiers.Control);

	public override HotKey SecondHotKey => new HotKey(Key.NumPad1, KeyModifiers.Control);
}
