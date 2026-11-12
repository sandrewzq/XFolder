using Files.Data.Enums;

namespace Files.Actions.Navigation;

internal sealed class ArrangePanesThreeColumnsAction : BaseArrangePanesAction
{
	protected override ShellPaneArrangement Arrangement => ShellPaneArrangement.ThreeColumns;
}
