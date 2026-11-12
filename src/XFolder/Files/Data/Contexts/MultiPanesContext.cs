using System;
using System.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using Files.Data.Contracts;
using Files.Data.Enums;
using Files.Data.Enums.Navigation;
using Files.Data.Messages;
using Files.Helpers.Shared;
using Files.UserControls.Sidebar;
using Files.ViewModels.Shells;
using Files.Views;

namespace Files.Data.Contexts;

internal sealed class MultiPanesContext : IMultiPanesContext, IDisposable, IRecipient<FileSystemChangedMessage>
{
	private IShellView? _ActivePane;

	private BaseShellViewModel? _ActivePaneViewModel;

	private ShellPaneArrangement _ShellPaneArrangement;

	public ShellPanesView? PanesView { get; set; }

	public SidebarView? SidebarView { get; set; }

	public IShellView? ActivePane => _ActivePane;

	public BaseShellViewModel? ActivePaneViewModel => _ActivePaneViewModel;

	public ShellPaneArrangement ShellPaneArrangement => _ShellPaneArrangement;

	public event EventHandler? ActivePaneChanging;

	public event EventHandler? ActivePaneChanged;

	public event EventHandler? ShellPaneArrangementChanged;

	public MultiPanesContext()
	{
		WeakReferenceMessenger.Default.Register(this);
	}

	public void Receive(FileSystemChangedMessage message)
	{
		switch (message.ChangeType)
		{
		case FileSystemChangeType.Deleted:
			HandleFileDeleted(message.Path);
			break;
		case FileSystemChangeType.Renamed:
			HandleFileRenamed(message.OldPath, message.Path);
			break;
		}
	}

	public void Dispose()
	{
		WeakReferenceMessenger.Default.Unregister<FileSystemChangedMessage>(this);
	}

	private void HandleFileDeleted(string path)
	{
		if (ActivePaneViewModel != null && !string.IsNullOrWhiteSpace(path) && !string.IsNullOrWhiteSpace(ActivePaneViewModel.WorkingDirectory) && PathHelpers.IsSubPathOf(ActivePaneViewModel.WorkingDirectory, path))
		{
			ActivePaneViewModel.SetWorkingDirectory(PathHelpers.GetNearestValidParentDirectory(path), SetNavPathLocationEnum.FileDeletedNotify);
		}
	}

	private void HandleFileRenamed(string oldPath, string newPath)
	{
		if (ActivePaneViewModel != null && !string.IsNullOrWhiteSpace(oldPath) && !string.IsNullOrWhiteSpace(newPath) && !string.IsNullOrWhiteSpace(ActivePaneViewModel.WorkingDirectory) && PathHelpers.IsSubPathOf(ActivePaneViewModel.WorkingDirectory, oldPath))
		{
			ActivePaneViewModel.SetWorkingDirectory(ActivePaneViewModel.WorkingDirectory.Replace(oldPath, newPath), SetNavPathLocationEnum.FileRenamedNotify);
		}
	}

	private void Page_PropertyChanged(object? sender, PropertyChangedEventArgs e)
	{
		string propertyName = e.PropertyName;
		if (!(propertyName == "ActivePane"))
		{
			if (propertyName == "ShellPaneArrangement")
			{
				UpdateShellPaneArrangement();
			}
		}
		else
		{
			UpdateContent();
		}
	}

	public void UpdateShellPanesView(ShellPanesView? newPage)
	{
		if (!object.Equals(PanesView, newPage))
		{
			if (PanesView != null)
			{
				PanesView.PropertyChanged -= Page_PropertyChanged;
			}
			PanesView = newPage;
			if (PanesView != null)
			{
				PanesView.PropertyChanged += Page_PropertyChanged;
			}
			UpdateContent();
		}
	}

	private void UpdateShellPaneArrangement()
	{
		_ShellPaneArrangement = PanesView?.ShellPaneArrangement ?? ShellPaneArrangement.Single;
		ShellPaneArrangementChanged?.Invoke(this, EventArgs.Empty);
	}

	private void UpdateContent()
	{
		if (PanesView?.ActivePane != _ActivePane)
		{
			ActivePaneChanging?.Invoke(this, EventArgs.Empty);
			_ActivePane = PanesView?.ActivePane;
			_ActivePaneViewModel = PanesView?.ActivePaneViewModel;
			ActivePaneChanged?.Invoke(this, EventArgs.Empty);
			UpdateShellPaneArrangement();
		}
	}
}
