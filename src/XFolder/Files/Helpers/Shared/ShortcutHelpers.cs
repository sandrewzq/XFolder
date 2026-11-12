using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Input;
using Files.Assets.Lang;
using Files.Data.Commands.HotKey;

namespace Files.Helpers.Shared;

internal static class ShortcutHelpers
{
	public static HotKey GetPrimaryHotKey(IEnumerable<HotKey> hotKeys)
	{
		return hotKeys.FirstOrDefault((HotKey x) => x.IsVisible && !x.IsNone);
	}

	public static KeyGesture? GetPrimaryKeyGesture(IEnumerable<HotKey> hotKeys)
	{
		HotKey primaryHotKey = GetPrimaryHotKey(hotKeys);
		if (!primaryHotKey.IsNone)
		{
			return new KeyGesture(primaryHotKey.Key, primaryHotKey.Modifier);
		}
		return null;
	}

	public static string? GetPrimaryMacDisplayText(IEnumerable<HotKey> hotKeys)
	{
		HotKey primaryHotKey = GetPrimaryHotKey(hotKeys);
		if (!primaryHotKey.IsNone)
		{
			return FormatMacDisplayText(primaryHotKey);
		}
		return null;
	}

	public static string FormatMacDisplayText(HotKey hotKey)
	{
		List<string> list = new List<string>();
		if (hotKey.Modifier.HasFlag(KeyModifiers.Control))
		{
			list.Add("⌃");
		}
		if (hotKey.Modifier.HasFlag(KeyModifiers.Shift))
		{
			list.Add("⇧");
		}
		if (hotKey.Modifier.HasFlag(KeyModifiers.Alt))
		{
			list.Add("⌥");
		}
		if (hotKey.Modifier.HasFlag(KeyModifiers.Meta))
		{
			list.Add("⌘");
		}
		list.Add(GetFriendlyKeyLabel(hotKey.Key));
		return string.Join(" + ", list);
	}

	private static string GetFriendlyKeyLabel(Key key)
	{
		switch (key)
		{
		case Key.D0:
			return "0";
		case Key.D1:
			return "1";
		case Key.D2:
			return "2";
		case Key.D3:
			return "3";
		case Key.D4:
			return "4";
		case Key.D5:
			return "5";
		case Key.D6:
			return "6";
		case Key.D7:
			return "7";
		case Key.D8:
			return "8";
		case Key.D9:
			return "9";
		case Key.NumPad0:
			return Resources.NumPadTypeName + " 0";
		case Key.NumPad1:
			return Resources.NumPadTypeName + " 1";
		case Key.NumPad2:
			return Resources.NumPadTypeName + " 2";
		case Key.NumPad3:
			return Resources.NumPadTypeName + " 3";
		case Key.NumPad4:
			return Resources.NumPadTypeName + " 4";
		case Key.NumPad5:
			return Resources.NumPadTypeName + " 5";
		case Key.NumPad6:
			return Resources.NumPadTypeName + " 6";
		case Key.NumPad7:
			return Resources.NumPadTypeName + " 7";
		case Key.NumPad8:
			return Resources.NumPadTypeName + " 8";
		case Key.NumPad9:
			return Resources.NumPadTypeName + " 9";
		case Key.Left:
			return "←";
		case Key.Right:
			return "→";
		case Key.Up:
			return "↑";
		case Key.Down:
			return "↓";
		case Key.OemTilde:
			return "~";
		default:
		{
			if (HotKey.LocalizedKeys.TryGetValue(key, out string value) && !value.StartsWith("Key/", StringComparison.Ordinal))
			{
				return value;
			}
			return key.ToString();
		}
		}
	}
}
