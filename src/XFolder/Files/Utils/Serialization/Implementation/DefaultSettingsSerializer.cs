using System;
using System.IO;

namespace Files.Utils.Serialization.Implementation;

internal sealed class DefaultSettingsSerializer : ISettingsSerializer
{
	private string? _filePath;

	public bool CreateFile(string path)
	{
		try
		{
			string directoryName = Path.GetDirectoryName(path);
			if (!string.IsNullOrWhiteSpace(directoryName))
			{
				Directory.CreateDirectory(directoryName);
			}
			using (File.Open(path, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite))
			{
			}
			_filePath = path;
			return true;
		}
		catch (Exception)
		{
			return false;
		}
	}

	public string ReadFromFile()
	{
		ArgumentNullException.ThrowIfNull(_filePath, "_filePath");
		return File.ReadAllText(_filePath);
	}

	public bool WriteToFile(string? text)
	{
		ArgumentNullException.ThrowIfNull(_filePath, "_filePath");
		File.WriteAllText(_filePath, text);
		return true;
	}
}
