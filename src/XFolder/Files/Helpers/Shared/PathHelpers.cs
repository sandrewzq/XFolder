using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Files.Extensions;

namespace Files.Helpers.Shared;

public static class PathHelpers
{
	public static bool IsHiddenDirectory(DirectoryInfo directory)
	{
		try
		{
			return directory.Name.StartsWith('.') || (directory.Attributes & FileAttributes.Hidden) == FileAttributes.Hidden;
		}
		catch
		{
			return false;
		}
	}

	public static bool IsHiddenFile(FileInfo fileInfo)
	{
		try
		{
			return fileInfo.Name.StartsWith('.') || (fileInfo.Attributes & FileAttributes.Hidden) == FileAttributes.Hidden;
		}
		catch
		{
			return false;
		}
	}

	public static string GetAppDataDirectory()
	{
		string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
		if (!string.IsNullOrWhiteSpace(folderPath))
		{
			return Path.Combine(folderPath, "com.zebrapixel.XFolder");
		}
		if (OperatingSystem.IsMacOS())
		{
			string folderPath2 = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
			if (!string.IsNullOrWhiteSpace(folderPath2))
			{
				return Path.Combine(folderPath2, "Library", "Application Support", "com.zebrapixel.XFolder");
			}
		}
		return AppContext.BaseDirectory;
	}

	public static string[] GetPathSegments(string path)
	{
		path = path.TrimEnd('/');
		if (path == string.Empty)
		{
			return new string[1] { "/" };
		}
		List<string> list = new List<string> { "/" };
		string[] array = path.Split('/', StringSplitOptions.RemoveEmptyEntries);
		string text = string.Empty;
		string[] array2 = array;
		foreach (string text2 in array2)
		{
			text = text + "/" + text2;
			list.Add(text);
		}
		return list.ToArray();
	}

	public static string NormalizePath(string path)
	{
		if (string.IsNullOrEmpty(path))
		{
			return path;
		}
		path = path.Trim();
		if (path == "~")
		{
			path = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
		}
		else if (path.StartsWith("~/"))
		{
			path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), path.Substring(2));
		}
		return path;
	}

	public static string NormalizeDirectoryPath(string? path)
	{
		if (string.IsNullOrWhiteSpace(path))
		{
			return string.Empty;
		}
		string text = NormalizePath(path);
		if (text.Length <= 1)
		{
			return text;
		}
		return text.TrimEnd('/');
	}

	public static bool IsCanAccessFolder(string path)
	{
		bool result = false;
		if (Directory.Exists(path))
		{
			try
			{
				Directory.EnumerateFileSystemEntries(path).FirstOrDefault();
				result = true;
			}
			catch (Exception)
			{
			}
		}
		return result;
	}

	public static string GetNearestValidParentDirectory(string path)
	{
		if (string.IsNullOrEmpty(path))
		{
			return path;
		}
		path = NormalizePath(path);
		string[] pathSegments = GetPathSegments(path);
		for (int num = pathSegments.Length - 2; num >= 0; num--)
		{
			string text = pathSegments[num];
			if (Directory.Exists(text))
			{
				return text;
			}
		}
		return pathSegments[0];
	}

	public static bool IsSubPathOf(string path, string baseDirPath)
	{
		string text = NormalizePath(path).WithEnding("/");
		string value = NormalizePath(baseDirPath).WithEnding("/");
		return text.StartsWith(value);
	}

	public static bool IsStrictSubPathOf(string? path, string? baseDirPath)
	{
		string text = NormalizeDirectoryPath(path);
		string text2 = NormalizeDirectoryPath(baseDirPath);
		if (string.IsNullOrWhiteSpace(text) || string.IsNullOrWhiteSpace(text2) || string.Equals(text, text2, StringComparison.Ordinal))
		{
			return false;
		}
		if (text2 == "/")
		{
			return text.StartsWith("/", StringComparison.Ordinal);
		}
		return text.StartsWith(text2.WithEnding("/"), StringComparison.Ordinal);
	}

	public static string? GetFirstChildPathUnderParent(string? descendantPath, string? parentPath)
	{
		string text = NormalizeDirectoryPath(descendantPath);
		string text2 = NormalizeDirectoryPath(parentPath);
		if (!IsStrictSubPathOf(text, text2))
		{
			return null;
		}
		string text3 = ((text2 == "/") ? text.TrimStart('/') : text.Substring(text2.Length).TrimStart('/')).Split('/', StringSplitOptions.RemoveEmptyEntries).FirstOrDefault();
		if (string.IsNullOrWhiteSpace(text3))
		{
			return null;
		}
		if (!(text2 == "/"))
		{
			return text2 + "/" + text3;
		}
		return "/" + text3;
	}

	public static bool IsPathInCurrentPathChain(string path, string currentPath)
	{
		if (string.IsNullOrWhiteSpace(path) || string.IsNullOrWhiteSpace(currentPath))
		{
			return false;
		}
		string text = NormalizePath(path);
		string text2 = NormalizePath(currentPath);
		text = ((text.Length > 1) ? text.TrimEnd('/') : text);
		text2 = ((text2.Length > 1) ? text2.TrimEnd('/') : text2);
		if (!string.Equals(text, text2, StringComparison.Ordinal))
		{
			return text2.StartsWith(text.WithEnding("/"), StringComparison.Ordinal);
		}
		return true;
	}

	public static bool IsRootPath(string path)
	{
		return path == "/";
	}

	public static string GetParentPath(string path)
	{
		if (string.IsNullOrEmpty(path) || IsRootPath(path))
		{
			return path;
		}
		path = NormalizePath(path).TrimEnd('/');
		int num = path.LastIndexOf('/');
		if (num <= 0)
		{
			return "/";
		}
		return path.Substring(0, (num == 0) ? 1 : num);
	}
}
