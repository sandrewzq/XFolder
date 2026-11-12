using Files.Data.Enums;

namespace Files.Actions.Navigation;

internal sealed class ArrangePanesLeftTwoRightAction : BaseArrangePanesAction
{
	protected override ShellPaneArrangement Arrangement => ShellPaneArrangement.LeftTwoRight;
}
