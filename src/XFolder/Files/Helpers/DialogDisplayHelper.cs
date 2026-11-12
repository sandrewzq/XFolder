using System;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Threading;
using DialogHostAvalonia;
using Files.Assets.Lang;
using Files.Data.Enums;
using Files.Dialogs;
using Files.ViewModels.Dialogs;

namespace Files.Helpers;

public sealed class DialogDisplayHelper
{
	private static readonly SemaphoreSlim DialogSemaphore = new SemaphoreSlim(1, 1);

	public static async Task<bool> ShowAskDialogAsync(string title, string message, string? primaryText = null, string? closeText = null)
	{
		if (primaryText == null)
		{
			primaryText = Resources.OK;
		}
		if (closeText == null)
		{
			closeText = Resources.Cancel;
		}
		if (await ShowDialogSafeAsync(new DynamicDialog(new DynamicDialogViewModel
		{
			TitleText = title,
			SubtitleText = message,
			PrimaryButtonText = primaryText,
			CloseButtonText = closeText,
			DynamicButtons = (DynamicDialogButtons)5
		})) is DynamicDialogResult dynamicDialogResult && dynamicDialogResult == DynamicDialogResult.Primary)
		{
			return true;
		}
		return false;
	}

	public static async Task<DynamicDialogResult> ShowOptionsDialogAsync(string title, string message, string primaryText, string secondaryText, string? closeText = null)
	{
		if (closeText == null)
		{
			closeText = Resources.Cancel;
		}
		return ParseDynamicDialogResult(await ShowDialogSafeAsync(new DynamicDialog(new DynamicDialogViewModel
		{
			TitleText = title,
			SubtitleText = message,
			PrimaryButtonText = primaryText,
			SecondaryButtonText = secondaryText,
			CloseButtonText = closeText,
			DynamicButtons = (DynamicDialogButtons)7
		})));
	}

	public static async Task<bool> ShowTipAsync(string title, string message, string? primaryText = null)
	{
		if (primaryText == null)
		{
			primaryText = Resources.OK;
		}
		if (await ShowDialogSafeAsync(new DynamicDialog(new DynamicDialogViewModel
		{
			TitleText = title,
			SubtitleText = message,
			PrimaryButtonText = primaryText,
			DynamicButtons = DynamicDialogButtons.Primary
		})) is DynamicDialogResult dynamicDialogResult && dynamicDialogResult == DynamicDialogResult.Primary)
		{
			return true;
		}
		return false;
	}

	private static async Task<object?> ShowDialogSafeAsync(DynamicDialog dialog)
	{
		await DialogSemaphore.WaitAsync();
		try
		{
			int waitCount = 0;
			while (DialogHost.IsDialogOpen("MainDialogHost") && waitCount < 30)
			{
				await Task.Delay(10);
				waitCount++;
			}
			await Dispatcher.UIThread.InvokeAsync(() =>
			{
			}, DispatcherPriority.Background);
			return await DialogHost.Show(dialog, "MainDialogHost");
		}
		finally
		{
			DialogSemaphore.Release();
		}
	}

	private static DynamicDialogResult ParseDynamicDialogResult(object? dialogResult)
	{
		if (dialogResult is DynamicDialogResult)
		{
			return (DynamicDialogResult)dialogResult;
		}
		if (dialogResult is int num && Enum.IsDefined(typeof(DynamicDialogResult), num))
		{
			return (DynamicDialogResult)num;
		}
		if (dialogResult is string value && Enum.TryParse<DynamicDialogResult>(value, ignoreCase: true, out var result))
		{
			return result;
		}
		return DynamicDialogResult.None;
	}
}
