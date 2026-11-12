using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using Files.Assets.Lang;
using Files.Data.Commands.HotKey;
using Files.Data.Contexts.ContentPage;
using Files.Data.Items;
using Files.Helpers;

namespace Files.Actions;

public class OpenInNewTabAction : ObservableObject, IAction
{
	protected IContentPageContext ContentPageContext { get; } = Ioc.Default.GetRequiredService<IContentPageContext>();

	public string Label => Resources.OpenInNewTab;

	public string Description => Resources.OpenDirectoryInNewTabDescription;

	public HotKey HotKey => default;

	public bool IsAccessibleGlobally => true;

	public bool IsExecutable
	{
		get
		{
			if (ContentPageContext.ShellView != null && ContentPageContext.SelectedItems.Count != 0 && ContentPageContext.SelectedItems.Count <= 5)
			{
				return ContentPageContext.SelectedItems.All((ListedItem x) => x.IsFolder);
			}
			return false;
		}
	}

	public OpenInNewTabAction()
	{
		ContentPageContext.PropertyChanged += Context_PropertyChanged;
	}

	public async Task ExecuteAsync(object? parameter = null)
	{
		foreach (ListedItem selectedItem in ContentPageContext.SelectedItems)
		{
			await NavigationHelpers.AddNewTabByPathAsync(selectedItem.ItemPath, switchToNewTab: false);
		}
	}

	protected virtual void Context_PropertyChanged(object? sender, PropertyChangedEventArgs e)
	{
		switch (e.PropertyName)
		{
		case "ShellView":
		case "PageType":
		case "HasSelection":
		case "SelectedItems":
			OnPropertyChanged("IsExecutable");
			break;
		}
	}
}
