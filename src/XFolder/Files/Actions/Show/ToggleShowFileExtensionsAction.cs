using System.ComponentModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using Files.Assets.Lang;
using Files.Data.Contracts;

namespace Files.Actions.Show;

internal sealed class ToggleShowFileExtensionsAction : ObservableObject, IToggleAction, IAction
{
	private readonly IFoldersSettingsService settings;

	public string Label => Resources.ShowFileExtensions;

	public string Description => Resources.ToggleShowFileExtensionsDescription;

	public bool IsOn => settings.ShowFileExtensions;

	public ToggleShowFileExtensionsAction()
	{
		settings = Ioc.Default.GetRequiredService<IFoldersSettingsService>();
		settings.PropertyChanged += Settings_PropertyChanged;
	}

	public Task ExecuteAsync(object? parameter = null)
	{
		settings.ShowFileExtensions = !settings.ShowFileExtensions;
		return Task.CompletedTask;
	}

	private void Settings_PropertyChanged(object? sender, PropertyChangedEventArgs e)
	{
		if (e.PropertyName == "ShowFileExtensions")
		{
			OnPropertyChanged("IsOn");
		}
	}
}
