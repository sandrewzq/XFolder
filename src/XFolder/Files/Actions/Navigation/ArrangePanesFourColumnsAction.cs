using Files.Data.Enums;

namespace Files.Actions.Navigation;

internal sealed class ArrangePanesFourColumnsAction : BaseArrangePanesAction
{
	protected override ShellPaneArrangement Arrangement => ShellPaneArrangement.FourColumns;
}
