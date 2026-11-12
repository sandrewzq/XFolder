using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.DependencyInjection;
using DialogHostAvalonia;
using Files.Data.Contexts.ContentPage;
using Files.Data.Enums;
using Files.Data.Items;
using Files.Dialogs;
using Files.Helpers.Shared;
using Files.Services.NativePlugin;
using Files.Utils.Storage.Helpers;
using Files.Utils.Storage.History;
using Files.ViewModels.Dialogs;
using Files.ViewModels.Shells;

namespace Files.Actions.Content.ImageManipulation;

internal abstract class BaseConvertImageAction : BaseUIAction, IAction
{
	protected const int PngFormat = 1;

	protected const int JpegFormat = 2;

	protected const int BmpFormat = 3;

	protected readonly IContentPageContext context;

	public abstract string Label { get; }

	public abstract string Description { get; }

	protected abstract int TargetFormat { get; }

	protected abstract string TargetExtension { get; }

	protected virtual string[] TargetEquivalentExtensions => new string[1] { TargetExtension };

	public override bool IsExecutable
	{
		get
		{
			if (base.IsExecutable && context.ShellView != null)
			{
				return GetSelectedConvertibleItem() != null;
			}
			return false;
		}
	}

	protected BaseConvertImageAction()
	{
		context = Ioc.Default.GetRequiredService<IContentPageContext>();
		context.PropertyChanged += Context_PropertyChanged;
	}

	public async Task ExecuteAsync(object? parameter = null)
	{
		ListedItem selectedConvertibleItem = GetSelectedConvertibleItem();
		if (selectedConvertibleItem == null)
		{
			return;
		}
		string sourcePath = selectedConvertibleItem.ItemPath;
		string destinationPath = GenerateDestinationPath(sourcePath);
		if (string.IsNullOrWhiteSpace(destinationPath))
		{
			return;
		}
		int targetFormat = TargetFormat;
		CancellationTokenSource cts = new CancellationTokenSource();
		Task<bool> conversionTask = Task.Run(() =>
		{
			cts.Token.ThrowIfCancellationRequested();
			return MacPluginInterop.ConvertImageToFormat(sourcePath, destinationPath, targetFormat);
		}, cts.Token);
		Task delayTask = Task.Delay(1000);
		bool isDialogShown = false;
		if (await Task.WhenAny(conversionTask, delayTask) == delayTask && !conversionTask.IsCompleted)
		{
			ProgressDialog content = new ProgressDialog(new ProgressDialogViewModel
			{
				TitleText = Label,
				SubtitleText = sourcePath,
				CurrentItemText = destinationPath,
				IsIndeterminate = true
			});
			Task<object?> dialogTask = DialogHost.Show(content, "MainDialogHost");
			isDialogShown = true;
			if (await Task.WhenAny(dialogTask, conversionTask) == dialogTask)
			{
				cts.Cancel();
				try
				{
					await conversionTask;
				}
				catch
				{
				}
				TryDeleteFile(destinationPath);
				return;
			}
		}
		bool flag = false;
		try
		{
			flag = await conversionTask;
		}
		catch (OperationCanceledException)
		{
			TryDeleteFile(destinationPath);
			return;
		}
		catch
		{
			TryDeleteFile(destinationPath);
			return;
		}
		finally
		{
			if (isDialogShown)
			{
				Dispatcher.UIThread.Post(() =>
				{
					DialogHost.Close("MainDialogHost", DynamicDialogResult.Primary);
				});
			}
		}
		if (flag)
		{
			RegisterHistory(sourcePath, destinationPath);
			RefreshParentDirectory(sourcePath);
		}
		else
		{
			TryDeleteFile(destinationPath);
		}
	}

	private ListedItem? GetSelectedConvertibleItem()
	{
		IReadOnlyList<ListedItem> selectedItems = context.SelectedItems;
		if (selectedItems.Count != 1)
		{
			return null;
		}
		ListedItem listedItem = selectedItems[0];
		if (listedItem.IsFolder || string.IsNullOrWhiteSpace(listedItem.ItemPath) || !File.Exists(listedItem.ItemPath))
		{
			return null;
		}
		if (!FileExtensionHelpers.IsImageFile(listedItem.FileExtension))
		{
			return null;
		}
		if (IsAlreadyTargetFormat(listedItem.ItemPath))
		{
			return null;
		}
		if (!MacPluginInterop.CanConvertImageToFormat(listedItem.ItemPath, TargetFormat))
		{
			return null;
		}
		return listedItem;
	}

	private bool IsAlreadyTargetFormat(string path)
	{
		string extension = Path.GetExtension(path);
		return TargetEquivalentExtensions.Any((string x) => string.Equals(extension, x, StringComparison.OrdinalIgnoreCase));
	}

	private string? GenerateDestinationPath(string sourcePath)
	{
		string directoryName = Path.GetDirectoryName(sourcePath);
		if (string.IsNullOrWhiteSpace(directoryName))
		{
			return null;
		}
		string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(sourcePath);
		string text = Path.Combine(directoryName, fileNameWithoutExtension + TargetExtension);
		int num = 1;
		while (File.Exists(text))
		{
			text = Path.Combine(directoryName, $"{fileNameWithoutExtension} ({num++}){TargetExtension}");
		}
		return text;
	}

	private void RefreshParentDirectory(string sourcePath)
	{
		BaseShellViewModel baseShellViewModel = context.ShellView?.InstanceViewModel;
		string text = ((!string.IsNullOrWhiteSpace(baseShellViewModel?.WorkingDirectory)) ? baseShellViewModel.WorkingDirectory : Path.GetDirectoryName(sourcePath));
		if (!string.IsNullOrWhiteSpace(text))
		{
			baseShellViewModel?.SelectedTab?.LayoutViewModel?.RefreshItems(text);
		}
	}

	private static void RegisterHistory(string sourcePath, string destinationPath)
	{
		App.HistoryWrapper.AddHistory(new StorageHistory(FileOperationType.ImageConvert, StorageHelpers.FromPathAndType(sourcePath, FilesystemItemType.File), StorageHelpers.FromPathAndType(destinationPath, FilesystemItemType.File)));
	}

	private static void TryDeleteFile(string path)
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

	private void Context_PropertyChanged(object? sender, PropertyChangedEventArgs e)
	{
		switch (e.PropertyName)
		{
		case "ShellView":
		case "HasSelection":
		case "SelectedItem":
		case "SelectedItems":
			OnPropertyChanged("IsExecutable");
			break;
		}
	}
}
