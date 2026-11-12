using System.ComponentModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using Files.Data.Commands.HotKey;
using Files.Data.Contexts;

namespace Files.Actions;

internal abstract class ToggleLayoutAction : ObservableObject, IToggleAction, IAction
{
	protected readonly IDisplayPageContext Context;

	protected abstract LayoutTypes LayoutType { get; }

	public abstract string Label { get; }

	public abstract string Description { get; }

	public abstract HotKey HotKey { get; }

	public abstract HotKey SecondHotKey { get; }

	public bool IsOn => Context.LayoutType == LayoutType;

	public virtual bool IsExecutable => true;

	public ToggleLayoutAction()
	{
		Context = Ioc.Default.GetRequiredService<IDisplayPageContext>();
		Context.PropertyChanged += Context_PropertyChanged;
	}

	public Task ExecuteAsync(object? parameter = null)
	{
		Context.LayoutType = LayoutType;
		return Task.CompletedTask;
	}

	private void Context_PropertyChanged(object? sender, PropertyChangedEventArgs e)
	{
		if (e.PropertyName == "LayoutType")
		{
			OnPropertyChanged("IsOn");
		}
		if (e.PropertyName != null)
		{
			OnContextChanged(e.PropertyName);
		}
	}

	protected virtual void OnContextChanged(string propertyName)
	{
	}
}
