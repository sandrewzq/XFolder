using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel.__Internals;
using CommunityToolkit.Mvvm.Input;
using Files.Assets.Lang;
using Files.Data.Enums.Navigation;
using Files.Data.Items;
using Files.Helpers.Shared;
using Files.ViewModels.Shells;

namespace Files.UserControls.NavigationToolbar;

public class NavigationToolbarViewModel : ObservableObject
{
	public ModernShellViewModel? ParentVM;

	[ObservableProperty]
	private string _pathControlDisplayText = string.Empty;

	[ObservableProperty]
	private bool _isFocused;

	[ObservableProperty]
	private bool _canRefresh;

	[ObservableProperty]
	private bool _canCopyPathInPage;

	[ObservableProperty]
	private bool _canNavigateToParent;

	[ObservableProperty]
	private bool _canGoBack;

	[ObservableProperty]
	private bool _canGoForward;

	[ObservableProperty]
	private bool _hasItem;

	[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.RelayCommandGenerator", "8.4.0.0")]
	private RelayCommand? navigateToParentCommand;

	[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.RelayCommandGenerator", "8.4.0.0")]
	private RelayCommand? goBackCommand;

	[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.RelayCommandGenerator", "8.4.0.0")]
	private RelayCommand? goForwardCommand;

	public string Path2Go { get; set; } = string.Empty;

	public ObservableCollection<PathBoxItem> PathComponents { get; } = new ObservableCollection<PathBoxItem>();

	public bool CanGoBackOrForward
	{
		get
		{
			if (!CanGoBack)
			{
				return CanGoForward;
			}
			return true;
		}
	}

	[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
	[ExcludeFromCodeCoverage]
	public string PathControlDisplayText
	{
		get
		{
			return _pathControlDisplayText;
		}
		[MemberNotNull("_pathControlDisplayText")]
		set
		{
			if (!EqualityComparer<string>.Default.Equals(_pathControlDisplayText, value))
			{
				OnPropertyChanging(__KnownINotifyPropertyChangingArgs.PathControlDisplayText);
				_pathControlDisplayText = value;
				OnPathControlDisplayTextChanged(value);
				OnPropertyChanged(__KnownINotifyPropertyChangedArgs.PathControlDisplayText);
			}
		}
	}

	[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
	[ExcludeFromCodeCoverage]
	public bool IsFocused
	{
		get
		{
			return _isFocused;
		}
		set
		{
			if (!EqualityComparer<bool>.Default.Equals(_isFocused, value))
			{
				OnPropertyChanging(__KnownINotifyPropertyChangingArgs.IsFocused);
				_isFocused = value;
				OnPropertyChanged(__KnownINotifyPropertyChangedArgs.IsFocused);
			}
		}
	}

	[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
	[ExcludeFromCodeCoverage]
	public bool CanRefresh
	{
		get
		{
			return _canRefresh;
		}
		set
		{
			if (!EqualityComparer<bool>.Default.Equals(_canRefresh, value))
			{
				OnPropertyChanging(__KnownINotifyPropertyChangingArgs.CanRefresh);
				_canRefresh = value;
				OnPropertyChanged(__KnownINotifyPropertyChangedArgs.CanRefresh);
			}
		}
	}

	[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
	[ExcludeFromCodeCoverage]
	public bool CanCopyPathInPage
	{
		get
		{
			return _canCopyPathInPage;
		}
		set
		{
			if (!EqualityComparer<bool>.Default.Equals(_canCopyPathInPage, value))
			{
				OnPropertyChanging(__KnownINotifyPropertyChangingArgs.CanCopyPathInPage);
				_canCopyPathInPage = value;
				OnPropertyChanged(__KnownINotifyPropertyChangedArgs.CanCopyPathInPage);
			}
		}
	}

	[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
	[ExcludeFromCodeCoverage]
	public bool CanNavigateToParent
	{
		get
		{
			return _canNavigateToParent;
		}
		set
		{
			if (!EqualityComparer<bool>.Default.Equals(_canNavigateToParent, value))
			{
				OnPropertyChanging(__KnownINotifyPropertyChangingArgs.CanNavigateToParent);
				_canNavigateToParent = value;
				OnPropertyChanged(__KnownINotifyPropertyChangedArgs.CanNavigateToParent);
			}
		}
	}

	[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
	[ExcludeFromCodeCoverage]
	public bool CanGoBack
	{
		get
		{
			return _canGoBack;
		}
		set
		{
			if (!EqualityComparer<bool>.Default.Equals(_canGoBack, value))
			{
				OnPropertyChanging(__KnownINotifyPropertyChangingArgs.CanGoBack);
				_canGoBack = value;
				OnCanGoBackChanged(value);
				OnPropertyChanged(__KnownINotifyPropertyChangedArgs.CanGoBack);
			}
		}
	}

	[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
	[ExcludeFromCodeCoverage]
	public bool CanGoForward
	{
		get
		{
			return _canGoForward;
		}
		set
		{
			if (!EqualityComparer<bool>.Default.Equals(_canGoForward, value))
			{
				OnPropertyChanging(__KnownINotifyPropertyChangingArgs.CanGoForward);
				_canGoForward = value;
				OnCanGoForwardChanged(value);
				OnPropertyChanged(__KnownINotifyPropertyChangedArgs.CanGoForward);
			}
		}
	}

	[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
	[ExcludeFromCodeCoverage]
	public bool HasItem
	{
		get
		{
			return _hasItem;
		}
		set
		{
			if (!EqualityComparer<bool>.Default.Equals(_hasItem, value))
			{
				OnPropertyChanging(__KnownINotifyPropertyChangingArgs.HasItem);
				_hasItem = value;
				OnPropertyChanged(__KnownINotifyPropertyChangedArgs.HasItem);
			}
		}
	}

	[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.RelayCommandGenerator", "8.4.0.0")]
	[ExcludeFromCodeCoverage]
	public IRelayCommand NavigateToParentCommand => navigateToParentCommand ?? (navigateToParentCommand = new RelayCommand(NavigateToParent));

	[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.RelayCommandGenerator", "8.4.0.0")]
	[ExcludeFromCodeCoverage]
	public IRelayCommand GoBackCommand => goBackCommand ?? (goBackCommand = new RelayCommand(GoBack));

	[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.RelayCommandGenerator", "8.4.0.0")]
	[ExcludeFromCodeCoverage]
	public IRelayCommand GoForwardCommand => goForwardCommand ?? (goForwardCommand = new RelayCommand(GoForward));

	public event EventHandler? PathModeRequested;

	private void UpdatePathComponents(string path)
	{
		PathComponents.Clear();
		if (!string.IsNullOrEmpty(path))
		{
			List<PathBoxItem> directoryPathComponents = GetDirectoryPathComponents(path);
			for (int i = 0; i < directoryPathComponents.Count; i++)
			{
				directoryPathComponents[i].IsLastItem = i == directoryPathComponents.Count - 1;
				PathComponents.Add(directoryPathComponents[i]);
			}
		}
	}

	public static List<PathBoxItem> GetDirectoryPathComponents(string value)
	{
		List<PathBoxItem> list = new List<PathBoxItem>();
		if (string.IsNullOrEmpty(value))
		{
			return list;
		}
		value = value.TrimEnd('/');
		if (string.IsNullOrEmpty(value))
		{
			value = "/";
		}
		list.Add(new PathBoxItem
		{
			Title = "/",
			Path = "/",
			ChevronToolTip = Resources.BreadcrumbViewSubfolders
		});
		if (value == "/")
		{
			return list;
		}
		string[] array = value.Split('/', StringSplitOptions.RemoveEmptyEntries);
		string text = string.Empty;
		string[] array2 = array;
		foreach (string text2 in array2)
		{
			text = text + "/" + text2;
			list.Add(new PathBoxItem
			{
				Title = text2,
				Path = text,
				ChevronToolTip = string.Format(Resources.BreadcrumbViewSubfoldersOf, text2)
			});
		}
		return list;
	}

	[RelayCommand]
	private void NavigateToParent()
	{
		string parentPath = PathHelpers.GetParentPath(PathControlDisplayText);
		ParentVM?.SetWorkingDirectory(parentPath, SetNavPathLocationEnum.NavigateToParent);
	}

	[RelayCommand]
	private void GoBack()
	{
		ParentVM?.GoBack();
	}

	[RelayCommand]
	private void GoForward()
	{
		ParentVM?.GoForward();
	}

	public Task SwitchToPathMode()
	{
		IsFocused = true;
		Path2Go = string.Empty;
		PathModeRequested?.Invoke(this, EventArgs.Empty);
		return Task.CompletedTask;
	}

	public void UpdateUIState()
	{
		CanGoBack = ParentVM?.SelectedTab?.CanGoBack() == true;
		CanGoForward = ParentVM?.SelectedTab?.CanGoForward() == true;
	}

	[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
	private void OnPathControlDisplayTextChanged(string value)
	{
		CanNavigateToParent = !PathHelpers.IsRootPath(value);
		UpdatePathComponents(value);
	}

	[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
	private void OnCanGoBackChanged(bool value)
	{
		OnPropertyChanged("CanGoBackOrForward");
	}

	[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
	private void OnCanGoForwardChanged(bool value)
	{
		OnPropertyChanged("CanGoBackOrForward");
	}
}
