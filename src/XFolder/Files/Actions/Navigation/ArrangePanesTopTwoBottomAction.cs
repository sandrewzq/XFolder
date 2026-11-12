using Files.Data.Enums;

namespace Files.Actions.Navigation;

internal sealed class ArrangePanesTopTwoBottomAction : BaseArrangePanesAction
{
	protected override ShellPaneArrangement Arrangement => ShellPaneArrangement.TopTwoBottom;
}
