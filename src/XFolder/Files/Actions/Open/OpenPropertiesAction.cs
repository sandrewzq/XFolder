using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using Files.Assets.Lang;
using Files.Data.Commands.HotKey;
using Files.Data.Contexts.ContentPage;
using Files.Data.Items;
using Files.Services.NativePlugin;

namespace Files.Actions.Open;

internal sealed class OpenPropertiesAction : ObservableObject, IAction
{
	private const int MaxInfoWindowCount = 5;

	private readonly IContentPageContext context;

	public string Label => Resources.ShowInfoInFinder;

	public string Description => Resources.ShowInfoInFinderDescription;

	public string? Icon => "info_regular";

	public HotKey HotKey => new HotKey(Key.I, KeyModifiers.Meta);

	public bool IsExecutable
	{
		get
		{
			int count = GetTargetPaths(null).Count;
			if (count > 0)
			{
				return count <= 5;
			}
			return false;
		}
	}

	public OpenPropertiesAction()
	{
		context = Ioc.Default.GetRequiredService<IContentPageContext>();
		context.PropertyChanged += Context_PropertyChanged;
	}

	public Task ExecuteAsync(object? parameter = null)
	{
		List<string> targetPaths = GetTargetPaths(parameter);
		if (targetPaths.Count == 0 || targetPaths.Count > 5)
		{
			return Task.CompletedTask;
		}
		MacPluginInterop.ShowItemInfoWindows(targetPaths);
		return Task.CompletedTask;
	}

	private List<string> GetTargetPaths(object? parameter)
	{
		if (!(parameter is string item))
		{
			if (!(parameter is ListedItem listedItem))
			{
				if (!(parameter is IEnumerable<string> paths))
				{
					if (parameter is IEnumerable<ListedItem> source)
					{
						return NormalizePaths(source.Select((ListedItem listedItem2) => listedItem2.ToStorageItemWithPath().Path));
					}
					return NormalizePaths(context.SelectedItems.Select((ListedItem listedItem2) => listedItem2.ToStorageItemWithPath().Path));
				}
				return NormalizePaths(paths);
			}
			return NormalizePaths(new _003C_003Ez__ReadOnlySingleElementList<string>(listedItem.ToStorageItemWithPath().Path));
		}
		return NormalizePaths(new _003C_003Ez__ReadOnlySingleElementList<string>(item));
	}

	private static List<string> NormalizePaths(IEnumerable<string?> paths)
	{
		return (from path in paths
			where !string.IsNullOrWhiteSpace(path)
			select (path) into path
			where File.Exists(path) || Directory.Exists(path)
			select path).Distinct(StringComparer.OrdinalIgnoreCase).ToList();
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
