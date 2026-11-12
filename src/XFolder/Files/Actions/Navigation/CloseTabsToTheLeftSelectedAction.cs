using System.ComponentModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.DependencyInjection;
using Files.Assets.Lang;
using Files.Data.Contexts;
using Files.ViewModels.Shells;

namespace Files.Actions.Navigation;

internal sealed class CloseTabsToTheLeftSelectedAction : CloseTabBaseAction
{
	private readonly IMultiPanesContext multiPanesContext;

	public override string Label => Resources.CloseTabsToTheLeft;

	public override string Description => Resources.CloseTabsToTheLeftSelectedDescription;

	public CloseTabsToTheLeftSelectedAction()
	{
		multiPanesContext = Ioc.Default.GetRequiredService<IMultiPanesContext>();
	}

	public override Task ExecuteAsync(object? parameter = null)
	{
		BaseShellViewModel activePaneViewModel = multiPanesContext.ActivePaneViewModel;
		if (activePaneViewModel?.SelectedTab != null)
		{
			activePaneViewModel.CloseTabsToTheLeft(activePaneViewModel.SelectedTab);
		}
		return Task.CompletedTask;
	}

	protected override bool GetIsExecutable()
	{
		return context.SelectedTabIndex > 0;
	}

	protected override void Context_PropertyChanged(object? sender, PropertyChangedEventArgs e)
	{
		if (e.PropertyName == "SelectedTabIndex")
		{
			OnPropertyChanged("IsExecutable");
		}
	}
}
