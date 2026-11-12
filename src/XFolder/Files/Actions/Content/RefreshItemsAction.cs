using System.ComponentModel;
using System.Threading.Tasks;
using Avalonia.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using Files.Assets.Lang;
using Files.Data.Commands.HotKey;
using Files.Data.Contexts;
using Files.Data.Contexts.ContentPage;
using Files.ViewModels.Layouts;
using Files.ViewModels.Shells;
using Files.Views.Layouts;

namespace Files.Actions.Content;

internal sealed class RefreshItemsAction : ObservableObject, IAction
{
	private readonly IContentPageContext contentPageContext;

	private readonly IMultitaskingContext multitaskingContext;

	public string Label => Resources.Refresh;

	public string Description => Resources.RefreshItemsDescription;

	public HotKey HotKey => new HotKey(Key.F5);

	public bool IsExecutable => GetIsExecutable();

	public RefreshItemsAction()
	{
		contentPageContext = Ioc.Default.GetRequiredService<IContentPageContext>();
		multitaskingContext = Ioc.Default.GetRequiredService<IMultitaskingContext>();
		contentPageContext.PropertyChanged += Context_PropertyChanged;
		multitaskingContext.PropertyChanged += MultitaskingContext_PropertyChanged;
	}

	public Task ExecuteAsync(object? parameter = null)
	{
		BaseShellViewModel baseShellViewModel = contentPageContext.ShellView?.InstanceViewModel;
		BaseLayoutViewModel baseLayoutViewModel = baseShellViewModel?.SelectedTab?.LayoutViewModel;
		string refreshPath = GetRefreshPath(baseShellViewModel);
		if (baseLayoutViewModel == null || string.IsNullOrWhiteSpace(refreshPath))
		{
			return Task.CompletedTask;
		}
		baseLayoutViewModel.RefreshItems(refreshPath);
		return Task.CompletedTask;
	}

	private bool GetIsExecutable()
	{
		BaseShellViewModel baseShellViewModel = contentPageContext.ShellView?.InstanceViewModel;
		BaseLayoutViewModel baseLayoutViewModel = baseShellViewModel?.SelectedTab?.LayoutViewModel;
		if (baseLayoutViewModel != null)
		{
			BaseLayoutView? layoutView = baseLayoutViewModel.LayoutView;
			if (layoutView == null || !layoutView.IsRenamingItem)
			{
				return !string.IsNullOrWhiteSpace(GetRefreshPath(baseShellViewModel));
			}
		}
		return false;
	}

	private static string? GetRefreshPath(BaseShellViewModel? shellViewModel)
	{
		if (!string.IsNullOrWhiteSpace(shellViewModel?.WorkingDirectory))
		{
			return shellViewModel.WorkingDirectory;
		}
		return shellViewModel?.SelectedTab?.NavPath;
	}

	private void Context_PropertyChanged(object? sender, PropertyChangedEventArgs e)
	{
		string propertyName = e.PropertyName;
		if (propertyName == "ShellView" || propertyName == "PageType")
		{
			OnPropertyChanged("IsExecutable");
		}
	}

	private void MultitaskingContext_PropertyChanged(object? sender, PropertyChangedEventArgs e)
	{
		string propertyName = e.PropertyName;
		if (propertyName == "SelectedTabViewModel" || propertyName == "SelectedTabIndex")
		{
			OnPropertyChanged("IsExecutable");
		}
	}
}
