using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.DependencyInjection;
using Files.Assets.Lang;
using Files.Data.Contexts.ContentPage;
using Files.Data.Items;
using Files.Extensions;
using Files.Helpers;
using Files.Helpers.UI;
using Files.ViewModels.Dialogs;
using SharpCompress.Common;
using SharpCompress.Writers;

namespace Files.Actions;

internal abstract class BaseCompressArchiveAction : BaseUIAction, IAction
{
	protected sealed class OverallProgressReporter : IProgress<ProgressReport>
	{
		private readonly long totalBytes;

		private readonly ProgressDialogViewModel vm;

		private readonly CancellationToken token;

		private long transferred;

		private long lastSpeedTimestamp;

		private long lastSpeedTransferred;

		private double smoothedBytesPerSecond;

		private long lastEtaUpdateTimestamp;

		private string? lastEtaText;

		private string? currentEntry;

		private long currentEntryTransferred;

		private long lastUpdateTimestamp;

		private double lastPercent;

		public OverallProgressReporter(long totalBytes, ProgressDialogViewModel vm, CancellationToken token)
		{
			this.totalBytes = totalBytes;
			this.vm = vm;
			this.token = token;
			lastSpeedTimestamp = Stopwatch.GetTimestamp();
			lastSpeedTransferred = 0L;
			lastEtaUpdateTimestamp = 0L;
			lastEtaText = null;
		}

		public void Report(ProgressReport value)
		{
			token.ThrowIfCancellationRequested();
			if (totalBytes <= 0)
			{
				return;
			}
			if (!string.Equals(currentEntry, value.EntryPath, StringComparison.Ordinal))
			{
				currentEntry = value.EntryPath;
				currentEntryTransferred = 0L;
			}
			long num = value.BytesTransferred - currentEntryTransferred;
			if (num < 0)
			{
				num = value.BytesTransferred;
			}
			currentEntryTransferred = value.BytesTransferred;
			transferred += num;
			long timestamp = Stopwatch.GetTimestamp();
			UpdateSpeed(timestamp);
			double percent = Math.Clamp((double)transferred * 100.0 / (double)totalBytes, 0.0, 100.0);
			if (ShouldUpdate(timestamp, percent) || !(percent < 100.0))
			{
				long safeTransferred = Math.Clamp(transferred, 0L, totalBytes);
				string subtitle = BuildSubtitle(timestamp, percent, safeTransferred);
				Dispatcher.UIThread.Post(() =>
				{
					vm.IsIndeterminate = false;
					vm.ProgressValue = percent;
					vm.SubtitleText = subtitle;
				});
			}
		}

		private void UpdateSpeed(long now)
		{
			if (lastSpeedTimestamp == 0L)
			{
				lastSpeedTimestamp = now;
				lastSpeedTransferred = transferred;
				return;
			}
			double num = (double)(now - lastSpeedTimestamp) / (double)Stopwatch.Frequency;
			if (!(num <= 0.0))
			{
				long num2 = transferred - lastSpeedTransferred;
				if (num2 < 0)
				{
					num2 = 0L;
				}
				double num3 = (double)num2 / num;
				if (num3 > 0.0)
				{
					smoothedBytesPerSecond = ((smoothedBytesPerSecond <= 0.0) ? num3 : (smoothedBytesPerSecond * 0.8 + num3 * 0.2));
				}
				lastSpeedTimestamp = now;
				lastSpeedTransferred = transferred;
			}
		}

		private string BuildSubtitle(long now, double percent, long safeTransferred)
		{
			string value = StringsHelper.FormatFileSize(safeTransferred);
			string value2 = StringsHelper.FormatFileSize(totalBytes);
			if (!(smoothedBytesPerSecond <= 0.0) && safeTransferred < totalBytes)
			{
				if (ShouldUpdateEta(now) || lastEtaText == null || percent >= 100.0)
				{
					double num = (double)(totalBytes - safeTransferred) / smoothedBytesPerSecond;
					if (double.IsFinite(num) && num >= 0.0)
					{
						string arg = FormatTime(TimeSpan.FromSeconds(num));
						lastEtaText = string.Format(Resources.RemainingTime, arg);
						lastEtaUpdateTimestamp = now;
					}
					else
					{
						lastEtaText = null;
						lastEtaUpdateTimestamp = now;
					}
				}
				if (lastEtaText == null)
				{
					return $"{percent:0}%  {value} / {value2}";
				}
				return $"{percent:0}%  {value} / {value2}  {lastEtaText}";
			}
			return $"{percent:0}%  {value} / {value2}";
		}

		private bool ShouldUpdateEta(long now)
		{
			if (lastEtaUpdateTimestamp == 0L)
			{
				return true;
			}
			return (double)(now - lastEtaUpdateTimestamp) * 1000.0 / (double)Stopwatch.Frequency >= 1000.0;
		}

		private static string FormatTime(TimeSpan time)
		{
			if (!(time.TotalHours >= 1.0))
			{
				return $"{time.Minutes:00}:{time.Seconds:00}";
			}
			return $"{(int)time.TotalHours}:{time.Minutes:00}:{time.Seconds:00}";
		}

		private bool ShouldUpdate(long timestamp, double percent)
		{
			if (lastUpdateTimestamp == 0L)
			{
				lastUpdateTimestamp = timestamp;
				lastPercent = percent;
				return true;
			}
			if ((double)(timestamp - lastUpdateTimestamp) * 1000.0 / (double)Stopwatch.Frequency < 50.0 && percent - lastPercent < 0.2)
			{
				return false;
			}
			lastUpdateTimestamp = timestamp;
			lastPercent = percent;
			return true;
		}
	}

