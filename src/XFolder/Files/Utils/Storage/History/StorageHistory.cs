using System.Collections.Generic;
using Files.Data.Enums;
using Files.Extensions;
using Files.Utils.Storage.Helpers;

namespace Files.Utils.Storage.History;

public sealed class StorageHistory : IStorageHistory
{
	public FileOperationType OperationType { get; private set; }

	public IList<IStorageItemWithPath> Source { get; private set; }

	public IList<IStorageItemWithPath> Destination { get; private set; }

	public StorageHistory(FileOperationType operationType, IStorageItemWithPath source, IStorageItemWithPath destination)
		: this(operationType, source.CreateList(), destination.CreateList())
	{
	}

	public StorageHistory(FileOperationType operationType, IList<IStorageItemWithPath> source, IList<IStorageItemWithPath> destination)
	{
		OperationType = operationType;
		Source = source;
		Destination = destination;
	}

	public void Modify(IStorageHistory newHistory)
	{
		FileOperationType operationType = newHistory.OperationType;
		IList<IStorageItemWithPath> source = newHistory.Source;
		IList<IStorageItemWithPath> destination = newHistory.Destination;
		OperationType = operationType;
		Source = source;
		Destination = destination;
	}

	public void Modify(FileOperationType operationType, IStorageItemWithPath source, IStorageItemWithPath destination)
	{
		FileOperationType operationType2 = operationType;
		IList<IStorageItemWithPath> source2 = source.CreateList();
		IList<IStorageItemWithPath> destination2 = destination.CreateList();
		OperationType = operationType2;
		Source = source2;
		Destination = destination2;
	}

	public void Modify(FileOperationType operationType, IList<IStorageItemWithPath> source, IList<IStorageItemWithPath> destination)
	{
		FileOperationType operationType2 = operationType;
		OperationType = operationType2;
		Source = source;
		Destination = destination;
	}
}
