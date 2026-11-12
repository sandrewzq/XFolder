using System.ComponentModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.DependencyInjection;
using Files.Assets.Lang;
using Files.Data.Commands.HotKey;
using Files.Data.Contexts.ContentPage;
using Files.Data.Enums;
using Files.Helpers.UI;

namespace Files.Actions;

internal sealed class CreateNewFileAction : BaseUIAction, IAction
{
	private readonly IContentPageContext context;

	public string Label => Resources.File;

	public string Description => Resources.NewFile;

	public string? Icon => "document_add_regular";

	public HotKey HotKey => HotKey.None;

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

	public CreateNewFileAction()
	{
		context = Ioc.Default.GetRequiredService<IContentPageContext>();
		context.PropertyChanged += Context_PropertyChanged;
	}

	public async Task ExecuteAsync(object? parameter = null)
	{
		if (context.ShellView != null)
		{
			await UIFilesystemHelpers.CreateFileFromDialogResultTypeAsync(AddItemDialogItemType.File, null, context.ShellView);
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
