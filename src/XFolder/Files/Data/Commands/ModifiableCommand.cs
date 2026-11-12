using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using Files.Data.Commands.HotKey;
using Files.Data.Commands.Manager;

namespace Files.Data.Commands;

[DebuggerDisplay("Command {Code} (Modifiable)")]
internal sealed class ModifiableCommand : ObservableObject, IRichCommand, ICommand, INotifyPropertyChanging, INotifyPropertyChanged
{
	private IRichCommand BaseCommand;

	private ImmutableDictionary<KeyModifiers, IRichCommand> ModifiedCommands;

	public CommandCodes Code => BaseCommand.Code;

	public string Label => BaseCommand.Label;

	public string LabelWithHotKey => BaseCommand.LabelWithHotKey;

	public string? Icon => BaseCommand.Icon;

	public string AutomationName => BaseCommand.AutomationName;

	public string Description => BaseCommand.Description;

	public bool IsCustomHotKeys => BaseCommand.IsCustomHotKeys;

	public string? HotKeyText => BaseCommand.HotKeyText;

	public HotKeyCollection HotKeys
	{
		get
		{
			return BaseCommand.HotKeys;
		}
		set
		{
			BaseCommand.HotKeys = value;
		}
	}

	public HotKeyCollection DefaultHotKeys { get; }

	public bool IsToggle => BaseCommand.IsToggle;

	public bool IsOn
	{
		get
		{
			return BaseCommand.IsOn;
		}
		set
		{
			BaseCommand.IsOn = value;
		}
	}

	public bool IsExecutable => BaseCommand.IsExecutable;

	public bool IsAccessibleGlobally => BaseCommand.IsAccessibleGlobally;

	public event EventHandler? CanExecuteChanged;

	public ModifiableCommand(IRichCommand baseCommand, Dictionary<KeyModifiers, IRichCommand> modifiedCommands)
	{
		BaseCommand = baseCommand;
		ModifiedCommands = modifiedCommands.ToImmutableDictionary();
		DefaultHotKeys = new HotKeyCollection(BaseCommand.HotKeys);
		if (baseCommand is ActionCommand actionCommand)
		{
			if (actionCommand.Action is INotifyPropertyChanging notifyPropertyChanging)
			{
				notifyPropertyChanging.PropertyChanging += Action_PropertyChanging;
			}
			if (actionCommand.Action is INotifyPropertyChanged notifyPropertyChanged)
			{
				notifyPropertyChanged.PropertyChanged += Action_PropertyChanged;
			}
		}
	}

	public bool CanExecute(object? parameter)
	{
		return BaseCommand.CanExecute(parameter);
	}

	public async void Execute(object? parameter)
	{
		await ExecuteAsync(parameter);
	}

	public Task ExecuteAsync(object? parameter = null)
	{
		if (ModifiedCommands.TryGetValue(HotKeyHelpers.GetCurrentKeyModifiers(), out IRichCommand value) && value.IsExecutable)
		{
			return value.ExecuteAsync(parameter);
		}
		return BaseCommand.ExecuteAsync(parameter);
	}

	public async void ExecuteTapped(object sender, TappedEventArgs e)
	{
		await ExecuteAsync();
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
