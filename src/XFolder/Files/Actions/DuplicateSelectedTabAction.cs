using System.ComponentModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using Files.Assets.Lang;
using Files.Data.Commands.HotKey;
using Files.Data.Contexts;
using Files.Helpers;

namespace Files.Actions;

internal sealed class DuplicateSelectedTabAction : ObservableObject, IAction
{
	private readonly IMultitaskingContext context;

	public string Label => Resources.DuplicateTab;

	public string Description => Resources.DuplicateSelectedTabDescription;

	public string? Icon => "tab_desktop_copy_regular";

	public HotKey HotKey => HotKey.None;

	public bool IsExecutable => context.SelectedTabViewModel != null;

	public DuplicateSelectedTabAction()
	{
		context = Ioc.Default.GetRequiredService<IMultitaskingContext>();
		context.PropertyChanged += MultitaskingContext_PropertyChanged;
	}

	public async Task ExecuteAsync(object? parameter = null)
	{
		if (context.SelectedTabViewModel == null)
		{
			await NavigationHelpers.AddNewTabByPathAsync(null, switchToNewTab: true);
		}
		else
		{
			await NavigationHelpers.AddNewTabByParamAsync(context.SelectedTabViewModel.NavigationParameter, switchToNewTab: true, context.SelectedTabIndex + 1);
		}
	}

	private void MultitaskingContext_PropertyChanged(object? sender, PropertyChangedEventArgs e)
	{
		if (e.PropertyName == "SelectedTabViewModel")
		{
			OnPropertyChanged("IsExecutable");
		}
	}
}
