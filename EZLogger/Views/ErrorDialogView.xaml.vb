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
        AddHandler OkBtn.Click, AddressOf OkBtn_Click
        AddHandler AbortBtn.Click, AddressOf AbortBtn_Click
        AddHandler CopyBtn.Click, AddressOf CopyBtn_Click
    End Sub

    Private Sub ErrorDialogView_Loaded(sender As Object, e As RoutedEventArgs)
        DateTimeTxt.Text = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss")
    End Sub

    Private Sub OkBtn_Click(sender As Object, e As RoutedEventArgs)
        _handler.HandleOkClick(_hostForm)
    End Sub

    Private Sub AbortBtn_Click(sender As Object, e As RoutedEventArgs)
        _handler.HandleAbortClick(_hostForm)
    End Sub

    Private Sub CopyBtn_Click(sender As Object, e As RoutedEventArgs)
        _handler.HandleCopyClick(_hostForm)
    End Sub

    Public Sub SetErrorFields(errorMessage As String,
                          errorNumber As String,
                          recommendation As String,
                          source As String)

        ' No need to set DateTime again here — it's already set in Loaded
        ErrorNumberTxt.Text = errorNumber
        ErrorDescriptionTxt.Text = errorMessage
        RecommendationTxt.Text = recommendation
    End Sub

End Class