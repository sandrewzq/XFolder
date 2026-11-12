using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Logging;
using CommunityToolkit.Mvvm.DependencyInjection;
using Files.Assets.Lang;
using Files.Data.Commands;
using Files.Data.Commands.Manager;
using Files.Data.Contexts;
using Files.Data.Contexts.ContentPage;
using Files.Data.Contexts.Multitasking;
using Files.Data.Contracts;
using Files.Data.Models;
using Files.Helpers.Shared;
using Files.Services.App;
using Files.Services.NativePlugin;
using Files.Services.PreviewPopupProviders;
using Files.Services.Settings;
using Files.Services.Storage;
using Files.Utils.Global;
using Files.Utils.Logger;
using Files.Utils.Serialization;
using Files.Utils.Storage.Helpers;
using Files.Utils.Storage.History;
using Files.ViewModels.Settings;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Files;

internal sealed class Program
{
	private static int _isCrashing;

	private static void ConfigureServices()
	{
		ServiceCollection services = new ServiceCollection();
		services.AddSingleton<IUserSettingsService, UserSettingsService>();
		services.AddSingleton<IAppearanceSettingsService, AppearanceSettingsService>((IServiceProvider sp) => new AppearanceSettingsService(GetUserSettingsSharingContext(sp)));
		services.AddSingleton<IGeneralSettingsService, GeneralSettingsService>((IServiceProvider sp) => new GeneralSettingsService(GetUserSettingsSharingContext(sp)));
		services.AddSingleton<IFoldersSettingsService, FoldersSettingsService>((IServiceProvider sp) => new FoldersSettingsService(GetUserSettingsSharingContext(sp)));
		services.AddSingleton<ILayoutSettingsService, LayoutSettingsService>((IServiceProvider sp) => new LayoutSettingsService(GetUserSettingsSharingContext(sp)));
		services.AddSingleton<IMultiPanesContext, MultiPanesContext>();
		services.AddSingleton<IContentPageContext, ContentPageContext>();
		services.AddSingleton<IDisplayPageContext, DisplayPageContext>();
		services.AddSingleton<IMultitaskingContext, MultitaskingContext>();
		services.AddSingleton<ICommandManager, CommandManager>();
		services.AddSingleton<IModifiableCommandManager, ModifiableCommandManager>();
		services.AddSingleton<IDialogService, AppDialogService>();
		services.AddSingleton<ISessionService, SessionService>();
		services.AddSingleton<IQuickAccessService, QuickAccessService>();
		services.AddSingleton<IAppThemeService, AppThemeService>();
		services.AddSingleton<IPreviewPopupService, PreviewPopupService>();
		services.AddSingleton<DeviceManager>();
		services.AddSingleton<MacStorageDeviceWatcher>();
		services.AddSingleton<IRemovableDrivesService, StorageDevicesService>();
		services.AddSingleton<UpdateService>();
		services.AddSingleton<VersionExpirationService>();
		services.AddTransient<SettingsWindowViewModel>();
		services.AddSingleton<DrivesViewModel>();
		services.AddSingleton<AppModel>();
		services.AddSingleton<StorageHistoryWrapper>();
		services.AddLogging((ILoggingBuilder loggingBuilder) =>
		{
			loggingBuilder.ClearProviders();
			loggingBuilder.AddProvider(new FileLoggerProvider("debug.log"));
			loggingBuilder.SetMinimumLevel(LogLevel.Information);
		});
		Ioc.Default.ConfigureServices(services.BuildServiceProvider());
	}

	private static ISettingsSharingContext GetUserSettingsSharingContext(IServiceProvider serviceProvider)
	{
		return ((UserSettingsService)serviceProvider.GetRequiredService<IUserSettingsService>()).GetSharingContext();
	}

	[STAThread]
	public static void Main(string[] args)
	{
		AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
		TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;
		try
		{
			ConfigureServices();
			BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
		}
		catch (Exception ex)
		{
			HandleException(ex);
		}
	}

	private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
	{
		if (e.ExceptionObject is Exception ex)
		{
			HandleException(ex);
		}
	}

	private static void TaskScheduler_UnobservedTaskException(object? sender, UnobservedTaskExceptionEventArgs e)
	{
		HandleException(e.Exception);
		e.SetObserved();
	}

	private static void HandleException(Exception ex)
	{
		if (Interlocked.Exchange(ref _isCrashing, 1) != 0)
		{
			return;
		}
		try
		{
			ILogger<Program> service = Ioc.Default.GetService<ILogger<Program>>();
			if (service != null)
			{
				service.LogCritical(ex, "Unhandled exception occurred");
			}
			File.AppendAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "crash.log"), $"[{DateTime.Now}] {ex}{Environment.NewLine}");
			int num = MacPluginInterop.ShowAlertDialog(Resources.ExceptionNotificationHeader, ex.Message, Resources.Restart, Resources.ClearCacheAndRestart, Resources.Exit);
			if (num == 2)
			{
				string path = Path.Combine(PathHelpers.GetAppDataDirectory(), "cache");
				if (Directory.Exists(path))
				{
					try
					{
						Directory.Delete(path, recursive: true);
					}
					catch (Exception exception)
					{
						if (service != null)
						{
							service.LogError(exception, "Failed to delete cache directory during crash handling");
						}
					}
				}
			}
			if (num != 1 && num != 2)
			{
				return;
			}
			string processPath = Environment.ProcessPath;
			if (!string.IsNullOrEmpty(processPath))
			{
				int num2 = processPath.IndexOf(".app", StringComparison.OrdinalIgnoreCase);
				if (num2 >= 0)
				{
					string text = processPath.Substring(0, num2 + 4);
					Process.Start("open", "-n \"" + text + "\"");
				}
			}
		}
		catch
		{
		}
		finally
		{
			Environment.Exit(1);
		}
	}

	public static AppBuilder BuildAvaloniaApp()
	{
		return AppBuilder.Configure<App>().UsePlatformDetect().WithInterFont()
			.LogToTrace(LogEventLevel.Warning);
	}
}
