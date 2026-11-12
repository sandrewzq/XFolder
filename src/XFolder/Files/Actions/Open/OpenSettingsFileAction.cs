using System.IO;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using Files.Assets.Lang;
using Files.Helpers;
using Files.Helpers.Shared;

namespace Files.Actions.Open;

internal sealed class OpenSettingsFileAction : ObservableObject, IAction
{
	public string Label => Resources.EditSettingsFile;

	public string Description => Resources.EditSettingsFileDescription;

	public bool IsExecutable => true;

	public async Task ExecuteAsync(object? parameter = null)
	{
		string path = Path.Combine(Path.Combine(PathHelpers.GetAppDataDirectory(), "cache"), "settings.json");
		if (File.Exists(path))
		{
			await NavigationHelpers.OpenPath(path);
		}
	}
}
