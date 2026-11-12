using Files.Data.Enums;

namespace Files.Actions.Navigation;

internal sealed class ArrangePanesHorizontallyAction : BaseArrangePanesAction
{
	protected override ShellPaneArrangement Arrangement => ShellPaneArrangement.Horizontal;
}
