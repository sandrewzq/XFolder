using Files.Data.Enums;

namespace Files.Actions.Navigation;

internal sealed class ArrangePanesTwoLeftRightAction : BaseArrangePanesAction
{
	protected override ShellPaneArrangement Arrangement => ShellPaneArrangement.TwoLeftRight;
}
