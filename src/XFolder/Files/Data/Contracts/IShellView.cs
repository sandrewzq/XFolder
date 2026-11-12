using System;
using System.ComponentModel;
using Files.Utils.Storage.History;
using Files.Utils.Storage.Operations;
using Files.ViewModels.Shells;

namespace Files.Data.Contracts;

public interface IShellView : IMultiPaneInfo, IDisposable, INotifyPropertyChanged
{
	BaseShellViewModel InstanceViewModel { get; }

	StorageHistoryHelpers StorageHistoryHelpers { get; }

	FilesystemHelpers FilesystemHelpers { get; protected set; }

	Type CurrentPageType { get; }
}
