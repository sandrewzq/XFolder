using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel.__Internals;

namespace Files.ViewModels.Items;

public class MenuItemViewModel : ObservableObject
{
	[ObservableProperty]
	private string _header = string.Empty;

	[ObservableProperty]
	private ICommand? _command;

	[ObservableProperty]
	private object? _commandParameter;

	[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
	[ExcludeFromCodeCoverage]
	public string Header
	{
		get
		{
			return _header;
		}
		[MemberNotNull("_header")]
		set
		{
			if (!EqualityComparer<string>.Default.Equals(_header, value))
			{
				OnPropertyChanging(__KnownINotifyPropertyChangingArgs.Header);
				_header = value;
				OnPropertyChanged(__KnownINotifyPropertyChangedArgs.Header);
			}
		}
	}

	[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
	[ExcludeFromCodeCoverage]
	public ICommand? Command
	{
		get
		{
			return _command;
		}
		set
		{
			if (!EqualityComparer<ICommand>.Default.Equals(_command, value))
			{
				OnPropertyChanging(__KnownINotifyPropertyChangingArgs.Command);
				_command = value;
				OnPropertyChanged(__KnownINotifyPropertyChangedArgs.Command);
			}
		}
	}

	[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
	[ExcludeFromCodeCoverage]
	public object? CommandParameter
	{
		get
		{
			return _commandParameter;
		}
		set
		{
			if (!EqualityComparer<object>.Default.Equals(_commandParameter, value))
			{
				OnPropertyChanging(__KnownINotifyPropertyChangingArgs.CommandParameter);
				_commandParameter = value;
				OnPropertyChanged(__KnownINotifyPropertyChangedArgs.CommandParameter);
			}
		}
	}
}
