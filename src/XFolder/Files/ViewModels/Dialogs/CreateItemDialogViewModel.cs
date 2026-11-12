using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel.__Internals;
using Files.Utils.Storage.Operations;

namespace Files.ViewModels.Dialogs;

public class CreateItemDialogViewModel : ObservableObject, IDisposable
{
	public object? AdditionalData;

	[ObservableProperty]
	private string _itemName = string.Empty;

	public bool IsNameValid => FilesystemHelpers.IsValidForFilename(ItemName);

	[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
	[ExcludeFromCodeCoverage]
	public string ItemName
	{
		get
		{
			return _itemName;
		}
		[MemberNotNull("_itemName")]
		set
		{
			if (!EqualityComparer<string>.Default.Equals(_itemName, value))
			{
				OnPropertyChanging(__KnownINotifyPropertyChangingArgs.ItemName);
				_itemName = value;
				OnItemNameChanged(value);
				OnPropertyChanged(__KnownINotifyPropertyChangedArgs.ItemName);
			}
		}
	}

	public void Dispose()
	{
	}

	[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
	private void OnItemNameChanged(string value)
	{
		OnPropertyChanged("IsNameValid");
		AdditionalData = (IsNameValid ? ItemName : null);
	}
}
