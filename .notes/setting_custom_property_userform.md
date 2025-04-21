WPF VB.NET Property Exposure Template
=====================================

This guide shows how to expose a property from a WPF UserControl using a DependencyProperty in VB.NET.

_**Also see the article titled "How to Expose Properties to a Form"**_

Template Code
-------------

Place this inside your **UserControl's code-behind (.xaml.vb)** file:

```vb
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
```

How to Customize
----------------

*   **MyProperty**: Replace with your desired property name (e.g., `LabelText`).
*   **String**: Replace with the data type you want (e.g., `Boolean`, `Brush`).
*   **MyControlClass**: Replace with the name of your UserControl class (e.g., `ReadOnlyField`).

Bind in XAML
------------

Inside your UserControl’s XAML file, bind to the property like this:

```xml
<Label Content="{Binding MyProperty, RelativeSource={RelativeSource AncestorType=UserControl}}" />
```

Set the Property When Using the Control
---------------------------------------

In the parent control or window where you use your UserControl, set the property like this:

```xml
<local:MyControlClass MyProperty="Your Value Here" />
```

* * *

_Tip: If you're using MVVM, you can bind the exposed property to your ViewModel for full data binding support._

<!-- @nested-tags:xaml -->