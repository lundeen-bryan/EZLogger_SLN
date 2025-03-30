Imports System.Windows
Imports System.Windows.Controls
Imports System.Windows.Input

Public Class CustomMsgBoxControl
    Inherits UserControl

    Public Property Message As String
        Get
            Return GetValue(MessageProperty)
        End Get
        Set(value As String)
            SetValue(MessageProperty, value)
        End Set
    End Property

    Public Shared ReadOnly MessageProperty As DependencyProperty =
        DependencyProperty.Register("Message", GetType(String), GetType(CustomMsgBoxControl), New PropertyMetadata(String.Empty))

    Public Property YesCommand As ICommand
    Public Property NoCommand As ICommand
    Public Property OkCommand As ICommand

    Public Sub New()
        InitializeComponent()
        YesCommand = New RelayCommand(AddressOf OnYes)
        NoCommand = New RelayCommand(AddressOf OnNo)
        OkCommand = New RelayCommand(AddressOf OnOk)
    End Sub

    Private Sub OnYes()
        MessageBox.Show("Yes clicked")
    End Sub

    Private Sub OnNo()
        MessageBox.Show("No clicked")
    End Sub

    Private Sub OnOk()
        MessageBox.Show("Ok clicked")
    End Sub
End Class

Public Class RelayCommand
    Implements ICommand

    Private ReadOnly _execute As Action

    Public Sub New(execute As Action)
        _execute = execute
    End Sub

    Public Custom Event CanExecuteChanged As EventHandler Implements ICommand.CanExecuteChanged
        AddHandler(value As EventHandler)
        End AddHandler

        RemoveHandler(value As EventHandler)
        End RemoveHandler

        RaiseEvent(sender As Object, e As EventArgs)
        End RaiseEvent
    End Event

    Public Function CanExecute(parameter As Object) As Boolean Implements ICommand.CanExecute
        Return True
    End Function

    Public Sub Execute(parameter As Object) Implements ICommand.Execute
        _execute()
    End Sub
End Class