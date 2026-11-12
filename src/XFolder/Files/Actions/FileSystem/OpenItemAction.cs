using System.ComponentModel;
using System.Threading.Tasks;
using Avalonia.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using Files.Assets.Lang;
using Files.Data.Commands.HotKey;
using Files.Data.Contexts.ContentPage;
using Files.Helpers;
using Files.Helpers.UI;

namespace Files.Actions.FileSystem;

internal sealed class OpenItemAction : ObservableObject, IAction
{
	private readonly IContentPageContext context;

	public string Label => Resources.Open;

	public string Description => Resources.OpenItemDescription;

	public string? Icon => "open_regular";

	public HotKey HotKey => new HotKey(Key.Return);

	public bool IsExecutable
	{
		get
		{
			if (context.HasSelection)
			{
				return UIHelpers.CanShowDialog;
			}
			return false;
		}
	}

	public OpenItemAction()
	{
		context = Ioc.Default.GetRequiredService<IContentPageContext>();
		context.PropertyChanged += Context_PropertyChanged;
	}

	public Task ExecuteAsync(object? parameter = null)
	{
		if (context.ShellView != null)
		{
			return NavigationHelpers.OpenSelectedItemsAsync();
		}
		return Task.CompletedTask;
	}

	private void Context_PropertyChanged(object? sender, PropertyChangedEventArgs e)
	{
		if (e.PropertyName == "HasSelection")
		{
			OnPropertyChanged("IsExecutable");
		}
	}
}
