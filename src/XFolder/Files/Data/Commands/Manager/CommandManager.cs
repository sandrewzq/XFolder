using System;
using System.Collections;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Files.Actions;
using Files.Actions.Content;
using Files.Actions.Content.ImageManipulation;
using Files.Actions.Content.PreviewPopup;
using Files.Actions.Content.Selection;
using Files.Actions.FileSystem;
using Files.Actions.Navigation;
using Files.Actions.Navigation.Open;
using Files.Actions.Open;
using Files.Actions.Show;
using Files.Data.Commands.HotKey;

namespace Files.Data.Commands.Manager;

internal sealed class CommandManager : ICommandManager, IEnumerable<IRichCommand>, IEnumerable
{
	private readonly FrozenDictionary<CommandCodes, IRichCommand> commands;

	private ImmutableDictionary<Files.Data.Commands.HotKey.HotKey, IRichCommand> _allKeyBindings = new Dictionary<Files.Data.Commands.HotKey.HotKey, IRichCommand>().ToImmutableDictionary();

	public IRichCommand this[CommandCodes code]
	{
		get
		{
			if (!commands.TryGetValue(code, out IRichCommand value))
			{
				return None;
			}
			return value;
		}
	}

	public IRichCommand this[string code]
	{
		get
		{
			try
			{
				return commands[Enum.Parse<CommandCodes>(code, ignoreCase: true)];
			}
			catch
			{
				return None;
			}
		}
	}

	public IRichCommand this[Files.Data.Commands.HotKey.HotKey hotKey]
	{
		get
		{
			ImmutableDictionary<Files.Data.Commands.HotKey.HotKey, IRichCommand> allKeyBindings = _allKeyBindings;
			Files.Data.Commands.HotKey.HotKey key = hotKey with { IsVisible = true };
			if (!allKeyBindings.TryGetValue(key, out IRichCommand value))
			{
				ImmutableDictionary<Files.Data.Commands.HotKey.HotKey, IRichCommand> allKeyBindings2 = _allKeyBindings;
				key = hotKey with { IsVisible = false };
				if (!allKeyBindings2.TryGetValue(key, out value))
				{
					return None;
				}
				return value;
			}
			return value;
		}
	}

	public IRichCommand None => commands[CommandCodes.None];

	public IRichCommand OpenTerminal => commands[CommandCodes.OpenTerminal];

	public CommandManager()
	{
		commands = (from action in CreateActions()
			select new ActionCommand(this, action.Key, action.Value)).Cast<IRichCommand>().Append(new NoneCommand()).ToFrozenDictionary((IRichCommand command) => command.Code);
		OverwriteKeyBindings();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}

	public IEnumerator<IRichCommand> GetEnumerator()
	{
		return ((IEnumerable<IRichCommand>)commands.Values).GetEnumerator();
	}

