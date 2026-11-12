using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Files.Helpers.UI;

namespace Files.Actions;

internal abstract class BaseUIAction : ObservableObject
{
	public virtual bool IsExecutable => UIHelpers.CanShowDialog;

	public BaseUIAction()
	{
		UIHelpers.PropertyChanged += UIHelpers_PropertyChanged;
	}

	private void UIHelpers_PropertyChanged(object? sender, PropertyChangedEventArgs e)
	{
		if (e.PropertyName == "CanShowDialog")
		{
			OnPropertyChanged("IsExecutable");
		}
	}
}
