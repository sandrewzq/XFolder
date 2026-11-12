using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel.__Internals;

namespace Files.Data.Items;

public sealed class RemovableDriveTreeItem : ObservableObject, IDisposable
{
	[ObservableProperty]
	private ObservableCollection<RemovableDriveTreeItem> _children = new ObservableCollection<RemovableDriveTreeItem>();

	[ObservableProperty]
	private string _name = string.Empty;

	[ObservableProperty]
	private string _path = string.Empty;

	[ObservableProperty]
	private bool _isExpanded;

	[ObservableProperty]
	private bool _isSelected;

	public DriveItem? Drive { get; }

	public bool HasDrive => Drive != null;

	[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
	[ExcludeFromCodeCoverage]
	public ObservableCollection<RemovableDriveTreeItem> Children
	{
		get
		{
			return _children;
		}
		[MemberNotNull("_children")]
		set
		{
			if (!EqualityComparer<ObservableCollection<RemovableDriveTreeItem>>.Default.Equals(_children, value))
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

	public RemovableDriveTreeItem(string name, bool isExpanded = false)
	{
		Name = name;
		IsExpanded = isExpanded;
	}

	public RemovableDriveTreeItem(DriveItem drive)
	{
		Drive = drive;
		UpdateDriveMetadata();
		drive.PropertyChanged += Drive_PropertyChanged;
	}

	public void Dispose()
	{
		if (Drive != null)
		{
			Drive.PropertyChanged -= Drive_PropertyChanged;
		}
		foreach (RemovableDriveTreeItem child in Children)
		{
			child.Dispose();
		}
	}

	private void Drive_PropertyChanged(object? sender, PropertyChangedEventArgs e)
	{
		if (Drive != null)
		{
			bool flag;
			switch (e.PropertyName)
			{
			case "Text":
			case "Name":
			case "Path":
				flag = true;
				break;
			default:
				flag = false;
				break;
			}
			if (flag)
			{
				UpdateDriveMetadata();
			}
		}
	}

	private void UpdateDriveMetadata()
	{
		if (Drive != null)
		{
			Path = Drive.Path;
			Name = System.IO.Path.GetFileName(Path);
		}
	}
}
