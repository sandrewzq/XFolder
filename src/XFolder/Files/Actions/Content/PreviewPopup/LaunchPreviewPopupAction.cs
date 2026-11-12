using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using Files.Assets.Lang;
using Files.Data.Commands.HotKey;
using Files.Data.Contexts.ContentPage;
using Files.Data.Contracts;
using Files.Data.Items;

namespace Files.Actions.Content.PreviewPopup;

internal sealed class LaunchPreviewPopupAction : ObservableObject, IAction
{
	private readonly IContentPageContext context;

	private readonly IPreviewPopupService previewPopupService;

	private long previewSwitchVersion;

	public string Label => Resources.LaunchPreviewPopup;

	public string Description => Resources.LaunchPreviewPopupDescription;

	public HotKey HotKey => new HotKey(Key.Space);

	public bool IsExecutable
	{
		get
		{
			if (context.HasSelection)
			{
				IShellView? shellView = context.ShellView;
				if (shellView == null)
				{
					return false;
				}
				return shellView.InstanceViewModel?.SelectedTab?.LayoutViewModel?.LayoutView?.IsRenamingItem == false;
			}
			return false;
		}
	}

	public LaunchPreviewPopupAction()
	{
		context = Ioc.Default.GetRequiredService<IContentPageContext>();
		previewPopupService = Ioc.Default.GetRequiredService<IPreviewPopupService>();
		context.PropertyChanged += Context_PropertyChanged;
	}

	public async Task ExecuteAsync(object? parameter = null)
	{
		if (!IsExecutable)
		{
			return;
		}
		IPreviewPopupProvider previewPopupProvider = await previewPopupService.GetProviderAsync();
		if (previewPopupProvider != null)
		{
			string previewTargetItemPath = GetPreviewTargetItemPath();
			if (!string.IsNullOrWhiteSpace(previewTargetItemPath))
			{
				await previewPopupProvider.TogglePreviewPopupAsync(previewTargetItemPath);
			}
		}
	}

	private async Task SwitchPopupPreviewAsync()
	{
		long version = Interlocked.Increment(ref previewSwitchVersion);
		if (!IsExecutable)
		{
			return;
		}
		IPreviewPopupProvider previewPopupProvider = await previewPopupService.GetProviderAsync();
		if (previewPopupProvider != null && version == Interlocked.Read(in previewSwitchVersion))
		{
			string previewTargetItemPath = GetPreviewTargetItemPath();
			if (!string.IsNullOrWhiteSpace(previewTargetItemPath))
			{
				await previewPopupProvider.SwitchPreviewAsync(previewTargetItemPath);
			}
		}
	}

	private string? GetPreviewTargetItemPath()
	{
		IReadOnlyList<ListedItem> selectedItems = context.SelectedItems;
		if (selectedItems.Count == 0)
		{
			return null;
		}
		return selectedItems[0]?.ItemPath;
	}

	private async void Context_PropertyChanged(object? sender, PropertyChangedEventArgs e)
	{
		switch (e.PropertyName)
		{
		case "SelectedItems":
		case "SelectedItem":
		case "ShellView":
			OnPropertyChanged("IsExecutable");
			await SwitchPopupPreviewAsync();
			break;
		}
	}
}
