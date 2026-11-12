using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Media.Imaging;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel.__Internals;
using CommunityToolkit.Mvvm.DependencyInjection;
using Files.Data.Contracts;
using Files.Data.Enums;
using Files.Helpers;
using Files.Helpers.Shared;
using Files.Utils.Storage.Helpers;

namespace Files.Data.Items;

public class ListedItem : ObservableObject
{
	[ObservableProperty]
	private Bitmap? thumbnail;

	private double opacity = 1.0;

	[ObservableProperty]
	private bool isHiddenItem;

	[ObservableProperty]
	private string itemPath = string.Empty;

	private string displayItemType = string.Empty;

	private string itemNameRaw = string.Empty;

	private string fileSizeDisplay = string.Empty;

	private long fileSizeBytes;

	private DateTimeOffset itemDateModifiedReal;

	private DateTimeOffset itemDateCreatedReal;

	private DateTimeOffset itemDateAccessedReal;

	public string ItemTooltipText
	{
		get
		{
			string value = DisplayItemType;
			StringBuilder stringBuilder = new StringBuilder();
			StringBuilder stringBuilder2 = stringBuilder;
			StringBuilder stringBuilder3 = stringBuilder2;
			StringBuilder.AppendInterpolatedStringHandler handler = new StringBuilder.AppendInterpolatedStringHandler(0, 1, stringBuilder2);
			handler.AppendFormatted(Name);
			stringBuilder3.AppendLine(ref handler);
			if (!string.IsNullOrWhiteSpace(value))
			{
				stringBuilder2 = stringBuilder;
				StringBuilder stringBuilder4 = stringBuilder2;
				handler = new StringBuilder.AppendInterpolatedStringHandler(0, 1, stringBuilder2);
				handler.AppendFormatted(value);
				stringBuilder4.AppendLine(ref handler);
			}
			if (!string.IsNullOrWhiteSpace(FileSizeDisplay))
			{
				stringBuilder2 = stringBuilder;
				StringBuilder stringBuilder5 = stringBuilder2;
				handler = new StringBuilder.AppendInterpolatedStringHandler(0, 1, stringBuilder2);
				handler.AppendFormatted(FileSizeDisplay);
				stringBuilder5.AppendLine(ref handler);
			}
			stringBuilder2 = stringBuilder;
			StringBuilder stringBuilder6 = stringBuilder2;
			handler = new StringBuilder.AppendInterpolatedStringHandler(0, 1, stringBuilder2);
			handler.AppendFormatted(ItemDateModified);
			stringBuilder6.Append(ref handler);
			return stringBuilder.ToString();
		}
	}

	public bool ContainsFilesOrFolders { get; set; } = true;

	public double Opacity
	{
		get
		{
			return opacity;
		}
		set
		{
			SetProperty(ref opacity, value, "Opacity");
		}
	}

	public string DisplayItemType
	{
		get
		{
			return displayItemType;
		}
		private set
		{
			SetProperty(ref displayItemType, value, "DisplayItemType");
		}
	}

	public string ItemNameRaw
	{
		get
		{
			return itemNameRaw;
		}
		set
		{
			if (SetProperty(ref itemNameRaw, value, "ItemNameRaw"))
			{
				OnPropertyChanged("Name");
			}
		}
	}

	public virtual string Name
	{
		get
		{
			if (!IsFolder)
			{
				string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(itemNameRaw);
				if (!string.IsNullOrEmpty(fileNameWithoutExtension) && !Ioc.Default.GetRequiredService<IFoldersSettingsService>().ShowFileExtensions)
				{
					return fileNameWithoutExtension;
				}
			}
			return itemNameRaw;
		}
	}

	public string FileExtension { get; set; } = string.Empty;

	public string FileSizeDisplay
	{
		get
		{
			return fileSizeDisplay;
		}
		set
		{
			SetProperty(ref fileSizeDisplay, value, "FileSizeDisplay");
		}
	}

	public long FileSizeBytes
	{
		get
		{
			return fileSizeBytes;
		}
		set
		{
			if (SetProperty(ref fileSizeBytes, value, "FileSizeBytes"))
			{
				FileSizeDisplay = StringsHelper.FormatFileSize(value);
			}
		}
	}

	public string ItemDateModified { get; private set; } = string.Empty;

	public string ItemDateCreated { get; private set; } = string.Empty;

	public string ItemDateAccessed { get; private set; } = string.Empty;

	public DateTimeOffset ItemDateModifiedReal
	{
		get
		{
			return itemDateModifiedReal;
		}
		set
		{
			ItemDateModified = value.ToLocalTime().ToString("yyyy-MM-dd HH:mm");
			itemDateModifiedReal = value;
			OnPropertyChanged("ItemDateModified");
		}
	}

	public DateTimeOffset ItemDateCreatedReal
	{
		get
		{
			return itemDateCreatedReal;
		}
		set
		{
			ItemDateCreated = value.ToString();
			itemDateCreatedReal = value;
			OnPropertyChanged("ItemDateCreated");
		}
	}

