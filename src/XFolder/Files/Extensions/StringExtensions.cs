using System;

namespace Files.Extensions;

public static class StringExtensions
{
	public static string Left(this string value, int length)
	{
		if (value == null)
		{
			throw new ArgumentNullException("value");
		}
		if (length < 0)
		{
			throw new ArgumentOutOfRangeException("length", length, "Length is less than zero");
		}
		if (length <= value.Length)
		{
			return value.Substring(0, length);
		}
		return value;
	}

	public static string Right(this string value, int length)
	{
		if (value == null)
		{
			throw new ArgumentNullException("value");
		}
		if (length < 0)
		{
			throw new ArgumentOutOfRangeException("length", length, "Length is less than zero");
		}
		if (length <= value.Length)
		{
			return value.Substring(value.Length - length);
		}
		return value;
	}

	public static string WithEnding(this string str, string ending)
	{
		if (str == null)
		{
			return ending;
		}
		for (int i = 0; i <= ending.Length; i++)
		{
			string text = str + ending.Right(i);
			if (text.EndsWith(ending, StringComparison.Ordinal))
			{
				return text;
			}
		}
		return str;
	}
}
