using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia.Input;
using Files.Data.Commands.HotKey;
using Files.Data.Commands.Manager;

namespace Files.Data.Commands;

public interface IRichCommand : ICommand, INotifyPropertyChanging, INotifyPropertyChanged
{
	CommandCodes Code { get; }

	string Label { get; }

	string LabelWithHotKey { get; }

	string? Icon { get; }

	string AutomationName { get; }

	string Description { get; }

	bool IsCustomHotKeys { get; }

	string? HotKeyText { get; }

	HotKeyCollection HotKeys { get; set; }

	HotKeyCollection DefaultHotKeys { get; }

	bool IsToggle { get; }

	bool IsOn { get; set; }

	bool IsExecutable { get; }

	bool IsAccessibleGlobally { get; }

	Task ExecuteAsync(object? parameter = null);

	void ExecuteTapped(object sender, TappedEventArgs e);
}
