using System;
using System.Threading.Tasks;
using Files.Data.Contracts;
using Files.Services.NativePlugin;

namespace Files.Services.PreviewPopupProviders;

public sealed class QuickLookProvider : IPreviewPopupProvider
{
	private string? currentPath;

	public static QuickLookProvider Instance { get; } = new QuickLookProvider();

	private QuickLookProvider()
	{
	}

	public Task TogglePreviewPopupAsync(string path)
	{
		if (string.IsNullOrWhiteSpace(path))
		{
			return Task.CompletedTask;
		}
		if (!MacPluginInterop.ToggleQuickLookPreview(path))
		{
			return Task.CompletedTask;
		}
		currentPath = (MacPluginInterop.IsQuickLookPreviewOpen() ? path : null);
		return Task.CompletedTask;
	}

	public Task SwitchPreviewAsync(string path)
	{
		if (string.IsNullOrWhiteSpace(path))
		{
			return Task.CompletedTask;
		}
		if (!MacPluginInterop.IsQuickLookPreviewOpen())
		{
			currentPath = null;
			return Task.CompletedTask;
		}
		if (string.Equals(currentPath, path, StringComparison.Ordinal))
		{
			return Task.CompletedTask;
		}
		if (MacPluginInterop.SwitchQuickLookPreview(path))
		{
			currentPath = path;
		}
		return Task.CompletedTask;
	}

	public Task<bool> DetectAvailability()
	{
		return Task.FromResult(MacPluginInterop.IsQuickLookPreviewAvailable());
	}
}
