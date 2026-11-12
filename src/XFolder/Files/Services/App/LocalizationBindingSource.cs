using System;
using CommunityToolkit.Mvvm.ComponentModel;
using Files.Assets.Lang;
using Files.Helpers.Application;

namespace Files.Services.App;

public sealed class LocalizationBindingSource : ObservableObject
{
	private bool _initialized;

	public static LocalizationBindingSource Instance { get; } = new LocalizationBindingSource();

	public string this[string resourceKey] => Resources.ResourceManager.GetString(resourceKey, Resources.Culture) ?? resourceKey;

	private LocalizationBindingSource()
	{
	}

	public void Initialize()
	{
		if (!_initialized)
		{
			AppLanguageHelper.LanguageChanged += OnLanguageChanged;
			_initialized = true;
		}
	}

	private void OnLanguageChanged(object? sender, EventArgs e)
	{
		OnPropertyChanged(string.Empty);
	}
}
