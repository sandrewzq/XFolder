using Files.Data.Enums;

namespace Files.Actions.Navigation;

internal sealed class ArrangePanesVerticallyAction : BaseArrangePanesAction
{
	protected override ShellPaneArrangement Arrangement => ShellPaneArrangement.Vertical;
}
