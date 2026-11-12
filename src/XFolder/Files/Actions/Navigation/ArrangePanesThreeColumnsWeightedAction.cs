using Files.Data.Enums;

namespace Files.Actions.Navigation;

internal sealed class ArrangePanesThreeColumnsWeightedAction : BaseArrangePanesAction
{
	protected override ShellPaneArrangement Arrangement => ShellPaneArrangement.ThreeColumnsWeighted;
}
