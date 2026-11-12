using System;
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

internal sealed class SaveWorkspaceAsAction : IAction
{
	private readonly ISessionService _sessionService;

	private readonly IMultiPanesContext _multiPanesContext;

	public string Label => Resources.SaveWorkspaceAs;

	public string Description => Resources.SaveWorkspaceTitle;

	public string? Icon => "save_as_regular";

	public HotKey HotKey => new HotKey(Key.S, KeyModifiers.Shift | KeyModifiers.Meta);

	public bool IsExecutable => true;

	public SaveWorkspaceAsAction()
	{
		_sessionService = Ioc.Default.GetRequiredService<ISessionService>();
		_multiPanesContext = Ioc.Default.GetRequiredService<IMultiPanesContext>();
	}

	public async Task ExecuteAsync(object? parameter = null)
	{
		try
		{
			IStorageProvider storageProvider = GetStorageProvider();
			if (storageProvider == null)
			{
				return;
			}
			IStorageFile storageFile = await storageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
			{
				Title = Resources.SaveWorkspaceTitle,
				FileTypeChoices = new FilePickerFileType[1]
				{
					new FilePickerFileType(Resources.XFolderWorkspaceDescription)
					{
						Patterns = new string[1] { "*.xfd" }
					}
				}
			});
			if (storageFile != null)
			{
				_multiPanesContext.PanesView?.SaveState();
				SessionModel sessionModel = _sessionService.RestoreSession();
				if (sessionModel != null)
				{
					_sessionService.SaveWorkspace(sessionModel, storageFile.Path.LocalPath);
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
