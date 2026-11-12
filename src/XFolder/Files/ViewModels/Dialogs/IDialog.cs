using System.ComponentModel;

namespace Files.ViewModels.Dialogs;

public interface IDialog<TViewModel> where TViewModel : class, INotifyPropertyChanged
{
	TViewModel ViewModel { get; set; }
}
