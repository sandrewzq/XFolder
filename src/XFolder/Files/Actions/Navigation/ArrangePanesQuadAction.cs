using Files.Data.Enums;

namespace Files.Actions.Navigation;

internal sealed class ArrangePanesQuadAction : BaseArrangePanesAction
{
	protected override ShellPaneArrangement Arrangement => ShellPaneArrangement.Quad;
}
