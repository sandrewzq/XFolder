using Files.Assets.Lang;
using Files.Data.Commands.HotKey;

namespace Files.Actions.Navigation.Open;

internal sealed class OpenMusicLocationAction : BaseOpenKnownLocationAction
{
	public override string Label => Resources.Music;

	public override HotKey HotKey => HotKey.None;

	protected override string? GetTargetPath()
	{
		return BaseOpenKnownLocationAction.GetUserKnownFolderPath("Music");
	}
}