	private static Dictionary<CommandCodes, IAction> CreateActions()
	{
		return new Dictionary<CommandCodes, IAction>
		{
			[CommandCodes.ToggleFullScreen] = new ToggleFullScreenAction(),
			[CommandCodes.ToggleSidebar] = new ToggleSidebarAction(),
			[CommandCodes.RefreshItems] = new RefreshItemsAction(),
			[CommandCodes.EditPath] = new EditPathAction(),
			[CommandCodes.OpenTerminal] = new OpenTerminalAction(),
			[CommandCodes.OpenFinder] = new OpenFinderAction(),
			[CommandCodes.OpenProperties] = new OpenPropertiesAction(),
			[CommandCodes.LaunchPreviewPopup] = new LaunchPreviewPopupAction(),
			[CommandCodes.OpenHashes] = new OpenHashesAction(),
			[CommandCodes.OpenSettings] = new OpenSettingsAction(),
			[CommandCodes.OpenSettingsFile] = new OpenSettingsFileAction(),
			[CommandCodes.OpenAppSupportFolder] = new OpenAppSupportFolderAction(),
			[CommandCodes.OpenWorkspaceFromFile] = new OpenWorkspaceFromFileAction(),
			[CommandCodes.SaveWorkspaceAs] = new SaveWorkspaceAsAction(),
			[CommandCodes.ArrangePanesReset] = new ArrangePanesResetAction(),
			[CommandCodes.ArrangePanesHorizontally] = new ArrangePanesHorizontallyAction(),
			[CommandCodes.ArrangePanesVertically] = new ArrangePanesVerticallyAction(),
			[CommandCodes.ArrangePanesQuad] = new ArrangePanesQuadAction(),
			[CommandCodes.ArrangePanesLeftTwoRight] = new ArrangePanesLeftTwoRightAction(),
			[CommandCodes.ArrangePanesTopTwoBottom] = new ArrangePanesTopTwoBottomAction(),
			[CommandCodes.ArrangePanesThreeColumns] = new ArrangePanesThreeColumnsAction(),
			[CommandCodes.ArrangePanesThreeRows] = new ArrangePanesThreeRowsAction(),
			[CommandCodes.ArrangePanesThreeColumnsWeighted] = new ArrangePanesThreeColumnsWeightedAction(),
			[CommandCodes.ArrangePanesThreeRowsWeighted] = new ArrangePanesThreeRowsWeightedAction(),
			[CommandCodes.ArrangePanesFourColumns] = new ArrangePanesFourColumnsAction(),
			[CommandCodes.ArrangePanesFourRows] = new ArrangePanesFourRowsAction(),
			[CommandCodes.ArrangePanesTwoLeftRight] = new ArrangePanesTwoLeftRightAction(),
			[CommandCodes.ArrangePanesTwoTopBottom] = new ArrangePanesTwoTopBottomAction(),
			[CommandCodes.LayoutDetails] = new LayoutDetailsAction(),
			[CommandCodes.LayoutGrid] = new LayoutGridAction(),
			[CommandCodes.ToggleShowHiddenItems] = new ToggleShowHiddenItemsAction(),
			[CommandCodes.ToggleShowFileExtensions] = new ToggleShowFileExtensionsAction(),
			[CommandCodes.OpenInNewTab] = new OpenInNewTabAction(),
			[CommandCodes.DuplicateSelectedTab] = new DuplicateSelectedTabAction(),
			[CommandCodes.NewTab] = new NewTabAction(),
			[CommandCodes.ReopenClosedTab] = new ReopenClosedTabAction(),
			[CommandCodes.CloseTabsToTheLeftSelected] = new CloseTabsToTheLeftSelectedAction(),
			[CommandCodes.CloseTabsToTheRightSelected] = new CloseTabsToTheRightSelectedAction(),
			[CommandCodes.CloseOtherTabsSelected] = new CloseOtherTabsSelectedAction(),
			[CommandCodes.NextTab] = new NextTabAction(),
			[CommandCodes.PreviousTab] = new PreviousTabAction(),
			[CommandCodes.SwitchToFirstTab] = new SwitchToFirstTabAction(),
			[CommandCodes.SwitchToSecondTab] = new SwitchToSecondTabAction(),
			[CommandCodes.SwitchToThirdTab] = new SwitchToThirdTabAction(),
			[CommandCodes.SwitchToFourthTab] = new SwitchToFourthTabAction(),
			[CommandCodes.SwitchToFifthTab] = new SwitchToFifthTabAction(),
			[CommandCodes.SwitchToSixthTab] = new SwitchToSixthTabAction(),
			[CommandCodes.SwitchToSeventhTab] = new SwitchToSeventhTabAction(),
			[CommandCodes.SwitchToEighthTab] = new SwitchToEighthTabAction(),
			[CommandCodes.SwitchToLastTab] = new SwitchToLastTabAction(),
			[CommandCodes.FocusNextPane] = new FocusNextPaneAction(),
			[CommandCodes.FocusPrevPane] = new FocusPrevPaneAction(),
			[CommandCodes.OpenItem] = new OpenItemAction(),
			[CommandCodes.CloseSelectedTab] = new CloseSelectedTabAction(),
			[CommandCodes.DeleteItem] = new DeleteItemAction(),
			[CommandCodes.DeleteItemPermanently] = new DeleteItemPermanentlyAction(),
			[CommandCodes.PasteItem] = new PasteItemAction(),
			[CommandCodes.PasteItemToSelection] = new PasteItemToSelectionAction(),
			[CommandCodes.CopyItem] = new CopyItemAction(),
			[CommandCodes.CopyItemPath] = new CopyItemPathAction(),
			[CommandCodes.CopyPath] = new CopyPathAction(),
			[CommandCodes.CopyItemPathWithQuotes] = new CopyItemPathWithQuotesAction(),
			[CommandCodes.CopyPathWithQuotes] = new CopyPathWithQuotesAction(),
			[CommandCodes.CutItem] = new CutItemAction(),
			[CommandCodes.CreateFolder] = new CreateFolderAction(),
			[CommandCodes.CreateNewFile] = new CreateNewFileAction(),
			[CommandCodes.CreateFolderWithSelection] = new CreateFolderWithSelectionAction(),
			[CommandCodes.Rename] = new RenameAction(),
			[CommandCodes.Undo] = new UndoAction(),
			[CommandCodes.Redo] = new RedoAction(),
			[CommandCodes.SelectAll] = new SelectAllAction(),
			[CommandCodes.InvertSelection] = new InvertSelectionAction(),
			[CommandCodes.ClearSelection] = new ClearSelectionAction(),
			[CommandCodes.PinToSidebar] = new PinFolderToSidebarAction(),
			[CommandCodes.CompressIntoZip] = new CompressIntoZipAction(),
			[CommandCodes.CompressIntoTar] = new CompressIntoTarAction(),
			[CommandCodes.RotateLeft] = new RotateLeftAction(),
			[CommandCodes.RotateRight] = new RotateRightAction(),
			[CommandCodes.ConvertToPng] = new ConvertToPngAction(),
			[CommandCodes.ConvertToJpeg] = new ConvertToJpegAction(),
			[CommandCodes.ConvertToBmp] = new ConvertToBmpAction(),
			[CommandCodes.NavigateUp] = new NavigateUpAction(),
			[CommandCodes.NavigateBack] = new NavigateBackAction(),
			[CommandCodes.NavigateForward] = new NavigateForwardAction(),
			[CommandCodes.NavigateHome] = new NavigateHomeAction(),
			[CommandCodes.NavigateToApplications] = new OpenApplicationsLocationAction(),
			[CommandCodes.NavigateToDesktop] = new OpenDesktopLocationAction(),
			[CommandCodes.NavigateToDocuments] = new OpenDocumentsLocationAction(),
			[CommandCodes.NavigateToDownloads] = new OpenDownloadsLocationAction(),
			[CommandCodes.NavigateToMusic] = new OpenMusicLocationAction(),
			[CommandCodes.NavigateToMovies] = new OpenMoviesLocationAction(),
			[CommandCodes.NavigateToPictures] = new OpenPicturesLocationAction()
		};
	}

	private void OverwriteKeyBindings()
	{
		foreach (ActionCommand item in commands.Values.OfType<ActionCommand>())
		{
			item.RestoreKeyBindings();
		}
		_allKeyBindings = (from command in commands.Values
			from hotKey in command.HotKeys
			select (Command: command, HotKey: hotKey)).ToImmutableDictionary(((IRichCommand Command, Files.Data.Commands.HotKey.HotKey HotKey) item) => item.HotKey, ((IRichCommand Command, Files.Data.Commands.HotKey.HotKey HotKey) item) => item.Command);
	}

	public static HotKeyCollection GetDefaultKeyBindings(IAction action)
	{
		return new HotKeyCollection(action.HotKey, action.SecondHotKey, action.ThirdHotKey, action.MediaHotKey);
	}
}
