using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Avalonia.Input;

namespace Files.Data.Commands.HotKey;

[DebuggerDisplay("{LocalizedLabel}")]
public readonly struct HotKey : IEquatable<HotKey>
{
	public static FrozenDictionary<KeyModifiers, string> LocalizedModifiers { get; } = new Dictionary<KeyModifiers, string>
	{
		[KeyModifiers.Alt] = GetLocalizedKey("Option"),
		[KeyModifiers.Control] = GetLocalizedKey("Control"),
		[KeyModifiers.Shift] = GetLocalizedKey("Shift"),
		[KeyModifiers.Meta] = GetLocalizedKey("Command")
	}.ToFrozenDictionary();

	public static FrozenDictionary<Key, string> LocalizedKeys { get; } = new Dictionary<Key, string>
	{
		[Key.Return] = GetLocalizedKey("Enter"),
		[Key.Space] = GetLocalizedKey("Space"),
		[Key.Escape] = GetLocalizedKey("Escape"),
		[Key.Back] = GetLocalizedKey("Back"),
		[Key.Tab] = GetLocalizedKey("Tab"),
		[Key.Insert] = GetLocalizedKey("Insert"),
		[Key.Delete] = GetLocalizedKey("Delete"),
		[Key.Left] = GetLocalizedKey("Left"),
		[Key.Right] = GetLocalizedKey("Right"),
		[Key.Down] = GetLocalizedKey("Down"),
		[Key.Up] = GetLocalizedKey("Up"),
		[Key.Home] = GetLocalizedKey("Home"),
		[Key.End] = GetLocalizedKey("End"),
		[Key.PageDown] = GetLocalizedKey("PageDown"),
		[Key.PageUp] = GetLocalizedKey("PageUp"),
		[Key.F1] = "F1",
		[Key.F2] = "F2",
		[Key.F3] = "F3",
		[Key.F4] = "F4",
		[Key.F5] = "F5",
		[Key.F6] = "F6",
		[Key.F7] = "F7",
		[Key.F8] = "F8",
		[Key.F9] = "F9",
		[Key.F10] = "F10",
		[Key.F11] = "F11",
		[Key.F12] = "F12",
		[Key.F13] = "F13",
		[Key.F14] = "F14",
		[Key.F15] = "F15",
		[Key.F16] = "F16",
		[Key.F17] = "F17",
		[Key.F18] = "F18",
		[Key.F19] = "F19",
		[Key.F20] = "F20",
		[Key.D0] = "0",
		[Key.D1] = "1",
		[Key.D2] = "2",
		[Key.D3] = "3",
		[Key.D4] = "4",
		[Key.D5] = "5",
		[Key.D6] = "6",
		[Key.D7] = "7",
		[Key.D8] = "8",
		[Key.D9] = "9",
		[Key.A] = "A",
		[Key.B] = "B",
		[Key.C] = "C",
		[Key.D] = "D",
		[Key.E] = "E",
		[Key.F] = "F",
		[Key.G] = "G",
		[Key.H] = "H",
		[Key.I] = "I",
		[Key.J] = "J",
		[Key.K] = "K",
		[Key.L] = "L",
		[Key.M] = "M",
		[Key.N] = "N",
		[Key.O] = "O",
		[Key.P] = "P",
		[Key.Q] = "Q",
		[Key.R] = "R",
		[Key.S] = "S",
		[Key.T] = "T",
		[Key.U] = "U",
		[Key.V] = "V",
		[Key.W] = "W",
		[Key.X] = "X",
		[Key.Y] = "Y",
		[Key.Z] = "Z",
		[Key.OemSemicolon] = ";",
		[Key.OemQuestion] = "/",
		[Key.OemTilde] = "`",
		[Key.OemOpenBrackets] = "[",
		[Key.OemPipe] = "]",
		[Key.OemCloseBrackets] = "'",
		[Key.OemQuotes] = "\\",
		[Key.Oem8] = "\\",
		[Key.OemPeriod] = ".",
		[Key.OemBackslash] = "\\",
		[Key.OemClear] = "Clear",
		[Key.Add] = "+",
		[Key.Subtract] = "-",
		[Key.Multiply] = "*",
		[Key.Divide] = "/",
		[Key.Decimal] = ".",
		[Key.NumPad0] = "NumPad 0",
		[Key.NumPad1] = "NumPad 1",
		[Key.NumPad2] = "NumPad 2",
		[Key.NumPad3] = "NumPad 3",
		[Key.NumPad4] = "NumPad 4",
		[Key.NumPad5] = "NumPad 5",
		[Key.NumPad6] = "NumPad 6",
		[Key.NumPad7] = "NumPad 7",
		[Key.NumPad8] = "NumPad 8",
		[Key.NumPad9] = "NumPad 9"
	}.ToFrozenDictionary();

	public static HotKey None { get; } = new HotKey(Key.None);

	public bool IsNone
	{
		get
		{
			if (Key == Key.None)
			{
				return Modifier == KeyModifiers.None;
			}
			return false;
		}
	}

	public bool IsVisible { get; init; }

	public Key Key { get; }

	public KeyModifiers Modifier { get; }

	public string RawLabel
	{
		get
		{
			Key key = Key;
			KeyModifiers modifier = Modifier;
			if (key == Key.None)
			{
				if (modifier == KeyModifiers.None)
				{
					return string.Empty;
				}
				return GetModifierCode(Modifier) ?? "";
			}
			if (modifier == KeyModifiers.None)
			{
				return $"{Key}";
			}
			return $"{GetModifierCode(Modifier)}+{Key}";
			static string GetModifierCode(KeyModifiers modifiers)
			{
				StringBuilder stringBuilder = new StringBuilder();
				if (modifiers.HasFlag(KeyModifiers.Alt))
				{
					StringBuilder stringBuilder2 = stringBuilder;
					StringBuilder stringBuilder3 = stringBuilder2;
					StringBuilder.AppendInterpolatedStringHandler handler = new StringBuilder.AppendInterpolatedStringHandler(1, 1, stringBuilder2);
					handler.AppendLiteral("+");
					handler.AppendFormatted(KeyModifiers.Alt);
					stringBuilder3.Append(ref handler);
				}
				if (modifiers.HasFlag(KeyModifiers.Control))
				{
					StringBuilder stringBuilder2 = stringBuilder;
					StringBuilder stringBuilder4 = stringBuilder2;
					StringBuilder.AppendInterpolatedStringHandler handler = new StringBuilder.AppendInterpolatedStringHandler(1, 1, stringBuilder2);
					handler.AppendLiteral("+");
					handler.AppendFormatted(KeyModifiers.Control);
					stringBuilder4.Append(ref handler);
				}
				if (modifiers.HasFlag(KeyModifiers.Shift))
				{
					StringBuilder stringBuilder2 = stringBuilder;
					StringBuilder stringBuilder5 = stringBuilder2;
					StringBuilder.AppendInterpolatedStringHandler handler = new StringBuilder.AppendInterpolatedStringHandler(1, 1, stringBuilder2);
					handler.AppendLiteral("+");
					handler.AppendFormatted(KeyModifiers.Shift);
					stringBuilder5.Append(ref handler);
				}
				if (modifiers.HasFlag(KeyModifiers.Meta))
				{
					StringBuilder stringBuilder2 = stringBuilder;
					StringBuilder stringBuilder6 = stringBuilder2;
					StringBuilder.AppendInterpolatedStringHandler handler = new StringBuilder.AppendInterpolatedStringHandler(1, 1, stringBuilder2);
					handler.AppendLiteral("+");
					handler.AppendFormatted(KeyModifiers.Meta);
					stringBuilder6.Append(ref handler);
				}
				stringBuilder.Remove(0, 1);
				return stringBuilder.ToString();
			}
		}
	}

	public string LocalizedLabel
	{
		get
		{
			Key key = Key;
			KeyModifiers modifier = Modifier;
			if (key == Key.None)
			{
				if (modifier == KeyModifiers.None)
				{
					return string.Empty;
				}
				return GetModifierCode(Modifier);
			}
			if (modifier == KeyModifiers.None)
			{
				return LocalizedKeys[Key];
			}
			return GetModifierCode(Modifier) + "+" + LocalizedKeys[Key];
			static string GetModifierCode(KeyModifiers keyModifiers)
			{
				StringBuilder stringBuilder = new StringBuilder();
				if (keyModifiers.HasFlag(KeyModifiers.Alt))
				{
					StringBuilder stringBuilder2 = stringBuilder;
					StringBuilder stringBuilder3 = stringBuilder2;
					StringBuilder.AppendInterpolatedStringHandler handler = new StringBuilder.AppendInterpolatedStringHandler(1, 1, stringBuilder2);
					handler.AppendLiteral("+");
					handler.AppendFormatted(LocalizedModifiers[KeyModifiers.Alt]);
					stringBuilder3.Append(ref handler);
				}
				if (keyModifiers.HasFlag(KeyModifiers.Control))
				{
					StringBuilder stringBuilder2 = stringBuilder;
					StringBuilder stringBuilder4 = stringBuilder2;
					StringBuilder.AppendInterpolatedStringHandler handler = new StringBuilder.AppendInterpolatedStringHandler(1, 1, stringBuilder2);
					handler.AppendLiteral("+");
					handler.AppendFormatted(LocalizedModifiers[KeyModifiers.Control]);
					stringBuilder4.Append(ref handler);
				}
				if (keyModifiers.HasFlag(KeyModifiers.Shift))
				{
					StringBuilder stringBuilder2 = stringBuilder;
					StringBuilder stringBuilder5 = stringBuilder2;
					StringBuilder.AppendInterpolatedStringHandler handler = new StringBuilder.AppendInterpolatedStringHandler(1, 1, stringBuilder2);
					handler.AppendLiteral("+");
					handler.AppendFormatted(LocalizedModifiers[KeyModifiers.Shift]);
					stringBuilder5.Append(ref handler);
				}
				if (keyModifiers.HasFlag(KeyModifiers.Meta))
				{
					StringBuilder stringBuilder2 = stringBuilder;
					StringBuilder stringBuilder6 = stringBuilder2;
					StringBuilder.AppendInterpolatedStringHandler handler = new StringBuilder.AppendInterpolatedStringHandler(1, 1, stringBuilder2);
					handler.AppendLiteral("+");
					handler.AppendFormatted(LocalizedModifiers[KeyModifiers.Meta]);
					stringBuilder6.Append(ref handler);
				}
				stringBuilder.Remove(0, 1);
				return stringBuilder.ToString();
			}
		}
	}

	public HotKey(Key key, KeyModifiers modifier = KeyModifiers.None, bool isVisible = true)
	{
		IsVisible = isVisible;
		Key = key;
		Modifier = modifier;
	}

	public static HotKey Parse(string code, bool localized = true)
	{
		Key key = Key.None;
		KeyModifiers keyModifiers = KeyModifiers.None;
		bool isVisible = true;
		char c = '+';
		code = code.Trim();
		List<string> list = code.Split(new string(c, 2)).ToList();
		if (list.Count == 2)
		{
			string[] array = list.First().Split(c);
			int num = 1 + array.Length;
			List<string> list2 = new List<string>(num);
			CollectionsMarshal.SetCount(list2, num);
			Span<string> span = CollectionsMarshal.AsSpan(list2);
			int num2 = 0;
			ReadOnlySpan<string> readOnlySpan = new ReadOnlySpan<string>(array);
			readOnlySpan.CopyTo(span.Slice(num2, readOnlySpan.Length));
			num2 += readOnlySpan.Length;
			span[num2] = c + list.Last();
			list = list2;
		}
		else
		{
			list = code.Split(c).ToList();
			if (list.Count == 2 && (string.IsNullOrEmpty(list.First()) || string.IsNullOrEmpty(list.Last())))
			{
				int num2 = 1;
				List<string> list3 = new List<string>(num2);
				CollectionsMarshal.SetCount(list3, num2);
				CollectionsMarshal.AsSpan(list3)[0] = code;
				list = list3;
			}
			else if (list.Count > 0 && string.IsNullOrEmpty(list.Last()))
			{
				list.RemoveAt(list.Count - 1);
				List<string> list4 = list;
				list4[list4.Count - 1] += c;
			}
		}
		list = list.Select((string text) => text.Trim()).ToList();
		foreach (string part in list)
		{
			if (localized)
			{
				key |= LocalizedKeys.FirstOrDefault((KeyValuePair<Key, string> x) => x.Value == part).Key;
				keyModifiers |= LocalizedModifiers.FirstOrDefault((KeyValuePair<KeyModifiers, string> x) => x.Value == part).Key;
				continue;
			}
			if (Enum.TryParse<Key>(part, ignoreCase: true, out var result))
			{
				key = result;
			}
			if (Enum.TryParse<KeyModifiers>(part, ignoreCase: true, out var result2))
			{
				keyModifiers |= result2;
			}
		}
		return new HotKey(key, keyModifiers, isVisible);
	}

	public HotKeyCollection AsCollection()
	{
		return new HotKeyCollection(this);
	}

	public static implicit operator string(HotKey hotKey)
	{
		return hotKey.LocalizedLabel;
	}

	public static bool operator ==(HotKey a, HotKey b)
	{
		return a.Equals(b);
	}

	public static bool operator !=(HotKey a, HotKey b)
	{
		return !a.Equals(b);
	}

	public override string ToString()
	{
		return LocalizedLabel;
	}

	public override int GetHashCode()
	{
		return (Key, Modifier, IsVisible).GetHashCode();
	}

	public override bool Equals(object? other)
	{
		if (other is HotKey other2)
		{
			return Equals(other2);
		}
		return false;
	}

	public bool Equals(HotKey other)
	{
		return (other.Key, other.Modifier, other.IsVisible).Equals((Key, Modifier, IsVisible));
	}

	private static string GetLocalizedKey(string key)
	{
		return "Key/" + key;
	}
}
