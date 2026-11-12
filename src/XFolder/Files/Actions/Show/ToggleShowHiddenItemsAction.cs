using System.ComponentModel;
using System.Threading.Tasks;
using Avalonia.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using Files.Assets.Lang;
using Files.Data.Commands.HotKey;
using Files.Data.Contracts;

namespace Files.Actions.Show;

internal sealed class ToggleShowHiddenItemsAction : ObservableObject, IToggleAction, IAction
{
	private readonly IFoldersSettingsService settings;

	public string Label => Resources.ShowHiddenItems;

	public string Description => Resources.ToggleShowHiddenItemsDescription;

	public HotKey HotKey => new HotKey(Key.H, KeyModifiers.Meta);

	public bool IsOn => settings.ShowHiddenItems;

	public ToggleShowHiddenItemsAction()
	{
		settings = Ioc.Default.GetRequiredService<IFoldersSettingsService>();
		settings.PropertyChanged += Settings_PropertyChanged;
	}

	public Task ExecuteAsync(object? parameter = null)
	{
		settings.ShowHiddenItems = !settings.ShowHiddenItems;
		return Task.CompletedTask;
	}

	private void Settings_PropertyChanged(object? sender, PropertyChangedEventArgs e)
	{
		if (e.PropertyName == "ShowHiddenItems")
		{
			OnPropertyChanged("IsOn");
		}
	}
}
