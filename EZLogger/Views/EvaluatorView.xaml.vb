Imports System.Windows
Imports EZLogger.Helpers
Imports EZLogger.Handlers
Imports System.Windows.Forms
Imports UserControl = System.Windows.Controls.UserControl

Public Class EvaluatorView
    Inherits UserControl

    Private ReadOnly _handler As New EvaluatorHandler()
    Private ReadOnly _hostForm As Form

    Public Sub New(Optional hostForm As Form = Nothing)
        InitializeComponent()
        _hostForm = hostForm
        WireUpButtons()
    End Sub

    Private Sub WireUpButtons()
        AddHandler Me.Loaded, AddressOf EvaluatorView_Loaded
        AddHandler BtnAddAuthor.Click, AddressOf BtnAddAuthor_Click
        AddHandler BtnAuthorFirstPage.Click, AddressOf BtnAuthorFirstPage_Click
        AddHandler BtnAuthorLastPage.Click, AddressOf BtnAuthorLastPage_Click
        AddHandler BtnAuthorDone.Click, AddressOf BtnAuthorDone_Click
        AddHandler DoneBtn.Click, AddressOf DoneBtn_Click
    End Sub

    Private Sub EvaluatorView_Loaded(sender As Object, e As RoutedEventArgs)
        Dim doctors As List(Of String) = ListHelper.GetDoctorList()
        AuthorCbo.ItemsSource = doctors
    End Sub

    Private Sub BtnAddAuthor_Click(sender As Object, e As RoutedEventArgs)
        _handler.HandleAddAuthorClick()
    End Sub

    Private Sub BtnAuthorFirstPage_Click(sender As Object, e As RoutedEventArgs)
        _handler.HandleFirstPageClick()
    End Sub

    Private Sub BtnAuthorLastPage_Click(sender As Object, e As RoutedEventArgs)
        _handler.HandleLastPageClick()
    End Sub

    Private Sub BtnAuthorDone_Click(sender As Object, e As RoutedEventArgs)
        _handler.HandleDoneSelectingClick()
    End Sub

    Private Sub DoneBtn_Click(sender As Object, e As RoutedEventArgs)
        Dim panel = TaskPaneHelper.GetTaskPane()
        panel?.MarkCheckboxAsDone("Btn_G")
        _handler.HandleCloseClick(_hostForm)
    End Sub

End Class