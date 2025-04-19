Imports System.Windows
Imports EZLogger.Helpers
Imports EZLogger.Handlers
Imports System.Windows.Forms

Public Class DueDates1370View
    Inherits Controls.UserControl

    Private ReadOnly _handler As New DueDates1370Handler()
    Private ReadOnly _hostForm As Form

    Public Sub New(Optional hostForm As Form = Nothing)
        InitializeComponent()
        _hostForm = hostForm
        WireUpButtons()
    End Sub

    Private Sub WireUpButtons()
        AddHandler GoBackBtn.Click, AddressOf GoBackBtn_Click
    End Sub

    Private Sub GoBackBtn_Click(sender As Object, e As RoutedEventArgs)
        ' Call the handler to handle the button click
        _handler.HandleGoBackClick(_hostForm)
    End Sub

    'Private Sub Btn_Close_Click(sender As Object, e As RoutedEventArgs)
    '    _handler.HandleCloseClick(_hostForm)
    'End Sub

End Class