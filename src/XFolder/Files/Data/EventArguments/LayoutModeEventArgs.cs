using Files.Data.Enums;

namespace Files.Data.EventArguments;

public sealed class LayoutModeEventArgs
{
	public readonly FolderLayoutModes LayoutMode;

	internal LayoutModeEventArgs(FolderLayoutModes layoutMode)
	{
		LayoutMode = layoutMode;
	}
}
