using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Files.Data.Contracts;
using Files.Data.Items;
using Files.Data.Models;
using Files.Utils.Global;
using Files.Utils.Storage.Helpers;

namespace Files.Services.Storage;

public sealed class StorageDevicesService : IRemovableDrivesService
{
	private readonly MacStorageDeviceWatcher storageDeviceWatcher;

	public StorageDevicesService(MacStorageDeviceWatcher storageDeviceWatcher)
	{
		this.storageDeviceWatcher = storageDeviceWatcher;
	}

	public IStorageDeviceWatcher CreateWatcher()
	{
		return storageDeviceWatcher;
	}

	public async IAsyncEnumerable<DriveItem> GetDrivesAsync([EnumeratorCancellation] CancellationToken cancellationToken = default(CancellationToken))
	{
		await foreach (DriveItem item in DriveHelpers.GetMountedUsbDrivesAsync(cancellationToken))
		{
			yield return item;
		}
	}

	public async Task<DriveItem?> GetPrimaryDriveAsync(CancellationToken cancellationToken = default(CancellationToken))
	{
		DriveInfo driveInfo = new DriveInfo("/");
		string extendedDriveLabel = DriveHelpers.GetExtendedDriveLabel(driveInfo, "Macintosh HD");
		return await DriveItem.CreateFromPropertiesAsync("/", "system-drive", extendedDriveLabel, Files.Data.Items.DriveType.Fixed, driveInfo.DriveFormat, string.Empty, isUsb: false, isExternal: false, driveInfo.TotalSize, driveInfo.AvailableFreeSpace);
	}

	public Task UpdateDrivePropertiesAsync(DriveItem drive, CancellationToken cancellationToken = default(CancellationToken))
	{
		return DriveHelpers.RefreshDriveAsync(drive, cancellationToken);
	}
}
