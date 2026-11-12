using System.ComponentModel;
using System.Threading.Tasks;
using Avalonia.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using Files.Assets.Lang;
using Files.Data.Commands.HotKey;
using Files.Data.Contexts.ContentPage;

namespace Files.Actions.Navigation;

internal sealed class NavigateForwardAction : ObservableObject, IAction
{
	private readonly IContentPageContext context;

	public string Label => Resources.Forward;

	public string Description => Resources.NavigateForwardDescription;

	public HotKey HotKey => new HotKey(Key.Right, KeyModifiers.Alt);

	public bool IsExecutable => context.CanGoForward;

	public NavigateForwardAction()
	{
		context = Ioc.Default.GetRequiredService<IContentPageContext>();
		context.PropertyChanged += Context_PropertyChanged;
	}

	public Task ExecuteAsync(object? parameter = null)
	{
		context.ShellView.InstanceViewModel.GoForward();
		return Task.CompletedTask;
	}

	private void Context_PropertyChanged(object? sender, PropertyChangedEventArgs e)
	{
		if (e.PropertyName == "CanGoForward")
		{
			OnPropertyChanged("IsExecutable");
		}
	}
}
