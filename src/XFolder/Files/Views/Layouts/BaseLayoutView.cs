using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using Avalonia.Threading;
using Avalonia.VisualTree;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Messaging;
using Files.Data.Commands;
using Files.Data.Commands.Manager;
using Files.Data.Contexts;
using Files.Data.Contracts;
using Files.Data.Enums;
using Files.Data.Enums.Storage;
using Files.Data.Factories;
using Files.Data.Items;
using Files.Data.Messages;
using Files.Data.Models;
using Files.Helpers;
using Files.Helpers.ContextFlyouts;
using Files.Helpers.Shared;
using Files.Helpers.UI;
using Files.Services.NativePlugin;
using Files.Utils.Storage.Operations;
using Files.ViewModels.Layouts;
using Files.ViewModels.Shells;

namespace Files.Views.Layouts;

public abstract class BaseLayoutView : UserControl, IBaseLayoutView, IDisposable, INotifyPropertyChanged
{
	public static readonly StyledProperty<double> BaseFontSizeProperty = AvaloniaProperty.Register<BaseLayoutView, double>("BaseFontSize", 13.0);

	private List<ListedItem> _selectedItems = new List<ListedItem>();

	private string _keyboardSearchText = string.Empty;

	private string _keyboardSearchRawInput = string.Empty;

	private long _lastKeyboardSearchTimeTick = DateTime.Now.Ticks;

	private const long KeyboardSearchTimeoutTicks = 5000000L;

	private const double DragThreshold = 80.0;

	private bool _isDragging;

	private Point _dragStartPoint;

	private PointerPressedEventArgs? _dragStartEvent;

	private bool _isDragOver;

	private bool _hasPerformedDrag;

	private ListedItem? _dragOverItem;

	private Control? _dragOverItemContainer;

	private LayoutDragData? _currentDragData;

	protected bool _isMarqueeSelecting;

	protected Point _marqueeStartPoint;

	protected List<ListedItem> _preMarqueeSelectedItems = new List<ListedItem>();

	protected bool _marqueeIsToggleMode;

	private bool _suppressedSelectionEvent;

	private ListedItem? _suppressedItem;

	protected bool _isCommittingRename;

	public double BaseFontSize
	{
		get
		{
			return GetValue(BaseFontSizeProperty);
		}
		set
		{
			SetValue(BaseFontSizeProperty, value);
		}
	}

	protected abstract Canvas LayoutSelectionCanvas { get; }

	protected abstract Rectangle LayoutSelectionRectangle { get; }

	public bool IsRenamingItem { get; set; }

	public ListedItem? RenamingItem { get; set; }

	public bool IsItemSelected => SelectedItems.Count > 0;

	public List<ListedItem> SelectedItems
	{
		get
		{
			return _selectedItems;
		}
		set
		{
			if (_selectedItems != value)
			{
				_selectedItems = value;
				if (value.Count > 0)
				{
					SelectedItem = value.First();
				}
				else
				{
					SelectedItem = null;
				}
				WeakReferenceMessenger.Default.Send(new LayoutViewSelectedItemsChanged
				{
					tabItemViewModel = (DataContext as BaseLayoutViewModel).AssociatedInstance,
					selectedItems = SelectedItems
				});
			}
		}
	}

	public ListedItem SelectedItem { get; set; }

	public virtual void FocusItemList()
	{
		Focus();
	}

	protected abstract IEnumerable<(ListedItem Item, Control Container)> GetVisibleItemsAndContainers();

	protected abstract void ApplyMarqueeSelection(HashSet<ListedItem> itemsToSelect, HashSet<ListedItem> itemsToUnselect);

	public BaseLayoutView()
	{
		DragDrop.SetAllowDrop(this, value: true);
		AddHandler(DragDrop.DragOverEvent, OnLayoutDragOver, RoutingStrategies.Tunnel | RoutingStrategies.Bubble, handledEventsToo: true);
		AddHandler(DragDrop.DragLeaveEvent, OnLayoutDragLeave, RoutingStrategies.Tunnel | RoutingStrategies.Bubble, handledEventsToo: true);
		AddHandler(DragDrop.DropEvent, OnLayoutDrop, RoutingStrategies.Tunnel | RoutingStrategies.Bubble, handledEventsToo: true);
		AddHandler(InputElement.PointerPressedEvent, OnPreviewPointerPressed, RoutingStrategies.Tunnel);
		AddHandler(InputElement.PointerReleasedEvent, OnPreviewPointerReleased, RoutingStrategies.Tunnel);
		AddHandler(InputElement.TextInputEvent, OnLayoutTextInput, RoutingStrategies.Bubble, handledEventsToo: true);
		Loaded += (object? sender, RoutedEventArgs args) =>
		{
			(DataContext as BaseLayoutViewModel).LayoutView = this;
			HookEvents();
			RequestItemsPresentedNotification();
		};
		Unloaded += (object? sender, RoutedEventArgs args) =>
		{
			// Keep the layout view model alive so switching tabs does not reload the
			// directory from disk. It is disposed when the owning tab is closed.
			UnhookEvents();
			(DataContext as BaseLayoutViewModel)?.OnViewDetached(this);
		};
	}

