using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Jeffijoe.MessageFormat;
using Microsoft.Extensions.Logging;

namespace Files.Extensions;

public static class MessageFormatExtensions
{
	private static readonly CultureInfo _locale = CultureInfo.CurrentCulture;

	private static readonly CustomValueFormatters _customFormatter = new CustomValueFormatters
	{
		Number = delegate(CultureInfo _, object? value, string? style, out string? formatted)
		{
			if (style != null && style == string.Empty)
			{
				formatted = $"{value:#,##0}";
				return true;
			}
			formatted = null;
			return false;
		}
	};

	private static readonly MessageFormatter _formatter = new MessageFormatter(useCache: false, _locale, _customFormatter);

	public static IReadOnlyDictionary<string, object?> ToFormatPairs(this string key, object value)
	{
		return new Dictionary<string, object> { [key] = value };
	}

	public static IReadOnlyDictionary<string, object?> ToFormatPairs(this int key, object value)
	{
		return new Dictionary<string, object> { [key.ToString()] = value };
	}

	public static string GetLocalizedFormatResource(this string resourceKey, IReadOnlyDictionary<string, object?> pairs)
	{
		string text = resourceKey;
		if (text == null)
		{
			return string.Empty;
		}
		try
		{
			text = _formatter.FormatMessage(text, pairs, _locale);
		}
		catch
		{
			text = string.Empty;
			App.Logger.LogWarning("Formatter could not get a valid result value for: '" + resourceKey + "'");
		}
		return text;
	}

	public static string GetLocalizedFormatResource(this string resourceKey, params IReadOnlyDictionary<string, object?>[] pairs)
	{
		Dictionary<string, object> pairs2 = pairs.SelectMany((IReadOnlyDictionary<string, object> dict) => dict).ToDictionary((KeyValuePair<string, object> pair) => pair.Key, (KeyValuePair<string, object> pair) => pair.Value);
		return resourceKey.GetLocalizedFormatResource(pairs2);
	}

	public static string GetLocalizedFormatResource(this string resourceKey, params object[] values)
	{
		Dictionary<string, object> pairs = values.Select((object value, int index) => new KeyValuePair<string, object>(index.ToString(), value)).ToDictionary((KeyValuePair<string, object> pair) => pair.Key, (KeyValuePair<string, object> pair) => pair.Value);
		return resourceKey.GetLocalizedFormatResource(pairs);
	}

	public static string GetLocalizedFormatResource(this string resourceKey)
	{
		return resourceKey.GetLocalizedFormatResource(new Dictionary<string, object>());
	}
}
