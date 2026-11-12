using System;

namespace Files.Utils.Storage.BaseStorageItems;

public interface IBaseStorageItem
{
	string Name { get; }

	string Path { get; }

	bool IsAvailable { get; }

	DateTimeOffset DateCreated { get; }
}
