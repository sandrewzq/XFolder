using System.ComponentModel;
using System.Threading.Tasks;
using Avalonia.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using Files.Assets.Lang;
using Files.Data.Commands.HotKey;
using Files.Data.Contexts.ContentPage;
using Files.Data.Enums.Navigation;
using Files.Helpers.Shared;
using Files.ViewModels.Shells;

namespace Files.Actions.Navigation;

internal sealed class NavigateUpAction : ObservableObject, IAction
{
	private readonly IContentPageContext context;

	public string Label => Resources.Up;

	public string Description => Resources.NavigateUpDescription;

	public HotKey HotKey => new HotKey(Key.Back);

	public bool IsExecutable => context.CanNavigateToParent;

	public NavigateUpAction()
	{
		context = Ioc.Default.GetRequiredService<IContentPageContext>();
		context.PropertyChanged += Context_PropertyChanged;
	}

	public Task ExecuteAsync(object? parameter = null)
	{
		BaseShellViewModel instanceViewModel = context.ShellView.InstanceViewModel;
		string parentPath = PathHelpers.GetParentPath(instanceViewModel.WorkingDirectory);
		instanceViewModel.SetWorkingDirectory(parentPath, SetNavPathLocationEnum.NavigateToParent);
		return Task.CompletedTask;
	}

	private void Context_PropertyChanged(object? sender, PropertyChangedEventArgs e)
	{
		if (e.PropertyName == "CanNavigateToParent")
		{
			OnPropertyChanged("IsExecutable");
		}
	}
}
