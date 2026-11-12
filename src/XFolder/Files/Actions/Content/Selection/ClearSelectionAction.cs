using System.ComponentModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using Files.Assets.Lang;
using Files.Data.Contexts.ContentPage;
using Files.ViewModels.Layouts;
using Files.Views.Layouts;

namespace Files.Actions.Content.Selection;

internal sealed class ClearSelectionAction : ObservableObject, IAction
{
	private readonly IContentPageContext context;

	public string Label => Resources.ClearSelection;

	public string Description => Resources.ClearSelectionDescription;

	public string? Icon => "select_all_off_regular";

	public bool IsExecutable => GetIsExecutable();

	public ClearSelectionAction()
	{
		context = Ioc.Default.GetRequiredService<IContentPageContext>();
		context.PropertyChanged += Context_PropertyChanged;
	}

	public Task ExecuteAsync(object? parameter = null)
	{
		GetLayoutViewModel()?._itemManipulationModel.ClearSelection();
		return Task.CompletedTask;
	}

	private BaseLayoutViewModel? GetLayoutViewModel()
	{
		return context.ShellView?.InstanceViewModel?.SelectedTab?.LayoutViewModel;
	}

	private bool GetIsExecutable()
	{
		BaseLayoutViewModel layoutViewModel = GetLayoutViewModel();
		if (context.PageType == ContentPageTypes.Folder && context.HasSelection && layoutViewModel != null)
		{
			BaseLayoutView? layoutView = layoutViewModel.LayoutView;
			if (layoutView == null)
			{
				return true;
			}
			return !layoutView.IsRenamingItem;
		}
		return false;
	}

	private void Context_PropertyChanged(object? sender, PropertyChangedEventArgs e)
	{
		switch (e.PropertyName)
		{
		case "PageType":
		case "HasSelection":
		case "SelectedItems":
		case "ShellView":
			OnPropertyChanged("IsExecutable");
			break;
		}
	}
}
