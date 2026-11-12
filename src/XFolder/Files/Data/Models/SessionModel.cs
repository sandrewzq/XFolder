using System.Collections.Generic;
using Files.Data.Enums;

namespace Files.Data.Models;

public class SessionModel
{
	public ShellPaneArrangement Arrangement { get; set; }

	public bool SidebarIsOpen { get; set; } = true;

	public double SidebarWidth { get; set; } = 220.0;

	public Dictionary<string, PaneLayoutStateModel> PaneLayoutStates { get; set; } = new Dictionary<string, PaneLayoutStateModel>();

	public List<ShellSessionModel> Shells { get; set; } = new List<ShellSessionModel>();
}
