using Files.Assets.Lang;

namespace Files.Actions.Content.ImageManipulation;

internal sealed class ConvertToBmpAction : BaseConvertImageAction
{
	public override string Label => Resources.ResourceManager.GetString("ConvertToBmp", Resources.Culture) ?? "Convert to BMP";

	public override string Description => Resources.ResourceManager.GetString("ConvertToBmpDescription", Resources.Culture) ?? "Convert selected image to BMP";

	protected override int TargetFormat => 3;

	protected override string TargetExtension => ".bmp";
}
