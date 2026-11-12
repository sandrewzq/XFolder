namespace Files.ViewModels.Settings;

public sealed class SettingsOptionItem<TValue>
{
	public required TValue Value { get; init; }

	public required string DisplayName { get; init; }
}
