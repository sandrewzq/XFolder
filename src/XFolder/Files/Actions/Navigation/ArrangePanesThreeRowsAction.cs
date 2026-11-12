using Files.Data.Enums;

namespace Files.Actions.Navigation;

internal sealed class ArrangePanesThreeRowsAction : BaseArrangePanesAction
{
	protected override ShellPaneArrangement Arrangement => ShellPaneArrangement.ThreeRows;
}
