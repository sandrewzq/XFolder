using System.ComponentModel;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Input.Platform;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using Files.Assets.Lang;
using Files.Data.Contexts.ContentPage;
using Files.Extensions;

namespace Files.Actions;

internal sealed class CopyPathWithQuotesAction : ObservableObject, IAction
{
	private readonly IContentPageContext context;

	public string Label => Resources.CopyPathWithQuotes;

	public string Description => Resources.CopyPathWithQuotesDescription;

	public bool IsExecutable
	{
		get
		{
			if (context.ShellView != null)
			{
				return IsPageTypeValid();
			}
			return false;
		}
	}

	public CopyPathWithQuotesAction()
	{
		context = Ioc.Default.GetRequiredService<IContentPageContext>();
		context.PropertyChanged += Context_PropertyChanged;
	}

	public async Task ExecuteAsync(object? parameter = null)
	{
		string text = parameter as string;
		if (string.IsNullOrWhiteSpace(text))
		{
			text = context.ShellView?.InstanceViewModel?.WorkingDirectory;
		}
		if (!string.IsNullOrWhiteSpace(text))
		{
			IClipboard clipboard = Application.Current.GetTopLevel()?.Clipboard;
			if (clipboard != null)
			{
				await clipboard.SetTextAsync("\"" + text + "\"");
			}
		}
	}

	private bool IsPageTypeValid()
	{
		if (context.PageType != ContentPageTypes.None && context.PageType != ContentPageTypes.Home && context.PageType != ContentPageTypes.RecycleBin && context.PageType != ContentPageTypes.ReleaseNotes)
		{
			return context.PageType != ContentPageTypes.Settings;
		}
		return false;
	}

	private void Context_PropertyChanged(object? sender, PropertyChangedEventArgs e)
	{
		string propertyName = e.PropertyName;
		if (propertyName == "ShellView" || propertyName == "PageType")
		{
			OnPropertyChanged("IsExecutable");
		}
	}
}
