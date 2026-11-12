using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace Files.Utils.Logger;

public sealed class FileLogger : ILogger, IDisposable
{
	private readonly object syncRoot = new object();

	private readonly string filePath;

	private StreamWriter? writer;

	public FileLogger(string filePath)
	{
		this.filePath = filePath;
	}

	public IDisposable? BeginScope<TState>(TState state) where TState : notnull
	{
		return null;
	}

	public bool IsEnabled(LogLevel logLevel)
	{
		return true;
	}

	public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
	{
		if (formatter == null)
		{
			return;
		}
		try
		{
			string value = exception?.ToString() ?? formatter(state, exception);
			lock (syncRoot)
			{
				writer ??= CreateWriter();
				writer.Write($"{DateTime.Now:yyyy-MM-dd HH:mm:ss.ffff}|{logLevel}|{value}");
				writer.Write(Environment.NewLine);
			}
		}
		catch (Exception)
		{
		}
	}

	private StreamWriter CreateWriter()
	{
		// Keep the file open between writes and allow readers (tail) to follow it.
		FileStream stream = new FileStream(filePath, FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
		return new StreamWriter(stream)
		{
			AutoFlush = true
		};
	}

	private void CloseWriter()
	{
		if (writer != null)
		{
			try
			{
				writer.Dispose();
			}
			catch (Exception)
			{
			}
			writer = null;
		}
	}

	public void PurgeLogs(int numberOfLinesKept)
	{
		if (!File.Exists(filePath))
		{
			return;
		}
		try
		{
			lock (syncRoot)
			{
				CloseWriter();
				string[] array = File.ReadAllLines(filePath);
				if (array.Length > numberOfLinesKept)
				{
					IEnumerable<string> contents = array.Skip(Math.Max(0, array.Length - numberOfLinesKept));
					File.WriteAllLines(filePath, contents);
				}
			}
		}
		catch (Exception)
		{
		}
	}

	public void Dispose()
	{
		lock (syncRoot)
		{
			CloseWriter();
		}
	}
}
