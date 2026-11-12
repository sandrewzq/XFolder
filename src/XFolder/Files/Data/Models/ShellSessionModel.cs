using System.Collections.Generic;

namespace Files.Data.Models;

public class ShellSessionModel
{
	public List<TabSessionModel> Tabs { get; set; } = new List<TabSessionModel>();

	public int SelectedTabIndex { get; set; }
}
