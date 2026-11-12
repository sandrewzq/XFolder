using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia.Input;
using Avalonia.Styling;
using Files.Data.Commands.HotKey;
using Files.Data.Commands.Manager;

namespace Files.Data.Commands;

[DebuggerDisplay("Command None")]
internal sealed class NoneCommand : IRichCommand, ICommand, INotifyPropertyChanging, INotifyPropertyChanged
{
	public CommandCodes Code => CommandCodes.None;

	public string Label => string.Empty;

	public string LabelWithHotKey => string.Empty;

	public string? Icon => null;

	public string AutomationName => string.Empty;

	public string Description => string.Empty;

	public Style? ThemedIconStyle => null;

	public bool IsCustomHotKeys => false;

	public string? HotKeyText => null;

	public HotKeyCollection HotKeys
	{
		get
		{
			return HotKeyCollection.Empty;
		}
		set
		{
			throw new InvalidOperationException("This command is readonly.");
		}
	}

	public HotKeyCollection DefaultHotKeys
	{
		get
		{
			return HotKeyCollection.Empty;
		}
		set
		{
			throw new InvalidOperationException("This command is readonly.");
		}
	}

	public bool IsToggle => false;

	public bool IsOn
	{
		get
		{
			return false;
		}
		set
		{
		}
	}

	public bool IsExecutable => false;

	public bool IsAccessibleGlobally => false;

	public event EventHandler? CanExecuteChanged
	{
		add
		{
		}
		remove
		{
		}
	}

	public event PropertyChangingEventHandler? PropertyChanging
	{
		add
		{
		}
		remove
		{
		}
	}

	public event PropertyChangedEventHandler? PropertyChanged
	{
		add
		{
		}
		remove
		{
		}
	}

	public bool CanExecute(object? parameter)
	{
		return false;
	}

	public void Execute(object? parameter)
	{
	}

	public Task ExecuteAsync(object? parameter = null)
	{
		return Task.CompletedTask;
	}

	public void ExecuteTapped(object sender, TappedEventArgs e)
	{
	}

	public void ResetHotKeys()
	{
	}
}
