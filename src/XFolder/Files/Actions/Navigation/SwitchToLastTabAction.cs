using Avalonia.Input;
using Files.Data.Commands.HotKey;

namespace Files.Actions.Navigation;

internal sealed class SwitchToLastTabAction : SwitchToNumberedTabAction
{
	public SwitchToLastTabAction()
		: base(-1, "SwitchToLastTab", new HotKey(Key.D9, KeyModifiers.Meta))
	{
	}
}
