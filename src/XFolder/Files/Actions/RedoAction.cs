using System.ComponentModel;
using System.Threading.Tasks;
using Avalonia.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using Files.Assets.Lang;
using Files.Data.Commands.HotKey;
using Files.Data.Contexts.ContentPage;

namespace Files.Actions;

internal sealed class RedoAction : ObservableObject, IAction
{
	private readonly IContentPageContext context;

	public string Label => Resources.Redo;

	public string Description => Resources.RedoDescription;

	public HotKey HotKey => new HotKey(Key.Y, KeyModifiers.Meta);

	public bool IsExecutable
	{
		get
		{
			if (context.ShellView != null)
			{
				return App.HistoryWrapper.CanRedo();
			}
			return false;
		}
	}

	public RedoAction()
	{
		context = Ioc.Default.GetRequiredService<IContentPageContext>();
		context.PropertyChanged += Context_PropertyChanged;
		App.HistoryWrapper.PropertyChanged += HistoryWrapper_PropertyChanged;
	}

	public Task ExecuteAsync(object? parameter = null)
	{
		return context.ShellView.StorageHistoryHelpers.TryRedo();
	}

	private void Context_PropertyChanged(object? sender, PropertyChangedEventArgs e)
	{
		if (e.PropertyName == "ShellView")
		{
			OnPropertyChanged("IsExecutable");
		}
	}

	private void HistoryWrapper_PropertyChanged(object? sender, PropertyChangedEventArgs e)
	{
		if (e.PropertyName == "CanRedo")
		{
			OnPropertyChanged("IsExecutable");
		}
	}
}