	protected void HookEvents()
	{
		UnhookEvents();
		if (DataContext is BaseLayoutViewModel { _itemManipulationModel: var itemManipulationModel })
		{
			itemManipulationModel.StartRenameItemInvoked += ItemManipulationModel_StartRenameItemInvoked;
			itemManipulationModel.SelectAllItemsInvoked += ItemManipulationModel_SelectAllItemsInvoked;
			itemManipulationModel.ClearSelectionInvoked += ItemManipulationModel_ClearSelectionInvoked;
			itemManipulationModel.InvertSelectionInvoked += ItemManipulationModel_InvertSelectionInvoked;
		}
	}

	protected void UnhookEvents()
	{
		if (DataContext is BaseLayoutViewModel { _itemManipulationModel: not null, _itemManipulationModel: var itemManipulationModel })
		{
			itemManipulationModel.StartRenameItemInvoked -= ItemManipulationModel_StartRenameItemInvoked;
			itemManipulationModel.SelectAllItemsInvoked -= ItemManipulationModel_SelectAllItemsInvoked;
			itemManipulationModel.ClearSelectionInvoked -= ItemManipulationModel_ClearSelectionInvoked;
			itemManipulationModel.InvertSelectionInvoked -= ItemManipulationModel_InvertSelectionInvoked;
		}
	}

	internal void RequestItemsPresentedNotification()
	{
		Avalonia.Threading.Dispatcher.UIThread.Post(() =>
		{
			if (VisualRoot != null)
			{
				OnItemsPresented();
			}
		}, DispatcherPriority.Loaded);
	}

	public void Dispose()
	{
		UnhookEvents();
		RemoveHandler(DragDrop.DragOverEvent, OnLayoutDragOver);
		RemoveHandler(DragDrop.DragLeaveEvent, OnLayoutDragLeave);
		RemoveHandler(DragDrop.DropEvent, OnLayoutDrop);
		RemoveHandler(InputElement.TextInputEvent, OnLayoutTextInput);
	}

	protected void OpenItem()
	{
		Ioc.Default.GetRequiredService<ICommandManager>()[CommandCodes.OpenItem].ExecuteAsync();
	}

	protected void NavigateUp()
	{
		IRichCommand richCommand = Ioc.Default.GetRequiredService<ICommandManager>()[CommandCodes.NavigateUp];
		if (richCommand.IsExecutable)
		{
			richCommand.ExecuteAsync();
		}
	}

	protected SortOption GetCurrentSortOption()
	{
		return (DataContext as BaseLayoutViewModel)?.CurrentSortOption ?? SortOption.Name;
	}

	protected SortDirection GetCurrentSortDirection()
	{
		return (DataContext as BaseLayoutViewModel)?.CurrentSortDirection ?? SortDirection.Ascending;
	}

	protected bool HasCustomSortState()
	{
		BaseLayoutViewModel obj = DataContext as BaseLayoutViewModel;
		if (obj == null)
		{
			return false;
		}
		return obj.AssociatedInstance?.HasCustomSort == true;
	}

	protected void UpdateCurrentSortState(SortOption sortOption, SortDirection sortDirection)
	{
		(DataContext as BaseLayoutViewModel)?.UpdateSortState(sortOption, sortDirection);
	}

	protected ContextMenu CreateContextMenu(bool isItemContextMenu)
	{
		bool shiftPressed = MacPluginInterop.IsShiftKeyPressed();
		List<ContextMenuFlyoutItemViewModel> itemContextCommandsWithoutShellItems = ContentPageContextFlyoutFactory.GetItemContextCommandsWithoutShellItems((DataContext as BaseLayoutViewModel).AssociatedInstance, SelectedItems, DataContext as BaseLayoutViewModel, shiftPressed, isItemContextMenu);
		ContextMenu contextMenu = new ContextMenu();
		contextMenu.ItemsSource = ContextFlyoutModelToElementHelper.ToContextMenuItemsSource(itemContextCommandsWithoutShellItems);
		List<ContextMenuFlyoutItemViewModel> hotKeyBoundItems = GetHotKeyBoundMenuItems(itemContextCommandsWithoutShellItems).ToList();
		bool keyReleased;
		if (hotKeyBoundItems.Count > 0)
		{
			keyReleased = true;
			contextMenu.AddHandler(InputElement.KeyDownEvent, OnContextMenuKeyDown, RoutingStrategies.Tunnel);
			contextMenu.AddHandler(InputElement.KeyUpEvent, OnContextMenuKeyUp, RoutingStrategies.Tunnel);
		}
		return contextMenu;
		async void OnContextMenuKeyDown(object? sender, KeyEventArgs e)
		{
			if (keyReleased && contextMenu.IsOpen)
			{
				Key key = e.Key;
				if ((uint)(key - 70) > 1u && (uint)(key - 116) > 5u)
				{
					ContextMenuFlyoutItemViewModel contextMenuFlyoutItemViewModel = hotKeyBoundItems.FirstOrDefault((ContextMenuFlyoutItemViewModel item) => item.InputGesture?.Matches(e) ?? false);
					if (contextMenuFlyoutItemViewModel != null && CanExecuteMenuCommand(contextMenuFlyoutItemViewModel))
					{
						keyReleased = false;
						e.Handled = true;
						contextMenu.Close();
						await ExecuteMenuCommandAsync(contextMenuFlyoutItemViewModel);
					}
				}
			}
		}
		void OnContextMenuKeyUp(object? sender, KeyEventArgs e)
		{
			keyReleased = true;
		}
	}

