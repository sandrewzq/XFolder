using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Threading;
using DialogHostAvalonia;
using Files.Assets.Lang;
using Files.Data.Enums;
using Files.Data.Items;
using Files.Dialogs;
using Files.Extensions;
using Files.Helpers;
using Files.ViewModels.Dialogs;
using SharpCompress.Common;
using SharpCompress.Writers;
using SharpCompress.Writers.Zip;

namespace Files.Actions;

internal sealed class CompressIntoZipAction : BaseCompressArchiveAction, IAction
{
	public override string Label => string.Format(Resources.CreateNamedArchive, BaseCompressArchiveAction.GenerateArchiveNameFromItems(context.SelectedItems) + ".zip");

	public override string Description => Resources.CompressIntoZipDescription.GetLocalizedFormatResource(context.SelectedItems.Count);

	public override async Task ExecuteAsync(object? parameter = null)
	{
		GetDestination(out IReadOnlyList<ListedItem> items, out string directory, out string fileName);
		if (items == null || items.Count == 0 || string.IsNullOrEmpty(directory))
		{
			return;
		}
		if (string.IsNullOrEmpty(fileName))
		{
			fileName = Resources.NewFile.GetLocalizedFormatResource();
		}
		string zipPath = Path.Combine(directory, fileName + ".zip");
		int num = 1;
		while (File.Exists(zipPath))
		{
			zipPath = Path.Combine(directory, $"{fileName} ({num++}).zip");
		}
		CancellationTokenSource cts = new CancellationTokenSource();
		ProgressDialogViewModel vm = new ProgressDialogViewModel
		{
			TitleText = Resources.Compress,
			SubtitleText = string.Empty,
			CurrentItemText = string.Empty
		};
		ProgressDialog content = new ProgressDialog(vm);
		Task<object?> dialogTask = DialogHost.Show(content, "MainDialogHost");
		Task compressionTask = Task.Run(() =>
		{
			CompressZip(items, zipPath, vm, cts.Token);
		}, cts.Token);
		if (await Task.WhenAny(dialogTask, compressionTask) == dialogTask)
		{
			cts.Cancel();
			try
			{
				await compressionTask;
			}
			catch
			{
			}
			BaseCompressArchiveAction.TryDeleteFile(zipPath);
			return;
		}
		try
		{
			await compressionTask;
		}
		catch (OperationCanceledException)
		{
			BaseCompressArchiveAction.TryDeleteFile(zipPath);
		}
		catch
		{
			BaseCompressArchiveAction.TryDeleteFile(zipPath);
		}
		finally
		{
			Dispatcher.UIThread.Post(() =>
			{
				DialogHost.Close("MainDialogHost", DynamicDialogResult.Primary);
			});
		}
	}

	private static void CompressZip(IReadOnlyList<ListedItem> items, string zipPath, ProgressDialogViewModel vm, CancellationToken token)
	{
		token.ThrowIfCancellationRequested();
		try
		{
			long totalBytes = BaseCompressArchiveAction.CalculateTotalBytes(items, vm, token);
			Dispatcher.UIThread.Post(() =>
			{
				vm.ProgressValue = 0.0;
				vm.IsIndeterminate = totalBytes <= 0;
				vm.SubtitleText = ((totalBytes > 0) ? ("0%  " + StringsHelper.FormatFileSize(0L) + " / " + StringsHelper.FormatFileSize(totalBytes)) : string.Empty);
			});
			using FileStream destination = new FileStream(zipPath, FileMode.Create, FileAccess.Write, FileShare.None);
			OverallProgressReporter progress = ((totalBytes > 0) ? new OverallProgressReporter(totalBytes, vm, token) : null);
			ZipWriterOptions zipWriterOptions = new ZipWriterOptions(CompressionType.Deflate)
			{
				Progress = progress
			};
			using ZipWriter writer = new ZipWriter(destination, zipWriterOptions);
			foreach (ListedItem item in items)
			{
				token.ThrowIfCancellationRequested();
				BaseCompressArchiveAction.UpdateCurrentItem(vm, item.ItemPath);
				if (item.IsFolder)
				{
					BaseCompressArchiveAction.AddDirectoryToArchive(writer, item.ItemPath, item.Name, vm, token);
				}
				else
				{
					writer.Write(item.Name, item.ItemPath);
				}
			}
			token.ThrowIfCancellationRequested();
			if (totalBytes > 0)
			{
				Dispatcher.UIThread.Post(() =>
				{
					vm.ProgressValue = 100.0;
					vm.IsIndeterminate = false;
					vm.SubtitleText = $"100%  {StringsHelper.FormatFileSize(totalBytes)} / {StringsHelper.FormatFileSize(totalBytes)}  " + string.Format(Resources.RemainingTime, "00:00");
				});
			}
		}
		finally
		{
			BaseCompressArchiveAction.UpdateCurrentItem(vm, string.Empty);
		}
	}
}
