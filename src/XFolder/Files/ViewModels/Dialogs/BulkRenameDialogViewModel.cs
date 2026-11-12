using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel.__Internals;
using Files.Utils.Storage.Operations;

namespace Files.ViewModels.Dialogs;

public sealed class BulkRenameDialogViewModel : ObservableObject, IDisposable
{
	public object? AdditionalData;

	[ObservableProperty]
	private string _FileName = string.Empty;

	public bool IsNameValid => FilesystemHelpers.IsValidForFilename(FileName);

	[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
	[ExcludeFromCodeCoverage]
	public string FileName
	{
		get
		{
			return _FileName;
		}
		[MemberNotNull("_FileName")]
		set
		{
			if (!EqualityComparer<string>.Default.Equals(_FileName, value))
			{
				OnPropertyChanging(__KnownINotifyPropertyChangingArgs.FileName);
				_FileName = value;
				OnFileNameChanged(value);
				OnPropertyChanged(__KnownINotifyPropertyChangedArgs.FileName);
			}
		}
	}

	public void Dispose()
	{
	}

	[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
	private void OnFileNameChanged(string value)
	{
		OnPropertyChanged("IsNameValid");
		AdditionalData = (IsNameValid ? FileName : null);
	}
}
