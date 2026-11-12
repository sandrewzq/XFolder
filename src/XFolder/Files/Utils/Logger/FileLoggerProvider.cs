using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Files.Utils.Logger;

public sealed class FileLoggerProvider : ILoggerProvider, IDisposable
{
	private readonly FileLogger logger;

	private int purgeScheduled;

	public FileLoggerProvider(string path)
	{
		logger = new FileLogger(path);
	}

	public ILogger CreateLogger(string categoryName)
	{
		// All categories share the same file and the same writer. Purging is a
		// full-file read/rewrite, so only do it once instead of per category.
		if (Interlocked.Exchange(ref purgeScheduled, 1) == 0)
		{
			Task.Run(() =>
			{
				logger.PurgeLogs(100);
			});
		}
		return logger;
	}

	public void Dispose()
	{
		logger.Dispose();
	}
}
