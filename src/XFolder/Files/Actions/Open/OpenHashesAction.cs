using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.DependencyInjection;
using Files.Assets.Lang;
using Files.Data.Contexts.ContentPage;
using Files.Data.Items;
using Files.Views.Properties;

namespace Files.Actions.Open;

internal sealed class OpenHashesAction : BaseUIAction, IAction
{
	private readonly IContentPageContext context;

	public string Label => Resources.Hashes;

	public string Description => Resources.Hashes;

	public override bool IsExecutable
	{
		get
		{
			if (base.IsExecutable)
			{
				return TryGetFileItem(null) != null;
			}
			return false;
		}
	}

	public OpenHashesAction()
	{
		context = Ioc.Default.GetRequiredService<IContentPageContext>();
		context.PropertyChanged += Context_PropertyChanged;
	}

	public Task ExecuteAsync(object? parameter = null)
	{
		ListedItem listedItem = TryGetFileItem(parameter);
		if (listedItem == null)
		{
			return Task.CompletedTask;
		}
		HashesWindow hashesWindow = new HashesWindow(listedItem);
		if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime classicDesktopStyleApplicationLifetime)
		{
			Window mainWindow = classicDesktopStyleApplicationLifetime.MainWindow;
			if (mainWindow != null)
			{
				Dispatcher.UIThread.Post(() =>
				{
					hashesWindow.ShowDialog(mainWindow);
				}, DispatcherPriority.Background);
				goto IL_0089;
			}
		}
		Dispatcher.UIThread.Post(() =>
		{
			hashesWindow.Show();
			hashesWindow.Activate();
		}, DispatcherPriority.Background);
		goto IL_0089;
		IL_0089:
		return Task.CompletedTask;
	}

	private ListedItem? TryGetFileItem(object? parameter)
	{
		if (parameter is ListedItem listedItem && IsFile(listedItem))
		{
			return listedItem;
		}
		ListedItem selectedItem = context.SelectedItem;
		if (selectedItem == null || !IsFile(selectedItem))
		{
			return null;
		}
		return selectedItem;
	}

	private static bool IsFile(ListedItem item)
	{
		if (!item.IsFolder)
		{
			return File.Exists(item.ItemPath);
		}
		return false;
	}

	private void Context_PropertyChanged(object? sender, PropertyChangedEventArgs e)
	{
		string propertyName = e.PropertyName;
		if ((propertyName == "SelectedItem" || propertyName == "SelectedItems") ? true : false)
		{
			OnPropertyChanged("IsExecutable");
		}
	}
}
