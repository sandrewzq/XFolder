using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using Avalonia.Data.Converters;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel.__Internals;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Files.Assets.Lang;
using Files.Data.Contracts;
using Files.Data.Items;
using Files.Data.Messages;
using Files.Data.Models;
using Files.Extensions;
using Files.Helpers.Application;
using Files.Helpers.Shared;
using Files.Services.NativePlugin;
using Files.ViewModels;

namespace Files.UserControls.Sidebar;

public class SidebarViewModel : ViewModelBase, IDisposable
{
	private class QuickAccessIconConverter : IMultiValueConverter
	{
		private readonly Bitmap _file;

		private readonly Bitmap _folder;

		private readonly Dictionary<string, Bitmap> _quickAccessIconCache = new Dictionary<string, Bitmap>();

		public QuickAccessIconConverter(Bitmap file, Bitmap folder)
		{
			_file = file;
			_folder = folder;
		}

		public object? Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
		{
			if (values.Count == 2 && values[0] is bool flag && values[1] is string text)
			{
				if (!flag)
				{
					return _file;
				}
				if (OperatingSystem.IsMacOS())
				{
					string key = text ?? "";
					if (!_quickAccessIconCache.TryGetValue(key, out Bitmap value))
					{
						value = MacPluginInterop.GetSidebarIconBitmapByPath(text) ?? _folder;
						_quickAccessIconCache[key] = value;
					}
					return value;
				}
				return _folder;
			}
			return null;
		}
	}

	private readonly IFoldersSettingsService _foldersSettingsService;

	private readonly ObservableCollection<DriveItem> _removableDrives;

	private readonly RemovableDriveTreeItem _removableDrivesRoot;

	[ObservableProperty]
	private ObservableCollection<FileTreeItem> _filesystemNodes = new ObservableCollection<FileTreeItem>();

	[ObservableProperty]
	private ObservableCollection<FileTreeItem> _quickAccessNodes = new ObservableCollection<FileTreeItem>();

	[ObservableProperty]
	private FileTreeItem _currentDirectoryNode;

	[ObservableProperty]
	private string _selectedPath;

	private static QuickAccessIconConverter? s_quickAccessIconConverter;

