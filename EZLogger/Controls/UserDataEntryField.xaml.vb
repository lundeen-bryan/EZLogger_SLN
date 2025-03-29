Imports System.Windows
Imports System.Windows.Controls

Public Class UserDataEntryField
    ' DependencyProperty for HeaderText
    Public Shared ReadOnly HeaderTextProperty As DependencyProperty = DependencyProperty.Register(
        "HeaderText", GetType(String), GetType(UserDataEntryField),
        New PropertyMetadata(String.Empty))

    Public Property HeaderText As String
        Get
            Return CType(GetValue(HeaderTextProperty), String)
        End Get
        Set(value As String)
            SetValue(HeaderTextProperty, value)
        End Set
    End Property
End Class