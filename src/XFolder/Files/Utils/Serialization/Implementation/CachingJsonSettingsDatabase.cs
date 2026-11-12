using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Files.Utils.Serialization.Implementation;

internal sealed class CachingJsonSettingsDatabase : DefaultJsonSettingsDatabase
{
	private IDictionary<string, object?>? _settingsCache;

	private readonly object _cacheGate = new();

	public CachingJsonSettingsDatabase(ISettingsSerializer settingsSerializer, IJsonSettingsSerializer jsonSettingsSerializer)
		: base(settingsSerializer, jsonSettingsSerializer)
	{
	}

	public override TValue? GetValue<TValue>(string key, TValue? defaultValue = default(TValue?)) where TValue : default
	{
		lock (_cacheGate)
		{
			_settingsCache ??= GetFreshSettings();
			if (_settingsCache.TryGetValue(key, out object? value))
			{
				TValue? valueFromObject = GetValueFromObject<TValue>(value);
				if (valueFromObject == null)
				{
					return defaultValue;
				}
				return valueFromObject;
			}
			if (base.SetValue(key, defaultValue))
			{
				_settingsCache[key] = defaultValue;
			}
			return defaultValue;
		}
	}

	public override bool SetValue<TValue>(string key, TValue? newValue) where TValue : default
	{
		lock (_cacheGate)
		{
			_settingsCache ??= GetFreshSettings();
			if (!_settingsCache.TryGetValue(key, out object? value))
			{
				_settingsCache[key] = newValue;
				return SaveSettings(_settingsCache);
			}
			bool hasChanged = (!(newValue is IEnumerable source) || !(value is IEnumerable source2))
				? value != (object?)newValue
				: !source2.Cast<object>().SequenceEqual(source.Cast<object>());
			if (hasChanged)
			{
				_settingsCache[key] = newValue;
				return SaveSettings(_settingsCache);
			}
			return false;
		}
	}

	public override bool RemoveKey(string key)
	{
		lock (_cacheGate)
		{
			_settingsCache ??= GetFreshSettings();
			if (_settingsCache.Remove(key))
			{
				return SaveSettings(_settingsCache);
			}
			return false;
		}
	}

	public override bool ImportSettings(object? import)
	{
		lock (_cacheGate)
		{
			if (base.ImportSettings(import))
			{
				_settingsCache = GetFreshSettings();
				return true;
			}
			return false;
		}
	}
}
