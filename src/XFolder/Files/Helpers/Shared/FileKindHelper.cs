using System;
using System.Collections.Concurrent;
using System.IO;
using Files.Services.NativePlugin;

namespace Files.Helpers.Shared;

public static class FileKindHelper
{
	private const string ApplicationKind = "Application";

	private const string DocumentKind = "Document";

	private const string FolderKind = "Folder";

	private static readonly ConcurrentDictionary<string, string> _displayKindCache = new ConcurrentDictionary<string, string>(StringComparer.OrdinalIgnoreCase);

	public static string GetDisplayKind(string? path)
	{
		if (string.IsNullOrWhiteSpace(path))
		{
			return string.Empty;
		}
		return _displayKindCache.GetOrAdd(path, (string pathValue) => ResolveDisplayKind(pathValue));
	}

	private static string ResolveDisplayKind(string path)
	{
		if (OperatingSystem.IsMacOS())
		{
			string localizedTypeDescription = MacPluginInterop.GetLocalizedTypeDescription(path);
			if (!string.IsNullOrWhiteSpace(localizedTypeDescription))
			{
				return localizedTypeDescription;
			}
		}
		if (path.EndsWith(".app", StringComparison.OrdinalIgnoreCase))
		{
			return "Application";
		}
		if (!Directory.Exists(path))
		{
			return "Document";
		}
		return "Folder";
	}

	public static bool IsMacOSApp(string filePath)
	{
		if (!string.IsNullOrWhiteSpace(filePath) && filePath.EndsWith(".app", StringComparison.OrdinalIgnoreCase))
		{
			return Directory.Exists(filePath);
		}
		return false;
	}
}
