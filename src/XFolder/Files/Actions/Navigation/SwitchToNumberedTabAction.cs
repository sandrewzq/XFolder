using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.DependencyInjection;
using Files.Assets.Lang;
using Files.Data.Commands.HotKey;
using Files.Data.Contexts;
using Files.ViewModels.Items;
using Files.ViewModels.Shells;

namespace Files.Actions.Navigation;

internal abstract class SwitchToNumberedTabAction : IAction
{
	private readonly int targetIndex;

	private readonly string resourceKey;

	private readonly HotKey hotKey;

	private readonly IMultiPanesContext multiPanesContext;

	public string Label => Resources.ResourceManager.GetString(resourceKey) ?? resourceKey;

	public string Description => Label;

	public HotKey HotKey => hotKey;

	public bool IsExecutable => true;

	protected SwitchToNumberedTabAction(int targetIndex, string resourceKey, HotKey hotKey)
	{
		this.targetIndex = targetIndex;
		this.resourceKey = resourceKey;
		this.hotKey = hotKey;
		multiPanesContext = Ioc.Default.GetRequiredService<IMultiPanesContext>();
	}

	public Task ExecuteAsync(object? parameter = null)
	{
		BaseShellViewModel activePaneViewModel = multiPanesContext.ActivePaneViewModel;
		ObservableCollection<TabItemViewModel> observableCollection = activePaneViewModel?.TabItems;
		if (observableCollection == null || observableCollection.Count == 0)
		{
			return Task.CompletedTask;
		}
		int num = ResolveTargetIndex(observableCollection.Count);
		if (num >= 0 && num < observableCollection.Count)
		{
			activePaneViewModel.SelectedTab = observableCollection[num];
		}
		return Task.CompletedTask;
	}

	private int ResolveTargetIndex(int tabCount)
	{
		if (targetIndex < 0)
		{
			return tabCount - 1;
		}
		return targetIndex;
	}
}
