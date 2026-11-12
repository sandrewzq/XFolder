using Avalonia.Input;
using Files.Assets.Lang;
using Files.Data.Commands.HotKey;

namespace Files.Actions.Navigation.Open;

internal sealed class NavigateHomeAction : BaseOpenKnownLocationAction
{
	public override string Label => Resources.Home;

	public override HotKey HotKey => new HotKey(Key.H, KeyModifiers.Shift | KeyModifiers.Meta);

	protected override string? GetTargetPath()
	{
		return BaseOpenKnownLocationAction.GetHomePath();
	}
}
