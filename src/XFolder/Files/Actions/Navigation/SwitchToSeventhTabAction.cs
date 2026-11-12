using Avalonia.Input;
using Files.Data.Commands.HotKey;

namespace Files.Actions.Navigation;

internal sealed class SwitchToSeventhTabAction : SwitchToNumberedTabAction
{
	public SwitchToSeventhTabAction()
		: base(6, "SwitchToSeventhTab", new HotKey(Key.D7, KeyModifiers.Meta))
	{
	}
}
