Imports System.Windows
Imports EZLogger.Helpers
Imports EZLogger.Handlers
Imports System.Windows.Forms
Imports UserControl = System.Windows.Controls.UserControl

Public Class ErrorDialogView
    Inherits UserControl

    Private ReadOnly _handler As New ErrorDialogHandler()
    Private ReadOnly _hostForm As Form

    Public Sub New(Optional hostForm As Form = Nothing)
        InitializeComponent()
        _hostForm = hostForm
        WireUpButtons()
    End Sub

    Private Sub WireUpButtons()
        AddHandler Me.Loaded, AddressOf ErrorDialogView_Loaded
        ' AddHandler Btn_Close.Click, AddressOf Btn_Close_Click
    End Sub

    Private Sub ErrorDialogView_Loaded(sender As Object, e As RoutedEventArgs)
        DateTimeTxt.Text = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss")
    End Sub

    Private Sub Btn_Close_Click(sender As Object, e As RoutedEventArgs)
        ' _handler.HandleCloseClick(_hostForm)
    End Sub

End Class