	private static IEnumerable<ContextMenuFlyoutItemViewModel> GetHotKeyBoundMenuItems(IEnumerable<ContextMenuFlyoutItemViewModel> items)
	{
		foreach (ContextMenuFlyoutItemViewModel item in items)
		{
			if (!item.ShowItem)
			{
				continue;
			}
			if (item.ItemType != ContextMenuFlyoutItemType.Separator && item.Command != null && (object)item.InputGesture != null)
			{
				yield return item;
			}
			List<ContextMenuFlyoutItemViewModel> items2 = item.Items;
			if (items2 == null || items2.Count <= 0)
			{
				continue;
			}
			foreach (ContextMenuFlyoutItemViewModel hotKeyBoundMenuItem in GetHotKeyBoundMenuItems(item.Items))
			{
				yield return hotKeyBoundMenuItem;
			}
		}
	}

	private static bool CanExecuteMenuCommand(ContextMenuFlyoutItemViewModel item)
	{
		ICommand command = item.Command;
		if (!(command is IRichCommand { IsExecutable: var isExecutable }))
		{
			return command?.CanExecute(item.CommandParameter) ?? false;
		}
		return isExecutable;
	}

	private static Task ExecuteMenuCommandAsync(ContextMenuFlyoutItemViewModel item)
	{
		ICommand command = item.Command;
		if (!(command is IRichCommand richCommand))
		{
			if (command != null)
			{
				return ExecuteCommandAsync(command, item.CommandParameter);
			}
			return Task.CompletedTask;
		}
		return richCommand.ExecuteAsync(item.CommandParameter);
	}

	private static Task ExecuteCommandAsync(ICommand command, object? parameter)
	{
		command.Execute(parameter);
		return Task.CompletedTask;
	}

	protected void FocusCurrentModernShellView()
	{
		(DataContext as BaseLayoutViewModel)?.AssociatedInstance?.ParentShellViewModel?.View?.Focus();
	}

	protected virtual void ItemManipulationModel_StartRenameItemInvoked(object? sender, EventArgs e)
	{
		StartRenameItem();
	}

	public virtual void StartRenameItem()
	{
	}

	protected virtual void ItemManipulationModel_SelectAllItemsInvoked(object? sender, EventArgs e)
	{
		SelectAllItems();
	}

	public virtual void SelectAllItems()
	{
	}

	protected virtual void ItemManipulationModel_ClearSelectionInvoked(object? sender, EventArgs e)
	{
		ClearSelection();
	}

	public virtual void ClearSelection()
	{
	}

	protected virtual void ItemManipulationModel_InvertSelectionInvoked(object? sender, EventArgs e)
	{
		InvertSelection();
	}

	public virtual void InvertSelection()
	{
	}

	protected virtual void OnItemsPresented()
	{
		SelectPreviousFolderAfterNavigatingUp();
		RestoreSelectionFromAssociatedInstance();
	}

	private void SelectPreviousFolderAfterNavigatingUp()
	{
		if (!(DataContext is BaseLayoutViewModel { AssociatedInstance: var associatedInstance } baseLayoutViewModel))
		{
			return;
		}
		BaseShellViewModel baseShellViewModel = associatedInstance?.ParentShellViewModel;
		if (baseShellViewModel == null || Ioc.Default.GetRequiredService<IMultiPanesContext>().ActivePaneViewModel != baseShellViewModel)
		{
			return;
		}
		string text = baseShellViewModel.ConsumePreviousDirectory();
		if (string.IsNullOrWhiteSpace(text) || !Ioc.Default.GetRequiredService<IFoldersSettingsService>().ScrollToPreviousFolderWhenNavigatingUp)
		{
			return;
		}
		string parentPath = PathHelpers.NormalizeDirectoryPath(baseShellViewModel.WorkingDirectory);
		string folderPathToSelect = PathHelpers.GetFirstChildPathUnderParent(text, parentPath);
		if (!string.IsNullOrWhiteSpace(folderPathToSelect))
		{
			ListedItem listedItem = baseLayoutViewModel.Items?.FirstOrDefault((ListedItem item) => string.Equals(PathHelpers.NormalizeDirectoryPath(item.ItemPath), folderPathToSelect, StringComparison.Ordinal));
			if (listedItem != null)
			{
				SelectItemInView(listedItem);
				SelectedItem = listedItem;
			}
		}
	}