	[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.RelayCommandGenerator", "8.4.0.0")]
	private RelayCommand<FileTreeItem>? removeFromSidebarCommand;

	public ObservableCollection<RemovableDriveTreeItem> RemovableDriveNodes { get; } = new ObservableCollection<RemovableDriveTreeItem>();

	public bool HasRemovableDrives => _removableDrivesRoot.Children.Count > 0;

	public static IMultiValueConverter QuickAccessIconConverterInstance
	{
		get
		{
			if (s_quickAccessIconConverter == null)
			{
				using Stream stream = AssetLoader.Open(new Uri("avares://XFolder/Assets/image/file.png"));
				using Stream stream2 = AssetLoader.Open(new Uri("avares://XFolder/Assets/image/folder.png"));
				Bitmap file = new Bitmap(stream);
				Bitmap folder = new Bitmap(stream2);
				s_quickAccessIconConverter = new QuickAccessIconConverter(file, folder);
			}
			return s_quickAccessIconConverter;
		}
	}

	[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
	[ExcludeFromCodeCoverage]
	public ObservableCollection<FileTreeItem> FilesystemNodes
	{
		get
		{
			return _filesystemNodes;
		}
		[MemberNotNull("_filesystemNodes")]
		set
		{
			if (!EqualityComparer<ObservableCollection<FileTreeItem>>.Default.Equals(_filesystemNodes, value))
			{
				OnPropertyChanging(__KnownINotifyPropertyChangingArgs.FilesystemNodes);
				_filesystemNodes = value;
				OnPropertyChanged(__KnownINotifyPropertyChangedArgs.FilesystemNodes);
			}
		}
	}

	[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
	[ExcludeFromCodeCoverage]
	public ObservableCollection<FileTreeItem> QuickAccessNodes
	{
		get
		{
			return _quickAccessNodes;
		}
		[MemberNotNull("_quickAccessNodes")]
		set
		{
			if (!EqualityComparer<ObservableCollection<FileTreeItem>>.Default.Equals(_quickAccessNodes, value))
			{
				OnPropertyChanging(__KnownINotifyPropertyChangingArgs.QuickAccessNodes);
				_quickAccessNodes = value;
				OnPropertyChanged(__KnownINotifyPropertyChangedArgs.QuickAccessNodes);
			}
		}
	}

	[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
	[ExcludeFromCodeCoverage]
	public FileTreeItem CurrentDirectoryNode
	{
		get
		{
			return _currentDirectoryNode;
		}
		[MemberNotNull("_currentDirectoryNode")]
		set
		{
			if (!EqualityComparer<FileTreeItem>.Default.Equals(_currentDirectoryNode, value))
			{
				OnPropertyChanging(__KnownINotifyPropertyChangingArgs.CurrentDirectoryNode);
				_currentDirectoryNode = value;
				OnPropertyChanged(__KnownINotifyPropertyChangedArgs.CurrentDirectoryNode);
			}
		}
	}

	[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
	[ExcludeFromCodeCoverage]
	public string SelectedPath
	{
		get
		{
			return _selectedPath;
		}
		[MemberNotNull("_selectedPath")]
		set
		{
			if (!EqualityComparer<string>.Default.Equals(_selectedPath, value))
			{
				OnPropertyChanging(__KnownINotifyPropertyChangingArgs.SelectedPath);
				_selectedPath = value;
				OnPropertyChanged(__KnownINotifyPropertyChangedArgs.SelectedPath);
			}
		}
	}

	[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.RelayCommandGenerator", "8.4.0.0")]
	[ExcludeFromCodeCoverage]
	public IRelayCommand<FileTreeItem> RemoveFromSidebarCommand => removeFromSidebarCommand ?? (removeFromSidebarCommand = new RelayCommand<FileTreeItem>(RemoveFromSidebar));

	public SidebarViewModel()
	{
		_foldersSettingsService = Ioc.Default.GetRequiredService<IFoldersSettingsService>();
		_removableDrives = Ioc.Default.GetRequiredService<DrivesViewModel>().Drives;
		_removableDrivesRoot = new RemovableDriveTreeItem(Resources.SidebarRemovableDrives, isExpanded: true);
		RemovableDriveNodes.Add(_removableDrivesRoot);
		_foldersSettingsService.PropertyChanged += FoldersSettingsService_PropertyChanged;
		_removableDrives.CollectionChanged += RemovableDrives_CollectionChanged;
		AppLanguageHelper.LanguageChanged += AppLanguageHelper_LanguageChanged;
		InitQuickAccessNodes();
		InitFilesystemNodes();
		RefreshRemovableDriveNodes();
		WeakReferenceMessenger.Default.Register(this, (object r, QuickAccessChangedMessage m) =>
		{
			Dispatcher.UIThread.Post(() =>
			{
				QuickAccessNodes.Clear();
				InitQuickAccessNodes();
			});
		});
	}

	public void Dispose()
	{
		_foldersSettingsService.PropertyChanged -= FoldersSettingsService_PropertyChanged;
		_removableDrives.CollectionChanged -= RemovableDrives_CollectionChanged;
		AppLanguageHelper.LanguageChanged -= AppLanguageHelper_LanguageChanged;
		_removableDrivesRoot.Dispose();
	}

	private void AppLanguageHelper_LanguageChanged(object? sender, EventArgs e)
	{
		Dispatcher.UIThread.Post(RefreshLocalizedSidebarText);
	}

	private void RemovableDrives_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
	{
		RefreshRemovableDriveNodes();
	}

	private void RefreshRemovableDriveNodes()
	{
		_removableDrivesRoot.Name = Resources.SidebarRemovableDrives;
		foreach (RemovableDriveTreeItem child in _removableDrivesRoot.Children)
		{
			child.Dispose();
		}
		_removableDrivesRoot.Children.Clear();
		foreach (DriveItem item in _removableDrives.OrderBy((DriveItem item) => item.Text, StringComparer.CurrentCultureIgnoreCase))
		{
			_removableDrivesRoot.Children.Add(new RemovableDriveTreeItem(item));
		}
		OnPropertyChanged("HasRemovableDrives");
	}

	private void RefreshLocalizedSidebarText()
	{
		_removableDrivesRoot.Name = Resources.SidebarRemovableDrives;
		FileTreeItem fileTreeItem = QuickAccessNodes.FirstOrDefault();
		if (fileTreeItem == null)
		{
			return;
		}
		fileTreeItem.Name = Resources.QuickAccess;
		if (fileTreeItem.Children == null)
		{
			return;
		}
		foreach (FileTreeItem child in fileTreeItem.Children)
		{
			child.Name = GetNameForPath(child.Path);
		}
	}

	private void FoldersSettingsService_PropertyChanged(object? sender, PropertyChangedEventArgs e)
	{
		if (!(e.PropertyName == "ShowHiddenItems"))
		{
			return;
		}
		Dispatcher.UIThread.Post(() =>
		{
			string selectedPath = SelectedPath;
			DisposeTreeItems(FilesystemNodes);
			FilesystemNodes.Clear();
			InitFilesystemNodes();
			if (!string.IsNullOrEmpty(selectedPath))
			{
				FileTreeItem fileTreeItem = OpenDirectory(selectedPath);
				if (fileTreeItem != null)
				{
					fileTreeItem.IsSelected = true;
					EnsureTreeItemVisible(fileTreeItem);
				}
			}
		});
	}

	private void DisposeTreeItems(ObservableCollection<FileTreeItem>? items)
	{
		if (items == null)
		{
			return;
		}
		foreach (FileTreeItem item in items)
		{
			DisposeTreeItems(item.Children);
			item.Dispose();
		}
	}

	private void InitFilesystemNodes()
	{
		FileTreeItem item = new FileTreeItem("/", isRoot: true);
		FilesystemNodes.Add(item);
	}

	private void InitQuickAccessNodes()
	{
		IEnumerable<string> enumerable = Ioc.Default.GetRequiredService<IQuickAccessService>().LoadQuickAccessPaths();
		FileTreeItem fileTreeItem = new FileTreeItem(string.Empty)
		{
			Name = Resources.QuickAccess,
			IsDirectory = true,
			IsExpanded = true,
			Children = new ObservableCollection<FileTreeItem>()
		};
		if (enumerable != null && enumerable.Any())
		{
			foreach (string item in enumerable)
			{
				if (!string.IsNullOrWhiteSpace(item))
				{
					fileTreeItem.Children.Add(new FileTreeItem(item, deferFilesystemAccess: true)
					{
						Name = GetNameForPath(item)
					});
				}
			}
		}
		else
		{
			string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
			string path = "/Applications";
			fileTreeItem.Children.Add(new FileTreeItem(path, deferFilesystemAccess: true)
			{
				Name = Resources.Applications
			});
			string path2 = Path.Combine(folderPath, "Desktop");
			fileTreeItem.Children.Add(new FileTreeItem(path2, deferFilesystemAccess: true)
			{
				Name = Resources.Desktop
			});
			string path3 = Path.Combine(folderPath, "Documents");
			fileTreeItem.Children.Add(new FileTreeItem(path3, deferFilesystemAccess: true)
			{
				Name = Resources.Documents
			});
			string path4 = Path.Combine(folderPath, "Downloads");
			fileTreeItem.Children.Add(new FileTreeItem(path4, deferFilesystemAccess: true)
			{
				Name = Resources.Downloads
			});
			string path5 = Path.Combine(folderPath, "Music");
			fileTreeItem.Children.Add(new FileTreeItem(path5, deferFilesystemAccess: true)
			{
				Name = Resources.Music
			});
			string path6 = Path.Combine(folderPath, "Movies");
			fileTreeItem.Children.Add(new FileTreeItem(path6, deferFilesystemAccess: true)
			{
				Name = Resources.Movies
			});
			string path7 = Path.Combine(folderPath, "Pictures");
			fileTreeItem.Children.Add(new FileTreeItem(path7, deferFilesystemAccess: true)
			{
				Name = Resources.Pictures
			});
			if (!string.IsNullOrWhiteSpace(folderPath))
			{
				fileTreeItem.Children.Add(new FileTreeItem(folderPath, deferFilesystemAccess: true)
				{
					Name = new DirectoryInfo(folderPath).Name
				});
			}
			SaveQuickAccessNodes(fileTreeItem);
		}
		QuickAccessNodes.Add(fileTreeItem);
	}

	private string GetNameForPath(string path)
	{
		string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
		if (path == Path.Combine(folderPath, "Desktop"))
		{
			return Resources.Desktop;
		}
		if (path == Path.Combine(folderPath, "Documents"))
		{
			return Resources.Documents;
		}
		if (path == Path.Combine(folderPath, "Downloads"))
		{
			return Resources.Downloads;
		}
		if (path == Path.Combine(folderPath, "Music"))
		{
			return Resources.Music;
		}
		if (path == Path.Combine(folderPath, "Movies"))
		{
			return Resources.Movies;
		}
		if (path == Path.Combine(folderPath, "Pictures"))
		{
			return Resources.Pictures;
		}
		if (path == folderPath)
		{
			return new DirectoryInfo(folderPath).Name;
		}
		if (path == "/Applications")
		{
			return Resources.Applications;
		}
		return new DirectoryInfo(path).Name;
	}

	private void SaveQuickAccessNodes(FileTreeItem root)
	{
		if (root?.Children != null)
		{
			List<string> paths = root.Children.Select((FileTreeItem x) => x.Path).ToList();
			Ioc.Default.GetRequiredService<IQuickAccessService>().SaveQuickAccessPaths(paths);
		}
	}

	private ObservableCollection<FileTreeItem> LoadFileItemChildren(string path, bool isIncludeFiles)
	{
		EnumerationOptions enumerationOptions = new EnumerationOptions
		{
			IgnoreInaccessible = true,
			AttributesToSkip = (_foldersSettingsService.ShowHiddenItems ? FileAttributes.System : (FileAttributes.Hidden | FileAttributes.System))
		};
		ObservableCollection<FileTreeItem> observableCollection = new ObservableCollection<FileTreeItem>();
		foreach (string item in Directory.EnumerateDirectories(path, "*", enumerationOptions))
		{
			observableCollection.Add(new FileTreeItem(item));
		}
		if (isIncludeFiles)
		{
			foreach (string item2 in Directory.EnumerateFiles(path, "*", enumerationOptions))
			{
				observableCollection.Add(new FileTreeItem(item2));
			}
		}
		return observableCollection;
	}

	public FileTreeItem GetDirectoryNode(ObservableCollection<FileTreeItem> nodes, string path)
	{
		if (nodes == null)
		{
			return null;
		}
		foreach (FileTreeItem node in nodes)
		{
			if (node.Path == path)
			{
				return node;
			}
			if (path.StartsWith(node.Path.WithEnding("/")))
			{
				return GetDirectoryNode(node.Children, path);
			}
		}
		return null;
	}

	public void GetAncestorNodeList(ObservableCollection<FileTreeItem> nodes, string path, ref List<FileTreeItem> ancestorNodes)
	{
		if (nodes == null)
		{
			return;
		}
		foreach (FileTreeItem node in nodes)
		{
			if (path.StartsWith(node.Path.WithEnding("/")))
			{
				ancestorNodes.Add(node);
				GetAncestorNodeList(node.Children, path, ref ancestorNodes);
			}
		}
	}

	public IEnumerable<FileTreeItem> GetAllVisibleFileItems(IEnumerable<FileTreeItem>? items)
	{
		if (items == null)
		{
			yield break;
		}
		foreach (FileTreeItem item in items)
		{
			yield return item;
			if (!item.IsDirectory || !item.IsExpanded || item.Children == null)
			{
				continue;
			}
			foreach (FileTreeItem allVisibleFileItem in GetAllVisibleFileItems(item.Children))
			{
				yield return allVisibleFileItem;
			}
		}
	}

	public void EnsureTreeItemVisible(FileTreeItem node)
	{
		if (node == null)
		{
			return;
		}
		List<FileTreeItem> ancestorNodes = new List<FileTreeItem>();
		GetAncestorNodeList(FilesystemNodes, node.Path, ref ancestorNodes);
		foreach (FileTreeItem item in ancestorNodes)
		{
			item.IsExpanded = true;
		}
		node.IsExpanded = true;
	}

	public FileTreeItem OpenDirectory(string dirPath)
	{
		if (Directory.Exists(dirPath))
		{
			string[] pathSegments = PathHelpers.GetPathSegments(dirPath);
			int num = pathSegments.Length - 1;
			while (num >= 0 && GetDirectoryNode(FilesystemNodes, pathSegments[num]) == null)
			{
				num--;
			}
			for (int i = num; i < pathSegments.Length; i++)
			{
				FileTreeItem directoryNode = GetDirectoryNode(FilesystemNodes, pathSegments[i]);
				if (directoryNode == null)
				{
					break;
				}
				if (directoryNode.Children == null)
				{
					directoryNode.Children = LoadFileItemChildren(pathSegments[i], isIncludeFiles: false);
				}
				SelectedPath = pathSegments[i];
			}
			return GetDirectoryNode(FilesystemNodes, SelectedPath);
		}
		return null;
	}

	[RelayCommand]
	public void RemoveFromSidebar(FileTreeItem item)
	{
		if (item != null)
		{
			FileTreeItem fileTreeItem = QuickAccessNodes.FirstOrDefault();
			if (fileTreeItem != null && fileTreeItem.Children != null && fileTreeItem.Children.Contains(item))
			{
				fileTreeItem.Children.Remove(item);
				item.Dispose();
				SaveQuickAccessNodes(fileTreeItem);
				WeakReferenceMessenger.Default.Send(new QuickAccessChangedMessage());
			}
		}
	}

	public void EjectRemovableDrive(RemovableDriveTreeItem? item)
	{
		if (item != null && item.HasDrive && !string.IsNullOrWhiteSpace(item.Path))
		{
			MacPluginInterop.EjectVolume(item.Path);
		}
	}
}
