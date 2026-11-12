using System;

namespace Files.Extensions;

public static class ArrayExtensions
{
	public static T[] CloneArray<T>(this T[] array)
	{
		T[] array2 = new T[array.Length];
		Array.Copy(array, 0, array2, 0, array.Length);
		return array2;
	}
}
