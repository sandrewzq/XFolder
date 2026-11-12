using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace Files.Extensions;

public static class ComponentModelExtensions
{
	public static string GetDescription<T>(this T enumValue) where T : Enum
	{
		string result = enumValue.ToString();
		FieldInfo field = enumValue.GetType().GetField(enumValue.ToString());
		if ((object)field != null)
		{
			object[] customAttributes = field.GetCustomAttributes(typeof(DescriptionAttribute), inherit: true);
			if (customAttributes != null && customAttributes.Length != 0)
			{
				result = ((DescriptionAttribute)customAttributes[0]).Description;
			}
		}
		return result;
	}

	public static T? GetValueFromDescription<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] T>(string description) where T : Enum
	{
		FieldInfo[] fields = typeof(T).GetFields();
		foreach (FieldInfo fieldInfo in fields)
		{
			if (Attribute.GetCustomAttribute(fieldInfo, typeof(DescriptionAttribute)) is DescriptionAttribute descriptionAttribute)
			{
				if (descriptionAttribute.Description == description)
				{
					return (T)fieldInfo.GetValue(null);
				}
			}
			else if (fieldInfo.Name == description)
			{
				return (T)fieldInfo.GetValue(null);
			}
		}
		return default;
	}
}
