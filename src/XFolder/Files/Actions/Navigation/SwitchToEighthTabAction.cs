using Avalonia.Input;
using Files.Data.Commands.HotKey;

namespace Files.Actions.Navigation;

internal sealed class SwitchToEighthTabAction : SwitchToNumberedTabAction
{
	public SwitchToEighthTabAction()
		: base(7, "SwitchToEighthTab", new HotKey(Key.D8, KeyModifiers.Meta))
	{
	}
}
