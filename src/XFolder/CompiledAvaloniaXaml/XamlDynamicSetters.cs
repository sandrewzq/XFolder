using System;
using System.Runtime.CompilerServices;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Media;

namespace CompiledAvaloniaXaml;

[CompilerGenerated]
internal class XamlDynamicSetters
{
	public static void _003C_003EXamlDynamicSetter_1(SolidColorBrush P_0, object P_1)
	{
		if (P_1 is UnsetValueType)
		{
			P_0.SetValue(SolidColorBrush.ColorProperty, AvaloniaProperty.UnsetValue);
			return;
		}
		if (P_1 is BindingBase)
		{
			BindingBase binding = (BindingBase)P_1;
			P_0.Bind(SolidColorBrush.ColorProperty, binding);
			return;
		}
		if (P_1 is Color)
		{
			P_0.Color = (Color)P_1;
			return;
		}
		if (P_1 == null)
		{
			throw new NullReferenceException();
		}
		throw new InvalidCastException();
	}

	public static void _003C_003EXamlDynamicSetter_2(ContentControl P_0, CompiledBinding P_1)
	{
		if (P_1 != null)
		{
			BindingBase binding = P_1;
			P_0.Bind(ContentControl.ContentProperty, binding);
		}
		else
		{
			P_0.Content = P_1;
		}
	}

	public static void _003C_003EXamlDynamicSetter_3(Control P_0, CompiledBinding P_1)
	{
		if (P_1 != null)
		{
			BindingBase binding = P_1;
			P_0.Bind(Control.TagProperty, binding);
		}
		else
		{
			P_0.Tag = P_1;
		}
	}

	public static void _003C_003EXamlDynamicSetter_4(Control P_0, CompiledBinding P_1)
	{
		if (P_1 != null)
		{
			BindingBase binding = P_1;
			P_0.Bind(ToolTip.TipProperty, binding);
		}
		else
		{
			ToolTip.SetTip(P_0, P_1);
		}
	}

	public static void _003C_003EXamlDynamicSetter_5(HeaderedSelectingItemsControl P_0, ReflectionBinding P_1)
	{
		if (P_1 != null)
		{
			BindingBase binding = P_1;
			P_0.Bind(HeaderedSelectingItemsControl.HeaderProperty, binding);
		}
		else
		{
			P_0.Header = P_1;
		}
	}

	public static void _003C_003EXamlDynamicSetter_6(MenuItem P_0, ReflectionBinding P_1)
	{
		if (P_1 != null)
		{
			BindingBase binding = P_1;
			P_0.Bind(MenuItem.CommandParameterProperty, binding);
		}
		else
		{
			P_0.CommandParameter = P_1;
		}
	}

	public static void _003C_003EXamlDynamicSetter_7(DataGridColumn P_0, CompiledBinding P_1)
	{
		if (P_1 != null)
		{
			BindingBase binding = P_1;
			P_0.Bind(DataGridColumn.HeaderProperty, binding);
		}
		else
		{
			P_0.Header = P_1;
		}
	}

	public static void _003C_003EXamlDynamicSetter_8(ContentControl P_0, ReflectionBinding P_1)
	{
		if (P_1 != null)
		{
			BindingBase binding = P_1;
			P_0.Bind(ContentControl.ContentProperty, binding);
		}
		else
		{
			P_0.Content = P_1;
		}
	}

	public static void _003C_003EXamlDynamicSetter_9(Control P_0, ReflectionBinding P_1)
	{
		if (P_1 != null)
		{
			BindingBase binding = P_1;
			P_0.Bind(ToolTip.TipProperty, binding);
		}
		else
		{
			ToolTip.SetTip(P_0, P_1);
		}
	}

	public static void _003C_003EXamlDynamicSetter_10(SelectingItemsControl P_0, ReflectionBinding P_1)
	{
		if (P_1 != null)
		{
			BindingBase binding = P_1;
			P_0.Bind(SelectingItemsControl.SelectedItemProperty, binding);
		}
		else
		{
			P_0.SelectedItem = P_1;
		}
	}

	public static void _003C_003EXamlDynamicSetter_11(StyledElement P_0, CompiledBinding P_1)
	{
		if (P_1 != null)
		{
			BindingBase binding = P_1;
			P_0.Bind(StyledElement.DataContextProperty, binding);
		}
		else
		{
			P_0.DataContext = P_1;
		}
	}

	public static void _003C_003EXamlDynamicSetter_12(SelectingItemsControl P_0, CompiledBinding P_1)
	{
		if (P_1 != null)
		{
			BindingBase binding = P_1;
			P_0.Bind(SelectingItemsControl.SelectedItemProperty, binding);
		}
		else
		{
			P_0.SelectedItem = P_1;
		}
	}

	public static void _003C_003EXamlDynamicSetter_13(HeaderedSelectingItemsControl P_0, CompiledBinding P_1)
	{
		if (P_1 != null)
		{
			BindingBase binding = P_1;
			P_0.Bind(HeaderedSelectingItemsControl.HeaderProperty, binding);
		}
		else
		{
			P_0.Header = P_1;
		}
	}

	public static void _003C_003EXamlDynamicSetter_14(ContentControl P_0, MultiBinding P_1)
	{
		if (P_1 != null)
		{
			BindingBase binding = P_1;
			P_0.Bind(ContentControl.ContentProperty, binding);
		}
		else
		{
			P_0.Content = P_1;
		}
	}

	public static void _003C_003EXamlDynamicSetter_15(PathIcon P_0, object P_1)
	{
		if (P_1 is UnsetValueType)
		{
			P_0.SetValue(PathIcon.DataProperty, AvaloniaProperty.UnsetValue);
			return;
		}
		if (P_1 is BindingBase)
		{
			BindingBase binding = (BindingBase)P_1;
			P_0.Bind(PathIcon.DataProperty, binding);
			return;
		}
		if (P_1 is Geometry)
		{
			P_0.Data = (Geometry?)P_1;
			return;
		}
		if (P_1 == null)
		{
			P_0.Data = (Geometry?)P_1;
			return;
		}
		throw new InvalidCastException();
	}
}
