using System.Collections.Generic;
using Avalonia.Input;
using Files.Data.Items;
using Files.Views.Layouts;

namespace Files.Data.Models;

public class LayoutDragData
{
	public static LayoutDragData? ActiveDrag { get; set; }

	public required BaseLayoutView SourceLayoutView { get; set; }

	public required List<ListedItem> Items { get; set; }

	public DragDropEffects Operation { get; set; }
}
