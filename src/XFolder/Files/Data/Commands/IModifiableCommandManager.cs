using System.Collections;
using System.Collections.Generic;
using Files.Data.Commands.Manager;

namespace Files.Data.Commands;

public interface IModifiableCommandManager : IEnumerable<IRichCommand>, IEnumerable
{
	IRichCommand this[CommandCodes code] { get; }

	IRichCommand None { get; }

	IRichCommand PasteItem { get; }

	IRichCommand DeleteItem { get; }

	IRichCommand OpenProperties { get; }
}
