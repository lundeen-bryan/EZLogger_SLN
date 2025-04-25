Imports System.Windows
Imports EZLogger.Helpers
Imports EZLogger.Handlers
Imports System.Windows.Forms
Imports MessageBox = System.Windows.MessageBox
Imports UserControl = System.Windows.Controls.UserControl


Public Class DueDatePprView
    Inherits UserControl

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
        AddHandler AcceptDatesBtn.Click, AddressOf AcceptDatesBtn_Click
        AddHandler SwitchDatesBtn.Click, AddressOf SwitchDatesBtn_Click
        AddHandler YearDownBtn.Click, AddressOf YearDownBtn_Click
    End Sub

    Private Sub GoBackBtn_Click(sender As Object, e As RoutedEventArgs)
        _handler.HandleGoBackClick(_hostForm)
    End Sub

    Private Sub SavePprChoiceBtn_Click(sender As Object, e As RoutedEventArgs)
        _handler.HandleSavePprChoiceClick(Me)
    End Sub
    Private Sub AcceptDatesBtn_Click(sender As Object, e As RoutedEventArgs)
        _handler.HandleAcceptDatesClick(Me)
    End Sub
    Private Sub SwitchDatesBtn_Click(sender As Object, e As RoutedEventArgs)
        _handler.HandleSwitchDatesClick(Me)
    End Sub
    Private Sub YearDownBtn_Click(sender As Object, e As RoutedEventArgs)
        _handler.HandleYearDownClick(Me)
    End Sub

End Class