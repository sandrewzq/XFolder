using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using Files.Assets.Lang;
using Files.Helpers;
using Files.Helpers.Shared;

namespace Files.Actions.Open;

internal sealed class OpenAppSupportFolderAction : ObservableObject, IAction
{
	public string Label => Resources.OpenAppSupportFolder;

	public string Description => Resources.OpenAppSupportFolderDescription;

	public bool IsExecutable => true;

	public async Task ExecuteAsync(object? parameter = null)
	{
		await NavigationHelpers.OpenPath(PathHelpers.GetAppDataDirectory());
	}
}
