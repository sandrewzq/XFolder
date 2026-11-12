using System.ComponentModel;
using System.Threading.Tasks;
using Files.Data.Enums;

namespace Files.Data.Contracts;

public interface IDialogService
{
	Task<DialogResult> ShowDialogAsync<TViewModel>(TViewModel viewModel) where TViewModel : class, INotifyPropertyChanged;
}
