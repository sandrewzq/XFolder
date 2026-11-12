using Files.Assets.Lang;

namespace Files.Actions.Content.ImageManipulation;

internal sealed class ConvertToPngAction : BaseConvertImageAction
{
	public override string Label => Resources.ResourceManager.GetString("ConvertToPng", Resources.Culture) ?? "Convert to PNG";

	public override string Description => Resources.ResourceManager.GetString("ConvertToPngDescription", Resources.Culture) ?? "Convert selected image to PNG";

	protected override int TargetFormat => 1;

	protected override string TargetExtension => ".png";
}
