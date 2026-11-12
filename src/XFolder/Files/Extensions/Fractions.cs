using System;
using System.Linq;

namespace Files.Extensions;

public static class Fractions
{
	public static string ToFractions(this double number, int precision = 4)
	{
		RoundToMixedFraction(number, precision, out var whole, out var numerator, out var denominator);
		string result = $"{whole}";
		if (whole > 0)
		{
			if (numerator > 0)
			{
				result = $"{whole} {numerator}/{denominator}";
			}
		}
		else if (numerator > 0)
		{
			result = $"{numerator}/{denominator}";
		}
		return result;
	}

	private static void RoundToMixedFraction(double input, int accuracy, out int whole, out int numerator, out int denominator)
	{
		double dblAccuracy = accuracy;
		whole = (int)Math.Truncate(input);
		double fraction = Math.Abs(input - (double)whole);
		if (fraction == 0.0)
		{
			numerator = 0;
			denominator = 1;
			return;
		}
		int num = Enumerable.Range(0, accuracy + 1).SkipWhile((int e) => (double)e / dblAccuracy < fraction).First();
		double num2 = (double)num / dblAccuracy;
		double num3 = (double)(num - 1) / dblAccuracy;
		if (fraction - num3 < num2 - fraction)
		{
			num--;
		}
		if (num == accuracy)
		{
			whole++;
			numerator = 0;
			denominator = 1;
		}
		else
		{
			int num4 = GCD(num, accuracy);
			numerator = num / num4;
			denominator = accuracy / num4;
		}
	}

	private static int GCD(int a, int b)
	{
		if (b != 0)
		{
			return GCD(b, a % b);
		}
		return a;
	}
}
