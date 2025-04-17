Imports System.Windows
Imports EZLogger.Handlers

Partial Public Class FaxCoverView
    Inherits Controls.UserControl

    Private ReadOnly _handler As FaxCoverHandler

    Public Sub New()
        InitializeComponent()
        _handler = New FaxCoverHandler()
        AddHandler Me.Loaded, AddressOf FaxCoverView_Loaded
    End Sub

    Private Sub FaxCoverView_Loaded(sender As Object, e As RoutedEventArgs)
        _handler.LoadCoverPages(Me)
    End Sub
End Class
