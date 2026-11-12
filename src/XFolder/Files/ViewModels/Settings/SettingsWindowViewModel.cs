using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel.__Internals;
using Files.Assets.Lang;
using Files.Data.Contracts;
using Files.Data.Enums;
using Files.Data.Models;
using Files.Helpers;
using Files.Helpers.Application;
using Files.Helpers.Shared;

namespace Files.ViewModels.Settings;

public class SettingsWindowViewModel : ObservableObject
{
	private readonly IAppThemeService _appThemeService;

	private readonly IFoldersSettingsService _foldersSettingsService;

	private readonly IGeneralSettingsService _generalSettingsService;

	private readonly ILayoutSettingsService _layoutSettingsService;

	[ObservableProperty]
	private AppLanguageItem? _selectedLanguage;

	[ObservableProperty]
	private AppThemeItem? _selectedTheme;

	[ObservableProperty]
	private string _newTabDefaultPath = string.Empty;

	[ObservableProperty]
	private bool _continueLastSessionOnStartUp;

	[ObservableProperty]
	private bool _openMainWindowMaximized;

	[ObservableProperty]
	private int _fontSize;

	[ObservableProperty]
	private bool _showHiddenItems;

	[ObservableProperty]
	private bool _showFileExtensions;

	[ObservableProperty]
	private bool _showThumbnails;

	[ObservableProperty]
	private bool _scrollToPreviousFolderWhenNavigatingUp;

	private bool _doubleClickToGoUp;

	[ObservableProperty]
	private bool _showFileExtensionWarning;

	[ObservableProperty]
	private SettingsOptionItem<FolderLayoutModes>? _selectedDefaultLayoutMode;

	[ObservableProperty]
	private SettingsOptionItem<SortOption>? _selectedDefaultSortOption;

	[ObservableProperty]
	private SettingsOptionItem<SortDirection>? _selectedDefaultSortDirection;

	[ObservableProperty]
	private SettingsOptionItem<DeleteConfirmationPolicies>? _selectedDeleteConfirmationPolicy;

	public IReadOnlyList<AppLanguageItem> SupportedLanguages => AppLanguageHelper.SupportedLanguages;

	public IReadOnlyList<AppThemeItem> SupportedThemes { get; } = new _003C_003Ez__ReadOnlyArray<AppThemeItem>(new AppThemeItem[3]
	{
		new AppThemeItem
		{
			Code = "auto",
			DisplayName = Resources.Auto
		},
		new AppThemeItem
		{
			Code = "light",
			DisplayName = Resources.LightTheme
		},
		new AppThemeItem
		{
			Code = "dark",
			DisplayName = Resources.DarkTheme
		}
	});

	public IReadOnlyList<SettingsOptionItem<FolderLayoutModes>> SupportedDefaultLayoutModes { get; } = new _003C_003Ez__ReadOnlyArray<SettingsOptionItem<FolderLayoutModes>>(new SettingsOptionItem<FolderLayoutModes>[2]
	{
		new SettingsOptionItem<FolderLayoutModes>
		{
			Value = FolderLayoutModes.DetailsView,
			DisplayName = Resources.Details
		},
		new SettingsOptionItem<FolderLayoutModes>
		{
			Value = FolderLayoutModes.GridView,
			DisplayName = Resources.Grid
		}
	});

	public IReadOnlyList<SettingsOptionItem<SortOption>> SupportedDefaultSortOptions { get; } = new _003C_003Ez__ReadOnlyArray<SettingsOptionItem<SortOption>>(new SettingsOptionItem<SortOption>[4]
	{
		new SettingsOptionItem<SortOption>
		{
			Value = SortOption.Name,
			DisplayName = Resources.Name
		},
		new SettingsOptionItem<SortOption>
		{
			Value = SortOption.Size,
			DisplayName = Resources.Size
		},
		new SettingsOptionItem<SortOption>
		{
			Value = SortOption.FileType,
			DisplayName = Resources.Type
		},
		new SettingsOptionItem<SortOption>
		{
			Value = SortOption.DateModified,
			DisplayName = Resources.DateModifiedLowerCase
		}
	});

