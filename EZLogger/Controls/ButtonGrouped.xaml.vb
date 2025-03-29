Imports System.Windows
Imports System.Windows.Controls

Public Class ButtonGrouped
    ' HeaderText DependencyProperty
    Public Shared ReadOnly HeaderTextProperty As DependencyProperty =
        DependencyProperty.Register(
            "HeaderText",
            GetType(String),
            GetType(ButtonGrouped),
            New PropertyMetadata("Group Header"))

    Public Property HeaderText As String
        Get
            Return CType(GetValue(HeaderTextProperty), String)
        End Get
        Set(value As String)
            SetValue(HeaderTextProperty, value)
        End Set
    End Property

    ' ButtonText DependencyProperty
    Public Shared ReadOnly ButtonTextProperty As DependencyProperty =
        DependencyProperty.Register(
            "ButtonText",
            GetType(String),
            GetType(ButtonGrouped),
            New PropertyMetadata("Click Me"))

    Public Property ButtonText As String
        Get
            Return CType(GetValue(ButtonTextProperty), String)
        End Get
        Set(value As String)
            SetValue(ButtonTextProperty, value)
        End Set
    End Property
End Class