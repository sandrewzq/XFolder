using Avalonia.Input;
using Files.Assets.Lang;
using Files.Data.Commands.HotKey;

namespace Files.Actions.Navigation.Open;

internal sealed class OpenDownloadsLocationAction : BaseOpenKnownLocationAction
{
	public override string Label => Resources.Downloads;

	public override HotKey HotKey => new HotKey(Key.L, KeyModifiers.Alt | KeyModifiers.Meta);

	protected override string? GetTargetPath()
	{
		return BaseOpenKnownLocationAction.GetUserKnownFolderPath("Downloads");
	}
}
