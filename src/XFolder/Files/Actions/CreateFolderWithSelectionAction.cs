using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using Files.Assets.Lang;
using Files.Data.Contexts.ContentPage;
using Files.Data.Items;
using Files.Extensions;
using Files.Helpers.UI;

namespace Files.Actions;

internal sealed class CreateFolderWithSelectionAction : ObservableObject, IAction
{
	private readonly IContentPageContext context;

	public string Label => Resources.CreateFolderWithSelection;

	public string Description => Resources.CreateFolderWithSelectionDescription.GetLocalizedFormatResource(context.SelectedItems.Count);

	public string? Icon => "folder_add_regular";

	public bool IsExecutable
	{
		get
		{
			if (context.ShellView != null && context.PageType != ContentPageTypes.RecycleBin && context.PageType != ContentPageTypes.ZipFolder)
			{
				return context.HasSelection;
			}
			return false;
		}
	}

	public CreateFolderWithSelectionAction()
	{
		context = Ioc.Default.GetRequiredService<IContentPageContext>();
		context.PropertyChanged += Context_PropertyChanged;
	}

	public Task ExecuteAsync(object? parameter = null)
	{
		List<ListedItem> selectedItems = context.SelectedItems.ToList();
		return UIFilesystemHelpers.CreateFolderWithSelectionAsync(context.ShellView, selectedItems);
	}

	private void Context_PropertyChanged(object? sender, PropertyChangedEventArgs e)
	{
		switch (e.PropertyName)
		{
		case "ShellView":
		case "HasSelection":
		case "PageType":
			OnPropertyChanged("IsExecutable");
			break;
		}
	}
}
