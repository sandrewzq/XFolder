using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace Files.Extensions;

public static class EnumerableExtensions
{
	public static IEnumerable<T> CreateEnumerable<T>(this T item)
	{
		return new _003C_003Ez__ReadOnlySingleElementList<T>(item);
	}

	public static List<T> CreateList<T>(this T item)
	{
		int num = 1;
		List<T> list = new List<T>(num);
		CollectionsMarshal.SetCount(list, num);
		CollectionsMarshal.AsSpan(list)[0] = item;
		return list;
	}

	public static IList<T> AddIfNotPresent<T>(this IList<T> list, T element)
	{
		if (!list.Contains(element))
		{
			list.Add(element);
		}
		return list;
	}

	public static ICollection<T> EnumeratedAdd<T>(this ICollection<T> collection, IEnumerable<T> source)
	{
		foreach (T item in source)
		{
			collection.Add(item);
		}
		return collection;
	}

	public static IDictionary<TKey, TValue> AddIfNotPresent<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue value)
	{
		if (!dictionary.ContainsKey(key))
		{
			dictionary.Add(key, value);
		}
		return dictionary;
	}

	public static async Task ParallelForEachAsync<T>(this IEnumerable<T> source, Func<T, Task> body, int maxDegreeOfParallelism = -1, CancellationToken cts = default(CancellationToken), TaskScheduler? scheduler = null)
	{
		ExecutionDataflowBlockOptions executionDataflowBlockOptions = new ExecutionDataflowBlockOptions
		{
			MaxDegreeOfParallelism = maxDegreeOfParallelism,
			CancellationToken = cts
		};
		if (scheduler != null)
		{
			executionDataflowBlockOptions.TaskScheduler = scheduler;
		}
		ActionBlock<T> actionBlock = new ActionBlock<T>(body, executionDataflowBlockOptions);
		foreach (T item in source)
		{
			actionBlock.Post(item);
		}
		actionBlock.Complete();
		await actionBlock.Completion;
	}

	public static Task<IList<T>> ToListAsync<T>(this IEnumerable<T> source)
	{
		return Task.Run((Func<IList<T>>)(() => source.ToList()));
	}

	public static IEnumerable<TResult> Zip<T1, T2, TResult>(this IEnumerable<T1> source, IEnumerable<T2> second, Func<T1, T2, int, TResult> func)
	{
		using IEnumerator<T1> e1 = source.GetEnumerator();
		using IEnumerator<T2> e2 = second.GetEnumerator();
		int index = 0;
		while (e1.MoveNext() && e2.MoveNext())
		{
			yield return func(e1.Current, e2.Current, index++);
		}
	}
}
