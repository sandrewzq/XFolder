using System;
using Files.Data.Items;
using Files.Data.Models;
using Files.Utils.Storage.Helpers;

namespace Files.Utils.Global;

public sealed class MacStorageDeviceWatcher : IStorageDeviceWatcher
{
	private readonly DeviceManager deviceManager;

	public bool CanBeStarted => deviceManager.CanBeStarted;

	public event EventHandler<DriveItem>? DeviceAdded;

	public event EventHandler<string>? DeviceRemoved;

	public event EventHandler? EnumerationCompleted;

	public event EventHandler<string>? DeviceModified;

	public MacStorageDeviceWatcher(DeviceManager deviceManager)
	{
		this.deviceManager = deviceManager;
		this.deviceManager.DeviceAdded += DeviceManager_DeviceAdded;
		this.deviceManager.DeviceRemoved += DeviceManager_DeviceRemoved;
		this.deviceManager.DeviceInserted += DeviceManager_DeviceInsertedOrEjected;
		this.deviceManager.DeviceEjected += DeviceManager_DeviceInsertedOrEjected;
	}

	public void Start()
	{
		deviceManager.Start();
		EnumerationCompleted?.Invoke(this, EventArgs.Empty);
	}

	public void Stop()
	{
		deviceManager.Stop();
	}

	private void DeviceManager_DeviceAdded(object? sender, DeviceManager.DeviceEventArgs e)
	{
		if (e.Drive != null)
		{
			DeviceAdded?.Invoke(this, e.Drive);
		}
	}

	private void DeviceManager_DeviceRemoved(object? sender, DeviceManager.DeviceEventArgs e)
	{
		DeviceRemoved?.Invoke(this, e.DeviceId);
	}

	private void DeviceManager_DeviceInsertedOrEjected(object? sender, DeviceManager.DeviceEventArgs e)
	{
		DeviceModified?.Invoke(this, e.DevicePath);
	}
}
