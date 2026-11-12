using System;
using System.Runtime.CompilerServices;
using Files.Data.EventArguments;

namespace Files.Utils.Serialization;

internal abstract class BaseJsonSettings : ISettingsSharingContext
{
	private ISettingsSharingContext? _settingsSharingContext;

	private ISettingsSerializer? _SettingsSerializer;

	private IJsonSettingsSerializer? _JsonSettingsSerializer;

	private IJsonSettingsDatabase? _JsonSettingsDatabase;

	public bool IsAvailable { get; protected set; }

	protected ISettingsSerializer? SettingsSerializer
	{
		get
		{
			return _settingsSharingContext?.Instance?.SettingsSerializer ?? _SettingsSerializer;
		}
		set
		{
			_SettingsSerializer = value;
		}
	}

	protected IJsonSettingsSerializer? JsonSettingsSerializer
	{
		get
		{
			return _settingsSharingContext?.Instance?.JsonSettingsSerializer ?? _JsonSettingsSerializer;
		}
		set
		{
			_JsonSettingsSerializer = value;
		}
	}

	protected IJsonSettingsDatabase? JsonSettingsDatabase
	{
		get
		{
			return _settingsSharingContext?.Instance?.JsonSettingsDatabase ?? _JsonSettingsDatabase;
		}
		set
		{
			_JsonSettingsDatabase = value;
		}
	}

	BaseJsonSettings ISettingsSharingContext.Instance => this;

	public event EventHandler<SettingChangedEventArgs>? OnSettingChangedEvent;

	public virtual bool FlushSettings()
	{
		return JsonSettingsDatabase?.FlushSettings() ?? false;
	}

	public virtual object ExportSettings()
	{
		return JsonSettingsDatabase?.ExportSettings() ?? ((object)false);
	}

	public virtual bool ImportSettings(object import)
	{
		return JsonSettingsDatabase?.ImportSettings(import) ?? false;
	}

	public bool RegisterSettingsContext(ISettingsSharingContext settingsSharingContext)
	{
		if (_settingsSharingContext == null)
		{
			_settingsSharingContext = settingsSharingContext;
			IsAvailable = settingsSharingContext.Instance.IsAvailable;
			return true;
		}
		return false;
	}

	public ISettingsSharingContext GetSharingContext()
	{
		return _settingsSharingContext ?? this;
	}

	protected virtual void Initialize(string filePath)
	{
		IsAvailable = SettingsSerializer?.CreateFile(filePath) ?? false;
	}

	protected virtual TValue? Get<TValue>(TValue? defaultValue, [CallerMemberName] string propertyName = "")
	{
		if (string.IsNullOrEmpty(propertyName))
		{
			return defaultValue;
		}
		if (JsonSettingsDatabase != null)
		{
			TValue value = JsonSettingsDatabase.GetValue(propertyName, defaultValue);
			if (value == null)
			{
				return defaultValue;
			}
			return value;
		}
		return defaultValue;
	}

	protected virtual bool Set<TValue>(TValue? value, [CallerMemberName] string propertyName = "")
	{
		if (string.IsNullOrEmpty(propertyName))
		{
			return false;
		}
		if (JsonSettingsDatabase != null)
		{
			TValue value2 = JsonSettingsDatabase.GetValue<TValue>(propertyName);
			if ((value2 == null || !value2.Equals(value)) && JsonSettingsDatabase.SetValue(propertyName, value))
			{
				RaiseOnSettingChangedEvent(this, new SettingChangedEventArgs(propertyName, value));
				return true;
			}
		}
		return false;
	}

	protected virtual void RaiseOnSettingChangedEvent(object sender, SettingChangedEventArgs e)
	{
		OnSettingChangedEvent?.Invoke(sender, e);
		_settingsSharingContext?.Instance.RaiseOnSettingChangedEvent(sender, e);
	}
}
