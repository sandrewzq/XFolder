using System.ComponentModel;
using System.Threading.Tasks;
using Avalonia.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using Files.Assets.Lang;
using Files.Data.Commands.HotKey;
using Files.Data.Contexts.ContentPage;
using Files.Data.Items;
using Files.Helpers.UI;

namespace Files.Actions;

internal sealed class PasteItemToSelectionAction : ObservableObject, IAction
{
	private readonly IContentPageContext context;

	public string Label => Resources.PasteToSelectedFolder;

	public string Description => Resources.PasteItemToSelectionDescription;

	public HotKey HotKey => new HotKey(Key.V, KeyModifiers.Shift | KeyModifiers.Meta);

	public bool IsExecutable => GetIsExecutable();

	public PasteItemToSelectionAction()
	{
		context = Ioc.Default.GetRequiredService<IContentPageContext>();
		context.PropertyChanged += Context_PropertyChanged;
		App.AppModel.PropertyChanged += AppModel_PropertyChanged;
	}

	public async Task ExecuteAsync(object? parameter = null)
	{
		if (context.ShellView != null)
		{
			ListedItem selectedItem = context.SelectedItem;
			await UIFilesystemHelpers.PasteItemAsync((selectedItem != null) ? selectedItem.ItemPath : context.ShellView.InstanceViewModel.WorkingDirectory, context.ShellView);
		}
	}

	public bool GetIsExecutable()
	{
		if (!App.AppModel.IsPasteEnabled())
		{
			return false;
		}
		ContentPageTypes pageType = context.PageType;
		if ((pageType - 1 <= ContentPageTypes.Home || pageType - 6 <= ContentPageTypes.RecycleBin) ? true : false)
		{
			return false;
		}
		if (!context.HasSelection)
		{
			return true;
		}
		ListedItem? selectedItem = context.SelectedItem;
		if (selectedItem != null && selectedItem.IsFolder)
		{
			return UIHelpers.CanShowDialog;
		}
		return false;
	}

	private void Context_PropertyChanged(object? sender, PropertyChangedEventArgs e)
	{
		string propertyName = e.PropertyName;
		if (propertyName == "PageType" || propertyName == "SelectedItem")
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
