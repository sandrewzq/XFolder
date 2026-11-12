using System.ComponentModel;
using System.Threading.Tasks;
using Avalonia.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using Files.Assets.Lang;
using Files.Data.Commands.HotKey;
using Files.Data.Contexts.ContentPage;

namespace Files.Actions;

internal sealed class UndoAction : ObservableObject, IAction
{
	private readonly IContentPageContext context;

	public string Label => Resources.Undo;

	public string Description => Resources.UndoDescription;

	public HotKey HotKey => new HotKey(Key.Z, KeyModifiers.Meta);

	public bool IsExecutable
	{
		get
		{
			if (context.ShellView != null)
			{
				return App.HistoryWrapper.CanUndo();
			}
			return false;
		}
	}

	public UndoAction()
	{
		context = Ioc.Default.GetRequiredService<IContentPageContext>();
		context.PropertyChanged += Context_PropertyChanged;
		App.HistoryWrapper.PropertyChanged += HistoryWrapper_PropertyChanged;
	}

	public Task ExecuteAsync(object? parameter = null)
	{
		if (context.ShellView != null)
		{
			return context.ShellView.StorageHistoryHelpers.TryUndo();
		}
		return Task.CompletedTask;
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
		if (e.PropertyName == "CanUndo")
		{
			OnPropertyChanged("IsExecutable");
		}
	}
}
