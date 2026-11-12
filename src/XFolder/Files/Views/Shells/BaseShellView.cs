using System;
using System.ComponentModel;
using System.Threading;
using Avalonia.Controls;
using Files.Data.Contracts;
using Files.Data.Enums;
using Files.Utils.Storage.History;
using Files.Utils.Storage.Operations;
using Files.ViewModels.Shells;
using Files.Views.Layouts;

namespace Files.Views.Shells;

public abstract class BaseShellView : UserControl, IShellView, IMultiPaneInfo, IDisposable, INotifyPropertyChanged
{
	protected readonly CancellationTokenSource cancellationTokenSource;

	public required IShellPanesView PaneHolder { get; init; }

	public StorageHistoryHelpers StorageHistoryHelpers { get; }

	public BaseShellViewModel? InstanceViewModel => DataContext as BaseShellViewModel;

	public FilesystemHelpers FilesystemHelpers { get; set; }

	public Type CurrentPageType
	{
		get
		{
			if (InstanceViewModel?.SelectedTab == null)
			{
				return typeof(DetailsLayoutView);
			}
			return InstanceViewModel.SelectedTab.FolderLayoutMode switch
			{
				FolderLayoutModes.DetailsView => typeof(DetailsLayoutView), 
				FolderLayoutModes.GridView => typeof(GridLayoutView), 
				_ => typeof(DetailsLayoutView), 
			};
		}
	}

	public BaseShellView()
	{
		FilesystemHelpers = new FilesystemHelpers(this);
		cancellationTokenSource = new CancellationTokenSource();
		StorageHistoryHelpers = new StorageHistoryHelpers(new StorageHistoryOperations(this, cancellationTokenSource.Token));
	}

	public virtual void Dispose()
	{
	}
}
