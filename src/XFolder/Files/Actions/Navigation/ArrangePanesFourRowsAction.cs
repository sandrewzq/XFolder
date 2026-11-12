using Files.Data.Enums;

namespace Files.Actions.Navigation;

internal sealed class ArrangePanesFourRowsAction : BaseArrangePanesAction
{
	protected override ShellPaneArrangement Arrangement => ShellPaneArrangement.FourRows;
}
