using System;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using Files.Assets.Lang;
using Files.Data.Commands.HotKey;

namespace Files.Actions;

internal sealed class ToggleFullScreenAction : ObservableObject, IToggleAction, IAction
{
	private sealed class WindowStateObserver : IObserver<WindowState>
	{
		private readonly ToggleFullScreenAction owner;

		public WindowStateObserver(ToggleFullScreenAction owner)
		{
			this.owner = owner;
		}

		public void OnCompleted()
		{
		}

		public void OnError(Exception error)
		{
		}

		public void OnNext(WindowState value)
		{
			if (value != WindowState.FullScreen)
			{
				owner.lastNonFullScreenState = ((value == WindowState.Maximized) ? WindowState.Maximized : WindowState.Normal);
			}
			owner.OnPropertyChanged("Label");
			owner.OnPropertyChanged("IsOn");
		}
	}

	private Window? observedWindow;

	private IDisposable? windowStateSubscription;

	private WindowState lastNonFullScreenState;

	public string Label
	{
		get
		{
			if (!IsOn)
			{
				return Resources.FullScreen;
			}
			return Resources.ExitFullScreen;
		}
	}

	public string Description => Resources.ToggleFullScreenDescription;

	public HotKey HotKey => new HotKey(Key.F11);

	public bool IsOn
	{
		get
		{
			Window window = TryGetMainWindow();
			EnsureWindowSubscription(window);
			if (window != null)
			{
				return window.WindowState == WindowState.FullScreen;
			}
			return false;
		}
	}

	public bool IsExecutable
	{
		get
		{
			Window window = TryGetMainWindow();
			EnsureWindowSubscription(window);
			return window != null;
		}
	}

	public Task ExecuteAsync(object? parameter = null)
	{
		Window window = TryGetMainWindow();
		if (window == null)
		{
			return Task.CompletedTask;
		}
		EnsureWindowSubscription(window);
		if (window.WindowState == WindowState.FullScreen)
		{
			window.WindowState = lastNonFullScreenState;
		}
		else
		{
			lastNonFullScreenState = ((window.WindowState == WindowState.Maximized) ? WindowState.Maximized : WindowState.Normal);
			window.WindowState = WindowState.FullScreen;
		}
		OnPropertyChanged("Label");
		OnPropertyChanged("IsOn");
		return Task.CompletedTask;
	}

	private static Window? TryGetMainWindow()
	{
		if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime classicDesktopStyleApplicationLifetime)
		{
			Window mainWindow = classicDesktopStyleApplicationLifetime.MainWindow;
			if (mainWindow != null)
			{
				return mainWindow;
			}
		}
		return null;
	}

	private void EnsureWindowSubscription(Window? window)
	{
		if (window != null && observedWindow != window)
		{
			windowStateSubscription?.Dispose();
			observedWindow = window;
			if (window.WindowState != WindowState.FullScreen)
			{
				lastNonFullScreenState = ((window.WindowState == WindowState.Maximized) ? WindowState.Maximized : WindowState.Normal);
			}
			windowStateSubscription = window.GetObservable(Window.WindowStateProperty).Subscribe(new WindowStateObserver(this));
			OnPropertyChanged("IsExecutable");
			OnPropertyChanged("Label");
			OnPropertyChanged("IsOn");
		}
	}
}