	public IReadOnlyList<SettingsOptionItem<SortDirection>> SupportedDefaultSortDirections { get; } = new _003C_003Ez__ReadOnlyArray<SettingsOptionItem<SortDirection>>(new SettingsOptionItem<SortDirection>[2]
	{
		new SettingsOptionItem<SortDirection>
		{
			Value = SortDirection.Ascending,
			DisplayName = Resources.Ascending
		},
		new SettingsOptionItem<SortDirection>
		{
			Value = SortDirection.Descending,
			DisplayName = Resources.Descending
		}
	});

	public IReadOnlyList<SettingsOptionItem<DeleteConfirmationPolicies>> SupportedDeleteConfirmationPolicies { get; } = new _003C_003Ez__ReadOnlyArray<SettingsOptionItem<DeleteConfirmationPolicies>>(new SettingsOptionItem<DeleteConfirmationPolicies>[2]
	{
		new SettingsOptionItem<DeleteConfirmationPolicies>
		{
			Value = DeleteConfirmationPolicies.Always,
			DisplayName = Resources.Always
		},
		new SettingsOptionItem<DeleteConfirmationPolicies>
		{
			Value = DeleteConfirmationPolicies.PermanentOnly,
			DisplayName = Resources.PermanentDeletionOnly
		}
	});

	public int FontSizeMinimum => 9;

	public int FontSizeMaximum => 24;

	public string FontSizeDisplayText => $"{FontSize}";

	public bool DoubleClickToGoUp
	{
		get
		{
			return _doubleClickToGoUp;
		}
		set
		{
			if (SetProperty(ref _doubleClickToGoUp, value, "DoubleClickToGoUp"))
			{
				_foldersSettingsService.DoubleClickToGoUp = value;
			}
		}
	}

