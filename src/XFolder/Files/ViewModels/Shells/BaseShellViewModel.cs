using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel.__Internals;
using CommunityToolkit.Mvvm.DependencyInjection;
using Files.Assets.Lang;
using Files.Data.Contexts;
using Files.Data.Enums.Navigation;
using Files.Data.Items;
using Files.Helpers;
using Files.Helpers.Shared;
using Files.UserControls.NavigationToolbar;
using Files.ViewModels.Items;
using Files.Views.Shells;

namespace Files.ViewModels.Shells;

public class BaseShellViewModel : ViewModelBase
{
	private NavigationToolbarViewModel? _navigationToolbarViewModel;

	private readonly IMultitaskingContext multitaskingContext = Ioc.Default.GetRequiredService<IMultitaskingContext>();

	public LayoutPreferencesManager FolderSettings = new LayoutPreferencesManager();

	[ObservableProperty]
	private ObservableCollection<TabItemViewModel> _tabItems = new ObservableCollection<TabItemViewModel>();

	[ObservableProperty]
	private int _selectedTabIndex;

	[ObservableProperty]
	private TabItemViewModel? _selectedTab;

	[ObservableProperty]
	private ListedItem? _currentFolder;

	private string previousDir = string.Empty;

	[ObservableProperty]
	private string _workingDirectory = string.Empty;

	public NavigationToolbarViewModel? NavigationToolbarViewModel
	{
		get
		{
			return _navigationToolbarViewModel;
		}
		set
		{
			SetProperty(ref _navigationToolbarViewModel, value, "NavigationToolbarViewModel");
		}
	}

	public BaseShellView? View { get; set; }

