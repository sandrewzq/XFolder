using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Avalonia.Styling;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel.__Internals;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Files.Data.Messages;
using Files.Helpers;
using Files.Helpers.Shared;
using Files.UserControls.NavigationToolbar;
using Files.ViewModels.Items;

namespace Files.ViewModels.Shells;

public class ModernShellViewModel : BaseShellViewModel, IDisposable, IRecipient<FileSystemChangedMessage>
{
	[ObservableProperty]
	private bool _isCurrentInstance;

	[ObservableProperty]
	private ThemeVariant? _currentThemeVariant;

	[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.RelayCommandGenerator", "8.4.0.0")]
	private RelayCommand? addNewTabCommand;

	[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
	[ExcludeFromCodeCoverage]
	public bool IsCurrentInstance
	{
		get
		{
			return _isCurrentInstance;
		}
		set
		{
			if (!EqualityComparer<bool>.Default.Equals(_isCurrentInstance, value))
			{
				OnPropertyChanging(__KnownINotifyPropertyChangingArgs.IsCurrentInstance);
				_isCurrentInstance = value;
				OnPropertyChanged(__KnownINotifyPropertyChangedArgs.IsCurrentInstance);
			}
		}
	}

	[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
	[ExcludeFromCodeCoverage]
	public ThemeVariant? CurrentThemeVariant
	{
		get
		{
			return _currentThemeVariant;
		}
		set
		{
			if (!EqualityComparer<ThemeVariant>.Default.Equals(_currentThemeVariant, value))
			{
				OnPropertyChanging(__KnownINotifyPropertyChangingArgs.CurrentThemeVariant);
				_currentThemeVariant = value;
				OnPropertyChanged(__KnownINotifyPropertyChangedArgs.CurrentThemeVariant);
			}
		}
	}

	[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.RelayCommandGenerator", "8.4.0.0")]
	[ExcludeFromCodeCoverage]
	public IRelayCommand AddNewTabCommand => addNewTabCommand ?? (addNewTabCommand = new RelayCommand(AddNewTab));

	public ModernShellViewModel()
	{
		NavigationToolbarViewModel = new NavigationToolbarViewModel();
		NavigationToolbarViewModel.ParentVM = this;
		AddNewTab();
		WeakReferenceMessenger.Default.Register(this);
	}

	public void Receive(FileSystemChangedMessage message)
	{
		switch (message.ChangeType)
		{
		case FileSystemChangeType.Deleted:
			HandleFileDeleted(message.Path);
			break;
		case FileSystemChangeType.Renamed:
			HandleFileRenamed(message.OldPath, message.Path);
			break;
		}
	}

	private void HandleFileDeleted(string path)
	{
		if (string.IsNullOrWhiteSpace(path))
		{
			return;
		}
		foreach (TabItemViewModel tabItem in TabItems)
		{
			if (!string.IsNullOrWhiteSpace(tabItem.NavPath) && PathHelpers.IsSubPathOf(tabItem.NavPath, path))
			{
				tabItem.NavPath = PathHelpers.GetNearestValidParentDirectory(path);
			}
		}
	}

	private void HandleFileRenamed(string oldPath, string newPath)
	{
		if (string.IsNullOrWhiteSpace(oldPath) || string.IsNullOrWhiteSpace(newPath))
		{
			return;
		}
		foreach (TabItemViewModel tabItem in TabItems)
		{
			if (!string.IsNullOrWhiteSpace(tabItem.NavPath) && PathHelpers.IsSubPathOf(tabItem.NavPath, oldPath))
			{
				tabItem.NavPath = tabItem.NavPath.Replace(oldPath, newPath);
			}
		}
	}

	public void Dispose()
	{
		WeakReferenceMessenger.Default.Unregister<FileSystemChangedMessage>(this);
	}

	[RelayCommand]
	private void AddNewTab()
	{
		TabItemViewModel tabItemViewModel = TabItemViewModel.CreateNewTab(NavigationHelpers.GetDefaultNewTabPath());
		tabItemViewModel.ParentShellViewModel = this;
		TabItems.Add(tabItemViewModel);
		SelectedTab = tabItemViewModel;
	}
}