	[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
	[ExcludeFromCodeCoverage]
	public AppLanguageItem? SelectedLanguage
	{
		get
		{
			return _selectedLanguage;
		}
		set
		{
			if (!EqualityComparer<AppLanguageItem>.Default.Equals(_selectedLanguage, value))
			{
				OnPropertyChanging(__KnownINotifyPropertyChangingArgs.SelectedLanguage);
				_selectedLanguage = value;
				OnSelectedLanguageChanged(value);
				OnPropertyChanged(__KnownINotifyPropertyChangedArgs.SelectedLanguage);
			}
		}
	}

	[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
	[ExcludeFromCodeCoverage]
	public AppThemeItem? SelectedTheme
	{
		get
		{
			return _selectedTheme;
		}
		set
		{
			if (!EqualityComparer<AppThemeItem>.Default.Equals(_selectedTheme, value))
			{
				OnPropertyChanging(__KnownINotifyPropertyChangingArgs.SelectedTheme);
				_selectedTheme = value;
				OnSelectedThemeChanged(value);
				OnPropertyChanged(__KnownINotifyPropertyChangedArgs.SelectedTheme);
			}
		}
	}

	[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
	[ExcludeFromCodeCoverage]
	public string NewTabDefaultPath
	{
		get
		{
			return _newTabDefaultPath;
		}
		[MemberNotNull("_newTabDefaultPath")]
		set
		{
			if (!EqualityComparer<string>.Default.Equals(_newTabDefaultPath, value))
			{
				OnPropertyChanging(__KnownINotifyPropertyChangingArgs.NewTabDefaultPath);
				_newTabDefaultPath = value;
				OnNewTabDefaultPathChanged(value);
				OnPropertyChanged(__KnownINotifyPropertyChangedArgs.NewTabDefaultPath);
			}
		}
	}

	[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
	[ExcludeFromCodeCoverage]
	public bool ContinueLastSessionOnStartUp
	{
		get
		{
			return _continueLastSessionOnStartUp;
		}
		set
		{
			if (!EqualityComparer<bool>.Default.Equals(_continueLastSessionOnStartUp, value))
			{
				OnPropertyChanging(__KnownINotifyPropertyChangingArgs.ContinueLastSessionOnStartUp);
				_continueLastSessionOnStartUp = value;
				OnContinueLastSessionOnStartUpChanged(value);
				OnPropertyChanged(__KnownINotifyPropertyChangedArgs.ContinueLastSessionOnStartUp);
			}
		}
	}

	[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
	[ExcludeFromCodeCoverage]
	public bool OpenMainWindowMaximized
	{
		get
		{
			return _openMainWindowMaximized;
		}
		set
		{
			if (!EqualityComparer<bool>.Default.Equals(_openMainWindowMaximized, value))
			{
				OnPropertyChanging(__KnownINotifyPropertyChangingArgs.OpenMainWindowMaximized);
				_openMainWindowMaximized = value;
				OnOpenMainWindowMaximizedChanged(value);
				OnPropertyChanged(__KnownINotifyPropertyChangedArgs.OpenMainWindowMaximized);
			}
		}
	}

	[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
	[ExcludeFromCodeCoverage]
	public int FontSize
	{
		get
		{
			return _fontSize;
		}
		set
		{
			if (!EqualityComparer<int>.Default.Equals(_fontSize, value))
			{
				OnPropertyChanging(__KnownINotifyPropertyChangingArgs.FontSize);
				_fontSize = value;
				OnFontSizeChanged(value);
				OnPropertyChanged(__KnownINotifyPropertyChangedArgs.FontSize);
			}
		}
	}

	[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
	[ExcludeFromCodeCoverage]
	public bool ShowHiddenItems
	{
		get
		{
			return _showHiddenItems;
		}
		set
		{
			if (!EqualityComparer<bool>.Default.Equals(_showHiddenItems, value))
			{
				OnPropertyChanging(__KnownINotifyPropertyChangingArgs.ShowHiddenItems);
				_showHiddenItems = value;
				OnShowHiddenItemsChanged(value);
				OnPropertyChanged(__KnownINotifyPropertyChangedArgs.ShowHiddenItems);
			}
		}
	}

	[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
	[ExcludeFromCodeCoverage]
	public bool ShowFileExtensions
	{
		get
		{
			return _showFileExtensions;
		}
		set
		{
			if (!EqualityComparer<bool>.Default.Equals(_showFileExtensions, value))
			{
				OnPropertyChanging(__KnownINotifyPropertyChangingArgs.ShowFileExtensions);
				_showFileExtensions = value;
				OnShowFileExtensionsChanged(value);
				OnPropertyChanged(__KnownINotifyPropertyChangedArgs.ShowFileExtensions);
			}
		}
	}

	[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
	[ExcludeFromCodeCoverage]
	public bool ShowThumbnails
	{
		get
		{
			return _showThumbnails;
		}
		set
		{
			if (!EqualityComparer<bool>.Default.Equals(_showThumbnails, value))
			{
				OnPropertyChanging(__KnownINotifyPropertyChangingArgs.ShowThumbnails);
				_showThumbnails = value;
				OnShowThumbnailsChanged(value);
				OnPropertyChanged(__KnownINotifyPropertyChangedArgs.ShowThumbnails);
			}
		}
	}

	[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
	[ExcludeFromCodeCoverage]
	public bool ScrollToPreviousFolderWhenNavigatingUp
	{
		get
		{
			return _scrollToPreviousFolderWhenNavigatingUp;
		}
		set
		{
			if (!EqualityComparer<bool>.Default.Equals(_scrollToPreviousFolderWhenNavigatingUp, value))
			{
				OnPropertyChanging(__KnownINotifyPropertyChangingArgs.ScrollToPreviousFolderWhenNavigatingUp);
				_scrollToPreviousFolderWhenNavigatingUp = value;
				OnScrollToPreviousFolderWhenNavigatingUpChanged(value);
				OnPropertyChanged(__KnownINotifyPropertyChangedArgs.ScrollToPreviousFolderWhenNavigatingUp);
			}
		}
	}

	[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
	[ExcludeFromCodeCoverage]
	public bool ShowFileExtensionWarning
	{
		get
		{
			return _showFileExtensionWarning;
		}
		set
		{
			if (!EqualityComparer<bool>.Default.Equals(_showFileExtensionWarning, value))
			{
				OnPropertyChanging(__KnownINotifyPropertyChangingArgs.ShowFileExtensionWarning);
				_showFileExtensionWarning = value;
				OnShowFileExtensionWarningChanged(value);
				OnPropertyChanged(__KnownINotifyPropertyChangedArgs.ShowFileExtensionWarning);
			}
		}
	}

	[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
	[ExcludeFromCodeCoverage]
	public SettingsOptionItem<FolderLayoutModes>? SelectedDefaultLayoutMode
	{
		get
		{
			return _selectedDefaultLayoutMode;
		}
		set
		{
			if (!EqualityComparer<SettingsOptionItem<FolderLayoutModes>>.Default.Equals(_selectedDefaultLayoutMode, value))
			{
				OnPropertyChanging(__KnownINotifyPropertyChangingArgs.SelectedDefaultLayoutMode);
				_selectedDefaultLayoutMode = value;
				OnSelectedDefaultLayoutModeChanged(value);
				OnPropertyChanged(__KnownINotifyPropertyChangedArgs.SelectedDefaultLayoutMode);
			}
		}
	}

	[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
	[ExcludeFromCodeCoverage]
	public SettingsOptionItem<SortOption>? SelectedDefaultSortOption
	{
		get
		{
			return _selectedDefaultSortOption;
		}
		set
		{
			if (!EqualityComparer<SettingsOptionItem<SortOption>>.Default.Equals(_selectedDefaultSortOption, value))
			{
				OnPropertyChanging(__KnownINotifyPropertyChangingArgs.SelectedDefaultSortOption);
				_selectedDefaultSortOption = value;
				OnSelectedDefaultSortOptionChanged(value);
				OnPropertyChanged(__KnownINotifyPropertyChangedArgs.SelectedDefaultSortOption);
			}
		}
	}

	[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
	[ExcludeFromCodeCoverage]
	public SettingsOptionItem<SortDirection>? SelectedDefaultSortDirection
	{
		get
		{
			return _selectedDefaultSortDirection;
		}
		set
		{
			if (!EqualityComparer<SettingsOptionItem<SortDirection>>.Default.Equals(_selectedDefaultSortDirection, value))
			{
				OnPropertyChanging(__KnownINotifyPropertyChangingArgs.SelectedDefaultSortDirection);
				_selectedDefaultSortDirection = value;
				OnSelectedDefaultSortDirectionChanged(value);
				OnPropertyChanged(__KnownINotifyPropertyChangedArgs.SelectedDefaultSortDirection);
			}
		}
	}

	[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
	[ExcludeFromCodeCoverage]
	public SettingsOptionItem<DeleteConfirmationPolicies>? SelectedDeleteConfirmationPolicy
	{
		get
		{
			return _selectedDeleteConfirmationPolicy;
		}
		set
		{
			if (!EqualityComparer<SettingsOptionItem<DeleteConfirmationPolicies>>.Default.Equals(_selectedDeleteConfirmationPolicy, value))
			{
				OnPropertyChanging(__KnownINotifyPropertyChangingArgs.SelectedDeleteConfirmationPolicy);
				_selectedDeleteConfirmationPolicy = value;
				OnSelectedDeleteConfirmationPolicyChanged(value);
				OnPropertyChanged(__KnownINotifyPropertyChangedArgs.SelectedDeleteConfirmationPolicy);
			}
		}
	}

	public SettingsWindowViewModel(IAppThemeService appThemeService, IFoldersSettingsService foldersSettingsService, IGeneralSettingsService generalSettingsService, ILayoutSettingsService layoutSettingsService)
	{
		_appThemeService = appThemeService;
		_foldersSettingsService = foldersSettingsService;
		_generalSettingsService = generalSettingsService;
		_layoutSettingsService = layoutSettingsService;
		AppLanguageHelper.LanguageChanged += AppLanguageHelperOnLanguageChanged;
		_appThemeService.ThemeChanged += AppThemeServiceOnThemeChanged;
		_foldersSettingsService.PropertyChanged += FoldersSettingsServiceOnPropertyChanged;
		_generalSettingsService.PropertyChanged += GeneralSettingsServiceOnPropertyChanged;
		_layoutSettingsService.PropertyChanged += LayoutSettingsServiceOnPropertyChanged;
		UpdateSelectedLanguage();
		UpdateSelectedTheme();
		UpdateNewTabDefaultPath();
		UpdateContinueLastSessionOnStartUp();
		UpdateOpenMainWindowMaximized();
		UpdateFontSize();
		UpdateShowHiddenItems();
		UpdateShowFileExtensions();
		UpdateShowThumbnails();
		UpdateScrollToPreviousFolderWhenNavigatingUp();
		UpdateDoubleClickToGoUp();
		UpdateShowFileExtensionWarning();
		UpdateSelectedDefaultLayoutMode();
		UpdateSelectedDefaultSortOption();
		UpdateSelectedDefaultSortDirection();
		UpdateSelectedDeleteConfirmationPolicy();
	}

	private void AppLanguageHelperOnLanguageChanged(object? sender, EventArgs e)
	{
		UpdateSelectedLanguage();
	}

	private void AppThemeServiceOnThemeChanged(object? sender, EventArgs e)
	{
		UpdateSelectedTheme();
	}

	private void FoldersSettingsServiceOnPropertyChanged(object? sender, PropertyChangedEventArgs e)
	{
		if (e.PropertyName == "ShowHiddenItems")
		{
			UpdateShowHiddenItems();
		}
		else if (e.PropertyName == "ShowFileExtensions")
		{
			UpdateShowFileExtensions();
		}
		else if (e.PropertyName == "ShowThumbnails")
		{
			UpdateShowThumbnails();
		}
		else if (e.PropertyName == "ScrollToPreviousFolderWhenNavigatingUp")
		{
			UpdateScrollToPreviousFolderWhenNavigatingUp();
		}
		else if (e.PropertyName == "DoubleClickToGoUp")
		{
			UpdateDoubleClickToGoUp();
		}
		else if (e.PropertyName == "ShowFileExtensionWarning")
		{
			UpdateShowFileExtensionWarning();
		}
		else if (e.PropertyName == "DeleteConfirmationPolicy")
		{
			UpdateSelectedDeleteConfirmationPolicy();
		}
	}

	private void GeneralSettingsServiceOnPropertyChanged(object? sender, PropertyChangedEventArgs e)
	{
		if (e.PropertyName == "NewTabDefaultPath")
		{
			UpdateNewTabDefaultPath();
		}
		else if (e.PropertyName == "ContinueLastSessionOnStartUp")
		{
			UpdateContinueLastSessionOnStartUp();
		}
		else if (e.PropertyName == "OpenMainWindowMaximized")
		{
			UpdateOpenMainWindowMaximized();
		}
		else if (e.PropertyName == "FontSize")
		{
			UpdateFontSize();
		}
	}

	private void LayoutSettingsServiceOnPropertyChanged(object? sender, PropertyChangedEventArgs e)
	{
		if (e.PropertyName == "DefaultLayoutMode")
		{
			UpdateSelectedDefaultLayoutMode();
		}
		else if (e.PropertyName == "DefaultSortOption")
		{
			UpdateSelectedDefaultSortOption();
		}
		else if (e.PropertyName == "DefaultSortDirection")
		{
			UpdateSelectedDefaultSortDirection();
		}
	}

	private void UpdateSelectedLanguage()
	{
		SelectedLanguage = SupportedLanguages.FirstOrDefault((AppLanguageItem lang) => lang.Code.Equals(AppLanguageHelper.CurrentLanguageCode, StringComparison.OrdinalIgnoreCase));
	}

	private void UpdateSelectedTheme()
	{
		SelectedTheme = SupportedThemes.FirstOrDefault((AppThemeItem theme) => theme.Code.Equals(_appThemeService.CurrentThemeCode, StringComparison.OrdinalIgnoreCase));
	}

	private void UpdateNewTabDefaultPath()
	{
		NewTabDefaultPath = NavigationHelpers.GetDefaultNewTabPath();
	}

	private void UpdateContinueLastSessionOnStartUp()
	{
		ContinueLastSessionOnStartUp = _generalSettingsService.ContinueLastSessionOnStartUp;
	}

	private void UpdateOpenMainWindowMaximized()
	{
		OpenMainWindowMaximized = _generalSettingsService.OpenMainWindowMaximized;
	}

	private void UpdateFontSize()
	{
		FontSize = _generalSettingsService.FontSize;
	}

	private void UpdateShowHiddenItems()
	{
		ShowHiddenItems = _foldersSettingsService.ShowHiddenItems;
	}

	private void UpdateShowFileExtensions()
	{
		ShowFileExtensions = _foldersSettingsService.ShowFileExtensions;
	}

	private void UpdateShowThumbnails()
	{
		ShowThumbnails = _foldersSettingsService.ShowThumbnails;
	}

	private void UpdateScrollToPreviousFolderWhenNavigatingUp()
	{
		ScrollToPreviousFolderWhenNavigatingUp = _foldersSettingsService.ScrollToPreviousFolderWhenNavigatingUp;
	}

	private void UpdateDoubleClickToGoUp()
	{
		SetProperty(ref _doubleClickToGoUp, _foldersSettingsService.DoubleClickToGoUp, "DoubleClickToGoUp");
	}

	private void UpdateShowFileExtensionWarning()
	{
		ShowFileExtensionWarning = _foldersSettingsService.ShowFileExtensionWarning;
	}

	private void UpdateSelectedDefaultLayoutMode()
	{
		SelectedDefaultLayoutMode = SupportedDefaultLayoutModes.FirstOrDefault((SettingsOptionItem<FolderLayoutModes> item) => item.Value == _layoutSettingsService.DefaultLayoutMode);
	}

	private void UpdateSelectedDefaultSortOption()
	{
		SelectedDefaultSortOption = SupportedDefaultSortOptions.FirstOrDefault((SettingsOptionItem<SortOption> item) => item.Value == _layoutSettingsService.DefaultSortOption);
	}

	private void UpdateSelectedDefaultSortDirection()
	{
		SelectedDefaultSortDirection = SupportedDefaultSortDirections.FirstOrDefault((SettingsOptionItem<SortDirection> item) => item.Value == _layoutSettingsService.DefaultSortDirection);
	}

	private void UpdateSelectedDeleteConfirmationPolicy()
	{
		SelectedDeleteConfirmationPolicy = SupportedDeleteConfirmationPolicies.FirstOrDefault((SettingsOptionItem<DeleteConfirmationPolicies> item) => item.Value == _foldersSettingsService.DeleteConfirmationPolicy);
	}

	[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
	private void OnSelectedLanguageChanged(AppLanguageItem? value)
	{
		if (value != null)
		{
			AppLanguageHelper.TryChange(value.Code);
		}
	}

	[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
	private void OnSelectedThemeChanged(AppThemeItem? value)
	{
		if (value != null)
		{
			_appThemeService.TryChangeTheme(value.Code);
		}
	}

	[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
	private void OnNewTabDefaultPathChanged(string value)
	{
		string text = PathHelpers.NormalizePath(value);
		_generalSettingsService.NewTabDefaultPath = (string.IsNullOrWhiteSpace(text) ? string.Empty : text);
	}

	[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
	private void OnContinueLastSessionOnStartUpChanged(bool value)
	{
		_generalSettingsService.ContinueLastSessionOnStartUp = value;
	}

	[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
	private void OnOpenMainWindowMaximizedChanged(bool value)
	{
		_generalSettingsService.OpenMainWindowMaximized = value;
	}

	[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
	private void OnFontSizeChanged(int value)
	{
		OnPropertyChanged("FontSizeDisplayText");
		_generalSettingsService.FontSize = value;
	}

	[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
	private void OnShowHiddenItemsChanged(bool value)
	{
		_foldersSettingsService.ShowHiddenItems = value;
	}

	[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
	private void OnShowFileExtensionsChanged(bool value)
	{
		_foldersSettingsService.ShowFileExtensions = value;
	}

	[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
	private void OnShowThumbnailsChanged(bool value)
	{
		_foldersSettingsService.ShowThumbnails = value;
	}

	[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
	private void OnScrollToPreviousFolderWhenNavigatingUpChanged(bool value)
	{
		_foldersSettingsService.ScrollToPreviousFolderWhenNavigatingUp = value;
	}

	[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
	private void OnShowFileExtensionWarningChanged(bool value)
	{
		_foldersSettingsService.ShowFileExtensionWarning = value;
	}

	[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
	private void OnSelectedDefaultLayoutModeChanged(SettingsOptionItem<FolderLayoutModes>? value)
	{
		if (value != null)
		{
			_layoutSettingsService.DefaultLayoutMode = value.Value;
		}
	}

	[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
	private void OnSelectedDefaultSortOptionChanged(SettingsOptionItem<SortOption>? value)
	{
		if (value != null)
		{
			_layoutSettingsService.DefaultSortOption = value.Value;
		}
	}

	[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
	private void OnSelectedDefaultSortDirectionChanged(SettingsOptionItem<SortDirection>? value)
	{
		if (value != null)
		{
			_layoutSettingsService.DefaultSortDirection = value.Value;
		}
	}

	[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
	private void OnSelectedDeleteConfirmationPolicyChanged(SettingsOptionItem<DeleteConfirmationPolicies>? value)
	{
		if (value != null)
		{
			_foldersSettingsService.DeleteConfirmationPolicy = value.Value;
		}
	}
}
