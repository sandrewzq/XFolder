using Avalonia.Input;
using Files.Assets.Lang;
using Files.Data.Commands.HotKey;

namespace Files.Actions.Navigation.Open;

internal sealed class OpenDocumentsLocationAction : BaseOpenKnownLocationAction
{
	public override string Label => Resources.Documents;

	public override HotKey HotKey => new HotKey(Key.O, KeyModifiers.Shift | KeyModifiers.Meta);

	protected override string? GetTargetPath()
	{
		return BaseOpenKnownLocationAction.GetUserKnownFolderPath("Documents");
	}
}
