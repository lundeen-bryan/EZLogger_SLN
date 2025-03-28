Imports System.Windows
Imports System.Windows.Media
Imports System.Windows.Controls

Public Class ReadOnlyField
    ' Define a DependencyProperty so it can be used in XAML and supports binding
    Public Shared ReadOnly LabelTextProperty As DependencyProperty = DependencyProperty.Register(
        "LabelText", GetType(String), GetType(ReadOnlyField),
        New PropertyMetadata(String.Empty, AddressOf OnLabelTextChanged))

    ' Create a CLR wrapper for the dependency property
    Public Property LabelText As String
        Get
            Return CType(GetValue(LabelTextProperty), String)
        End Get
        Set(value As String)
            SetValue(LabelTextProperty, value)
        End Set
    End Property

    ' This is where the Label inside your UserControl gets updated
    Private Shared Sub OnLabelTextChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        Dim control As ReadOnlyField = CType(d, ReadOnlyField)
        If control IsNot Nothing AndAlso control.IsInitialized Then
            control.UpdateLabelText()
        End If
    End Sub

    ' Update the Label’s Content
    Private Sub UpdateLabelText()
        If Me.IsLoaded Then
            ' Find the label and set the Content
            If Me.Content IsNot Nothing Then
                Dim label = FindLabel(Me)
                If label IsNot Nothing Then
                    label.Content = Me.LabelText
                End If
            End If
        End If
    End Sub

    ' Helper to find the Label inside the visual tree
    Private Function FindLabel(obj As DependencyObject) As Label
        If TypeOf obj Is Label Then
            Return CType(obj, Label)
        End If

        For i As Integer = 0 To VisualTreeHelper.GetChildrenCount(obj) - 1
            Dim child = VisualTreeHelper.GetChild(obj, i)
            Dim result = FindLabel(child)
            If result IsNot Nothing Then
                Return result
            End If
        Next

        Return Nothing
    End Function

    ' Optional: ensure LabelText is synced on load
    Private Sub ReadOnlyField_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        UpdateLabelText()
    End Sub
End Class

