using System;
using System.Collections.Generic;
using Files.Data.Items;

namespace Files.Views.Layouts;

public interface IBaseLayoutView : IDisposable
{
	bool IsRenamingItem { get; }

	bool IsItemSelected { get; }

	List<ListedItem> SelectedItems { get; }

	ListedItem? SelectedItem { get; }
}
