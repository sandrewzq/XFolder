using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel.__Internals;
using CommunityToolkit.Mvvm.DependencyInjection;
using Files.ViewModels.Items;
using Files.ViewModels.Shells;

namespace Files.Data.Contexts.Multitasking;

public sealed class MultitaskingContext : ObservableObject, IMultitaskingContext, INotifyPropertyChanged
{
	private sealed record RecentlyClosedTabState(string NavPath, int TabIndex, WeakReference<TabItemViewModel>? LeftNeighbor, WeakReference<TabItemViewModel>? RightNeighbor);

	private readonly IMultiPanesContext context = Ioc.Default.GetRequiredService<IMultiPanesContext>();

	private readonly Dictionary<BaseShellViewModel, Stack<RecentlyClosedTabState>> recentlyClosedTabsByPane = new Dictionary<BaseShellViewModel, Stack<RecentlyClosedTabState>>();

	[ObservableProperty]
	private int _tabCount;

	[ObservableProperty]
	private int _selectedTabIndex;

	[ObservableProperty]
	private TabItemViewModel? _selectedTabViewModel;

	[ObservableProperty]
	private int _recentlyClosedTabCount;

	[ObservableProperty]
	private bool _isRestoringClosedTab;

	[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
	[ExcludeFromCodeCoverage]
	public int TabCount
	{
		get
		{
			return _tabCount;
		}
		set
		{
			if (!EqualityComparer<int>.Default.Equals(_tabCount, value))
			{
				OnPropertyChanging(__KnownINotifyPropertyChangingArgs.TabCount);
				_tabCount = value;
				OnPropertyChanged(__KnownINotifyPropertyChangedArgs.TabCount);
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
	public TabItemViewModel? SelectedTabViewModel
	{
		get
		{
			return _selectedTabViewModel;
		}
		set
		{
			if (!EqualityComparer<TabItemViewModel>.Default.Equals(_selectedTabViewModel, value))
			{
				OnPropertyChanging(__KnownINotifyPropertyChangingArgs.SelectedTabViewModel);
				_selectedTabViewModel = value;
				OnPropertyChanged(__KnownINotifyPropertyChangedArgs.SelectedTabViewModel);
			}
		}
	}

	[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
	[ExcludeFromCodeCoverage]
	public int RecentlyClosedTabCount
	{
		get
		{
			return _recentlyClosedTabCount;
		}
		set
		{
			if (!EqualityComparer<int>.Default.Equals(_recentlyClosedTabCount, value))
			{
				OnPropertyChanging(__KnownINotifyPropertyChangingArgs.RecentlyClosedTabCount);
				_recentlyClosedTabCount = value;
				OnPropertyChanged(__KnownINotifyPropertyChangedArgs.RecentlyClosedTabCount);
			}
		}
	}

	[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
	[ExcludeFromCodeCoverage]
	public bool IsRestoringClosedTab
	{
		get
		{
			return _isRestoringClosedTab;
		}
		set
		{
			if (!EqualityComparer<bool>.Default.Equals(_isRestoringClosedTab, value))
			{
				OnPropertyChanging(__KnownINotifyPropertyChangingArgs.IsRestoringClosedTab);
				_isRestoringClosedTab = value;
				OnPropertyChanged(__KnownINotifyPropertyChangedArgs.IsRestoringClosedTab);
			}
		}
	}

	public MultitaskingContext()
	{
		context.ActivePaneChanging += Context_Changing;
		context.ActivePaneChanged += Context_Changed;
	}

	private void Context_Changing(object? sender, EventArgs e)
	{
		if (context.ActivePaneViewModel is ModernShellViewModel modernShellViewModel)
		{
			modernShellViewModel.PropertyChanged -= ShellViewModel_PropertyChanged;
			modernShellViewModel.TabItems.CollectionChanged -= TabItems_CollectionChanged;
		}
	}

	private void Context_Changed(object? sender, EventArgs e)
	{
		if (context.ActivePaneViewModel is ModernShellViewModel modernShellViewModel)
		{
			modernShellViewModel.PropertyChanged += ShellViewModel_PropertyChanged;
			modernShellViewModel.TabItems.CollectionChanged += TabItems_CollectionChanged;
			InitFromNewViewModel(modernShellViewModel);
		}
		else
		{
			RecentlyClosedTabCount = 0;
		}
	}

	private void ShellViewModel_PropertyChanged(object? sender, PropertyChangedEventArgs e)
	{
		if (!(sender is ModernShellViewModel modernShellViewModel))
		{
			return;
		}
		string propertyName = e.PropertyName;
		if (!(propertyName == "SelectedTabIndex"))
		{
			if (propertyName == "SelectedTab")
			{
				SelectedTabViewModel = modernShellViewModel.SelectedTab;
			}
		}
		else
		{
			SelectedTabIndex = modernShellViewModel.SelectedTabIndex;
		}
	}

	private void TabItems_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
	{
		if (sender is ObservableCollection<TabItemViewModel> observableCollection)
		{
			TabCount = observableCollection.Count;
		}
	}

	private void InitFromNewViewModel(ModernShellViewModel modernShellViewModel)
	{
		TabCount = modernShellViewModel.TabItems.Count;
		SelectedTabIndex = modernShellViewModel.SelectedTabIndex;
		SelectedTabViewModel = modernShellViewModel.SelectedTab;
		RecentlyClosedTabCount = GetRecentlyClosedTabs(modernShellViewModel).Count;
	}

	public void PushRecentlyClosedTab(TabItemViewModel tabItemViewModel, BaseShellViewModel parentShellViewModel, int tabIndex)
	{
		if (tabItemViewModel != null && parentShellViewModel != null && tabIndex >= 0 && !string.IsNullOrWhiteSpace(tabItemViewModel.NavPath))
		{
			GetRecentlyClosedTabs(parentShellViewModel).Push(CreateRecentlyClosedTabState(tabItemViewModel, parentShellViewModel, tabIndex));
			RefreshRecentlyClosedTabCount();
		}
	}

	public Task<bool> ReopenClosedTabAsync()
	{
		if (IsRestoringClosedTab || context.ActivePaneViewModel == null)
		{
			return Task.FromResult(result: false);
		}
		BaseShellViewModel activePaneViewModel = context.ActivePaneViewModel;
		Stack<RecentlyClosedTabState> recentlyClosedTabs = GetRecentlyClosedTabs(activePaneViewModel);
		if (recentlyClosedTabs.Count == 0)
		{
			return Task.FromResult(result: false);
		}
		IsRestoringClosedTab = true;
		try
		{
			RecentlyClosedTabState recentlyClosedTabState = recentlyClosedTabs.Pop();
			RefreshRecentlyClosedTabCount();
			TabItemViewModel tabItemViewModel = TabItemViewModel.CreateNewTab(recentlyClosedTabState.NavPath);
			tabItemViewModel.ParentShellViewModel = activePaneViewModel;
			tabItemViewModel.AddPathToHistoryLast(tabItemViewModel.NavPath);
			int index = ResolveInsertIndex(activePaneViewModel, recentlyClosedTabState);
			activePaneViewModel.TabItems.Insert(index, tabItemViewModel);
			activePaneViewModel.SelectedTab = tabItemViewModel;
			return Task.FromResult(result: true);
		}
		finally
		{
			IsRestoringClosedTab = false;
		}
	}

	private Stack<RecentlyClosedTabState> GetRecentlyClosedTabs(BaseShellViewModel paneViewModel)
	{
		if (!recentlyClosedTabsByPane.TryGetValue(paneViewModel, out Stack<RecentlyClosedTabState> value))
		{
			value = new Stack<RecentlyClosedTabState>();
			recentlyClosedTabsByPane[paneViewModel] = value;
		}
		return value;
	}

	private RecentlyClosedTabState CreateRecentlyClosedTabState(TabItemViewModel tabItemViewModel, BaseShellViewModel parentShellViewModel, int tabIndex)
	{
		ObservableCollection<TabItemViewModel> tabItems = parentShellViewModel.TabItems;
		WeakReference<TabItemViewModel> leftNeighbor = ((tabIndex > 0) ? new WeakReference<TabItemViewModel>(tabItems[tabIndex - 1]) : null);
		WeakReference<TabItemViewModel> rightNeighbor = ((tabIndex + 1 < tabItems.Count) ? new WeakReference<TabItemViewModel>(tabItems[tabIndex + 1]) : null);
		return new RecentlyClosedTabState(tabItemViewModel.NavPath, tabIndex, leftNeighbor, rightNeighbor);
	}

	private int ResolveInsertIndex(BaseShellViewModel targetPaneViewModel, RecentlyClosedTabState closedTab)
	{
		if (TryGetNeighborIndex(targetPaneViewModel, closedTab.RightNeighbor, out var neighborIndex))
		{
			return neighborIndex;
		}
		if (TryGetNeighborIndex(targetPaneViewModel, closedTab.LeftNeighbor, out var neighborIndex2))
		{
			return neighborIndex2 + 1;
		}
		return Math.Clamp(closedTab.TabIndex, 0, targetPaneViewModel.TabItems.Count);
	}

	private static bool TryGetNeighborIndex(BaseShellViewModel targetPaneViewModel, WeakReference<TabItemViewModel>? neighborReference, out int neighborIndex)
	{
		neighborIndex = -1;
		if (neighborReference == null || !neighborReference.TryGetTarget(out TabItemViewModel target))
		{
			return false;
		}
		neighborIndex = targetPaneViewModel.TabItems.IndexOf(target);
		return neighborIndex >= 0;
	}

	private void RefreshRecentlyClosedTabCount()
	{
		if (context.ActivePaneViewModel == null)
		{
			RecentlyClosedTabCount = 0;
		}
		else
		{
			RecentlyClosedTabCount = GetRecentlyClosedTabs(context.ActivePaneViewModel).Count;
		}
	}
}
