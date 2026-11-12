using System;
using System.Diagnostics.CodeAnalysis;

namespace Files.Extensions;

public static class GenericExtensions
{
	[return: NotNullIfNotNull("defaultValue")]
	public static TOut? TryCast<TOut>(this object? value, Func<TOut>? defaultValue = null)
	{
		if (value is TOut)
		{
			return (TOut)value;
		}
		if (defaultValue == null)
		{
			return default;
		}
		return defaultValue();
	}
}
