using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia.Threading;
using Microsoft.Extensions.Logging;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel.__Internals;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Files.Data.Commands.Manager;
using Files.Data.Contracts;
using Files.Data.Enums;
using Files.Data.Items;
using Files.Data.Messages;
using Files.Data.Models;
using Files.Data.Parameters;
using Files.Extensions;
using Files.Helpers;
using Files.Helpers.Shared;
using Files.Helpers.UI;
using Files.ViewModels.Items;
using Files.Views.Layouts;

namespace Files.ViewModels.Layouts;

public class BaseLayoutViewModel : ViewModelBase, IDisposable, IRecipient<FileSystemChangedMessage>
{
	private FileSystemWatcher? watcher;

	private CancellationTokenSource? _loadItemsCancellation;

	private int _loadItemsVersion;

	private bool _isLoadingItems;

	private readonly object _watcherEventsLock = new object();

	private readonly List<(WatcherChangeTypes ChangeType, string Path, string? OldPath, string? Name)> _pendingWatcherEvents = new List<(WatcherChangeTypes, string, string?, string?)>();

	private bool _watcherFlushScheduled;

	[ObservableProperty]
	private NavigationParams _navParams;

	[ObservableProperty]
	private ObservableCollection<ListedItem> _items;

	private readonly IFoldersSettingsService _foldersSettingsService = Ioc.Default.GetRequiredService<IFoldersSettingsService>();

	public TabItemViewModel? AssociatedInstance;

	public ItemManipulationModel _itemManipulationModel = new ItemManipulationModel();

	public BaseLayoutView? LayoutView { get; set; }

	protected ICommandManager Commands { get; } = Ioc.Default.GetRequiredService<ICommandManager>();

	public bool IsDisposed { get; private set; }

	public SortOption CurrentSortOption => AssociatedInstance?.CurrentSortOption ?? SortOption.Name;

	public SortDirection CurrentSortDirection => AssociatedInstance?.CurrentSortDirection ?? SortDirection.Ascending;

	public ICommand CreateNewFileCommand { get; private set; }

	public ICommand ItemPointerPressedCommand { get; private set; }

	public ICommand PointerWheelChangedCommand { get; private set; }

	public ICommand SortItemsCommand { get; private set; }

	public ICommand SortDirectionCommand { get; private set; }

	public List<ListedItem>? PendingSelectedItems { get; set; }

