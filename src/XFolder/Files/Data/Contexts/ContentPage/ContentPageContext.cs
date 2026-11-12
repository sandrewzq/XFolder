using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Messaging;
using Files.Data.Contracts;
using Files.Data.Items;
using Files.Data.Messages;
using Files.UserControls.NavigationToolbar;
using Files.ViewModels.Items;
using Files.Views.Layouts;

namespace Files.Data.Contexts.ContentPage;

internal sealed class ContentPageContext : ObservableObject, IContentPageContext, INotifyPropertyChanged
{
	private static readonly IReadOnlyList<ListedItem> emptyItems = Enumerable.Empty<ListedItem>().ToImmutableList();

	private readonly IMultiPanesContext context = Ioc.Default.GetRequiredService<IMultiPanesContext>();

	private readonly IMultitaskingContext multitaskingContext = Ioc.Default.GetRequiredService<IMultitaskingContext>();

	private IReadOnlyList<ListedItem> selectedItems = emptyItems;

	private NavigationToolbarViewModel? navigationToolbarViewModel;

	public IShellPanesView? ShellPanesView => context.PanesView;

	public IShellView? ShellView => context?.ActivePane;

	public ContentPageTypes PageType => ContentPageTypes.Folder;

	public Type PageLayoutType => ShellView?.CurrentPageType ?? typeof(DetailsLayoutView);

	public bool HasSelection => SelectedItems.Count != 0;

	public ListedItem? SelectedItem
	{
		get
		{
			if (SelectedItems.Count != 1)
			{
				return null;
			}
			return SelectedItems[0];
		}
	}

	public bool HasItem => ShellView?.InstanceViewModel?.NavigationToolbarViewModel?.HasItem == true;

	public IReadOnlyList<ListedItem> SelectedItems => selectedItems;

	public bool CanRefresh => ShellView?.InstanceViewModel?.NavigationToolbarViewModel?.CanRefresh == true;

	public bool CanGoBack => ShellView?.InstanceViewModel?.NavigationToolbarViewModel?.CanGoBack == true;

	public bool CanGoForward => ShellView?.InstanceViewModel?.NavigationToolbarViewModel?.CanGoForward == true;

	public bool CanNavigateToParent => ShellView?.InstanceViewModel?.NavigationToolbarViewModel?.CanNavigateToParent == true;

	public bool CanCreateItem => multitaskingContext?.SelectedTabViewModel?.CanCreateFileInPage == true;

	public ContentPageContext()
	{
		context.ActivePaneChanged += Context_ActivePaneChanged;
		multitaskingContext.PropertyChanged += MultitaskingContext_PropertyChanged;
		WeakReferenceMessenger.Default.Register(this, (object recipient, LayoutViewSelectedItemsChanged message) =>
		{
			((ContentPageContext)recipient).HandleSelectedItemsChanged(message);
		});
		selectedItems = GetSelectedItemsSnapshot();
		UpdateNavigationToolbarSubscription();
	}

	private void Context_ActivePaneChanged(object? sender, EventArgs e)
	{
		UpdateNavigationToolbarSubscription();
		RaiseShellStateChanged();
	}

	private void UpdateNavigationToolbarSubscription()
	{
		NavigationToolbarViewModel navigationToolbarViewModel = ShellView?.InstanceViewModel?.NavigationToolbarViewModel;
		if (this.navigationToolbarViewModel != navigationToolbarViewModel)
		{
			if (this.navigationToolbarViewModel != null)
			{
				this.navigationToolbarViewModel.PropertyChanged -= NavigationToolbarViewModel_PropertyChanged;
			}
			this.navigationToolbarViewModel = navigationToolbarViewModel;
			if (this.navigationToolbarViewModel != null)
			{
				this.navigationToolbarViewModel.PropertyChanged += NavigationToolbarViewModel_PropertyChanged;
			}
		}
	}

	private void NavigationToolbarViewModel_PropertyChanged(object? sender, PropertyChangedEventArgs e)
	{
		switch (e.PropertyName)
		{
		case "HasItem":
			OnPropertyChanged("HasItem");
			break;
		case "CanRefresh":
			OnPropertyChanged("CanRefresh");
			break;
		case "CanGoBack":
			OnPropertyChanged("CanGoBack");
			break;
		case "CanGoForward":
			OnPropertyChanged("CanGoForward");
			break;
		case "CanNavigateToParent":
			OnPropertyChanged("CanNavigateToParent");
			break;
		}
	}

	private void MultitaskingContext_PropertyChanged(object? sender, PropertyChangedEventArgs e)
	{
		string propertyName = e.PropertyName;
		if (propertyName == "SelectedTabViewModel" || propertyName == "SelectedTabIndex")
		{
			selectedItems = GetSelectedItemsSnapshot();
			RaiseSelectionStateChanged();
			OnPropertyChanged("CanCreateItem");
		}
	}

	private void HandleSelectedItemsChanged(LayoutViewSelectedItemsChanged message)
	{
		if (message.tabItemViewModel == multitaskingContext.SelectedTabViewModel)
		{
			IReadOnlyList<ListedItem> readOnlyList = message.selectedItems?.ToImmutableList();
			selectedItems = readOnlyList ?? emptyItems;
			RaiseSelectionStateChanged();
		}
	}

	private void RaiseShellStateChanged()
	{
		OnPropertyChanged("ShellView");
		OnPropertyChanged("PageLayoutType");
		OnPropertyChanged("HasItem");
		OnPropertyChanged("CanRefresh");
		OnPropertyChanged("CanGoBack");
		OnPropertyChanged("CanGoForward");
		OnPropertyChanged("CanNavigateToParent");
		RaiseSelectionStateChanged();
	}

	private void RaiseSelectionStateChanged()
	{
		OnPropertyChanged("HasSelection");
		OnPropertyChanged("SelectedItems");
		OnPropertyChanged("SelectedItem");
	}

	private IReadOnlyList<ListedItem> GetSelectedItemsSnapshot()
	{
		TabItemViewModel selectedTabViewModel = multitaskingContext.SelectedTabViewModel;
		if (selectedTabViewModel?.SelectedItems == null || selectedTabViewModel.SelectedItems.Count == 0)
		{
			return emptyItems;
		}
		return selectedTabViewModel.SelectedItems.ToImmutableList();
	}
}
