using Files.Assets.Lang;
using Files.Extensions;

namespace Files.Actions.Content.ImageManipulation;

internal sealed class RotateLeftAction : BaseRotateAction
{
	public override string Label => Resources.RotateLeft;

	public override string Description => Resources.RotateLeftDescription.GetLocalizedFormatResource(context.SelectedItems.Count);

	protected override bool RotateClockwise => false;
}
