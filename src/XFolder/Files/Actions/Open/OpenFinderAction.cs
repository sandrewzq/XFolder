using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Avalonia.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using Files.Assets.Lang;
using Files.Data.Commands.HotKey;
using Files.Data.Contexts;
using Files.Data.Contexts.ContentPage;
using Files.Helpers;

namespace Files.Actions.Open;

internal class OpenFinderAction : ObservableObject, IAction
{
	private readonly IContentPageContext context;

	public virtual string Label => Resources.OpenFinder;

	public virtual string Description => Resources.OpenFinder;

	public virtual HotKey HotKey => new HotKey(Key.E, KeyModifiers.Meta);

	public virtual bool IsExecutable => GetIsExecutable();

	public OpenFinderAction()
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
		OpenFinder(text);
		return Task.CompletedTask;
	}

	private bool GetIsExecutable()
	{
		return true;
	}

	private void Context_PropertyChanged(object? sender, PropertyChangedEventArgs e)
	{
	}

	public bool OpenFinder(string path)
	{
		try
		{
			using Process process = new Process();
			process.StartInfo.FileName = "open";
			process.StartInfo.Arguments = "\"" + path + "\"";
			process.StartInfo.UseShellExecute = false;
			process.Start();
			return true;
		}
		catch (Exception)
		{
			return false;
		}
	}
}
