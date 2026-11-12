using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Files.Data.Items;
using Files.Data.Models;

namespace Files.Data.Contracts;

public interface IRemovableDrivesService
{
	Task<DriveItem?> GetPrimaryDriveAsync(CancellationToken cancellationToken = default(CancellationToken));

	IStorageDeviceWatcher CreateWatcher();

	IAsyncEnumerable<DriveItem> GetDrivesAsync(CancellationToken cancellationToken = default(CancellationToken));

	Task UpdateDrivePropertiesAsync(DriveItem drive, CancellationToken cancellationToken = default(CancellationToken));
}
