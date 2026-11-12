using System.ComponentModel;
using System.Threading.Tasks;
using Files.ViewModels.Items;
using Files.ViewModels.Shells;

namespace Files.Data.Contexts;

public interface IMultitaskingContext : INotifyPropertyChanged
{
	int TabCount { get; }

	int SelectedTabIndex { get; }

	TabItemViewModel SelectedTabViewModel { get; }

	int RecentlyClosedTabCount { get; }

	bool IsRestoringClosedTab { get; }

	void PushRecentlyClosedTab(TabItemViewModel tabItemViewModel, BaseShellViewModel parentShellViewModel, int tabIndex);

	Task<bool> ReopenClosedTabAsync();
}
