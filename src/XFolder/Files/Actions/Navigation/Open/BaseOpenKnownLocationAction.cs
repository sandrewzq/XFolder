using System;
using System.IO;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using Files.Data.Commands.HotKey;
using Files.Data.Contexts;
using Files.Helpers;

namespace Files.Actions.Navigation.Open;

internal abstract class BaseOpenKnownLocationAction : ObservableObject, IAction
{
	private readonly IMultiPanesContext multiPanesContext;

	public abstract string Label { get; }

	public virtual string Description => Label;

	public abstract HotKey HotKey { get; }

	public virtual bool IsExecutable
	{
		get
		{
			if (multiPanesContext.ActivePaneViewModel != null)
			{
				return !string.IsNullOrWhiteSpace(GetTargetPath());
			}
			return false;
		}
	}

	protected BaseOpenKnownLocationAction()
	{
		multiPanesContext = Ioc.Default.GetRequiredService<IMultiPanesContext>();
		multiPanesContext.ActivePaneChanged += MultiPanesContext_ActivePaneChanged;
	}

	public async Task ExecuteAsync(object? parameter = null)
	{
		string targetPath = GetTargetPath();
		if (!string.IsNullOrWhiteSpace(targetPath))
		{
			await NavigationHelpers.OpenPath(targetPath);
		}
	}

	protected abstract string? GetTargetPath();

	protected static string? GetHomePath()
	{
		return Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
	}

	protected static string? GetUserKnownFolderPath(string folderName)
	{
		string homePath = GetHomePath();
		if (!string.IsNullOrWhiteSpace(homePath))
		{
			return Path.Combine(homePath, folderName);
		}
		return null;
	}

	private void MultiPanesContext_ActivePaneChanged(object? sender, EventArgs e)
	{
		OnPropertyChanged("IsExecutable");
	}
}
