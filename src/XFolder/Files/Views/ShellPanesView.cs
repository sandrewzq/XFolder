using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Markup.Xaml.MarkupExtensions;
using Avalonia.Markup.Xaml.XamlIl.Runtime;
using Avalonia.Styling;
using Avalonia.Threading;
using Avalonia.VisualTree;
using CommunityToolkit.Mvvm.DependencyInjection;
using CompiledAvaloniaXaml;
using Files.Data.Contexts;
using Files.Data.Contracts;
using Files.Data.Enums;
using Files.Data.Models;
using Files.Helpers;
using Files.ViewModels;
using Files.ViewModels.Items;
using Files.ViewModels.Shells;
using Files.Views.Shells;

namespace Files.Views;

public class ShellPanesView : UserControl, IShellPanesView, IDisposable, INotifyPropertyChanged
{
	private sealed class ThemeVariantObserver(Action onNext) : IObserver<ThemeVariant>
	{
		private readonly Action _onNext = onNext;

		public void OnCompleted()
		{
		}

		public void OnError(Exception error)
		{
		}

		public void OnNext(ThemeVariant value)
		{
			_onNext();
		}
	}

	private IDisposable? _themeVariantSubscription;

	private readonly IMultiPanesContext multiPanesContext = Ioc.Default.GetRequiredService<IMultiPanesContext>();

	private readonly IGeneralSettingsService generalSettingsService = Ioc.Default.GetRequiredService<IGeneralSettingsService>();

	private readonly ISessionService sessionService = Ioc.Default.GetRequiredService<ISessionService>();

	private SessionModel? sessionCache;

	private List<BaseShellViewModel> shellViewModels = new List<BaseShellViewModel>();

	private readonly HashSet<GridSplitter> draggingSplitters = new HashSet<GridSplitter>();

	private int showPanesCount;

	private int currentShowPanesIndex;

	private IShellView? _ActivePane;

	private BaseShellViewModel? _ActivePaneViewModel;

	private ShellPaneArrangement _ShellPaneArrangement;

	[GeneratedCode("Avalonia.Generators.NameGenerator.InitializeComponentCodeGenerator", "12.0.1.0")]
	internal Grid RootGrid;

	[CompilerGenerated]
	private static Action<object> _0021XamlIlPopulateOverride;

	public IShellView? ActivePane
	{
		get
		{
			return _ActivePane;
		}
		set
		{
			if (_ActivePane == value)
			{
				return;
			}
			_ActivePane = value;
			foreach (BaseShellViewModel shellViewModel in shellViewModels)
			{
				if (shellViewModel is ModernShellViewModel modernShellViewModel)
				{
					modernShellViewModel.IsCurrentInstance = false;
				}
			}
			NotifyPropertyChanged("ActivePane");
		}
	}

	public BaseShellViewModel? ActivePaneViewModel
	{
		get
		{
			return _ActivePaneViewModel;
		}
		set
		{
			if (_ActivePaneViewModel != value)
			{
				_ActivePaneViewModel = value;
				NotifyPropertyChanged("ActivePaneViewModel");
			}
		}
	}

	public ShellPaneArrangement ShellPaneArrangement
	{
		get
		{
			return _ShellPaneArrangement;
		}
		set
		{
			if (_ShellPaneArrangement != value)
			{
				_ShellPaneArrangement = value;
				NotifyPropertyChanged("ShellPaneArrangement");
			}
		}
	}

	public new event PropertyChangedEventHandler? PropertyChanged;

	public ShellPanesView()
	{
		InitializeComponent();
		_themeVariantSubscription = Application.Current?.GetObservable(Application.ActualThemeVariantProperty).Subscribe(new ThemeVariantObserver(RefreshPaneCurrentThemeVariant));
		DetachedFromVisualTree += (object? _, VisualTreeAttachmentEventArgs _) =>
		{
			_themeVariantSubscription?.Dispose();
			_themeVariantSubscription = null;
		};
		multiPanesContext.UpdateShellPanesView(this);
		if (generalSettingsService.ContinueLastSessionOnStartUp)
		{
			LoadState();
			return;
		}
		sessionCache = null;
		ArrangePanes(ShellPaneArrangement.Single);
	}

