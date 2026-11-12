using System.Collections;
using System.Collections.Generic;
using Files.Data.Commands.HotKey;

namespace Files.Data.Commands.Manager;

public interface ICommandManager : IEnumerable<IRichCommand>, IEnumerable
{
	IRichCommand this[CommandCodes code] { get; }

	IRichCommand this[string code] { get; }

	IRichCommand this[Files.Data.Commands.HotKey.HotKey customHotKey] { get; }

	IRichCommand None { get; }

	IRichCommand OpenTerminal { get; }
}
