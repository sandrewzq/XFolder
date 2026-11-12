using System.Threading.Tasks;
using CommunityToolkit.Mvvm.DependencyInjection;
using Files.Assets.Lang;
using Files.Data.Contexts;
using Files.ViewModels.Shells;

namespace Files.Actions.Navigation;

internal sealed class CloseOtherTabsSelectedAction : CloseTabBaseAction
{
	private readonly IMultiPanesContext multiPanesContext;

	public override string Label => Resources.CloseOtherTabs;

	public override string Description => Resources.CloseOtherTabsSelectedDescription;

	public CloseOtherTabsSelectedAction()
	{
		multiPanesContext = Ioc.Default.GetRequiredService<IMultiPanesContext>();
	}

	public override Task ExecuteAsync(object? parameter = null)
	{
		BaseShellViewModel activePaneViewModel = multiPanesContext.ActivePaneViewModel;
		if (activePaneViewModel?.SelectedTab != null)
		{
			activePaneViewModel.CloseOtherTabs(activePaneViewModel.SelectedTab);
		}
		return Task.CompletedTask;
	}
}
