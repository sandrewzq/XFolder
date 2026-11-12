using System;
using Files.Data.Contracts;
using Files.Data.Enums;
using Files.UserControls.Sidebar;
using Files.ViewModels.Shells;
using Files.Views;

namespace Files.Data.Contexts;

public interface IMultiPanesContext
{
	ShellPanesView? PanesView { get; set; }

	SidebarView? SidebarView { get; set; }

	IShellView? ActivePane { get; }

	BaseShellViewModel? ActivePaneViewModel { get; }

	ShellPaneArrangement ShellPaneArrangement { get; }

	event EventHandler? ActivePaneChanging;

	event EventHandler? ActivePaneChanged;

	event EventHandler? ShellPaneArrangementChanged;

	void UpdateShellPanesView(ShellPanesView? newPage);
}
