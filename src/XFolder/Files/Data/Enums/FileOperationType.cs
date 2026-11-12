namespace Files.Data.Enums;

public enum FileOperationType : byte
{
	CreateNew = 0,
	Rename = 1,
	Copy = 3,
	Move = 4,
	Extract = 5,
	Recycle = 6,
	Restore = 7,
	Delete = 8,
	CreateLink = 9,
	Prepare = 10,
	Compressed = 11,
	GitClone = 12,
	InstallFont = 13,
	ImageConvert = 14,
	ImageRotate = 15
}
