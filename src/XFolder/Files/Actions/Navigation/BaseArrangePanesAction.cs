using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using Files.Data.Commands.HotKey;
using Files.Data.Contexts.ContentPage;
using Files.Data.Enums;

namespace Files.Actions.Navigation;

internal abstract class BaseArrangePanesAction : ObservableObject, IAction
{
	protected readonly IContentPageContext ContentPageContext = Ioc.Default.GetRequiredService<IContentPageContext>();

	public virtual string Label => string.Empty;

	public virtual string Description => string.Empty;

	public virtual bool IsExecutable => true;

	public virtual HotKey HotKey => HotKey.None;

	protected abstract ShellPaneArrangement Arrangement { get; }

	public Task ExecuteAsync(object? parameter = null)
	{
		ContentPageContext.ShellPanesView.ArrangePanes(Arrangement);
		return Task.CompletedTask;
	}
}
