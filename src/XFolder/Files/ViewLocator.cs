using System;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Files.ViewModels;

namespace Files;

public class ViewLocator : IDataTemplate, ITemplate<object?, Control?>
{
	public Control? Build(object? param)
	{
		if (param == null)
		{
			return null;
		}
		string text = param.GetType().FullName.Replace("ViewModel", "View", StringComparison.Ordinal);
		Type type = Type.GetType(text);
		if (type != null)
		{
			return (Control)Activator.CreateInstance(type);
		}
		return new TextBlock
		{
			Text = "Not Found: " + text
		};
	}

	public bool Match(object? data)
	{
		return data is ViewModelBase;
	}
}
