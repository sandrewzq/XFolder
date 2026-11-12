using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel.__Internals;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Files.Data.Commands;
using Files.Data.Commands.Manager;
using Files.Data.Contracts;
using Files.Data.Enums;
using Files.Data.Items;
using Files.Data.Messages;
using Files.Data.Parameters;
using Files.Helpers;
using Files.Helpers.Shared;
using Files.ViewModels.Layouts;
using Files.ViewModels.Shells;
using Files.Views.Layouts;
using SkiaSharp;

namespace Files.ViewModels.Items;

public class TabItemViewModel : ViewModelBase, IDisposable, IRecipient<LayoutViewSelectedItemsChanged>
{
	private static readonly ICommandManager CommandManager = Ioc.Default.GetRequiredService<ICommandManager>();

	[ObservableProperty]
	private TabItemViewModel _mySelf;

	private BaseShellViewModel? _parentShellViewModel;

	private readonly Dictionary<(FolderLayoutModes LayoutMode, int OwnerId), Control> _cachedViews = new Dictionary<(FolderLayoutModes, int), Control>();

	private List<string> _historyPathList = new List<string>();

	private int _historyPathIndex;

	[ObservableProperty]
	private string _title = string.Empty;

	[ObservableProperty]
	private FolderLayoutModes _folderLayoutMode;

	[ObservableProperty]
	private SortOption _currentSortOption;

	[ObservableProperty]
	private SortDirection _currentSortDirection;

	[ObservableProperty]
	private bool _hasCustomSort;

	[ObservableProperty]
	private double _gridViewIconSize = 64.0;

	[ObservableProperty]
	private bool _hasItem;

	[ObservableProperty]
	private int _itemCount;

	[ObservableProperty]
	private string _navPath = string.Empty;

	[ObservableProperty]
	private ObservableCollection<ListedItem> _selectedItems = new ObservableCollection<ListedItem>();

	private bool isPageTypeRecycleBin;

	private bool isPageTypeZipFolder;

