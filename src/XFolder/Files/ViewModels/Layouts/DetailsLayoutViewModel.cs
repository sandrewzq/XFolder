using System.Collections.Generic;
using Files.ViewModels.Items;

namespace Files.ViewModels.Layouts;

public class DetailsLayoutViewModel : BaseLayoutViewModel
{
	public DetailsLayoutViewModel(string navPath, TabItemViewModel tabItemViewModel, IEnumerable<string>? selectItems = null)
		: base(navPath, tabItemViewModel, selectItems)
	{
	}
}
