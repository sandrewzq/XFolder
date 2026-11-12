using System.ComponentModel;
using System.Threading.Tasks;
using Avalonia.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using Files.Assets.Lang;
using Files.Data.Commands.HotKey;
using Files.Data.Contexts.ContentPage;
using Files.Data.Contracts;
using Files.Data.Enums;
using Files.Data.Enums.Storage;
using Files.Data.Items;
using Files.ViewModels.Dialogs;

namespace Files.Actions;

internal sealed class RenameAction : ObservableObject, IAction
{
	private readonly IContentPageContext context;

	public string Label => Resources.Rename;

	public string Description => Resources.RenameDescription;

	public string? Icon => "rename_regular";

	public HotKey HotKey => new HotKey(Key.F2);

	public bool IsExecutable
	{
		get
		{
			if (context.ShellView != null && IsPageTypeValid())
			{
				return context.HasSelection;
			}
			return false;
		}
	}

	public RenameAction()
	{
		context = Ioc.Default.GetRequiredService<IContentPageContext>();
		context.PropertyChanged += Context_PropertyChanged;
	}

	public async Task ExecuteAsync(object? parameter = null)
	{
		if (context.SelectedItems.Count > 1)
		{
			BulkRenameDialogViewModel dlgViewModel = new BulkRenameDialogViewModel();
			if (await Ioc.Default.GetRequiredService<IDialogService>().ShowDialogAsync(dlgViewModel) == DialogResult.Primary)
			{
				string text = dlgViewModel.AdditionalData as string;
				if (!string.IsNullOrEmpty(text))
				{
					await DoCommitRenameAsync(text);
				}
			}
		}
		else
		{
			context.ShellView?.InstanceViewModel.SelectedTab.LayoutViewModel._itemManipulationModel.StartRenameItem();
		}
	}

	private bool IsPageTypeValid()
	{
		if (context.PageType != ContentPageTypes.None && context.PageType != ContentPageTypes.Home && context.PageType != ContentPageTypes.RecycleBin && context.PageType != ContentPageTypes.ZipFolder && context.PageType != ContentPageTypes.ReleaseNotes)
		{
			return context.PageType != ContentPageTypes.Settings;
		}
		return false;
	}

	private void Context_PropertyChanged(object? sender, PropertyChangedEventArgs e)
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

	private async Task DoCommitRenameAsync(string newName)
	{
		if (context.ShellView == null)
		{
			return;
		}
		foreach (ListedItem selectedItem in context.SelectedItems)
		{
			await context.ShellView.FilesystemHelpers.RenameAsync(selectedItem.ToStorageItemWithPath(), newName, NameCollisionOption.GenerateUniqueName, registerHistory: true, showExtensionDialog: false);
		}
	}
}
