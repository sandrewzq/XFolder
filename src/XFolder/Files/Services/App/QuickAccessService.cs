using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Files.Data.Contracts;
using Files.Helpers.Shared;

namespace Files.Services.App;

public class QuickAccessService : IQuickAccessService
{
	private readonly string _quickAccessFilePath;

	public QuickAccessService()
	{
		string text = Path.Combine(PathHelpers.GetAppDataDirectory(), "cache");
		Directory.CreateDirectory(text);
		_quickAccessFilePath = Path.Combine(text, "quickaccess.json");
	}

	public IEnumerable<string>? LoadQuickAccessPaths()
	{
		if (!File.Exists(_quickAccessFilePath))
		{
			return null;
		}
		try
		{
			return JsonSerializer.Deserialize<List<string>>(File.ReadAllText(_quickAccessFilePath));
		}
		catch (Exception)
		{
			return null;
		}
	}

	public IEnumerable<string> LoadCustomQuickAccessPaths()
	{
		HashSet<string> builtInPaths = GetBuiltInQuickAccessPaths();
		IEnumerable<string> enumerable = LoadQuickAccessPaths();
		if (enumerable == null)
		{
			return Enumerable.Empty<string>();
		}
		return (from path in enumerable.Select(NormalizePath)
			where !string.IsNullOrWhiteSpace(path)
			where !builtInPaths.Contains(path)
			select path).Where(Directory.Exists).Distinct(StringComparer.OrdinalIgnoreCase).ToList();
	}

	public bool IsPinnedToQuickAccess(string? path)
	{
		if (string.IsNullOrWhiteSpace(path))
		{
			return false;
		}
		string normalizedPath = NormalizePath(path);
		IEnumerable<string> enumerable = LoadQuickAccessPaths();
		if (enumerable == null)
		{
			return false;
		}
		return enumerable.Any((string savedPath) => string.Equals(NormalizePath(savedPath), normalizedPath, StringComparison.OrdinalIgnoreCase));
	}

	public void SaveQuickAccessPaths(IEnumerable<string> paths)
	{
		try
		{
			string contents = JsonSerializer.Serialize(paths);
			File.WriteAllText(_quickAccessFilePath, contents);
		}
		catch (Exception)
		{
		}
	}

	private static string NormalizePath(string path)
	{
		string text = PathHelpers.NormalizePath(path);
		if (text.Length <= 1)
		{
			return text;
		}
		return text.TrimEnd('/');
	}

	private static HashSet<string> GetBuiltInQuickAccessPaths()
	{
		string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
		HashSet<string> hashSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "/Applications" };
		if (string.IsNullOrWhiteSpace(folderPath))
		{
			return hashSet;
		}
		hashSet.Add(folderPath);
		hashSet.Add(Path.Combine(folderPath, "Desktop"));
		hashSet.Add(Path.Combine(folderPath, "Documents"));
		hashSet.Add(Path.Combine(folderPath, "Downloads"));
		hashSet.Add(Path.Combine(folderPath, "Pictures"));
		hashSet.Add(Path.Combine(folderPath, "Music"));
		hashSet.Add(Path.Combine(folderPath, "Movies"));
		return hashSet;
	}
}
