namespace Files.Data.Enums;

public enum FilesystemStatusCode
{
	Success = 0,
	Generic = 1,
	Unauthorized = 2,
	NotFound = 4,
	InUse = 8,
	NameTooLong = 0x10,
	AlreadyExists = 0x20,
	NotAFolder = 0x40,
	NotAFile = 0x80,
	ReadOnly = 0x100,
	PropertyLoss = 0x200,
	InProgress = 0x400,
	FileTooLarge = 0x800
}
