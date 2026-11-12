using System.Collections.Generic;
using Avalonia.Input;
using Files.Data.Commands;
using Files.Data.Enums;
using Files.Helpers.Shared;
using Files.Helpers.UI;

namespace Files.Data.Models;

public sealed class ContextMenuFlyoutItemViewModelBuilder
{
	private static readonly ContextMenuFlyoutItemViewModel none = new ContextMenuFlyoutItemViewModel
	{
		ShowItem = false
	};

	private readonly IRichCommand command;

	private bool? isVisible;

	public bool IsVisible
	{
		get
		{
			return isVisible ?? command.IsExecutable;
		}
		init
		{
			isVisible = value;
		}
	}

	public bool IsToggle { get; init; }

	public string AccessKey { get; init; } = string.Empty;

	public object? Tag { get; init; }

	public bool ShowOnShift { get; init; }

	public List<ContextMenuFlyoutItemViewModel>? Items { get; init; }

	public ContextMenuFlyoutItemViewModelBuilder(IRichCommand command)
	{
		this.command = command;
	}

	public ContextMenuFlyoutItemViewModel Build()
	{
		if ((!isVisible) ?? false)
		{
			return none;
		}
		bool isExecutable = command.IsExecutable;
		bool? flag = isVisible;
		if (!flag.HasValue && !isExecutable)
		{
			return none;
		}
		ContextMenuFlyoutItemType itemType = (IsToggle ? ContextMenuFlyoutItemType.Toggle : ContextMenuFlyoutItemType.Item);
		return new ContextMenuFlyoutItemViewModel
		{
			Text = command.Label,
			HotKeyText = command.HotKeyText,
			Icon = AvaloniaUIHelpers.GetIconResource(command.Icon),
			InputGesture = BuildInputGesture(),
			Tag = Tag,
			Command = command,
			IsEnabled = isExecutable,
			IsChecked = command.IsOn,
			Items = Items,
			ItemType = itemType,
			ShowItem = true,
			ShowOnShift = ShowOnShift,
			ShowInRecycleBin = true,
			ShowInSearchPage = true,
			ShowInFtpPage = true,
			ShowInZipPage = true,
			AccessKey = AccessKey
		};
	}

	private KeyGesture? BuildInputGesture()
	{
		return ShortcutHelpers.GetPrimaryKeyGesture(command.HotKeys);
	}
}
