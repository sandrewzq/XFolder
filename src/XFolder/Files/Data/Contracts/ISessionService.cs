using Files.Data.Models;

namespace Files.Data.Contracts;

public interface ISessionService
{
	void SaveSession(SessionModel session);

	SessionModel? RestoreSession();

	void SaveWorkspace(SessionModel session, string filePath);

	SessionModel? LoadWorkspace(string filePath);
}
