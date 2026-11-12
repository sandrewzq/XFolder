using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace Files.Assets.Lang;

[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
[DebuggerNonUserCode]
[CompilerGenerated]
public class Resources
{
	private static ResourceManager resourceMan;

	private static CultureInfo resourceCulture;

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public static ResourceManager ResourceManager
	{
		get
		{
			if (resourceMan == null)
			{
				resourceMan = new ResourceManager("Files.Assets.Lang.Resources", typeof(Resources).Assembly);
			}
			return resourceMan;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public static CultureInfo Culture
	{
		get
		{
			return resourceCulture;
		}
		set
		{
			resourceCulture = value;
		}
	}

	public static string About => ResourceManager.GetString("About", resourceCulture);

	public static string Access => ResourceManager.GetString("Access", resourceCulture);

	public static string AccessDenied => ResourceManager.GetString("AccessDenied", resourceCulture);

	public static string AccessDeniedCreateDialog_Text => ResourceManager.GetString("AccessDeniedCreateDialog.Text", resourceCulture);

	public static string AccessDeniedDeleteDialog_Text => ResourceManager.GetString("AccessDeniedDeleteDialog.Text", resourceCulture);

	public static string AccessDeniedToFolder => ResourceManager.GetString("AccessDeniedToFolder", resourceCulture);

	public static string Accessed => ResourceManager.GetString("Accessed", resourceCulture);

	public static string Acrylic => ResourceManager.GetString("Acrylic", resourceCulture);

	public static string Actions => ResourceManager.GetString("Actions", resourceCulture);

	public static string Adaptive => ResourceManager.GetString("Adaptive", resourceCulture);

	public static string AdaptiveLayoutDisabledNotification => ResourceManager.GetString("AdaptiveLayoutDisabledNotification", resourceCulture);

	public static string AdaptiveShortcutTooltip => ResourceManager.GetString("AdaptiveShortcutTooltip", resourceCulture);

	public static string Add => ResourceManager.GetString("Add", resourceCulture);

	public static string AddCommand => ResourceManager.GetString("AddCommand", resourceCulture);

	public static string AddDialog_Title => ResourceManager.GetString("AddDialog.Title", resourceCulture);

	public static string AddDialogDescription_Text => ResourceManager.GetString("AddDialogDescription.Text", resourceCulture);

	public static string AddDialogListFileSubHeader => ResourceManager.GetString("AddDialogListFileSubHeader", resourceCulture);

	public static string AddDialogListFolderSubHeader => ResourceManager.GetString("AddDialogListFolderSubHeader", resourceCulture);

	public static string AddDialogListShortcutSubHeader => ResourceManager.GetString("AddDialogListShortcutSubHeader", resourceCulture);

	public static string Added => ResourceManager.GetString("Added", resourceCulture);

	public static string AddFile => ResourceManager.GetString("AddFile", resourceCulture);

	public static string AddHorizontalPane => ResourceManager.GetString("AddHorizontalPane", resourceCulture);

	public static string AddHorizontalPaneDescription => ResourceManager.GetString("AddHorizontalPaneDescription", resourceCulture);

	public static string AddItemDescription => ResourceManager.GetString("AddItemDescription", resourceCulture);

	public static string AddPage => ResourceManager.GetString("AddPage", resourceCulture);

	public static string AddPane => ResourceManager.GetString("AddPane", resourceCulture);

	public static string Address => ResourceManager.GetString("Address", resourceCulture);

	public static string AddToShelf => ResourceManager.GetString("AddToShelf", resourceCulture);

	public static string AddVerticalPane => ResourceManager.GetString("AddVerticalPane", resourceCulture);

	public static string AddVerticalPaneDescription => ResourceManager.GetString("AddVerticalPaneDescription", resourceCulture);

	public static string Advanced => ResourceManager.GetString("Advanced", resourceCulture);

	public static string AdvancedPermissions => ResourceManager.GetString("AdvancedPermissions", resourceCulture);

	public static string Algorithm => ResourceManager.GetString("Algorithm", resourceCulture);

	public static string AllLocations => ResourceManager.GetString("AllLocations", resourceCulture);

	public static string Allow => ResourceManager.GetString("Allow", resourceCulture);

	public static string Always => ResourceManager.GetString("Always", resourceCulture);

	public static string AlwaysSwitchToNewlyOpenedTab => ResourceManager.GetString("AlwaysSwitchToNewlyOpenedTab", resourceCulture);

	public static string Appearance => ResourceManager.GetString("Appearance", resourceCulture);

	public static string Application => ResourceManager.GetString("Application", resourceCulture);

	public static string ApplicationExtension => ResourceManager.GetString("ApplicationExtension", resourceCulture);

	public static string ApplyToAllConflictingItems => ResourceManager.GetString("ApplyToAllConflictingItems", resourceCulture);

	public static string AppVersion => ResourceManager.GetString("AppVersion", resourceCulture);

	public static string ArchiveExtractionCompletedSuccessfullyText => ResourceManager.GetString("ArchiveExtractionCompletedSuccessfullyText", resourceCulture);

	public static string ArchivePassword => ResourceManager.GetString("ArchivePassword", resourceCulture);

	public static string Arguments => ResourceManager.GetString("Arguments", resourceCulture);

	public static string ArrangePanes => ResourceManager.GetString("ArrangePanes", resourceCulture);

	public static string ArrangePanesHorizontally => ResourceManager.GetString("ArrangePanesHorizontally", resourceCulture);

	public static string ArrangePanesHorizontallyDescription => ResourceManager.GetString("ArrangePanesHorizontallyDescription", resourceCulture);

	public static string ArrangePanesVertically => ResourceManager.GetString("ArrangePanesVertically", resourceCulture);

	public static string ArrangePanesVerticallyDescription => ResourceManager.GetString("ArrangePanesVerticallyDescription", resourceCulture);

	public static string Artist => ResourceManager.GetString("Artist", resourceCulture);

	public static string Ascending => ResourceManager.GetString("Ascending", resourceCulture);

	public static string AskCredentialDialog_Title => ResourceManager.GetString("AskCredentialDialog.Title", resourceCulture);

	public static string Attributes => ResourceManager.GetString("Attributes", resourceCulture);

	public static string Author => ResourceManager.GetString("Author", resourceCulture);

	public static string AuthorizationSucceded => ResourceManager.GetString("AuthorizationSucceded", resourceCulture);

	public static string Available => ResourceManager.GetString("Available", resourceCulture);

	public static string Back => ResourceManager.GetString("Back", resourceCulture);

	public static string BackdropMaterial => ResourceManager.GetString("BackdropMaterial", resourceCulture);

	public static string BackgroundColor => ResourceManager.GetString("BackgroundColor", resourceCulture);

	public static string BackgroundImage => ResourceManager.GetString("BackgroundImage", resourceCulture);

	public static string BackgroundRunningNotificationBody => ResourceManager.GetString("BackgroundRunningNotificationBody", resourceCulture);

	public static string BackgroundRunningNotificationHeader => ResourceManager.GetString("BackgroundRunningNotificationHeader", resourceCulture);

	public static string NewVersionAvailablePrompt => ResourceManager.GetString("NewVersionAvailablePrompt", resourceCulture);

	public static string BasedOn => ResourceManager.GetString("BasedOn", resourceCulture);

	public static string BaseLayoutContextFlyoutAdaptive_KeyboardAcceleratorTextOverride => ResourceManager.GetString("BaseLayoutContextFlyoutAdaptive.KeyboardAcceleratorTextOverride", resourceCulture);

	public static string BaseLayoutContextFlyoutNew_Label => ResourceManager.GetString("BaseLayoutContextFlyoutNew.Label", resourceCulture);

	public static string BaseLayoutContextFlyoutRunAsAnotherUser_Text => ResourceManager.GetString("BaseLayoutContextFlyoutRunAsAnotherUser.Text", resourceCulture);

	public static string BaseLayoutItemContextFlyoutExtractToChildFolder => ResourceManager.GetString("BaseLayoutItemContextFlyoutExtractToChildFolder", resourceCulture);

	public static string BaseLayoutItemContextFlyoutOpenParentFolder_Text => ResourceManager.GetString("BaseLayoutItemContextFlyoutOpenParentFolder.Text", resourceCulture);

	public static string BaseLayoutItemContextFlyoutSetAs_Text => ResourceManager.GetString("BaseLayoutItemContextFlyoutSetAs.Text", resourceCulture);

	public static string Behaviors => ResourceManager.GetString("Behaviors", resourceCulture);

	public static string Binary => ResourceManager.GetString("Binary", resourceCulture);

	public static string BitmapFiles => ResourceManager.GetString("BitmapFiles", resourceCulture);

	public static string Blue => ResourceManager.GetString("Blue", resourceCulture);

	public static string BlueGray => ResourceManager.GetString("BlueGray", resourceCulture);

	public static string Bluray => ResourceManager.GetString("Bluray", resourceCulture);

	public static string Bottom => ResourceManager.GetString("Bottom", resourceCulture);

	public static string Branches => ResourceManager.GetString("Branches", resourceCulture);

	public static string BreadcrumbBarChevronButtonToolTip => ResourceManager.GetString("BreadcrumbBarChevronButtonToolTip", resourceCulture);

	public static string BreadcrumbBarEllipsisButtonToolTip => ResourceManager.GetString("BreadcrumbBarEllipsisButtonToolTip", resourceCulture);

	public static string BrickRed => ResourceManager.GetString("BrickRed", resourceCulture);

	public static string BringChanges => ResourceManager.GetString("BringChanges", resourceCulture);

	public static string Browse => ResourceManager.GetString("Browse", resourceCulture);

	public static string BulkRename => ResourceManager.GetString("BulkRename", resourceCulture);

	public static string Bytes => ResourceManager.GetString("Bytes", resourceCulture);

	public static string ByteSymbol => ResourceManager.GetString("ByteSymbol", resourceCulture);

	public static string CalculateFolderSizes => ResourceManager.GetString("CalculateFolderSizes", resourceCulture);

	public static string Calculating => ResourceManager.GetString("Calculating", resourceCulture);

	public static string CalculationError => ResourceManager.GetString("CalculationError", resourceCulture);

	public static string CalculationErrorFileIsOpen => ResourceManager.GetString("CalculationErrorFileIsOpen", resourceCulture);

	public static string CalculationOnlineFileHashError => ResourceManager.GetString("CalculationOnlineFileHashError", resourceCulture);

	public static string Camouflage => ResourceManager.GetString("Camouflage", resourceCulture);

	public static string Cancel => ResourceManager.GetString("Cancel", resourceCulture);

	public static string Canceling => ResourceManager.GetString("Canceling", resourceCulture);

	public static string CannotAccessPropertiesContent => ResourceManager.GetString("CannotAccessPropertiesContent", resourceCulture);

	public static string CannotAccessPropertiesTitle => ResourceManager.GetString("CannotAccessPropertiesTitle", resourceCulture);

	public static string CannotCloneRepoTitle => ResourceManager.GetString("CannotCloneRepoTitle", resourceCulture);

	public static string CannotCreateShortcutDialogMessage => ResourceManager.GetString("CannotCreateShortcutDialogMessage", resourceCulture);

	public static string CannotCreateShortcutDialogTitle => ResourceManager.GetString("CannotCreateShortcutDialogTitle", resourceCulture);

	public static string CannotInitializeGitRepo => ResourceManager.GetString("CannotInitializeGitRepo", resourceCulture);

	public static string CannotReachGitHubError => ResourceManager.GetString("CannotReachGitHubError", resourceCulture);

	public static string Cards => ResourceManager.GetString("Cards", resourceCulture);

	public static string CD => ResourceManager.GetString("CD", resourceCulture);

	public static string Center => ResourceManager.GetString("Center", resourceCulture);

	public static string Change => ResourceManager.GetString("Change", resourceCulture);

	public static string ChangeAlbumCover => ResourceManager.GetString("ChangeAlbumCover", resourceCulture);

	public static string ChooseAnAction => ResourceManager.GetString("ChooseAnAction", resourceCulture);

	public static string ChooseCustomIcon => ResourceManager.GetString("ChooseCustomIcon", resourceCulture);

	public static string Cleanup => ResourceManager.GetString("Cleanup", resourceCulture);

	public static string CleanupDrive => ResourceManager.GetString("CleanupDrive", resourceCulture);

	public static string Clear => ResourceManager.GetString("Clear", resourceCulture);

	public static string ClearCompleted => ResourceManager.GetString("ClearCompleted", resourceCulture);

	public static string ClearItems => ResourceManager.GetString("ClearItems", resourceCulture);

	public static string ClearPropertiesButton_Content => ResourceManager.GetString("ClearPropertiesButton.Content", resourceCulture);

	public static string ClearPropertiesFlyoutText_Text => ResourceManager.GetString("ClearPropertiesFlyoutText.Text", resourceCulture);

	public static string ClearSelection => ResourceManager.GetString("ClearSelection", resourceCulture);

	public static string ClearSelectionDescription => ResourceManager.GetString("ClearSelectionDescription", resourceCulture);

	public static string Clone => ResourceManager.GetString("Clone", resourceCulture);

	public static string CloneRepo => ResourceManager.GetString("CloneRepo", resourceCulture);

	public static string CloneToFolderCaptionText => ResourceManager.GetString("CloneToFolderCaptionText", resourceCulture);

	public static string Close => ResourceManager.GetString("Close", resourceCulture);

	public static string CloseActivePane => ResourceManager.GetString("CloseActivePane", resourceCulture);

	public static string CloseActivePaneDescription => ResourceManager.GetString("CloseActivePaneDescription", resourceCulture);

	public static string CloseAllTabs => ResourceManager.GetString("CloseAllTabs", resourceCulture);

	public static string CloseAllTabsDescription => ResourceManager.GetString("CloseAllTabsDescription", resourceCulture);

	public static string CloseOtherTabs => ResourceManager.GetString("CloseOtherTabs", resourceCulture);

	public static string CloseOtherTabsCurrentDescription => ResourceManager.GetString("CloseOtherTabsCurrentDescription", resourceCulture);

	public static string CloseOtherTabsSelectedDescription => ResourceManager.GetString("CloseOtherTabsSelectedDescription", resourceCulture);

	public static string CloseSelectedTabDescription => ResourceManager.GetString("CloseSelectedTabDescription", resourceCulture);

	public static string CloseTab => ResourceManager.GetString("CloseTab", resourceCulture);

	public static string CloseTabsToTheLeft => ResourceManager.GetString("CloseTabsToTheLeft", resourceCulture);

	public static string CloseTabsToTheLeftCurrentDescription => ResourceManager.GetString("CloseTabsToTheLeftCurrentDescription", resourceCulture);

	public static string CloseTabsToTheLeftSelectedDescription => ResourceManager.GetString("CloseTabsToTheLeftSelectedDescription", resourceCulture);

	public static string CloseTabsToTheRight => ResourceManager.GetString("CloseTabsToTheRight", resourceCulture);

	public static string CloseTabsToTheRightCurrentDescription => ResourceManager.GetString("CloseTabsToTheRightCurrentDescription", resourceCulture);

	public static string CloseTabsToTheRightSelectedDescription => ResourceManager.GetString("CloseTabsToTheRightSelectedDescription", resourceCulture);

	public static string CloudDriveSyncStatus_Excluded => ResourceManager.GetString("CloudDriveSyncStatus_Excluded", resourceCulture);

	public static string CloudDriveSyncStatus_Offline => ResourceManager.GetString("CloudDriveSyncStatus_Offline", resourceCulture);

	public static string CloudDriveSyncStatus_Online => ResourceManager.GetString("CloudDriveSyncStatus_Online", resourceCulture);

	public static string CloudDriveSyncStatus_PartialOffline => ResourceManager.GetString("CloudDriveSyncStatus_PartialOffline", resourceCulture);

	public static string CloudDriveSyncStatus_Sync => ResourceManager.GetString("CloudDriveSyncStatus_Sync", resourceCulture);

	public static string CloudDriveSyncStatus_Unknown => ResourceManager.GetString("CloudDriveSyncStatus_Unknown", resourceCulture);

	public static string Columns => ResourceManager.GetString("Columns", resourceCulture);

	public static string ColumnsShortcutTooltip => ResourceManager.GetString("ColumnsShortcutTooltip", resourceCulture);

	public static string CommandNotExecutable => ResourceManager.GetString("CommandNotExecutable", resourceCulture);

	public static string CommandNotExecutableContent => ResourceManager.GetString("CommandNotExecutableContent", resourceCulture);

	public static string CommandPalette => ResourceManager.GetString("CommandPalette", resourceCulture);

	public static string Commands => ResourceManager.GetString("Commands", resourceCulture);

	public static string CommitMessage => ResourceManager.GetString("CommitMessage", resourceCulture);

	public static string CommitSha => ResourceManager.GetString("CommitSha", resourceCulture);

	public static string Compact => ResourceManager.GetString("Compact", resourceCulture);

	public static string CompareFile => ResourceManager.GetString("CompareFile", resourceCulture);

	public static string Compatibility => ResourceManager.GetString("Compatibility", resourceCulture);

	public static string CompatibilityDisableFullscreenOptimizations => ResourceManager.GetString("CompatibilityDisableFullscreenOptimizations", resourceCulture);

	public static string CompatibilityDoNotAdjustDPI => ResourceManager.GetString("CompatibilityDoNotAdjustDPI", resourceCulture);

	public static string CompatibilityDoNotOverrideDPI => ResourceManager.GetString("CompatibilityDoNotOverrideDPI", resourceCulture);

	public static string CompatibilityMode => ResourceManager.GetString("CompatibilityMode", resourceCulture);

	public static string CompatibilityNoReducedColor => ResourceManager.GetString("CompatibilityNoReducedColor", resourceCulture);

	public static string CompatibilityOnProgramStart => ResourceManager.GetString("CompatibilityOnProgramStart", resourceCulture);

	public static string CompatibilityOnWindowsLogin => ResourceManager.GetString("CompatibilityOnWindowsLogin", resourceCulture);

	public static string CompatibilityOverrideHighDPIBehavior => ResourceManager.GetString("CompatibilityOverrideHighDPIBehavior", resourceCulture);

	public static string CompatibilityReducedColorMode => ResourceManager.GetString("CompatibilityReducedColorMode", resourceCulture);

	public static string CompatibilityReducedColorModeColor16bit => ResourceManager.GetString("CompatibilityReducedColorModeColor16bit", resourceCulture);

	public static string CompatibilityReducedColorModeColor8bit => ResourceManager.GetString("CompatibilityReducedColorModeColor8bit", resourceCulture);

	public static string CompatibilityRegisterThisProgramForRestart => ResourceManager.GetString("CompatibilityRegisterThisProgramForRestart", resourceCulture);

	public static string CompatibilityRunIn640x480Resolution => ResourceManager.GetString("CompatibilityRunIn640x480Resolution", resourceCulture);

	public static string CompatibilityRunTroubleshooter => ResourceManager.GetString("CompatibilityRunTroubleshooter", resourceCulture);

	public static string CompatibilitySystemEnhanced => ResourceManager.GetString("CompatibilitySystemEnhanced", resourceCulture);

	public static string CompatibilityUseDPISettings => ResourceManager.GetString("CompatibilityUseDPISettings", resourceCulture);

	public static string Compress => ResourceManager.GetString("Compress", resourceCulture);

	public static string CompressContents => ResourceManager.GetString("CompressContents", resourceCulture);

	public static string CompressIntoArchiveDescription => ResourceManager.GetString("CompressIntoArchiveDescription", resourceCulture);

	public static string CompressIntoSevenZipDescription => ResourceManager.GetString("CompressIntoSevenZipDescription", resourceCulture);

	public static string CompressIntoZipDescription => ResourceManager.GetString("CompressIntoZipDescription", resourceCulture);

	public static string CompressIntoTarDescription => ResourceManager.GetString("CompressIntoTarDescription", resourceCulture);

	public static string Compression => ResourceManager.GetString("Compression", resourceCulture);

	public static string CompressionLevel => ResourceManager.GetString("CompressionLevel", resourceCulture);

	public static string CompressionLevelFast => ResourceManager.GetString("CompressionLevelFast", resourceCulture);

	public static string CompressionLevelHigh => ResourceManager.GetString("CompressionLevelHigh", resourceCulture);

	public static string CompressionLevelLow => ResourceManager.GetString("CompressionLevelLow", resourceCulture);

	public static string CompressionLevelNone => ResourceManager.GetString("CompressionLevelNone", resourceCulture);

	public static string CompressionLevelNormal => ResourceManager.GetString("CompressionLevelNormal", resourceCulture);

	public static string CompressionLevelUltra => ResourceManager.GetString("CompressionLevelUltra", resourceCulture);

	public static string Conductor => ResourceManager.GetString("Conductor", resourceCulture);

	public static string Confirm => ResourceManager.GetString("Confirm", resourceCulture);

	public static string ConfirmDeleteTag => ResourceManager.GetString("ConfirmDeleteTag", resourceCulture);

	public static string ConfirmEmptyBinDialogContent => ResourceManager.GetString("ConfirmEmptyBinDialogContent", resourceCulture);

	public static string ConfirmEmptyBinDialogTitle => ResourceManager.GetString("ConfirmEmptyBinDialogTitle", resourceCulture);

	public static string ConfirmRestoreBinDialogContent => ResourceManager.GetString("ConfirmRestoreBinDialogContent", resourceCulture);

	public static string ConfirmRestoreBinDialogTitle => ResourceManager.GetString("ConfirmRestoreBinDialogTitle", resourceCulture);

	public static string ConfirmRestoreSelectionBinDialogContent => ResourceManager.GetString("ConfirmRestoreSelectionBinDialogContent", resourceCulture);

	public static string ConfirmRestoreSelectionBinDialogTitle => ResourceManager.GetString("ConfirmRestoreSelectionBinDialogTitle", resourceCulture);

	public static string ConflictingItemsDialogPrimaryButtonText => ResourceManager.GetString("ConflictingItemsDialogPrimaryButtonText", resourceCulture);

	public static string ConflictingItemsDialogSubtitleConflicts => ResourceManager.GetString("ConflictingItemsDialogSubtitleConflicts", resourceCulture);

	public static string ConflictingItemsDialogSubtitleConflictsNonConflicts => ResourceManager.GetString("ConflictingItemsDialogSubtitleConflictsNonConflicts", resourceCulture);

	public static string ConflictingItemsDialogTitle => ResourceManager.GetString("ConflictingItemsDialogTitle", resourceCulture);

	public static string ConnectedToGitHub => ResourceManager.GetString("ConnectedToGitHub", resourceCulture);

	public static string ConnectGitHub => ResourceManager.GetString("ConnectGitHub", resourceCulture);

	public static string ConnectGitHubDescription => ResourceManager.GetString("ConnectGitHubDescription", resourceCulture);

	public static string ConnectToGitHub => ResourceManager.GetString("ConnectToGitHub", resourceCulture);

	public static string ConsentDialogContent => ResourceManager.GetString("ConsentDialogContent", resourceCulture);

	public static string ConsentDialogPrimaryButtonText => ResourceManager.GetString("ConsentDialogPrimaryButtonText", resourceCulture);

	public static string ConsentDialogTitle => ResourceManager.GetString("ConsentDialogTitle", resourceCulture);

	public static string ContextMenuOptions => ResourceManager.GetString("ContextMenuOptions", resourceCulture);

	public static string ContributingArtist => ResourceManager.GetString("ContributingArtist", resourceCulture);

	public static string CoolBlueBright => ResourceManager.GetString("CoolBlueBright", resourceCulture);

	public static string Copy => ResourceManager.GetString("Copy", resourceCulture);

	public static string CopyCode => ResourceManager.GetString("CopyCode", resourceCulture);

	public static string CopyFailureFileToLarge => ResourceManager.GetString("CopyFailureFileToLarge", resourceCulture);

	public static string CopyFileWithoutProperties => ResourceManager.GetString("CopyFileWithoutProperties", resourceCulture);

	public static string CopyItemDescription => ResourceManager.GetString("CopyItemDescription", resourceCulture);

	public static string CopyItemPath => ResourceManager.GetString("CopyItemPath", resourceCulture);

	public static string CopyItemPathDescription => ResourceManager.GetString("CopyItemPathDescription", resourceCulture);

	public static string CopyItemPathWithQuotes => ResourceManager.GetString("CopyItemPathWithQuotes", resourceCulture);

	public static string CopyItemPathWithQuotesDescription => ResourceManager.GetString("CopyItemPathWithQuotesDescription", resourceCulture);

	public static string CopyItemsDialogSubtitle => ResourceManager.GetString("CopyItemsDialogSubtitle", resourceCulture);

	public static string CopyItemsDialogTitle => ResourceManager.GetString("CopyItemsDialogTitle", resourceCulture);

	public static string CopyPath => ResourceManager.GetString("CopyPath", resourceCulture);

	public static string CopyPathDescription => ResourceManager.GetString("CopyPathDescription", resourceCulture);

	public static string CopyPathWithQuotes => ResourceManager.GetString("CopyPathWithQuotes", resourceCulture);

	public static string CopyPathWithQuotesDescription => ResourceManager.GetString("CopyPathWithQuotesDescription", resourceCulture);

	public static string CopyToFolderCaptionText => ResourceManager.GetString("CopyToFolderCaptionText", resourceCulture);

	public static string CPUThreads => ResourceManager.GetString("CPUThreads", resourceCulture);

	public static string Create => ResourceManager.GetString("Create", resourceCulture);

	public static string CreateAlternateDataStream => ResourceManager.GetString("CreateAlternateDataStream", resourceCulture);

	public static string CreateAlternateDataStreamDescription => ResourceManager.GetString("CreateAlternateDataStreamDescription", resourceCulture);

	public static string CreateArchive => ResourceManager.GetString("CreateArchive", resourceCulture);

	public static string CreateBranch => ResourceManager.GetString("CreateBranch", resourceCulture);

	public static string CreateFolderDescription => ResourceManager.GetString("CreateFolderDescription", resourceCulture);

	public static string CreateFolderWithSelection => ResourceManager.GetString("CreateFolderWithSelection", resourceCulture);

	public static string CreateFolderWithSelectionDescription => ResourceManager.GetString("CreateFolderWithSelectionDescription", resourceCulture);

	public static string CreateLibraryErrorAlreadyExists => ResourceManager.GetString("CreateLibraryErrorAlreadyExists", resourceCulture);

	public static string CreateNamedArchive => ResourceManager.GetString("CreateNamedArchive", resourceCulture);

	public static string CreateNewBranch => ResourceManager.GetString("CreateNewBranch", resourceCulture);

	public static string CreateNewItemTitle => ResourceManager.GetString("CreateNewItemTitle", resourceCulture);

	public static string CreateNewTag => ResourceManager.GetString("CreateNewTag", resourceCulture);

	public static string CreateShortcut => ResourceManager.GetString("CreateShortcut", resourceCulture);

	public static string CreateShortcutDescription => ResourceManager.GetString("CreateShortcutDescription", resourceCulture);

	public static string CreateShortcutFromDialogDescription => ResourceManager.GetString("CreateShortcutFromDialogDescription", resourceCulture);

	public static string CredentialDialogAnonymous_Content => ResourceManager.GetString("CredentialDialogAnonymous.Content", resourceCulture);

	public static string CredentialDialogDescription_Text => ResourceManager.GetString("CredentialDialogDescription.Text", resourceCulture);

	public static string CredentialDialogUserName_PlaceholderText => ResourceManager.GetString("CredentialDialogUserName.PlaceholderText", resourceCulture);

	public static string Custom => ResourceManager.GetString("Custom", resourceCulture);

	public static string Customization => ResourceManager.GetString("Customization", resourceCulture);

	public static string Customized => ResourceManager.GetString("Customized", resourceCulture);

	public static string Cut => ResourceManager.GetString("Cut", resourceCulture);

	public static string CutItemDescription => ResourceManager.GetString("CutItemDescription", resourceCulture);

	public static string DarkTheme => ResourceManager.GetString("DarkTheme", resourceCulture);

	public static string DataStreamsAreHiddenDescription => ResourceManager.GetString("DataStreamsAreHiddenDescription", resourceCulture);

	public static string DataStreamsAreHiddenTitle => ResourceManager.GetString("DataStreamsAreHiddenTitle", resourceCulture);

	public static string DateColumn => ResourceManager.GetString("DateColumn", resourceCulture);

	public static string DateCommitted => ResourceManager.GetString("DateCommitted", resourceCulture);

	public static string DateCreated => ResourceManager.GetString("DateCreated", resourceCulture);

	public static string DateCreatedColumn => ResourceManager.GetString("DateCreatedColumn", resourceCulture);

	public static string DateDeleted => ResourceManager.GetString("DateDeleted", resourceCulture);

	public static string DateFormat => ResourceManager.GetString("DateFormat", resourceCulture);

	public static string DateFormatSample => ResourceManager.GetString("DateFormatSample", resourceCulture);

	public static string DateModifiedLowerCase => ResourceManager.GetString("DateModifiedLowerCase", resourceCulture);

	public static string Day => ResourceManager.GetString("Day", resourceCulture);

	public static string DaysAgo => ResourceManager.GetString("DaysAgo", resourceCulture);

	public static string DaysAgo_Plural => ResourceManager.GetString("DaysAgo_Plural", resourceCulture);

	public static string DaysAgo_Singular => ResourceManager.GetString("DaysAgo_Singular", resourceCulture);

	public static string Decimal => ResourceManager.GetString("Decimal", resourceCulture);

	public static string DecompressArchiveDescription => ResourceManager.GetString("DecompressArchiveDescription", resourceCulture);

	public static string DecompressArchiveDialogOpenDestinationWhenComplete_Content => ResourceManager.GetString("DecompressArchiveDialogOpenDestinationWhenComplete.Content", resourceCulture);

	public static string DecompressArchiveHereDescription => ResourceManager.GetString("DecompressArchiveHereDescription", resourceCulture);

	public static string DecompressArchiveHereSmartDescription => ResourceManager.GetString("DecompressArchiveHereSmartDescription", resourceCulture);

	public static string DecompressArchiveToChildFolderDescription => ResourceManager.GetString("DecompressArchiveToChildFolderDescription", resourceCulture);

	public static string DecreaseSize => ResourceManager.GetString("DecreaseSize", resourceCulture);

	public static string Default => ResourceManager.GetString("Default", resourceCulture);

	public static string DefaultPaneArrangement => ResourceManager.GetString("DefaultPaneArrangement", resourceCulture);

	public static string Delete => ResourceManager.GetString("Delete", resourceCulture);

	public static string Deleted => ResourceManager.GetString("Deleted", resourceCulture);

	public static string DeleteItemDescription => ResourceManager.GetString("DeleteItemDescription", resourceCulture);

	public static string DeleteItemPermanentlyDescription => ResourceManager.GetString("DeleteItemPermanentlyDescription", resourceCulture);

	public static string DeleteItemsDialogPermanentlyDeleteCheckBox_Content => ResourceManager.GetString("DeleteItemsDialogPermanentlyDeleteCheckBox.Content", resourceCulture);

	public static string DeleteItemsDialogSubtitle => ResourceManager.GetString("DeleteItemsDialogSubtitle", resourceCulture);

	public static string DeleteItemsDialogTitle => ResourceManager.GetString("DeleteItemsDialogTitle", resourceCulture);

	public static string DeletePermanently => ResourceManager.GetString("DeletePermanently", resourceCulture);

	public static string DeleteShortcutDescription => ResourceManager.GetString("DeleteShortcutDescription", resourceCulture);

	public static string Deny => ResourceManager.GetString("Deny", resourceCulture);

	public static string Descending => ResourceManager.GetString("Descending", resourceCulture);

	public static string DesiredName => ResourceManager.GetString("DesiredName", resourceCulture);

	public static string Desktop => ResourceManager.GetString("Desktop", resourceCulture);

	public static string Details => ResourceManager.GetString("Details", resourceCulture);

	public static string DetailsArchiveItems => ResourceManager.GetString("DetailsArchiveItems", resourceCulture);

	public static string DetailsPanePreviewNotAvaliableText => ResourceManager.GetString("DetailsPanePreviewNotAvaliableText", resourceCulture);

	public static string DetailsView => ResourceManager.GetString("DetailsView", resourceCulture);

	public static string DetailsViewHeaderFlyout_ShowDateCreated_Text => ResourceManager.GetString("DetailsViewHeaderFlyout_ShowDateCreated.Text", resourceCulture);

	public static string DetailsViewHeaderFlyout_ShowDateDeleted_Text => ResourceManager.GetString("DetailsViewHeaderFlyout_ShowDateDeleted.Text", resourceCulture);

	public static string DetailsViewHeaderFlyout_ShowDateModified_Text => ResourceManager.GetString("DetailsViewHeaderFlyout_ShowDateModified.Text", resourceCulture);

	public static string DetailsViewHeaderFlyout_ShowItemSize_Text => ResourceManager.GetString("DetailsViewHeaderFlyout_ShowItemSize.Text", resourceCulture);

	public static string DetailsViewHeaderFlyout_ShowItemType_Text => ResourceManager.GetString("DetailsViewHeaderFlyout_ShowItemType.Text", resourceCulture);

	public static string DetailsViewHeaderFlyout_ShowOriginalPath_Text => ResourceManager.GetString("DetailsViewHeaderFlyout_ShowOriginalPath.Text", resourceCulture);

	public static string DetailsViewHeaderFlyout_ShowSyncStatus_Text => ResourceManager.GetString("DetailsViewHeaderFlyout_ShowSyncStatus.Text", resourceCulture);

	public static string DevTools => ResourceManager.GetString("DevTools", resourceCulture);

	public static string DialogRestoreLibrariesSubtitleText => ResourceManager.GetString("DialogRestoreLibrariesSubtitleText", resourceCulture);

	public static string DialogRestoreLibrariesTitleText => ResourceManager.GetString("DialogRestoreLibrariesTitleText", resourceCulture);

	public static string Disable => ResourceManager.GetString("Disable", resourceCulture);

	public static string DiscardChanges => ResourceManager.GetString("DiscardChanges", resourceCulture);

	public static string Dismiss => ResourceManager.GetString("Dismiss", resourceCulture);

	public static string Display => ResourceManager.GetString("Display", resourceCulture);

	public static string DisplayOpenIDE => ResourceManager.GetString("DisplayOpenIDE", resourceCulture);

	public static string Document => ResourceManager.GetString("Document", resourceCulture);

	public static string Documentation => ResourceManager.GetString("Documentation", resourceCulture);

	public static string Documents => ResourceManager.GetString("Documents", resourceCulture);

	public static string DoNotSplit => ResourceManager.GetString("DoNotSplit", resourceCulture);

	public static string DontShowAgain => ResourceManager.GetString("DontShowAgain", resourceCulture);

	public static string DoubleClickBlankSpaceToGoUp => ResourceManager.GetString("DoubleClickBlankSpaceToGoUp", resourceCulture);

	public static string Downloads => ResourceManager.GetString("Downloads", resourceCulture);

	public static string DriveFreeSpaceAndCapacity => ResourceManager.GetString("DriveFreeSpaceAndCapacity", resourceCulture);

	public static string Drives => ResourceManager.GetString("Drives", resourceCulture);

	public static string DrivesWidgetOptionsFlyoutMapNetDriveMenuItem_Text => ResourceManager.GetString("DrivesWidgetOptionsFlyoutMapNetDriveMenuItem.Text", resourceCulture);

	public static string DriveTypeCDRom => ResourceManager.GetString("DriveTypeCDRom", resourceCulture);

	public static string DriveTypeCloudDrive => ResourceManager.GetString("DriveTypeCloudDrive", resourceCulture);

	public static string DriveTypeFixed => ResourceManager.GetString("DriveTypeFixed", resourceCulture);

	public static string DriveTypeFloppyDisk => ResourceManager.GetString("DriveTypeFloppyDisk", resourceCulture);

	public static string DriveTypeNetwork => ResourceManager.GetString("DriveTypeNetwork", resourceCulture);

	public static string DriveTypeNoRootDirectory => ResourceManager.GetString("DriveTypeNoRootDirectory", resourceCulture);

	public static string DriveTypeRam => ResourceManager.GetString("DriveTypeRam", resourceCulture);

	public static string DriveTypeRemovable => ResourceManager.GetString("DriveTypeRemovable", resourceCulture);

	public static string DriveTypeUnknown => ResourceManager.GetString("DriveTypeUnknown", resourceCulture);

	public static string DriveTypeVirtualDrive => ResourceManager.GetString("DriveTypeVirtualDrive", resourceCulture);

	public static string DriveUnpluggedDialog_Text => ResourceManager.GetString("DriveUnpluggedDialog.Text", resourceCulture);

	public static string DriveUnpluggedDialog_Title => ResourceManager.GetString("DriveUnpluggedDialog.Title", resourceCulture);

	public static string DriveWithLetter => ResourceManager.GetString("DriveWithLetter", resourceCulture);

	public static string DuplicateCurrentTabDescription => ResourceManager.GetString("DuplicateCurrentTabDescription", resourceCulture);

	public static string DuplicateSelectedTabDescription => ResourceManager.GetString("DuplicateSelectedTabDescription", resourceCulture);

	public static string DuplicateTab => ResourceManager.GetString("DuplicateTab", resourceCulture);

	public static string DuringOngoingFileOperations => ResourceManager.GetString("DuringOngoingFileOperations", resourceCulture);

	public static string DVD => ResourceManager.GetString("DVD", resourceCulture);

	public static string EarlierThisMonth => ResourceManager.GetString("EarlierThisMonth", resourceCulture);

	public static string EarlierThisWeek => ResourceManager.GetString("EarlierThisWeek", resourceCulture);

	public static string EarlierThisYear => ResourceManager.GetString("EarlierThisYear", resourceCulture);

	public static string Edit => ResourceManager.GetString("Edit", resourceCulture);

	public static string EditInNotepad => ResourceManager.GetString("EditInNotepad", resourceCulture);

	public static string EditInNotepadDescription => ResourceManager.GetString("EditInNotepadDescription", resourceCulture);

	public static string EditPath => ResourceManager.GetString("EditPath", resourceCulture);

	public static string EditPathDescription => ResourceManager.GetString("EditPathDescription", resourceCulture);

	public static string EditPermissions => ResourceManager.GetString("EditPermissions", resourceCulture);

	public static string EditSettingsFile => ResourceManager.GetString("EditSettingsFile", resourceCulture);

	public static string EditSettingsFileDescription => ResourceManager.GetString("EditSettingsFileDescription", resourceCulture);

	public static string OpenAppSupportFolder => ResourceManager.GetString("OpenAppSupportFolder", resourceCulture);

	public static string OpenAppSupportFolderDescription => ResourceManager.GetString("OpenAppSupportFolderDescription", resourceCulture);

	public static string EditTags => ResourceManager.GetString("EditTags", resourceCulture);

	public static string Eject => ResourceManager.GetString("Eject", resourceCulture);

	public static string EjectNotificationBody => ResourceManager.GetString("EjectNotificationBody", resourceCulture);

	public static string EjectNotificationErrorDialogBody => ResourceManager.GetString("EjectNotificationErrorDialogBody", resourceCulture);

	public static string EjectNotificationErrorDialogHeader => ResourceManager.GetString("EjectNotificationErrorDialogHeader", resourceCulture);

	public static string EjectNotificationHeader => ResourceManager.GetString("EjectNotificationHeader", resourceCulture);

	public static string ElevateConfirmDialog_Content => ResourceManager.GetString("ElevateConfirmDialog.Content", resourceCulture);

	public static string ElevateConfirmDialog_Title => ResourceManager.GetString("ElevateConfirmDialog.Title", resourceCulture);

	public static string EmptyFolder_Text => ResourceManager.GetString("EmptyFolder.Text", resourceCulture);

	public static string EmptyRecycleBin => ResourceManager.GetString("EmptyRecycleBin", resourceCulture);

	public static string EmptyRecycleBinDescription => ResourceManager.GetString("EmptyRecycleBinDescription", resourceCulture);

	public static string EmptyShelfText => ResourceManager.GetString("EmptyShelfText", resourceCulture);

	public static string EnableOmnibar => ResourceManager.GetString("EnableOmnibar", resourceCulture);

	public static string Encoding => ResourceManager.GetString("Encoding", resourceCulture);

	public static string EncodingDetected => ResourceManager.GetString("EncodingDetected", resourceCulture);

	public static string Encryption => ResourceManager.GetString("Encryption", resourceCulture);

	public static string EnterAnItemName => ResourceManager.GetString("EnterAnItemName", resourceCulture);

	public static string EnterCompactOverlay => ResourceManager.GetString("EnterCompactOverlay", resourceCulture);

	public static string EnterCompactOverlayDescription => ResourceManager.GetString("EnterCompactOverlayDescription", resourceCulture);

	public static string EnterDataStreamName => ResourceManager.GetString("EnterDataStreamName", resourceCulture);

	public static string EnterHashToCompare => ResourceManager.GetString("EnterHashToCompare", resourceCulture);

	public static string EnterName => ResourceManager.GetString("EnterName", resourceCulture);

	public static string ErrorApplyingTagContent => ResourceManager.GetString("ErrorApplyingTagContent", resourceCulture);

	public static string ErrorApplyingTagTitle => ResourceManager.GetString("ErrorApplyingTagTitle", resourceCulture);

	public static string ErrorCreatingDataStreamDescription => ResourceManager.GetString("ErrorCreatingDataStreamDescription", resourceCulture);

	public static string ErrorCreatingDataStreamTitle => ResourceManager.GetString("ErrorCreatingDataStreamTitle", resourceCulture);

	public static string ErrorDialogIsASubfolder => ResourceManager.GetString("ErrorDialogIsASubfolder", resourceCulture);

	public static string ErrorDialogNameNotAllowed => ResourceManager.GetString("ErrorDialogNameNotAllowed", resourceCulture);

	public static string ErrorDialogTheDestinationFolder => ResourceManager.GetString("ErrorDialogTheDestinationFolder", resourceCulture);

	public static string ErrorDialogThisActionCannotBeDone => ResourceManager.GetString("ErrorDialogThisActionCannotBeDone", resourceCulture);

	public static string ErrorDialogUnsupportedMoveOperation => ResourceManager.GetString("ErrorDialogUnsupportedMoveOperation", resourceCulture);

	public static string ErrorDialogUnsupportedOperation => ResourceManager.GetString("ErrorDialogUnsupportedOperation", resourceCulture);

	public static string ErrorInputEmpty => ResourceManager.GetString("ErrorInputEmpty", resourceCulture);

	public static string ErrorNameInputRestricted => ResourceManager.GetString("ErrorNameInputRestricted", resourceCulture);

	public static string ErrorNameInputRestrictedCharacters => ResourceManager.GetString("ErrorNameInputRestrictedCharacters", resourceCulture);

	public static string ExceptionNotificationBody => ResourceManager.GetString("ExceptionNotificationBody", resourceCulture);

	public static string ExceptionNotificationHeader => ResourceManager.GetString("ExceptionNotificationHeader", resourceCulture);

	public static string ExceptionNotificationReportButton => ResourceManager.GetString("ExceptionNotificationReportButton", resourceCulture);

	public static string ExitCompactOverlay => ResourceManager.GetString("ExitCompactOverlay", resourceCulture);

	public static string ExitCompactOverlayDescription => ResourceManager.GetString("ExitCompactOverlayDescription", resourceCulture);

	public static string ExperimentalFeatureFlags => ResourceManager.GetString("ExperimentalFeatureFlags", resourceCulture);

	public static string ExportSettings => ResourceManager.GetString("ExportSettings", resourceCulture);

	public static string Extract => ResourceManager.GetString("Extract", resourceCulture);

	public static string ExtractArchive => ResourceManager.GetString("ExtractArchive", resourceCulture);

	public static string ExtractFiles => ResourceManager.GetString("ExtractFiles", resourceCulture);

	public static string ExtractHere => ResourceManager.GetString("ExtractHere", resourceCulture);

	public static string ExtractHereSmart => ResourceManager.GetString("ExtractHereSmart", resourceCulture);

	public static string ExtractingArchiveText => ResourceManager.GetString("ExtractingArchiveText", resourceCulture);

	public static string ExtractingCompleteText => ResourceManager.GetString("ExtractingCompleteText", resourceCulture);

	public static string ExtractKeyboardAcceleratorTextOverride => ResourceManager.GetString("ExtractKeyboardAcceleratorTextOverride", resourceCulture);

	public static string ExtractToChildFolder => ResourceManager.GetString("ExtractToChildFolder", resourceCulture);

	public static string ExtractToPath => ResourceManager.GetString("ExtractToPath", resourceCulture);

	public static string ExtraLarge => ResourceManager.GetString("ExtraLarge", resourceCulture);

	public static string FaildToShareItems => ResourceManager.GetString("FaildToShareItems", resourceCulture);

	public static string FailedToOpenSettingsFile => ResourceManager.GetString("FailedToOpenSettingsFile", resourceCulture);

	public static string FailedToRestore => ResourceManager.GetString("FailedToRestore", resourceCulture);

	public static string FailedToRotateImage => ResourceManager.GetString("FailedToRotateImage", resourceCulture);

	public static string FailedToSetBackground => ResourceManager.GetString("FailedToSetBackground", resourceCulture);

	public static string FAT => ResourceManager.GetString("FAT", resourceCulture);

	public static string Feedback => ResourceManager.GetString("Feedback", resourceCulture);

	public static string File => ResourceManager.GetString("File", resourceCulture);

	public static string FileBrowserThumbnailIconColumnGrid_AutomationProperties_Name => ResourceManager.GetString("FileBrowserThumbnailIconColumnGrid.AutomationProperties.Name", resourceCulture);

	public static string FileInUseByDialog_Text => ResourceManager.GetString("FileInUseByDialog.Text", resourceCulture);

	public static string FileInUseDialog_Text => ResourceManager.GetString("FileInUseDialog.Text", resourceCulture);

	public static string FileInUseDialog_Title => ResourceManager.GetString("FileInUseDialog.Title", resourceCulture);

	public static string FileNameTeachingTip_Subtitle => ResourceManager.GetString("FileNameTeachingTip.Subtitle", resourceCulture);

	public static string FileNotFoundDialog_Text => ResourceManager.GetString("FileNotFoundDialog.Text", resourceCulture);

	public static string FileNotFoundDialog_Title => ResourceManager.GetString("FileNotFoundDialog.Title", resourceCulture);

	public static string FilePropertiesCannotBeCopied => ResourceManager.GetString("FilePropertiesCannotBeCopied", resourceCulture);

	public static string FilePropertiesCannotBeMoved => ResourceManager.GetString("FilePropertiesCannotBeMoved", resourceCulture);

	public static string FilesAndFolders => ResourceManager.GetString("FilesAndFolders", resourceCulture);

	public static string FilesRunningAsAdmin => ResourceManager.GetString("FilesRunningAsAdmin", resourceCulture);

	public static string FilesRunningAsAdminContent => ResourceManager.GetString("FilesRunningAsAdminContent", resourceCulture);

	public static string FileTags => ResourceManager.GetString("FileTags", resourceCulture);

	public static string Fill => ResourceManager.GetString("Fill", resourceCulture);

	public static string Flatten => ResourceManager.GetString("Flatten", resourceCulture);

	public static string FlattenFolder => ResourceManager.GetString("FlattenFolder", resourceCulture);

	public static string FlattenFolderDescription => ResourceManager.GetString("FlattenFolderDescription", resourceCulture);

	public static string FlattenFolderDialogContent => ResourceManager.GetString("FlattenFolderDialogContent", resourceCulture);

	public static string FocusOtherPane => ResourceManager.GetString("FocusOtherPane", resourceCulture);

	public static string FocusOtherPaneDescription => ResourceManager.GetString("FocusOtherPaneDescription", resourceCulture);

	public static string Folder => ResourceManager.GetString("Folder", resourceCulture);

	public static string FolderNotFoundDialog_Text => ResourceManager.GetString("FolderNotFoundDialog.Text", resourceCulture);

	public static string FolderNotFoundDialog_Title => ResourceManager.GetString("FolderNotFoundDialog.Title", resourceCulture);

	public static string FolderPath => ResourceManager.GetString("FolderPath", resourceCulture);

	public static string FolderWidgetCreateNewLibraryDialogTitleText => ResourceManager.GetString("FolderWidgetCreateNewLibraryDialogTitleText", resourceCulture);

	public static string FolderWidgetCreateNewLibraryInputPlaceholderText => ResourceManager.GetString("FolderWidgetCreateNewLibraryInputPlaceholderText", resourceCulture);

	public static string Format => ResourceManager.GetString("Format", resourceCulture);

	public static string FormatDrive => ResourceManager.GetString("FormatDrive", resourceCulture);

	public static string FormatDriveDescription => ResourceManager.GetString("FormatDriveDescription", resourceCulture);

	public static string FormatDriveText => ResourceManager.GetString("FormatDriveText", resourceCulture);

	public static string Forward => ResourceManager.GetString("Forward", resourceCulture);

	public static string FullScreen => ResourceManager.GetString("FullScreen", resourceCulture);

	public static string Tools => ResourceManager.GetString("Tools", resourceCulture);

	public static string ExitFullScreen => ResourceManager.GetString("ExitFullScreen", resourceCulture);

	public static string Future => ResourceManager.GetString("Future", resourceCulture);

	public static string General => ResourceManager.GetString("General", resourceCulture);

	public static string GenerateNewName => ResourceManager.GetString("GenerateNewName", resourceCulture);

	public static string GigaByteSymbol => ResourceManager.GetString("GigaByteSymbol", resourceCulture);

	public static string Git => ResourceManager.GetString("Git", resourceCulture);

	public static string GitCloneDescription => ResourceManager.GetString("GitCloneDescription", resourceCulture);

	public static string GitCurrentBranch => ResourceManager.GetString("GitCurrentBranch", resourceCulture);

	public static string GitDeleteBranch => ResourceManager.GetString("GitDeleteBranch", resourceCulture);

	public static string GitDeleteBranchSubtitle => ResourceManager.GetString("GitDeleteBranchSubtitle", resourceCulture);

	public static string GitError => ResourceManager.GetString("GitError", resourceCulture);

	public static string GitFetch => ResourceManager.GetString("GitFetch", resourceCulture);

	public static string GitFetchDescription => ResourceManager.GetString("GitFetchDescription", resourceCulture);

	public static string GitOriginRepositoryName => ResourceManager.GetString("GitOriginRepositoryName", resourceCulture);

	public static string GitPull => ResourceManager.GetString("GitPull", resourceCulture);

	public static string GitPullDescription => ResourceManager.GetString("GitPullDescription", resourceCulture);

	public static string GitPushDescription => ResourceManager.GetString("GitPushDescription", resourceCulture);

	public static string GitRepos => ResourceManager.GetString("GitRepos", resourceCulture);

	public static string GitStatus => ResourceManager.GetString("GitStatus", resourceCulture);

	public static string GitSync => ResourceManager.GetString("GitSync", resourceCulture);

	public static string GitSyncDescription => ResourceManager.GetString("GitSyncDescription", resourceCulture);

	public static string GitSyncStatusExtendedInfo => ResourceManager.GetString("GitSyncStatusExtendedInfo", resourceCulture);

	public static string Gray => ResourceManager.GetString("Gray", resourceCulture);

	public static string GrayDark => ResourceManager.GetString("GrayDark", resourceCulture);

	public static string Green => ResourceManager.GetString("Green", resourceCulture);

	public static string Grid => ResourceManager.GetString("Grid", resourceCulture);

	public static string GroupAscendingDescription => ResourceManager.GetString("GroupAscendingDescription", resourceCulture);

	public static string GroupBy => ResourceManager.GetString("GroupBy", resourceCulture);

	public static string GroupByDateCreatedDayDescription => ResourceManager.GetString("GroupByDateCreatedDayDescription", resourceCulture);

	public static string GroupByDateCreatedDescription => ResourceManager.GetString("GroupByDateCreatedDescription", resourceCulture);

	public static string GroupByDateCreatedMonthDescription => ResourceManager.GetString("GroupByDateCreatedMonthDescription", resourceCulture);

	public static string GroupByDateCreatedYearDescription => ResourceManager.GetString("GroupByDateCreatedYearDescription", resourceCulture);

	public static string GroupByDateDeletedDayDescription => ResourceManager.GetString("GroupByDateDeletedDayDescription", resourceCulture);

	public static string GroupByDateDeletedDescription => ResourceManager.GetString("GroupByDateDeletedDescription", resourceCulture);

	public static string GroupByDateDeletedMonthDescription => ResourceManager.GetString("GroupByDateDeletedMonthDescription", resourceCulture);

	public static string GroupByDateDeletedYearDescription => ResourceManager.GetString("GroupByDateDeletedYearDescription", resourceCulture);

	public static string GroupByDateModifiedDayDescription => ResourceManager.GetString("GroupByDateModifiedDayDescription", resourceCulture);

	public static string GroupByDateModifiedDescription => ResourceManager.GetString("GroupByDateModifiedDescription", resourceCulture);

	public static string GroupByDateModifiedMonthDescription => ResourceManager.GetString("GroupByDateModifiedMonthDescription", resourceCulture);

	public static string GroupByDateModifiedYearDescription => ResourceManager.GetString("GroupByDateModifiedYearDescription", resourceCulture);

	public static string GroupByDateUnit => ResourceManager.GetString("GroupByDateUnit", resourceCulture);

	public static string GroupByFolderPathDescription => ResourceManager.GetString("GroupByFolderPathDescription", resourceCulture);

	public static string GroupByMonthDescription => ResourceManager.GetString("GroupByMonthDescription", resourceCulture);

	public static string GroupByNameDescription => ResourceManager.GetString("GroupByNameDescription", resourceCulture);

	public static string GroupByNoneDescription => ResourceManager.GetString("GroupByNoneDescription", resourceCulture);

	public static string GroupByOriginalFolderDescription => ResourceManager.GetString("GroupByOriginalFolderDescription", resourceCulture);

	public static string GroupBySizeDescription => ResourceManager.GetString("GroupBySizeDescription", resourceCulture);

	public static string GroupBySyncStatusDescription => ResourceManager.GetString("GroupBySyncStatusDescription", resourceCulture);

	public static string GroupByTagDescription => ResourceManager.GetString("GroupByTagDescription", resourceCulture);

	public static string GroupByTypeDescription => ResourceManager.GetString("GroupByTypeDescription", resourceCulture);

	public static string GroupByYearDescription => ResourceManager.GetString("GroupByYearDescription", resourceCulture);

	public static string GroupDescendingDescription => ResourceManager.GetString("GroupDescendingDescription", resourceCulture);

	public static string GroupInDescendingOrder => ResourceManager.GetString("GroupInDescendingOrder", resourceCulture);

	public static string GroupItemsCount_Plural => ResourceManager.GetString("GroupItemsCount_Plural", resourceCulture);

	public static string GroupItemsCount_Singular => ResourceManager.GetString("GroupItemsCount_Singular", resourceCulture);

	public static string ItemsSelectedCountFormat => ResourceManager.GetString("ItemsSelectedCountFormat", resourceCulture);

	public static string Hashes => ResourceManager.GetString("Hashes", resourceCulture);

	public static string HashesDoNotMatch => ResourceManager.GetString("HashesDoNotMatch", resourceCulture);

	public static string HashesMatch => ResourceManager.GetString("HashesMatch", resourceCulture);

	public static string HashValue => ResourceManager.GetString("HashValue", resourceCulture);

	public static string Height => ResourceManager.GetString("Height", resourceCulture);

	public static string Hello => ResourceManager.GetString("Hello", resourceCulture);

	public static string Help => ResourceManager.GetString("Help", resourceCulture);

	public static string HelpAndSupport => ResourceManager.GetString("HelpAndSupport", resourceCulture);

	public static string Hidden => ResourceManager.GetString("Hidden", resourceCulture);

	public static string HiddenItems => ResourceManager.GetString("HiddenItems", resourceCulture);

	public static string ShowHiddenItems => ResourceManager.GetString("ShowHiddenItems", resourceCulture);

	public static string HigherPermissionsRequired => ResourceManager.GetString("HigherPermissionsRequired", resourceCulture);

	public static string Home => ResourceManager.GetString("Home", resourceCulture);

	public static string Horizontal => ResourceManager.GetString("Horizontal", resourceCulture);

	public static string HorizontalAlignment => ResourceManager.GetString("HorizontalAlignment", resourceCulture);

	public static string HorizontalMultitaskingControlMoveTabToNewWindow_Text => ResourceManager.GetString("HorizontalMultitaskingControlMoveTabToNewWindow.Text", resourceCulture);

	public static string HoursAgo => ResourceManager.GetString("HoursAgo", resourceCulture);

	public static string HoursAgo_Plural => ResourceManager.GetString("HoursAgo_Plural", resourceCulture);

	public static string HoursAgo_Singular => ResourceManager.GetString("HoursAgo_Singular", resourceCulture);

	public static string IclFileCapitalized => ResourceManager.GetString("IclFileCapitalized", resourceCulture);

	public static string IcoFileCapitalized => ResourceManager.GetString("IcoFileCapitalized", resourceCulture);

	public static string IconFiles => ResourceManager.GetString("IconFiles", resourceCulture);

	public static string IDENotLocatedContent => ResourceManager.GetString("IDENotLocatedContent", resourceCulture);

	public static string IDENotLocatedTitle => ResourceManager.GetString("IDENotLocatedTitle", resourceCulture);

	public static string IDEPathPlaceholder => ResourceManager.GetString("IDEPathPlaceholder", resourceCulture);

	public static string ImageFit => ResourceManager.GetString("ImageFit", resourceCulture);

	public static string ImportSettings => ResourceManager.GetString("ImportSettings", resourceCulture);

	public static string ImproveTranslation => ResourceManager.GetString("ImproveTranslation", resourceCulture);

	public static string IncreaseSize => ResourceManager.GetString("IncreaseSize", resourceCulture);

	public static string Inherited => ResourceManager.GetString("Inherited", resourceCulture);

	public static string InitRepo => ResourceManager.GetString("InitRepo", resourceCulture);

	public static string InitRepoDescription => ResourceManager.GetString("InitRepoDescription", resourceCulture);

	public static string InsertDiscDialog_OpenDriveButton => ResourceManager.GetString("InsertDiscDialog.OpenDriveButton", resourceCulture);

	public static string InsertDiscDialog_Text => ResourceManager.GetString("InsertDiscDialog.Text", resourceCulture);

	public static string InsertDiscDialog_Title => ResourceManager.GetString("InsertDiscDialog.Title", resourceCulture);

	public static string Install => ResourceManager.GetString("Install", resourceCulture);

	public static string InstallCertificate => ResourceManager.GetString("InstallCertificate", resourceCulture);

	public static string InstallCertificateDescription => ResourceManager.GetString("InstallCertificateDescription", resourceCulture);

	public static string InstallDriver => ResourceManager.GetString("InstallDriver", resourceCulture);

	public static string InstallFont => ResourceManager.GetString("InstallFont", resourceCulture);

	public static string InstallFontDescription => ResourceManager.GetString("InstallFontDescription", resourceCulture);

	public static string InstallInfDriverDescription => ResourceManager.GetString("InstallInfDriverDescription", resourceCulture);

	public static string InvalidBranchName => ResourceManager.GetString("InvalidBranchName", resourceCulture);

	public static string InvalidCommand => ResourceManager.GetString("InvalidCommand", resourceCulture);

	public static string InvalidCommandContent => ResourceManager.GetString("InvalidCommandContent", resourceCulture);

	public static string InvalidFilename_Text => ResourceManager.GetString("InvalidFilename.Text", resourceCulture);

	public static string InvalidIDENameError => ResourceManager.GetString("InvalidIDENameError", resourceCulture);

	public static string InvalidItemDialogContent => ResourceManager.GetString("InvalidItemDialogContent", resourceCulture);

	public static string InvalidItemDialogTitle => ResourceManager.GetString("InvalidItemDialogTitle", resourceCulture);

	public static string InvalidLocation => ResourceManager.GetString("InvalidLocation", resourceCulture);

	public static string InvalidPath => ResourceManager.GetString("InvalidPath", resourceCulture);

	public static string InvalidTagNameWarning => ResourceManager.GetString("InvalidTagNameWarning", resourceCulture);

	public static string InvertSelection => ResourceManager.GetString("InvertSelection", resourceCulture);

	public static string InvertSelectionDescription => ResourceManager.GetString("InvertSelectionDescription", resourceCulture);

	public static string IrisPastel => ResourceManager.GetString("IrisPastel", resourceCulture);

	public static string ItemAlreadyExistsDialogContent => ResourceManager.GetString("ItemAlreadyExistsDialogContent", resourceCulture);

	public static string ItemAlreadyExistsDialogSecondaryButtonText => ResourceManager.GetString("ItemAlreadyExistsDialogSecondaryButtonText", resourceCulture);

	public static string ItemAlreadyExistsDialogTitle => ResourceManager.GetString("ItemAlreadyExistsDialogTitle", resourceCulture);

	public static string ItemRenameFailed => ResourceManager.GetString("ItemRenameFailed", resourceCulture);

	public static string Items => ResourceManager.GetString("Items", resourceCulture);

	public static string ItemSizeBytes => ResourceManager.GetString("ItemSizeBytes", resourceCulture);

	public static string ItemSizeNotCalculated => ResourceManager.GetString("ItemSizeNotCalculated", resourceCulture);

	public static string ItemSizeText_Huge => ResourceManager.GetString("ItemSizeText_Huge", resourceCulture);

	public static string ItemSizeText_Large => ResourceManager.GetString("ItemSizeText_Large", resourceCulture);

	public static string ItemSizeText_Medium => ResourceManager.GetString("ItemSizeText_Medium", resourceCulture);

	public static string ItemSizeText_Small => ResourceManager.GetString("ItemSizeText_Small", resourceCulture);

	public static string ItemSizeText_Tiny => ResourceManager.GetString("ItemSizeText_Tiny", resourceCulture);

	public static string ItemSizeText_VeryLarge => ResourceManager.GetString("ItemSizeText_VeryLarge", resourceCulture);

	public static string ItemsLower => ResourceManager.GetString("ItemsLower", resourceCulture);

	public static string ItemType => ResourceManager.GetString("ItemType", resourceCulture);

	public static string JumpListPinnedGroupHeader => ResourceManager.GetString("JumpListPinnedGroupHeader", resourceCulture);

	public static string JumpListRecentGroupHeader => ResourceManager.GetString("JumpListRecentGroupHeader", resourceCulture);

	public static string Key_Application => ResourceManager.GetString("Key.Application", resourceCulture);

	public static string Key_Application1 => ResourceManager.GetString("Key.Application1", resourceCulture);

	public static string Key_Application2 => ResourceManager.GetString("Key.Application2", resourceCulture);

	public static string Key_Back => ResourceManager.GetString("Key.Back", resourceCulture);

	public static string Key_BrowserFavorites => ResourceManager.GetString("Key.BrowserFavorites", resourceCulture);

	public static string Key_BrowserGoBack => ResourceManager.GetString("Key.BrowserGoBack", resourceCulture);

	public static string Key_BrowserGoForward => ResourceManager.GetString("Key.BrowserGoForward", resourceCulture);

	public static string Key_BrowserGoHome => ResourceManager.GetString("Key.BrowserGoHome", resourceCulture);

	public static string Key_BrowserRefresh => ResourceManager.GetString("Key.BrowserRefresh", resourceCulture);

	public static string Key_BrowserSearch => ResourceManager.GetString("Key.BrowserSearch", resourceCulture);

	public static string Key_BrowserStop => ResourceManager.GetString("Key.BrowserStop", resourceCulture);

	public static string Key_Clear => ResourceManager.GetString("Key.Clear", resourceCulture);

	public static string Key_Control => ResourceManager.GetString("Key.Control", resourceCulture);

	public static string Key_Delete => ResourceManager.GetString("Key.Delete", resourceCulture);

	public static string Key_Down => ResourceManager.GetString("Key.Down", resourceCulture);

	public static string Key_End => ResourceManager.GetString("Key.End", resourceCulture);

	public static string Key_Enter => ResourceManager.GetString("Key.Enter", resourceCulture);

	public static string Key_Escape => ResourceManager.GetString("Key.Escape", resourceCulture);

	public static string Key_Help => ResourceManager.GetString("Key.Help", resourceCulture);

	public static string Key_Home => ResourceManager.GetString("Key.Home", resourceCulture);

	public static string Key_Insert => ResourceManager.GetString("Key.Insert", resourceCulture);

	public static string Key_Left => ResourceManager.GetString("Key.Left", resourceCulture);

	public static string Key_Mail => ResourceManager.GetString("Key.Mail", resourceCulture);

	public static string Key_MediaMute => ResourceManager.GetString("Key.MediaMute", resourceCulture);

	public static string Key_MediaNextTrack => ResourceManager.GetString("Key.MediaNextTrack", resourceCulture);

	public static string Key_MediaPlayPause => ResourceManager.GetString("Key.MediaPlayPause", resourceCulture);

	public static string Key_MediaPreviousTrack => ResourceManager.GetString("Key.MediaPreviousTrack", resourceCulture);

	public static string Key_MediaSelect => ResourceManager.GetString("Key.MediaSelect", resourceCulture);

	public static string Key_MediaStop => ResourceManager.GetString("Key.MediaStop", resourceCulture);

	public static string Key_MediaVolumeDown => ResourceManager.GetString("Key.MediaVolumeDown", resourceCulture);

	public static string Key_MediaVolumeUp => ResourceManager.GetString("Key.MediaVolumeUp", resourceCulture);

	public static string Key_Menu => ResourceManager.GetString("Key.Menu", resourceCulture);

	public static string Key_Mouse4 => ResourceManager.GetString("Key.Mouse4", resourceCulture);

	public static string Key_Mouse5 => ResourceManager.GetString("Key.Mouse5", resourceCulture);

	public static string Key_PageDown => ResourceManager.GetString("Key.PageDown", resourceCulture);

	public static string Key_PageUp => ResourceManager.GetString("Key.PageUp", resourceCulture);

	public static string Key_Pause => ResourceManager.GetString("Key.Pause", resourceCulture);

	public static string Key_Print => ResourceManager.GetString("Key.Print", resourceCulture);

	public static string Key_Right => ResourceManager.GetString("Key.Right", resourceCulture);

	public static string Key_Separator => ResourceManager.GetString("Key.Separator", resourceCulture);

	public static string Key_Shift => ResourceManager.GetString("Key.Shift", resourceCulture);

	public static string Key_Sleep => ResourceManager.GetString("Key.Sleep", resourceCulture);

	public static string Key_Space => ResourceManager.GetString("Key.Space", resourceCulture);

	public static string Key_Tab => ResourceManager.GetString("Key.Tab", resourceCulture);

	public static string Key_Up => ResourceManager.GetString("Key.Up", resourceCulture);

	public static string Key_Windows => ResourceManager.GetString("Key.Windows", resourceCulture);

	public static string KeybindingAlreadyUsedNotification => ResourceManager.GetString("KeybindingAlreadyUsedNotification", resourceCulture);

	public static string KeybindingInvalidKeyNotification => ResourceManager.GetString("KeybindingInvalidKeyNotification", resourceCulture);

	public static string KiloByteSymbol => ResourceManager.GetString("KiloByteSymbol", resourceCulture);

	public static string Language => ResourceManager.GetString("Language", resourceCulture);

	public static string Large => ResourceManager.GetString("Large", resourceCulture);

	public static string Large_ => ResourceManager.GetString("Large+", resourceCulture);

	public static string Large__ => ResourceManager.GetString("Large++", resourceCulture);

	public static string Large___ => ResourceManager.GetString("Large+++", resourceCulture);

	public static string LastMonth => ResourceManager.GetString("LastMonth", resourceCulture);

	public static string LastWeek => ResourceManager.GetString("LastWeek", resourceCulture);

	public static string LastYear => ResourceManager.GetString("LastYear", resourceCulture);

	public static string LaunchPreviewPopup => ResourceManager.GetString("LaunchPreviewPopup", resourceCulture);

	public static string LaunchPreviewPopupDescription => ResourceManager.GetString("LaunchPreviewPopupDescription", resourceCulture);

	public static string Layout => ResourceManager.GetString("Layout", resourceCulture);

	public static string LayoutAdaptiveDescription => ResourceManager.GetString("LayoutAdaptiveDescription", resourceCulture);

	public static string LayoutCardsDescription => ResourceManager.GetString("LayoutCardsDescription", resourceCulture);

	public static string LayoutColumnsDescription => ResourceManager.GetString("LayoutColumnsDescription", resourceCulture);

	public static string LayoutDecreaseSizeDescription => ResourceManager.GetString("LayoutDecreaseSizeDescription", resourceCulture);

	public static string LayoutDetailsDescription => ResourceManager.GetString("LayoutDetailsDescription", resourceCulture);

	public static string LayoutGridDescription => ResourceManager.GetString("LayoutGridDescription", resourceCulture);

	public static string LayoutIncreaseSizeDescription => ResourceManager.GetString("LayoutIncreaseSizeDescription", resourceCulture);

	public static string LayoutListDescription => ResourceManager.GetString("LayoutListDescription", resourceCulture);

	public static string LayoutType => ResourceManager.GetString("LayoutType", resourceCulture);

	public static string Left => ResourceManager.GetString("Left", resourceCulture);

	public static string Library => ResourceManager.GetString("Library", resourceCulture);

	public static string LibraryLocations_Text => ResourceManager.GetString("LibraryLocations.Text", resourceCulture);

	public static string LibraryNoLocations_Text => ResourceManager.GetString("LibraryNoLocations.Text", resourceCulture);

	public static string Auto => ResourceManager.GetString("Auto", resourceCulture);

	public static string LightTheme => ResourceManager.GetString("LightTheme", resourceCulture);

	public static string LinkToFolderCaptionText => ResourceManager.GetString("LinkToFolderCaptionText", resourceCulture);

	public static string List => ResourceManager.GetString("List", resourceCulture);

	public static string Loading => ResourceManager.GetString("Loading", resourceCulture);

	public static string Locals => ResourceManager.GetString("Locals", resourceCulture);

	public static string Location => ResourceManager.GetString("Location", resourceCulture);

	public static string Lockscreen => ResourceManager.GetString("Lockscreen", resourceCulture);

	public static string Login => ResourceManager.GetString("Login", resourceCulture);

	public static string Logout => ResourceManager.GetString("Logout", resourceCulture);

	public static string ManageBitLocker => ResourceManager.GetString("ManageBitLocker", resourceCulture);

	public static string ManageBranches => ResourceManager.GetString("ManageBranches", resourceCulture);

	public static string ManageTags => ResourceManager.GetString("ManageTags", resourceCulture);

	public static string Maximized => ResourceManager.GetString("Maximized", resourceCulture);

	public static string Medium => ResourceManager.GetString("Medium", resourceCulture);

	public static string Medium_ => ResourceManager.GetString("Medium+", resourceCulture);

	public static string Medium__ => ResourceManager.GetString("Medium++", resourceCulture);

	public static string Medium___ => ResourceManager.GetString("Medium+++", resourceCulture);

	public static string Medium____ => ResourceManager.GetString("Medium++++", resourceCulture);

	public static string Medium_____ => ResourceManager.GetString("Medium+++++", resourceCulture);

	public static string MegaByteSymbol => ResourceManager.GetString("MegaByteSymbol", resourceCulture);

	public static string Mica => ResourceManager.GetString("Mica", resourceCulture);

	public static string MicaAlt => ResourceManager.GetString("MicaAlt", resourceCulture);

	public static string Minimized => ResourceManager.GetString("Minimized", resourceCulture);

	public static string MintLight => ResourceManager.GetString("MintLight", resourceCulture);

	public static string MinutesAgo => ResourceManager.GetString("MinutesAgo", resourceCulture);

	public static string MinutesAgo_Plural => ResourceManager.GetString("MinutesAgo_Plural", resourceCulture);

	public static string MinutesAgo_Singular => ResourceManager.GetString("MinutesAgo_Singular", resourceCulture);

	public static string Modified => ResourceManager.GetString("Modified", resourceCulture);

	public static string Modify => ResourceManager.GetString("Modify", resourceCulture);

	public static string ModRed => ResourceManager.GetString("ModRed", resourceCulture);

	public static string Month => ResourceManager.GetString("Month", resourceCulture);

	public static string MoreDetails => ResourceManager.GetString("MoreDetails", resourceCulture);

	public static string MoreOptions => ResourceManager.GetString("MoreOptions", resourceCulture);

	public static string MoveFileWithoutProperties => ResourceManager.GetString("MoveFileWithoutProperties", resourceCulture);

	public static string MoveItemsDialogPrimaryButtonText => ResourceManager.GetString("MoveItemsDialogPrimaryButtonText", resourceCulture);

	public static string MoveItemsDialogSubtitle => ResourceManager.GetString("MoveItemsDialogSubtitle", resourceCulture);

	public static string MoveItemsDialogTitle => ResourceManager.GetString("MoveItemsDialogTitle", resourceCulture);

	public static string MoveToFolderCaptionText => ResourceManager.GetString("MoveToFolderCaptionText", resourceCulture);

	public static string MultipleValues => ResourceManager.GetString("MultipleValues", resourceCulture);

	public static string MultiSelect => ResourceManager.GetString("MultiSelect", resourceCulture);

	public static string Music => ResourceManager.GetString("Music", resourceCulture);

	public static string Name => ResourceManager.GetString("Name", resourceCulture);

	public static string NameWithColon => ResourceManager.GetString("NameWithColon", resourceCulture);

	public static string NavigateBackDescription => ResourceManager.GetString("NavigateBackDescription", resourceCulture);

	public static string NavigateForwardDescription => ResourceManager.GetString("NavigateForwardDescription", resourceCulture);

	public static string NavigateHomeDescription => ResourceManager.GetString("NavigateHomeDescription", resourceCulture);

	public static string NavigateUpDescription => ResourceManager.GetString("NavigateUpDescription", resourceCulture);

	public static string NavigationToolbarNewPane_Label => ResourceManager.GetString("NavigationToolbarNewPane.Label", resourceCulture);

	public static string NavigationToolbarVisiblePath_PlaceholderText => ResourceManager.GetString("NavigationToolbarVisiblePath.PlaceholderText", resourceCulture);

	public static string NavigationToolbarVisiblePathNoResults => ResourceManager.GetString("NavigationToolbarVisiblePathNoResults", resourceCulture);

	public static string NavResfreshButton_AutomationProperties_Name => ResourceManager.GetString("NavResfreshButton.AutomationProperties.Name", resourceCulture);

	public static string NavToolbarDetails_ToolTipService_ToolTip => ResourceManager.GetString("NavToolbarDetails.ToolTipService.ToolTip", resourceCulture);

	public static string NavToolbarSelectionOptions_Label => ResourceManager.GetString("NavToolbarSelectionOptions.Label", resourceCulture);

	public static string Network => ResourceManager.GetString("Network", resourceCulture);

	public static string NetworkAuthenticationDialogMessage => ResourceManager.GetString("NetworkAuthenticationDialogMessage", resourceCulture);

	public static string NetworkAuthenticationDialogTitle => ResourceManager.GetString("NetworkAuthenticationDialogTitle", resourceCulture);

	public static string NetworkAuthenticationSaveCheckbox => ResourceManager.GetString("NetworkAuthenticationSaveCheckbox", resourceCulture);

	public static string NetworkFolderErrorDialogTitle => ResourceManager.GetString("NetworkFolderErrorDialogTitle", resourceCulture);

	public static string NetworkLocations => ResourceManager.GetString("NetworkLocations", resourceCulture);

	public static string Never => ResourceManager.GetString("Never", resourceCulture);

	public static string NewBranch => ResourceManager.GetString("NewBranch", resourceCulture);

	public static string NewFile => ResourceManager.GetString("NewFile", resourceCulture);

	public static string NewFolder => ResourceManager.GetString("NewFolder", resourceCulture);

	public static string NewShortcutDialogDescription => ResourceManager.GetString("NewShortcutDialogDescription", resourceCulture);

	public static string NewShortcutDialogPrompt => ResourceManager.GetString("NewShortcutDialogPrompt", resourceCulture);

	public static string NewShortcutDialogTitle => ResourceManager.GetString("NewShortcutDialogTitle", resourceCulture);

	public static string NewTab => ResourceManager.GetString("NewTab", resourceCulture);

	public static string NewTabDescription => ResourceManager.GetString("NewTabDescription", resourceCulture);

	public static string NewTag => ResourceManager.GetString("NewTag", resourceCulture);

	public static string NewWindow => ResourceManager.GetString("NewWindow", resourceCulture);

	public static string NewWindowDescription => ResourceManager.GetString("NewWindowDescription", resourceCulture);

	public static string NextTab => ResourceManager.GetString("NextTab", resourceCulture);

	public static string NextTabDescription => ResourceManager.GetString("NextTabDescription", resourceCulture);

	public static string No => ResourceManager.GetString("No", resourceCulture);

	public static string NoCommandsFound => ResourceManager.GetString("NoCommandsFound", resourceCulture);

	public static string NoFileOperations => ResourceManager.GetString("NoFileOperations", resourceCulture);

	public static string NoItemSelected => ResourceManager.GetString("NoItemSelected", resourceCulture);

	public static string None => ResourceManager.GetString("None", resourceCulture);

	public static string NoNetworkLocationsInfo => ResourceManager.GetString("NoNetworkLocationsInfo", resourceCulture);

	public static string NormalWindow => ResourceManager.GetString("NormalWindow", resourceCulture);

	public static string NoSearchResultsFound => ResourceManager.GetString("NoSearchResultsFound", resourceCulture);

	public static string Now => ResourceManager.GetString("Now", resourceCulture);

	public static string NumPadTypeName => ResourceManager.GetString("NumPadTypeName", resourceCulture);

	public static string OK => ResourceManager.GetString("OK", resourceCulture);

	public static string OmnibarCommandPaletteModeTextPlaceholder => ResourceManager.GetString("OmnibarCommandPaletteModeTextPlaceholder", resourceCulture);

	public static string OmnibarPathModeTextPlaceholder => ResourceManager.GetString("OmnibarPathModeTextPlaceholder", resourceCulture);

	public static string OmnibarSearchModeTextPlaceholder => ResourceManager.GetString("OmnibarSearchModeTextPlaceholder", resourceCulture);

	public static string OngoingTasksTeachingTip_Subtitle => ResourceManager.GetString("OngoingTasksTeachingTip.Subtitle", resourceCulture);

	public static string OngoingTasksTeachingTip_Title => ResourceManager.GetString("OngoingTasksTeachingTip.Title", resourceCulture);

	public static string Opacity => ResourceManager.GetString("Opacity", resourceCulture);

	public static string Open => ResourceManager.GetString("Open", resourceCulture);

	public static string OpenAllTaggedItems => ResourceManager.GetString("OpenAllTaggedItems", resourceCulture);

	public static string OpenAllTaggedItemsDescription => ResourceManager.GetString("OpenAllTaggedItemsDescription", resourceCulture);

	public static string OpenClassicProperties => ResourceManager.GetString("OpenClassicProperties", resourceCulture);

	public static string OpenClassicPropertiesDescription => ResourceManager.GetString("OpenClassicPropertiesDescription", resourceCulture);

	public static string OpenCommandPaletteDescription => ResourceManager.GetString("OpenCommandPaletteDescription", resourceCulture);

	public static string OpenDirectoryInNewPaneDescription => ResourceManager.GetString("OpenDirectoryInNewPaneDescription", resourceCulture);

	public static string OpenDirectoryInNewTabDescription => ResourceManager.GetString("OpenDirectoryInNewTabDescription", resourceCulture);

	public static string OpenFileLocation => ResourceManager.GetString("OpenFileLocation", resourceCulture);

	public static string OpenFileLocationDescription => ResourceManager.GetString("OpenFileLocationDescription", resourceCulture);

	public static string OpenFoldersInNewTab => ResourceManager.GetString("OpenFoldersInNewTab", resourceCulture);

	public static string OpenFolderWithOneClickColumnsLayout => ResourceManager.GetString("OpenFolderWithOneClickColumnsLayout", resourceCulture);

	public static string OpenGitHubRepo => ResourceManager.GetString("OpenGitHubRepo", resourceCulture);

	public static string OpenHelpDescription => ResourceManager.GetString("OpenHelpDescription", resourceCulture);

	public static string OpeningItems => ResourceManager.GetString("OpeningItems", resourceCulture);

	public static string OpenInIDE => ResourceManager.GetString("OpenInIDE", resourceCulture);

	public static string OpenInIDEDescription => ResourceManager.GetString("OpenInIDEDescription", resourceCulture);

	public static string OpenInNewPane => ResourceManager.GetString("OpenInNewPane", resourceCulture);

	public static string OpenInNewTab => ResourceManager.GetString("OpenInNewTab", resourceCulture);

	public static string OpenInNewWindow => ResourceManager.GetString("OpenInNewWindow", resourceCulture);

	public static string OpenInNewWindowDescription => ResourceManager.GetString("OpenInNewWindowDescription", resourceCulture);

	public static string OpenItemDescription => ResourceManager.GetString("OpenItemDescription", resourceCulture);

	public static string OpenItemWithApplicationPickerDescription => ResourceManager.GetString("OpenItemWithApplicationPickerDescription", resourceCulture);

	public static string OpenLogLocation => ResourceManager.GetString("OpenLogLocation", resourceCulture);

	public static string OpenOnStartupDisabled => ResourceManager.GetString("OpenOnStartupDisabled", resourceCulture);

	public static string OpenParentFolderDescription => ResourceManager.GetString("OpenParentFolderDescription", resourceCulture);

	public static string OpenProperties => ResourceManager.GetString("OpenProperties", resourceCulture);

	public static string OpenPropertiesDescription => ResourceManager.GetString("OpenPropertiesDescription", resourceCulture);

	public static string OpenRepoInIDE => ResourceManager.GetString("OpenRepoInIDE", resourceCulture);

	public static string OpenRepoInIDEDescription => ResourceManager.GetString("OpenRepoInIDEDescription", resourceCulture);

	public static string OpenSettings => ResourceManager.GetString("OpenSettings", resourceCulture);

	public static string OpenSettingsDescription => ResourceManager.GetString("OpenSettingsDescription", resourceCulture);

	public static string OpenSource => ResourceManager.GetString("OpenSource", resourceCulture);

	public static string OpenStorageSense => ResourceManager.GetString("OpenStorageSense", resourceCulture);

	public static string OpenStorageSenseDescription => ResourceManager.GetString("OpenStorageSenseDescription", resourceCulture);

	public static string OpenTabInExistingInstance => ResourceManager.GetString("OpenTabInExistingInstance", resourceCulture);

	public static string OpenTerminal => ResourceManager.GetString("OpenTerminal", resourceCulture);

	public static string OpenTerminalAsAdmin => ResourceManager.GetString("OpenTerminalAsAdmin", resourceCulture);

	public static string OpenTerminalAsAdminDescription => ResourceManager.GetString("OpenTerminalAsAdminDescription", resourceCulture);

	public static string OpenTerminalDescription => ResourceManager.GetString("OpenTerminalDescription", resourceCulture);

	public static string OpenWith => ResourceManager.GetString("OpenWith", resourceCulture);

	public static string OrangeBright => ResourceManager.GetString("OrangeBright", resourceCulture);

	public static string OriginalFolder => ResourceManager.GetString("OriginalFolder", resourceCulture);

	public static string OriginalPath => ResourceManager.GetString("OriginalPath", resourceCulture);

	public static string Overcast => ResourceManager.GetString("Overcast", resourceCulture);

	public static string Password => ResourceManager.GetString("Password", resourceCulture);

	public static string Paste => ResourceManager.GetString("Paste", resourceCulture);

	public static string PasteItemDescription => ResourceManager.GetString("PasteItemDescription", resourceCulture);

	public static string PasteItemToSelectionDescription => ResourceManager.GetString("PasteItemToSelectionDescription", resourceCulture);

	public static string PasteShortcut => ResourceManager.GetString("PasteShortcut", resourceCulture);

	public static string PasteShortcutDescription => ResourceManager.GetString("PasteShortcutDescription", resourceCulture);

	public static string PasteToSelectedFolder => ResourceManager.GetString("PasteToSelectedFolder", resourceCulture);

	public static string Path => ResourceManager.GetString("Path", resourceCulture);

	public static string PathColumn => ResourceManager.GetString("PathColumn", resourceCulture);

	public static string PathOrAlias => ResourceManager.GetString("PathOrAlias", resourceCulture);

	public static string PermanentDeletionOnly => ResourceManager.GetString("PermanentDeletionOnly", resourceCulture);

	public static string Permissions => ResourceManager.GetString("Permissions", resourceCulture);

	public static string PetaByteSymbol => ResourceManager.GetString("PetaByteSymbol", resourceCulture);

	public static string Pictures => ResourceManager.GetString("Pictures", resourceCulture);

	public static string PinFolderToSidebar => ResourceManager.GetString("PinFolderToSidebar", resourceCulture);

	public static string PinFolderToSidebarDescription => ResourceManager.GetString("PinFolderToSidebarDescription", resourceCulture);

	public static string PinItemToStart_Text => ResourceManager.GetString("PinItemToStart.Text", resourceCulture);

	public static string Pinned => ResourceManager.GetString("Pinned", resourceCulture);

	public static string PinToSidebarByDraggingCaptionText => ResourceManager.GetString("PinToSidebarByDraggingCaptionText", resourceCulture);

	public static string PinToStartDescription => ResourceManager.GetString("PinToStartDescription", resourceCulture);

	public static string PlayAll => ResourceManager.GetString("PlayAll", resourceCulture);

	public static string PlayAllDescription => ResourceManager.GetString("PlayAllDescription", resourceCulture);

	public static string Preview => ResourceManager.GetString("Preview", resourceCulture);

	public static string PreviewPaneFileDetails_AutomationProperties_Name => ResourceManager.GetString("PreviewPaneFileDetails.AutomationProperties.Name", resourceCulture);

	public static string PreviewPaneFilePreview_AutomationProperties_Name => ResourceManager.GetString("PreviewPaneFilePreview.AutomationProperties.Name", resourceCulture);

	public static string PreviewPaneLoadCloudItemButton_Text => ResourceManager.GetString("PreviewPaneLoadCloudItemButton.Text", resourceCulture);

	public static string PreviewPaneLoadCloudItemButton_ToolTipService_ToolTip => ResourceManager.GetString("PreviewPaneLoadCloudItemButton.ToolTipService.ToolTip", resourceCulture);

	public static string PreviousTab => ResourceManager.GetString("PreviousTab", resourceCulture);

	public static string PreviousTabDescription => ResourceManager.GetString("PreviousTabDescription", resourceCulture);

	public static string Principal => ResourceManager.GetString("Principal", resourceCulture);

	public static string Privacy => ResourceManager.GetString("Privacy", resourceCulture);

	public static string ProcessingItems => ResourceManager.GetString("ProcessingItems", resourceCulture);

	public static string Properties => ResourceManager.GetString("Properties", resourceCulture);

	public static string PropertiesCombinedItemLocation => ResourceManager.GetString("PropertiesCombinedItemLocation", resourceCulture);

	public static string PropertiesCreated_Text => ResourceManager.GetString("PropertiesCreated.Text", resourceCulture);

	public static string PropertiesDialogReadOnly_Text => ResourceManager.GetString("PropertiesDialogReadOnly.Text", resourceCulture);

	public static string PropertiesDriveCapacity_Text => ResourceManager.GetString("PropertiesDriveCapacity.Text", resourceCulture);

	public static string PropertiesDriveFileSystem_Text => ResourceManager.GetString("PropertiesDriveFileSystem.Text", resourceCulture);

	public static string PropertiesDriveFreeSpace_Text => ResourceManager.GetString("PropertiesDriveFreeSpace.Text", resourceCulture);

	public static string PropertiesDriveItemTypeDifferent => ResourceManager.GetString("PropertiesDriveItemTypeDifferent", resourceCulture);

	public static string PropertiesDriveItemTypesEquals => ResourceManager.GetString("PropertiesDriveItemTypesEquals", resourceCulture);

	public static string PropertiesDriveUsedSpace_Text => ResourceManager.GetString("PropertiesDriveUsedSpace.Text", resourceCulture);

	public static string PropertiesFilesAndFoldersAndLocationsCount => ResourceManager.GetString("PropertiesFilesAndFoldersAndLocationsCount", resourceCulture);

	public static string PropertiesFilesAndFoldersCountString => ResourceManager.GetString("PropertiesFilesAndFoldersCountString", resourceCulture);

	public static string PropertiesModified_Text => ResourceManager.GetString("PropertiesModified.Text", resourceCulture);

	public static string PropertiesShortcutItemArgs_Text => ResourceManager.GetString("PropertiesShortcutItemArgs.Text", resourceCulture);

	public static string PropertiesShortcutItemPath_Text => ResourceManager.GetString("PropertiesShortcutItemPath.Text", resourceCulture);

	public static string PropertiesShortcutItemType_Text => ResourceManager.GetString("PropertiesShortcutItemType.Text", resourceCulture);

	public static string PropertiesShortcutTypeLink => ResourceManager.GetString("PropertiesShortcutTypeLink", resourceCulture);

	public static string PropertyAlbumArtist => ResourceManager.GetString("PropertyAlbumArtist", resourceCulture);

	public static string PropertyAlbumTitle => ResourceManager.GetString("PropertyAlbumTitle", resourceCulture);

	public static string PropertyAltitude => ResourceManager.GetString("PropertyAltitude", resourceCulture);

	public static string PropertyAperture => ResourceManager.GetString("PropertyAperture", resourceCulture);

	public static string PropertyAuthorUrl => ResourceManager.GetString("PropertyAuthorUrl", resourceCulture);

	public static string PropertyBeatsPerMinute => ResourceManager.GetString("PropertyBeatsPerMinute", resourceCulture);

	public static string PropertyBitDepth => ResourceManager.GetString("PropertyBitDepth", resourceCulture);

	public static string PropertyCameraManufacturer => ResourceManager.GetString("PropertyCameraManufacturer", resourceCulture);

	public static string PropertyCameraModel => ResourceManager.GetString("PropertyCameraModel", resourceCulture);

	public static string PropertyChannelCount => ResourceManager.GetString("PropertyChannelCount", resourceCulture);

	public static string PropertyCharacterCount => ResourceManager.GetString("PropertyCharacterCount", resourceCulture);

	public static string PropertyColorSpace => ResourceManager.GetString("PropertyColorSpace", resourceCulture);

	public static string PropertyColorSpace_Value0 => ResourceManager.GetString("PropertyColorSpace_Value0", resourceCulture);

	public static string PropertyColorSpace_Value65535 => ResourceManager.GetString("PropertyColorSpace_Value65535", resourceCulture);

	public static string PropertyComment => ResourceManager.GetString("PropertyComment", resourceCulture);

	public static string PropertyCompany => ResourceManager.GetString("PropertyCompany", resourceCulture);

	public static string PropertyComposer => ResourceManager.GetString("PropertyComposer", resourceCulture);

	public static string PropertyContentDistributor => ResourceManager.GetString("PropertyContentDistributor", resourceCulture);

	public static string PropertyContributor => ResourceManager.GetString("PropertyContributor", resourceCulture);

	public static string PropertyCopyright => ResourceManager.GetString("PropertyCopyright", resourceCulture);

	public static string PropertyDateCreated => ResourceManager.GetString("PropertyDateCreated", resourceCulture);

	public static string PropertyDateModified => ResourceManager.GetString("PropertyDateModified", resourceCulture);

	public static string PropertyDateReleased => ResourceManager.GetString("PropertyDateReleased", resourceCulture);

	public static string PropertyDateTaken => ResourceManager.GetString("PropertyDateTaken", resourceCulture);

	public static string PropertyDimensions => ResourceManager.GetString("PropertyDimensions", resourceCulture);

	public static string PropertyDimensionsColon => ResourceManager.GetString("PropertyDimensionsColon", resourceCulture);

	public static string PropertyDiscNumber => ResourceManager.GetString("PropertyDiscNumber", resourceCulture);

	public static string PropertyDuration => ResourceManager.GetString("PropertyDuration", resourceCulture);

	public static string PropertyEncodingBitrate => ResourceManager.GetString("PropertyEncodingBitrate", resourceCulture);

	public static string PropertyEpisodeNumber => ResourceManager.GetString("PropertyEpisodeNumber", resourceCulture);

	public static string PropertyExposureTime => ResourceManager.GetString("PropertyExposureTime", resourceCulture);

	public static string PropertyFileDescription => ResourceManager.GetString("PropertyFileDescription", resourceCulture);

	public static string PropertyFileOwner => ResourceManager.GetString("PropertyFileOwner", resourceCulture);

	public static string PropertyFileVersion => ResourceManager.GetString("PropertyFileVersion", resourceCulture);

	public static string PropertyFocalLength => ResourceManager.GetString("PropertyFocalLength", resourceCulture);

	public static string PropertyFormat => ResourceManager.GetString("PropertyFormat", resourceCulture);

	public static string PropertyFrameCount => ResourceManager.GetString("PropertyFrameCount", resourceCulture);

	public static string PropertyFrameHeight => ResourceManager.GetString("PropertyFrameHeight", resourceCulture);

	public static string PropertyFrameRate => ResourceManager.GetString("PropertyFrameRate", resourceCulture);

	public static string PropertyFrameWidth => ResourceManager.GetString("PropertyFrameWidth", resourceCulture);

	public static string PropertyGenre => ResourceManager.GetString("PropertyGenre", resourceCulture);

	public static string PropertyHorizontalResolution => ResourceManager.GetString("PropertyHorizontalResolution", resourceCulture);

	public static string PropertyHorizontalSize => ResourceManager.GetString("PropertyHorizontalSize", resourceCulture);

	public static string PropertyISO => ResourceManager.GetString("PropertyISO", resourceCulture);

	public static string PropertyItemCount => ResourceManager.GetString("PropertyItemCount", resourceCulture);

	public static string PropertyItemName => ResourceManager.GetString("PropertyItemName", resourceCulture);

	public static string PropertyItemTarget => ResourceManager.GetString("PropertyItemTarget", resourceCulture);

	public static string PropertyItemTypeText => ResourceManager.GetString("PropertyItemTypeText", resourceCulture);

	public static string PropertyLanguage => ResourceManager.GetString("PropertyLanguage", resourceCulture);

	public static string PropertyLastAuthor => ResourceManager.GetString("PropertyLastAuthor", resourceCulture);

	public static string PropertyLatitudeDecimal => ResourceManager.GetString("PropertyLatitudeDecimal", resourceCulture);

	public static string PropertyLineCount => ResourceManager.GetString("PropertyLineCount", resourceCulture);

	public static string PropertyLongitudeDecimal => ResourceManager.GetString("PropertyLongitudeDecimal", resourceCulture);

	public static string PropertyOrientation => ResourceManager.GetString("PropertyOrientation", resourceCulture);

	public static string PropertyPageCount => ResourceManager.GetString("PropertyPageCount", resourceCulture);

	public static string PropertyParagraphCount => ResourceManager.GetString("PropertyParagraphCount", resourceCulture);

	public static string PropertyParsingPath => ResourceManager.GetString("PropertyParsingPath", resourceCulture);

	public static string PropertyPartOfSet => ResourceManager.GetString("PropertyPartOfSet", resourceCulture);

	public static string PropertyPeopleNames => ResourceManager.GetString("PropertyPeopleNames", resourceCulture);

	public static string PropertyPreviewAudioEncodingBitrate => ResourceManager.GetString("PropertyPreviewAudioEncodingBitrate", resourceCulture);

	public static string PropertyPreviewVideoEncodingBitrate => ResourceManager.GetString("PropertyPreviewVideoEncodingBitrate", resourceCulture);

	public static string PropertyProducer => ResourceManager.GetString("PropertyProducer", resourceCulture);

	public static string PropertyPromotionUrl => ResourceManager.GetString("PropertyPromotionUrl", resourceCulture);

	public static string PropertyProtectionType => ResourceManager.GetString("PropertyProtectionType", resourceCulture);

	public static string PropertyProviderStyle => ResourceManager.GetString("PropertyProviderStyle", resourceCulture);

	public static string PropertyPublisher => ResourceManager.GetString("PropertyPublisher", resourceCulture);

	public static string PropertyRatingText => ResourceManager.GetString("PropertyRatingText", resourceCulture);

	public static string PropertyRevisionNumber => ResourceManager.GetString("PropertyRevisionNumber", resourceCulture);

	public static string PropertySampleRate => ResourceManager.GetString("PropertySampleRate", resourceCulture);

	public static string PropertySaveErrorDialog_SecondaryButtonText => ResourceManager.GetString("PropertySaveErrorDialog.SecondaryButtonText", resourceCulture);

	public static string PropertySaveErrorDialog_Title => ResourceManager.GetString("PropertySaveErrorDialog.Title", resourceCulture);

	public static string PropertySaveErrorMessage_Text => ResourceManager.GetString("PropertySaveErrorMessage.Text", resourceCulture);

	public static string PropertySeasonNumber => ResourceManager.GetString("PropertySeasonNumber", resourceCulture);

	public static string PropertySectionAudio => ResourceManager.GetString("PropertySectionAudio", resourceCulture);

	public static string PropertySectionCore => ResourceManager.GetString("PropertySectionCore", resourceCulture);

	public static string PropertySectionGPS => ResourceManager.GetString("PropertySectionGPS", resourceCulture);

	public static string PropertySectionImage => ResourceManager.GetString("PropertySectionImage", resourceCulture);

	public static string PropertySectionMedia => ResourceManager.GetString("PropertySectionMedia", resourceCulture);

	public static string PropertySectionMusic => ResourceManager.GetString("PropertySectionMusic", resourceCulture);

	public static string PropertySectionPhoto => ResourceManager.GetString("PropertySectionPhoto", resourceCulture);

	public static string PropertySectionVideo => ResourceManager.GetString("PropertySectionVideo", resourceCulture);

	public static string PropertySeriesName => ResourceManager.GetString("PropertySeriesName", resourceCulture);

	public static string PropertySlideCount => ResourceManager.GetString("PropertySlideCount", resourceCulture);

	public static string PropertySubject => ResourceManager.GetString("PropertySubject", resourceCulture);

	public static string PropertyTemplate => ResourceManager.GetString("PropertyTemplate", resourceCulture);

	public static string PropertyThumbnailLargePath => ResourceManager.GetString("PropertyThumbnailLargePath", resourceCulture);

	public static string PropertyThumbnailLargeUri => ResourceManager.GetString("PropertyThumbnailLargeUri", resourceCulture);

	public static string PropertyThumbnailSmallPath => ResourceManager.GetString("PropertyThumbnailSmallPath", resourceCulture);

	public static string PropertyThumbnailSmallUri => ResourceManager.GetString("PropertyThumbnailSmallUri", resourceCulture);

	public static string PropertyTitle => ResourceManager.GetString("PropertyTitle", resourceCulture);

	public static string PropertyTotalEditingTime => ResourceManager.GetString("PropertyTotalEditingTime", resourceCulture);

	public static string PropertyTrackNumber => ResourceManager.GetString("PropertyTrackNumber", resourceCulture);

	public static string PropertyTrademarks => ResourceManager.GetString("PropertyTrademarks", resourceCulture);

	public static string PropertyUncompressedSize => ResourceManager.GetString("PropertyUncompressedSize", resourceCulture);

	public static string PropertyUserWebUrl => ResourceManager.GetString("PropertyUserWebUrl", resourceCulture);

	public static string PropertyVersion => ResourceManager.GetString("PropertyVersion", resourceCulture);

	public static string PropertyVerticalResolution => ResourceManager.GetString("PropertyVerticalResolution", resourceCulture);

	public static string PropertyVerticalSize => ResourceManager.GetString("PropertyVerticalSize", resourceCulture);

	public static string PropertyWordCount => ResourceManager.GetString("PropertyWordCount", resourceCulture);

	public static string PropertyWriter => ResourceManager.GetString("PropertyWriter", resourceCulture);

	public static string PropertyYear => ResourceManager.GetString("PropertyYear", resourceCulture);

	public static string PullTimeoutError => ResourceManager.GetString("PullTimeoutError", resourceCulture);

	public static string Push => ResourceManager.GetString("Push", resourceCulture);

	public static string QuestionsAndDiscussions => ResourceManager.GetString("QuestionsAndDiscussions", resourceCulture);

	public static string QuickAccess => ResourceManager.GetString("QuickAccess", resourceCulture);

	public static string Quit => ResourceManager.GetString("Quit", resourceCulture);

	public static string RateUs => ResourceManager.GetString("RateUs", resourceCulture);

	public static string ReadOnly => ResourceManager.GetString("ReadOnly", resourceCulture);

	public static string RecentFiles => ResourceManager.GetString("RecentFiles", resourceCulture);

	public static string RecentFilesDisabledOnWindowsWarning => ResourceManager.GetString("RecentFilesDisabledOnWindowsWarning", resourceCulture);

	public static string RecentItemClearAll_Text => ResourceManager.GetString("RecentItemClearAll.Text", resourceCulture);

	public static string RecentItemDescription_Text => ResourceManager.GetString("RecentItemDescription.Text", resourceCulture);

	public static string RecentItemRemove_Text => ResourceManager.GetString("RecentItemRemove.Text", resourceCulture);

	public static string RecycleBin => ResourceManager.GetString("RecycleBin", resourceCulture);

	public static string RecycleBinItemAutomation => ResourceManager.GetString("RecycleBinItemAutomation", resourceCulture);

	public static string Red => ResourceManager.GetString("Red", resourceCulture);

	public static string Redo => ResourceManager.GetString("Redo", resourceCulture);

	public static string RedoDescription => ResourceManager.GetString("RedoDescription", resourceCulture);

	public static string Refresh => ResourceManager.GetString("Refresh", resourceCulture);

	public static string RefreshItemsDescription => ResourceManager.GetString("RefreshItemsDescription", resourceCulture);

	public static string ReleaseNotes => ResourceManager.GetString("ReleaseNotes", resourceCulture);

	public static string ReleaseNotesDescription => ResourceManager.GetString("ReleaseNotesDescription", resourceCulture);

	public static string Remotes => ResourceManager.GetString("Remotes", resourceCulture);

	public static string Remove => ResourceManager.GetString("Remove", resourceCulture);

	public static string RemoveFromShelf => ResourceManager.GetString("RemoveFromShelf", resourceCulture);

	public static string Rename => ResourceManager.GetString("Rename", resourceCulture);

	public static string RenameDescription => ResourceManager.GetString("RenameDescription", resourceCulture);

	public static string RenameError_ItemDeleted_Text => ResourceManager.GetString("RenameError.ItemDeleted.Text", resourceCulture);

	public static string RenameError_ItemDeleted_Title => ResourceManager.GetString("RenameError.ItemDeleted.Title", resourceCulture);

	public static string RenameError_NameInvalid_Text => ResourceManager.GetString("RenameError.NameInvalid.Text", resourceCulture);

	public static string RenameError_NameInvalid_Title => ResourceManager.GetString("RenameError.NameInvalid.Title", resourceCulture);

	public static string RenameError_TooLong_Text => ResourceManager.GetString("RenameError.TooLong.Text", resourceCulture);

	public static string RenameError_TooLong_Title => ResourceManager.GetString("RenameError.TooLong.Title", resourceCulture);

	public static string RenameFileDialog_Text => ResourceManager.GetString("RenameFileDialog.Text", resourceCulture);

	public static string ReopenClosedTab => ResourceManager.GetString("ReopenClosedTab", resourceCulture);

	public static string ReopenClosedTabDescription => ResourceManager.GetString("ReopenClosedTabDescription", resourceCulture);

	public static string ReorderSidebarItemsDialogText => ResourceManager.GetString("ReorderSidebarItemsDialogText", resourceCulture);

	public static string ReplaceExisting => ResourceManager.GetString("ReplaceExisting", resourceCulture);

	public static string RepositoryURL => ResourceManager.GetString("RepositoryURL", resourceCulture);

	public static string Restart => ResourceManager.GetString("Restart", resourceCulture);

	public static string RestartNotificationText_Text => ResourceManager.GetString("RestartNotificationText.Text", resourceCulture);

	public static string Restore => ResourceManager.GetString("Restore", resourceCulture);

	public static string RestoreAllItems => ResourceManager.GetString("RestoreAllItems", resourceCulture);

	public static string RestoreAllRecycleBinDescription => ResourceManager.GetString("RestoreAllRecycleBinDescription", resourceCulture);

	public static string RestoreDefault => ResourceManager.GetString("RestoreDefault", resourceCulture);

	public static string RestoreDefaultKeybindingsConfirmation => ResourceManager.GetString("RestoreDefaultKeybindingsConfirmation", resourceCulture);

	public static string RestoreDefaults => ResourceManager.GetString("RestoreDefaults", resourceCulture);

	public static string RestoreRecycleBinDescription => ResourceManager.GetString("RestoreRecycleBinDescription", resourceCulture);

	public static string Retry => ResourceManager.GetString("Retry", resourceCulture);

	public static string ReviewFilesSubtitle => ResourceManager.GetString("ReviewFilesSubtitle", resourceCulture);

	public static string Right => ResourceManager.GetString("Right", resourceCulture);

	public static string RoseBright => ResourceManager.GetString("RoseBright", resourceCulture);

	public static string RotateLeft => ResourceManager.GetString("RotateLeft", resourceCulture);

	public static string Rotate => ResourceManager.GetString("Rotate", resourceCulture);

	public static string Convert => ResourceManager.GetString("Convert", resourceCulture);

	public static string RotateLeftDescription => ResourceManager.GetString("RotateLeftDescription", resourceCulture);

	public static string RotateRight => ResourceManager.GetString("RotateRight", resourceCulture);

	public static string RotateRightDescription => ResourceManager.GetString("RotateRightDescription", resourceCulture);

	public static string RunAsAdminDescription => ResourceManager.GetString("RunAsAdminDescription", resourceCulture);

	public static string RunAsAdministrator => ResourceManager.GetString("RunAsAdministrator", resourceCulture);

	public static string RunAsAnotherUserDescription => ResourceManager.GetString("RunAsAnotherUserDescription", resourceCulture);

	public static string RunScript => ResourceManager.GetString("RunScript", resourceCulture);

	public static string RunWithPowerShell => ResourceManager.GetString("RunWithPowerShell", resourceCulture);

	public static string RunWithPowershellDescription => ResourceManager.GetString("RunWithPowershellDescription", resourceCulture);

	public static string Save => ResourceManager.GetString("Save", resourceCulture);

	public static string ScrollToPreviousFolderWhenNavigatingUp => ResourceManager.GetString("ScrollToPreviousFolderWhenNavigatingUp", resourceCulture);

	public static string Seafoam => ResourceManager.GetString("Seafoam", resourceCulture);

	public static string Search => ResourceManager.GetString("Search", resourceCulture);

	public static string SearchDescription => ResourceManager.GetString("SearchDescription", resourceCulture);

	public static string SearchPagePathBoxOverrideText => ResourceManager.GetString("SearchPagePathBoxOverrideText", resourceCulture);

	public static string SecondsAgo => ResourceManager.GetString("SecondsAgo", resourceCulture);

	public static string SecondsAgo_Plural => ResourceManager.GetString("SecondsAgo_Plural", resourceCulture);

	public static string SecondsAgo_Singular => ResourceManager.GetString("SecondsAgo_Singular", resourceCulture);

	public static string SectionsHiddenMessage => ResourceManager.GetString("SectionsHiddenMessage", resourceCulture);

	public static string Security => ResourceManager.GetString("Security", resourceCulture);

	public static string SecurityAdvancedAppliesTo => ResourceManager.GetString("SecurityAdvancedAppliesTo", resourceCulture);

	public static string SecurityAdvancedCannotReadProperties_Text => ResourceManager.GetString("SecurityAdvancedCannotReadProperties.Text", resourceCulture);

	public static string SecurityAdvancedFlagsFilesLabel => ResourceManager.GetString("SecurityAdvancedFlagsFilesLabel", resourceCulture);

	public static string SecurityAdvancedFlagsFolderLabel => ResourceManager.GetString("SecurityAdvancedFlagsFolderLabel", resourceCulture);

	public static string SecurityAdvancedFlagsSubfoldersLabel => ResourceManager.GetString("SecurityAdvancedFlagsSubfoldersLabel", resourceCulture);

	public static string SecurityAdvancedFolderFiles_Text => ResourceManager.GetString("SecurityAdvancedFolderFiles.Text", resourceCulture);

	public static string SecurityAdvancedFolderSubfolders_Text => ResourceManager.GetString("SecurityAdvancedFolderSubfolders.Text", resourceCulture);

	public static string SecurityAdvancedFolderSubfoldersFiles_Text => ResourceManager.GetString("SecurityAdvancedFolderSubfoldersFiles.Text", resourceCulture);

	public static string SecurityAdvancedInheritedConvert_Text => ResourceManager.GetString("SecurityAdvancedInheritedConvert.Text", resourceCulture);

	public static string SecurityAdvancedInheritedEnable_Text => ResourceManager.GetString("SecurityAdvancedInheritedEnable.Text", resourceCulture);

	public static string SecurityAdvancedInheritedRemove_Text => ResourceManager.GetString("SecurityAdvancedInheritedRemove.Text", resourceCulture);

	public static string SecurityAdvancedOnlyFiles_Text => ResourceManager.GetString("SecurityAdvancedOnlyFiles.Text", resourceCulture);

	public static string SecurityAdvancedOnlySubfolders_Text => ResourceManager.GetString("SecurityAdvancedOnlySubfolders.Text", resourceCulture);

	public static string SecurityAdvancedOnlySubfoldersFiles_Text => ResourceManager.GetString("SecurityAdvancedOnlySubfoldersFiles.Text", resourceCulture);

	public static string SecurityAdvancedOnlyThisFolder_Text => ResourceManager.GetString("SecurityAdvancedOnlyThisFolder.Text", resourceCulture);

	public static string SecurityAdvancedReplaceChildPermissions_Text => ResourceManager.GetString("SecurityAdvancedReplaceChildPermissions.Text", resourceCulture);

	public static string SecurityAdvancedReplaceChildPermissions2_ToolTipService_ToolTip => ResourceManager.GetString("SecurityAdvancedReplaceChildPermissions2.ToolTipService.ToolTip", resourceCulture);

	public static string SecurityAppendDataLabel_Text => ResourceManager.GetString("SecurityAppendDataLabel.Text", resourceCulture);

	public static string SecurityCannotReadProperties_Text => ResourceManager.GetString("SecurityCannotReadProperties.Text", resourceCulture);

	public static string SecurityChangePermissionsLabel_Text => ResourceManager.GetString("SecurityChangePermissionsLabel.Text", resourceCulture);

	public static string SecurityClickAdvancedPermissions => ResourceManager.GetString("SecurityClickAdvancedPermissions", resourceCulture);

	public static string SecurityCreateDirectoriesLabel_Text => ResourceManager.GetString("SecurityCreateDirectoriesLabel.Text", resourceCulture);

	public static string SecurityCreateFilesLabel_Text => ResourceManager.GetString("SecurityCreateFilesLabel.Text", resourceCulture);

	public static string SecurityDeleteSubdirectoriesAndFilesLabel_Text => ResourceManager.GetString("SecurityDeleteSubdirectoriesAndFilesLabel.Text", resourceCulture);

	public static string SecurityExecuteFileLabel_Text => ResourceManager.GetString("SecurityExecuteFileLabel.Text", resourceCulture);

	public static string SecurityFullControlLabel_Text => ResourceManager.GetString("SecurityFullControlLabel.Text", resourceCulture);

	public static string SecurityListDirectoryLabel_Text => ResourceManager.GetString("SecurityListDirectoryLabel.Text", resourceCulture);

	public static string SecurityNoAccessControlEntriesText => ResourceManager.GetString("SecurityNoAccessControlEntriesText", resourceCulture);

	public static string SecurityOwnerLabel_Text => ResourceManager.GetString("SecurityOwnerLabel.Text", resourceCulture);

	public static string SecurityPermissionsHeaderText => ResourceManager.GetString("SecurityPermissionsHeaderText", resourceCulture);

	public static string SecurityReadAndExecuteLabel_Text => ResourceManager.GetString("SecurityReadAndExecuteLabel.Text", resourceCulture);

	public static string SecurityReadAttributesLabel_Text => ResourceManager.GetString("SecurityReadAttributesLabel.Text", resourceCulture);

	public static string SecurityReadDataLabel_Text => ResourceManager.GetString("SecurityReadDataLabel.Text", resourceCulture);

	public static string SecurityReadExtendedAttributesLabel_Text => ResourceManager.GetString("SecurityReadExtendedAttributesLabel.Text", resourceCulture);

	public static string SecurityReadLabel_Text => ResourceManager.GetString("SecurityReadLabel.Text", resourceCulture);

	public static string SecurityReadPermissionsLabel_Text => ResourceManager.GetString("SecurityReadPermissionsLabel.Text", resourceCulture);

	public static string SecurityRequireReadPermissions => ResourceManager.GetString("SecurityRequireReadPermissions", resourceCulture);

	public static string SecuritySpecialLabel_Text => ResourceManager.GetString("SecuritySpecialLabel.Text", resourceCulture);

	public static string SecuritySuggestToTakeOwnership => ResourceManager.GetString("SecuritySuggestToTakeOwnership", resourceCulture);

	public static string SecurityTakeOwnershipLabel_Text => ResourceManager.GetString("SecurityTakeOwnershipLabel.Text", resourceCulture);

	public static string SecurityTraverseLabel_Text => ResourceManager.GetString("SecurityTraverseLabel.Text", resourceCulture);

	public static string SecurityUnableToDisplayPermissions => ResourceManager.GetString("SecurityUnableToDisplayPermissions", resourceCulture);

	public static string SecurityUnknownAccount => ResourceManager.GetString("SecurityUnknownAccount", resourceCulture);

	public static string SecurityUnknownOwnerText_Text => ResourceManager.GetString("SecurityUnknownOwnerText.Text", resourceCulture);

	public static string SecurityUsersGroupLabel_Text => ResourceManager.GetString("SecurityUsersGroupLabel.Text", resourceCulture);

	public static string SecurityWriteAttributesLabel_Text => ResourceManager.GetString("SecurityWriteAttributesLabel.Text", resourceCulture);

	public static string SecurityWriteDataLabel_Text => ResourceManager.GetString("SecurityWriteDataLabel.Text", resourceCulture);

	public static string SecurityWriteExtendedAttributesLabel_Text => ResourceManager.GetString("SecurityWriteExtendedAttributesLabel.Text", resourceCulture);

	public static string SeeMore => ResourceManager.GetString("SeeMore", resourceCulture);

	public static string SelectAll => ResourceManager.GetString("SelectAll", resourceCulture);

	public static string SelectAllDescription => ResourceManager.GetString("SelectAllDescription", resourceCulture);

	public static string SelectedFilePreviewPane_AutomationProperties_Name => ResourceManager.GetString("SelectedFilePreviewPane.AutomationProperties.Name", resourceCulture);

	public static string SelectedItems => ResourceManager.GetString("SelectedItems", resourceCulture);

	public static string SelectFilesAndFoldersOnHover => ResourceManager.GetString("SelectFilesAndFoldersOnHover", resourceCulture);

	public static string SelectHashesToShow => ResourceManager.GetString("SelectHashesToShow", resourceCulture);

	public static string SelectionOptions => ResourceManager.GetString("SelectionOptions", resourceCulture);

	public static string SendTo => ResourceManager.GetString("SendTo", resourceCulture);

	public static string SetAsAppBackground => ResourceManager.GetString("SetAsAppBackground", resourceCulture);

	public static string SetAsAppBackgroundDescription => ResourceManager.GetString("SetAsAppBackgroundDescription", resourceCulture);

	public static string SetAsBackground => ResourceManager.GetString("SetAsBackground", resourceCulture);

	public static string SetAsBackgroundFlyout => ResourceManager.GetString("SetAsBackgroundFlyout", resourceCulture);

	public static string SetAsDefault => ResourceManager.GetString("SetAsDefault", resourceCulture);

	public static string SetAsDefaultSavePath => ResourceManager.GetString("SetAsDefaultSavePath", resourceCulture);

	public static string SetAsLockscreen => ResourceManager.GetString("SetAsLockscreen", resourceCulture);

	public static string SetAsLockscreenBackgroundDescription => ResourceManager.GetString("SetAsLockscreenBackgroundDescription", resourceCulture);

	public static string SetAsSlideshow => ResourceManager.GetString("SetAsSlideshow", resourceCulture);

	public static string SetAsSlideshowBackgroundDescription => ResourceManager.GetString("SetAsSlideshowBackgroundDescription", resourceCulture);

	public static string SetAsWallpaperBackgroundDescription => ResourceManager.GetString("SetAsWallpaperBackgroundDescription", resourceCulture);

	public static string Settings => ResourceManager.GetString("Settings", resourceCulture);

	public static string SettingsAboutVersionTitle => ResourceManager.GetString("SettingsAboutVersionTitle", resourceCulture);

	public static string SettingsAppearanceTheme => ResourceManager.GetString("SettingsAppearanceTheme", resourceCulture);

	public static string SettingsFontSize => ResourceManager.GetString("SettingsFontSize", resourceCulture);

	public static string SettingsNewTabDefaultPath => ResourceManager.GetString("SettingsNewTabDefaultPath", resourceCulture);

	public static string SortDirection => ResourceManager.GetString("SortDirection", resourceCulture);

	public static string SettingsContextMenu_Text => ResourceManager.GetString("SettingsContextMenu.Text", resourceCulture);

	public static string SettingsContextMenuOverflow => ResourceManager.GetString("SettingsContextMenuOverflow", resourceCulture);

	public static string SettingsFilesAndFoldersShowFileExtensions => ResourceManager.GetString("SettingsFilesAndFoldersShowFileExtensions", resourceCulture);

	public static string SettingsFilesAndFoldersShowHiddenItems => ResourceManager.GetString("SettingsFilesAndFoldersShowHiddenItems", resourceCulture);

	public static string SettingsFilesAndFoldersShowThumbnails => ResourceManager.GetString("SettingsFilesAndFoldersShowThumbnails", resourceCulture);

	public static string SettingsImportErrorDescription => ResourceManager.GetString("SettingsImportErrorDescription", resourceCulture);

	public static string SettingsImportErrorTitle => ResourceManager.GetString("SettingsImportErrorTitle", resourceCulture);

	public static string SettingsLeaveAppRunning => ResourceManager.GetString("SettingsLeaveAppRunning", resourceCulture);

	public static string SettingsMultitaskingAlwaysOpenDualPane => ResourceManager.GetString("SettingsMultitaskingAlwaysOpenDualPane", resourceCulture);

	public static string SettingsOnStartupContinueWhereYouLeftOff_Content => ResourceManager.GetString("SettingsOnStartupContinueWhereYouLeftOff.Content", resourceCulture);

	public static string SettingsContinueLastSessionOnStartUp => ResourceManager.GetString("SettingsContinueLastSessionOnStartUp", resourceCulture);

	public static string SettingsOnStartupOpenANewTab_Content => ResourceManager.GetString("SettingsOnStartupOpenANewTab.Content", resourceCulture);

	public static string SettingsOnStartupOpenASpecificPage_Content => ResourceManager.GetString("SettingsOnStartupOpenASpecificPage.Content", resourceCulture);

	public static string SettingsOpenInLogin => ResourceManager.GetString("SettingsOpenInLogin", resourceCulture);

	public static string SettingsOpenItemsWithOneClick => ResourceManager.GetString("SettingsOpenItemsWithOneClick", resourceCulture);

	public static string SettingsPreferencesSystemDefaultLanguageOption => ResourceManager.GetString("SettingsPreferencesSystemDefaultLanguageOption", resourceCulture);

	public static string SettingsSetAsDefaultFileManager => ResourceManager.GetString("SettingsSetAsDefaultFileManager", resourceCulture);

	public static string SettingsSetAsDefaultFileManagerDescription => ResourceManager.GetString("SettingsSetAsDefaultFileManagerDescription", resourceCulture);

	public static string SettingsSetAsOpenDialog => ResourceManager.GetString("SettingsSetAsOpenDialog", resourceCulture);

	public static string Share => ResourceManager.GetString("Share", resourceCulture);

	public static string ShareDialogFailMessage => ResourceManager.GetString("ShareDialogFailMessage", resourceCulture);

	public static string ShareDialogMultipleItemsDescription => ResourceManager.GetString("ShareDialogMultipleItemsDescription", resourceCulture);

	public static string ShareDialogSingleItemDescription => ResourceManager.GetString("ShareDialogSingleItemDescription", resourceCulture);

	public static string ShareDialogTitle => ResourceManager.GetString("ShareDialogTitle", resourceCulture);

	public static string ShareDialogTitleMultipleItems => ResourceManager.GetString("ShareDialogTitleMultipleItems", resourceCulture);

	public static string ShareItemDescription => ResourceManager.GetString("ShareItemDescription", resourceCulture);

	public static string Shelf => ResourceManager.GetString("Shelf", resourceCulture);

	public static string Shortcut => ResourceManager.GetString("Shortcut", resourceCulture);

	public static string ShortcutCannotBeOpened => ResourceManager.GetString("ShortcutCannotBeOpened", resourceCulture);

	public static string ShortcutCreateNewSuffix => ResourceManager.GetString("ShortcutCreateNewSuffix", resourceCulture);

	public static string ShortcutItemAutomation => ResourceManager.GetString("ShortcutItemAutomation", resourceCulture);

	public static string ShortcutItemWorkingDir => ResourceManager.GetString("ShortcutItemWorkingDir", resourceCulture);

	public static string ShortcutWebLinkFileType => ResourceManager.GetString("ShortcutWebLinkFileType", resourceCulture);

	public static string ShowAlternateStreams => ResourceManager.GetString("ShowAlternateStreams", resourceCulture);

	public static string ShowCheckboxesWhenSelectingItems => ResourceManager.GetString("ShowCheckboxesWhenSelectingItems", resourceCulture);

	public static string ShowCompressionOptions => ResourceManager.GetString("ShowCompressionOptions", resourceCulture);

	public static string ShowInfoInFinder => ResourceManager.GetString("ShowInfoInFinder", resourceCulture);

	public static string ShowInfoInFinderDescription => ResourceManager.GetString("ShowInfoInFinderDescription", resourceCulture);

	public static string ShowConfirmationWhenDeletingItems => ResourceManager.GetString("ShowConfirmationWhenDeletingItems", resourceCulture);

	public static string ShowCopyPath => ResourceManager.GetString("ShowCopyPath", resourceCulture);

	public static string ShowCreateAlternateDataStream => ResourceManager.GetString("ShowCreateAlternateDataStream", resourceCulture);

	public static string ShowCreateFolderWithSelection => ResourceManager.GetString("ShowCreateFolderWithSelection", resourceCulture);

	public static string ShowCreateShortcut => ResourceManager.GetString("ShowCreateShortcut", resourceCulture);

	public static string ShowDotFiles => ResourceManager.GetString("ShowDotFiles", resourceCulture);

	public static string ShowEditTagsMenu => ResourceManager.GetString("ShowEditTagsMenu", resourceCulture);

	public static string ShowFileExtensions => ResourceManager.GetString("ShowFileExtensions", resourceCulture);

	public static string ShowFileExtensionWarning => ResourceManager.GetString("ShowFileExtensionWarning", resourceCulture);

	public static string ShowFlattenOptions => ResourceManager.GetString("ShowFlattenOptions", resourceCulture);

	public static string ShowFlattenOptionsDescription => ResourceManager.GetString("ShowFlattenOptionsDescription", resourceCulture);

	public static string ShowFolderSizesWarning => ResourceManager.GetString("ShowFolderSizesWarning", resourceCulture);

	public static string ShowMoreOptions => ResourceManager.GetString("ShowMoreOptions", resourceCulture);

	public static string ShowOpenInNewPane => ResourceManager.GetString("ShowOpenInNewPane", resourceCulture);

	public static string ShowOpenInNewTab => ResourceManager.GetString("ShowOpenInNewTab", resourceCulture);

	public static string ShowOpenInNewWindow => ResourceManager.GetString("ShowOpenInNewWindow", resourceCulture);

	public static string ShowProtectedSystemFiles => ResourceManager.GetString("ShowProtectedSystemFiles", resourceCulture);

	public static string ShowSendToMenu => ResourceManager.GetString("ShowSendToMenu", resourceCulture);

	public static string ShowShelfPaneButtonInAddressBar => ResourceManager.GetString("ShowShelfPaneButtonInAddressBar", resourceCulture);

	public static string ShowStatusCenterButton => ResourceManager.GetString("ShowStatusCenterButton", resourceCulture);

	public static string ShowSystemTrayIcon => ResourceManager.GetString("ShowSystemTrayIcon", resourceCulture);

	public static string ShowTabActions => ResourceManager.GetString("ShowTabActions", resourceCulture);

	public static string ShowToolbar => ResourceManager.GetString("ShowToolbar", resourceCulture);

	public static string Sidebar => ResourceManager.GetString("Sidebar", resourceCulture);

	public static string SidebarCloudDrives => ResourceManager.GetString("SidebarCloudDrives", resourceCulture);

	public static string SidebarRemovableDrives => ResourceManager.GetString("SidebarRemovableDrives", resourceCulture);

	public static string SideBarCreateNewLibrary_Text => ResourceManager.GetString("SideBarCreateNewLibrary.Text", resourceCulture);

	public static string SideBarHideSectionFromSideBar_Text => ResourceManager.GetString("SideBarHideSectionFromSideBar.Text", resourceCulture);

	public static string SidebarLibraries => ResourceManager.GetString("SidebarLibraries", resourceCulture);

	public static string SideBarRestoreLibraries_Text => ResourceManager.GetString("SideBarRestoreLibraries.Text", resourceCulture);

	public static string Size => ResourceManager.GetString("Size", resourceCulture);

	public static string SizeAllColumnsToFit => ResourceManager.GetString("SizeAllColumnsToFit", resourceCulture);

	public static string SizeColumn => ResourceManager.GetString("SizeColumn", resourceCulture);

	public static string SizeFormat => ResourceManager.GetString("SizeFormat", resourceCulture);

	public static string SizeLabel => ResourceManager.GetString("SizeLabel", resourceCulture);

	public static string SizeOnDiskLabel => ResourceManager.GetString("SizeOnDiskLabel", resourceCulture);

	public static string Skip => ResourceManager.GetString("Skip", resourceCulture);

	public static string Small => ResourceManager.GetString("Small", resourceCulture);

	public static string Solid => ResourceManager.GetString("Solid", resourceCulture);

	public static string Sort => ResourceManager.GetString("Sort", resourceCulture);

	public static string SortAscendingDescription => ResourceManager.GetString("SortAscendingDescription", resourceCulture);

	public static string SortBy => ResourceManager.GetString("SortBy", resourceCulture);

	public static string SortByDateCreatedDescription => ResourceManager.GetString("SortByDateCreatedDescription", resourceCulture);

	public static string SortByDateDeletedDescription => ResourceManager.GetString("SortByDateDeletedDescription", resourceCulture);

	public static string SortByDateModifiedDescription => ResourceManager.GetString("SortByDateModifiedDescription", resourceCulture);

	public static string SortByNameDescription => ResourceManager.GetString("SortByNameDescription", resourceCulture);

	public static string SortByOriginalFolderDescription => ResourceManager.GetString("SortByOriginalFolderDescription", resourceCulture);

	public static string SortByPathDescription => ResourceManager.GetString("SortByPathDescription", resourceCulture);

	public static string SortBySizeDescription => ResourceManager.GetString("SortBySizeDescription", resourceCulture);

	public static string SortBySyncStatusDescription => ResourceManager.GetString("SortBySyncStatusDescription", resourceCulture);

	public static string SortByTagDescription => ResourceManager.GetString("SortByTagDescription", resourceCulture);

	public static string SortByTypeDescription => ResourceManager.GetString("SortByTypeDescription", resourceCulture);

	public static string SortDescendingDescription => ResourceManager.GetString("SortDescendingDescription", resourceCulture);

	public static string SortFilesAndFoldersTogether => ResourceManager.GetString("SortFilesAndFoldersTogether", resourceCulture);

	public static string SortFilesAndFoldersTogetherDescription => ResourceManager.GetString("SortFilesAndFoldersTogetherDescription", resourceCulture);

	public static string SortFilesFirst => ResourceManager.GetString("SortFilesFirst", resourceCulture);

	public static string SortFilesFirstDescription => ResourceManager.GetString("SortFilesFirstDescription", resourceCulture);

	public static string SortFoldersFirst => ResourceManager.GetString("SortFoldersFirst", resourceCulture);

	public static string SortFoldersFirstDescription => ResourceManager.GetString("SortFoldersFirstDescription", resourceCulture);

	public static string SortInDescendingOrder => ResourceManager.GetString("SortInDescendingOrder", resourceCulture);

	public static string SortingAndGrouping => ResourceManager.GetString("SortingAndGrouping", resourceCulture);

	public static string SortPriority => ResourceManager.GetString("SortPriority", resourceCulture);

	public static string SpeedWithColon => ResourceManager.GetString("SpeedWithColon", resourceCulture);

	public static string SplittingSize => ResourceManager.GetString("SplittingSize", resourceCulture);

	public static string Sponsor => ResourceManager.GetString("Sponsor", resourceCulture);

	public static string SponsorSubtitle => ResourceManager.GetString("SponsorSubtitle", resourceCulture);

	public static string SponsorUsOnGitHub => ResourceManager.GetString("SponsorUsOnGitHub", resourceCulture);

	public static string StartupSettings => ResourceManager.GetString("StartupSettings", resourceCulture);

	public static string StartWindow => ResourceManager.GetString("StartWindow", resourceCulture);

	public static string StashChanges => ResourceManager.GetString("StashChanges", resourceCulture);

	public static string Status => ResourceManager.GetString("Status", resourceCulture);

	public static string StatusCenter => ResourceManager.GetString("StatusCenter", resourceCulture);

	public static string StatusCenter_CompressCanceled_Header => ResourceManager.GetString("StatusCenter_CompressCanceled_Header", resourceCulture);

	public static string StatusCenter_CompressCanceled_SubHeader => ResourceManager.GetString("StatusCenter_CompressCanceled_SubHeader", resourceCulture);

	public static string StatusCenter_CompressComplete_Header => ResourceManager.GetString("StatusCenter_CompressComplete_Header", resourceCulture);

	public static string StatusCenter_CompressComplete_SubHeader => ResourceManager.GetString("StatusCenter_CompressComplete_SubHeader", resourceCulture);

	public static string StatusCenter_CompressFailed_Header => ResourceManager.GetString("StatusCenter_CompressFailed_Header", resourceCulture);

	public static string StatusCenter_CompressFailed_SubHeader => ResourceManager.GetString("StatusCenter_CompressFailed_SubHeader", resourceCulture);

	public static string StatusCenter_CompressInProgress_Header => ResourceManager.GetString("StatusCenter_CompressInProgress_Header", resourceCulture);

	public static string StatusCenter_CompressInProgress_SubHeader => ResourceManager.GetString("StatusCenter_CompressInProgress_SubHeader", resourceCulture);

	public static string StatusCenter_CopyCanceled_Header => ResourceManager.GetString("StatusCenter_CopyCanceled_Header", resourceCulture);

	public static string StatusCenter_CopyCanceled_SubHeader => ResourceManager.GetString("StatusCenter_CopyCanceled_SubHeader", resourceCulture);

	public static string StatusCenter_CopyComplete_Header => ResourceManager.GetString("StatusCenter_CopyComplete_Header", resourceCulture);

	public static string StatusCenter_CopyComplete_SubHeader => ResourceManager.GetString("StatusCenter_CopyComplete_SubHeader", resourceCulture);

	public static string StatusCenter_CopyFailed_Header => ResourceManager.GetString("StatusCenter_CopyFailed_Header", resourceCulture);

	public static string StatusCenter_CopyFailed_SubHeader => ResourceManager.GetString("StatusCenter_CopyFailed_SubHeader", resourceCulture);

	public static string StatusCenter_CopyInProgress_Header => ResourceManager.GetString("StatusCenter_CopyInProgress_Header", resourceCulture);

	public static string StatusCenter_CopyInProgress_SubHeader => ResourceManager.GetString("StatusCenter_CopyInProgress_SubHeader", resourceCulture);

	public static string StatusCenter_DecompressCanceled_Header => ResourceManager.GetString("StatusCenter_DecompressCanceled_Header", resourceCulture);

	public static string StatusCenter_DecompressCanceled_SubHeader => ResourceManager.GetString("StatusCenter_DecompressCanceled_SubHeader", resourceCulture);

	public static string StatusCenter_DecompressComplete_Header => ResourceManager.GetString("StatusCenter_DecompressComplete_Header", resourceCulture);

	public static string StatusCenter_DecompressComplete_SubHeader => ResourceManager.GetString("StatusCenter_DecompressComplete_SubHeader", resourceCulture);

	public static string StatusCenter_DecompressFailed_Header => ResourceManager.GetString("StatusCenter_DecompressFailed_Header", resourceCulture);

	public static string StatusCenter_DecompressFailed_SubHeader => ResourceManager.GetString("StatusCenter_DecompressFailed_SubHeader", resourceCulture);

	public static string StatusCenter_DecompressInProgress_Header => ResourceManager.GetString("StatusCenter_DecompressInProgress_Header", resourceCulture);

	public static string StatusCenter_DecompressInProgress_SubHeader => ResourceManager.GetString("StatusCenter_DecompressInProgress_SubHeader", resourceCulture);

	public static string StatusCenter_DeleteCanceled_Header => ResourceManager.GetString("StatusCenter_DeleteCanceled_Header", resourceCulture);

	public static string StatusCenter_DeleteComplete_Header => ResourceManager.GetString("StatusCenter_DeleteComplete_Header", resourceCulture);

	public static string StatusCenter_DeleteFailed_Header => ResourceManager.GetString("StatusCenter_DeleteFailed_Header", resourceCulture);

	public static string StatusCenter_DeleteFailed_SubHeader => ResourceManager.GetString("StatusCenter_DeleteFailed_SubHeader", resourceCulture);

	public static string StatusCenter_DeleteInProgress_Header => ResourceManager.GetString("StatusCenter_DeleteInProgress_Header", resourceCulture);

	public static string StatusCenter_EmptyRecycleBinCancel_Header => ResourceManager.GetString("StatusCenter_EmptyRecycleBinCancel_Header", resourceCulture);

	public static string StatusCenter_EmptyRecycleBinComplete_Header => ResourceManager.GetString("StatusCenter_EmptyRecycleBinComplete_Header", resourceCulture);

	public static string StatusCenter_EmptyRecycleBinFailed_Header => ResourceManager.GetString("StatusCenter_EmptyRecycleBinFailed_Header", resourceCulture);

	public static string StatusCenter_EmptyRecycleBinFailed_SubHeader => ResourceManager.GetString("StatusCenter_EmptyRecycleBinFailed_SubHeader", resourceCulture);

	public static string StatusCenter_EmptyRecycleBinInProgress_Header => ResourceManager.GetString("StatusCenter_EmptyRecycleBinInProgress_Header", resourceCulture);

	public static string StatusCenter_GitCloneCanceled_Header => ResourceManager.GetString("StatusCenter_GitCloneCanceled_Header", resourceCulture);

	public static string StatusCenter_GitCloneCanceled_SubHeader => ResourceManager.GetString("StatusCenter_GitCloneCanceled_SubHeader", resourceCulture);

	public static string StatusCenter_GitCloneComplete_Header => ResourceManager.GetString("StatusCenter_GitCloneComplete_Header", resourceCulture);

	public static string StatusCenter_GitCloneComplete_SubHeader => ResourceManager.GetString("StatusCenter_GitCloneComplete_SubHeader", resourceCulture);

	public static string StatusCenter_GitCloneFailed_Header => ResourceManager.GetString("StatusCenter_GitCloneFailed_Header", resourceCulture);

	public static string StatusCenter_GitCloneFailed_SubHeader => ResourceManager.GetString("StatusCenter_GitCloneFailed_SubHeader", resourceCulture);

	public static string StatusCenter_GitCloneInProgress_Header => ResourceManager.GetString("StatusCenter_GitCloneInProgress_Header", resourceCulture);

	public static string StatusCenter_GitCloneInProgress_SubHeader => ResourceManager.GetString("StatusCenter_GitCloneInProgress_SubHeader", resourceCulture);

	public static string StatusCenter_InstallFontCanceled_Header => ResourceManager.GetString("StatusCenter_InstallFontCanceled_Header", resourceCulture);

	public static string StatusCenter_InstallFontCanceled_SubHeader => ResourceManager.GetString("StatusCenter_InstallFontCanceled_SubHeader", resourceCulture);

	public static string StatusCenter_InstallFontComplete_Header => ResourceManager.GetString("StatusCenter_InstallFontComplete_Header", resourceCulture);

	public static string StatusCenter_InstallFontComplete_SubHeader => ResourceManager.GetString("StatusCenter_InstallFontComplete_SubHeader", resourceCulture);

	public static string StatusCenter_InstallFontFailed_Header => ResourceManager.GetString("StatusCenter_InstallFontFailed_Header", resourceCulture);

	public static string StatusCenter_InstallFontFailed_SubHeader => ResourceManager.GetString("StatusCenter_InstallFontFailed_SubHeader", resourceCulture);

	public static string StatusCenter_InstallFontInProgress_Header => ResourceManager.GetString("StatusCenter_InstallFontInProgress_Header", resourceCulture);

	public static string StatusCenter_InstallFontInProgress_SubHeader => ResourceManager.GetString("StatusCenter_InstallFontInProgress_SubHeader", resourceCulture);

	public static string StatusCenter_MoveCanceled_Header => ResourceManager.GetString("StatusCenter_MoveCanceled_Header", resourceCulture);

	public static string StatusCenter_MoveCanceled_SubHeader => ResourceManager.GetString("StatusCenter_MoveCanceled_SubHeader", resourceCulture);

	public static string StatusCenter_MoveComplete_Header => ResourceManager.GetString("StatusCenter_MoveComplete_Header", resourceCulture);

	public static string StatusCenter_MoveComplete_SubHeader => ResourceManager.GetString("StatusCenter_MoveComplete_SubHeader", resourceCulture);

	public static string StatusCenter_MoveFailed_Header => ResourceManager.GetString("StatusCenter_MoveFailed_Header", resourceCulture);

	public static string StatusCenter_MoveFailed_SubHeader => ResourceManager.GetString("StatusCenter_MoveFailed_SubHeader", resourceCulture);

	public static string StatusCenter_MoveInProgress_Header => ResourceManager.GetString("StatusCenter_MoveInProgress_Header", resourceCulture);

	public static string StatusCenter_MoveInProgress_SubHeader => ResourceManager.GetString("StatusCenter_MoveInProgress_SubHeader", resourceCulture);

	public static string StatusCenter_Prepare_Header => ResourceManager.GetString("StatusCenter_Prepare_Header", resourceCulture);

	public static string StatusCenter_ProcessedItems_Header => ResourceManager.GetString("StatusCenter_ProcessedItems_Header", resourceCulture);

	public static string StatusCenter_ProcessedSize_Header => ResourceManager.GetString("StatusCenter_ProcessedSize_Header", resourceCulture);

	public static string StatusCenterProgressRing => ResourceManager.GetString("StatusCenterProgressRing", resourceCulture);

	public static string StatusWithColon => ResourceManager.GetString("StatusWithColon", resourceCulture);

	public static string Storm => ResourceManager.GetString("Storm", resourceCulture);

	public static string SubDirectoryAccessDenied => ResourceManager.GetString("SubDirectoryAccessDenied", resourceCulture);

	public static string SubmitBugReport => ResourceManager.GetString("SubmitBugReport", resourceCulture);

	public static string SubmitFeatureRequest => ResourceManager.GetString("SubmitFeatureRequest", resourceCulture);

	public static string Switch => ResourceManager.GetString("Switch", resourceCulture);

	public static string SwitchBranch => ResourceManager.GetString("SwitchBranch", resourceCulture);

	public static string SwitchToNewBranch => ResourceManager.GetString("SwitchToNewBranch", resourceCulture);

	public static string SyncFolderPreferencesAcrossDirectories => ResourceManager.GetString("SyncFolderPreferencesAcrossDirectories", resourceCulture);

	public static string SyncStatus => ResourceManager.GetString("SyncStatus", resourceCulture);

	public static string System => ResourceManager.GetString("System", resourceCulture);

	public static string SystemTimeStyle => ResourceManager.GetString("SystemTimeStyle", resourceCulture);

	public static string TabActions => ResourceManager.GetString("TabActions", resourceCulture);

	public static string TabStripDragAndDropUIOverrideCaption => ResourceManager.GetString("TabStripDragAndDropUIOverrideCaption", resourceCulture);

	public static string Tag => ResourceManager.GetString("Tag", resourceCulture);

	public static string TagColor => ResourceManager.GetString("TagColor", resourceCulture);

	public static string TagColumn => ResourceManager.GetString("TagColumn", resourceCulture);

	public static string TagNameRules => ResourceManager.GetString("TagNameRules", resourceCulture);

	public static string TeraByteSymbol => ResourceManager.GetString("TeraByteSymbol", resourceCulture);

	public static string TestIntegration => ResourceManager.GetString("TestIntegration", resourceCulture);

	public static string TheRecycleBin => ResourceManager.GetString("TheRecycleBin", resourceCulture);

	public static string ThinAcrylic => ResourceManager.GetString("ThinAcrylic", resourceCulture);

	public static string ThirdPartyLibraries => ResourceManager.GetString("ThirdPartyLibraries", resourceCulture);

	public static string ThisPC => ResourceManager.GetString("ThisPC", resourceCulture);

	public static string Today => ResourceManager.GetString("Today", resourceCulture);

	public static string ToggleCompactOverlay => ResourceManager.GetString("ToggleCompactOverlay", resourceCulture);

	public static string ToggleCompactOverlayDescription => ResourceManager.GetString("ToggleCompactOverlayDescription", resourceCulture);

	public static string ToggleDetailsPane => ResourceManager.GetString("ToggleDetailsPane", resourceCulture);

	public static string ToggleDetailsPaneDescription => ResourceManager.GetString("ToggleDetailsPaneDescription", resourceCulture);

	public static string ToggleDotFilesSettingDescription => ResourceManager.GetString("ToggleDotFilesSettingDescription", resourceCulture);

	public static string ToggleFullScreenDescription => ResourceManager.GetString("ToggleFullScreenDescription", resourceCulture);

	public static string ToggleGroupByDateUnitDescription => ResourceManager.GetString("ToggleGroupByDateUnitDescription", resourceCulture);

	public static string ToggleGroupDirectionDescription => ResourceManager.GetString("ToggleGroupDirectionDescription", resourceCulture);

	public static string ToggleGroupingUnit => ResourceManager.GetString("ToggleGroupingUnit", resourceCulture);

	public static string ToggleInfoPane => ResourceManager.GetString("ToggleInfoPane", resourceCulture);

	public static string ToggleInfoPaneDescription => ResourceManager.GetString("ToggleInfoPaneDescription", resourceCulture);

	public static string TogglePreviewPane => ResourceManager.GetString("TogglePreviewPane", resourceCulture);

	public static string TogglePreviewPaneDescription => ResourceManager.GetString("TogglePreviewPaneDescription", resourceCulture);

	public static string ToggleSelect => ResourceManager.GetString("ToggleSelect", resourceCulture);

	public static string ToggleSelectDescription => ResourceManager.GetString("ToggleSelectDescription", resourceCulture);

	public static string ToggleShelfPane => ResourceManager.GetString("ToggleShelfPane", resourceCulture);

	public static string ToggleShelfPaneDescription => ResourceManager.GetString("ToggleShelfPaneDescription", resourceCulture);

	public static string ToggleShowFileExtensionsDescription => ResourceManager.GetString("ToggleShowFileExtensionsDescription", resourceCulture);

	public static string ToggleShowHiddenItemsDescription => ResourceManager.GetString("ToggleShowHiddenItemsDescription", resourceCulture);

	public static string ToggleSidebar => ResourceManager.GetString("ToggleSidebar", resourceCulture);

	public static string ToggleSidebarDescription => ResourceManager.GetString("ToggleSidebarDescription", resourceCulture);

	public static string ToggleSortDirection => ResourceManager.GetString("ToggleSortDirection", resourceCulture);

	public static string ToggleSortDirectionDescription => ResourceManager.GetString("ToggleSortDirectionDescription", resourceCulture);

	public static string ToggleToolbar => ResourceManager.GetString("ToggleToolbar", resourceCulture);

	public static string ToggleToolbarDescription => ResourceManager.GetString("ToggleToolbarDescription", resourceCulture);

	public static string Toolbars => ResourceManager.GetString("Toolbars", resourceCulture);

	public static string ToolTipDescriptionDate => ResourceManager.GetString("ToolTipDescriptionDate", resourceCulture);

	public static string Top => ResourceManager.GetString("Top", resourceCulture);

	public static string Total => ResourceManager.GetString("Total", resourceCulture);

	public static string TurnOnBitLocker => ResourceManager.GetString("TurnOnBitLocker", resourceCulture);

	public static string Type => ResourceManager.GetString("Type", resourceCulture);

	public static string TypeColumn => ResourceManager.GetString("TypeColumn", resourceCulture);

	public static string UnableToDisplayOwner => ResourceManager.GetString("UnableToDisplayOwner", resourceCulture);

	public static string UnblockDownloadedFile => ResourceManager.GetString("UnblockDownloadedFile", resourceCulture);

	public static string UncommittedChanges => ResourceManager.GetString("UncommittedChanges", resourceCulture);

	public static string UncompressedSize => ResourceManager.GetString("UncompressedSize", resourceCulture);

	public static string Undo => ResourceManager.GetString("Undo", resourceCulture);

	public static string UndoDescription => ResourceManager.GetString("UndoDescription", resourceCulture);

	public static string Uniform => ResourceManager.GetString("Uniform", resourceCulture);

	public static string UniformToFill => ResourceManager.GetString("UniformToFill", resourceCulture);

	public static string Universal => ResourceManager.GetString("Universal", resourceCulture);

	public static string Unknown => ResourceManager.GetString("Unknown", resourceCulture);

	public static string UnpinFolderFromSidebar => ResourceManager.GetString("UnpinFolderFromSidebar", resourceCulture);

	public static string UnpinFolderFromSidebarDescription => ResourceManager.GetString("UnpinFolderFromSidebarDescription", resourceCulture);

	public static string UnpinFromStartDescription => ResourceManager.GetString("UnpinFromStartDescription", resourceCulture);

	public static string UnpinItemFromStart_Text => ResourceManager.GetString("UnpinItemFromStart.Text", resourceCulture);

	public static string Untagged => ResourceManager.GetString("Untagged", resourceCulture);

	public static string Untracked => ResourceManager.GetString("Untracked", resourceCulture);

	public static string Up => ResourceManager.GetString("Up", resourceCulture);

	public static string UpdateFiles => ResourceManager.GetString("UpdateFiles", resourceCulture);

	public static string UserID => ResourceManager.GetString("UserID", resourceCulture);

	public static string Vertical => ResourceManager.GetString("Vertical", resourceCulture);

	public static string VerticalAlignment => ResourceManager.GetString("VerticalAlignment", resourceCulture);

	public static string Videos => ResourceManager.GetString("Videos", resourceCulture);

	public static string ViewMore => ResourceManager.GetString("ViewMore", resourceCulture);

	public static string View => ResourceManager.GetString("View", resourceCulture);

	public static string Visibility => ResourceManager.GetString("Visibility", resourceCulture);

	public static string VioletRedLight => ResourceManager.GetString("VioletRedLight", resourceCulture);

	public static string VisualStudioCode => ResourceManager.GetString("VisualStudioCode", resourceCulture);

	public static string WelcomeDialog_PrimaryButtonText => ResourceManager.GetString("WelcomeDialog.PrimaryButtonText", resourceCulture);

	public static string WelcomeDialog_Title => ResourceManager.GetString("WelcomeDialog.Title", resourceCulture);

	public static string WelcomeDialogTextBlock_Text => ResourceManager.GetString("WelcomeDialogTextBlock.Text", resourceCulture);

	public static string Widgets => ResourceManager.GetString("Widgets", resourceCulture);

	public static string Width => ResourceManager.GetString("Width", resourceCulture);

	public static string WindowsVersion => ResourceManager.GetString("WindowsVersion", resourceCulture);

	public static string Write => ResourceManager.GetString("Write", resourceCulture);

	public static string WSL => ResourceManager.GetString("WSL", resourceCulture);

	public static string Year => ResourceManager.GetString("Year", resourceCulture);

	public static string YearN => ResourceManager.GetString("YearN", resourceCulture);

	public static string YellowGold => ResourceManager.GetString("YellowGold", resourceCulture);

	public static string Yes => ResourceManager.GetString("Yes", resourceCulture);

	public static string Yesterday => ResourceManager.GetString("Yesterday", resourceCulture);

	public static string ZipFileCapitalized => ResourceManager.GetString("ZipFileCapitalized", resourceCulture);

	public static string OpenFinder => ResourceManager.GetString("OpenFinder", resourceCulture);

	public static string MoveItemsToTrashDescription => ResourceManager.GetString("MoveItemsToTrashDescription", resourceCulture);

	public static string FocusNextPane => ResourceManager.GetString("FocusNextPane", resourceCulture);

	public static string FocusPreviousPane => ResourceManager.GetString("FocusPreviousPane", resourceCulture);

	public static string Exit => ResourceManager.GetString("Exit", resourceCulture);

	public static string KeyboardShortcuts => ResourceManager.GetString("KeyboardShortcuts", resourceCulture);

	public static string SubmitFeedback => ResourceManager.GetString("SubmitFeedback", resourceCulture);

	public static string Applications => ResourceManager.GetString("Applications", resourceCulture);

	public static string SaveWorkspaceAs => ResourceManager.GetString("SaveWorkspaceAs", resourceCulture);

	public static string OpenWorkspaceFromFile => ResourceManager.GetString("OpenWorkspaceFromFile", resourceCulture);

	public static string SaveWorkspaceTitle => ResourceManager.GetString("SaveWorkspaceTitle", resourceCulture);

	public static string OpenWorkspaceTitle => ResourceManager.GetString("OpenWorkspaceTitle", resourceCulture);

	public static string XFolderWorkspaceDescription => ResourceManager.GetString("XFolderWorkspaceDescription", resourceCulture);

	public static string Movies => ResourceManager.GetString("Movies", resourceCulture);

	public static string UnpinFromSidebar => ResourceManager.GetString("UnpinFromSidebar", resourceCulture);

	public static string BreadcrumbViewSubfolders => ResourceManager.GetString("BreadcrumbViewSubfolders", resourceCulture);

	public static string BreadcrumbViewSubfoldersOf => ResourceManager.GetString("BreadcrumbViewSubfoldersOf", resourceCulture);

	public static string RemainingTime => ResourceManager.GetString("RemainingTime", resourceCulture);

	public static string ArchiveTitle => ResourceManager.GetString("ArchiveTitle", resourceCulture);

	public static string ClearCacheAndRestart => ResourceManager.GetString("ClearCacheAndRestart", resourceCulture);

	internal Resources()
	{
	}
}
