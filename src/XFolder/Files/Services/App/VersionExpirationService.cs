using System;
using System.Diagnostics;
using System.Globalization;
using System.Threading.Tasks;
using Avalonia.Threading;
using DialogHostAvalonia;
using Files.Dialogs;
using Microsoft.Extensions.Logging;

namespace Files.Services.App;

public class VersionExpirationService
{
	private const string ExpirationDateFormat = "yyyyMMdd";

	private readonly ILogger<VersionExpirationService> _logger;

	private bool _hasShownExpiredDialog;

	public VersionExpirationService(ILogger<VersionExpirationService> logger)
	{
		_logger = logger;
	}

	public async Task<bool> CheckExpirationAsync()
	{
		if (!TryGetExpirationDate(out var expirationDate))
		{
			return false;
		}
		if (DateTime.Today <= expirationDate)
		{
			return false;
		}
		await ShowExpiredDialogAsync();
		return true;
	}

	private bool TryGetExpirationDate(out DateTime expirationDate)
	{
		if (DateTime.TryParseExact("20270501", "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out expirationDate))
		{
			return true;
		}
		_logger.LogError("Invalid expiration time: {ExpirationTime}", "20270501");
		return false;
	}

	private async Task ShowExpiredDialogAsync()
	{
		if (_hasShownExpiredDialog)
		{
			return;
		}
		_hasShownExpiredDialog = true;
		await Dispatcher.UIThread.InvokeAsync(async () =>
		{
			ExpiredVersionDialog expiredVersionDialog = new ExpiredVersionDialog();
			expiredVersionDialog.UpgradeRequested += (object? _, EventArgs _) =>
			{
				OpenDownloadUrl();
			};
			await DialogHost.Show(expiredVersionDialog, "MainDialogHost");
		});
	}

	private void OpenDownloadUrl()
	{
		try
		{
			Process.Start(new ProcessStartInfo
			{
				FileName = "https://github.com/zebrapixel/XFolder/releases/latest",
				UseShellExecute = true
			});
		}
		catch (Exception exception)
		{
			_logger.LogError(exception, "Failed to open download url: {DownloadUrl}", "https://github.com/zebrapixel/XFolder/releases/latest");
		}
	}
}
