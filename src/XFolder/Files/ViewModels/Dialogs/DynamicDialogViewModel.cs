using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel.__Internals;
using CommunityToolkit.Mvvm.Input;
using Files.Data.Enums;

namespace Files.ViewModels.Dialogs;

public class DynamicDialogViewModel : ObservableObject, IDisposable
{
	[ObservableProperty]
	private object additionalData = new object();

	[ObservableProperty]
	private DynamicDialogButtons dynamicButtons;

	[ObservableProperty]
	private string titleText = string.Empty;

	[ObservableProperty]
	private string subtitleText = string.Empty;

	[ObservableProperty]
	private string primaryButtonText = string.Empty;

	[ObservableProperty]
	private string secondaryButtonText = string.Empty;

	[ObservableProperty]
	private string closeButtonText = string.Empty;

	public IRelayCommand<object> KeyDownCommand { get; }

	public Action<DynamicDialogViewModel, object>? KeyDownAction { get; set; }

	[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
	[ExcludeFromCodeCoverage]
	public object AdditionalData
	{
		get
		{
			return additionalData;
		}
		[MemberNotNull("additionalData")]
		set
		{
			if (!EqualityComparer<object>.Default.Equals(additionalData, value))
			{
				OnPropertyChanging(__KnownINotifyPropertyChangingArgs.AdditionalData);
				additionalData = value;
				OnPropertyChanged(__KnownINotifyPropertyChangedArgs.AdditionalData);
			}
		}
	}

	[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
	[ExcludeFromCodeCoverage]
	public DynamicDialogButtons DynamicButtons
	{
		get
		{
			return dynamicButtons;
		}
		set
		{
			if (!EqualityComparer<DynamicDialogButtons>.Default.Equals(dynamicButtons, value))
			{
				OnPropertyChanging(__KnownINotifyPropertyChangingArgs.DynamicButtons);
				dynamicButtons = value;
				OnDynamicButtonsChanged(value);
				OnPropertyChanged(__KnownINotifyPropertyChangedArgs.DynamicButtons);
			}
		}
	}

	[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
	[ExcludeFromCodeCoverage]
	public string TitleText
	{
		get
		{
			return titleText;
		}
		[MemberNotNull("titleText")]
		set
		{
			if (!EqualityComparer<string>.Default.Equals(titleText, value))
			{
				OnPropertyChanging(__KnownINotifyPropertyChangingArgs.TitleText);
				titleText = value;
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
			return subtitleText;
		}
		[MemberNotNull("subtitleText")]
		set
		{
			if (!EqualityComparer<string>.Default.Equals(subtitleText, value))
			{
				OnPropertyChanging(__KnownINotifyPropertyChangingArgs.SubtitleText);
				subtitleText = value;
				OnPropertyChanged(__KnownINotifyPropertyChangedArgs.SubtitleText);
			}
		}
	}

	[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
	[ExcludeFromCodeCoverage]
	public string PrimaryButtonText
	{
		get
		{
			return primaryButtonText;
		}
		[MemberNotNull("primaryButtonText")]
		set
		{
			if (!EqualityComparer<string>.Default.Equals(primaryButtonText, value))
			{
				OnPropertyChanging(__KnownINotifyPropertyChangingArgs.PrimaryButtonText);
				primaryButtonText = value;
				OnPropertyChanged(__KnownINotifyPropertyChangedArgs.PrimaryButtonText);
			}
		}
	}

	[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
	[ExcludeFromCodeCoverage]
	public string SecondaryButtonText
	{
		get
		{
			return secondaryButtonText;
		}
		[MemberNotNull("secondaryButtonText")]
		set
		{
			if (!EqualityComparer<string>.Default.Equals(secondaryButtonText, value))
			{
				OnPropertyChanging(__KnownINotifyPropertyChangingArgs.SecondaryButtonText);
				secondaryButtonText = value;
				OnPropertyChanged(__KnownINotifyPropertyChangedArgs.SecondaryButtonText);
			}
		}
	}

	[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
	[ExcludeFromCodeCoverage]
	public string CloseButtonText
	{
		get
		{
			return closeButtonText;
		}
		[MemberNotNull("closeButtonText")]
		set
		{
			if (!EqualityComparer<string>.Default.Equals(closeButtonText, value))
			{
				OnPropertyChanging(__KnownINotifyPropertyChangingArgs.CloseButtonText);
				closeButtonText = value;
				OnPropertyChanged(__KnownINotifyPropertyChangedArgs.CloseButtonText);
			}
		}
	}

	public DynamicDialogViewModel()
	{
		KeyDownCommand = new RelayCommand<object>(ExecuteKeyDownCommand);
	}

	private void ExecuteKeyDownCommand(object? parameter)
	{
		KeyDownAction?.Invoke(this, parameter ?? new object());
	}

	private void UpdateButtonVisibility(DynamicDialogButtons buttons)
	{
		if (buttons.HasFlag(DynamicDialogButtons.None))
		{
			PrimaryButtonText = string.Empty;
			SecondaryButtonText = string.Empty;
			CloseButtonText = string.Empty;
			return;
		}
		if (!buttons.HasFlag(DynamicDialogButtons.Primary))
		{
			PrimaryButtonText = string.Empty;
		}
		if (!buttons.HasFlag(DynamicDialogButtons.Secondary))
		{
			SecondaryButtonText = string.Empty;
		}
		if (!buttons.HasFlag(DynamicDialogButtons.Cancel))
		{
			CloseButtonText = string.Empty;
		}
	}

	public void Dispose()
	{
		AdditionalData = new object();
		KeyDownAction = null;
	}

	[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
	private void OnDynamicButtonsChanged(DynamicDialogButtons value)
	{
		UpdateButtonVisibility(value);
	}
}
