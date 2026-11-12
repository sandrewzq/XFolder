using Files.Data.Enums;

namespace Files.Utils.Storage.Helpers;

public class FilesystemResult
{
	public FilesystemStatusCode ErrorCode { get; }

	public FilesystemResult(FilesystemStatusCode errorCode)
	{
		ErrorCode = errorCode;
	}

	public static implicit operator FilesystemStatusCode(FilesystemResult res)
	{
		return res.ErrorCode;
	}

	public static implicit operator FilesystemResult(FilesystemStatusCode res)
	{
		return new FilesystemResult(res);
	}

	public static implicit operator bool(FilesystemResult res)
	{
		FilesystemStatusCode? filesystemStatusCode = res?.ErrorCode;
		if (filesystemStatusCode.HasValue)
		{
			return filesystemStatusCode.GetValueOrDefault() == FilesystemStatusCode.Success;
		}
		return false;
	}

	public static explicit operator FilesystemResult(bool res)
	{
		return new FilesystemResult((!res) ? FilesystemStatusCode.Generic : FilesystemStatusCode.Success);
	}
}
public sealed class FilesystemResult<T> : FilesystemResult
{
	public T Result { get; }

	public FilesystemResult(T result, FilesystemStatusCode errorCode)
		: base(errorCode)
	{
		Result = result;
	}

	public static implicit operator T(FilesystemResult<T> res)
	{
		return res.Result;
	}
}
