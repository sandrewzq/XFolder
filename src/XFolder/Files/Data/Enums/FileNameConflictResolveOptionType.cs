namespace Files.Data.Enums;

public enum FileNameConflictResolveOptionType : uint
{
	GenerateNewName = 0u,
	ReplaceExisting = 1u,
	Skip = 2u,
	None = 4u
}
