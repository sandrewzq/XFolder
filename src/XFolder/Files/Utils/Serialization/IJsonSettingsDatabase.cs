namespace Files.Utils.Serialization;

internal interface IJsonSettingsDatabase
{
	TValue? GetValue<TValue>(string key, TValue? defaultValue = default(TValue?));

	bool SetValue<TValue>(string key, TValue? newValue);

	bool RemoveKey(string key);

	bool FlushSettings();

	bool ImportSettings(object? import);

	object? ExportSettings();
}
