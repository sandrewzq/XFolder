using System;

namespace Files.Data.Items;

public sealed class ShellNewEntry
{
	public string Extension { get; set; } = string.Empty;

	public string Name { get; set; } = string.Empty;

	public string Command { get; set; } = string.Empty;

	public string IconBase64 { get; set; } = string.Empty;

	public byte[] Data { get; set; } = Array.Empty<byte>();

	public string Template { get; set; } = string.Empty;
}
