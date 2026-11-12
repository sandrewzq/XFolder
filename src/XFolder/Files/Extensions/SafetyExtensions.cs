using System;

namespace Files.Extensions;

public class SafetyExtensions
{
	public static bool IgnoreExceptions(Action action)
	{
		try
		{
			action();
			return true;
		}
		catch (Exception)
		{
			return false;
		}
	}
}
