using System.Collections.Generic;
using Files.Data.Items;
using Files.ViewModels.Items;

namespace Files.Data.Messages;

public class LayoutViewSelectedItemsChanged
{
	public required List<ListedItem> selectedItems;

	public required TabItemViewModel tabItemViewModel { get; set; }
}
