Imports System.Windows
Imports EZLogger.Helpers
Imports EZLogger.Handlers
Imports System.Windows.Forms

Public Class DueDatePprView
    Inherits Controls.UserControl

    Private ReadOnly _handler As New DueDatePprHandler()
    Private ReadOnly _hostForm As Form

    Public Sub New(Optional hostForm As Form = Nothing)
        InitializeComponent()
        _hostForm = hostForm
        WireUpButtons()
    End Sub

    Private Sub WireUpButtons()
        AddHandler GoBackBtn.Click, AddressOf GoBackBtn_Click
        AddHandler ContinueBtn.Click, AddressOf SavePprChoiceBtn_Click
    End Sub

    Private Sub GoBackBtn_Click(sender As Object, e As RoutedEventArgs)
        _handler.HandleGoBackClick(_hostForm)
    End Sub

    Private Sub SavePprChoiceBtn_Click(sender As Object, e As RoutedEventArgs)
        _handler.HandleSavePprChoiceClick(Me)
    End Sub

End Class