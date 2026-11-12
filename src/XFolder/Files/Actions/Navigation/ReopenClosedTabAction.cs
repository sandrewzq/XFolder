using System.ComponentModel;
using System.Threading.Tasks;
using Avalonia.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using Files.Assets.Lang;
using Files.Data.Commands.HotKey;
using Files.Data.Contexts;

namespace Files.Actions.Navigation;

internal sealed class ReopenClosedTabAction : ObservableObject, IAction
{
	private readonly IMultitaskingContext context;

	public string Label => Resources.ReopenClosedTab;

	public string Description => Resources.ReopenClosedTabDescription;

	public HotKey HotKey => new HotKey(Key.T, KeyModifiers.Shift | KeyModifiers.Meta);

	public bool IsExecutable
	{
		get
		{
			if (!context.IsRestoringClosedTab)
			{
				return context.RecentlyClosedTabCount > 0;
			}
			return false;
		}
	}

	public ReopenClosedTabAction()
	{
		context = Ioc.Default.GetRequiredService<IMultitaskingContext>();
		context.PropertyChanged += Context_PropertyChanged;
	}

	public Task ExecuteAsync(object? parameter = null)
	{
		return context.ReopenClosedTabAsync();
	}

	private void Context_PropertyChanged(object? sender, PropertyChangedEventArgs e)
	{
		string propertyName = e.PropertyName;
		if (propertyName == "RecentlyClosedTabCount" || propertyName == "IsRestoringClosedTab")
		{
			OnPropertyChanged("IsExecutable");
		}
	}
}
