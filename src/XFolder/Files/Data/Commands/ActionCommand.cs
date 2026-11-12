using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using Files.Actions;
using Files.Data.Commands.HotKey;
using Files.Data.Commands.Manager;

namespace Files.Data.Commands;

[DebuggerDisplay("Command {Code}")]
internal sealed class ActionCommand : ObservableObject, IRichCommand, ICommand, INotifyPropertyChanging, INotifyPropertyChanged
{
	private bool isCustomHotKeys;

	private HotKeyCollection hotKeys;

	public IAction Action { get; }

	public CommandCodes Code { get; }

	public string Label => Action.Label;

	public string LabelWithHotKey
	{
		get
		{
			if (HotKeyText != null)
			{
				return Label + " (" + HotKeyText + ")";
			}
			return Label;
		}
	}

	public string? Icon => Action.Icon;

	public string AutomationName => Label;

	public string Description => Action.Description;

	public bool IsCustomHotKeys
	{
		get
		{
			return isCustomHotKeys;
		}
		set
		{
			SetProperty(ref isCustomHotKeys, value, "IsCustomHotKeys");
		}
	}

	public string? HotKeyText
	{
		get
		{
			string localizedLabel = HotKeys.LocalizedLabel;
			if (string.IsNullOrEmpty(localizedLabel))
			{
				return null;
			}
			return localizedLabel;
		}
	}

	public HotKeyCollection HotKeys
	{
		get
		{
			return hotKeys;
		}
		set
		{
			if (SetProperty(ref hotKeys, value, "HotKeys"))
			{
				OnPropertyChanged("HotKeyText");
				OnPropertyChanged("LabelWithHotKey");
				IsCustomHotKeys = true;
			}
		}
	}

	public HotKeyCollection DefaultHotKeys { get; }

	public bool IsToggle => Action is IToggleAction;

	public bool IsOn
	{
		get
		{
			if (Action is IToggleAction toggleAction)
			{
				return toggleAction.IsOn;
			}
			return false;
		}
		set
		{
			if (Action is IToggleAction toggleAction && toggleAction.IsOn != value)
			{
				Execute(null);
			}
		}
	}

	public bool IsExecutable => Action.IsExecutable;

	public bool IsAccessibleGlobally => Action.IsAccessibleGlobally;

	public event EventHandler? CanExecuteChanged;

	public ActionCommand(CommandManager manager, CommandCodes code, IAction action)
	{
		Code = code;
		Action = action;
		hotKeys = CommandManager.GetDefaultKeyBindings(action);
		DefaultHotKeys = CommandManager.GetDefaultKeyBindings(action);
		if (action is INotifyPropertyChanging notifyPropertyChanging)
		{
			notifyPropertyChanging.PropertyChanging += Action_PropertyChanging;
		}
		if (action is INotifyPropertyChanged notifyPropertyChanged)
		{
			notifyPropertyChanged.PropertyChanged += Action_PropertyChanged;
		}
	}

	public bool CanExecute(object? parameter)
	{
		return Action.IsExecutable;
	}

	public async void Execute(object? parameter)
	{
		await ExecuteAsync(parameter);
	}

	public Task ExecuteAsync(object? parameter = null)
	{
		if (IsExecutable)
		{
			return Action.ExecuteAsync(parameter);
		}
		return Task.CompletedTask;
	}

	public async void ExecuteTapped(object sender, TappedEventArgs e)
	{
		await ExecuteAsync();
	}

	internal void OverwriteKeyBindings(HotKeyCollection hotKeys)
	{
		HotKeys = hotKeys;
	}

	internal void RestoreKeyBindings()
	{
		hotKeys = DefaultHotKeys;
		IsCustomHotKeys = false;
	}

	private void Action_PropertyChanging(object? sender, PropertyChangingEventArgs e)
	{
		switch (e.PropertyName)
		{
		case "Label":
			OnPropertyChanging("Label");
			OnPropertyChanging("LabelWithHotKey");
			OnPropertyChanging("AutomationName");
			break;
		case "IsOn":
			if (IsToggle)
			{
				OnPropertyChanging("IsOn");
			}
			break;
		case "IsExecutable":
			OnPropertyChanging("IsExecutable");
			break;
		}
	}

	private void Action_PropertyChanged(object? sender, PropertyChangedEventArgs e)
	{
		switch (e.PropertyName)
		{
		case "Label":
			OnPropertyChanged("Label");
			OnPropertyChanged("LabelWithHotKey");
			OnPropertyChanged("AutomationName");
			break;
		case "IsOn":
			if (IsToggle)
			{
				OnPropertyChanged("IsOn");
			}
			break;
		case "IsExecutable":
			OnPropertyChanged("IsExecutable");
			CanExecuteChanged?.Invoke(this, EventArgs.Empty);
			break;
		}
	}
}
