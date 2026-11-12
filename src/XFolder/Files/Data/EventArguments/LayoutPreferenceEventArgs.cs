using Files.Helpers.Layout;

namespace Files.Data.EventArguments;

public sealed class LayoutPreferenceEventArgs
{
	public readonly LayoutPreferencesItem LayoutPreference;

	internal LayoutPreferenceEventArgs(LayoutPreferencesItem layoutPref)
	{
		LayoutPreference = layoutPref;
	}
}
