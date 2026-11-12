using System.Threading.Tasks;
using Avalonia.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using Files.Assets.Lang;
using Files.Data.Commands.HotKey;
using Files.Data.Contexts.ContentPage;

namespace Files.Actions;

internal sealed class FocusPrevPaneAction : ObservableObject, IAction
{
	private readonly IContentPageContext context;

	public string Label => Resources.FocusPreviousPane;

	public string Description => Resources.FocusPreviousPane;

	public HotKey HotKey => new HotKey(Key.Left, KeyModifiers.Control | KeyModifiers.Shift);

	public bool IsExecutable => true;

	public FocusPrevPaneAction()
	{
		context = Ioc.Default.GetRequiredService<IContentPageContext>();
	}

	public Task ExecuteAsync(object? parameter = null)
	{
		context.ShellPanesView?.ActivePrevPane();
		return Task.CompletedTask;
	}
}
