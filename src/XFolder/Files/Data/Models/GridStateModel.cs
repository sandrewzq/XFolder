using System.Collections.Generic;

namespace Files.Data.Models;

public class GridStateModel
{
	public List<GridLengthStateModel> Columns { get; set; } = new List<GridLengthStateModel>();

	public List<GridLengthStateModel> Rows { get; set; } = new List<GridLengthStateModel>();
}
