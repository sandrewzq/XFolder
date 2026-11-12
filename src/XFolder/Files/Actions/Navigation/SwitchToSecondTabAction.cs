using Avalonia.Input;
using Files.Data.Commands.HotKey;

namespace Files.Actions.Navigation;

internal sealed class SwitchToSecondTabAction : SwitchToNumberedTabAction
{
	public SwitchToSecondTabAction()
		: base(1, "SwitchToSecondTab", new HotKey(Key.D2, KeyModifiers.Meta))
	{
	}
}
