using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using Files.Assets.Lang;
using Files.Helpers;
using Files.Helpers.Shared;

namespace Files.Services.App;

public class UpdateService
{
	private readonly string _updateCacheFilePath;

	private readonly HttpClient _httpClient;

	public UpdateService()
	{
		string text = Path.Combine(PathHelpers.GetAppDataDirectory(), "cache");
		if (!Directory.Exists(text))
		{
			Directory.CreateDirectory(text);
		}
		_updateCacheFilePath = Path.Combine(text, "ignore_version.json");
		_httpClient = new HttpClient();
		_httpClient.Timeout = TimeSpan.FromSeconds(10L);
	}

	public async Task CheckForUpdatesAsync()
	{
		_ = 1;
		try
		{
			string text = "https://gist.githubusercontent.com/zebrapixel/37299fa225b2645a8a146654245fa7b6/raw/xfolder_version.config";
			string value = (text.Contains('?') ? "&" : "?");
			string requestUri = $"{text}{value}t={DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()}";
			string latestVersionStr = (await _httpClient.GetStringAsync(requestUri))?.Trim();
			if (string.IsNullOrEmpty(latestVersionStr))
			{
				return;
			}
			string text2 = Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyFileVersionAttribute>()?.Version;
			if (string.IsNullOrEmpty(text2) || !Version.TryParse(latestVersionStr, out Version result) || !Version.TryParse(text2, out Version result2) || !(result > result2))
			{
				return;
			}
			List<string> ignoredVersions = new List<string>();
			if (File.Exists(_updateCacheFilePath))
			{
				string text3 = File.ReadAllText(_updateCacheFilePath)?.Trim();
				if (!string.IsNullOrEmpty(text3))
				{
					try
					{
						ignoredVersions = JsonSerializer.Deserialize<List<string>>(text3) ?? new List<string>();
					}
					catch (JsonException)
					{
					}
				}
				if (ignoredVersions.Contains(latestVersionStr))
				{
					return;
				}
			}
			if (await DialogDisplayHelper.ShowAskDialogAsync(Resources.ConsentDialogTitle, string.Format(Resources.NewVersionAvailablePrompt, latestVersionStr), Resources.OK, Resources.Dismiss))
			{
				Process.Start(new ProcessStartInfo
				{
					FileName = "https://github.com/zebrapixel/XFolder/releases/latest",
					UseShellExecute = true
				});
			}
			ignoredVersions.Add(latestVersionStr);
			string contents = JsonSerializer.Serialize(ignoredVersions);
			File.WriteAllText(_updateCacheFilePath, contents);
		}
		catch (Exception)
		{
		}
	}
}