	protected readonly IContentPageContext context;

	public abstract string Label { get; }

	public abstract string Description { get; }

	public string? Icon => "folder_zip_regular";

	public override bool IsExecutable
	{
		get
		{
			if (context.ShellView != null && context.HasSelection)
			{
				return UIHelpers.CanShowDialog;
			}
			return false;
		}
	}

	protected BaseCompressArchiveAction()
	{
		context = Ioc.Default.GetRequiredService<IContentPageContext>();
		context.PropertyChanged += Context_PropertyChanged;
	}

	public abstract Task ExecuteAsync(object? parameter = null);

	protected void GetDestination(out IReadOnlyList<ListedItem>? items, out string directory, out string fileName)
	{
		items = context.SelectedItems;
		directory = context.ShellView?.InstanceViewModel?.WorkingDirectory ?? string.Empty;
		fileName = string.Empty;
		if (items != null && items.Count != 0)
		{
			if (string.IsNullOrEmpty(directory))
			{
				directory = Path.GetDirectoryName(items[0].ItemPath) ?? string.Empty;
			}
			if (items.Count == 1)
			{
				fileName = items[0].Name;
			}
			else
			{
				fileName = Path.GetFileName(directory);
			}
		}
	}

	private void Context_PropertyChanged(object? sender, PropertyChangedEventArgs e)
	{
		switch (e.PropertyName)
		{
		case "ShellView":
		case "HasSelection":
		case "SelectedItems":
			OnPropertyChanged("IsExecutable");
			OnPropertyChanged("Label");
			OnPropertyChanged("Description");
			break;
		}
	}

	protected static string GenerateArchiveNameFromItems(IReadOnlyList<ListedItem>? items)
	{
		if (items == null || items.Count == 0)
		{
			return Resources.NewFile.GetLocalizedFormatResource();
		}
		if (items.Count == 1)
		{
			return items[0].Name;
		}
		string directoryName = Path.GetDirectoryName(items[0].ItemPath);
		if (string.IsNullOrEmpty(directoryName))
		{
			return Resources.ArchiveTitle;
		}
		string fileName = Path.GetFileName(directoryName);
		if (string.IsNullOrEmpty(fileName))
		{
			return Resources.ArchiveTitle;
		}
		return fileName;
	}

	protected static long CalculateTotalBytes(IReadOnlyList<ListedItem> items, ProgressDialogViewModel vm, CancellationToken token)
	{
		long num = 0L;
		foreach (ListedItem item in items)
		{
			token.ThrowIfCancellationRequested();
			if (item.IsFolder)
			{
				foreach (string item2 in EnumerateFilesRecursive(item.ItemPath, token))
				{
					token.ThrowIfCancellationRequested();
					UpdateCurrentItem(vm, item2);
					try
					{
						num += new FileInfo(item2).Length;
					}
					catch
					{
					}
				}
			}
			else
			{
				try
				{
					num += new FileInfo(item.ItemPath).Length;
				}
				catch
				{
				}
			}
		}
		return num;
	}

	protected static IEnumerable<string> EnumerateFilesRecursive(string root, CancellationToken token)
	{
		Stack<string> stack = new Stack<string>();
		stack.Push(root);
		while (stack.Count > 0)
		{
			token.ThrowIfCancellationRequested();
			string dir = stack.Pop();
			string[] files;
			try
			{
				files = Directory.GetFiles(dir);
			}
			catch
			{
				continue;
			}
			string[] array = files;
			foreach (string text in array)
			{
				token.ThrowIfCancellationRequested();
				yield return text;
			}
			string[] directories;
			try
			{
				directories = Directory.GetDirectories(dir);
			}
			catch
			{
				continue;
			}
			string[] array2 = directories;
			foreach (string item in array2)
			{
				token.ThrowIfCancellationRequested();
				stack.Push(item);
			}
		}
	}

	protected static void AddDirectoryToArchive(IWriter writer, string sourceDir, string entryPrefix, ProgressDialogViewModel vm, CancellationToken token)
	{
		try
		{
			token.ThrowIfCancellationRequested();
			string[] files = Directory.GetFiles(sourceDir);
			foreach (string text in files)
			{
				token.ThrowIfCancellationRequested();
				UpdateCurrentItem(vm, text);
				string fileName = Path.GetFileName(text);
				string entryPath = Path.Combine(entryPrefix, fileName);
				writer.Write(entryPath, text);
			}
			files = Directory.GetDirectories(sourceDir);
			foreach (string text2 in files)
			{
				token.ThrowIfCancellationRequested();
				UpdateCurrentItem(vm, text2);
				string fileName2 = Path.GetFileName(text2);
				AddDirectoryToArchive(writer, text2, Path.Combine(entryPrefix, fileName2), vm, token);
			}
		}
		catch (OperationCanceledException)
		{
			throw;
		}
		catch
		{
		}
	}

	protected static void UpdateCurrentItem(ProgressDialogViewModel vm, string path)
	{
		Dispatcher.UIThread.Post(() =>
		{
			vm.CurrentItemText = path;
		});
	}

	protected static void TryDeleteFile(string path)
	{
		try
		{
			if (File.Exists(path))
			{
				File.Delete(path);
			}
		}
		catch
		{
		}
	}
}
