using System.ComponentModel;
using System.Threading.Tasks;
using Avalonia.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using Files.Assets.Lang;
using Files.Data.Commands.HotKey;
using Files.Data.Contexts.ContentPage;
using Files.Data.Contracts;
using Files.ViewModels.Layouts;

namespace Files.Actions.Content.Selection;

internal class SelectAllAction : ObservableObject, IAction
{
	private readonly IContentPageContext context;

	public virtual string Label => Resources.SelectAll;

	public virtual string Description => Resources.SelectAllDescription;

	public string? Icon => "select_all_regular";

	public virtual HotKey HotKey => new HotKey(Key.A, KeyModifiers.Meta);

	public virtual bool IsExecutable => GetIsExecutable();

	public SelectAllAction()
	{
		context = Ioc.Default.GetRequiredService<IContentPageContext>();
		context.PropertyChanged += Context_PropertyChanged;
	}

	public Task ExecuteAsync(object? parameter = null)
	{
		IShellView shellView = context.ShellView;
		if (shellView == null)
		{
			return Task.CompletedTask;
		}
		BaseLayoutViewModel baseLayoutViewModel = shellView.InstanceViewModel?.SelectedTab?.LayoutViewModel;
		if (baseLayoutViewModel == null)
		{
			return Task.CompletedTask;
		}
		baseLayoutViewModel._itemManipulationModel.SelectAllItems();
		return Task.CompletedTask;
	}

	private bool GetIsExecutable()
	{
		if (context.PageType != ContentPageTypes.Folder)
		{
			return false;
		}
		IShellView shellView = context.ShellView;
		if (shellView == null)
		{
			return false;
		}
		BaseLayoutViewModel baseLayoutViewModel = shellView.InstanceViewModel?.SelectedTab?.LayoutViewModel;
		if (baseLayoutViewModel == null)
		{
			return false;
		}
		int num = baseLayoutViewModel.Items?.Count ?? 0;
		int count = context.SelectedItems.Count;
		if (num == 0 || num == count)
		{
			return false;
		}
		return true;
	}

	private void Context_PropertyChanged(object? sender, PropertyChangedEventArgs e)
	{
		switch (e.PropertyName)
		{
		case "PageType":
		case "HasItem":
		case "SelectedItems":
			OnPropertyChanged("IsExecutable");
			break;
		}
	}
}
