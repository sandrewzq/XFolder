using System.Threading.Tasks;
using Files.Data.Commands.HotKey;

namespace Files.Actions;

public interface IAction
{
	string Label { get; }

	string Description { get; }

	string? Icon => null;

	HotKey HotKey => HotKey.None;

	HotKey SecondHotKey => HotKey.None;

	HotKey ThirdHotKey => HotKey.None;

	HotKey MediaHotKey => HotKey.None;

	bool IsExecutable => true;

	bool IsAccessibleGlobally => true;

	Task ExecuteAsync(object? parameter = null);
}
