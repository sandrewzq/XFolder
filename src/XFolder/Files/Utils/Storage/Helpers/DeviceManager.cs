using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Files.Data.Items;

namespace Files.Utils.Storage.Helpers;

public sealed class DeviceManager : IDisposable
{
	public sealed class DeviceEventArgs : EventArgs
	{
		public string DeviceName { get; }

		public string DeviceId { get; }

		public string DevicePath { get; }

		public DriveItem? Drive { get; }

		public DeviceEventArgs(string deviceName, string deviceId, string devicePath, DriveItem? drive)
		{
			DeviceName = deviceName;
			DeviceId = deviceId;
			DevicePath = devicePath;
			Drive = drive;
		}
	}

	private sealed class DeviceSnapshot
	{
		public string Path { get; }

		public string Name { get; private set; }

		public string DeviceId { get; private set; }

		public DriveItem? Drive { get; private set; }

		public bool IsSupported { get; private set; }

		public DeviceSnapshot(string path, string name, string deviceId)
		{
			Path = path;
			Name = name;
			DeviceId = deviceId;
		}

		public void Update(DriveItem drive)
		{
			Drive = drive;
			Name = drive.Text;
			DeviceId = drive.DeviceID;
			IsSupported = true;
		}

		public DeviceEventArgs ToEventArgs()
		{
			return new DeviceEventArgs(Name, DeviceId, Path, Drive);
		}
	}

	private readonly object syncRoot = new object();

	private readonly Dictionary<string, DeviceSnapshot> knownVolumes = new Dictionary<string, DeviceSnapshot>(StringComparer.Ordinal);

	private FileSystemWatcher? volumeWatcher;

	private PeriodicTimer? periodicTimer;

	private CancellationTokenSource? periodicScanCancellation;

	private Task? periodicScanTask;

	public bool CanBeStarted
	{
		get
		{
			if (DriveHelpers.IsSupportedPlatform)
			{
				return Directory.Exists("/Volumes");
			}
			return false;
		}
	}

	public event EventHandler<DeviceEventArgs>? DeviceAdded;

	public event EventHandler<DeviceEventArgs>? DeviceRemoved;

	public event EventHandler<DeviceEventArgs>? DeviceInserted;

	public event EventHandler<DeviceEventArgs>? DeviceEjected;

	public void Start()
	{
		if (!CanBeStarted)
		{
			return;
		}
		lock (syncRoot)
		{
			if (volumeWatcher == null)
			{
				PrimeKnownVolumes();
				volumeWatcher = new FileSystemWatcher
				{
					Path = "/Volumes",
					Filter = "*",
					IncludeSubdirectories = false,
					NotifyFilter = (NotifyFilters.DirectoryName | NotifyFilters.CreationTime)
				};
				volumeWatcher.Created += VolumeWatcher_Created;
				volumeWatcher.Deleted += VolumeWatcher_Deleted;
				volumeWatcher.Renamed += VolumeWatcher_Renamed;
				volumeWatcher.EnableRaisingEvents = true;
				periodicScanCancellation = new CancellationTokenSource();
				periodicTimer = new PeriodicTimer(TimeSpan.FromSeconds(3L));
				periodicScanTask = Task.Run(() => RunPeriodicScanAsync(periodicTimer, periodicScanCancellation.Token));
			}
		}
	}

	public void Stop()
	{
		FileSystemWatcher fileSystemWatcher;
		CancellationTokenSource cancellationTokenSource;
		PeriodicTimer periodicTimer;
		Task task;
		lock (syncRoot)
		{
			fileSystemWatcher = volumeWatcher;
			cancellationTokenSource = periodicScanCancellation;
			periodicTimer = this.periodicTimer;
			task = periodicScanTask;
			volumeWatcher = null;
			periodicScanCancellation = null;
			this.periodicTimer = null;
			periodicScanTask = null;
			knownVolumes.Clear();
		}
		if (fileSystemWatcher != null)
		{
			fileSystemWatcher.Created -= VolumeWatcher_Created;
			fileSystemWatcher.Deleted -= VolumeWatcher_Deleted;
			fileSystemWatcher.Renamed -= VolumeWatcher_Renamed;
			fileSystemWatcher.Dispose();
		}
		cancellationTokenSource?.Cancel();
		periodicTimer?.Dispose();
		if (task != null)
		{
			try
			{
				task.GetAwaiter().GetResult();
			}
			catch (OperationCanceledException)
			{
			}
		}
		cancellationTokenSource?.Dispose();
	}

	public void Dispose()
	{
		Stop();
		GC.SuppressFinalize(this);
	}

