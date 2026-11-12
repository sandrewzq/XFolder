using Avalonia.Input;
using Files.Assets.Lang;
using Files.Data.Commands.HotKey;
using Files.Data.Contexts;

namespace Files.Actions;

internal sealed class LayoutGridAction : ToggleLayoutAction
{
	protected override LayoutTypes LayoutType => LayoutTypes.Grid;

	public override string Label => Resources.Grid;

	public override string Description => Resources.LayoutGridDescription;

	public string? Icon => "grid_regular";

	public override HotKey HotKey => new HotKey(Key.D2, KeyModifiers.Control);

	public override HotKey SecondHotKey => new HotKey(Key.NumPad2, KeyModifiers.Control);
}
