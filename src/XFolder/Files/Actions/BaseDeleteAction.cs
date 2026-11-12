using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.DependencyInjection;
using Files.Data.Contexts.ContentPage;
using Files.Data.Contracts;
using Files.Data.Items;
using Files.Helpers.UI;

namespace Files.Actions;

internal abstract class BaseDeleteAction : BaseUIAction
{
	protected readonly IContentPageContext context;

	private readonly IFoldersSettingsService foldersSettingsService;

	public override bool IsExecutable
	{
		get
		{
			if (context.HasSelection)
			{
				IShellView? shellView = context.ShellView;
				if (shellView != null && shellView.InstanceViewModel?.SelectedTab?.LayoutViewModel?.LayoutView?.IsRenamingItem == false)
				{
					return UIHelpers.CanShowDialog;
				}
			}
			return false;
		}
	}

	public BaseDeleteAction()
	{
		context = Ioc.Default.GetRequiredService<IContentPageContext>();
		foldersSettingsService = Ioc.Default.GetRequiredService<IFoldersSettingsService>();
		context.PropertyChanged += Context_PropertyChanged;
	}

	protected async Task DeleteItemsAsync(bool permanently)
	{
		IReadOnlyList<ListedItem> selectedItems = context.SelectedItems;
		IShellView shellView = context.ShellView;
		if (shellView != null)
		{
			await shellView.FilesystemHelpers.DeleteItemsAsync(selectedItems, foldersSettingsService.DeleteConfirmationPolicy, permanently, registerHistory: true);
		}
	}

	private void Context_PropertyChanged(object? sender, PropertyChangedEventArgs e)
	{
		if (e.PropertyName == "HasSelection")
		{
			OnPropertyChanged("IsExecutable");
		}
	}
}