	private void PrimeKnownVolumes()
	{
		foreach (string volumePath in DriveHelpers.GetMountedVolumePaths().Distinct(StringComparer.Ordinal))
		{
			DeviceSnapshot deviceSnapshot = CreateSnapshot(volumePath);
			DriveItem result = Task.Run(() => DriveHelpers.TryCreateDriveItemAsync(volumePath)).GetAwaiter().GetResult();
			if (result != null)
			{
				deviceSnapshot.Update(result);
			}
			knownVolumes[deviceSnapshot.Path] = deviceSnapshot;
		}
	}

	private async Task RunPeriodicScanAsync(PeriodicTimer timer, CancellationToken cancellationToken)
	{
		_ = 1;
		try
		{
			while (await timer.WaitForNextTickAsync(cancellationToken))
			{
				await ScanVolumesAsync(cancellationToken);
			}
		}
		catch (OperationCanceledException)
		{
		}
	}

	private async Task ScanVolumesAsync(CancellationToken cancellationToken)
	{
		HashSet<string> currentVolumePaths = DriveHelpers.GetMountedVolumePaths().Distinct(StringComparer.Ordinal).ToHashSet(StringComparer.Ordinal);
		List<string> list;
		List<DeviceSnapshot> list2;
		lock (syncRoot)
		{
			List<string> source = knownVolumes.Keys.ToList();
			list = currentVolumePaths.Where((string path) => !knownVolumes.ContainsKey(path)).ToList();
			list2 = source.Where((string path) => !currentVolumePaths.Contains(path)).Select((string path) =>
			{
				DeviceSnapshot result = knownVolumes[path];
				knownVolumes.Remove(path);
				return result;
			}).ToList();
			foreach (string item in list)
			{
				knownVolumes[item] = CreateSnapshot(item);
			}
		}
		foreach (DeviceSnapshot item2 in list2)
		{
			RaiseRemovalEvents(item2);
		}
		foreach (string item3 in list)
		{
			cancellationToken.ThrowIfCancellationRequested();
			await TryRegisterAddedVolumeAsync(item3, cancellationToken);
		}
	}

	private void VolumeWatcher_Created(object sender, FileSystemEventArgs e)
	{
		TryRegisterAddedVolumeAsync(e.FullPath, CancellationToken.None);
	}

	private void VolumeWatcher_Deleted(object sender, FileSystemEventArgs e)
	{
		RemoveVolume(e.FullPath);
	}

	private void VolumeWatcher_Renamed(object sender, RenamedEventArgs e)
	{
		RemoveVolume(e.OldFullPath);
		TryRegisterAddedVolumeAsync(e.FullPath, CancellationToken.None);
	}

	private async Task TryRegisterAddedVolumeAsync(string volumePath, CancellationToken cancellationToken)
	{
		string normalizedPath = DriveHelpers.NormalizeVolumePath(volumePath);
		if (!DriveHelpers.IsCandidateVolumePath(normalizedPath))
		{
			return;
		}
		lock (syncRoot)
		{
			if (!knownVolumes.ContainsKey(normalizedPath))
			{
				knownVolumes[normalizedPath] = CreateSnapshot(normalizedPath);
			}
		}
		for (int attempt = 0; attempt < 10; attempt++)
		{
			cancellationToken.ThrowIfCancellationRequested();
			DriveItem driveItem = await DriveHelpers.TryCreateDriveItemAsync(normalizedPath, cancellationToken);
			if (driveItem != null)
			{
				bool flag = false;
				DeviceSnapshot value;
				lock (syncRoot)
				{
					if (!knownVolumes.TryGetValue(normalizedPath, out value))
					{
						break;
					}
					flag = !value.IsSupported;
					value.Update(driveItem);
				}
				if (flag)
				{
					DeviceEventArgs e = value.ToEventArgs();
					DeviceAdded?.Invoke(this, e);
					DeviceInserted?.Invoke(this, e);
				}
				break;
			}
			await Task.Delay(200, cancellationToken);
		}
	}

	private void RemoveVolume(string volumePath)
	{
		string text = DriveHelpers.NormalizeVolumePath(volumePath);
		if (string.IsNullOrWhiteSpace(text))
		{
			return;
		}
		DeviceSnapshot value = null;
		lock (syncRoot)
		{
			if (knownVolumes.TryGetValue(text, out value))
			{
				knownVolumes.Remove(text);
			}
		}
		if (value != null)
		{
			RaiseRemovalEvents(value);
		}
	}

	private void RaiseRemovalEvents(DeviceSnapshot snapshot)
	{
		if (snapshot.IsSupported)
		{
			DeviceEventArgs e = snapshot.ToEventArgs();
			DeviceRemoved?.Invoke(this, e);
			DeviceEjected?.Invoke(this, e);
		}
	}

	private static DeviceSnapshot CreateSnapshot(string volumePath)
	{
		string text = DriveHelpers.NormalizeVolumePath(volumePath);
		string fileName = Path.GetFileName(text);
		return new DeviceSnapshot(text, string.IsNullOrWhiteSpace(fileName) ? text : fileName, text);
	}
}
