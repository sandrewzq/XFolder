using System.ComponentModel;
using System.Threading.Tasks;
using Avalonia.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using Files.Assets.Lang;
using Files.Data.Commands.HotKey;
using Files.Data.Contexts.ContentPage;
using Files.UserControls.NavigationToolbar;

namespace Files.Actions;

internal sealed class EditPathAction : ObservableObject, IAction
{
	private readonly IContentPageContext context;

	public string Label => Resources.EditPath;

	public string Description => Resources.EditPathDescription;

	public string? Icon => "edit_regular";

	public HotKey HotKey => new HotKey(Key.L, KeyModifiers.Meta);

	public HotKey SecondHotKey => new HotKey(Key.D, KeyModifiers.Alt);

	public bool IsExecutable => GetNavigationToolbarViewModel() != null;

	public EditPathAction()
	{
		context = Ioc.Default.GetRequiredService<IContentPageContext>();
		context.PropertyChanged += Context_PropertyChanged;
	}

	public Task ExecuteAsync(object? parameter = null)
	{
		return GetNavigationToolbarViewModel()?.SwitchToPathMode() ?? Task.CompletedTask;
	}

	private NavigationToolbarViewModel? GetNavigationToolbarViewModel()
	{
		return context.ShellView?.InstanceViewModel?.NavigationToolbarViewModel;
	}

	private void Context_PropertyChanged(object? sender, PropertyChangedEventArgs e)
	{
		string propertyName = e.PropertyName;
		if (propertyName == "ShellView" || propertyName == "PageType")
		{
			OnPropertyChanged("IsExecutable");
		}
	}
}
