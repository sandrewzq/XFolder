using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.DependencyInjection;
using Files.Assets.Lang;
using Files.Data.Contexts.ContentPage;
using Files.Data.Enums;
using Files.Data.Items;
using Files.Helpers;
using Files.Helpers.Shared;
using Files.Utils.Storage.Helpers;
using Files.Utils.Storage.History;

namespace Files.Actions.Content.ImageManipulation;

internal abstract class BaseRotateAction : BaseUIAction, IAction
{
	protected readonly IContentPageContext context;

	protected abstract bool RotateClockwise { get; }

	public abstract string Label { get; }

	public abstract string Description { get; }

	public override bool IsExecutable
	{
		get
		{
			if (base.IsExecutable && context.ShellView != null && context.PageType != ContentPageTypes.RecycleBin && context.PageType != ContentPageTypes.ZipFolder && context.PageType != ContentPageTypes.ReleaseNotes && context.PageType != ContentPageTypes.Settings && context.HasSelection)
			{
				return context.SelectedItems.All(IsRotatableItem);
			}
			return false;
		}
	}

	protected BaseRotateAction()
	{
		context = Ioc.Default.GetRequiredService<IContentPageContext>();
		context.PropertyChanged += Context_PropertyChanged;
	}

	public async Task ExecuteAsync(object? parameter = null)
	{
		List<ListedItem> selectedItems = context.SelectedItems.Where(IsRotatableItem).ToList();
		if (selectedItems.Count == 0)
		{
			return;
		}
		for (int i = 0; i < selectedItems.Count; i++)
		{
			FileThumbnailHelper.InvalidateImageThumbnail(selectedItems[i].ItemPath);
		}
		bool[] results = await Task.WhenAll(selectedItems.Select((ListedItem item) => Task.Run(() => ImageRotationHelper.Rotate90(item.ItemPath, RotateClockwise))));
		if (results.Any((bool x) => !x))
		{
			await DialogDisplayHelper.ShowTipAsync(string.Empty, Resources.FailedToRotateImage);
		}
		List<ListedItem> list = selectedItems.Where((ListedItem item, int index) => results[index]).ToList();
		if (list.Count > 0)
		{
			RegisterHistory(list, RotateClockwise);
		}
	}

	private static void RegisterHistory(List<ListedItem> successfulItems, bool rotateClockwise)
	{
		List<IStorageItemWithPath> source = successfulItems.Select((ListedItem listedItem) => StorageHelpers.FromPathAndType(listedItem.ItemPath, FilesystemItemType.File)).ToList();
		IStorageItemWithPath item = StorageHelpers.FromPathAndType(rotateClockwise ? "Clockwise" : "CounterClockwise", FilesystemItemType.File);
		App.HistoryWrapper.AddHistory(new StorageHistory(FileOperationType.ImageRotate, source, new List<IStorageItemWithPath> { item }));
	}

	private static bool IsRotatableItem(ListedItem item)
	{
		if (item.IsFolder || !File.Exists(item.ItemPath))
		{
			return false;
		}
		return ImageRotationHelper.CanRotateImage(item.FileExtension);
	}

	private void Context_PropertyChanged(object? sender, PropertyChangedEventArgs e)
	{
		switch (e.PropertyName)
		{
		case "ShellView":
		case "PageType":
		case "HasSelection":
		case "SelectedItem":
		case "SelectedItems":
			OnPropertyChanged("IsExecutable");
			break;
		}
	}
}
