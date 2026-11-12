using System.Threading.Tasks;
using Avalonia.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using Files.Assets.Lang;
using Files.Data.Commands.HotKey;
using Files.Data.Contexts.ContentPage;

namespace Files.Actions;

internal sealed class FocusNextPaneAction : ObservableObject, IAction
{
	private readonly IContentPageContext context;

	public string Label => Resources.FocusNextPane;

	public string Description => Resources.FocusNextPane;

	public HotKey HotKey => new HotKey(Key.Right, KeyModifiers.Control | KeyModifiers.Shift);

	public bool IsExecutable => true;

	public FocusNextPaneAction()
	{
		context = Ioc.Default.GetRequiredService<IContentPageContext>();
	}

	public Task ExecuteAsync(object? parameter = null)
	{
		context.ShellPanesView?.ActiveNextPane();
		return Task.CompletedTask;
	}
}
