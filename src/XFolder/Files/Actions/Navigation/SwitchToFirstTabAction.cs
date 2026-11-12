using Avalonia.Input;
using Files.Data.Commands.HotKey;

namespace Files.Actions.Navigation;

internal sealed class SwitchToFirstTabAction : SwitchToNumberedTabAction
{
	public SwitchToFirstTabAction()
		: base(0, "SwitchToFirstTab", new HotKey(Key.D1, KeyModifiers.Meta))
	{
	}
}