	[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.RelayCommandGenerator", "8.4.0.0")]
	private RelayCommand? newTabCommand;

	[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.RelayCommandGenerator", "8.4.0.0")]
	private RelayCommand? duplicateTabCommand;

	[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.RelayCommandGenerator", "8.4.0.0")]
	private RelayCommand? closeTabCommand;

	[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.RelayCommandGenerator", "8.4.0.0")]
	private RelayCommand? closeOtherTabsCommand;

	[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.RelayCommandGenerator", "8.4.0.0")]
	private RelayCommand? closeTabsToTheLeftCommand;

	[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.RelayCommandGenerator", "8.4.0.0")]
	private RelayCommand? closeTabsToTheRightCommand;

	public BaseShellViewModel? ParentShellViewModel
	{
		get
		{
			return _parentShellViewModel;
		}
		set
		{
			if (SetProperty(ref _parentShellViewModel, value, "ParentShellViewModel"))
			{
				NotifyTabCommandStatesChanged();
			}
		}
	}

	public BaseLayoutViewModel? LayoutViewModel { get; set; }

	public string SelectedFileSizeDisplay
	{
		get
		{
			if (SelectedItems.Count == 0)
			{
				return string.Empty;
			}
			if (SelectedItems.Any(ShouldHideSelectedFileSize))
			{
				return string.Empty;
			}
			return StringsHelper.FormatFileSize(SelectedItems.Sum((ListedItem item) => item.FileSizeBytes));
		}
	}

	public bool HasSelectedFileSizeDisplay => !string.IsNullOrWhiteSpace(SelectedFileSizeDisplay);

	public string SelectedFileImageSizeDisplay
	{
		get
		{
			if (SelectedItems.Count != 1)
			{
				return string.Empty;
			}
			ListedItem listedItem = SelectedItems[0];
			if (listedItem.IsFolder || string.IsNullOrWhiteSpace(listedItem.ItemPath) || !FileExtensionHelpers.IsImageFile(listedItem.ItemPath))
			{
				return string.Empty;
			}
			try
			{
				using FileStream stream = File.OpenRead(listedItem.ItemPath);
				using SKCodec sKCodec = SKCodec.Create(stream);
				if (sKCodec != null)
				{
					return $"{sKCodec.Info.Width} * {sKCodec.Info.Height}";
				}
			}
			catch
			{
			}
			return string.Empty;
		}
	}

	public bool HasSelectedFileImageSizeDisplay => !string.IsNullOrWhiteSpace(SelectedFileImageSizeDisplay);

	public NavigationParams NavigationParameter => new NavigationParams
	{
		NavPath = NavPath
	};

	public KeyGesture? NewTabInputGesture => BuildInputGesture(CommandCodes.NewTab);

	public KeyGesture? DuplicateTabInputGesture => BuildInputGesture(CommandCodes.DuplicateSelectedTab);

	public KeyGesture? CloseTabInputGesture => BuildInputGesture(CommandCodes.CloseSelectedTab);

	public KeyGesture? CloseOtherTabsInputGesture => BuildInputGesture(CommandCodes.CloseOtherTabsSelected);

	public KeyGesture? CloseTabsToTheLeftInputGesture => BuildInputGesture(CommandCodes.CloseTabsToTheLeftSelected);

	public KeyGesture? CloseTabsToTheRightInputGesture => BuildInputGesture(CommandCodes.CloseTabsToTheRightSelected);

	public KeyGesture? ReopenClosedTabInputGesture => BuildInputGesture(CommandCodes.ReopenClosedTab);

	public IRichCommand ReopenClosedTabCommand => CommandManager[CommandCodes.ReopenClosedTab];

	public bool IsPageTypeRecycleBin
	{
		get
		{
			return isPageTypeRecycleBin;
		}
		set
		{
			SetProperty(ref isPageTypeRecycleBin, value, "IsPageTypeRecycleBin");
			OnPropertyChanged("CanCreateFileInPage");
			OnPropertyChanged("CanCopyPathInPage");
			OnPropertyChanged("CanTagFilesInPage");
		}
	}

	public bool IsPageTypeZipFolder
	{
		get
		{
			return isPageTypeZipFolder;
		}
		set
		{
			SetProperty(ref isPageTypeZipFolder, value, "IsPageTypeZipFolder");
			OnPropertyChanged("CanCreateFileInPage");
			OnPropertyChanged("CanTagFilesInPage");
		}
	}

	public bool CanCopyPathInPage => !isPageTypeRecycleBin;

	public bool CanCreateFileInPage
	{
		get
		{
			if (!isPageTypeRecycleBin)
			{
				return !isPageTypeZipFolder;
			}
			return false;
		}
	}

	public bool CanTagFilesInPage
	{
		get
		{
			if (!isPageTypeRecycleBin)
			{
				return !isPageTypeZipFolder;
			}
			return false;
		}
	}

	[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
	[ExcludeFromCodeCoverage]
	public TabItemViewModel MySelf
	{
		get
		{
			return _mySelf;
		}
		[MemberNotNull("_mySelf")]
		set
		{
			if (!EqualityComparer<TabItemViewModel>.Default.Equals(_mySelf, value))
			{
				OnPropertyChanging(__KnownINotifyPropertyChangingArgs.MySelf);
				_mySelf = value;
				OnPropertyChanged(__KnownINotifyPropertyChangedArgs.MySelf);
			}
		}
	}

	[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
	[ExcludeFromCodeCoverage]
	public string Title
	{
		get
		{
			return _title;
		}
		[MemberNotNull("_title")]
		set
		{
			if (!EqualityComparer<string>.Default.Equals(_title, value))
			{
				OnPropertyChanging(__KnownINotifyPropertyChangingArgs.Title);
				_title = value;
				OnPropertyChanged(__KnownINotifyPropertyChangedArgs.Title);
			}
		}
	}

	[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
	[ExcludeFromCodeCoverage]
	public FolderLayoutModes FolderLayoutMode
	{
		get
		{
			return _folderLayoutMode;
		}
		set
		{
			if (!EqualityComparer<FolderLayoutModes>.Default.Equals(_folderLayoutMode, value))
			{
				OnPropertyChanging(__KnownINotifyPropertyChangingArgs.FolderLayoutMode);
				_folderLayoutMode = value;
				OnPropertyChanged(__KnownINotifyPropertyChangedArgs.FolderLayoutMode);
			}
		}
	}

	[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
	[ExcludeFromCodeCoverage]
	public SortOption CurrentSortOption
	{
		get
		{
			return _currentSortOption;
		}
		set
		{
			if (!EqualityComparer<SortOption>.Default.Equals(_currentSortOption, value))
			{
				OnPropertyChanging(__KnownINotifyPropertyChangingArgs.CurrentSortOption);
				_currentSortOption = value;
				OnPropertyChanged(__KnownINotifyPropertyChangedArgs.CurrentSortOption);
			}
		}
	}

	[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
	[ExcludeFromCodeCoverage]
	public SortDirection CurrentSortDirection
	{
		get
		{
			return _currentSortDirection;
		}
		set
		{
			if (!EqualityComparer<SortDirection>.Default.Equals(_currentSortDirection, value))
			{
				OnPropertyChanging(__KnownINotifyPropertyChangingArgs.CurrentSortDirection);
				_currentSortDirection = value;
				OnPropertyChanged(__KnownINotifyPropertyChangedArgs.CurrentSortDirection);
			}
		}
	}

	[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
	[ExcludeFromCodeCoverage]
	public bool HasCustomSort
	{
		get
		{
			return _hasCustomSort;
		}
		set
		{
			if (!EqualityComparer<bool>.Default.Equals(_hasCustomSort, value))
			{
				OnPropertyChanging(__KnownINotifyPropertyChangingArgs.HasCustomSort);
				_hasCustomSort = value;
				OnPropertyChanged(__KnownINotifyPropertyChangedArgs.HasCustomSort);
			}
		}
	}

	[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
	[ExcludeFromCodeCoverage]
	public double GridViewIconSize
	{
		get
		{
			return _gridViewIconSize;
		}
		set
		{
			if (!EqualityComparer<double>.Default.Equals(_gridViewIconSize, value))
			{
				OnPropertyChanging(__KnownINotifyPropertyChangingArgs.GridViewIconSize);
				_gridViewIconSize = value;
				OnPropertyChanged(__KnownINotifyPropertyChangedArgs.GridViewIconSize);
			}
		}
	}

	[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
	[ExcludeFromCodeCoverage]
	public bool HasItem
	{
		get
		{
			return _hasItem;
		}
		set
		{
			if (!EqualityComparer<bool>.Default.Equals(_hasItem, value))
			{
				OnPropertyChanging(__KnownINotifyPropertyChangingArgs.HasItem);
				_hasItem = value;
				OnPropertyChanged(__KnownINotifyPropertyChangedArgs.HasItem);
			}
		}
	}

	[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
	[ExcludeFromCodeCoverage]
	public int ItemCount
	{
		get
		{
			return _itemCount;
		}
		set
		{
			if (!EqualityComparer<int>.Default.Equals(_itemCount, value))
			{
				OnPropertyChanging(__KnownINotifyPropertyChangingArgs.ItemCount);
				_itemCount = value;
				OnPropertyChanged(__KnownINotifyPropertyChangedArgs.ItemCount);
			}
		}
	}

	[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
	[ExcludeFromCodeCoverage]
	public string NavPath
	{
		get
		{
			return _navPath;
		}
		[MemberNotNull("_navPath")]
		set
		{
			if (!EqualityComparer<string>.Default.Equals(_navPath, value))
			{
				OnPropertyChanging(__KnownINotifyPropertyChangingArgs.NavPath);
				_navPath = value;
				OnNavPathChanged(value);
				OnPropertyChanged(__KnownINotifyPropertyChangedArgs.NavPath);
			}
		}
	}

	[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
	[ExcludeFromCodeCoverage]
	public ObservableCollection<ListedItem> SelectedItems
	{
		get
		{
			return _selectedItems;
		}
		[MemberNotNull("_selectedItems")]
		set
		{
			if (!EqualityComparer<ObservableCollection<ListedItem>>.Default.Equals(_selectedItems, value))
			{
				OnPropertyChanging(__KnownINotifyPropertyChangingArgs.SelectedItems);
				_selectedItems = value;
				OnSelectedItemsChanged(value);
				OnPropertyChanged(__KnownINotifyPropertyChangedArgs.SelectedItems);
			}
		}
	}

	[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.RelayCommandGenerator", "8.4.0.0")]
	[ExcludeFromCodeCoverage]
	public IRelayCommand NewTabCommand => newTabCommand ?? (newTabCommand = new RelayCommand(NewTab));

	[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.RelayCommandGenerator", "8.4.0.0")]
	[ExcludeFromCodeCoverage]
	public IRelayCommand DuplicateTabCommand => duplicateTabCommand ?? (duplicateTabCommand = new RelayCommand(DuplicateTab));

	[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.RelayCommandGenerator", "8.4.0.0")]
	[ExcludeFromCodeCoverage]
	public IRelayCommand CloseTabCommand => closeTabCommand ?? (closeTabCommand = new RelayCommand(CloseTab, CanCloseTab));

	[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.RelayCommandGenerator", "8.4.0.0")]
	[ExcludeFromCodeCoverage]
	public IRelayCommand CloseOtherTabsCommand => closeOtherTabsCommand ?? (closeOtherTabsCommand = new RelayCommand(CloseOtherTabs, CanCloseOtherTabs));

	[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.RelayCommandGenerator", "8.4.0.0")]
	[ExcludeFromCodeCoverage]
	public IRelayCommand CloseTabsToTheLeftCommand => closeTabsToTheLeftCommand ?? (closeTabsToTheLeftCommand = new RelayCommand(CloseTabsToTheLeft, CanCloseTabsToTheLeft));

	[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.RelayCommandGenerator", "8.4.0.0")]
	[ExcludeFromCodeCoverage]
	public IRelayCommand CloseTabsToTheRightCommand => closeTabsToTheRightCommand ?? (closeTabsToTheRightCommand = new RelayCommand(CloseTabsToTheRight, CanCloseTabsToTheRight));

	public TabItemViewModel()
	{
		MySelf = this;
		WeakReferenceMessenger.Default.Register(this);
	}

	public static TabItemViewModel CreateNewTab(string navPath)
	{
		ILayoutSettingsService requiredService = Ioc.Default.GetRequiredService<ILayoutSettingsService>();
		return new TabItemViewModel
		{
			NavPath = navPath,
			FolderLayoutMode = NormalizeDefaultLayoutMode(requiredService.DefaultLayoutMode),
			CurrentSortOption = NormalizeDefaultSortOption(requiredService.DefaultSortOption),
			CurrentSortDirection = NormalizeDefaultSortDirection(requiredService.DefaultSortDirection),
			HasCustomSort = true
		};
	}

	private static FolderLayoutModes NormalizeDefaultLayoutMode(FolderLayoutModes layoutMode)
	{
		if (layoutMode != FolderLayoutModes.GridView)
		{
			return FolderLayoutModes.DetailsView;
		}
		return FolderLayoutModes.GridView;
	}

	private static SortOption NormalizeDefaultSortOption(SortOption sortOption)
	{
		if ((sortOption != SortOption.DateModified && (uint)(sortOption - 3) > 1u) || 1 == 0)
		{
			return SortOption.Name;
		}
		return sortOption;
	}

	private static SortDirection NormalizeDefaultSortDirection(SortDirection sortDirection)
	{
		if (sortDirection != SortDirection.Descending)
		{
			return SortDirection.Ascending;
		}
		return SortDirection.Descending;
	}

	public Control GetOrCreateLayoutView()
	{
		return GetOrCreateLayoutView(null);
	}

	public Control GetOrCreateLayoutView(ContentControl? owner)
	{
		int item = ((owner != null) ? RuntimeHelpers.GetHashCode(owner) : 0);
		(FolderLayoutModes, int) key = (FolderLayoutMode, item);
		if (item != 0)
		{
			RemoveStaleCachedViews(item);
		}
		if (_cachedViews.TryGetValue(key, out Control value))
		{
			if (value.DataContext is BaseLayoutViewModel baseLayoutViewModel)
			{
				if (baseLayoutViewModel.IsDisposed)
				{
					RemoveCachedView(key);
				}
				else
				{
					if (baseLayoutViewModel.NavParams?.NavPath != NavPath)
					{
						baseLayoutViewModel.RefreshItems(NavPath);
					}
					if (owner == null || value.Parent == null || IsOwnerAncestorOfView(owner, value))
					{
						LayoutViewModel = baseLayoutViewModel;
						return value;
					}
					RemoveCachedView(key);
				}
			}
			else
			{
				RemoveCachedView(key);
			}
		}
		BaseLayoutViewModel baseLayoutViewModel2;
		Control control;
		if (FolderLayoutMode == FolderLayoutModes.GridView)
		{
			baseLayoutViewModel2 = new GridLayoutViewModel(NavPath, this);
			control = new GridLayoutView
			{
				DataContext = baseLayoutViewModel2
			};
		}
		else
		{
			baseLayoutViewModel2 = new DetailsLayoutViewModel(NavPath, this);
			control = new DetailsLayoutView
			{
				DataContext = baseLayoutViewModel2
			};
		}
		LayoutViewModel = baseLayoutViewModel2;
		_cachedViews[key] = control;
		return control;
	}

	private void RemoveStaleCachedViews(int ownerId)
	{
		List<(FolderLayoutModes, int)> list = null;
		foreach (KeyValuePair<(FolderLayoutModes LayoutMode, int OwnerId), Control> cachedView in _cachedViews)
		{
			if (cachedView.Key.OwnerId != ownerId)
			{
				if (list == null)
				{
					list = new List<(FolderLayoutModes, int)>();
				}
				list.Add(cachedView.Key);
			}
		}
		if (list != null)
		{
			foreach (var key in list)
			{
				RemoveCachedView(key);
			}
		}
	}

	private void RemoveCachedView((FolderLayoutModes LayoutMode, int OwnerId) key)
	{
		if (_cachedViews.TryGetValue(key, out Control value))
		{
			_cachedViews.Remove(key);
			DisposeLayoutView(value);
		}
	}

	private static void DisposeLayoutView(Control value)
	{
		if (value.DataContext is IDisposable disposable)
		{
			disposable.Dispose();
		}
		if (value is IDisposable disposable2)
		{
			disposable2.Dispose();
		}
	}

	private static bool IsOwnerAncestorOfView(ContentControl owner, Control view)
	{
		for (StyledElement parent = view.Parent; parent != null; parent = parent.Parent)
		{
			if (parent == owner)
			{
				return true;
			}
		}
		return false;
	}

	private static bool ShouldHideSelectedFileSize(ListedItem item)
	{
		if (item.IsFolder)
		{
			return true;
		}
		string itemPath = item.ItemPath;
		if (string.IsNullOrWhiteSpace(itemPath))
		{
			return false;
		}
		if (itemPath.TrimEnd('/').EndsWith(".app", StringComparison.OrdinalIgnoreCase))
		{
			return Directory.Exists(itemPath);
		}
		return false;
	}

	public void AddPathToHistoryLast(string path)
	{
		if (!string.IsNullOrWhiteSpace(path) && !string.Equals((_historyPathList.Count > 0 && _historyPathIndex >= 0 && _historyPathIndex < _historyPathList.Count) ? _historyPathList[_historyPathIndex] : null, path, StringComparison.Ordinal))
		{
			if (_historyPathIndex + 1 < _historyPathList.Count)
			{
				_historyPathList.RemoveRange(_historyPathIndex + 1, _historyPathList.Count - _historyPathIndex - 1);
			}
			_historyPathList.Add(path);
			_historyPathIndex = _historyPathList.Count - 1;
		}
	}

	public bool CanGoBack()
	{
		if (_historyPathList.Count > 1 && _historyPathIndex > 0)
		{
			return true;
		}
		return false;
	}

	public string GetBackHistoryPath()
	{
		return _historyPathList[_historyPathIndex - 1];
	}

	public void BackHistoryPathIndex()
	{
		_historyPathIndex--;
	}

	public bool CanGoForward()
	{
		if (_historyPathList.Count > 1 && _historyPathIndex < _historyPathList.Count - 1)
		{
			return true;
		}
		return false;
	}

	public string GetForwardHistoryPath()
	{
		return _historyPathList[_historyPathIndex + 1];
	}

	public void ForwardHistoryPathIndex()
	{
		_historyPathIndex++;
	}

	[RelayCommand]
	private void NewTab()
	{
		ParentShellViewModel?.NewTab(this);
	}

	[RelayCommand]
	private void DuplicateTab()
	{
		ParentShellViewModel?.DuplicateTab(this);
	}

	[RelayCommand(CanExecute = "CanCloseTab")]
	public void CloseTab()
	{
		ParentShellViewModel?.CloseTab(this);
	}

	private bool CanCloseTab()
	{
		return ParentShellViewModel?.CanCloseTab(this) ?? false;
	}

	[RelayCommand(CanExecute = "CanCloseOtherTabs")]
	private void CloseOtherTabs()
	{
		ParentShellViewModel?.CloseOtherTabs(this);
	}

	private bool CanCloseOtherTabs()
	{
		return ParentShellViewModel?.CanCloseOtherTabs(this) ?? false;
	}

	[RelayCommand(CanExecute = "CanCloseTabsToTheLeft")]
	private void CloseTabsToTheLeft()
	{
		ParentShellViewModel?.CloseTabsToTheLeft(this);
	}

	private bool CanCloseTabsToTheLeft()
	{
		return ParentShellViewModel?.CanCloseTabsToTheLeft(this) ?? false;
	}

	[RelayCommand(CanExecute = "CanCloseTabsToTheRight")]
	private void CloseTabsToTheRight()
	{
		ParentShellViewModel?.CloseTabsToTheRight(this);
	}

	private bool CanCloseTabsToTheRight()
	{
		return ParentShellViewModel?.CanCloseTabsToTheRight(this) ?? false;
	}

	public void NotifyTabCommandStatesChanged()
	{
		CloseTabCommand.NotifyCanExecuteChanged();
		CloseOtherTabsCommand.NotifyCanExecuteChanged();
		CloseTabsToTheLeftCommand.NotifyCanExecuteChanged();
		CloseTabsToTheRightCommand.NotifyCanExecuteChanged();
	}

	private static KeyGesture? BuildInputGesture(CommandCodes commandCode)
	{
		return ShortcutHelpers.GetPrimaryKeyGesture(CommandManager[commandCode].HotKeys);
	}

	public void Receive(LayoutViewSelectedItemsChanged message)
	{
		if (message.tabItemViewModel == this)
		{
			SelectedItems = new ObservableCollection<ListedItem>(message.selectedItems);
		}
	}

	public void Dispose()
	{
		WeakReferenceMessenger.Default.Unregister<LayoutViewSelectedItemsChanged>(this);
		foreach (Control value in _cachedViews.Values)
		{
			DisposeLayoutView(value);
		}
		_cachedViews.Clear();
		LayoutViewModel?.Dispose();
		LayoutViewModel = null;
	}

	[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
	private void OnNavPathChanged(string value)
	{
		if (value == "/")
		{
			Title = value;
		}
		else
		{
			Title = Path.GetFileName(value);
		}
	}

	[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
	private void OnSelectedItemsChanged(ObservableCollection<ListedItem> value)
	{
		OnPropertyChanged("SelectedFileSizeDisplay");
		OnPropertyChanged("HasSelectedFileSizeDisplay");
		OnPropertyChanged("SelectedFileImageSizeDisplay");
		OnPropertyChanged("HasSelectedFileImageSizeDisplay");
	}
}