	private void RestoreSelectionFromAssociatedInstance()
	{
		if (!(DataContext is BaseLayoutViewModel baseLayoutViewModel))
		{
			return;
		}
		List<ListedItem> list = baseLayoutViewModel.PendingSelectedItems ?? baseLayoutViewModel.AssociatedInstance?.SelectedItems?.ToList();
		baseLayoutViewModel.PendingSelectedItems = null;
		if (list == null || list.Count == 0)
		{
			SelectItemsInView(Array.Empty<ListedItem>());
			SelectedItems = new List<ListedItem>();
			return;
		}
		Dictionary<string, ListedItem> itemsByPath = baseLayoutViewModel.Items.Where((ListedItem item) => !string.IsNullOrWhiteSpace(item.ItemPath)).GroupBy((ListedItem item) => PathHelpers.NormalizeDirectoryPath(item.ItemPath), StringComparer.Ordinal).ToDictionary((IGrouping<string, ListedItem> group) => group.Key, (IGrouping<string, ListedItem> group) => group.First(), StringComparer.Ordinal);
		List<ListedItem> list2 = (from path in (from item in list
				select PathHelpers.NormalizeDirectoryPath(item.ItemPath) into path
				where !string.IsNullOrWhiteSpace(path)
				select path).Distinct(StringComparer.Ordinal)
			select (!itemsByPath.TryGetValue(path, out var value)) ? null : value).OfType<ListedItem>().ToList();
		SelectItemsInView(list2);
		SelectedItems = list2;
	}

	protected void BeginInlineRenameInContainer(Control container)
	{
		if (RenamingItem != null && container != null)
		{
			TextBox textBox = AvaloniaUIHelpers.FindVisualChildByName<TextBox>(container, "TextBoxItemName");
			TextBlock textBlock = AvaloniaUIHelpers.FindVisualChildByName<TextBlock>(container, "ItemName");
			if (textBox != null && textBlock != null)
			{
				int num = RenamingItem.FileExtension?.Length ?? 0;
				int selectionEnd = Math.Max(0, RenamingItem.Name.Length - num);
				textBox.Text = RenamingItem.Name;
				textBox.SelectionStart = 0;
				textBox.SelectionEnd = selectionEnd;
				textBlock.IsVisible = true;
				textBlock.IsVisible = false;
				textBox.IsVisible = true;
				IsRenamingItem = true;
				textBox.Focus();
			}
		}
	}

	protected virtual async Task CommitRenameAsync(TextBox textBox)
	{
		if (!_isCommittingRename)
		{
			_isCommittingRename = true;
			EndRename(textBox);
			string newName = textBox.Text.Trim();
			BaseLayoutViewModel baseLayoutViewModel = DataContext as BaseLayoutViewModel;
			if (RenamingItem != null && baseLayoutViewModel?.AssociatedInstance?.ParentShellViewModel?.View != null)
			{
				await UIFilesystemHelpers.RenameFileItemAsync(RenamingItem, newName, baseLayoutViewModel.AssociatedInstance.ParentShellViewModel.View);
			}
			_isCommittingRename = false;
		}
	}

	protected async void RenameTextBox_OnLostFocus(object? sender, RoutedEventArgs e)
	{
		if (sender is TextBox textBox)
		{
			await CommitRenameAsync(textBox);
		}
	}

	protected virtual void SelectItemInView(ListedItem item)
	{
	}

	protected virtual void SelectItemsInView(IReadOnlyList<ListedItem> items)
	{
	}

	private void OnLayoutTextInput(object? sender, TextInputEventArgs e)
	{
		if (!IsRenamingItem && !IsEditableTextInputSource(e.Source) && !string.IsNullOrWhiteSpace(e.Text) && SelectItemByKeyboardSearchText(e.Text))
		{
			e.Handled = true;
		}
	}

	private bool SelectItemByKeyboardSearchText(string text)
	{
		BaseLayoutViewModel obj = DataContext as BaseLayoutViewModel;
		object obj2;
		if (obj == null)
		{
			obj2 = null;
		}
		else
		{
			ObservableCollection<ListedItem> items = obj.Items;
			obj2 = ((items != null) ? items.OfType<ListedItem>().ToList() : null);
		}
		List<ListedItem> list = (List<ListedItem>)obj2;
		if (list == null || list.Count == 0)
		{
			return false;
		}
		string text2 = text.ToLower();
		if (DateTime.Now.Ticks - _lastKeyboardSearchTimeTick > 5000000)
		{
			_keyboardSearchText = string.Empty;
			_keyboardSearchRawInput = string.Empty;
		}
		_lastKeyboardSearchTimeTick = DateTime.Now.Ticks;
		_keyboardSearchRawInput += text2;
		if (_keyboardSearchText.Length == 1)
		{
			if (text2 != _keyboardSearchText)
			{
				_keyboardSearchText = _keyboardSearchRawInput;
			}
		}
		else
		{
			_keyboardSearchText += text2;
		}
		ListedItem listedItem = FindKeyboardSearchMatch(list);
		if (listedItem == null)
		{
			return false;
		}
		SelectItemInView(listedItem);
		SelectedItem = listedItem;
		return true;
	}

