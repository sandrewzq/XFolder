using System.ComponentModel;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using Files.Assets.Lang;
using Files.Data.Commands.HotKey;
using Files.ViewModels;

namespace Files.Actions;

internal sealed class ToggleSidebarAction : ObservableObject, IToggleAction, IAction
{
	private MainWindowViewModel? observedViewModel;

	public string Label => Resources.ToggleSidebar;

	public string Description => Resources.ToggleSidebarDescription;

	public HotKey HotKey => new HotKey(Key.B, KeyModifiers.Meta);

	public bool IsOn
	{
		get
		{
			MainWindowViewModel mainWindowViewModel = TryGetMainWindowViewModel();
			EnsureViewModelSubscription(mainWindowViewModel);
			return mainWindowViewModel?.IsPaneOpen ?? false;
		}
	}

	public bool IsExecutable
	{
		get
		{
			MainWindowViewModel mainWindowViewModel = TryGetMainWindowViewModel();
			EnsureViewModelSubscription(mainWindowViewModel);
			return mainWindowViewModel != null;
		}
	}

	public Task ExecuteAsync(object? parameter = null)
	{
		MainWindowViewModel mainWindowViewModel = TryGetMainWindowViewModel();
		if (mainWindowViewModel == null)
		{
			return Task.CompletedTask;
		}
		EnsureViewModelSubscription(mainWindowViewModel);
		mainWindowViewModel.IsPaneOpen = !mainWindowViewModel.IsPaneOpen;
		return Task.CompletedTask;
	}

	private static MainWindowViewModel? TryGetMainWindowViewModel()
	{
		if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime classicDesktopStyleApplicationLifetime)
		{
			Window mainWindow = classicDesktopStyleApplicationLifetime.MainWindow;
			if (mainWindow != null && mainWindow.DataContext is MainWindowViewModel result)
			{
				return result;
			}
		}
		return null;
	}

	private void EnsureViewModelSubscription(MainWindowViewModel? viewModel)
	{
		if (viewModel != null && observedViewModel != viewModel)
		{
			if (observedViewModel != null)
			{
				observedViewModel.PropertyChanged -= MainWindowViewModel_PropertyChanged;
			}
			observedViewModel = viewModel;
			observedViewModel.PropertyChanged += MainWindowViewModel_PropertyChanged;
			OnPropertyChanged("IsExecutable");
			OnPropertyChanged("IsOn");
		}
	}

	private void MainWindowViewModel_PropertyChanged(object? sender, PropertyChangedEventArgs e)
	{
		if (e.PropertyName == "IsPaneOpen")
		{
			OnPropertyChanged("IsOn");
		}
	}
}
