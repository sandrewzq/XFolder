namespace Files.Data.Messages;

public class FileSystemChangedMessage
{
	public required string Path { get; set; }

	public FileSystemChangeType ChangeType { get; set; }

	public string? OldPath { get; set; }
}