	[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
	[ExcludeFromCodeCoverage]
	public ObservableCollection<TabItemViewModel> TabItems
	{
		get
		{
			return _tabItems;
		}
		[MemberNotNull("_tabItems")]
		set
		{
			if (!EqualityComparer<ObservableCollection<TabItemViewModel>>.Default.Equals(_tabItems, value))
			{
				OnTabItemsChanging(value);
				OnPropertyChanging(__KnownINotifyPropertyChangingArgs.TabItems);
				_tabItems = value;
				OnTabItemsChanged(value);
				OnPropertyChanged(__KnownINotifyPropertyChangedArgs.TabItems);
			}
		}
	}

	[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
	[ExcludeFromCodeCoverage]
	public int SelectedTabIndex
	{
		get
		{
			return _selectedTabIndex;
		}
		set
		{
			if (!EqualityComparer<int>.Default.Equals(_selectedTabIndex, value))
			{
				OnPropertyChanging(__KnownINotifyPropertyChangingArgs.SelectedTabIndex);
				_selectedTabIndex = value;
				OnPropertyChanged(__KnownINotifyPropertyChangedArgs.SelectedTabIndex);
			}
		}
	}

	[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
	[ExcludeFromCodeCoverage]
	public TabItemViewModel? SelectedTab
	{
		get
		{
			return _selectedTab;
		}
		set
		{
			if (!EqualityComparer<TabItemViewModel>.Default.Equals(_selectedTab, value))
			{
				OnSelectedTabChanging(value);
				OnPropertyChanging(__KnownINotifyPropertyChangingArgs.SelectedTab);
				_selectedTab = value;
				OnSelectedTabChanged(value);
				OnPropertyChanged(__KnownINotifyPropertyChangedArgs.SelectedTab);
			}
		}
	}

	[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
	[ExcludeFromCodeCoverage]
	public ListedItem? CurrentFolder
	{
		get
		{
			return _currentFolder;
		}
		set
		{
			if (!EqualityComparer<ListedItem>.Default.Equals(_currentFolder, value))
			{
				OnPropertyChanging(__KnownINotifyPropertyChangingArgs.CurrentFolder);
				_currentFolder = value;
				OnPropertyChanged(__KnownINotifyPropertyChangedArgs.CurrentFolder);
			}
		}
	}

	[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
	[ExcludeFromCodeCoverage]
	public string WorkingDirectory
	{
		get
		{
			return _workingDirectory;
		}
		[MemberNotNull("_workingDirectory")]
		set
		{
			if (!EqualityComparer<string>.Default.Equals(_workingDirectory, value))
			{
				OnPropertyChanging(__KnownINotifyPropertyChangingArgs.WorkingDirectory);
				_workingDirectory = value;
				OnWorkingDirectoryChanged(value);
				OnPropertyChanged(__KnownINotifyPropertyChangedArgs.WorkingDirectory);
			}
		}
	}

	public BaseShellViewModel()
	{
		TabItems.CollectionChanged += TabItems_CollectionChanged;
	}

	private void TabItems_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
	{
		NotifyTabCommandStatesChanged();
	}

	private void NotifyTabCommandStatesChanged()
	{
		foreach (TabItemViewModel tabItem in TabItems)
		{
			tabItem.NotifyTabCommandStatesChanged();
		}
	}

	private void SelectedTab_PropertyChanged(object? sender, PropertyChangedEventArgs e)
	{
		if (e.PropertyName == "HasItem" && NavigationToolbarViewModel != null && SelectedTab != null)
		{
			NavigationToolbarViewModel.HasItem = SelectedTab.HasItem;
		}
	}

	public string ConsumePreviousDirectory()
	{
		string result = previousDir;
		previousDir = string.Empty;
		return result;
	}

	public bool SetWorkingDirectory(string newPath, SetNavPathLocationEnum location)
	{
		bool result = true;
		if (string.IsNullOrWhiteSpace(newPath))
		{
			NavigationToolbarViewModel?.UpdateUIState();
			return false;
		}
		if (WorkingDirectory != newPath)
		{
			if (PathHelpers.IsCanAccessFolder(newPath))
			{
				if (location != SetNavPathLocationEnum.SelectedTabChanged)
				{
					previousDir = WorkingDirectory;
				}
				else
				{
					previousDir = string.Empty;
				}
				WorkingDirectory = newPath;
				switch (location)
				{
				case SetNavPathLocationEnum.BackHistoryPath:
					SelectedTab?.BackHistoryPathIndex();
					break;
				case SetNavPathLocationEnum.ForwardHistoryPath:
					SelectedTab?.ForwardHistoryPathIndex();
					break;
				default:
					SelectedTab?.AddPathToHistoryLast(newPath);
					break;
				}
			}
			else
			{
				DialogDisplayHelper.ShowTipAsync("", Resources.AccessDenied);
				result = false;
			}
		}
		NavigationToolbarViewModel.UpdateUIState();
		return result;
	}

	public void GoBack()
	{
		TabItemViewModel? selectedTab = SelectedTab;
		if (selectedTab != null && selectedTab.CanGoBack())
		{
			string backHistoryPath = SelectedTab.GetBackHistoryPath();
			SetWorkingDirectory(backHistoryPath, SetNavPathLocationEnum.BackHistoryPath);
		}
	}

	public void GoForward()
	{
		TabItemViewModel? selectedTab = SelectedTab;
		if (selectedTab != null && selectedTab.CanGoForward())
		{
			string forwardHistoryPath = SelectedTab.GetForwardHistoryPath();
			SetWorkingDirectory(forwardHistoryPath, SetNavPathLocationEnum.ForwardHistoryPath);
		}
	}

	public void NewTab(TabItemViewModel? tabItem)
	{
		if (tabItem != null && TabItems.Contains(tabItem))
		{
			InsertAdjacentTab(tabItem, NavigationHelpers.GetDefaultNewTabPath());
		}
	}

	public void DuplicateTab(TabItemViewModel tabItem)
	{
		if (tabItem != null && TabItems.Contains(tabItem))
		{
			InsertAdjacentTab(tabItem, tabItem.NavPath);
		}
	}

	public void CloseTab(TabItemViewModel tabItem)
	{
		if (CanCloseTab(tabItem))
		{
			int num = TabItems.IndexOf(tabItem);
			bool flag = SelectedTab == tabItem;
			RemoveTab(tabItem);
			if (flag)
			{
				int index = ((num > 0) ? (num - 1) : 0);
				SelectedTab = TabItems[index];
			}
			else if (SelectedTab == null || !TabItems.Contains(SelectedTab))
			{
				SelectedTab = TabItems[Math.Clamp((num > 0) ? (num - 1) : 0, 0, TabItems.Count - 1)];
			}
			SelectedTabIndex = TabItems.IndexOf(SelectedTab);
		}
	}

	public bool CanCloseTab(TabItemViewModel? tabItem)
	{
		if (tabItem != null && TabItems.Count > 1)
		{
			return TabItems.Contains(tabItem);
		}
		return false;
	}

	public void CloseOtherTabs(TabItemViewModel tabItem)
	{
		if (!CanCloseOtherTabs(tabItem))
		{
			return;
		}
		for (int num = TabItems.Count - 1; num >= 0; num--)
		{
			if (TabItems[num] != tabItem)
			{
				RemoveTab(TabItems[num]);
			}
		}
		SelectedTab = tabItem;
		SelectedTabIndex = TabItems.IndexOf(SelectedTab);
	}

	public bool CanCloseOtherTabs(TabItemViewModel? tabItem)
	{
		if (tabItem != null && TabItems.Count > 1)
		{
			return TabItems.Contains(tabItem);
		}
		return false;
	}

	public void CloseTabsToTheLeft(TabItemViewModel tabItem)
	{
		if (CanCloseTabsToTheLeft(tabItem))
		{
			for (int num = TabItems.IndexOf(tabItem) - 1; num >= 0; num--)
			{
				RemoveTab(TabItems[num]);
			}
			if (SelectedTab == null || !TabItems.Contains(SelectedTab))
			{
				SelectedTab = tabItem;
			}
			SelectedTabIndex = TabItems.IndexOf(SelectedTab);
		}
	}

	public bool CanCloseTabsToTheLeft(TabItemViewModel? tabItem)
	{
		return ((tabItem == null) ? (-1) : TabItems.IndexOf(tabItem)) > 0;
	}

	public void CloseTabsToTheRight(TabItemViewModel tabItem)
	{
		if (CanCloseTabsToTheRight(tabItem))
		{
			int num = TabItems.IndexOf(tabItem);
			for (int num2 = TabItems.Count - 1; num2 > num; num2--)
			{
				RemoveTab(TabItems[num2]);
			}
			if (SelectedTab == null || !TabItems.Contains(SelectedTab))
			{
				SelectedTab = tabItem;
			}
			SelectedTabIndex = TabItems.IndexOf(SelectedTab);
		}
	}

	public bool CanCloseTabsToTheRight(TabItemViewModel? tabItem)
	{
		int num = ((tabItem == null) ? (-1) : TabItems.IndexOf(tabItem));
		if (num >= 0)
		{
			return num < TabItems.Count - 1;
		}
		return false;
	}

	private void RemoveTab(TabItemViewModel tabItem)
	{
		int num = TabItems.IndexOf(tabItem);
		if (num >= 0)
		{
			multitaskingContext.PushRecentlyClosedTab(tabItem, this, num);
			TabItems.RemoveAt(num);
			tabItem.Dispose();
		}
	}

	private void InsertAdjacentTab(TabItemViewModel tabItem, string navPath)
	{
		int num = TabItems.IndexOf(tabItem);
		if (num >= 0)
		{
			TabItemViewModel tabItemViewModel = TabItemViewModel.CreateNewTab(navPath);
			tabItemViewModel.ParentShellViewModel = this;
			TabItems.Insert(num + 1, tabItemViewModel);
			SelectedTab = tabItemViewModel;
		}
	}

	[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
	private void OnTabItemsChanging(ObservableCollection<TabItemViewModel> value)
	{
		if (TabItems != null)
		{
			TabItems.CollectionChanged -= TabItems_CollectionChanged;
		}
	}

	[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
	private void OnTabItemsChanged(ObservableCollection<TabItemViewModel> value)
	{
		if (value != null)
		{
			value.CollectionChanged += TabItems_CollectionChanged;
			NotifyTabCommandStatesChanged();
		}
	}

	[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
	private void OnSelectedTabChanging(TabItemViewModel? value)
	{
		if (SelectedTab != null)
		{
			SelectedTab.PropertyChanged -= SelectedTab_PropertyChanged;
		}
	}

	[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
	private void OnSelectedTabChanged(TabItemViewModel? value)
	{
		if (value != null)
		{
			value.PropertyChanged += SelectedTab_PropertyChanged;
			SelectedTabIndex = TabItems.IndexOf(SelectedTab);
			string text = (string.IsNullOrWhiteSpace(value.NavPath) ? NavigationHelpers.GetDefaultNewTabPath() : value.NavPath);
			if (!string.Equals(value.NavPath, text, StringComparison.Ordinal))
			{
				value.NavPath = text;
			}
			SetWorkingDirectory(text, SetNavPathLocationEnum.SelectedTabChanged);
			if (NavigationToolbarViewModel != null)
			{
				NavigationToolbarViewModel.HasItem = value.HasItem;
			}
		}
	}

	[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
	private void OnWorkingDirectoryChanged(string value)
	{
		if (NavigationToolbarViewModel != null)
		{
			NavigationToolbarViewModel.PathControlDisplayText = value;
		}
	}
}
