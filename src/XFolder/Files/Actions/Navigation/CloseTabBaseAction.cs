using System.ComponentModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using Files.Data.Commands.HotKey;
using Files.Data.Contexts;

namespace Files.Actions.Navigation;

internal abstract class CloseTabBaseAction : ObservableObject, IAction
{
	protected readonly IMultitaskingContext context;

	public abstract string Label { get; }

	public abstract string Description { get; }

	public bool IsExecutable => GetIsExecutable();

	public virtual HotKey HotKey => HotKey.None;

	public virtual HotKey SecondHotKey => HotKey.None;

	public CloseTabBaseAction()
	{
		context = Ioc.Default.GetRequiredService<IMultitaskingContext>();
		context.PropertyChanged += Context_PropertyChanged;
	}

	public abstract Task ExecuteAsync(object? parameter = null);

	protected virtual bool GetIsExecutable()
	{
		return context.TabCount > 1;
	}

	protected virtual void Context_PropertyChanged(object? sender, PropertyChangedEventArgs e)
	{
		if (e.PropertyName == "TabCount")
		{
			OnPropertyChanged("IsExecutable");
		}
	}
}
