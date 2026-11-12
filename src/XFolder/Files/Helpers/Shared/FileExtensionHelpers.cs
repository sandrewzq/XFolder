using System;
using System.IO;
using System.Linq;

namespace Files.Helpers.Shared;

public static class FileExtensionHelpers
{
	public static bool HasExtension(string? filePathToCheck, params string[] extensions)
	{
		if (string.IsNullOrWhiteSpace(filePathToCheck))
		{
			return false;
		}
		if (Directory.Exists(filePathToCheck))
		{
			return false;
		}
		return extensions.Any((string ext) => Path.GetExtension(filePathToCheck).Equals(ext, StringComparison.OrdinalIgnoreCase));
	}

	public static bool IsImageFile(string? fileExtensionToCheck)
	{
		return HasExtension(fileExtensionToCheck, ".png", ".bmp", ".jpg", ".jpeg", ".jfif", ".gif", ".tiff", ".tif", ".webp", ".jxr");
	}

	public static bool IsAudioFile(string? fileExtensionToCheck)
	{
		return HasExtension(fileExtensionToCheck, ".mp3", ".m4a", ".wav", ".wma", ".aac", ".adt", ".adts", ".cda", ".flac");
	}

	public static bool IsVideoFile(string? fileExtensionToCheck)
	{
		return HasExtension(fileExtensionToCheck, ".mp4", ".webm", ".ogg", ".mov", ".qt", ".m4v", ".mp4v", ".3g2", ".3gp2", ".3gp", ".3gpp", ".mkv");
	}

	public static bool IsZipFile(string? fileExtensionToCheck)
	{
		return HasExtension(fileExtensionToCheck, ".zip", ".msix", ".appx", ".msixbundle", ".appxbundle", ".7z", ".rar", ".tar", ".mcpack", ".mcworld", ".mrpack", ".jar", ".gz", ".lzh");
	}

	public static bool IsBrowsableZipFile(string? filePath, out string? ext)
	{
		if (string.IsNullOrWhiteSpace(filePath))
		{
			ext = null;
			return false;
		}
		ext = new string[8] { ".zip", ".7z", ".rar", ".tar", ".gz", ".lzh", ".mrpack", ".jar" }.FirstOrDefault((string x) => filePath.Contains(x, StringComparison.OrdinalIgnoreCase));
		return ext != null;
	}

	public static bool IsFontFile(string? fileExtensionToCheck)
	{
		return HasExtension(fileExtensionToCheck, ".fon", ".otf", ".ttc", ".ttf");
	}

	public static bool IsMediaFile(string? filePathToCheck)
	{
		return HasExtension(filePathToCheck, ".mp4", ".m4v", ".mp4v", ".3g2", ".3gp2", ".3gp", ".3gpp", ".mpg", ".mp2", ".mpeg", ".mpe", ".mpv", ".mkv", ".ogg", ".avi", ".wmv", ".mov", ".qt");
	}

	public static bool IsCertificateFile(string? filePathToCheck)
	{
		return HasExtension(filePathToCheck, ".cer", ".crt", ".der", ".pfx");
	}

	public static bool IsScriptFile(string? filePathToCheck)
	{
		return HasExtension(filePathToCheck, ".py");
	}

	public static bool IsValidXFolderWorkspaceFile(string? filePath)
	{
		if (string.IsNullOrWhiteSpace(filePath))
		{
			return false;
		}
		if (!File.Exists(filePath))
		{
			return false;
		}
		return string.Equals(Path.GetExtension(filePath), ".xfd", StringComparison.OrdinalIgnoreCase);
	}
}
