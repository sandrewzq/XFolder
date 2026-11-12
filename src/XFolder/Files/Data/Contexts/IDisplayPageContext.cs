using System.ComponentModel;

namespace Files.Data.Contexts;

public interface IDisplayPageContext : INotifyPropertyChanging, INotifyPropertyChanged
{
	LayoutTypes LayoutType { get; set; }
}
