using System;
using System.Runtime.InteropServices.ComTypes;

namespace Files.Extensions;

public static class DateExtensions
{
	public static DateTime ToDateTime(this FILETIME time)
	{
		uint dwLowDateTime = (uint)time.dwLowDateTime;
		long fileTime = ((long)time.dwHighDateTime << 32) + dwLowDateTime;
		try
		{
			return DateTime.FromFileTimeUtc(fileTime);
		}
		catch
		{
			return DateTime.FromFileTimeUtc(4294967295L);
		}
	}
}
