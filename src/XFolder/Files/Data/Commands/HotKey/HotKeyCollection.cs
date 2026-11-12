using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;

namespace Files.Data.Commands.HotKey;

[DebuggerDisplay("{LocalizedLabel}")]
public readonly struct HotKeyCollection : IEnumerable<HotKey>, IEnumerable, IEquatable<HotKeyCollection>
{
	private static readonly char[] parseSeparators = new char[2] { ' ', '\t' };

	private readonly ImmutableArray<HotKey> hotKeys;

	public static HotKeyCollection Empty { get; } = new HotKeyCollection(ImmutableArray<HotKey>.Empty);

	public HotKey this[int index]
	{
		get
		{
			if (index < 0 || index >= hotKeys.Length)
			{
				return HotKey.None;
			}
			return hotKeys[index];
		}
	}

	public bool IsEmpty => hotKeys.IsDefaultOrEmpty;

	public int Length => hotKeys.Length;

	public string RawLabel => string.Join(' ', hotKeys.Select((HotKey hotKey) => hotKey.RawLabel));

	public string LocalizedLabel => string.Join(",  ", from hotKey in hotKeys
		where hotKey.IsVisible
		select hotKey.LocalizedLabel);

	public HotKeyCollection()
	{
		hotKeys = ImmutableArray<HotKey>.Empty;
	}

	public HotKeyCollection(params HotKey[] hotKeys)
	{
		this.hotKeys = Standardize(hotKeys);
	}

	public HotKeyCollection(IEnumerable<HotKey> hotKeys)
	{
		this.hotKeys = Standardize(hotKeys);
	}

	public static bool operator ==(HotKeyCollection hotKeysA, HotKeyCollection hotKeysB)
	{
		return hotKeysA.Equals(hotKeysB);
	}

	public static bool operator !=(HotKeyCollection hotKeysA, HotKeyCollection hotKeysB)
	{
		return !hotKeysA.Equals(hotKeysB);
	}

	public static HotKeyCollection Parse(string code, bool localized = true)
	{
		return new HotKeyCollection(from part in code.Split(parseSeparators)
			select part.Trim() into x
			select HotKey.Parse(x, localized));
	}

	public bool Contains(HotKey hotKey)
	{
		return hotKeys.Contains(hotKey);
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}

	public IEnumerator<HotKey> GetEnumerator()
	{
		return ((IEnumerable<HotKey>)hotKeys).GetEnumerator();
	}

	public override string ToString()
	{
		return LocalizedLabel;
	}

	public override int GetHashCode()
	{
		return hotKeys.GetHashCode();
	}

	public override bool Equals(object? other)
	{
		if (other is HotKeyCollection other2)
		{
			return Equals(other2);
		}
		return false;
	}

	public bool Equals(HotKeyCollection other)
	{
		return hotKeys.SequenceEqual(other.hotKeys);
	}

	private static ImmutableArray<HotKey> Standardize(IEnumerable<HotKey> hotKeys)
	{
		return (from @group in (from hotKey in hotKeys.Distinct()
				where !hotKey.IsNone
				select hotKey).GroupBy((HotKey hotKey) =>
			{
				return hotKey with { IsVisible = true };
			})
			select @group.OrderBy((HotKey hotKey) => hotKey.IsVisible).Last()).ToImmutableArray();
	}
}
