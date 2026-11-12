using System.Collections.Generic;
using Files.ViewModels.Items;

namespace Files.ViewModels.Layouts;

public class GridLayoutViewModel : BaseLayoutViewModel
{
	public GridLayoutViewModel(string navPath, TabItemViewModel tabItemViewModel, IEnumerable<string>? selectItems = null)
		: base(navPath, tabItemViewModel, selectItems)
	{
	}
}
