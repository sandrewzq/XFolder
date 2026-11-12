using System.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using Files.Services.NativePlugin;

namespace Files.Data.Models;

public sealed class AppModel : ObservableObject
{
	private volatile int _IsMainWindowClosed;

	public bool IsMainWindowClosed
	{
		get
		{
			return _IsMainWindowClosed == 1;
		}
		set
		{
			int num = Interlocked.Exchange(ref _IsMainWindowClosed, value ? 1 : 0);
			if (_IsMainWindowClosed != num)
			{
				OnPropertyChanged("IsMainWindowClosed");
			}
		}
	}

	public bool IsPasteEnabled()
	{
		return MacPluginInterop.ClipboardHasFiles();
	}

	public void NotifyPasteAvailabilityChanged()
	{
		OnPropertyChanged("IsPasteEnabled");
	}
}
