using System.ComponentModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using Files.Data.Contracts;

namespace Files.Services.PreviewPopupProviders;

internal sealed class PreviewPopupService : ObservableObject, IPreviewPopupService, INotifyPropertyChanged
{
	public async Task<IPreviewPopupProvider?> GetProviderAsync()
	{
		if (await QuickLookProvider.Instance.DetectAvailability())
		{
			return QuickLookProvider.Instance;
		}
		return null;
	}
}
