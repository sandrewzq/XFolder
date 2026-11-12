using Files.Data.Enums;

namespace Files.Data.Models;

public class TabSessionModel
{
	public string NavPath { get; set; } = "";

	public FolderLayoutModes FolderLayoutMode { get; set; }

	public SortOption CurrentSortOption { get; set; }

	public SortDirection CurrentSortDirection { get; set; }

	public bool HasCustomSort { get; set; }
}
