using Avalonia.Input;
using Files.Assets.Lang;
using Files.Data.Commands.HotKey;

namespace Files.Actions.Navigation.Open;

internal sealed class OpenDesktopLocationAction : BaseOpenKnownLocationAction
{
	public override string Label => Resources.Desktop;

	public override HotKey HotKey => new HotKey(Key.D, KeyModifiers.Shift | KeyModifiers.Meta);

	protected override string? GetTargetPath()
	{
		return BaseOpenKnownLocationAction.GetUserKnownFolderPath("Desktop");
	}
}
