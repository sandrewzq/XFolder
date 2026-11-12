using Avalonia.Input;
using Files.Data.Commands.HotKey;

namespace Files.Actions.Navigation;

internal sealed class SwitchToThirdTabAction : SwitchToNumberedTabAction
{
	public SwitchToThirdTabAction()
		: base(2, "SwitchToThirdTab", new HotKey(Key.D3, KeyModifiers.Meta))
	{
	}
}
