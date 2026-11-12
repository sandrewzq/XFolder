using System;
using Files.Data.Items;

namespace Files.Data.Models;

public interface IStorageDeviceWatcher
{
	bool CanBeStarted { get; }

	event EventHandler<DriveItem>? DeviceAdded;

	event EventHandler<string>? DeviceRemoved;

	event EventHandler? EnumerationCompleted;

	event EventHandler<string>? DeviceModified;

	void Start();

	void Stop();
}
