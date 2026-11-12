using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Avalonia.Controls;
using Files.Data.Contracts;
using Files.Data.Enums;
using Files.Dialogs;
using Files.Helpers.UI;
using Files.ViewModels.Dialogs;

namespace Files.Services.App;

public class AppDialogService : IDialogService
{
	private readonly Dictionary<Type, Func<UserControl>> _dialogContentFactories;

	public AppDialogService()
	{
		_dialogContentFactories = new Dictionary<Type, Func<UserControl>>
		{
			{
				typeof(CreateItemDialogViewModel),
				() => new CreateItemDialog()
			},
			{
				typeof(BulkRenameDialogViewModel),
				() => new BulkRenameDialog()
			}
		};
	}

	private UserControl GetDialog<TViewModel>(TViewModel viewModel) where TViewModel : class, INotifyPropertyChanged
	{
		if (!_dialogContentFactories.TryGetValue(typeof(TViewModel), out Func<UserControl> value))
		{
			throw new ArgumentException($"{typeof(TViewModel)} does not have an appropriate dialog associated with it.");
		}
		UserControl userControl = value();
		userControl.DataContext = viewModel;
		return userControl;
	}

	public async Task<DialogResult> ShowDialogAsync<TViewModel>(TViewModel viewModel) where TViewModel : class, INotifyPropertyChanged
	{
		try
		{
			return await GetDialog(viewModel).TryShowAsync();
		}
		catch (Exception)
		{
			return DialogResult.None;
		}
	}
}
