using System.ComponentModel;
using System.Threading.Tasks;
using Avalonia.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using Files.Assets.Lang;
using Files.Data.Commands.HotKey;
using Files.Data.Contexts.ContentPage;
using Files.Helpers.UI;

namespace Files.Actions;

internal sealed class PasteItemAction : ObservableObject, IAction
{
	private readonly IContentPageContext context;

	public string Label => Resources.Paste;

	public string Description => Resources.PasteItemDescription;

	public HotKey HotKey => new HotKey(Key.V, KeyModifiers.Meta);

	public bool IsExecutable => GetIsExecutable();

	public PasteItemAction()
	{
		context = Ioc.Default.GetRequiredService<IContentPageContext>();
		context.PropertyChanged += Context_PropertyChanged;
		App.AppModel.PropertyChanged += AppModel_PropertyChanged;
	}

	public async Task ExecuteAsync(object? parameter = null)
	{
		if (context.ShellView != null)
		{
			await UIFilesystemHelpers.PasteItemAsync(context.ShellView.InstanceViewModel.WorkingDirectory, context.ShellView);
		}
	}

	public bool GetIsExecutable()
	{
		return App.AppModel.IsPasteEnabled();
	}

	private void Context_PropertyChanged(object? sender, PropertyChangedEventArgs e)
	{
		if (e.PropertyName == "PageType")
		{
			OnPropertyChanged("IsExecutable");
		}
	}

	private void AppModel_PropertyChanged(object? sender, PropertyChangedEventArgs e)
	{
		if (e.PropertyName == "IsPasteEnabled")
		{
			OnPropertyChanged("IsExecutable");
		}
	}
}
