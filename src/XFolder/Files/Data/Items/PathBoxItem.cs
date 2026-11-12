using System;

namespace Files.Data.Items;

public sealed class PathBoxItem
{
	public string? Title { get; set; }

	public string? Path { get; set; }

	public string? ChevronToolTip { get; set; }

	public bool IsLastItem { get; set; }

	public bool IsRootPath => string.Equals(Path, "/", StringComparison.Ordinal);
}