	private ListedItem? FindKeyboardSearchMatch(List<ListedItem> items)
	{
		if (SelectedItem == null)
		{
			return items.FirstOrDefault(ItemNameStartsWithKeyboardSearchText);
		}
		int num = items.IndexOf(SelectedItem);
		if (num < 0)
		{
			return items.FirstOrDefault(ItemNameStartsWithKeyboardSearchText);
		}
		num = (num + 1) % items.Count;
		return items.Skip(num).FirstOrDefault(ItemNameStartsWithKeyboardSearchText) ?? items.Take(num).FirstOrDefault(ItemNameStartsWithKeyboardSearchText);
	}

	private bool ItemNameStartsWithKeyboardSearchText(ListedItem item)
	{
		return item.Name.ToLower().StartsWith(_keyboardSearchText);
	}

	private static bool IsEditableTextInputSource(object? source)
	{
		for (Visual visual = source as Visual; visual != null; visual = visual.GetVisualParent())
		{
			if (visual is TextBox)
			{
				return true;
			}
		}
		return false;
	}

	protected void NavigateToAdjacentItem(bool goNext)
	{
		if (IsRenamingItem)
		{
			return;
		}
		BaseLayoutViewModel baseLayoutViewModel = DataContext as BaseLayoutViewModel;
		ListedItem selectedItem = SelectedItem;
		if (baseLayoutViewModel?.Items == null)
		{
			return;
		}
		List<ListedItem> list = baseLayoutViewModel.Items.OfType<ListedItem>().ToList();
		int num = 0;
		if (selectedItem != null)
		{
			num = list.IndexOf(selectedItem);
			if (num != -1 && list.Count > 0)
			{
				num = (num + (goNext ? 1 : (-1)) + list.Count) % list.Count;
			}
		}
		else
		{
			num = (goNext ? (list.Count - 1) : 0);
		}
		ListedItem listedItem = list[num];
		SelectItemInView(listedItem);
		SelectedItem = listedItem;
	}

	protected async void RenameTextBox_OnKeyDown(object? sender, KeyEventArgs e)
	{
		if (!(sender is TextBox textBox))
		{
			return;
		}
		bool flag = MacPluginInterop.IsShiftKeyPressed();
		switch (e.Key)
		{
		case Key.Escape:
			textBox.Text = RenamingItem?.Name;
			EndRename(textBox);
			e.Handled = true;
			break;
		case Key.Return:
			await CommitRenameAsync(textBox);
			e.Handled = true;
			break;
		case Key.Up:
			if (!flag)
			{
				textBox.SelectionStart = 0;
				textBox.SelectionEnd = 0;
			}
			e.Handled = true;
			break;
		case Key.Down:
			if (!flag)
			{
				textBox.SelectionStart = textBox.Text.Length;
				textBox.SelectionEnd = textBox.Text.Length;
			}
			e.Handled = true;
			break;
		case Key.Left:
			e.Handled = true;
			break;
		case Key.Right:
			e.Handled = true;
			break;
		case Key.Tab:
			e.Handled = true;
			NavigateToAdjacentForRenameAsync(textBox, flag);
			break;
		}
	}

	private async Task NavigateToAdjacentForRenameAsync(TextBox textBox, bool isShiftPressed)
	{
		await CommitRenameAsync(textBox);
		BaseLayoutViewModel baseLayoutViewModel = DataContext as BaseLayoutViewModel;
		ListedItem renamingItem = RenamingItem;
		if (baseLayoutViewModel?.Items != null && renamingItem != null)
		{
			List<ListedItem> list = baseLayoutViewModel.Items.OfType<ListedItem>().ToList();
			int num = list.IndexOf(renamingItem);
			if (num != -1 && list.Count > 0)
			{
				num = (num + ((!isShiftPressed) ? 1 : (-1)) + list.Count) % list.Count;
				ListedItem listedItem = list[num];
				SelectItemInView(listedItem);
				SelectedItem = listedItem;
				StartRenameItem();
			}
		}
	}

	protected void EndRename(TextBox textBox)
	{
		IsRenamingItem = false;
		textBox.IsVisible = false;
		textBox.SelectionStart = 0;
		Grid grid = textBox.FindAncestorOfType<Grid>();
		if (grid != null)
		{
			TextBlock textBlock = AvaloniaUIHelpers.FindVisualChildByName<TextBlock>(grid, "ItemName");
			if (textBlock != null)
			{
				textBlock.IsVisible = true;
			}
		}
	}

	protected ListedItem? GetItemFromSource(object? source)
	{
		for (Visual visual = source as Visual; visual != null; visual = visual.GetVisualParent())
		{
			if (visual?.DataContext is ListedItem result)
			{
				return result;
			}
		}
		return null;
	}

