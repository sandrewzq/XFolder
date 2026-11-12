using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using DialogHostAvalonia;
using Files.Data.Enums;

namespace Files.Dialogs;

public class BaseUserDialog : UserControl
{
	public BaseUserDialog()
	{
		AttachedToVisualTree += (object? s, VisualTreeAttachmentEventArgs e) =>
		{
			Focusable = true;
			Focus();
		};
		AddHandler(InputElement.KeyDownEvent, OnPreviewKeyDown, RoutingStrategies.Tunnel);
		AddHandler(InputElement.KeyUpEvent, OnPreviewKeyUp, RoutingStrategies.Tunnel);
	}

	private void OnPreviewKeyDown(object? sender, KeyEventArgs e)
	{
		if (e.Key == Key.Escape)
		{
			DialogHost.Close("MainDialogHost", DynamicDialogResult.Cancel);
			e.Handled = true;
		}
	}

	private void OnPreviewKeyUp(object? sender, KeyEventArgs e)
	{
	}

	protected override void OnKeyDown(KeyEventArgs e)
	{
		if (e.Key == Key.Escape)
		{
			DialogHost.Close("MainDialogHost", DynamicDialogResult.Cancel);
			e.Handled = true;
		}
		else
		{
			base.OnKeyDown(e);
		}
	}

	protected override void OnKeyUp(KeyEventArgs e)
	{
		base.OnKeyUp(e);
	}

	protected override void OnTextInput(TextInputEventArgs e)
	{
		base.OnTextInput(e);
	}
}
