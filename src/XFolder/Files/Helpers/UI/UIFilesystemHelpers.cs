using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Messaging;
using DialogHostAvalonia;
using Files.Assets.Lang;
using Files.Data.Contracts;
using Files.Data.Enums;
using Files.Data.Enums.Storage;
using Files.Data.Items;
using Files.Data.Messages;
using Files.Dialogs;
using Files.Extensions;
using Files.Services.NativePlugin;
using Files.Utils.Storage.Helpers;
using Files.Utils.Storage.Operations;
using Files.ViewModels.Dialogs;

namespace Files.Helpers.UI;

public static class UIFilesystemHelpers
{
	private sealed class TransferProgressReporter : IProgress<FileOperationProgress>
	{
		private const int SubtitleTextWidth = 64;

		private const int CurrentItemTextWidth = 58;

		private const int FileSizePairTextWidth = 23;

		private const int SpeedTextWidth = 12;

		private const int RemainingTimeTextWidth = 18;

		private readonly ProgressDialogViewModel vm;

		private readonly CancellationToken token;

		private long lastSpeedTimestamp;

		private long lastSpeedBytes;

		private double smoothedBytesPerSecond;

		private long lastEtaUpdateTimestamp;

		private string? lastEtaText;

		private long lastUpdateTimestamp;

		private double lastPercent;

		public TransferProgressReporter(ProgressDialogViewModel vm, CancellationToken token)
		{
			this.vm = vm;
			this.token = token;
			lastSpeedTimestamp = Stopwatch.GetTimestamp();
		}

		public void Report(FileOperationProgress value)
		{
			if (token.IsCancellationRequested)
			{
				return;
			}
			if (value.IsPreparing)
			{
				Dispatcher.UIThread.Post(() =>
				{
					vm.IsIndeterminate = true;
					vm.SubtitleText = ToFixedWidth("DiscoveringItems".GetLocalizedFormatResource(), 64);
					vm.CurrentItemText = ToFixedPath(value.CurrentPath);
				});
			}
			else if (value.TotalBytes > 0)
			{
				ReportBytes(value);
			}
			else if (value.TotalItems > 0)
			{
				double percent = Math.Clamp((double)value.ProcessedItems * 100.0 / (double)value.TotalItems, 0.0, 100.0);
				Dispatcher.UIThread.Post(() =>
				{
					vm.IsIndeterminate = false;
					vm.ProgressValue = percent;
					vm.SubtitleText = ToFixedWidth($"{percent,3:0}%  {value.ProcessedItems,4} / {value.TotalItems,4}", 64);
					vm.CurrentItemText = ToFixedPath(value.CurrentPath);
				});
			}
			else
			{
				Dispatcher.UIThread.Post(() =>
				{
					vm.IsIndeterminate = true;
					vm.SubtitleText = ToFixedWidth(Resources.ProcessingItems, 64);
					vm.CurrentItemText = ToFixedPath(value.CurrentPath);
				});
			}
		}

		private void ReportBytes(FileOperationProgress value)
		{
			long timestamp = Stopwatch.GetTimestamp();
			double percent = Math.Clamp((double)value.ProcessedBytes * 100.0 / (double)value.TotalBytes, 0.0, 100.0);
			UpdateSpeed(timestamp, value.ProcessedBytes);
			if (ShouldUpdate(timestamp, percent) || !(percent < 100.0))
			{
				string subtitle = BuildSubtitle(timestamp, percent, value.ProcessedBytes, value.TotalBytes);
				Dispatcher.UIThread.Post(() =>
				{
					vm.IsIndeterminate = false;
					vm.ProgressValue = percent;
					vm.SubtitleText = subtitle;
					vm.CurrentItemText = ToFixedPath(value.CurrentPath);
				});
			}
		}

		private void UpdateSpeed(long now, long processedBytes)
		{
			double num = (double)(now - lastSpeedTimestamp) / (double)Stopwatch.Frequency;
			if (!(num <= 0.0))
			{
				long num2 = processedBytes - lastSpeedBytes;
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
				lastSpeedBytes = processedBytes;
			}
		}

