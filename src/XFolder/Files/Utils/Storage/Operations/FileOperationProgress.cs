namespace Files.Utils.Storage.Operations;

public sealed class FileOperationProgress
{
	public bool IsPreparing { get; init; }

	public long TotalBytes { get; init; }

	public long ProcessedBytes { get; init; }

	public long TotalItems { get; init; }

	public long ProcessedItems { get; init; }

	public string CurrentPath { get; init; } = string.Empty;
}
