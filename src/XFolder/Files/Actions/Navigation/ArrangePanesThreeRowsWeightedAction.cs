using Files.Data.Enums;

namespace Files.Actions.Navigation;

internal sealed class ArrangePanesThreeRowsWeightedAction : BaseArrangePanesAction
{
	protected override ShellPaneArrangement Arrangement => ShellPaneArrangement.ThreeRowsWeighted;
}