	[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
	[ExcludeFromCodeCoverage]
	public NavigationParams NavParams
	{
		get
		{
			return _navParams;
		}
		[MemberNotNull("_navParams")]
		set
		{
			if (!EqualityComparer<NavigationParams>.Default.Equals(_navParams, value))
			{
				OnPropertyChanging(__KnownINotifyPropertyChangingArgs.NavParams);
				_navParams = value;
				OnPropertyChanged(__KnownINotifyPropertyChangedArgs.NavParams);
			}
		}
	}

	[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
	[ExcludeFromCodeCoverage]
	public ObservableCollection<ListedItem> Items
	{
		get
		{
			return _items;
		}
		[MemberNotNull("_items")]
		set
		{
			if (!EqualityComparer<ObservableCollection<ListedItem>>.Default.Equals(_items, value))
			{
				OnItemsChanging(value);
				OnPropertyChanging(__KnownINotifyPropertyChangingArgs.Items);
				_items = value;
				OnItemsChanged(value);
				OnPropertyChanged(__KnownINotifyPropertyChangedArgs.Items);
			}
		}
	}

	private void Items_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
	{
		UpdateHasItem();
	}

	private void UpdateHasItem()
	{
		if (AssociatedInstance != null)
		{
			AssociatedInstance.HasItem = Items != null && Items.Any();
			AssociatedInstance.ItemCount = Items?.Count ?? 0;
		}
	}

	public BaseLayoutViewModel(string navPath, TabItemViewModel tabItemViewModel, IEnumerable<string>? selectItems = null)
	{
		SortItemsCommand = new RelayCommand<SortOption>(SortItems);
		SortDirectionCommand = new RelayCommand<SortDirection>(SetSortDirection);
		AssociatedInstance = tabItemViewModel;
		if (AssociatedInstance != null)
		{
			AssociatedInstance.LayoutViewModel = this;
		}
		NavParams = new NavigationParams
		{
			NavPath = navPath
		};
		Items = new ObservableCollection<ListedItem>();
		RefreshItemsOpacityByClipboardCutStatus();
		WatchForFolderChanges(navPath);
		WeakReferenceMessenger.Default.Register(this);
		_foldersSettingsService.PropertyChanged += FoldersSettingsService_PropertyChanged;
		BeginLoadItems(navPath);
	}

	public void Dispose()
	{
		if (!IsDisposed)
		{
			IsDisposed = true;
			Interlocked.Increment(ref _loadItemsVersion);
			_loadItemsCancellation?.Cancel();
			_loadItemsCancellation?.Dispose();
			_loadItemsCancellation = null;
			CloseWatcher();
			LayoutView = null;
			WeakReferenceMessenger.Default.Unregister<FileSystemChangedMessage>(this);
			_foldersSettingsService.PropertyChanged -= FoldersSettingsService_PropertyChanged;
		}
	}

	public void OnViewDetached(BaseLayoutView view)
	{
		if (LayoutView == view)
		{
			LayoutView = null;
		}
	}

	private void FoldersSettingsService_PropertyChanged(object? sender, PropertyChangedEventArgs e)
	{
		if (!(e.PropertyName == "ShowHiddenItems") && !(e.PropertyName == "ShowFileExtensions") && !(e.PropertyName == "ShowThumbnails"))
		{
			return;
		}
		Dispatcher.UIThread.Post(() =>
		{
			if (NavParams?.NavPath != null)
			{
				RefreshItems(NavParams.NavPath);
			}
		});
	}

	public void Receive(FileSystemChangedMessage message)
	{
		if (message.ChangeType != FileSystemChangeType.Renamed || message.OldPath == null)
		{
			return;
		}
		Dispatcher.UIThread.Post(() =>
		{
			if (Items != null && NavParams?.NavPath != null)
			{
				string? directoryName = Path.GetDirectoryName(message.Path);
				string text = NavParams.NavPath.TrimEnd(Path.DirectorySeparatorChar);
				bool flag = directoryName?.TrimEnd(Path.DirectorySeparatorChar) == text;
				for (int i = 0; i < Items.Count; i++)
				{
					ListedItem listedItem = Items[i];
					if (listedItem.ItemPath == message.OldPath)
					{
						if (flag)
						{
							listedItem.ItemPath = message.Path;
							listedItem.ItemNameRaw = Path.GetFileName(message.Path);
						}
						else
						{
							Items.RemoveAt(i);
						}
						break;
					}
				}
			}
		});
	}

	public void RefreshItems(string newPath)
	{
		if (AssociatedInstance?.SelectedItems != null && AssociatedInstance.SelectedItems.Count > 0)
		{
			PendingSelectedItems = AssociatedInstance.SelectedItems.ToList();
		}
		CloseWatcher();
		NavParams = new NavigationParams
		{
			NavPath = newPath
		};
		_isLoadingItems = true;
		Items = new ObservableCollection<ListedItem>();
		RefreshItemsOpacityByClipboardCutStatus();
		WatchForFolderChanges(newPath);
		BeginLoadItems(newPath);
	}

	private void BeginLoadItems(string path)
	{
		CancellationTokenSource cancellation = new CancellationTokenSource();
		CancellationTokenSource? previous = Interlocked.Exchange(ref _loadItemsCancellation, cancellation);
		previous?.Cancel();
		previous?.Dispose();
		_isLoadingItems = true;
		int version = Interlocked.Increment(ref _loadItemsVersion);
		bool showHiddenItems = _foldersSettingsService.ShowHiddenItems;
		Task.Run(delegate
		{
			try
			{
				ObservableCollection<ListedItem> loadedItems = LoadFileItemChildren(path, showHiddenItems, cancellation.Token);
				if (cancellation.IsCancellationRequested)
				{
					return;
				}
				Dispatcher.UIThread.Post(delegate
				{
					if (!cancellation.IsCancellationRequested && version == Volatile.Read(ref _loadItemsVersion) && !IsDisposed)
					{
						_isLoadingItems = false;
						Items = loadedItems;
						RefreshItemsOpacityByClipboardCutStatus();
					}
				});
			}
			catch (OperationCanceledException)
			{
			}
			catch (Exception exception)
			{
				Ioc.Default.GetService<ILogger<BaseLayoutViewModel>>()?.LogError(exception, "Failed to load directory '{Path}'", path);
				Dispatcher.UIThread.Post(delegate
				{
					if (version == Volatile.Read(ref _loadItemsVersion) && !IsDisposed)
					{
						_isLoadingItems = false;
					}
				});
			}
		}, cancellation.Token);
	}

	public void UpdateSortState(SortOption sortOption, SortDirection sortDirection)
	{
		if (AssociatedInstance != null)
		{
			AssociatedInstance.CurrentSortOption = sortOption;
			AssociatedInstance.CurrentSortDirection = sortDirection;
			AssociatedInstance.HasCustomSort = true;
			Items = new ObservableCollection<ListedItem>(ApplyCurrentSort(Items));
		}
	}

	private void SortItems(SortOption sortOption)
	{
		UpdateSortState(sortOption, CurrentSortDirection);
	}

	private void SetSortDirection(SortDirection sortDirection)
	{
		UpdateSortState(CurrentSortOption, sortDirection);
	}

	public static SortOption GetSortOption(string? memberPath)
	{
		return memberPath switch
		{
			"FileSizeBytes" => SortOption.Size, 
			"DisplayItemType" => SortOption.FileType, 
			"ItemDateModified" => SortOption.DateModified, 
			"ItemDateCreated" => SortOption.DateCreated, 
			_ => SortOption.Name, 
		};
	}

	public IEnumerable<ListedItem> ApplyCurrentSort(IEnumerable<ListedItem>? items)
	{
		if (items == null)
		{
			return Enumerable.Empty<ListedItem>();
		}
		TabItemViewModel? associatedInstance = AssociatedInstance;
		if (associatedInstance == null || !associatedInstance.HasCustomSort)
		{
			return items.OrderBy((ListedItem i) => (!i.IsFolder) ? 1 : 0).ThenBy((ListedItem i) => i.Name, StringComparer.CurrentCultureIgnoreCase);
		}
		return ApplySort(items, CurrentSortOption, CurrentSortDirection);
	}

	private static IOrderedEnumerable<ListedItem> ApplySort(IEnumerable<ListedItem> items, SortOption sortOption, SortDirection sortDirection)
	{
		bool flag = sortDirection == SortDirection.Ascending;
		return sortOption switch
		{
			SortOption.Size => flag ? (from i in items
				orderby (!i.IsFolder) ? 1 : 0, (!i.IsFolder) ? i.FileSizeBytes : 0
				select i).ThenBy((ListedItem i) => i.Name, StringComparer.CurrentCultureIgnoreCase) : (from i in items
				orderby i.IsFolder ? 1 : 0, (!i.IsFolder) ? i.FileSizeBytes : 0 descending
				select i).ThenBy((ListedItem i) => i.Name, StringComparer.CurrentCultureIgnoreCase), 
			SortOption.FileType => flag ? items.OrderBy((ListedItem i) => i.DisplayItemType, StringComparer.CurrentCultureIgnoreCase).ThenBy((ListedItem i) => i.Name, StringComparer.CurrentCultureIgnoreCase) : items.OrderByDescending((ListedItem i) => i.DisplayItemType, StringComparer.CurrentCultureIgnoreCase).ThenBy((ListedItem i) => i.Name, StringComparer.CurrentCultureIgnoreCase), 
			SortOption.DateModified => flag ? items.OrderBy((ListedItem i) => i.ItemDateModifiedReal).ThenBy((ListedItem i) => i.Name, StringComparer.CurrentCultureIgnoreCase) : items.OrderByDescending((ListedItem i) => i.ItemDateModifiedReal).ThenBy((ListedItem i) => i.Name, StringComparer.CurrentCultureIgnoreCase), 
			SortOption.DateCreated => flag ? items.OrderBy((ListedItem i) => i.ItemDateCreatedReal).ThenBy((ListedItem i) => i.Name, StringComparer.CurrentCultureIgnoreCase) : items.OrderByDescending((ListedItem i) => i.ItemDateCreatedReal).ThenBy((ListedItem i) => i.Name, StringComparer.CurrentCultureIgnoreCase), 
			_ => flag ? items.OrderBy((ListedItem i) => i.Name, StringComparer.CurrentCultureIgnoreCase) : items.OrderByDescending((ListedItem i) => i.Name, StringComparer.CurrentCultureIgnoreCase), 
		};
	}

	public void RefreshItemsOpacityByClipboardCutStatus()
	{
		ClipboardCutOpacityHelper.RefreshByLayout(this);
	}

	protected void WatchForFolderChanges(string? folderPath)
	{
		if (Directory.Exists(folderPath))
		{
			SafetyExtensions.IgnoreExceptions(() =>
			{
				watcher = new FileSystemWatcher
				{
					Path = folderPath,
					Filter = "*",
					NotifyFilter = (NotifyFilters.FileName | NotifyFilters.DirectoryName | NotifyFilters.LastWrite)
				};
				watcher.Created += DirectoryWatcher_OnCreated;
				watcher.Deleted += DirectoryWatcher_OnDeleted;
				watcher.Renamed += DirectoryWatcher_OnRenamed;
				watcher.Error += DirectoryWatcher_OnError;
				watcher.EnableRaisingEvents = true;
			});
		}
	}

	private void DirectoryWatcher_OnCreated(object sender, FileSystemEventArgs e)
	{
		EnqueueWatcherEvent(WatcherChangeTypes.Created, e.FullPath, null, null);
	}

	private void DirectoryWatcher_OnDeleted(object sender, FileSystemEventArgs e)
	{
		EnqueueWatcherEvent(WatcherChangeTypes.Deleted, e.FullPath, null, null);
	}

	private void DirectoryWatcher_OnRenamed(object sender, RenamedEventArgs e)
	{
		EnqueueWatcherEvent(WatcherChangeTypes.Renamed, e.FullPath, e.OldFullPath, e.Name);
	}

	private void DirectoryWatcher_OnError(object sender, ErrorEventArgs e)
	{
		// The watcher buffer may have overflowed or the watched folder became
		// unavailable. Reload the directory and recreate the watcher.
		if (IsDisposed)
		{
			return;
		}
		Dispatcher.UIThread.Post(delegate
		{
			if (!IsDisposed && NavParams?.NavPath != null)
			{
				Ioc.Default.GetService<ILogger<BaseLayoutViewModel>>()?.LogWarning(e.GetException(), "File system watcher failed for '{Path}', reloading", NavParams.NavPath);
				RefreshItems(NavParams.NavPath);
			}
		});
	}

	private void EnqueueWatcherEvent(WatcherChangeTypes changeType, string path, string? oldPath, string? name)
	{
		bool scheduleFlush = false;
		lock (_watcherEventsLock)
		{
			if (IsDisposed)
			{
				return;
			}
			_pendingWatcherEvents.Add((changeType, path, oldPath, name));
			if (!_watcherFlushScheduled)
			{
				_watcherFlushScheduled = true;
				scheduleFlush = true;
			}
		}
		if (scheduleFlush)
		{
			// The watcher callbacks run on a pool thread. Create the timer on the
			// UI dispatcher explicitly, otherwise it would be attached to the
			// calling thread's dispatcher and never fire.
			if (Dispatcher.UIThread.CheckAccess())
			{
				DispatcherTimer.RunOnce(FlushWatcherEvents, TimeSpan.FromMilliseconds(150.0), DispatcherPriority.Background);
			}
			else
			{
				Dispatcher.UIThread.Post(delegate
				{
					DispatcherTimer.RunOnce(FlushWatcherEvents, TimeSpan.FromMilliseconds(150.0), DispatcherPriority.Background);
				});
			}
		}
	}

	private void ClearPendingWatcherEvents()
	{
		lock (_watcherEventsLock)
		{
			_pendingWatcherEvents.Clear();
			_watcherFlushScheduled = false;
		}
	}

	private void FlushWatcherEvents()
	{
		List<(WatcherChangeTypes ChangeType, string Path, string? OldPath, string? Name)> events;
		lock (_watcherEventsLock)
		{
			events = new List<(WatcherChangeTypes, string, string?, string?)>(_pendingWatcherEvents);
			_pendingWatcherEvents.Clear();
			_watcherFlushScheduled = false;
		}
		if (events.Count == 0 || IsDisposed || Items == null)
		{
			return;
		}
		string? watchedPath = watcher?.Path;
		if (string.IsNullOrEmpty(watchedPath))
		{
			return;
		}
		Dictionary<string, ListedItem> itemsByPath = new Dictionary<string, ListedItem>(Items.Count, StringComparer.Ordinal);
		foreach (ListedItem listedItem in Items)
		{
			itemsByPath[listedItem.ItemPath] = listedItem;
		}
		bool itemsChanged = false;
		foreach (var pendingEvent in events)
		{
			switch (pendingEvent.ChangeType)
			{
			case WatcherChangeTypes.Created:
			{
				ListedItem? listedItem2 = Path2ListedItem(pendingEvent.Path);
				if (listedItem2 == null)
				{
					break;
				}
				if (itemsByPath.TryGetValue(pendingEvent.Path, out ListedItem? value2))
				{
					int index = Items.IndexOf(value2);
					if (index >= 0)
					{
						Items[index] = listedItem2;
					}
				}
				else
				{
					InsertItemSorted(listedItem2);
				}
				itemsByPath[pendingEvent.Path] = listedItem2;
				itemsChanged = true;
				break;
			}
			case WatcherChangeTypes.Deleted:
			{
				if (itemsByPath.TryGetValue(pendingEvent.Path, out ListedItem? value))
				{
					Items.Remove(value);
					itemsByPath.Remove(pendingEvent.Path);
					itemsChanged = true;
				}
				break;
			}
			case WatcherChangeTypes.Renamed:
			{
				if (pendingEvent.OldPath == null || !itemsByPath.TryGetValue(pendingEvent.OldPath, out ListedItem? value3))
				{
					break;
				}
				string? directoryName = Path.GetDirectoryName(pendingEvent.Path);
				string text = watchedPath.TrimEnd(Path.DirectorySeparatorChar);
				bool flag = directoryName?.TrimEnd(Path.DirectorySeparatorChar) == text;
				itemsByPath.Remove(pendingEvent.OldPath);
				if (flag)
				{
					value3.ItemPath = pendingEvent.Path;
					value3.ItemNameRaw = pendingEvent.Name ?? string.Empty;
					itemsByPath[pendingEvent.Path] = value3;
				}
				else
				{
					Items.Remove(value3);
				}
				itemsChanged = true;
				break;
			}
			}
		}
		if (itemsChanged)
		{
			RefreshItemsOpacityByClipboardCutStatus();
		}
	}

	private void InsertItemSorted(ListedItem item)
	{
		TabItemViewModel? associatedInstance = AssociatedInstance;
		bool customSort = associatedInstance != null && associatedInstance.HasCustomSort;
		SortOption sortOption = CurrentSortOption;
		bool ascending = CurrentSortDirection == SortDirection.Ascending;
		int num = 0;
		int num2 = Items.Count;
		while (num < num2)
		{
			int num3 = num + (num2 - num) / 2;
			if (CompareItems(Items[num3], item, customSort, sortOption, ascending) <= 0)
			{
				num = num3 + 1;
			}
			else
			{
				num2 = num3;
			}
		}
		Items.Insert(num, item);
	}

	private static int CompareItems(ListedItem x, ListedItem y, bool customSort, SortOption sortOption, bool ascending)
	{
		int num = (x.IsFolder ? 0 : 1).CompareTo(y.IsFolder ? 0 : 1);
		int num2;
		if (!customSort)
		{
			return (num != 0) ? num : StringComparer.CurrentCultureIgnoreCase.Compare(x.Name, y.Name);
		}
		switch (sortOption)
		{
		case SortOption.Size:
			if (num != 0)
			{
				return num;
			}
			num2 = (x.IsFolder ? 0 : x.FileSizeBytes).CompareTo(y.IsFolder ? 0 : y.FileSizeBytes);
			break;
		case SortOption.FileType:
			num2 = StringComparer.CurrentCultureIgnoreCase.Compare(x.DisplayItemType, y.DisplayItemType);
			break;
		case SortOption.DateModified:
			num2 = x.ItemDateModifiedReal.CompareTo(y.ItemDateModifiedReal);
			break;
		case SortOption.DateCreated:
			num2 = x.ItemDateCreatedReal.CompareTo(y.ItemDateCreatedReal);
			break;
		default:
			num2 = StringComparer.CurrentCultureIgnoreCase.Compare(x.Name, y.Name);
			break;
		}
		if (!ascending)
		{
			num2 = -num2;
		}
		if (num2 == 0)
		{
			num2 = StringComparer.CurrentCultureIgnoreCase.Compare(x.Name, y.Name);
		}
		return num2;
	}

	public void CloseWatcher()
	{
		ClearPendingWatcherEvents();
		watcher?.Dispose();
		watcher = null;
	}

	protected ObservableCollection<ListedItem> LoadFileItemChildren(string path, bool showHiddenItems, CancellationToken cancellationToken)
	{
		EnumerationOptions enumerationOptions = new EnumerationOptions
		{
			IgnoreInaccessible = true,
			AttributesToSkip = (showHiddenItems ? FileAttributes.System : (FileAttributes.Hidden | FileAttributes.System))
		};
		ObservableCollection<ListedItem> observableCollection = new ObservableCollection<ListedItem>();
		if (Directory.Exists(path))
		{
			foreach (string item3 in Directory.EnumerateDirectories(path, "*", enumerationOptions))
			{
				cancellationToken.ThrowIfCancellationRequested();
				ListedItem? item = DirPath2ListedItem(item3);
				if (item != null)
				{
					observableCollection.Add(item);
				}
			}
			foreach (string item4 in Directory.EnumerateFiles(path, "*", enumerationOptions))
			{
				cancellationToken.ThrowIfCancellationRequested();
				ListedItem? item2 = FilePath2ListedItem(item4);
				if (item2 != null)
				{
					observableCollection.Add(item2);
				}
			}
		}
		cancellationToken.ThrowIfCancellationRequested();
		return new ObservableCollection<ListedItem>(ApplyCurrentSort(observableCollection).ToList());
	}

	private static ListedItem? Path2ListedItem(string path)
	{
		try
		{
			if (Directory.Exists(path))
			{
				return DirPath2ListedItem(path);
			}
			if (File.Exists(path))
			{
				return FilePath2ListedItem(path);
			}
			return null;
		}
		catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
		{
			return null;
		}
	}

	private static ListedItem? FilePath2ListedItem(string f)
	{
		try
		{
			FileInfo fileInfo = new FileInfo(f);
			return new ListedItem
			{
				ItemPath = f,
				ItemNameRaw = fileInfo.Name,
				FileExtension = fileInfo.Extension,
				FileSizeBytes = fileInfo.Length,
				FileSizeDisplay = StringsHelper.FormatFileSize(fileInfo.Length),
				ItemDateCreatedReal = fileInfo.CreationTimeUtc,
				ItemDateModifiedReal = fileInfo.LastWriteTimeUtc,
				ItemDateAccessedReal = fileInfo.LastAccessTimeUtc,
				IsHiddenItem = PathHelpers.IsHiddenFile(fileInfo)
			};
		}
		catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
		{
			return null;
		}
	}

	private static ListedItem? DirPath2ListedItem(string d)
	{
		try
		{
			DirectoryInfo directoryInfo = new DirectoryInfo(d);
			return new ListedItem
			{
				ItemPath = d,
				ItemNameRaw = directoryInfo.Name,
				FileExtension = string.Empty,
				FileSizeBytes = 0L,
				FileSizeDisplay = string.Empty,
				ItemDateCreatedReal = directoryInfo.CreationTimeUtc,
				ItemDateModifiedReal = directoryInfo.LastWriteTimeUtc,
				ItemDateAccessedReal = directoryInfo.LastAccessTimeUtc,
				IsHiddenItem = PathHelpers.IsHiddenDirectory(directoryInfo)
			};
		}
		catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
		{
			return null;
		}
	}

	[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
	private void OnItemsChanging(ObservableCollection<ListedItem> value)
	{
		if (Items != null)
		{
			Items.CollectionChanged -= Items_CollectionChanged;
		}
	}

	[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
	private void OnItemsChanged(ObservableCollection<ListedItem> value)
	{
		if (value != null)
		{
			value.CollectionChanged += Items_CollectionChanged;
			UpdateHasItem();
			if (!_isLoadingItems)
			{
				LayoutView?.RequestItemsPresentedNotification();
			}
		}
	}
}
