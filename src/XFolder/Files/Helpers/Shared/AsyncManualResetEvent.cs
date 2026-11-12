using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Files.Helpers.Shared;

public sealed class AsyncManualResetEvent
{
	private volatile TaskCompletionSource<bool> m_tcs = new TaskCompletionSource<bool>();

	public async Task WaitAsync(CancellationToken cancellationToken = default(CancellationToken))
	{
		TaskCompletionSource<bool> tcs = m_tcs;
		TaskCompletionSource<bool> taskCompletionSource = new TaskCompletionSource<bool>();
		cancellationToken.Register((object? s) =>
		{
			((TaskCompletionSource<bool>)s).TrySetCanceled();
		}, taskCompletionSource);
		await (await Task.WhenAny(tcs.Task, taskCompletionSource.Task));
	}

	private async Task<bool> Delay(int milliseconds)
	{
		await Task.Delay(milliseconds);
		return false;
	}

	public async Task<bool> WaitAsync(int milliseconds, CancellationToken cancellationToken = default(CancellationToken))
	{
		TaskCompletionSource<bool> tcs = m_tcs;
		TaskCompletionSource<bool> taskCompletionSource = new TaskCompletionSource<bool>();
		cancellationToken.Register((object? s) =>
		{
			((TaskCompletionSource<bool>)s).TrySetCanceled();
		}, taskCompletionSource);
		InlineArray3<Task<bool>> buffer = default;
		buffer[0] = tcs.Task;
		buffer[1] = taskCompletionSource.Task;
		buffer[2] = Delay(milliseconds);
		return await (await Task.WhenAny<bool>(buffer));
	}

	public void Set()
	{
		TaskCompletionSource<bool> tcs = m_tcs;
		Task.Factory.StartNew((object? s) => ((TaskCompletionSource<bool>)s).TrySetResult(result: true), tcs, CancellationToken.None, TaskCreationOptions.PreferFairness, TaskScheduler.Default);
		tcs.Task.Wait();
	}

	public void Reset()
	{
		TaskCompletionSource<bool> value = new TaskCompletionSource<bool>();
		TaskCompletionSource<bool> tcs;
		do
		{
			tcs = m_tcs;
		}
		while (tcs.Task.IsCompleted && Interlocked.CompareExchange(ref m_tcs, value, tcs) != tcs);
	}
}
