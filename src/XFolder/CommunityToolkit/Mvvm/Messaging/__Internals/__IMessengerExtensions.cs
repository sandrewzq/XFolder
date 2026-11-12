using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Files.Data.Contexts;
using Files.ViewModels.Items;
using Files.ViewModels.Layouts;
using Files.ViewModels.Shells;
using Files.Views;

namespace CommunityToolkit.Mvvm.Messaging.__Internals;

[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.IMessengerRegisterAllGenerator", "8.4.0.0")]
[DebuggerNonUserCode]
[ExcludeFromCodeCoverage]
[EditorBrowsable(EditorBrowsableState.Never)]
[Obsolete("This type is not intended to be used directly by user code")]
[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicMethods)]
internal static class __IMessengerExtensions
{
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("This method is not intended to be called directly by user code")]
	public static Action<IMessenger, object> CreateAllMessagesRegistrator(MultiPanesContext _)
	{
		return RegisterAll;
		static void RegisterAll(IMessenger messenger, object obj)
		{
			MultiPanesContext recipient = (MultiPanesContext)obj;
			messenger.Register(recipient);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("This method is not intended to be called directly by user code")]
	public static Action<IMessenger, object, TToken> CreateAllMessagesRegistratorWithToken<TToken>(MultiPanesContext _) where TToken : IEquatable<TToken>
	{
		return RegisterAll;
		static void RegisterAll(IMessenger messenger, object obj, TToken token)
		{
			MultiPanesContext recipient = (MultiPanesContext)obj;
			messenger.Register(recipient, token);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("This method is not intended to be called directly by user code")]
	public static Action<IMessenger, object> CreateAllMessagesRegistrator(TabItemViewModel _)
	{
		return RegisterAll;
		static void RegisterAll(IMessenger messenger, object obj)
		{
			TabItemViewModel recipient = (TabItemViewModel)obj;
			messenger.Register(recipient);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("This method is not intended to be called directly by user code")]
	public static Action<IMessenger, object, TToken> CreateAllMessagesRegistratorWithToken<TToken>(TabItemViewModel _) where TToken : IEquatable<TToken>
	{
		return RegisterAll;
		static void RegisterAll(IMessenger messenger, object obj, TToken token)
		{
			TabItemViewModel recipient = (TabItemViewModel)obj;
			messenger.Register(recipient, token);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("This method is not intended to be called directly by user code")]
	public static Action<IMessenger, object> CreateAllMessagesRegistrator(BaseLayoutViewModel _)
	{
		return RegisterAll;
		static void RegisterAll(IMessenger messenger, object obj)
		{
			BaseLayoutViewModel recipient = (BaseLayoutViewModel)obj;
			messenger.Register(recipient);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("This method is not intended to be called directly by user code")]
	public static Action<IMessenger, object, TToken> CreateAllMessagesRegistratorWithToken<TToken>(BaseLayoutViewModel _) where TToken : IEquatable<TToken>
	{
		return RegisterAll;
		static void RegisterAll(IMessenger messenger, object obj, TToken token)
		{
			BaseLayoutViewModel recipient = (BaseLayoutViewModel)obj;
			messenger.Register(recipient, token);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("This method is not intended to be called directly by user code")]
	public static Action<IMessenger, object> CreateAllMessagesRegistrator(DetailsLayoutViewModel _)
	{
		return RegisterAll;
		static void RegisterAll(IMessenger messenger, object obj)
		{
			DetailsLayoutViewModel recipient = (DetailsLayoutViewModel)obj;
			messenger.Register(recipient);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("This method is not intended to be called directly by user code")]
	public static Action<IMessenger, object, TToken> CreateAllMessagesRegistratorWithToken<TToken>(DetailsLayoutViewModel _) where TToken : IEquatable<TToken>
	{
		return RegisterAll;
		static void RegisterAll(IMessenger messenger, object obj, TToken token)
		{
			DetailsLayoutViewModel recipient = (DetailsLayoutViewModel)obj;
			messenger.Register(recipient, token);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("This method is not intended to be called directly by user code")]
	public static Action<IMessenger, object> CreateAllMessagesRegistrator(GridLayoutViewModel _)
	{
		return RegisterAll;
		static void RegisterAll(IMessenger messenger, object obj)
		{
			GridLayoutViewModel recipient = (GridLayoutViewModel)obj;
			messenger.Register(recipient);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("This method is not intended to be called directly by user code")]
	public static Action<IMessenger, object, TToken> CreateAllMessagesRegistratorWithToken<TToken>(GridLayoutViewModel _) where TToken : IEquatable<TToken>
	{
		return RegisterAll;
		static void RegisterAll(IMessenger messenger, object obj, TToken token)
		{
			GridLayoutViewModel recipient = (GridLayoutViewModel)obj;
			messenger.Register(recipient, token);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("This method is not intended to be called directly by user code")]
	public static Action<IMessenger, object> CreateAllMessagesRegistrator(ModernShellViewModel _)
	{
		return RegisterAll;
		static void RegisterAll(IMessenger messenger, object obj)
		{
			ModernShellViewModel recipient = (ModernShellViewModel)obj;
			messenger.Register(recipient);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("This method is not intended to be called directly by user code")]
	public static Action<IMessenger, object, TToken> CreateAllMessagesRegistratorWithToken<TToken>(ModernShellViewModel _) where TToken : IEquatable<TToken>
	{
		return RegisterAll;
		static void RegisterAll(IMessenger messenger, object obj, TToken token)
		{
			ModernShellViewModel recipient = (ModernShellViewModel)obj;
			messenger.Register(recipient, token);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("This method is not intended to be called directly by user code")]
	public static Action<IMessenger, object> CreateAllMessagesRegistrator(MainWindow _)
	{
		return RegisterAll;
		static void RegisterAll(IMessenger messenger, object obj)
		{
			MainWindow recipient = (MainWindow)obj;
			messenger.Register(recipient);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("This method is not intended to be called directly by user code")]
	public static Action<IMessenger, object, TToken> CreateAllMessagesRegistratorWithToken<TToken>(MainWindow _) where TToken : IEquatable<TToken>
	{
		return RegisterAll;
		static void RegisterAll(IMessenger messenger, object obj, TToken token)
		{
			MainWindow recipient = (MainWindow)obj;
			messenger.Register(recipient, token);
		}
	}
}
