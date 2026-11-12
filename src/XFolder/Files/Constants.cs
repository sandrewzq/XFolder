namespace Files;

public static class Constants
{
	public static class Global
	{
		public const string BundleIdentifier = "com.zebrapixel.XFolder";

		public const int CacheLifeTime = 604800;

		public const string ExpirationTime = "20270501";
	}

	public static class Dialog
	{
		public const string MainDialogHostIdentifier = "MainDialogHost";
	}

	public static class FileName
	{
		public const string SessionFile = "session.json";

		public const string QuickAccessFile = "quickaccess.json";

		public const string IgnoreVersionFile = "ignore_version.json";

		public const string ThumbnailCacheFile = "thumbnail_cache.json";

		public const string UserSettingsFile = "settings.json";
	}

	public static class FileExtension
	{
		public const string XFolderWorkspace = ".xfd";

		public const string XFolderWorkspaceRegex = "*.xfd";
	}

	public static class DirectoryName
	{
		public const string CacheDirectory = "cache";

		public const string ThumbnailCacheDirectory = "thumbnail";
	}

	public static class Paths
	{
		public const string RootPath = "/";
	}

	public static class AssetPaths
	{
	}

	public static class ResourceFilePaths
	{
	}

	public static class UI
	{
		public static class FontSize
		{
			public const int Default = 13;

			public const int Minimum = 9;

			public const int Maximum = 24;
		}

		public static class ColorKeyNames
		{
			public const string PaneActiveBackground = "PaneActiveBackground";

			public const string PaneInactiveBackground = "PaneInactiveBackground";
		}

		public static class ResourceKeys
		{
			public const string GlobalFontSize = "GlobalFontSize";
		}

		public const double DefaultOpacity = 1.0;

		public const double CutOpacity = 0.45;

		public const double HiddenItemOpacity = 0.45;
	}

	public static class ShellIconSizes
	{
		public const int Small = 16;

		public const int Large = 32;

		public const int ExtraLarge = 48;

		public const int Jumbo = 256;
	}

	public static class ExternalUrl
	{
		public const string GitHubRepoUrl = "https://github.com/zebrapixel/XFolder";

		public const string CreateIssuesUrl = "https://github.com/zebrapixel/XFolder/issues/new";

		public const string CurrentVersionConfigUrl = "https://gist.githubusercontent.com/zebrapixel/37299fa225b2645a8a146654245fa7b6/raw/xfolder_version.config";

		public const string DownloadUrl = "https://github.com/zebrapixel/XFolder/releases/latest";
	}

	public static class Actions
	{
		public const int MaxSelectedItems = 5;
	}

	public static class DragAndDrop
	{
		public const int HoverToOpenTimespan = 1300;
	}

	public static class FormatString
	{
		public const string ModifiedDate = "yyyy-MM-dd HH:mm";
	}

	public static class Theme
	{
		public const string Auto = "auto";

		public const string Light = "light";

		public const string Dark = "dark";
	}
}
