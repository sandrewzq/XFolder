using System.ComponentModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.DependencyInjection;
using Files.Assets.Lang;
using Files.Data.Contexts;
using Files.ViewModels.Shells;

namespace Files.Actions.Navigation;

internal sealed class CloseTabsToTheRightSelectedAction : CloseTabBaseAction
{
	private readonly IMultiPanesContext multiPanesContext;

	public override string Label => Resources.CloseTabsToTheRight;

	public override string Description => Resources.CloseTabsToTheRightSelectedDescription;

	public CloseTabsToTheRightSelectedAction()
	{
		multiPanesContext = Ioc.Default.GetRequiredService<IMultiPanesContext>();
	}

	public override Task ExecuteAsync(object? parameter = null)
	{
		BaseShellViewModel activePaneViewModel = multiPanesContext.ActivePaneViewModel;
		if (activePaneViewModel?.SelectedTab != null)
		{
			activePaneViewModel.CloseTabsToTheRight(activePaneViewModel.SelectedTab);
		}
		return Task.CompletedTask;
	}

	protected override bool GetIsExecutable()
	{
		return context.SelectedTabIndex < context.TabCount - 1;
	}

	protected override void Context_PropertyChanged(object? sender, PropertyChangedEventArgs e)
	{
		string propertyName = e.PropertyName;
		if (propertyName == "TabCount" || propertyName == "SelectedTabIndex")
		{
			OnPropertyChanged("IsExecutable");
		}
	}
}