	public DateTimeOffset ItemDateAccessedReal
	{
		get
		{
			return itemDateAccessedReal;
		}
		set
		{
			ItemDateAccessed = value.ToString();
			itemDateAccessedReal = value;
			OnPropertyChanged("ItemDateAccessed");
		}
	}

	public bool IsFolder
	{
		get
		{
			if (Directory.Exists(ItemPath))
			{
				return !ItemPath.EndsWith(".app", StringComparison.OrdinalIgnoreCase);
			}
			return false;
		}
	}

	public bool IsDriveRoot => ItemPath == Path.GetPathRoot(ItemPath);

	[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
	[ExcludeFromCodeCoverage]
	public Bitmap? Thumbnail
	{
		get
		{
			return thumbnail;
		}
		set
		{
			if (!EqualityComparer<Bitmap>.Default.Equals(thumbnail, value))
			{
				OnPropertyChanging(__KnownINotifyPropertyChangingArgs.Thumbnail);
				thumbnail = value;
				OnPropertyChanged(__KnownINotifyPropertyChangedArgs.Thumbnail);
			}
		}
	}

	[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
	[ExcludeFromCodeCoverage]
	public bool IsHiddenItem
	{
		get
		{
			return isHiddenItem;
		}
		set
		{
			if (!EqualityComparer<bool>.Default.Equals(isHiddenItem, value))
			{
				OnPropertyChanging(__KnownINotifyPropertyChangingArgs.IsHiddenItem);
				isHiddenItem = value;
				OnIsHiddenItemChanged(value);
				OnPropertyChanged(__KnownINotifyPropertyChangedArgs.IsHiddenItem);
			}
		}
	}

	[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
	[ExcludeFromCodeCoverage]
	public string ItemPath
	{
		get
		{
			return itemPath;
		}
		[MemberNotNull("itemPath")]
		set
		{
			if (!EqualityComparer<string>.Default.Equals(itemPath, value))
			{
				OnPropertyChanging(__KnownINotifyPropertyChangingArgs.ItemPath);
				itemPath = value;
				OnItemPathChanged(value);
				OnPropertyChanged(__KnownINotifyPropertyChangedArgs.ItemPath);
			}
		}
	}

	public IStorageItemWithPath ToStorageItemWithPath()
	{
		return StorageHelpers.FromPathAndType(ItemPath) ?? StorageHelpers.FromPathAndType(ItemPath, (!IsFolder) ? FilesystemItemType.File : FilesystemItemType.Directory);
	}

	public void UpdateContainsFilesFolders()
	{
		if (IsFolder)
		{
			ContainsFilesOrFolders = Directory.GetFiles(ItemPath).Length != 0 || Directory.GetDirectories(ItemPath).Length != 0;
		}
	}

	[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
	private void OnIsHiddenItemChanged(bool value)
	{
		Opacity = (value ? 0.45 : 1.0);
	}

	[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
	private void OnItemPathChanged(string value)
	{
		string extension = Path.GetExtension(value);
		FileExtension = extension;
		DisplayItemType = FileKindHelper.GetDisplayKind(value);
		if (Directory.Exists(value))
		{
			Bitmap placeholder = FileThumbnailHelper.GetDefaultFolderIcon();
			if (placeholder != null)
			{
				Thumbnail = placeholder;
			}
			Task.Run(async () =>
			{
				Bitmap bmp;
				if (value.EndsWith(".app", StringComparison.OrdinalIgnoreCase))
				{
					bmp = await FileThumbnailHelper.GetAppIconBitmapAsync(value);
				}
				else
				{
					bmp = await FileThumbnailHelper.GetFolderIconBitmapAsync(value);
				}
				if (ItemPath == value && bmp != null && bmp != placeholder)
				{
					Dispatcher.UIThread.Post(() =>
					{
						Thumbnail = bmp;
					});
				}
			});
			return;
		}
		string ext = Path.GetExtension(value);
		Bitmap defaultFileIcon = FileThumbnailHelper.GetDefaultFileIcon(ext);
		if (defaultFileIcon != null)
		{
			Thumbnail = defaultFileIcon;
		}
		Task.Run(async () =>
		{
			Bitmap bmp;
			if (FileExtensionHelpers.IsImageFile(ext) && Ioc.Default.GetRequiredService<IFoldersSettingsService>().ShowThumbnails)
			{
				bmp = await FileThumbnailHelper.GetImageThumbnailAsync(value);
				if (bmp == null)
				{
					bmp = await FileThumbnailHelper.GetThumbnailByExtensionAsync(ext);
				}
			}
			else
			{
				bmp = await FileThumbnailHelper.GetThumbnailByExtensionAsync(ext);
			}
			if (ItemPath == value && bmp != null)
			{
				Dispatcher.UIThread.Post(() =>
				{
					Thumbnail = bmp;
				});
			}
		});
	}
}
