using System.Collections.Generic;

namespace Files.Data.Models;

public class PaneLayoutStateModel
{
	public Dictionary<string, GridStateModel> Grids { get; set; } = new Dictionary<string, GridStateModel>();
}
