namespace Files.Actions;

public interface IToggleAction : IAction
{
	bool IsOn { get; }
}
