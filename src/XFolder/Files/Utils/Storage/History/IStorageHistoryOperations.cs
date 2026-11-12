using System;
using System.Threading.Tasks;
using Files.Data.Enums;

namespace Files.Utils.Storage.History;

public interface IStorageHistoryOperations : IDisposable
{
	Task<ReturnResult> Undo(IStorageHistory history);

	Task<ReturnResult> Redo(IStorageHistory history);
}
