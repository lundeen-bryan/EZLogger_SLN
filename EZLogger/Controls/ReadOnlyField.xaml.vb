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
        UpdateHeaderText()
    End Sub

    Public Shared ReadOnly HeaderTextProperty As DependencyProperty = DependencyProperty.Register(
        "HeaderText", GetType(String), GetType(ReadOnlyField),
        New PropertyMetadata(String.Empty, AddressOf OnHeaderTextChanged))

    Public Property HeaderText As String
        Get
            Return CType(GetValue(HeaderTextProperty), String)
        End Get
        Set(value As String)
            SetValue(HeaderTextProperty, value)
        End Set
    End Property

    Private Shared Sub OnHeaderTextChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        Dim control As ReadOnlyField = CType(d, ReadOnlyField)
        If control IsNot Nothing AndAlso control.IsInitialized Then
            control.UpdateHeaderText()
        End If
    End Sub

    Private Sub UpdateHeaderText()
        If Me.IsLoaded Then
            Dim groupBox = FindGroupBox(Me)
            If groupBox IsNot Nothing Then
                groupBox.Header = Me.HeaderText
            End If
        End If
    End Sub

    Private Function FindGroupBox(obj As DependencyObject) As GroupBox
        If TypeOf obj Is GroupBox Then
            Return CType(obj, GroupBox)
        End If

        For i As Integer = 0 To VisualTreeHelper.GetChildrenCount(obj) - 1
            Dim child = VisualTreeHelper.GetChild(obj, i)
            Dim result = FindGroupBox(child)
            If result IsNot Nothing Then
                Return result
            End If
        Next

        Return Nothing
    End Function
    Public Shared ReadOnly GroupBoxWidthProperty As DependencyProperty = DependencyProperty.Register(
        "GroupBoxWidth", GetType(Double), GetType(ReadOnlyField),
        New PropertyMetadata(335.0)) ' default width

    Public Property GroupBoxWidth As Double
        Get
            Return CType(GetValue(GroupBoxWidthProperty), Double)
        End Get
        Set(value As Double)
            SetValue(GroupBoxWidthProperty, value)
        End Set
    End Property
End Class