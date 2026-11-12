using System.Threading.Tasks;
using Avalonia.Input;
using Files.Assets.Lang;
using Files.Data.Commands.HotKey;
using Files.Helpers;

namespace Files.Actions.Navigation;

internal sealed class NewTabAction : IAction
{
	public string Label => Resources.NewTab;

	public string Description => Resources.NewTabDescription;

	public string? Icon => "tab_new_regular";

	public HotKey HotKey => new HotKey(Key.T, KeyModifiers.Meta);

	public bool IsExecutable => true;

	public Task ExecuteAsync(object? parameter = null)
	{
		return NavigationHelpers.AddNewTabByPathAsync(null, switchToNewTab: true);
	}
}
