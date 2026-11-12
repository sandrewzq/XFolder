using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel.__Internals;

namespace Files.ViewModels.Dialogs;

public class ProgressDialogViewModel : ObservableObject, IDisposable
{
	[ObservableProperty]
	private string _titleText = string.Empty;

	[ObservableProperty]
	private string _subtitleText = string.Empty;

	[ObservableProperty]
	private string _currentItemText = string.Empty;

	[ObservableProperty]
	private bool _isIndeterminate = true;

	[ObservableProperty]
	private double _progressValue;

	[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
	[ExcludeFromCodeCoverage]
	public string TitleText
	{
		get
		{
			return _titleText;
		}
		[MemberNotNull("_titleText")]
		set
		{
			if (!EqualityComparer<string>.Default.Equals(_titleText, value))
			{
				OnPropertyChanging(__KnownINotifyPropertyChangingArgs.TitleText);
				_titleText = value;
				OnPropertyChanged(__KnownINotifyPropertyChangedArgs.TitleText);
			}
		}
	}

	[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
	[ExcludeFromCodeCoverage]
	public string SubtitleText
	{
		get
		{
			return _subtitleText;
		}
		[MemberNotNull("_subtitleText")]
		set
		{
			if (!EqualityComparer<string>.Default.Equals(_subtitleText, value))
			{
				OnPropertyChanging(__KnownINotifyPropertyChangingArgs.SubtitleText);
				_subtitleText = value;
				OnPropertyChanged(__KnownINotifyPropertyChangedArgs.SubtitleText);
			}
		}
	}

	[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
	[ExcludeFromCodeCoverage]
	public string CurrentItemText
	{
		get
		{
			return _currentItemText;
		}
		[MemberNotNull("_currentItemText")]
		set
		{
			if (!EqualityComparer<string>.Default.Equals(_currentItemText, value))
			{
				OnPropertyChanging(__KnownINotifyPropertyChangingArgs.CurrentItemText);
				_currentItemText = value;
				OnPropertyChanged(__KnownINotifyPropertyChangedArgs.CurrentItemText);
			}
		}
	}

	[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
	[ExcludeFromCodeCoverage]
	public bool IsIndeterminate
	{
		get
		{
			return _isIndeterminate;
		}
		set
		{
			if (!EqualityComparer<bool>.Default.Equals(_isIndeterminate, value))
			{
				OnPropertyChanging(__KnownINotifyPropertyChangingArgs.IsIndeterminate);
				_isIndeterminate = value;
				OnPropertyChanged(__KnownINotifyPropertyChangedArgs.IsIndeterminate);
			}
		}
	}

	[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
	[ExcludeFromCodeCoverage]
	public double ProgressValue
	{
		get
		{
			return _progressValue;
		}
		set
		{
			if (!EqualityComparer<double>.Default.Equals(_progressValue, value))
			{
				OnPropertyChanging(__KnownINotifyPropertyChangingArgs.ProgressValue);
				_progressValue = value;
				OnPropertyChanged(__KnownINotifyPropertyChangedArgs.ProgressValue);
			}
		}
	}

	public void Dispose()
	{
	}
}
