using System.ComponentModel;
using System.Threading.Tasks;
using Avalonia.Controls;
using DialogHostAvalonia;
using Files.Data.Enums;

namespace Files.Helpers.UI;

public static class UIHelpers
{
	private static bool canShowDialog = true;

	public static bool CanShowDialog
	{
		get
		{
			return canShowDialog;
		}
		private set
		{
			if (value != canShowDialog)
			{
				canShowDialog = value;
				PropertyChanged?.Invoke(null, new PropertyChangedEventArgs("CanShowDialog"));
			}
		}
	}

	public static event PropertyChangedEventHandler? PropertyChanged;

	public static async Task<DialogResult> TryShowAsync(this UserControl dialog)
	{
		if (!canShowDialog)
		{
			return DialogResult.None;
		}
		try
		{
			CanShowDialog = false;
			return (DialogResult)(await DialogHost.Show(dialog, "MainDialogHost"));
		}
		catch
		{
			return DialogResult.None;
		}
		finally
		{
			CanShowDialog = true;
		}
	}
}
