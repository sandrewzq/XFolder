namespace Files.Helpers;

public static class StringsHelper
{
	public static string FormatFileSize(long bytes)
	{
		if (bytes == 0L)
		{
			return "0 bytes";
		}
		string[] array = new string[5] { "bytes", "KB", "MB", "GB", "TB" };
		int num = 0;
		double num2 = bytes;
		while (num2 >= 1024.0 && num < array.Length - 1)
		{
			num++;
			num2 /= 1024.0;
		}
		if (num != 0)
		{
			return $"{num2:F2} {array[num]}";
		}
		return $"{num2:F0} {array[num]}";
	}
}
