using Files.Assets.Lang;
using Files.Extensions;

namespace Files.Actions.Content.ImageManipulation;

internal sealed class RotateRightAction : BaseRotateAction
{
	public override string Label => Resources.RotateRight;

	public override string Description => Resources.RotateRightDescription.GetLocalizedFormatResource(context.SelectedItems.Count);

	protected override bool RotateClockwise => true;
}
