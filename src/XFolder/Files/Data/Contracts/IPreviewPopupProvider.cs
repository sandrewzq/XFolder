using System.Threading.Tasks;

namespace Files.Data.Contracts;

public interface IPreviewPopupProvider
{
	Task TogglePreviewPopupAsync(string path);

	Task SwitchPreviewAsync(string path);

	Task<bool> DetectAvailability();
}
