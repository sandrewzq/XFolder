using Files.Data.Enums;

namespace Files.Actions.Navigation;

internal sealed class ArrangePanesTwoTopBottomAction : BaseArrangePanesAction
{
	protected override ShellPaneArrangement Arrangement => ShellPaneArrangement.TwoTopBottom;
}
