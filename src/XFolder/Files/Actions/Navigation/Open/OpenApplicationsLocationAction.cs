using Avalonia.Input;
using Files.Assets.Lang;
using Files.Data.Commands.HotKey;

namespace Files.Actions.Navigation.Open;

internal sealed class OpenApplicationsLocationAction : BaseOpenKnownLocationAction
{
	public override string Label => Resources.Applications;

	public override HotKey HotKey => new HotKey(Key.A, KeyModifiers.Shift | KeyModifiers.Meta);

	protected override string? GetTargetPath()
	{
		return "/Applications";
	}
}
