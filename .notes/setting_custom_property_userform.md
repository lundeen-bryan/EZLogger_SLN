 WPF VB.NET UserControl Property Template body { font-family: Consolas, monospace; background-color: #f4f4f4; color: #333; padding: 20px; max-width: 800px; margin: auto; } code, pre { background-color: #eee; padding: 10px; display: block; border: 1px solid #ccc; overflow-x: auto; } h1, h2 { color: #005a9c; } ul { line-height: 1.6; }

WPF VB.NET Property Exposure Template
=====================================

This guide shows how to expose a property from a WPF UserControl using a DependencyProperty in VB.NET.

Template Code
-------------

Place this inside your **UserControl's code-behind (.xaml.vb)** file:

    ' 1. Register the DependencyProperty
    Public Shared ReadOnly MyPropertyProperty As DependencyProperty =
        DependencyProperty.Register("MyProperty", GetType(String), GetType(MyControlClass), New PropertyMetadata(String.Empty))

    ' 2. Create the CLR wrapper
    Public Property MyProperty As String
        Get
            Return CType(GetValue(MyPropertyProperty), String)
        End Get
        Set(value As String)
            SetValue(MyPropertyProperty, value)
        End Set
    End Property


How to Customize
----------------

*   **MyProperty**: Replace with your desired property name (e.g., `LabelText`).
*   **String**: Replace with the data type you want (e.g., `Boolean`, `Brush`).
*   **MyControlClass**: Replace with the name of your UserControl class (e.g., `ReadOnlyField`).

Bind in XAML
------------

Inside your UserControl’s XAML file, bind to the property like this:

    <Label Content="{Binding MyProperty, RelativeSource={RelativeSource AncestorType=UserControl}}" />

Set the Property When Using the Control
---------------------------------------

In the parent control or window where you use your UserControl, set the property like this:

    <local:MyControlClass MyProperty="Your Value Here" />

* * *

_Tip: If you're using MVVM, you can bind the exposed property to your ViewModel for full data binding support._

<!-- @nested-tags:xaml -->