	protected bool ShouldClearSelectionOnEmptyAreaPressed(PointerPressedEventArgs e)
	{
		if (!e.GetCurrentPoint(this).Properties.IsLeftButtonPressed || SelectedItems.Count == 0)
		{
			return false;
		}
		if (e.KeyModifiers != KeyModifiers.None)
		{
			return false;
		}
		if (AvaloniaUIHelpers.IsDataGridColumnHeader(e.Source) || AvaloniaUIHelpers.IsScrollBar(e.Source))
		{
			return false;
		}
		return GetItemFromSource(e.Source) == null;
	}

	protected bool ShouldNavigateUpOnEmptyAreaDoubleTapped(TappedEventArgs e)
	{
		if (!Ioc.Default.GetRequiredService<IFoldersSettingsService>().DoubleClickToGoUp)
		{
			return false;
		}
		if (e.KeyModifiers != KeyModifiers.None)
		{
			return false;
		}
		if (AvaloniaUIHelpers.IsDataGridColumnHeader(e.Source) || AvaloniaUIHelpers.IsScrollBar(e.Source))
		{
			return false;
		}
		return GetItemFromSource(e.Source) == null;
	}

	private Control? GetItemContainerFromSource(object? source)
	{
		for (Visual visual = source as Visual; visual != null; visual = visual.GetVisualParent())
		{
			if (visual is DataGridRow || visual is ListBoxItem)
			{
				return visual as Control;
			}
		}
		return null;
	}

	protected virtual void OnItemPointerPressed(object? sender, PointerPressedEventArgs e)
	{
		if (!AvaloniaUIHelpers.IsDataGridColumnHeader(e.Source) && e.GetCurrentPoint(this).Properties.IsLeftButtonPressed && SelectedItems.Count > 0)
		{
			_dragStartPoint = e.GetPosition(this);
			_dragStartEvent = e;
			_isDragging = false;
		}
	}

	protected virtual async void OnItemPointerMoved(object? sender, PointerEventArgs e)
	{
		if (!AvaloniaUIHelpers.IsDataGridColumnHeader(e.Source) && e.GetCurrentPoint(this).Properties.IsLeftButtonPressed && !_isDragging && SelectedItems.Count > 0)
		{
			Point position = e.GetPosition(this);
			if (Math.Sqrt(Math.Pow(position.X - _dragStartPoint.X, 2.0) + Math.Pow(position.Y - _dragStartPoint.Y, 2.0)) > 80.0 && !AvaloniaUIHelpers.IsScrollBar(e.Source))
			{
				_isDragging = true;
				await StartDragOperation();
			}
		}
	}

	private async Task StartDragOperation()
	{
		_hasPerformedDrag = true;
		if (SelectedItems.Count == 0 || _dragStartEvent == null)
		{
			_isDragging = false;
			return;
		}
		_currentDragData = new LayoutDragData
		{
			SourceLayoutView = this,
			Items = SelectedItems.ToList(),
			Operation = DragDropEffects.Move
		};
		LayoutDragData.ActiveDrag = _currentDragData;
		DataTransfer dragData = new DataTransfer();
		IStorageProvider storageProvider = TopLevel.GetTopLevel(this)?.StorageProvider;
		if (storageProvider != null)
		{
			foreach (ListedItem selectedItem in SelectedItems)
			{
				IStorageItem storageItem = await TryGetStorageItemForDragAsync(storageProvider, selectedItem);
				if (storageItem != null)
				{
					dragData.Add(DataTransferItem.CreateFile(storageItem));
				}
			}
		}
		await DragDrop.DoDragDropAsync(_dragStartEvent, dragData, DragDropEffects.Move);
		LayoutDragData.ActiveDrag = null;
		_currentDragData = null;
		_dragStartEvent = null;
		_isDragging = false;
	}

	protected void HandleMarqueePointerPressed(PointerPressedEventArgs e)
	{
		if (e.GetCurrentPoint(this).Properties.IsLeftButtonPressed && GetItemFromSource(e.Source) == null && !AvaloniaUIHelpers.IsScrollBar(e.Source) && !AvaloniaUIHelpers.IsDataGridColumnHeader(e.Source))
		{
			_marqueeIsToggleMode = e.KeyModifiers.HasFlag(KeyModifiers.Control) || e.KeyModifiers.HasFlag(KeyModifiers.Meta);
			if (!_marqueeIsToggleMode)
			{
				ClearSelection();
				_preMarqueeSelectedItems.Clear();
			}
			else
			{
				_preMarqueeSelectedItems = SelectedItems.ToList();
			}
			_isMarqueeSelecting = true;
			_marqueeStartPoint = e.GetPosition(LayoutSelectionCanvas);
			LayoutSelectionRectangle.Width = 0.0;
			LayoutSelectionRectangle.Height = 0.0;
			Canvas.SetLeft(LayoutSelectionRectangle, _marqueeStartPoint.X);
			Canvas.SetTop(LayoutSelectionRectangle, _marqueeStartPoint.Y);
			LayoutSelectionRectangle.IsVisible = true;
			if (e.Source is IInputElement control)
			{
				e.Pointer.Capture(control);
			}
			e.Handled = true;
		}
	}

