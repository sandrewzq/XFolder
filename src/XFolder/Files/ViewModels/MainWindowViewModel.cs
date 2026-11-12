using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel.__Internals;

namespace Files.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
	[ObservableProperty]
	private bool _isPaneOpen = true;

	[ObservableProperty]
	private ViewModelBase? _panesView;

	private GridLength _lastSidebarWidth = new GridLength(220.0);

	[ObservableProperty]
	private GridLength _sidebarGridLength = new GridLength(220.0);

	[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
	[ExcludeFromCodeCoverage]
	public bool IsPaneOpen
	{
		get
		{
			return _isPaneOpen;
		}
		set
		{
			if (!EqualityComparer<bool>.Default.Equals(_isPaneOpen, value))
			{
				OnPropertyChanging(__KnownINotifyPropertyChangingArgs.IsPaneOpen);
				_isPaneOpen = value;
				OnIsPaneOpenChanged(value);
				OnPropertyChanged(__KnownINotifyPropertyChangedArgs.IsPaneOpen);
			}
		}
	}

	[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
	[ExcludeFromCodeCoverage]
	public ViewModelBase? PanesView
	{
		get
		{
			return _panesView;
		}
		set
		{
			if (!EqualityComparer<ViewModelBase>.Default.Equals(_panesView, value))
			{
				OnPropertyChanging(__KnownINotifyPropertyChangingArgs.PanesView);
				_panesView = value;
				OnPropertyChanged(__KnownINotifyPropertyChangedArgs.PanesView);
			}
		}
	}

	[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
	[ExcludeFromCodeCoverage]
	public GridLength SidebarGridLength
	{
		get
		{
			return _sidebarGridLength;
		}
		set
		{
			if (!EqualityComparer<GridLength>.Default.Equals(_sidebarGridLength, value))
			{
				OnPropertyChanging(__KnownINotifyPropertyChangingArgs.SidebarGridLength);
				_sidebarGridLength = value;
				OnSidebarGridLengthChanged(value);
				OnPropertyChanged(__KnownINotifyPropertyChangedArgs.SidebarGridLength);
			}
		}
	}

	public double GetSidebarWidthForSession()
	{
		if (!IsPaneOpen)
		{
			return _lastSidebarWidth.Value;
		}
		return SidebarGridLength.Value;
	}

	public void SetSidebarWidthForSession(double width)
	{
		if (!(width <= 0.0))
		{
			_lastSidebarWidth = new GridLength(width);
			if (IsPaneOpen)
			{
				SidebarGridLength = _lastSidebarWidth;
			}
		}
	}

	[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
	private void OnIsPaneOpenChanged(bool value)
	{
		if (value)
		{
			SidebarGridLength = _lastSidebarWidth;
			return;
		}
		if (SidebarGridLength.Value > 0.0)
		{
			_lastSidebarWidth = SidebarGridLength;
		}
		SidebarGridLength = new GridLength(0.0);
	}

	[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
	private void OnSidebarGridLengthChanged(GridLength value)
	{
		if (IsPaneOpen)
		{
			_lastSidebarWidth = value;
		}
	}
}
