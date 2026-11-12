using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using Files.Data.Contracts;
using Files.Data.Items;
using Microsoft.Extensions.Logging;

namespace Files.Data.Models;

public sealed class DrivesViewModel : ObservableObject, IDisposable
{
	private readonly IRemovableDrivesService removableDrivesService;

	private readonly IStorageDeviceWatcher watcher;

	private readonly ILogger<DrivesViewModel> logger;

	private int isInitialized;

	public ObservableCollection<DriveItem> Drives { get; } = new ObservableCollection<DriveItem>();

	public DrivesViewModel(IRemovableDrivesService removableDrivesService, ILogger<DrivesViewModel> logger)
	{
		this.removableDrivesService = removableDrivesService;
		this.logger = logger;
		watcher = removableDrivesService.CreateWatcher();
		watcher.DeviceAdded += Watcher_DeviceAdded;
		watcher.DeviceRemoved += Watcher_DeviceRemoved;
		watcher.DeviceModified += Watcher_DeviceModified;
		watcher.EnumerationCompleted += Watcher_EnumerationCompleted;
	}

	public async Task InitializeAsync(CancellationToken cancellationToken = default(CancellationToken))
	{
		if (Interlocked.Exchange(ref isInitialized, 1) == 0)
		{
			await UpdateDrivesAsync(cancellationToken);
			if (watcher.CanBeStarted)
			{
				watcher.Start();
			}
		}
	}

	public async Task UpdateDrivesAsync(CancellationToken cancellationToken = default(CancellationToken))
	{
		List<DriveItem> currentDrives = new List<DriveItem>();
		await foreach (DriveItem item in removableDrivesService.GetDrivesAsync(cancellationToken))
		{
			currentDrives.Add(item);
		}
		await Dispatcher.UIThread.InvokeAsync(() =>
		{
			Drives.Clear();
			foreach (DriveItem item2 in currentDrives.OrderBy((DriveItem item) => item.Text, StringComparer.CurrentCultureIgnoreCase))
			{
				Drives.Add(item2);
			}
		});
	}

	public void Dispose()
	{
		watcher.Stop();
		watcher.DeviceAdded -= Watcher_DeviceAdded;
		watcher.DeviceRemoved -= Watcher_DeviceRemoved;
		watcher.DeviceModified -= Watcher_DeviceModified;
		watcher.EnumerationCompleted -= Watcher_EnumerationCompleted;
	}

	private async void Watcher_DeviceAdded(object? sender, DriveItem drive)
	{
		try
		{
			await Dispatcher.UIThread.InvokeAsync(() =>
			{
				DriveItem? driveItem = FindDrive(drive.DeviceID, drive.Path);
				if (driveItem != null)
				{
					Drives.Remove(driveItem);
				}
				Drives.Add(drive);
			});
		}
		catch (Exception ex)
		{
			logger.LogWarning(ex, "Failed to add storage device {DeviceId}.", drive.DeviceID);
		}
	}

	private async void Watcher_DeviceRemoved(object? sender, string deviceIdOrPath)
	{
		try
		{
			await Dispatcher.UIThread.InvokeAsync(() =>
			{
				DriveItem? removedDrive = FindDrive(deviceIdOrPath, deviceIdOrPath);
				if (removedDrive != null)
				{
					Drives.Remove(removedDrive);
				}
			});
		}
		catch (Exception ex)
		{
			logger.LogWarning(ex, "Failed to remove storage device {DeviceIdOrPath}.", deviceIdOrPath);
		}
	}

	private async void Watcher_DeviceModified(object? sender, string devicePath)
	{
		try
		{
			DriveItem? driveItem = null;
			await Dispatcher.UIThread.InvokeAsync(() =>
			{
				driveItem = FindDrive(devicePath, devicePath);
			});
			if (driveItem != null)
			{
				await removableDrivesService.UpdateDrivePropertiesAsync(driveItem);
			}
		}
		catch (Exception ex)
		{
			logger.LogWarning(ex, "Failed to update storage device {DevicePath}.", devicePath);
		}
	}

	private DriveItem? FindDrive(string? deviceId, string? path)
	{
		return Drives.FirstOrDefault(item =>
			string.Equals(item.DeviceID, deviceId, StringComparison.Ordinal) ||
			string.Equals(item.Path, path, StringComparison.Ordinal));
	}

	private void Watcher_EnumerationCompleted(object? sender, EventArgs e)
	{
	}

	private void PrintDriveMessage(string action, string name, string path, string deviceId, string filesystem, string busProtocol)
	{
		logger.LogInformation("{Action}: Name={Name}, Path={Path}, DeviceId={DeviceId}, Filesystem={Filesystem}, BusProtocol={BusProtocol}", action, name, path, deviceId, filesystem, busProtocol);
		InlineArray6<object> buffer = default;
		buffer[0] = action;
		buffer[1] = name;
		buffer[2] = path;
		buffer[3] = deviceId;
		buffer[4] = filesystem;
		buffer[5] = busProtocol;
		Console.WriteLine("[USB] {0}: Name={1}, Path={2}, DeviceId={3}, Filesystem={4}, BusProtocol={5}", (ReadOnlySpan<object?>)buffer);
	}
}
