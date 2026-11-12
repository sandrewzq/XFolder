using Avalonia.Input;
using Files.Data.Commands.HotKey;

namespace Files.Actions.Navigation;

internal sealed class SwitchToFourthTabAction : SwitchToNumberedTabAction
{
	public SwitchToFourthTabAction()
		: base(3, "SwitchToFourthTab", new HotKey(Key.D4, KeyModifiers.Meta))
	{
	}
}
