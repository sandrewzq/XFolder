using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using Files.Data.Contexts.ContentPage;

namespace Files.Actions;

internal abstract class BaseTransferItemAction : ObservableObject
{
	protected readonly IContentPageContext ContentPageContext = Ioc.Default.GetRequiredService<IContentPageContext>();

	public bool IsExecutable => ContentPageContext.HasSelection;

	public BaseTransferItemAction()
	{
		ContentPageContext.PropertyChanged += ContentPageContext_PropertyChanged;
	}

	private void ContentPageContext_PropertyChanged(object? sender, PropertyChangedEventArgs e)
	{
		if (e.PropertyName == "HasSelection")
		{
			OnPropertyChanged("IsExecutable");
		}
	}
}
