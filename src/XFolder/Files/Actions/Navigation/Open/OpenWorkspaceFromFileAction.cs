using System;
using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Input;
using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.DependencyInjection;
using Files.Assets.Lang;
using Files.Data.Commands.HotKey;
using Files.Data.Contexts;
using Files.Data.Contracts;
using Files.Data.Models;

namespace Files.Actions.Navigation.Open;

internal sealed class OpenWorkspaceFromFileAction : IAction
{
	private readonly ISessionService _sessionService;

	private readonly IMultiPanesContext _multiPanesContext;

	public string Label => Resources.OpenWorkspaceFromFile;

	public string Description => Resources.OpenWorkspaceTitle;

	public HotKey HotKey => new HotKey(Key.O, KeyModifiers.Meta);

	public bool IsExecutable => true;

	public OpenWorkspaceFromFileAction()
	{
		_sessionService = Ioc.Default.GetRequiredService<ISessionService>();
		_multiPanesContext = Ioc.Default.GetRequiredService<IMultiPanesContext>();
	}

	public async Task ExecuteAsync(object? parameter = null)
	{
		try
		{
			string text = parameter as string;
			if (string.IsNullOrWhiteSpace(text))
			{
				IStorageProvider storageProvider = GetStorageProvider();
				if (storageProvider == null)
				{
					return;
				}
				text = (await storageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
				{
					Title = Resources.OpenWorkspaceTitle,
					AllowMultiple = false,
					FileTypeFilter = new FilePickerFileType[1]
					{
						new FilePickerFileType(Resources.XFolderWorkspaceDescription)
						{
							Patterns = new string[1] { "*.xfd" }
						}
					}
				})).FirstOrDefault()?.Path.LocalPath;
			}
			if (!string.IsNullOrWhiteSpace(text))
			{
				SessionModel sessionModel = _sessionService.LoadWorkspace(text);
				if (sessionModel != null)
				{
					_multiPanesContext.PanesView?.LoadSessionDirectly(sessionModel);
				}
			}
		}
		catch (Exception)
		{
		}
	}

	private static IStorageProvider? GetStorageProvider()
	{
		if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime classicDesktopStyleApplicationLifetime)
		{
			Window mainWindow = classicDesktopStyleApplicationLifetime.MainWindow;
			if (mainWindow != null)
			{
				return mainWindow.StorageProvider;
			}
		}
		return null;
	}
}
