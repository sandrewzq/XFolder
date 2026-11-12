using System.ComponentModel;
using System.Threading.Tasks;
using Avalonia.Input;
using Files.Actions.Navigation;
using Files.Assets.Lang;
using Files.Data.Commands.HotKey;
using Files.Helpers;

namespace Files.Actions;

internal sealed class CloseSelectedTabAction : CloseTabBaseAction
{
	public override string Label => Resources.CloseTab;

	public override string Description => Resources.CloseSelectedTabDescription;

	public string? Icon => "close_regular";

	public override HotKey HotKey => new HotKey(Key.W, KeyModifiers.Meta);

	public override Task ExecuteAsync(object? parameter = null)
	{
		NavigationHelpers.CloseSelectedTab();
		return Task.CompletedTask;
	}

	protected override bool GetIsExecutable()
	{
		if (context.TabCount > 1)
		{
			return context.SelectedTabViewModel != null;
		}
		return false;
	}

	protected override void Context_PropertyChanged(object? sender, PropertyChangedEventArgs e)
	{
		string propertyName = e.PropertyName;
		if (propertyName == "TabCount" || propertyName == "SelectedTabViewModel")
		{
			OnPropertyChanged("IsExecutable");
		}
	}
}
