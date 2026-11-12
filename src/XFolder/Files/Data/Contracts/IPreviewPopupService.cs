using System.ComponentModel;
using System.Threading.Tasks;

namespace Files.Data.Contracts;

public interface IPreviewPopupService : INotifyPropertyChanged
{
	Task<IPreviewPopupProvider?> GetProviderAsync();
}
