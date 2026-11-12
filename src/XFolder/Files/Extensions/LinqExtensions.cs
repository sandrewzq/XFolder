using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Files.Extensions;

public static class LinqExtensions
{
	public static bool IsEmpty<T>(this IEnumerable<T> enumerable)
	{
		if (enumerable != null)
		{
			return !enumerable.Any();
		}
		return true;
	}

	public static TOut? Get<TOut, TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TOut? defaultValue = default(TOut?)) where TKey : notnull
	{
		if (dictionary == null || key == null)
		{
			return defaultValue;
		}
		if (dictionary is ConcurrentDictionary<TKey, TValue> concurrentDictionary)
		{
			if (!concurrentDictionary.ContainsKey(key))
			{
				if (defaultValue is TValue)
				{
					TValue value = (TValue)((((object)defaultValue) is TValue) ? ((object)defaultValue) : null);
					concurrentDictionary.TryAdd(key, value);
				}
				return defaultValue;
			}
		}
		else
		{
			lock (dictionary)
			{
				if (!dictionary.ContainsKey(key))
				{
					if (defaultValue is TValue)
					{
						TValue value2 = (TValue)((((object)defaultValue) is TValue) ? ((object)defaultValue) : null);
						dictionary.Add(key, value2);
					}
					return defaultValue;
				}
			}
		}
		TValue val = dictionary[key];
		if (val is TOut)
		{
			return (TOut)((((object)val) is TOut) ? ((object)val) : null);
		}
		return defaultValue;
	}

	public static Task<TValue?> GetAsync<TKey, TValue>(this IDictionary<TKey, Task<TValue?>> dictionary, TKey key, Func<Task<TValue?>> defaultValueFunc) where TKey : notnull
	{
		if (dictionary == null || key == null)
		{
			return defaultValueFunc();
		}
		if (dictionary is ConcurrentDictionary<TKey, Task<TValue>> concurrentDictionary)
		{
			if (!concurrentDictionary.ContainsKey(key))
			{
				Task<TValue> task = defaultValueFunc();
				if (task != null)
				{
					Task<TValue> value = task;
					concurrentDictionary.TryAdd(key, value);
				}
				return task;
			}
		}
		else
		{
			lock (dictionary)
			{
				if (!dictionary.ContainsKey(key))
				{
					Task<TValue> task2 = defaultValueFunc();
					if (task2 != null)
					{
						Task<TValue> value2 = task2;
						dictionary.Add(key, value2);
					}
					return task2;
				}
			}
		}
		return dictionary[key];
	}

	public static async Task<IEnumerable<T>> WhereAsync<T>(this IEnumerable<T> source, Func<T, Task<bool>> predicate)
	{
		return (await Task.WhenAll(source.Select(async (T x) => (x: x, await predicate(x))))).Where(((T x, bool) x) => x.Item2).Select(((T x, bool) x) => x.x);
	}

	public static IEnumerable<TSource> ExceptBy<TSource, TKey>(this IEnumerable<TSource> source, IEnumerable<TSource> other, Func<TSource, TKey> keySelector)
	{
		HashSet<TKey> set = new HashSet<TKey>(other.Select(keySelector));
		foreach (TSource item in source)
		{
			if (set.Add(keySelector(item)))
			{
				yield return item;
			}
		}
	}

	public static IEnumerable<T> IntersectBy<T, TKey>(this IEnumerable<T> items, IEnumerable<T> others, Func<T, TKey> keySelector)
	{
		return items.Join(others.Select(keySelector), keySelector, (TKey id) => id, (T o, TKey id) => o);
	}

	public static void ForEach<T>(this IEnumerable<T> collection, Action<T> action)
	{
		foreach (T item in collection)
		{
			action(item);
		}
	}

	public static int AddSorted<T>(this IList<T> list, T item) where T : IComparable<T>
	{
		if (!list.Any() || list.Last().CompareTo(item) <= 0)
		{
			list.Add(item);
			return list.Count;
		}
		if (list[0].CompareTo(item) >= 0)
		{
			list.Insert(0, item);
			return 0;
		}
		int num = list.ToList().BinarySearch(item);
		if (num < 0)
		{
			num = ~num;
		}
		list.Insert(num, item);
		return num;
	}

	public static List<T> RemoveFrom<T>(this List<T> list, int index)
	{
		if (!list.Any())
		{
			return list;
		}
		if (index > 0)
		{
			return list.Take(index - 1).ToList();
		}
		return new List<T>();
	}
}
