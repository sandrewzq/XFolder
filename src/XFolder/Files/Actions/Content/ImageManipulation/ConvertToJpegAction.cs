using Files.Assets.Lang;

namespace Files.Actions.Content.ImageManipulation;

internal sealed class ConvertToJpegAction : BaseConvertImageAction
{
	public override string Label => Resources.ResourceManager.GetString("ConvertToJpeg", Resources.Culture) ?? "Convert to JPEG";

	public override string Description => Resources.ResourceManager.GetString("ConvertToJpegDescription", Resources.Culture) ?? "Convert selected image to JPEG";

	protected override int TargetFormat => 2;

	protected override string TargetExtension => ".jpg";

	protected override string[] TargetEquivalentExtensions => new string[3] { ".jpg", ".jpeg", ".jfif" };
}
