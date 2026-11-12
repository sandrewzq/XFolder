using System;
using System.IO;
using System.Text.Json;
using Files.Data.Contracts;
using Files.Data.Models;
using Files.Helpers.Shared;

namespace Files.Services.App;

public class SessionService : ISessionService
{
	private readonly string _sessionFilePath;

	public SessionService()
	{
		string text = Path.Combine(PathHelpers.GetAppDataDirectory(), "cache");
		Directory.CreateDirectory(text);
		_sessionFilePath = Path.Combine(text, "session.json");
	}

	public void SaveSession(SessionModel session)
	{
		try
		{
			string contents = JsonSerializer.Serialize(session);
			File.WriteAllText(_sessionFilePath, contents);
		}
		catch (Exception)
		{
		}
	}

	public SessionModel? RestoreSession()
	{
		if (!File.Exists(_sessionFilePath))
		{
			return null;
		}
		try
		{
			SessionModel sessionModel = JsonSerializer.Deserialize<SessionModel>(File.ReadAllText(_sessionFilePath));
			if (sessionModel != null)
			{
				foreach (ShellSessionModel shell in sessionModel.Shells)
				{
					foreach (TabSessionModel tab in shell.Tabs)
					{
						if (!string.IsNullOrEmpty(tab.NavPath) && !Directory.Exists(tab.NavPath))
						{
							tab.NavPath = PathHelpers.GetNearestValidParentDirectory(tab.NavPath);
						}
					}
				}
			}
			return sessionModel;
		}
		catch (Exception)
		{
			return null;
		}
	}

	public void SaveWorkspace(SessionModel session, string filePath)
	{
		try
		{
			string contents = JsonSerializer.Serialize(session);
			File.WriteAllText(filePath, contents);
		}
		catch (Exception innerException)
		{
			throw new IOException("Failed to save workspace to " + filePath, innerException);
		}
	}

	public SessionModel? LoadWorkspace(string filePath)
	{
		if (!File.Exists(filePath))
		{
			return null;
		}
		try
		{
			SessionModel sessionModel = JsonSerializer.Deserialize<SessionModel>(File.ReadAllText(filePath));
			if (sessionModel != null)
			{
				foreach (ShellSessionModel shell in sessionModel.Shells)
				{
					foreach (TabSessionModel tab in shell.Tabs)
					{
						if (!string.IsNullOrEmpty(tab.NavPath) && !Directory.Exists(tab.NavPath))
						{
							tab.NavPath = PathHelpers.GetNearestValidParentDirectory(tab.NavPath);
						}
					}
				}
			}
			return sessionModel;
		}
		catch (Exception innerException)
		{
			throw new IOException("Failed to load workspace from " + filePath, innerException);
		}
	}
}
