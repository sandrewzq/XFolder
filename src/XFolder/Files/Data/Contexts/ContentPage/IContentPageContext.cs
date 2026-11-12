using System;
using System.Collections.Generic;
using System.ComponentModel;
using Files.Data.Contracts;
using Files.Data.Items;

namespace Files.Data.Contexts.ContentPage;

public interface IContentPageContext : INotifyPropertyChanged
{
	IShellPanesView? ShellPanesView { get; }

	IShellView? ShellView { get; }

	ContentPageTypes PageType { get; }

	Type PageLayoutType { get; }

	bool HasItem { get; }

	bool HasSelection { get; }

	bool CanRefresh { get; }

	ListedItem? SelectedItem { get; }

	IReadOnlyList<ListedItem> SelectedItems { get; }

	bool CanGoBack { get; }

	bool CanGoForward { get; }

	bool CanNavigateToParent { get; }

	bool CanCreateItem { get; }
}