	private MainWindowViewModel? GetMainWindowViewModel()
	{
		TopLevel topLevel = TopLevel.GetTopLevel(this);
		if (topLevel is Window && topLevel.DataContext is MainWindowViewModel result)
		{
			return result;
		}
		if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime classicDesktopStyleApplicationLifetime)
		{
			Window mainWindow = classicDesktopStyleApplicationLifetime.MainWindow;
			if (mainWindow != null && mainWindow.DataContext is MainWindowViewModel result2)
			{
				return result2;
			}
		}
		return null;
	}

	private void Pane_GotFocus(object sender, RoutedEventArgs e)
	{
		if (!(sender is ModernShellView modernShellView))
		{
			return;
		}
		for (int i = 0; i < shellViewModels.Count; i++)
		{
			if (shellViewModels[i].View != null)
			{
				if (shellViewModels[i].View == modernShellView)
				{
					ActivePaneByIndex(i);
				}
				else
				{
					UnactivePaneByIndex(i);
				}
			}
		}
	}

	private void UnactivePaneByIndex(int index)
	{
		if (shellViewModels[index] is ModernShellViewModel modernShellViewModel)
		{
			modernShellViewModel.IsCurrentInstance = false;
		}
	}

	private void ActivePaneByIndex(int i)
	{
		ActivePaneViewModel = shellViewModels[i];
		ActivePane = shellViewModels[i].View;
		if (shellViewModels[i] is ModernShellViewModel modernShellViewModel)
		{
			modernShellViewModel.IsCurrentInstance = true;
		}
		currentShowPanesIndex = i;
	}

	private void RefreshPaneCurrentThemeVariant()
	{
		ThemeVariant currentThemeVariant = Application.Current?.ActualThemeVariant;
		for (int i = 0; i < shellViewModels.Count; i++)
		{
			if (shellViewModels[i] is ModernShellViewModel modernShellViewModel)
			{
				modernShellViewModel.CurrentThemeVariant = currentThemeVariant;
			}
		}
	}

	public void ActivePrevPane()
	{
		UnactivePaneByIndex(currentShowPanesIndex);
		int i = (currentShowPanesIndex - 1 + showPanesCount) % showPanesCount;
		ActivePaneByIndex(i);
		FocusPaneContent(i);
	}

	public void ActiveNextPane()
	{
		UnactivePaneByIndex(currentShowPanesIndex);
		int i = (currentShowPanesIndex + 1) % showPanesCount;
		ActivePaneByIndex(i);
		FocusPaneContent(i);
	}

	private void FocusPaneContent(int index)
	{
		if (index < 0 || index >= shellViewModels.Count)
		{
			return;
		}
		Avalonia.Threading.Dispatcher.UIThread.Post(() =>
		{
			shellViewModels[index].SelectedTab?.LayoutViewModel?.LayoutView?.FocusItemList();
		});
	}

	public void ArrangePanes(ShellPaneArrangement arrangement)
	{
		if (ShellPaneArrangement != arrangement)
		{
			switch (arrangement)
			{
			case ShellPaneArrangement.Single:
				ArrangeResetLayout();
				break;
			case ShellPaneArrangement.Horizontal:
				ArrangeHorizontalLayout();
				break;
			case ShellPaneArrangement.Vertical:
				ArrangeVerticalLayout();
				break;
			case ShellPaneArrangement.Quad:
				ArrangeQuadLayout();
				break;
			case ShellPaneArrangement.LeftTwoRight:
				ArrangeLeftTwoRightLayout();
				break;
			case ShellPaneArrangement.TopTwoBottom:
				ArrangeTopTwoBottomLayout();
				break;
			case ShellPaneArrangement.ThreeColumns:
				ArrangeThreeColumnsLayout();
				break;
			case ShellPaneArrangement.ThreeRows:
				ArrangeThreeRowsLayout();
				break;
			case ShellPaneArrangement.ThreeColumnsWeighted:
				ArrangeThreeColumnsWeightedLayout();
				break;
			case ShellPaneArrangement.ThreeRowsWeighted:
				ArrangeThreeRowsWeightedLayout();
				break;
			case ShellPaneArrangement.FourColumns:
				ArrangeFourColumnsLayout();
				break;
			case ShellPaneArrangement.FourRows:
				ArrangeFourRowsLayout();
				break;
			case ShellPaneArrangement.TwoLeftRight:
				ArrangeTwoLeftRightLayout();
				break;
			case ShellPaneArrangement.TwoTopBottom:
				ArrangeTwoTopBottomLayout();
				break;
			}
			ApplySavedLayoutState(arrangement);
		}
		RefreshPaneHeaders();
	}

	private void RefreshPaneHeaders()
	{
		bool showPaneHeader = showPanesCount > 1;
		foreach (BaseShellViewModel shellViewModel in shellViewModels)
		{
			if (shellViewModel.View is ModernShellView shellView)
			{
				shellView.ShowPaneHeader = showPaneHeader;
			}
		}
	}

	private void ApplySavedLayoutState(ShellPaneArrangement arrangement)
	{
		if (sessionCache?.PaneLayoutStates == null)
		{
			return;
		}
		string key = arrangement.ToString();
		if (!sessionCache.PaneLayoutStates.TryGetValue(key, out PaneLayoutStateModel value) || RootGrid.Children.Count == 0 || !(RootGrid.Children[0] is Grid root))
		{
			return;
		}
		foreach (var (grid, key2) in EnumerateGridsWithSplitters(root))
		{
			if (!value.Grids.TryGetValue(key2, out GridStateModel value2))
			{
				continue;
			}
			if (value2.Columns != null && value2.Columns.Count == grid.ColumnDefinitions.Count)
			{
				for (int i = 0; i < grid.ColumnDefinitions.Count; i++)
				{
					grid.ColumnDefinitions[i].Width = new GridLength(value2.Columns[i].Value, value2.Columns[i].UnitType);
				}
			}
			if (value2.Rows != null && value2.Rows.Count == grid.RowDefinitions.Count)
			{
				for (int j = 0; j < grid.RowDefinitions.Count; j++)
				{
					grid.RowDefinitions[j].Height = new GridLength(value2.Rows[j].Value, value2.Rows[j].UnitType);
				}
			}
		}
	}

	private IEnumerable<(Grid Grid, string Key)> EnumerateGridsWithSplitters(Grid root)
	{
		Stack<(Grid grid, string key)> stack = new Stack<(Grid, string)>();
		stack.Push((root, "0"));
		while (stack.Count > 0)
		{
			(Grid, string) tuple = stack.Pop();
			Grid grid = tuple.Item1;
			string key = tuple.Item2;
			bool flag = false;
			foreach (Control child in grid.Children)
			{
				if (child is GridSplitter)
				{
					flag = true;
					break;
				}
			}
			if (flag)
			{
				yield return (Grid: grid, Key: key);
			}
			int num = 0;
			foreach (Control child2 in grid.Children)
			{
				if (child2 is Grid item)
				{
					stack.Push((item, $"{key}.{num}"));
					num++;
				}
			}
		}
	}

	private void CaptureCurrentLayoutState(SessionModel session)
	{
		if (RootGrid.Children.Count == 0 || !(RootGrid.Children[0] is Grid root))
		{
			return;
		}
		string key = ShellPaneArrangement.ToString();
		if (!session.PaneLayoutStates.TryGetValue(key, out PaneLayoutStateModel value))
		{
			value = new PaneLayoutStateModel();
			session.PaneLayoutStates[key] = value;
		}
		foreach (var item3 in EnumerateGridsWithSplitters(root))
		{
			Grid item = item3.Grid;
			string item2 = item3.Key;
			GridStateModel gridStateModel = new GridStateModel();
			foreach (ColumnDefinition columnDefinition in item.ColumnDefinitions)
			{
				gridStateModel.Columns.Add(new GridLengthStateModel
				{
					Value = columnDefinition.Width.Value,
					UnitType = columnDefinition.Width.GridUnitType
				});
			}
			foreach (RowDefinition rowDefinition in item.RowDefinitions)
			{
				gridStateModel.Rows.Add(new GridLengthStateModel
				{
					Value = rowDefinition.Height.Value,
					UnitType = rowDefinition.Height.GridUnitType
				});
			}
			value.Grids[item2] = gridStateModel;
		}
	}

	private void OnSplitterPointerReleased(object? sender, PointerReleasedEventArgs e)
	{
		SaveState();
	}

	private void OnSplitterPointerEntered(object? sender, PointerEventArgs e)
	{
		if (sender is GridSplitter splitter)
		{
			SetSplitterHighlight(splitter, isHighlighted: true);
		}
	}

	private void OnSplitterPointerExited(object? sender, PointerEventArgs e)
	{
		if (sender is GridSplitter gridSplitter && !draggingSplitters.Contains(gridSplitter))
		{
			SetSplitterHighlight(gridSplitter, isHighlighted: false);
		}
	}

	private void OnSplitterDragStarted(object? sender, VectorEventArgs e)
	{
		if (sender is GridSplitter gridSplitter)
		{
			draggingSplitters.Add(gridSplitter);
			SetSplitterHighlight(gridSplitter, isHighlighted: true);
		}
	}

	private void OnSplitterDragCompleted(object? sender, VectorEventArgs e)
	{
		if (sender is GridSplitter gridSplitter)
		{
			draggingSplitters.Remove(gridSplitter);
			SetSplitterHighlight(gridSplitter, gridSplitter.IsPointerOver);
		}
	}

	private static void SetSplitterHighlight(GridSplitter splitter, bool isHighlighted)
	{
		splitter[!TemplatedControl.BackgroundProperty] = new DynamicResourceExtension(isHighlighted ? "SystemAccentColorBrush" : "TopToolbarBackground");
	}

	private void PrepareShellViewModels(int count)
	{
		if (shellViewModels.Count < count)
		{
			int num = count - shellViewModels.Count;
			for (int i = 0; i < num; i++)
			{
				shellViewModels.Add(new ModernShellViewModel());
			}
		}
	}

	private void SetPaneData(TransitioningContentControl content, BaseShellViewModel shellViewModel, bool isSetActive)
	{
		content.Loaded += (object? sender, RoutedEventArgs args) =>
		{
			ModernShellView shellView = content.GetVisualDescendants().OfType<ModernShellView>().FirstOrDefault();
			if (shellView != null)
			{
				shellViewModel.View = shellView;
				shellView.PaneNumber = shellViewModels.IndexOf(shellViewModel) + 1;
				shellView.ShowPaneHeader = showPanesCount > 1;
				shellView.GotFocus -= Pane_GotFocus;
				shellView.GotFocus += Pane_GotFocus;
				if (isSetActive)
				{
					Avalonia.Threading.Dispatcher.UIThread.Post(() =>
					{
						ActivePaneViewModel = shellViewModel;
						ActivePane = shellView;
						shellView.Focus();
					});
				}
			}
		};
	}

	private void ArrangeResetLayout()
	{
		PrepareShellViewModels(1);
		TransitioningContentControl transitioningContentControl = new TransitioningContentControl
		{
			Content = shellViewModels[0]
		};
		RootGrid.Children.Clear();
		RootGrid.Children.Add(transitioningContentControl);
		SetPaneData(transitioningContentControl, shellViewModels[0], isSetActive: true);
		showPanesCount = 1;
		ShellPaneArrangement = ShellPaneArrangement.Single;
	}

	private void ArrangeHorizontalLayout()
	{
		PrepareShellViewModels(2);
		Grid grid = new Grid();
		grid.ColumnDefinitions.Add(new ColumnDefinition
		{
			Width = new GridLength(1.0, GridUnitType.Star)
		});
		grid.ColumnDefinitions.Add(new ColumnDefinition
		{
			Width = GridLength.Auto
		});
		grid.ColumnDefinitions.Add(new ColumnDefinition
		{
			Width = new GridLength(1.0, GridUnitType.Star)
		});
		TransitioningContentControl transitioningContentControl = new TransitioningContentControl
		{
			Content = shellViewModels[0]
		};
		Grid.SetColumn(transitioningContentControl, 0);
		GridSplitter gridSplitter = CreateVerticalSplitter();
		Grid.SetColumn(gridSplitter, 1);
		TransitioningContentControl transitioningContentControl2 = new TransitioningContentControl
		{
			Content = shellViewModels[1]
		};
		Grid.SetColumn(transitioningContentControl2, 2);
		grid.Children.Add(transitioningContentControl);
		grid.Children.Add(gridSplitter);
		grid.Children.Add(transitioningContentControl2);
		RootGrid.Children.Clear();
		RootGrid.Children.Add(grid);
		int num = ((currentShowPanesIndex < 2) ? currentShowPanesIndex : 0);
		SetPaneData(transitioningContentControl, shellViewModels[0], num == 0);
		SetPaneData(transitioningContentControl2, shellViewModels[1], 1 == num);
		showPanesCount = 2;
		ShellPaneArrangement = ShellPaneArrangement.Horizontal;
	}

	private void ArrangeVerticalLayout()
	{
		PrepareShellViewModels(2);
		Grid grid = new Grid();
		grid.RowDefinitions.Add(new RowDefinition
		{
			Height = new GridLength(1.0, GridUnitType.Star)
		});
		grid.RowDefinitions.Add(new RowDefinition
		{
			Height = GridLength.Auto
		});
		grid.RowDefinitions.Add(new RowDefinition
		{
			Height = new GridLength(1.0, GridUnitType.Star)
		});
		TransitioningContentControl transitioningContentControl = new TransitioningContentControl
		{
			Content = shellViewModels[0]
		};
		Grid.SetRow(transitioningContentControl, 0);
		GridSplitter gridSplitter = CreateHorizontalSplitter();
		Grid.SetRow(gridSplitter, 1);
		TransitioningContentControl transitioningContentControl2 = new TransitioningContentControl
		{
			Content = shellViewModels[1]
		};
		Grid.SetRow(transitioningContentControl2, 2);
		grid.Children.Add(transitioningContentControl);
		grid.Children.Add(gridSplitter);
		grid.Children.Add(transitioningContentControl2);
		RootGrid.Children.Clear();
		RootGrid.Children.Add(grid);
		int num = ((currentShowPanesIndex < 2) ? currentShowPanesIndex : 0);
		SetPaneData(transitioningContentControl, shellViewModels[0], num == 0);
		SetPaneData(transitioningContentControl2, shellViewModels[1], 1 == num);
		showPanesCount = 2;
		ShellPaneArrangement = ShellPaneArrangement.Vertical;
	}

	private void ArrangeQuadLayout()
	{
		PrepareShellViewModels(4);
		Grid grid = new Grid();
		grid.ColumnDefinitions.Add(new ColumnDefinition
		{
			Width = new GridLength(1.0, GridUnitType.Star)
		});
		grid.ColumnDefinitions.Add(new ColumnDefinition
		{
			Width = GridLength.Auto
		});
		grid.ColumnDefinitions.Add(new ColumnDefinition
		{
			Width = new GridLength(1.0, GridUnitType.Star)
		});
		Grid grid2 = new Grid();
		grid2.RowDefinitions.Add(new RowDefinition
		{
			Height = new GridLength(1.0, GridUnitType.Star)
		});
		grid2.RowDefinitions.Add(new RowDefinition
		{
			Height = GridLength.Auto
		});
		grid2.RowDefinitions.Add(new RowDefinition
		{
			Height = new GridLength(1.0, GridUnitType.Star)
		});
		TransitioningContentControl transitioningContentControl = new TransitioningContentControl
		{
			Content = shellViewModels[0]
		};
		Grid.SetRow(transitioningContentControl, 0);
		GridSplitter gridSplitter = CreateHorizontalSplitter();
		Grid.SetRow(gridSplitter, 1);
		TransitioningContentControl transitioningContentControl2 = new TransitioningContentControl
		{
			Content = shellViewModels[2]
		};
		Grid.SetRow(transitioningContentControl2, 2);
		grid2.Children.Add(transitioningContentControl);
		grid2.Children.Add(gridSplitter);
		grid2.Children.Add(transitioningContentControl2);
		Grid.SetColumn(grid2, 0);
		GridSplitter gridSplitter2 = CreateVerticalSplitter();
		Grid.SetColumn(gridSplitter2, 1);
		Grid grid3 = new Grid();
		grid3.RowDefinitions.Add(new RowDefinition
		{
			Height = new GridLength(1.0, GridUnitType.Star)
		});
		grid3.RowDefinitions.Add(new RowDefinition
		{
			Height = GridLength.Auto
		});
		grid3.RowDefinitions.Add(new RowDefinition
		{
			Height = new GridLength(1.0, GridUnitType.Star)
		});
		TransitioningContentControl transitioningContentControl3 = new TransitioningContentControl
		{
			Content = shellViewModels[1]
		};
		Grid.SetRow(transitioningContentControl3, 0);
		GridSplitter gridSplitter3 = CreateHorizontalSplitter();
		Grid.SetRow(gridSplitter3, 1);
		TransitioningContentControl transitioningContentControl4 = new TransitioningContentControl
		{
			Content = shellViewModels[3]
		};
		Grid.SetRow(transitioningContentControl4, 2);
		grid3.Children.Add(transitioningContentControl3);
		grid3.Children.Add(gridSplitter3);
		grid3.Children.Add(transitioningContentControl4);
		Grid.SetColumn(grid3, 2);
		grid.Children.Add(grid2);
		grid.Children.Add(gridSplitter2);
		grid.Children.Add(grid3);
		RootGrid.Children.Clear();
		RootGrid.Children.Add(grid);
		int num = ((currentShowPanesIndex < 4) ? currentShowPanesIndex : 0);
		SetPaneData(transitioningContentControl, shellViewModels[0], num == 0);
		SetPaneData(transitioningContentControl3, shellViewModels[1], 1 == num);
		SetPaneData(transitioningContentControl2, shellViewModels[2], 2 == num);
		SetPaneData(transitioningContentControl4, shellViewModels[3], 3 == num);
		showPanesCount = 4;
		ShellPaneArrangement = ShellPaneArrangement.Quad;
	}

	private void ArrangeLeftTwoRightLayout()
	{
		PrepareShellViewModels(3);
		Grid grid = new Grid();
		grid.ColumnDefinitions.Add(new ColumnDefinition
		{
			Width = new GridLength(1.0, GridUnitType.Star)
		});
		grid.ColumnDefinitions.Add(new ColumnDefinition
		{
			Width = GridLength.Auto
		});
		grid.ColumnDefinitions.Add(new ColumnDefinition
		{
			Width = new GridLength(1.0, GridUnitType.Star)
		});
		grid.RowDefinitions.Add(new RowDefinition
		{
			Height = new GridLength(1.0, GridUnitType.Star)
		});
		grid.RowDefinitions.Add(new RowDefinition
		{
			Height = GridLength.Auto
		});
		grid.RowDefinitions.Add(new RowDefinition
		{
			Height = new GridLength(1.0, GridUnitType.Star)
		});
		TransitioningContentControl transitioningContentControl = new TransitioningContentControl
		{
			Content = shellViewModels[0]
		};
		Grid.SetColumn(transitioningContentControl, 0);
		Grid.SetRowSpan(transitioningContentControl, 3);
		TransitioningContentControl transitioningContentControl2 = new TransitioningContentControl
		{
			Content = shellViewModels[1]
		};
		Grid.SetColumn(transitioningContentControl2, 2);
		Grid.SetRow(transitioningContentControl2, 0);
		TransitioningContentControl transitioningContentControl3 = new TransitioningContentControl
		{
			Content = shellViewModels[2]
		};
		Grid.SetColumn(transitioningContentControl3, 2);
		Grid.SetRow(transitioningContentControl3, 2);
		GridSplitter gridSplitter = CreateVerticalSplitter();
		Grid.SetColumn(gridSplitter, 1);
		Grid.SetRowSpan(gridSplitter, 3);
		GridSplitter gridSplitter2 = CreateHorizontalSplitter();
		Grid.SetColumn(gridSplitter2, 2);
		Grid.SetRow(gridSplitter2, 1);
		grid.Children.Add(transitioningContentControl);
		grid.Children.Add(transitioningContentControl2);
		grid.Children.Add(transitioningContentControl3);
		grid.Children.Add(gridSplitter);
		grid.Children.Add(gridSplitter2);
		RootGrid.Children.Clear();
		RootGrid.Children.Add(grid);
		int num = ((currentShowPanesIndex < 3) ? currentShowPanesIndex : 0);
		SetPaneData(transitioningContentControl, shellViewModels[0], num == 0);
		SetPaneData(transitioningContentControl2, shellViewModels[1], 1 == num);
		SetPaneData(transitioningContentControl3, shellViewModels[2], 2 == num);
		showPanesCount = 3;
		ShellPaneArrangement = ShellPaneArrangement.LeftTwoRight;
	}

	private void ArrangeTopTwoBottomLayout()
	{
		PrepareShellViewModels(3);
		Grid grid = new Grid();
		grid.RowDefinitions.Add(new RowDefinition
		{
			Height = new GridLength(1.0, GridUnitType.Star)
		});
		grid.RowDefinitions.Add(new RowDefinition
		{
			Height = GridLength.Auto
		});
		grid.RowDefinitions.Add(new RowDefinition
		{
			Height = new GridLength(1.0, GridUnitType.Star)
		});
		grid.ColumnDefinitions.Add(new ColumnDefinition
		{
			Width = new GridLength(1.0, GridUnitType.Star)
		});
		grid.ColumnDefinitions.Add(new ColumnDefinition
		{
			Width = GridLength.Auto
		});
		grid.ColumnDefinitions.Add(new ColumnDefinition
		{
			Width = new GridLength(1.0, GridUnitType.Star)
		});
		TransitioningContentControl transitioningContentControl = new TransitioningContentControl
		{
			Content = shellViewModels[0]
		};
		Grid.SetRow(transitioningContentControl, 0);
		Grid.SetColumnSpan(transitioningContentControl, 3);
		TransitioningContentControl transitioningContentControl2 = new TransitioningContentControl
		{
			Content = shellViewModels[1]
		};
		Grid.SetRow(transitioningContentControl2, 2);
		Grid.SetColumn(transitioningContentControl2, 0);
		TransitioningContentControl transitioningContentControl3 = new TransitioningContentControl
		{
			Content = shellViewModels[2]
		};
		Grid.SetRow(transitioningContentControl3, 2);
		Grid.SetColumn(transitioningContentControl3, 2);
		GridSplitter gridSplitter = CreateHorizontalSplitter();
		Grid.SetRow(gridSplitter, 1);
		Grid.SetColumnSpan(gridSplitter, 3);
		GridSplitter gridSplitter2 = CreateVerticalSplitter();
		Grid.SetRow(gridSplitter2, 2);
		Grid.SetColumn(gridSplitter2, 1);
		grid.Children.Add(transitioningContentControl);
		grid.Children.Add(transitioningContentControl2);
		grid.Children.Add(transitioningContentControl3);
		grid.Children.Add(gridSplitter);
		grid.Children.Add(gridSplitter2);
		RootGrid.Children.Clear();
		RootGrid.Children.Add(grid);
		int num = ((currentShowPanesIndex < 3) ? currentShowPanesIndex : 0);
		SetPaneData(transitioningContentControl, shellViewModels[0], num == 0);
		SetPaneData(transitioningContentControl2, shellViewModels[1], 1 == num);
		SetPaneData(transitioningContentControl3, shellViewModels[2], 2 == num);
		showPanesCount = 3;
		ShellPaneArrangement = ShellPaneArrangement.TopTwoBottom;
	}

	private void ArrangeThreeColumnsLayout()
	{
		PrepareShellViewModels(3);
		Grid grid = new Grid();
		grid.ColumnDefinitions.Add(new ColumnDefinition
		{
			Width = new GridLength(1.0, GridUnitType.Star)
		});
		grid.ColumnDefinitions.Add(new ColumnDefinition
		{
			Width = GridLength.Auto
		});
		grid.ColumnDefinitions.Add(new ColumnDefinition
		{
			Width = new GridLength(1.0, GridUnitType.Star)
		});
		grid.ColumnDefinitions.Add(new ColumnDefinition
		{
			Width = GridLength.Auto
		});
		grid.ColumnDefinitions.Add(new ColumnDefinition
		{
			Width = new GridLength(1.0, GridUnitType.Star)
		});
		TransitioningContentControl transitioningContentControl = new TransitioningContentControl
		{
			Content = shellViewModels[0]
		};
		Grid.SetColumn(transitioningContentControl, 0);
		TransitioningContentControl transitioningContentControl2 = new TransitioningContentControl
		{
			Content = shellViewModels[1]
		};
		Grid.SetColumn(transitioningContentControl2, 2);
		TransitioningContentControl transitioningContentControl3 = new TransitioningContentControl
		{
			Content = shellViewModels[2]
		};
		Grid.SetColumn(transitioningContentControl3, 4);
		GridSplitter gridSplitter = CreateVerticalSplitter();
		Grid.SetColumn(gridSplitter, 1);
		GridSplitter gridSplitter2 = CreateVerticalSplitter();
		Grid.SetColumn(gridSplitter2, 3);
		grid.Children.Add(transitioningContentControl);
		grid.Children.Add(transitioningContentControl2);
		grid.Children.Add(transitioningContentControl3);
		grid.Children.Add(gridSplitter);
		grid.Children.Add(gridSplitter2);
		RootGrid.Children.Clear();
		RootGrid.Children.Add(grid);
		int num = ((currentShowPanesIndex < 3) ? currentShowPanesIndex : 0);
		SetPaneData(transitioningContentControl, shellViewModels[0], num == 0);
		SetPaneData(transitioningContentControl2, shellViewModels[1], 1 == num);
		SetPaneData(transitioningContentControl3, shellViewModels[2], 2 == num);
		showPanesCount = 3;
		ShellPaneArrangement = ShellPaneArrangement.ThreeColumns;
	}

	private void ArrangeThreeRowsLayout()
	{
		PrepareShellViewModels(3);
		Grid grid = new Grid();
		grid.RowDefinitions.Add(new RowDefinition
		{
			Height = new GridLength(1.0, GridUnitType.Star)
		});
		grid.RowDefinitions.Add(new RowDefinition
		{
			Height = GridLength.Auto
		});
		grid.RowDefinitions.Add(new RowDefinition
		{
			Height = new GridLength(1.0, GridUnitType.Star)
		});
		grid.RowDefinitions.Add(new RowDefinition
		{
			Height = GridLength.Auto
		});
		grid.RowDefinitions.Add(new RowDefinition
		{
			Height = new GridLength(1.0, GridUnitType.Star)
		});
		TransitioningContentControl transitioningContentControl = new TransitioningContentControl
		{
			Content = shellViewModels[0]
		};
		Grid.SetRow(transitioningContentControl, 0);
		TransitioningContentControl transitioningContentControl2 = new TransitioningContentControl
		{
			Content = shellViewModels[1]
		};
		Grid.SetRow(transitioningContentControl2, 2);
		TransitioningContentControl transitioningContentControl3 = new TransitioningContentControl
		{
			Content = shellViewModels[2]
		};
		Grid.SetRow(transitioningContentControl3, 4);
		GridSplitter gridSplitter = CreateHorizontalSplitter();
		Grid.SetRow(gridSplitter, 1);
		GridSplitter gridSplitter2 = CreateHorizontalSplitter();
		Grid.SetRow(gridSplitter2, 3);
		grid.Children.Add(transitioningContentControl);
		grid.Children.Add(transitioningContentControl2);
		grid.Children.Add(transitioningContentControl3);
		grid.Children.Add(gridSplitter);
		grid.Children.Add(gridSplitter2);
		RootGrid.Children.Clear();
		RootGrid.Children.Add(grid);
		int num = ((currentShowPanesIndex < 3) ? currentShowPanesIndex : 0);
		SetPaneData(transitioningContentControl, shellViewModels[0], num == 0);
		SetPaneData(transitioningContentControl2, shellViewModels[1], 1 == num);
		SetPaneData(transitioningContentControl3, shellViewModels[2], 2 == num);
		showPanesCount = 3;
		ShellPaneArrangement = ShellPaneArrangement.ThreeRows;
	}

	private void ArrangeThreeColumnsWeightedLayout()
	{
		PrepareShellViewModels(3);
		Grid grid = new Grid();
		grid.ColumnDefinitions.Add(new ColumnDefinition
		{
			Width = new GridLength(1.0, GridUnitType.Star)
		});
		grid.ColumnDefinitions.Add(new ColumnDefinition
		{
			Width = GridLength.Auto
		});
		grid.ColumnDefinitions.Add(new ColumnDefinition
		{
			Width = new GridLength(1.0, GridUnitType.Star)
		});
		grid.ColumnDefinitions.Add(new ColumnDefinition
		{
			Width = GridLength.Auto
		});
		grid.ColumnDefinitions.Add(new ColumnDefinition
		{
			Width = new GridLength(2.0, GridUnitType.Star)
		});
		TransitioningContentControl transitioningContentControl = new TransitioningContentControl
		{
			Content = shellViewModels[0]
		};
		Grid.SetColumn(transitioningContentControl, 0);
		TransitioningContentControl transitioningContentControl2 = new TransitioningContentControl
		{
			Content = shellViewModels[1]
		};
		Grid.SetColumn(transitioningContentControl2, 2);
		TransitioningContentControl transitioningContentControl3 = new TransitioningContentControl
		{
			Content = shellViewModels[2]
		};
		Grid.SetColumn(transitioningContentControl3, 4);
		GridSplitter gridSplitter = CreateVerticalSplitter();
		Grid.SetColumn(gridSplitter, 1);
		GridSplitter gridSplitter2 = CreateVerticalSplitter();
		Grid.SetColumn(gridSplitter2, 3);
		grid.Children.Add(transitioningContentControl);
		grid.Children.Add(transitioningContentControl2);
		grid.Children.Add(transitioningContentControl3);
		grid.Children.Add(gridSplitter);
		grid.Children.Add(gridSplitter2);
		RootGrid.Children.Clear();
		RootGrid.Children.Add(grid);
		int num = ((currentShowPanesIndex < 3) ? currentShowPanesIndex : 0);
		SetPaneData(transitioningContentControl, shellViewModels[0], num == 0);
		SetPaneData(transitioningContentControl2, shellViewModels[1], 1 == num);
		SetPaneData(transitioningContentControl3, shellViewModels[2], 2 == num);
		showPanesCount = 3;
		ShellPaneArrangement = ShellPaneArrangement.ThreeColumnsWeighted;
	}

	private void ArrangeThreeRowsWeightedLayout()
	{
		PrepareShellViewModels(3);
		Grid grid = new Grid();
		grid.RowDefinitions.Add(new RowDefinition
		{
			Height = new GridLength(1.0, GridUnitType.Star)
		});
		grid.RowDefinitions.Add(new RowDefinition
		{
			Height = GridLength.Auto
		});
		grid.RowDefinitions.Add(new RowDefinition
		{
			Height = new GridLength(1.0, GridUnitType.Star)
		});
		grid.RowDefinitions.Add(new RowDefinition
		{
			Height = GridLength.Auto
		});
		grid.RowDefinitions.Add(new RowDefinition
		{
			Height = new GridLength(2.0, GridUnitType.Star)
		});
		TransitioningContentControl transitioningContentControl = new TransitioningContentControl
		{
			Content = shellViewModels[0]
		};
		Grid.SetRow(transitioningContentControl, 0);
		TransitioningContentControl transitioningContentControl2 = new TransitioningContentControl
		{
			Content = shellViewModels[1]
		};
		Grid.SetRow(transitioningContentControl2, 2);
		TransitioningContentControl transitioningContentControl3 = new TransitioningContentControl
		{
			Content = shellViewModels[2]
		};
		Grid.SetRow(transitioningContentControl3, 4);
		GridSplitter gridSplitter = CreateHorizontalSplitter();
		Grid.SetRow(gridSplitter, 1);
		GridSplitter gridSplitter2 = CreateHorizontalSplitter();
		Grid.SetRow(gridSplitter2, 3);
		grid.Children.Add(transitioningContentControl);
		grid.Children.Add(transitioningContentControl2);
		grid.Children.Add(transitioningContentControl3);
		grid.Children.Add(gridSplitter);
		grid.Children.Add(gridSplitter2);
		RootGrid.Children.Clear();
		RootGrid.Children.Add(grid);
		int num = ((currentShowPanesIndex < 3) ? currentShowPanesIndex : 0);
		SetPaneData(transitioningContentControl, shellViewModels[0], num == 0);
		SetPaneData(transitioningContentControl2, shellViewModels[1], 1 == num);
		SetPaneData(transitioningContentControl3, shellViewModels[2], 2 == num);
		showPanesCount = 3;
		ShellPaneArrangement = ShellPaneArrangement.ThreeRowsWeighted;
	}

	private void ArrangeFourColumnsLayout()
	{
		PrepareShellViewModels(4);
		Grid grid = new Grid();
		grid.ColumnDefinitions.Add(new ColumnDefinition
		{
			Width = new GridLength(1.0, GridUnitType.Star)
		});
		grid.ColumnDefinitions.Add(new ColumnDefinition
		{
			Width = GridLength.Auto
		});
		grid.ColumnDefinitions.Add(new ColumnDefinition
		{
			Width = new GridLength(1.0, GridUnitType.Star)
		});
		grid.ColumnDefinitions.Add(new ColumnDefinition
		{
			Width = GridLength.Auto
		});
		grid.ColumnDefinitions.Add(new ColumnDefinition
		{
			Width = new GridLength(1.0, GridUnitType.Star)
		});
		grid.ColumnDefinitions.Add(new ColumnDefinition
		{
			Width = GridLength.Auto
		});
		grid.ColumnDefinitions.Add(new ColumnDefinition
		{
			Width = new GridLength(1.0, GridUnitType.Star)
		});
		TransitioningContentControl transitioningContentControl = new TransitioningContentControl
		{
			Content = shellViewModels[0]
		};
		Grid.SetColumn(transitioningContentControl, 0);
		TransitioningContentControl transitioningContentControl2 = new TransitioningContentControl
		{
			Content = shellViewModels[1]
		};
		Grid.SetColumn(transitioningContentControl2, 2);
		TransitioningContentControl transitioningContentControl3 = new TransitioningContentControl
		{
			Content = shellViewModels[2]
		};
		Grid.SetColumn(transitioningContentControl3, 4);
		TransitioningContentControl transitioningContentControl4 = new TransitioningContentControl
		{
			Content = shellViewModels[3]
		};
		Grid.SetColumn(transitioningContentControl4, 6);
		GridSplitter gridSplitter = CreateVerticalSplitter();
		Grid.SetColumn(gridSplitter, 1);
		GridSplitter gridSplitter2 = CreateVerticalSplitter();
		Grid.SetColumn(gridSplitter2, 3);
		GridSplitter gridSplitter3 = CreateVerticalSplitter();
		Grid.SetColumn(gridSplitter3, 5);
		grid.Children.Add(transitioningContentControl);
		grid.Children.Add(transitioningContentControl2);
		grid.Children.Add(transitioningContentControl3);
		grid.Children.Add(transitioningContentControl4);
		grid.Children.Add(gridSplitter);
		grid.Children.Add(gridSplitter2);
		grid.Children.Add(gridSplitter3);
		RootGrid.Children.Clear();
		RootGrid.Children.Add(grid);
		int num = ((currentShowPanesIndex < 4) ? currentShowPanesIndex : 0);
		SetPaneData(transitioningContentControl, shellViewModels[0], num == 0);
		SetPaneData(transitioningContentControl2, shellViewModels[1], 1 == num);
		SetPaneData(transitioningContentControl3, shellViewModels[2], 2 == num);
		SetPaneData(transitioningContentControl4, shellViewModels[3], 3 == num);
		showPanesCount = 4;
		ShellPaneArrangement = ShellPaneArrangement.FourColumns;
	}

	private void ArrangeFourRowsLayout()
	{
		PrepareShellViewModels(4);
		Grid grid = new Grid();
		grid.RowDefinitions.Add(new RowDefinition
		{
			Height = new GridLength(1.0, GridUnitType.Star)
		});
		grid.RowDefinitions.Add(new RowDefinition
		{
			Height = GridLength.Auto
		});
		grid.RowDefinitions.Add(new RowDefinition
		{
			Height = new GridLength(1.0, GridUnitType.Star)
		});
		grid.RowDefinitions.Add(new RowDefinition
		{
			Height = GridLength.Auto
		});
		grid.RowDefinitions.Add(new RowDefinition
		{
			Height = new GridLength(1.0, GridUnitType.Star)
		});
		grid.RowDefinitions.Add(new RowDefinition
		{
			Height = GridLength.Auto
		});
		grid.RowDefinitions.Add(new RowDefinition
		{
			Height = new GridLength(1.0, GridUnitType.Star)
		});
		TransitioningContentControl transitioningContentControl = new TransitioningContentControl
		{
			Content = shellViewModels[0]
		};
		Grid.SetRow(transitioningContentControl, 0);
		TransitioningContentControl transitioningContentControl2 = new TransitioningContentControl
		{
			Content = shellViewModels[1]
		};
		Grid.SetRow(transitioningContentControl2, 2);
		TransitioningContentControl transitioningContentControl3 = new TransitioningContentControl
		{
			Content = shellViewModels[2]
		};
		Grid.SetRow(transitioningContentControl3, 4);
		TransitioningContentControl transitioningContentControl4 = new TransitioningContentControl
		{
			Content = shellViewModels[3]
		};
		Grid.SetRow(transitioningContentControl4, 6);
		GridSplitter gridSplitter = CreateHorizontalSplitter();
		Grid.SetRow(gridSplitter, 1);
		GridSplitter gridSplitter2 = CreateHorizontalSplitter();
		Grid.SetRow(gridSplitter2, 3);
		GridSplitter gridSplitter3 = CreateHorizontalSplitter();
		Grid.SetRow(gridSplitter3, 5);
		grid.Children.Add(transitioningContentControl);
		grid.Children.Add(transitioningContentControl2);
		grid.Children.Add(transitioningContentControl3);
		grid.Children.Add(transitioningContentControl4);
		grid.Children.Add(gridSplitter);
		grid.Children.Add(gridSplitter2);
		grid.Children.Add(gridSplitter3);
		RootGrid.Children.Clear();
		RootGrid.Children.Add(grid);
		int num = ((currentShowPanesIndex < 4) ? currentShowPanesIndex : 0);
		SetPaneData(transitioningContentControl, shellViewModels[0], num == 0);
		SetPaneData(transitioningContentControl2, shellViewModels[1], 1 == num);
		SetPaneData(transitioningContentControl3, shellViewModels[2], 2 == num);
		SetPaneData(transitioningContentControl4, shellViewModels[3], 3 == num);
		showPanesCount = 4;
		ShellPaneArrangement = ShellPaneArrangement.FourRows;
	}

	private void ArrangeTwoLeftRightLayout()
	{
		PrepareShellViewModels(3);
		Grid grid = new Grid();
		grid.ColumnDefinitions.Add(new ColumnDefinition
		{
			Width = new GridLength(1.0, GridUnitType.Star)
		});
		grid.ColumnDefinitions.Add(new ColumnDefinition
		{
			Width = GridLength.Auto
		});
		grid.ColumnDefinitions.Add(new ColumnDefinition
		{
			Width = new GridLength(1.0, GridUnitType.Star)
		});
		grid.RowDefinitions.Add(new RowDefinition
		{
			Height = new GridLength(1.0, GridUnitType.Star)
		});
		grid.RowDefinitions.Add(new RowDefinition
		{
			Height = GridLength.Auto
		});
		grid.RowDefinitions.Add(new RowDefinition
		{
			Height = new GridLength(1.0, GridUnitType.Star)
		});
		TransitioningContentControl transitioningContentControl = new TransitioningContentControl
		{
			Content = shellViewModels[0]
		};
		Grid.SetColumn(transitioningContentControl, 0);
		Grid.SetRow(transitioningContentControl, 0);
		TransitioningContentControl transitioningContentControl2 = new TransitioningContentControl
		{
			Content = shellViewModels[1]
		};
		Grid.SetColumn(transitioningContentControl2, 0);
		Grid.SetRow(transitioningContentControl2, 2);
		TransitioningContentControl transitioningContentControl3 = new TransitioningContentControl
		{
			Content = shellViewModels[2]
		};
		Grid.SetColumn(transitioningContentControl3, 2);
		Grid.SetRowSpan(transitioningContentControl3, 3);
		GridSplitter gridSplitter = CreateVerticalSplitter();
		Grid.SetColumn(gridSplitter, 1);
		Grid.SetRowSpan(gridSplitter, 3);
		GridSplitter gridSplitter2 = CreateHorizontalSplitter();
		Grid.SetColumn(gridSplitter2, 0);
		Grid.SetRow(gridSplitter2, 1);
		grid.Children.Add(transitioningContentControl);
		grid.Children.Add(transitioningContentControl2);
		grid.Children.Add(transitioningContentControl3);
		grid.Children.Add(gridSplitter);
		grid.Children.Add(gridSplitter2);
		RootGrid.Children.Clear();
		RootGrid.Children.Add(grid);
		int num = ((currentShowPanesIndex < 3) ? currentShowPanesIndex : 0);
		SetPaneData(transitioningContentControl, shellViewModels[0], num == 0);
		SetPaneData(transitioningContentControl2, shellViewModels[1], 1 == num);
		SetPaneData(transitioningContentControl3, shellViewModels[2], 2 == num);
		showPanesCount = 3;
		ShellPaneArrangement = ShellPaneArrangement.TwoLeftRight;
	}

	private void ArrangeTwoTopBottomLayout()
	{
		PrepareShellViewModels(3);
		Grid grid = new Grid();
		grid.RowDefinitions.Add(new RowDefinition
		{
			Height = new GridLength(1.0, GridUnitType.Star)
		});
		grid.RowDefinitions.Add(new RowDefinition
		{
			Height = GridLength.Auto
		});
		grid.RowDefinitions.Add(new RowDefinition
		{
			Height = new GridLength(1.0, GridUnitType.Star)
		});
		grid.ColumnDefinitions.Add(new ColumnDefinition
		{
			Width = new GridLength(1.0, GridUnitType.Star)
		});
		grid.ColumnDefinitions.Add(new ColumnDefinition
		{
			Width = GridLength.Auto
		});
		grid.ColumnDefinitions.Add(new ColumnDefinition
		{
			Width = new GridLength(1.0, GridUnitType.Star)
		});
		TransitioningContentControl transitioningContentControl = new TransitioningContentControl
		{
			Content = shellViewModels[0]
		};
		Grid.SetRow(transitioningContentControl, 0);
		Grid.SetColumn(transitioningContentControl, 0);
		TransitioningContentControl transitioningContentControl2 = new TransitioningContentControl
		{
			Content = shellViewModels[1]
		};
		Grid.SetRow(transitioningContentControl2, 0);
		Grid.SetColumn(transitioningContentControl2, 2);
		TransitioningContentControl transitioningContentControl3 = new TransitioningContentControl
		{
			Content = shellViewModels[2]
		};
		Grid.SetRow(transitioningContentControl3, 2);
		Grid.SetColumnSpan(transitioningContentControl3, 3);
		GridSplitter gridSplitter = CreateHorizontalSplitter();
		Grid.SetRow(gridSplitter, 1);
		Grid.SetColumnSpan(gridSplitter, 3);
		GridSplitter gridSplitter2 = CreateVerticalSplitter();
		Grid.SetRow(gridSplitter2, 0);
		Grid.SetColumn(gridSplitter2, 1);
		grid.Children.Add(transitioningContentControl);
		grid.Children.Add(transitioningContentControl2);
		grid.Children.Add(transitioningContentControl3);
		grid.Children.Add(gridSplitter);
		grid.Children.Add(gridSplitter2);
		RootGrid.Children.Clear();
		RootGrid.Children.Add(grid);
		int num = ((currentShowPanesIndex < 3) ? currentShowPanesIndex : 0);
		SetPaneData(transitioningContentControl, shellViewModels[0], num == 0);
		SetPaneData(transitioningContentControl2, shellViewModels[1], 1 == num);
		SetPaneData(transitioningContentControl3, shellViewModels[2], 2 == num);
		showPanesCount = 3;
		ShellPaneArrangement = ShellPaneArrangement.TwoTopBottom;
	}

	public void SaveState()
	{
		SessionModel sessionModel = (sessionCache = sessionCache ?? sessionService.RestoreSession() ?? new SessionModel());
		sessionModel.Arrangement = ShellPaneArrangement;
		MainWindowViewModel mainWindowViewModel = GetMainWindowViewModel();
		if (mainWindowViewModel != null)
		{
			sessionModel.SidebarIsOpen = mainWindowViewModel.IsPaneOpen;
			sessionModel.SidebarWidth = mainWindowViewModel.GetSidebarWidthForSession();
		}
		CaptureCurrentLayoutState(sessionModel);
		sessionModel.Shells.Clear();
		foreach (BaseShellViewModel item in shellViewModels.Take(showPanesCount))
		{
			ShellSessionModel shellSessionModel = new ShellSessionModel
			{
				SelectedTabIndex = item.SelectedTabIndex
			};
			foreach (TabItemViewModel tabItem in item.TabItems)
			{
				shellSessionModel.Tabs.Add(new TabSessionModel
				{
					NavPath = tabItem.NavPath,
					FolderLayoutMode = NormalizeRestoredLayoutMode(tabItem.FolderLayoutMode),
					CurrentSortOption = NormalizeRestoredSortOption(tabItem.CurrentSortOption),
					CurrentSortDirection = NormalizeRestoredSortDirection(tabItem.CurrentSortDirection),
					HasCustomSort = tabItem.HasCustomSort
				});
			}
			sessionModel.Shells.Add(shellSessionModel);
		}
		sessionService.SaveSession(sessionModel);
	}

	private GridSplitter CreateVerticalSplitter()
	{
		GridSplitter gridSplitter = new GridSplitter();
		gridSplitter.Width = 1.0;
		gridSplitter.HorizontalAlignment = HorizontalAlignment.Left;
		gridSplitter.ResizeDirection = GridResizeDirection.Columns;
		gridSplitter.ResizeBehavior = GridResizeBehavior.PreviousAndNext;
		gridSplitter[!TemplatedControl.BackgroundProperty] = new DynamicResourceExtension("TopToolbarBackground");
		gridSplitter.PointerEntered += OnSplitterPointerEntered;
		gridSplitter.PointerExited += OnSplitterPointerExited;
		gridSplitter.DragStarted += OnSplitterDragStarted;
		gridSplitter.DragCompleted += OnSplitterDragCompleted;
		gridSplitter.AddHandler(InputElement.PointerReleasedEvent, OnSplitterPointerReleased, RoutingStrategies.Bubble, handledEventsToo: true);
		return gridSplitter;
	}

	private GridSplitter CreateHorizontalSplitter()
	{
		GridSplitter gridSplitter = new GridSplitter();
		gridSplitter.Height = 1.0;
		gridSplitter.VerticalAlignment = VerticalAlignment.Top;
		gridSplitter.ResizeDirection = GridResizeDirection.Rows;
		gridSplitter.ResizeBehavior = GridResizeBehavior.PreviousAndNext;
		gridSplitter[!TemplatedControl.BackgroundProperty] = new DynamicResourceExtension("TopToolbarBackground");
		gridSplitter.PointerEntered += OnSplitterPointerEntered;
		gridSplitter.PointerExited += OnSplitterPointerExited;
		gridSplitter.DragStarted += OnSplitterDragStarted;
		gridSplitter.DragCompleted += OnSplitterDragCompleted;
		gridSplitter.AddHandler(InputElement.PointerReleasedEvent, OnSplitterPointerReleased, RoutingStrategies.Bubble, handledEventsToo: true);
		return gridSplitter;
	}

	private void LoadState()
	{
		SessionModel session = sessionService.RestoreSession();
		if (session == null || session.Shells.Count == 0)
		{
			sessionCache = session;
			ArrangePanes(ShellPaneArrangement.Single);
			return;
		}
		sessionCache = session;
		PrepareShellViewModels(session.Shells.Count);
		for (int i = 0; i < session.Shells.Count; i++)
		{
			ShellSessionModel shellSessionModel = session.Shells[i];
			BaseShellViewModel baseShellViewModel = shellViewModels[i];
			if (shellSessionModel.Tabs == null || shellSessionModel.Tabs.Count <= 0)
			{
				continue;
			}
			baseShellViewModel.TabItems.Clear();
			foreach (TabSessionModel tab in shellSessionModel.Tabs)
			{
				TabItemViewModel item = new TabItemViewModel
				{
					NavPath = NormalizeRestoredTabPath(tab.NavPath),
					FolderLayoutMode = NormalizeRestoredLayoutMode(tab.FolderLayoutMode),
					CurrentSortOption = NormalizeRestoredSortOption(tab.CurrentSortOption),
					CurrentSortDirection = NormalizeRestoredSortDirection(tab.CurrentSortDirection),
					HasCustomSort = tab.HasCustomSort,
					ParentShellViewModel = baseShellViewModel
				};
				baseShellViewModel.TabItems.Add(item);
			}
			if (shellSessionModel.SelectedTabIndex >= 0 && shellSessionModel.SelectedTabIndex < baseShellViewModel.TabItems.Count)
			{
				baseShellViewModel.SelectedTab = baseShellViewModel.TabItems[shellSessionModel.SelectedTabIndex];
			}
			else
			{
				baseShellViewModel.SelectedTab = baseShellViewModel.TabItems[0];
			}
			ApplyRestoredLayoutMode(baseShellViewModel);
		}
		ArrangePanes(session.Arrangement);
		Avalonia.Threading.Dispatcher.UIThread.Post(() =>
		{
			MainWindowViewModel mainWindowViewModel = GetMainWindowViewModel();
			if (mainWindowViewModel != null)
			{
				mainWindowViewModel.SetSidebarWidthForSession(session.SidebarWidth);
				mainWindowViewModel.IsPaneOpen = session.SidebarIsOpen;
			}
		});
	}

	public void LoadSessionDirectly(SessionModel session)
	{
		if (session == null || session.Shells.Count == 0)
		{
			sessionCache = session;
			ArrangePanes(ShellPaneArrangement.Single);
			return;
		}
		sessionCache = session;
		PrepareShellViewModels(session.Shells.Count);
		for (int i = 0; i < session.Shells.Count; i++)
		{
			ShellSessionModel shellSessionModel = session.Shells[i];
			BaseShellViewModel baseShellViewModel = shellViewModels[i];
			if (shellSessionModel.Tabs == null || shellSessionModel.Tabs.Count <= 0)
			{
				continue;
			}
			baseShellViewModel.TabItems.Clear();
			foreach (TabSessionModel tab in shellSessionModel.Tabs)
			{
				TabItemViewModel item = new TabItemViewModel
				{
					NavPath = NormalizeRestoredTabPath(tab.NavPath),
					FolderLayoutMode = NormalizeRestoredLayoutMode(tab.FolderLayoutMode),
					CurrentSortOption = NormalizeRestoredSortOption(tab.CurrentSortOption),
					CurrentSortDirection = NormalizeRestoredSortDirection(tab.CurrentSortDirection),
					HasCustomSort = tab.HasCustomSort,
					ParentShellViewModel = baseShellViewModel
				};
				baseShellViewModel.TabItems.Add(item);
			}
			if (shellSessionModel.SelectedTabIndex >= 0 && shellSessionModel.SelectedTabIndex < baseShellViewModel.TabItems.Count)
			{
				baseShellViewModel.SelectedTab = baseShellViewModel.TabItems[shellSessionModel.SelectedTabIndex];
			}
			else
			{
				baseShellViewModel.SelectedTab = baseShellViewModel.TabItems[0];
			}
			ApplyRestoredLayoutMode(baseShellViewModel);
		}
		ArrangePanes(session.Arrangement);
		Avalonia.Threading.Dispatcher.UIThread.Post(() =>
		{
			MainWindowViewModel mainWindowViewModel = GetMainWindowViewModel();
			if (mainWindowViewModel != null)
			{
				mainWindowViewModel.SetSidebarWidthForSession(session.SidebarWidth);
				mainWindowViewModel.IsPaneOpen = session.SidebarIsOpen;
			}
		});
	}

	private static string NormalizeRestoredTabPath(string? navPath)
	{
		if (!string.IsNullOrWhiteSpace(navPath))
		{
			return navPath;
		}
		return NavigationHelpers.GetDefaultNewTabPath();
	}

	private static FolderLayoutModes NormalizeRestoredLayoutMode(FolderLayoutModes folderLayoutMode)
	{
		if (folderLayoutMode == FolderLayoutModes.GridView)
		{
			return FolderLayoutModes.GridView;
		}
		return FolderLayoutModes.DetailsView;
	}

	private static SortOption NormalizeRestoredSortOption(SortOption sortOption)
	{
		if (!Enum.IsDefined(typeof(SortOption), sortOption))
		{
			return SortOption.Name;
		}
		return sortOption;
	}

	private static SortDirection NormalizeRestoredSortDirection(SortDirection sortDirection)
	{
		if (!Enum.IsDefined(typeof(SortDirection), sortDirection))
		{
			return SortDirection.Ascending;
		}
		return sortDirection;
	}

	private static void ApplyRestoredLayoutMode(BaseShellViewModel shellViewModel)
	{
		if (shellViewModel.SelectedTab != null)
		{
			shellViewModel.FolderSettings.LayoutMode = NormalizeRestoredLayoutMode(shellViewModel.SelectedTab.FolderLayoutMode);
		}
	}

	private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
	{
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}

	public void Dispose()
	{
	}

	[GeneratedCode("Avalonia.Generators.NameGenerator.InitializeComponentCodeGenerator", "12.0.1.0")]
	[ExcludeFromCodeCoverage]
	public void InitializeComponent(bool loadXaml = true)
	{
		if (loadXaml)
		{
			_0021XamlIlPopulateTrampoline(this);
		}
		INameScope nameScope = this.FindNameScope();
		RootGrid = ((nameScope != null) ? nameScope.Find<Grid>("RootGrid") : null);
	}

	[CompilerGenerated]
	private static void _0021XamlIlPopulate(IServiceProvider P_0, ShellPanesView P_1)
	{
		XamlIlContext.Context<ShellPanesView> context = new XamlIlContext.Context<ShellPanesView>(P_0, new object[1] { _0021AvaloniaResources.NamespaceInfo_003A_002FViews_002FShellPanesView_002Eaxaml.Singleton }, "avares://XFolder/Views/ShellPanesView.axaml")
		{
			RootObject = P_1,
			IntermediateRoot = P_1
		};
		((ISupportInitialize)P_1).BeginInit();
		Grid grid2;
		Grid grid = (grid2 = new Grid());
		((ISupportInitialize)grid).BeginInit();
		P_1.Content = grid;
		grid2.Name = "RootGrid";
		object element = grid2;
		context.AvaloniaNameScope.Register("RootGrid", element);
		((ISupportInitialize)grid2).EndInit();
		((ISupportInitialize)P_1).EndInit();
		if (P_1 is StyledElement styled)
		{
			NameScope.SetNameScope(styled, context.AvaloniaNameScope);
		}
		context.AvaloniaNameScope.Complete();
	}

	[CompilerGenerated]
	private static void _0021XamlIlPopulateTrampoline(ShellPanesView P_0)
	{
		if (_0021XamlIlPopulateOverride != null)
		{
			_0021XamlIlPopulateOverride(P_0);
		}
		else
		{
			_0021XamlIlPopulate(XamlIlRuntimeHelpers.CreateRootServiceProviderV3(null), P_0);
		}
	}
}