	protected void HandleMarqueePointerMoved(PointerEventArgs e)
	{
		if (_isMarqueeSelecting)
		{
			Point position = e.GetPosition(LayoutSelectionCanvas);
			double num = Math.Min(_marqueeStartPoint.X, position.X);
			double num2 = Math.Min(_marqueeStartPoint.Y, position.Y);
			double width = Math.Abs(position.X - _marqueeStartPoint.X);
			double height = Math.Abs(position.Y - _marqueeStartPoint.Y);
			Canvas.SetLeft(LayoutSelectionRectangle, num);
			Canvas.SetTop(LayoutSelectionRectangle, num2);
			LayoutSelectionRectangle.Width = width;
			LayoutSelectionRectangle.Height = height;
			UpdateMarqueeSelection(new Rect(num, num2, width, height));
			e.Handled = true;
		}
	}

	protected void HandleMarqueePointerReleased(PointerReleasedEventArgs e)
	{
		if (_isMarqueeSelecting)
		{
			_isMarqueeSelecting = false;
			LayoutSelectionRectangle.IsVisible = false;
			e.Pointer.Capture(null);
			e.Handled = true;
		}
	}

	private void UpdateMarqueeSelection(Rect marqueeRect)
	{
		HashSet<ListedItem> hashSet = new HashSet<ListedItem>();
		if (_marqueeIsToggleMode)
		{
			foreach (ListedItem preMarqueeSelectedItem in _preMarqueeSelectedItems)
			{
				hashSet.Add(preMarqueeSelectedItem);
			}
		}
		foreach (var visibleItemsAndContainer in GetVisibleItemsAndContainers())
		{
			ListedItem item = visibleItemsAndContainer.Item;
			Control item2 = visibleItemsAndContainer.Container;
			Matrix? matrix = item2.TransformToVisual(LayoutSelectionCanvas);
			if (!matrix.HasValue)
			{
				continue;
			}
			Rect rect = new Rect(item2.Bounds.Size).TransformToAABB(matrix.Value);
			if (!marqueeRect.Intersects(rect))
			{
				continue;
			}
			if (_marqueeIsToggleMode)
			{
				if (_preMarqueeSelectedItems.Contains(item))
				{
					hashSet.Remove(item);
				}
				else
				{
					hashSet.Add(item);
				}
			}
			else
			{
				hashSet.Add(item);
			}
		}
		HashSet<ListedItem> hashSet2 = SelectedItems.ToHashSet();
		HashSet<ListedItem> itemsToSelect = hashSet.Except(hashSet2).ToHashSet();
		HashSet<ListedItem> itemsToUnselect = hashSet2.Except(hashSet).ToHashSet();
		ApplyMarqueeSelection(itemsToSelect, itemsToUnselect);
	}

	private static async Task<IStorageItem?> TryGetStorageItemForDragAsync(IStorageProvider storageProvider, ListedItem item)
	{
		if (Directory.Exists(item.ItemPath))
		{
			return await storageProvider.TryGetFolderFromPathAsync(item.ItemPath);
		}
		if (File.Exists(item.ItemPath))
		{
			return await storageProvider.TryGetFileFromPathAsync(item.ItemPath);
		}
		return null;
	}

	private void OnLayoutDragOver(object? sender, DragEventArgs e)
	{
		IDataTransfer dataTransfer = e.DataTransfer;
		if (dataTransfer == null)
		{
			return;
		}
		e.DragEffects = DragDropEffects.None;
		ListedItem itemFromSource = GetItemFromSource(e.Source);
		Control itemContainerFromSource = GetItemContainerFromSource(e.Source);
		if (_dragOverItem != itemFromSource)
		{
			ClearDragOverItemVisual();
			_dragOverItem = null;
			_dragOverItemContainer = null;
		}
		if (!CanAcceptDragData(dataTransfer, itemFromSource))
		{
			return;
		}
		e.DragEffects = DragDropEffects.Move;
		if (itemFromSource != null && itemFromSource.IsFolder)
		{
			_dragOverItem = itemFromSource;
			_dragOverItemContainer = itemContainerFromSource;
			if (_dragOverItemContainer != null)
			{
				_dragOverItemContainer.Opacity = 0.7;
			}
		}
		else if (!_isDragOver)
		{
			_isDragOver = true;
			Opacity = 0.8;
		}
	}

	private void OnLayoutDragLeave(object? sender, DragEventArgs e)
	{
		ClearDragOverItemVisual();
		_dragOverItem = null;
		_dragOverItemContainer = null;
		if (_isDragOver)
		{
			_isDragOver = false;
			Opacity = 1.0;
		}
	}