		private string BuildSubtitle(long now, double percent, long processedBytes, long totalBytes)
		{
			string value = $"{percent,3:0}%";
			string text = StringsHelper.FormatFileSize(Math.Clamp(processedBytes, 0L, totalBytes));
			string text2 = StringsHelper.FormatFileSize(totalBytes);
			string value2 = ToFixedWidth(text + " / " + text2, 23);
			if (smoothedBytesPerSecond <= 0.0 || processedBytes >= totalBytes)
			{
				return ToFixedWidth($"{value}  {value2}  {new string(' ', 12)}  {new string(' ', 18)}", 64);
			}
			if (ShouldUpdateEta(now) || lastEtaText == null || percent >= 100.0)
			{
				double num = (double)(totalBytes - processedBytes) / smoothedBytesPerSecond;
				if (double.IsFinite(num) && num >= 0.0)
				{
					lastEtaText = string.Format(Resources.RemainingTime, FormatTime(TimeSpan.FromSeconds(num)));
					lastEtaUpdateTimestamp = now;
				}
				else
				{
					lastEtaText = null;
					lastEtaUpdateTimestamp = now;
				}
			}
			string value3 = ToFixedWidth(StringsHelper.FormatFileSize((long)smoothedBytesPerSecond) + "/s", 12, alignRight: true);
			string value4 = ToFixedWidth(lastEtaText ?? string.Empty, 18);
			return ToFixedWidth($"{value}  {value2}  {value3}  {value4}", 64);
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

		private static string ToFixedPath(string path)
		{
			if (string.IsNullOrEmpty(path))
			{
				return new string(' ', 58);
			}
			string text = path.Replace(Environment.NewLine, " ");
			if (text.Length <= 58)
			{
				return text.PadRight(58);
			}
			int num = 58 - "...".Length;
			int num2 = num / 2;
			int num3 = num - num2;
			string text2 = text.Substring(0, num2);
			int num4 = num3;
			int length = text.Length;
			int num5 = length - num4;
			return text2 + "..." + text.Substring(num5, length - num5);
		}

		private static string ToFixedWidth(string text, int width, bool alignRight = false)
		{
			if (string.IsNullOrEmpty(text))
			{
				return new string(' ', width);
			}
			string text2 = text.Replace(Environment.NewLine, " ");
			if (text2.Length > width)
			{
				return text2.Substring(0, width);
			}
			if (!alignRight)
			{
				return text2.PadRight(width);
			}
			return text2.PadLeft(width);
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

	public static async Task PasteItemAsync(string destinationPath, IShellView associatedInstance)
	{
		if (string.IsNullOrWhiteSpace(destinationPath) || !Directory.Exists(destinationPath))
		{
			return;
		}
		bool isCut = MacPluginInterop.ClipboardIsCutMode();
		List<string> clipboardFileList = MacPluginInterop.GetClipboardFileList();
		if (clipboardFileList == null || clipboardFileList.Count == 0)
		{
			return;
		}
		List<IStorageItemWithPath> sourceItems = clipboardFileList.Select((string path) => StorageHelpers.FromPathAndType(path)).OfType<IStorageItemWithPath>().ToList();
		if (sourceItems.Count == 0)
		{
			return;
		}
		List<string> destinations = sourceItems.Select((IStorageItemWithPath item) => Path.Combine(destinationPath, item.Name)).ToList();
		(List<NameCollisionOption>, bool) tuple = await associatedInstance.FilesystemHelpers.GetCollision(sourceItems, destinations, showDialog: true);
		var (collisions, _) = tuple;
		if (tuple.Item2)
		{
			return;
		}
		try
		{
			ReturnResult returnResult = await ShowTransferProgressAsync(isCut, sourceItems, destinations, collisions, associatedInstance);
			if (isCut && returnResult == ReturnResult.Success)
			{
				MacPluginInterop.ClearClipboard();
				App.AppModel.NotifyPasteAvailabilityChanged();
			}
		}
		catch
		{
		}
		finally
		{
			associatedInstance.InstanceViewModel?.SelectedTab?.LayoutViewModel?.RefreshItemsOpacityByClipboardCutStatus();
		}
	}

	private static async Task<ReturnResult> ShowTransferProgressAsync(bool isCut, IReadOnlyList<IStorageItemWithPath> sourceItems, IReadOnlyList<string> destinations, IReadOnlyList<NameCollisionOption> collisions, IShellView associatedInstance)
	{
		using CancellationTokenSource cts = new CancellationTokenSource();
		ProgressDialogViewModel vm = new ProgressDialogViewModel
		{
			TitleText = (isCut ? Resources.MoveItemsDialogPrimaryButtonText : Resources.Copy),
			SubtitleText = "DiscoveringItems".GetLocalizedFormatResource(),
			CurrentItemText = string.Empty
		};
		TransferProgressReporter progress = new TransferProgressReporter(vm, cts.Token);
		Task<ReturnResult> operationTask = (isCut ? associatedInstance.FilesystemHelpers.MoveItemsAsync(sourceItems, destinations, collisions, registerHistory: true, progress, cts.Token) : associatedInstance.FilesystemHelpers.CopyItemsAsync(sourceItems, destinations, collisions, registerHistory: true, progress, cts.Token));
		Task task = Task.Delay(TimeSpan.FromSeconds(1L));
		if (await Task.WhenAny(operationTask, task) == operationTask || operationTask.IsCompleted)
		{
			try
			{
				return await operationTask;
			}
			catch (OperationCanceledException)
			{
				return ReturnResult.Cancelled;
			}
		}
		ProgressDialog content = new ProgressDialog(vm);
		Task<object?> dialogTask = DialogHost.Show(content, "MainDialogHost");
		if (await Task.WhenAny(dialogTask, operationTask) == dialogTask)
		{
			cts.Cancel();
			try
			{
				return await operationTask;
			}
			catch
			{
				return ReturnResult.Cancelled;
			}
		}
		ReturnResult result;
		try
		{
			_ = 4;
			try
			{
				result = await operationTask;
			}
			catch (OperationCanceledException)
			{
				result = ReturnResult.Cancelled;
			}
		}
		finally
		{
			await Dispatcher.UIThread.InvokeAsync(() =>
			{
				if (DialogHost.IsDialogOpen("MainDialogHost"))
				{
					DialogHost.Close("MainDialogHost", DynamicDialogResult.Primary);
				}
			});
		}
		try
		{
			await dialogTask;
		}
		catch
		{
		}
		return result;
	}

	public static async Task CreateFileFromDialogResultTypeAsync(AddItemDialogItemType itemType, ShellNewEntry? itemInfo, IShellView associatedInstance)
	{
		await CreateFileFromDialogResultTypeForResult(itemType, itemInfo, associatedInstance);
	}

	private static async Task<IStorageItemWithPath?> CreateFileFromDialogResultTypeForResult(AddItemDialogItemType itemType, ShellNewEntry? itemInfo, IShellView associatedInstance)
	{
		string currentPath = associatedInstance.InstanceViewModel.WorkingDirectory;
		CreateItemDialogViewModel dlgViewModel = new CreateItemDialogViewModel();
		if (await Ioc.Default.GetRequiredService<IDialogService>().ShowDialogAsync(dlgViewModel) == DialogResult.Primary)
		{
			string text = dlgViewModel.AdditionalData as string;
			FilesystemItemType filesystemItemType;
			string createdName;
			switch (itemType)
			{
			case AddItemDialogItemType.Folder:
				createdName = ((!string.IsNullOrWhiteSpace(text)) ? text : Resources.NewFolder.GetLocalizedFormatResource());
				filesystemItemType = FilesystemItemType.Directory;
				break;
			case AddItemDialogItemType.File:
				createdName = ((!string.IsNullOrWhiteSpace(text)) ? text : (itemInfo?.Name ?? Resources.NewFile.GetLocalizedFormatResource()));
				createdName += itemInfo?.Extension;
				filesystemItemType = FilesystemItemType.File;
				break;
			default:
				return null;
			}
			string createdPath = Path.Combine(currentPath, createdName);
			ReturnResult item = (await associatedInstance.FilesystemHelpers.CreateAsync(StorageHelpers.FromPathAndType(createdPath, filesystemItemType), registerHistory: true)).Item1;
			if (item != ReturnResult.Success)
			{
				await DialogDisplayHelper.ShowTipAsync("", item switch
				{
					ReturnResult.AlreadyExists => string.Format(Resources.ItemAlreadyExistsDialogContent, createdName), 
					ReturnResult.AccessUnauthorized => Resources.AccessDenied, 
					_ => Resources.ExceptionNotificationBody, 
				});
				return null;
			}
			return StorageHelpers.FromPathAndType(createdPath, filesystemItemType);
		}
		return null;
	}

	public static async Task<bool> RenameFileItemAsync(ListedItem item, string newName, IShellView associatedInstance, bool showExtensionDialog = true)
	{
		if (item.ItemNameRaw == newName || string.IsNullOrEmpty(newName))
		{
			return true;
		}
		string oldPath = item.ItemPath;
		string newPath = Path.Combine(Path.GetDirectoryName(oldPath) ?? string.Empty, newName);
		if (await associatedInstance.FilesystemHelpers.RenameAsync(item.ToStorageItemWithPath(), newName, NameCollisionOption.FailIfExists, registerHistory: true, showExtensionDialog) == ReturnResult.Success)
		{
			WeakReferenceMessenger.Default.Send(new FileSystemChangedMessage
			{
				ChangeType = FileSystemChangeType.Renamed,
				OldPath = oldPath,
				Path = newPath
			});
			return true;
		}
		return false;
	}

	public static async Task CreateFolderWithSelectionAsync(IShellView associatedInstance, IReadOnlyList<ListedItem> selectedItems)
	{
		if (selectedItems.Count == 0)
		{
			return;
		}
		List<IStorageItemWithPath> sources = selectedItems.Select((ListedItem item) => item.ToStorageItemWithPath()).ToList();
		IStorageItemWithPath folder = await CreateFileFromDialogResultTypeForResult(AddItemDialogItemType.Folder, null, associatedInstance);
		if (folder != null)
		{
			List<string> destination = sources.Select((IStorageItemWithPath item) => Path.Combine(folder.Path, item.Name)).ToList();
			if (await associatedInstance.FilesystemHelpers.MoveItemsAsync(sources, destination, showDialog: false, registerHistory: true) != ReturnResult.Success)
			{
				await DialogDisplayHelper.ShowTipAsync(string.Empty, Resources.ExceptionNotificationBody);
			}
		}
	}
}
