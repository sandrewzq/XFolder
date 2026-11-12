using System.Collections.Generic;
using Files.Data.Enums;
using Files.Utils.Storage.Helpers;

namespace Files.Utils.Storage.History;

public interface IStorageHistory
{
	FileOperationType OperationType { get; }

	IList<IStorageItemWithPath> Source { get; }

	IList<IStorageItemWithPath> Destination { get; }

	void Modify(IStorageHistory newHistory);

	void Modify(FileOperationType operationType, IStorageItemWithPath source, IStorageItemWithPath destination);

	void Modify(FileOperationType operationType, IList<IStorageItemWithPath> source, IList<IStorageItemWithPath> destination);
}
