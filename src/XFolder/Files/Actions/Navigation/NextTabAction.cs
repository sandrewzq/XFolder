using System.ComponentModel;
using System.Threading.Tasks;
using Avalonia.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using Files.Assets.Lang;
using Files.Data.Commands.HotKey;
using Files.Data.Contexts;
using Files.ViewModels.Shells;

namespace Files.Actions.Navigation;

internal sealed class NextTabAction : ObservableObject, IAction
{
	private readonly IMultitaskingContext multitaskingContext;

	private readonly IMultiPanesContext multiPanesContext;

	public string Label => Resources.NextTab;

	public string Description => Resources.NextTabDescription;

	public bool IsExecutable => multitaskingContext.TabCount > 1;

	public HotKey HotKey => new HotKey(Key.Right, KeyModifiers.Meta);

	public NextTabAction()
	{
		multitaskingContext = Ioc.Default.GetRequiredService<IMultitaskingContext>();
		multiPanesContext = Ioc.Default.GetRequiredService<IMultiPanesContext>();
		multitaskingContext.PropertyChanged += MultitaskingContext_PropertyChanged;
	}

	public Task ExecuteAsync(object? parameter = null)
	{
		BaseShellViewModel activePaneViewModel = multiPanesContext.ActivePaneViewModel;
		if (activePaneViewModel != null && activePaneViewModel.TabItems.Count > 1)
		{
			int num = activePaneViewModel.TabItems.Count - 1;
			int selectedTabIndex = activePaneViewModel.SelectedTabIndex;
			int index = ((selectedTabIndex != num) ? (selectedTabIndex + 1) : 0);
			activePaneViewModel.SelectedTab = activePaneViewModel.TabItems[index];
		}
		return Task.CompletedTask;
	}

	private void MultitaskingContext_PropertyChanged(object? sender, PropertyChangedEventArgs e)
	{
		if (e.PropertyName == "TabCount")
		{
			OnPropertyChanged("IsExecutable");
		}
	}
}
