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

internal sealed class PreviousTabAction : ObservableObject, IAction
{
	private readonly IMultitaskingContext multitaskingContext;

	private readonly IMultiPanesContext multiPanesContext;

	public string Label => Resources.PreviousTab;

	public string Description => Resources.PreviousTabDescription;

	public bool IsExecutable => multitaskingContext.TabCount > 1;

	public HotKey HotKey => new HotKey(Key.Left, KeyModifiers.Meta);

	public PreviousTabAction()
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
			int selectedTabIndex = activePaneViewModel.SelectedTabIndex;
			int index = ((selectedTabIndex == 0) ? (activePaneViewModel.TabItems.Count - 1) : (selectedTabIndex - 1));
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
