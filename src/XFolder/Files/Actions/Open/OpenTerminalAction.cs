using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using Files.Assets.Lang;
using Files.Data.Commands.HotKey;
using Files.Data.Contexts;
using Files.Data.Contexts.ContentPage;
using Files.Helpers;
using Files.Helpers.Shared;

namespace Files.Actions.Open;

internal class OpenTerminalAction : ObservableObject, IAction
{
	private readonly IContentPageContext context;

	public virtual string Label => Resources.OpenTerminal;

	public virtual string Description => Resources.OpenTerminalDescription;

	public virtual HotKey HotKey => new HotKey(Key.OemTilde, KeyModifiers.Control);

	public virtual bool IsExecutable => GetIsExecutable();

	public virtual bool IsAccessibleGlobally => true;

	public OpenTerminalAction()
	{
		context = Ioc.Default.GetRequiredService<IContentPageContext>();
		context.PropertyChanged += Context_PropertyChanged;
	}

	public Task ExecuteAsync(object? parameter = null)
	{
		string text = parameter as string;
		if (string.IsNullOrWhiteSpace(text))
		{
			IMultiPanesContext requiredService = Ioc.Default.GetRequiredService<IMultiPanesContext>();
			text = (string.IsNullOrEmpty(requiredService.ActivePaneViewModel?.WorkingDirectory) ? NavigationHelpers.GetDefaultNewTabPath() : requiredService.ActivePaneViewModel.WorkingDirectory);
		}
		if (string.IsNullOrWhiteSpace(text))
		{
			return Task.CompletedTask;
		}
		if (OpenITermWithAppleScriptIfInstalled(text))
		{
			_ = 1;
		}
		else
			OpenTerminal(text);
		return Task.CompletedTask;
	}

	private bool OpenITermWithAppleScriptIfInstalled(string path)
	{
		try
		{
			if (string.IsNullOrWhiteSpace(path))
			{
				return false;
			}
			path = PathHelpers.NormalizePath(path);
			if (!PathHelpers.IsCanAccessFolder(path))
			{
				return false;
			}
			if (string.IsNullOrEmpty(GetITermAppPath()))
			{
				return false;
			}
			using Process process = Process.Start(BuildOsascriptStartInfo(BuildITermAppleScript(path)));
			if (process == null)
			{
				return false;
			}
			string value = process.StandardOutput.ReadToEnd();
			string value2 = process.StandardError.ReadToEnd();
			process.WaitForExit();
			if (process.ExitCode != 0)
			{
				string.IsNullOrEmpty(value2);
				return false;
			}
			string.IsNullOrEmpty(value);
			return true;
		}
		catch (Exception)
		{
			return false;
		}
	}

	private static string BuildITermAppleScript(string path)
	{
		string text = path.Replace("\\", "\\\\").Replace("\"", "\\\"");
		return "tell application \"iTerm\"\n\tif it is running then\n\t\tcreate window with default profile\n\telse\n\t\tlaunch\n\t\trepeat until it is running\n\t\t\tdelay 0.05\n\t\tend repeat\n\t\trepeat until (count of windows) > 0\n\t\t\tdelay 0.05\n\t\tend repeat\n\tend if\n\ttell current session of current window\n\t\twrite text \"cd \\\"" + text + "\\\"\"\n\tend tell\nend tell";
	}

	private static ProcessStartInfo BuildOsascriptStartInfo(string appleScript)
	{
		string[] array = appleScript.Replace("\r\n", "\n").Replace("\r", "\n").Split('\n');
		ProcessStartInfo processStartInfo = new ProcessStartInfo
		{
			FileName = "osascript",
			UseShellExecute = false,
			RedirectStandardOutput = true,
			RedirectStandardError = true,
			CreateNoWindow = true
		};
		string[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			string text = array2[i].TrimEnd();
			if (!string.IsNullOrWhiteSpace(text))
			{
				processStartInfo.ArgumentList.Add("-e");
				processStartInfo.ArgumentList.Add(text);
			}
		}
		return processStartInfo;
	}

	private static string? GetITermAppPath()
	{
		return new List<string>
		{
			"/Applications/iTerm.app",
			Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "Applications", "iTerm.app")
		}.FirstOrDefault(Directory.Exists);
	}

	private bool GetIsExecutable()
	{
		return true;
	}

	private void Context_PropertyChanged(object? sender, PropertyChangedEventArgs e)
	{
	}

	public bool OpenTerminal(string path)
	{
		if (string.IsNullOrWhiteSpace(path))
		{
			return false;
		}
		try
		{
			path = PathHelpers.NormalizePath(path);
			if (!PathHelpers.IsCanAccessFolder(path))
			{
				return false;
			}
			using Process process = Process.Start(new ProcessStartInfo
			{
				FileName = "open",
				Arguments = "-a Terminal \"" + path + "\"",
				UseShellExecute = false,
				RedirectStandardOutput = true,
				RedirectStandardError = true,
				CreateNoWindow = true
			});
			if (process == null)
			{
				return false;
			}
			string value = process.StandardOutput.ReadToEnd();
			process.StandardError.ReadToEnd();
			process.WaitForExit();
			if (process.ExitCode != 0)
			{
				return false;
			}
			string.IsNullOrEmpty(value);
			return true;
		}
		catch (Exception)
		{
			return false;
		}
	}
}
