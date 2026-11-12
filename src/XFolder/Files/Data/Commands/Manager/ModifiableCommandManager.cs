using System.Collections;
using System.Collections.Frozen;
using System.Collections.Generic;
using CommunityToolkit.Mvvm.DependencyInjection;

namespace Files.Data.Commands.Manager;

internal sealed class ModifiableCommandManager : IModifiableCommandManager, IEnumerable<IRichCommand>, IEnumerable
{
	private static readonly ICommandManager Commands = Ioc.Default.GetRequiredService<ICommandManager>();

	private readonly FrozenDictionary<CommandCodes, IRichCommand> ModifiableCommands;

	public IRichCommand this[CommandCodes code]
	{
		get
		{
			if (!ModifiableCommands.TryGetValue(code, out IRichCommand value))
			{
				return None;
			}
			return value;
		}
	}

	public IRichCommand None => ModifiableCommands[CommandCodes.None];

	public IRichCommand PasteItem => ModifiableCommands[CommandCodes.PasteItem];

	public IRichCommand DeleteItem => ModifiableCommands[CommandCodes.DeleteItem];

	public IRichCommand OpenProperties => ModifiableCommands[CommandCodes.OpenProperties];

	public ModifiableCommandManager()
	{
		ModifiableCommands = CreateModifiableCommands();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}

	public IEnumerator<IRichCommand> GetEnumerator()
	{
		return ((IEnumerable<IRichCommand>)ModifiableCommands.Values).GetEnumerator();
	}

	private static FrozenDictionary<CommandCodes, IRichCommand> CreateModifiableCommands()
	{
		return new Dictionary<CommandCodes, IRichCommand> { [CommandCodes.None] = new NoneCommand() }.ToFrozenDictionary();
	}
}
