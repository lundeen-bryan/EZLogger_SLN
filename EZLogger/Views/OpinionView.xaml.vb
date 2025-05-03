Imports EZLogger.Handlers
Imports EZLogger.Helpers
Imports UserControl = System.Windows.Controls.UserControl
Imports System.Windows
Imports System.Windows.Forms ' Needed for Form

Public Class OpinionView
    Inherits UserControl

    Private ReadOnly _handler As OpinionHandler
    Private ReadOnly _hostForm As Form

    ' Constructor
    Public Sub New(Optional hostForm As Form = Nothing)
        InitializeComponent()
        _hostForm = hostForm
        _handler = New OpinionHandler(Globals.ThisAddIn.Application)
        WireUpButtons()
    End Sub

    Public Sub WireUpButtons()
        ' Load combo data when the view is created
        AddHandler Me.Loaded, AddressOf OpinionView_Loaded

        ' Wire up buttons
        AddHandler BtnOpinionOk.Click, AddressOf BtnOpinionOk_Click
        AddHandler BtnOpinionFirstPage.Click, AddressOf BtnOpinionFirstPage_Click
        AddHandler BtnOpinionLastPage.Click, AddressOf BtnOpinionLastPage_Click
        AddHandler DoneBtn.Click, AddressOf DoneBtn_Click
    End Sub

    Private Sub OpinionView_Loaded(sender As Object, e As RoutedEventArgs)
        Dim opinions As List(Of String) = ListHelper.GetListFromGlobalConfig("listbox", "opinions")
        OpinionCbo.Items.Clear()
        OpinionCbo.ItemsSource = opinions
    End Sub

    Private Sub BtnOpinionOk_Click(sender As Object, e As RoutedEventArgs)
        Dim selectedOpinion As String = TryCast(OpinionCbo.SelectedItem, String)
        _handler.HandleOpinionOkClick(selectedOpinion)
    End Sub

    Private Sub BtnOpinionFirstPage_Click(sender As Object, e As RoutedEventArgs)
        _handler.HandleOpinionFirstPageClick()
    End Sub

    Private Sub BtnOpinionLastPage_Click(sender As Object, e As RoutedEventArgs)
        _handler.HandleOpinionLastPageClick()
    End Sub

    '''<summary>
    '''Checks the checkbox and closes the form
    '''</summary>
    Private Sub DoneBtn_Click(sender As Object, e As RoutedEventArgs)
        Dim panel = TaskPaneHelper.GetTaskPane()
        panel?.MarkCheckboxAsDone("Btn_F")
        _handler.HandleCloseClick(_hostForm)
    End Sub
End Class
