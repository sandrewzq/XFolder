using System.Reflection;
using System.Threading.Tasks;

namespace Files.Helpers.Application;

public static class AppLifecycleHelper
{
	public static string AppVersion => Assembly.GetEntryAssembly().GetName().Version.ToString();

	static AppLifecycleHelper()
	{
	}

	public static async Task InitializeAppComponentsAsync()
	{
	}
}
