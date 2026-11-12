using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using Files.Assets.Lang;
using Files.Data.Items;
using Files.Data.Models;
using Files.Helpers.Shared;

namespace Files.ViewModels.Properties;

public sealed class HashesViewModel : ObservableObject, IDisposable
{
	private readonly ListedItem item;

	private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

	private string hashInput = string.Empty;

	private string infoBarTitle = string.Empty;

	public string HashInput
	{
		get
		{
			return hashInput;
		}
		set
		{
			if (SetProperty(ref hashInput, value, "HashInput"))
			{
				OnHashInputTextChanged();
				OnPropertyChanged("IsInfoBarOpen");
			}
		}
	}

	public string InfoBarTitle
	{
		get
		{
			return infoBarTitle;
		}
		set
		{
			SetProperty(ref infoBarTitle, value, "InfoBarTitle");
		}
	}

	public bool IsInfoBarOpen => !string.IsNullOrWhiteSpace(HashInput);

	public string ItemName => item.Name;

	public ObservableCollection<HashInfoItem> Hashes { get; } = new ObservableCollection<HashInfoItem>
	{
		new HashInfoItem
		{
			Algorithm = "CRC32"
		},
		new HashInfoItem
		{
			Algorithm = "MD5"
		},
		new HashInfoItem
		{
			Algorithm = "SHA1"
		},
		new HashInfoItem
		{
			Algorithm = "SHA256"
		},
		new HashInfoItem
		{
			Algorithm = "SHA384"
		},
		new HashInfoItem
		{
			Algorithm = "SHA512"
		}
	};

	public HashesViewModel(ListedItem item)
	{
		this.item = item;
		CalculateHashesAsync();
	}

	public async Task CompareFileAsync(string filePath)
	{
		_ = 1;
		try
		{
			await using FileStream stream = File.OpenRead(filePath);
			HashInput = await ChecksumHelpers.CreateMD5(stream, cancellationTokenSource.Token);
		}
		catch (OperationCanceledException)
		{
		}
		catch
		{
			HashInput = string.Empty;
		}
	}

	private async Task CalculateHashesAsync()
	{
		foreach (HashInfoItem hash in Hashes)
		{
			hash.IsCalculating = true;
		}
		try
		{
			Dictionary<string, string> dictionary = await Task.Run(() => ChecksumHelpers.CreateHashesForPathAsync(item.ItemPath, cancellationTokenSource.Token), cancellationTokenSource.Token);
			foreach (HashInfoItem hash2 in Hashes)
			{
				hash2.HashValue = dictionary[hash2.Algorithm];
				hash2.IsCalculated = true;
			}
		}
		catch (OperationCanceledException)
		{
		}
		catch (IOException)
		{
			SetCalculationError(Resources.CalculationErrorFileIsOpen);
		}
		catch
		{
			SetCalculationError(Resources.CalculationError);
		}
		finally
		{
			foreach (HashInfoItem hash3 in Hashes)
			{
				hash3.IsCalculating = false;
			}
			OnHashInputTextChanged();
		}
	}

	private void SetCalculationError(string message)
	{
		foreach (HashInfoItem hash in Hashes)
		{
			hash.HashValue = message;
		}
	}

	private void OnHashInputTextChanged()
	{
		string text = FindMatchingAlgorithm(HashInput);
		InfoBarTitle = (string.IsNullOrEmpty(text) ? Resources.HashesDoNotMatch : string.Format(Resources.HashesMatch, text));
	}

	private string FindMatchingAlgorithm(string hash)
	{
		if (string.IsNullOrWhiteSpace(hash))
		{
			return string.Empty;
		}
		string normalizedHash = hash.Trim();
		return Hashes.FirstOrDefault((HashInfoItem h) => h.IsCalculated && h.HashValue.Equals(normalizedHash, StringComparison.OrdinalIgnoreCase))?.Algorithm ?? string.Empty;
	}

	public void Dispose()
	{
		cancellationTokenSource.Cancel();
		cancellationTokenSource.Dispose();
	}
}