	private void ClearDragOverItemVisual()
	{
		if (_dragOverItemContainer != null)
		{
			_dragOverItemContainer.Opacity = 1.0;
		}
	}

	private async void OnLayoutDrop(object? sender, DragEventArgs e)
	{
		IDataTransfer dataTransfer = e.DataTransfer;
		if (dataTransfer != null)
		{
			ListedItem dragOverItem = _dragOverItem;
			ClearDragOverItemVisual();
			_dragOverItem = null;
			_dragOverItemContainer = null;
			if (_isDragOver)
			{
				_isDragOver = false;
				Opacity = 1.0;
			}
			if (CanAcceptDragData(dataTransfer, dragOverItem))
			{
				await HandleDropOperation(dataTransfer, dragOverItem);
			}
		}
	}

	private LayoutDragData? FindInternalDragData()
	{
		if (_currentDragData != null)
		{
			return _currentDragData;
		}
		return LayoutDragData.ActiveDrag;
	}

	private bool CanAcceptDragData(IDataTransfer data, ListedItem? targetItem = null)
	{
		LayoutDragData layoutDragData = FindInternalDragData();
		if (layoutDragData != null)
		{
			if (layoutDragData.SourceLayoutView == this)
			{
				if (targetItem != null && targetItem.IsFolder)
				{
					if (layoutDragData.Items.Any((ListedItem item) => item.ItemPath == targetItem.ItemPath))
					{
						return false;
					}
					return !layoutDragData.Items.Any((ListedItem item) => item.IsFolder && PathHelpers.IsSubPathOf(targetItem.ItemPath, item.ItemPath));
				}
				return false;
			}
			return true;
		}
		if (data.Contains(DataFormat.File))
		{
			return true;
		}
		return false;
	}

	private async Task HandleDropOperation(IDataTransfer data, ListedItem? targetItem = null)
	{
		try
		{
			BaseLayoutViewModel baseLayoutViewModel = DataContext as BaseLayoutViewModel;
			string text = ((targetItem != null && targetItem.IsFolder) ? targetItem.ItemPath : baseLayoutViewModel?.NavParams?.NavPath);
			if (string.IsNullOrEmpty(text))
			{
				return;
			}
			FilesystemHelpers filesystemHelpers = baseLayoutViewModel?.AssociatedInstance?.ParentShellViewModel?.View?.FilesystemHelpers;
			if (filesystemHelpers == null)
			{
				return;
			}
			LayoutDragData layoutDragData = FindInternalDragData();
			if (layoutDragData != null)
			{
				if (layoutDragData.Items != null && (targetItem == null || !targetItem.IsFolder || !layoutDragData.Items.Any((ListedItem item) => item.IsFolder && PathHelpers.IsSubPathOf(targetItem.ItemPath, item.ItemPath))))
				{
					await filesystemHelpers.MoveItemsAsync(layoutDragData.Items, text, NameCollisionOption.GenerateUniqueName, showDialog: true);
				}
			}
			else if (data.Contains(DataFormat.File))
			{
				IStorageItem[] array = data.TryGetFiles();
				if (array != null)
				{
					await filesystemHelpers.MoveItemsAsync(array, text, NameCollisionOption.GenerateUniqueName, showDialog: true);
				}
			}
		}
		catch (Exception)
		{
		}
	}

	private void OnPreviewPointerPressed(object? sender, PointerPressedEventArgs e)
	{
		_hasPerformedDrag = false;
		_suppressedSelectionEvent = false;
		_suppressedItem = null;
		if (AvaloniaUIHelpers.IsDataGridColumnHeader(e.Source) || AvaloniaUIHelpers.IsScrollBar(e.Source))
		{
			return;
		}
		PointerPoint currentPoint = e.GetCurrentPoint(this);
		if (currentPoint.Properties.IsMiddleButtonPressed)
		{
			ListedItem itemFromSource = GetItemFromSource(e.Source);
			if (itemFromSource != null && itemFromSource.IsFolder)
			{
				NavigationHelpers.AddNewTabByPathAsync(itemFromSource.ItemPath, switchToNewTab: false);
				e.Handled = true;
			}
		}
		else if (currentPoint.Properties.IsLeftButtonPressed && SelectedItems.Count > 1)
		{
			ListedItem itemFromSource2 = GetItemFromSource(e.Source);
			if (itemFromSource2 != null && SelectedItems.Contains(itemFromSource2) && e.KeyModifiers == KeyModifiers.None)
			{
				_suppressedSelectionEvent = true;
				_suppressedItem = itemFromSource2;
				e.Handled = true;
			}
		}
	}

	private void OnPreviewPointerReleased(object? sender, PointerReleasedEventArgs e)
	{
		if (_suppressedSelectionEvent && !_hasPerformedDrag && _suppressedItem != null)
		{
			SelectItemInView(_suppressedItem);
		}
		_suppressedSelectionEvent = false;
		_suppressedItem = null;
		_isDragging = false;
		_dragStartEvent = null;
	}
}
