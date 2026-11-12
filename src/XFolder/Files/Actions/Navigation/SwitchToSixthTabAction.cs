using Avalonia.Input;
using Files.Data.Commands.HotKey;

namespace Files.Actions.Navigation;

internal sealed class SwitchToSixthTabAction : SwitchToNumberedTabAction
{
	public SwitchToSixthTabAction()
		: base(5, "SwitchToSixthTab", new HotKey(Key.D6, KeyModifiers.Meta))
	{
	}
}
