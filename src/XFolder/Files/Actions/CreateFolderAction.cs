using System.ComponentModel;
using System.Threading.Tasks;
using Avalonia.Input;
using CommunityToolkit.Mvvm.DependencyInjection;
using Files.Assets.Lang;
using Files.Data.Commands.HotKey;
using Files.Data.Contexts.ContentPage;
using Files.Data.Enums;
using Files.Helpers.UI;

namespace Files.Actions;

internal sealed class CreateFolderAction : BaseUIAction, IAction
{
	private readonly IContentPageContext context;

	public string Label => Resources.Folder;

	public string Description => Resources.CreateFolderDescription;

	public string? Icon => "folder_add_regular";

	public HotKey HotKey => new HotKey(Key.N, KeyModifiers.Shift | KeyModifiers.Meta);

	public override bool IsExecutable
	{
		get
		{
			if (context.CanCreateItem)
			{
				return UIHelpers.CanShowDialog;
			}
			return false;
		}
	}

	public CreateFolderAction()
	{
		context = Ioc.Default.GetRequiredService<IContentPageContext>();
		context.PropertyChanged += Context_PropertyChanged;
	}

	public async Task ExecuteAsync(object? parameter = null)
	{
		if (context.ShellView != null)
		{
			await UIFilesystemHelpers.CreateFileFromDialogResultTypeAsync(AddItemDialogItemType.Folder, null, context.ShellView);
		}
	}

	private void Context_PropertyChanged(object? sender, PropertyChangedEventArgs e)
	{
		string propertyName = e.PropertyName;
		if (propertyName == "CanCreateItem" || propertyName == "HasSelection")
		{
			OnPropertyChanged("IsExecutable");
		}
	}
}
