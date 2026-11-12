using Files.Data.Enums;

namespace Files.Actions.Navigation;

internal sealed class ArrangePanesResetAction : BaseArrangePanesAction
{
	protected override ShellPaneArrangement Arrangement => ShellPaneArrangement.Single;
}
