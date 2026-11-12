using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Files.Helpers;

public sealed class TypesConverter : JsonConverter<Type>
{
	public override Type Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		return typeToConvert;
	}

	public override void Write(Utf8JsonWriter writer, Type value, JsonSerializerOptions options)
	{
		writer.WriteStringValue(value.FullName);
	}
}
