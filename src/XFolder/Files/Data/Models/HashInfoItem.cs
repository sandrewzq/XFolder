using CommunityToolkit.Mvvm.ComponentModel;

namespace Files.Data.Models;

public sealed class HashInfoItem : ObservableObject
{
	private const int MaxVisibleHashLength = 65;

	private const string MiddleEllipsis = "..";

	private string algorithm = string.Empty;

	private string hashValue = string.Empty;

	private bool isCalculating;

	private bool isCalculated;

	public string Algorithm
	{
		get
		{
			return algorithm;
		}
		set
		{
			SetProperty(ref algorithm, value, "Algorithm");
		}
	}

	public string HashValue
	{
		get
		{
			return hashValue;
		}
		set
		{
			if (SetProperty(ref hashValue, value, "HashValue"))
			{
				OnPropertyChanged("DisplayHashValue");
			}
		}
	}

	public string DisplayHashValue => GetDisplayHashValue(HashValue);

	public bool IsCalculating
	{
		get
		{
			return isCalculating;
		}
		set
		{
			SetProperty(ref isCalculating, value, "IsCalculating");
		}
	}

	public bool IsCalculated
	{
		get
		{
			return isCalculated;
		}
		set
		{
			SetProperty(ref isCalculated, value, "IsCalculated");
		}
	}

	private static string GetDisplayHashValue(string value)
	{
		if (string.IsNullOrEmpty(value) || value.Length <= 65)
		{
			return value;
		}
		int num = 65 - "..".Length;
		int num2 = (num + 1) / 2;
		int num3 = num - num2;
		string text = value.Substring(0, num2);
		int num4 = num3;
		int length = value.Length;
		int num5 = length - num4;
		return text + ".." + value.Substring(num5, length - num5);
	}
}
