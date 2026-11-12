using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel.__Internals;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Files.Data.Messages;
using Files.Extensions;
using Files.Helpers.Shared;

namespace Files.Data.Items;

public class FileTreeItem : ObservableObject, IDisposable
{
	[ObservableProperty]
	private ObservableCollection<FileTreeItem> _children;

	[ObservableProperty]
	private string _name;

	[ObservableProperty]
	private string _path;

	[ObservableProperty]
	private long _size;

	[ObservableProperty]
	private DateTimeOffset _modified;

	[ObservableProperty]
	private bool _isSelected;

	[ObservableProperty]
	private bool _isExpanded;

	[ObservableProperty]
	private bool _isDirectory;

	[ObservableProperty]
	private bool _isHiddenItem;

	[ObservableProperty]
	private double _opacity = 1.0;

	private FileSystemWatcher? watcher;

	[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.RelayCommandGenerator", "8.4.0.0")]
	private RelayCommand? backCommand;

	[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
	[ExcludeFromCodeCoverage]
	public ObservableCollection<FileTreeItem> Children
	{
		get
		{
			return _children;
		}
		[MemberNotNull("_children")]
		set
		{
			if (!EqualityComparer<ObservableCollection<FileTreeItem>>.Default.Equals(_children, value))
			{
				OnPropertyChanging(__KnownINotifyPropertyChangingArgs.Children);
				_children = value;
				OnPropertyChanged(__KnownINotifyPropertyChangedArgs.Children);
			}
		}
	}

	[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
	[ExcludeFromCodeCoverage]
	public string Name
	{
		get
		{
			return _name;
		}
		[MemberNotNull("_name")]
		set
		{
			if (!EqualityComparer<string>.Default.Equals(_name, value))
			{
				OnPropertyChanging(__KnownINotifyPropertyChangingArgs.Name);
				_name = value;
				OnPropertyChanged(__KnownINotifyPropertyChangedArgs.Name);
			}
		}
	}

	[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
	[ExcludeFromCodeCoverage]
	public string Path
	{
		get
		{
			return _path;
		}
		[MemberNotNull("_path")]
		set
		{
			if (!EqualityComparer<string>.Default.Equals(_path, value))
			{
				OnPropertyChanging(__KnownINotifyPropertyChangingArgs.Path);
				_path = value;
				OnPropertyChanged(__KnownINotifyPropertyChangedArgs.Path);
			}
		}
	}

	[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
	[ExcludeFromCodeCoverage]
	public long Size
	{
		get
		{
			return _size;
		}
		set
		{
			if (!EqualityComparer<long>.Default.Equals(_size, value))
			{
				OnPropertyChanging(__KnownINotifyPropertyChangingArgs.Size);
				_size = value;
				OnPropertyChanged(__KnownINotifyPropertyChangedArgs.Size);
			}
		}
	}

	[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
	[ExcludeFromCodeCoverage]
	public DateTimeOffset Modified
	{
		get
		{
			return _modified;
		}
		set
		{
			if (!EqualityComparer<DateTimeOffset>.Default.Equals(_modified, value))
			{
				OnPropertyChanging(__KnownINotifyPropertyChangingArgs.Modified);
				_modified = value;
				OnPropertyChanged(__KnownINotifyPropertyChangedArgs.Modified);
			}
		}
	}

	[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
	[ExcludeFromCodeCoverage]
	public bool IsSelected
	{
		get
		{
			return _isSelected;
		}
		set
		{
			if (!EqualityComparer<bool>.Default.Equals(_isSelected, value))
			{
				OnPropertyChanging(__KnownINotifyPropertyChangingArgs.IsSelected);
				_isSelected = value;
				OnPropertyChanged(__KnownINotifyPropertyChangedArgs.IsSelected);
			}
		}
	}

	[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
	[ExcludeFromCodeCoverage]
	public bool IsExpanded
	{
		get
		{
			return _isExpanded;
		}
		set
		{
			if (!EqualityComparer<bool>.Default.Equals(_isExpanded, value))
			{
				OnPropertyChanging(__KnownINotifyPropertyChangingArgs.IsExpanded);
				_isExpanded = value;
				OnPropertyChanged(__KnownINotifyPropertyChangedArgs.IsExpanded);
				// Only watch folders that are actually expanded in the tree. Watching
				// every directory node would create one FileSystemWatcher per node.
				if (value)
				{
					if (watcher == null && IsDirectory)
					{
						WatchForFolderChanges(Path);
					}
				}
				else
				{
					CloseWatcher();
				}
			}
		}
	}

	[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
	[ExcludeFromCodeCoverage]
	public bool IsDirectory
	{
		get
		{
			return _isDirectory;
		}
		set
		{
			if (!EqualityComparer<bool>.Default.Equals(_isDirectory, value))
			{
				OnPropertyChanging(__KnownINotifyPropertyChangingArgs.IsDirectory);
				_isDirectory = value;
				OnPropertyChanged(__KnownINotifyPropertyChangedArgs.IsDirectory);
			}
		}
	}

	[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
	[ExcludeFromCodeCoverage]
	public bool IsHiddenItem
	{
		get
		{
			return _isHiddenItem;
		}
		set
		{
			if (!EqualityComparer<bool>.Default.Equals(_isHiddenItem, value))
			{
				OnPropertyChanging(__KnownINotifyPropertyChangingArgs.IsHiddenItem);
				_isHiddenItem = value;
				OnPropertyChanged(__KnownINotifyPropertyChangedArgs.IsHiddenItem);
			}
		}
	}

	[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
	[ExcludeFromCodeCoverage]
	public double Opacity
	{
		get
		{
			return _opacity;
		}
		set
		{
			if (!EqualityComparer<double>.Default.Equals(_opacity, value))
			{
				OnPropertyChanging(__KnownINotifyPropertyChangingArgs.Opacity);
				_opacity = value;
				OnPropertyChanged(__KnownINotifyPropertyChangedArgs.Opacity);
			}
		}
	}

	[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.RelayCommandGenerator", "8.4.0.0")]
	[ExcludeFromCodeCoverage]
	public IRelayCommand BackCommand => backCommand ?? (backCommand = new RelayCommand(Back));

	public FileTreeItem(string path, bool isRoot = false, bool deferFilesystemAccess = false)
	{
		_path = path;
		_name = (isRoot ? path : System.IO.Path.GetFileName(Path));
		_isExpanded = isRoot;
		if (string.IsNullOrEmpty(path))
		{
			IsDirectory = true;
			Size = 0L;
			return;
		}
		if (deferFilesystemAccess)
		{
			IsDirectory = true;
			Size = 0L;
			Modified = DateTimeOffset.Now;
			return;
		}
		try
		{
			IsDirectory = Directory.Exists(Path);
			if (IsDirectory)
			{
				DirectoryInfo directoryInfo = new DirectoryInfo(path);
				Size = 0L;
				Modified = directoryInfo.LastWriteTimeUtc;
				IsHiddenItem = PathHelpers.IsHiddenDirectory(directoryInfo);
				Opacity = (IsHiddenItem ? 0.45 : 1.0);
				if (isRoot)
				{
					WatchForFolderChanges(path);
				}
			}
			else if (File.Exists(path))
			{
				FileInfo fileInfo = new FileInfo(path);
				Size = fileInfo.Length;
				Modified = fileInfo.LastWriteTimeUtc;
				IsHiddenItem = PathHelpers.IsHiddenFile(fileInfo);
				Opacity = (IsHiddenItem ? 0.45 : 1.0);
			}
			else
			{
				Size = 0L;
				Modified = DateTimeOffset.Now;
			}
		}
		catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
		{
			// Watcher notifications can arrive after an editor removes its temporary path.
			Size = 0L;
			Modified = DateTimeOffset.Now;
		}
	}

	protected void WatchForFolderChanges(string? folderPath)
	{
		if (watcher == null && !string.IsNullOrWhiteSpace(folderPath))
		{
			SafetyExtensions.IgnoreExceptions(() =>
			{
				watcher = new FileSystemWatcher
				{
					Path = folderPath,
					Filter = "*",
					NotifyFilter = (NotifyFilters.DirectoryName | NotifyFilters.LastWrite)
				};
				watcher.Created += DirectoryWatcher_OnCreated;
				watcher.Deleted += DirectoryWatcher_OnDeleted;
				watcher.Renamed += DirectoryWatcher_OnRenamed;
				watcher.EnableRaisingEvents = true;
			});
		}
	}

	public void Dispose()
	{
		CloseWatcher();
		if (Children == null)
		{
			return;
		}
		foreach (FileTreeItem child in Children)
		{
			child.Dispose();
		}
		Children.Clear();
	}

	private void DirectoryWatcher_OnCreated(object sender, FileSystemEventArgs e)
	{
		Dispatcher.UIThread.Post(() =>
		{
			Modified = DateTimeOffset.Now;
			if (Children == null || !Directory.Exists(e.FullPath))
			{
				return;
			}
			bool flag = true;
			int index = -1;
			for (int i = 0; i < Children.Count; i++)
			{
				if (Children[i].Path == e.FullPath)
				{
					flag = false;
					index = i;
					break;
				}
			}
			try
			{
				FileTreeItem fileTreeItem = new FileTreeItem(e.FullPath);
				if (!Directory.Exists(e.FullPath))
				{
					fileTreeItem.Dispose();
					return;
				}
				if (flag)
				{
					Children.Add(fileTreeItem);
				}
				else
				{
					FileTreeItem replaced = Children[index];
					Children[index] = fileTreeItem;
					replaced.Dispose();
				}
			}
			catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
			{
				// A transient watcher entry is not a usable tree node.
			}
		});
	}

	private void DirectoryWatcher_OnDeleted(object sender, FileSystemEventArgs e)
	{
		Dispatcher.UIThread.Post(() =>
		{
			Modified = DateTimeOffset.Now;
			if (Children != null)
			{
				for (int i = 0; i < Children.Count; i++)
				{
					if (Children[i].Path == e.FullPath)
					{
						FileTreeItem removed = Children[i];
						Children.RemoveAt(i);
						removed.Dispose();
						break;
					}
				}
				WeakReferenceMessenger.Default.Send(new FileSystemChangedMessage
				{
					Path = e.FullPath,
					ChangeType = FileSystemChangeType.Deleted
				});
			}
		});
	}

	private void DirectoryWatcher_OnRenamed(object sender, RenamedEventArgs e)
	{
		string? watchedPath = sender is FileSystemWatcher fileSystemWatcher ? fileSystemWatcher.Path : watcher?.Path;
		Dispatcher.UIThread.Post(() =>
		{
			Modified = DateTimeOffset.Now;
			if (Children != null)
			{
				string? directoryName = System.IO.Path.GetDirectoryName(e.FullPath);
				string text = watchedPath?.TrimEnd(System.IO.Path.DirectorySeparatorChar) ?? string.Empty;
				bool flag = directoryName?.TrimEnd(System.IO.Path.DirectorySeparatorChar) == text;
				for (int i = 0; i < Children.Count; i++)
				{
					FileTreeItem fileTreeItem = Children[i];
					if (fileTreeItem.Path == e.OldFullPath)
					{
						if (flag)
						{
							fileTreeItem.Path = e.FullPath;
							fileTreeItem.Name = System.IO.Path.GetFileName(e.FullPath);
							try
							{
								fileTreeItem.Modified = new DirectoryInfo(e.FullPath).LastWriteTimeUtc;
							}
							catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
							{
								fileTreeItem.Modified = DateTimeOffset.Now;
							}
							if (fileTreeItem.Children != null)
							{
								foreach (FileTreeItem child in fileTreeItem.Children)
								{
									child.Dispose();
								}
								fileTreeItem.Children.Clear();
								fileTreeItem.Children = null;
							}
							fileTreeItem.IsExpanded = false;
							fileTreeItem.IsSelected = false;
						}
						else
						{
							FileTreeItem removed = Children[i];
							Children.RemoveAt(i);
							removed.Dispose();
						}
						break;
					}
				}
				WeakReferenceMessenger.Default.Send(new FileSystemChangedMessage
				{
					Path = e.FullPath,
					OldPath = e.OldFullPath,
					ChangeType = FileSystemChangeType.Renamed
				});
			}
		});
	}

	public void CloseWatcher()
	{
		watcher?.Dispose();
		watcher = null;
	}

	[RelayCommand]
	private void Back()
	{
	}
}
