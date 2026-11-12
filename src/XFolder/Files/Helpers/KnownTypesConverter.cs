using System.Text.Json;

namespace Files.Helpers;

public sealed class KnownTypesConverter
{
	public JsonSerializerOptions Options { get; } = new JsonSerializerOptions();

	public KnownTypesConverter()
	{
		Options.Converters.Add(new TypesConverter());
	}
}
