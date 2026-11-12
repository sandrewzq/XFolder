using System.Collections.Generic;

namespace Files.Data.Contracts;

public interface IQuickAccessService
{
	IEnumerable<string>? LoadQuickAccessPaths();

	IEnumerable<string> LoadCustomQuickAccessPaths();

	bool IsPinnedToQuickAccess(string? path);

	void SaveQuickAccessPaths(IEnumerable<string> paths);
}
