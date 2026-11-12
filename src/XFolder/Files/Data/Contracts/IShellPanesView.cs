using System;
using System.ComponentModel;
using Files.Data.Enums;
using Files.ViewModels.Shells;

namespace Files.Data.Contracts;

public interface IShellPanesView : IDisposable, INotifyPropertyChanged
{
	IShellView? ActivePane { get; }

	BaseShellViewModel? ActivePaneViewModel { get; }

	ShellPaneArrangement ShellPaneArrangement { get; }

	void ArrangePanes(ShellPaneArrangement arrangement = ShellPaneArrangement.Single);

	void ActivePrevPane();

	void ActiveNextPane();
}
