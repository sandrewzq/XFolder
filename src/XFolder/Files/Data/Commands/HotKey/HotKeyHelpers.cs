using Avalonia.Input;
using Files.Services.NativePlugin;

namespace Files.Data.Commands.HotKey;

internal static class HotKeyHelpers
{
	public static KeyModifiers GetCurrentKeyModifiers()
	{
		KeyModifiers keyModifiers = KeyModifiers.None;
		if (MacPluginInterop.IsShiftKeyPressed())
		{
			keyModifiers |= KeyModifiers.Shift;
		}
		if (MacPluginInterop.IsControlKeyPressed())
		{
			keyModifiers |= KeyModifiers.Control;
		}
		if (MacPluginInterop.IsOptionKeyPressed())
		{
			keyModifiers |= KeyModifiers.Alt;
		}
		if (MacPluginInterop.IsCommandKeyPressed())
		{
			keyModifiers |= KeyModifiers.Meta;
		}
		return keyModifiers;
	}
}
