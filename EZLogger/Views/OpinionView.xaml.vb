Imports System.Windows
Imports System.Windows.Controls
Imports System.Windows.Forms ' Needed for Form
Imports EZLogger.Helpers
Imports EZLogger.Handlers

Public Class OpinionView
    Inherits Controls.UserControl

    Private ReadOnly _handler As OpinionHandler
    Private ReadOnly _hostForm As Form

    ' ✅ Constructor
    Public Sub New(Optional hostForm As Form = Nothing)
        InitializeComponent()
        _hostForm = hostForm
        _handler = New OpinionHandler(Globals.ThisAddIn.Application)

        ' Load combo data when the view is created
        AddHandler Me.Loaded, AddressOf OpinionView_Loaded

        ' Wire up buttons
        AddHandler BtnOpinionOk.Click, AddressOf BtnOpinionOk_Click
        AddHandler BtnOpinionFirstPage.Click, AddressOf BtnOpinionFirstPage_Click
        AddHandler BtnOpinionLastPage.Click, AddressOf BtnOpinionLastPage_Click
        AddHandler BtnClose.Click, AddressOf BtnClose_Click
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

    Private Sub BtnClose_Click(sender As Object, e As RoutedEventArgs)
        _handler.HandleCloseClick(_hostForm)
    End Sub
End Class
