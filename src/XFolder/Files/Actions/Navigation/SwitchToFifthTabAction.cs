using Avalonia.Input;
using Files.Data.Commands.HotKey;

namespace Files.Actions.Navigation;

internal sealed class SwitchToFifthTabAction : SwitchToNumberedTabAction
{
	public SwitchToFifthTabAction()
		: base(4, "SwitchToFifthTab", new HotKey(Key.D5, KeyModifiers.Meta))
	{
	}
}
