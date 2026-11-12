using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.Json;

namespace Files.Utils.Serialization.Implementation;

internal class DefaultJsonSettingsDatabase : IJsonSettingsDatabase
{
	protected ISettingsSerializer SettingsSerializer { get; }

	protected IJsonSettingsSerializer JsonSettingsSerializer { get; }

	public DefaultJsonSettingsDatabase(ISettingsSerializer settingsSerializer, IJsonSettingsSerializer jsonSettingsSerializer)
	{
		SettingsSerializer = settingsSerializer;
		JsonSettingsSerializer = jsonSettingsSerializer;
	}

	protected IDictionary<string, object?> GetFreshSettings()
	{
		string text = SettingsSerializer.ReadFromFile();
		if (string.IsNullOrWhiteSpace(text))
		{
			text = "null";
		}
		try
		{
			return JsonSettingsSerializer.DeserializeFromJson<ConcurrentDictionary<string, object>>(text) ?? new ConcurrentDictionary<string, object>();
		}
		catch (Exception)
		{
			return JsonSettingsSerializer.DeserializeFromJson<ConcurrentDictionary<string, object>>("null") ?? new ConcurrentDictionary<string, object>();
		}
	}

	protected bool SaveSettings(IDictionary<string, object?> data)
	{
		string text = JsonSettingsSerializer.SerializeToJson(data);
		return SettingsSerializer.WriteToFile(text);
	}

	public virtual TValue? GetValue<TValue>(string key, TValue? defaultValue = default(TValue?))
	{
		if (GetFreshSettings().TryGetValue(key, out object value))
		{
			TValue valueFromObject = GetValueFromObject<TValue>(value);
			if (valueFromObject == null)
			{
				return defaultValue;
			}
			return valueFromObject;
		}
		SetValue(key, defaultValue);
		return defaultValue;
	}

	public virtual bool SetValue<TValue>(string key, TValue? newValue)
	{
		IDictionary<string, object> freshSettings = GetFreshSettings();
		if (!freshSettings.TryAdd(key, newValue))
		{
			freshSettings[key] = newValue;
		}
		return SaveSettings(freshSettings);
	}

	public virtual bool RemoveKey(string key)
	{
		IDictionary<string, object> freshSettings = GetFreshSettings();
		if (freshSettings.Remove(key))
		{
			return SaveSettings(freshSettings);
		}
		return false;
	}

	public bool FlushSettings()
	{
		return true;
	}

	public virtual bool ImportSettings(object? import)
	{
		try
		{
			IDictionary<string, object> dictionary = (IDictionary<string, object>)import;
			if (dictionary == null)
			{
				return false;
			}
			string text = JsonSettingsSerializer.SerializeToJson(dictionary);
			return SettingsSerializer.WriteToFile(text);
		}
		catch (Exception)
		{
			Debugger.Break();
			return false;
		}
	}

	public object? ExportSettings()
	{
		return GetFreshSettings();
	}

	protected static TValue? GetValueFromObject<TValue>(object? obj)
	{
		if (obj is JsonElement element)
		{
			return element.Deserialize<TValue>();
		}
		return (TValue)obj;
	}
}
