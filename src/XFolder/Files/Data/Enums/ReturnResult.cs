namespace Files.Data.Enums;

public enum ReturnResult : byte
{
	InProgress,
	Success,
	Failed,
	IntegrityCheckFailed,
	UnknownException,
	BadArgumentException,
	NullException,
	AccessUnauthorized,
	Cancelled,
	AlreadyExists,
	PartialSuccess
}
