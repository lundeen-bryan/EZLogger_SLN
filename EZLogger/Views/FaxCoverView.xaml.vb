Imports EZLogger.Helpers
Imports EZLogger.Handlers
Imports System.Windows
Imports System.Windows.Forms
Imports Microsoft.Office.Interop.Word

Public Class FaxCoverView
    Inherits System.Windows.Controls.UserControl

    Private ReadOnly _handler As FaxCoverHandler
    Private ReadOnly _hostForm As Form

    Public Sub New(Optional hostForm As Form = Nothing)
        InitializeComponent()

        _hostForm = hostForm
        _handler = New FaxCoverHandler()

        WireUpButtons()
    End Sub

    Public Sub WireUpButtons()
        AddHandler Me.Loaded, AddressOf FaxCoverView_Loaded
        AddHandler ConvertPdfBtn.Click, AddressOf ConvertPdfBtn_Click
        AddHandler DoneBtn.Click, AddressOf DoneBtn_Click
    End Sub

    ''' <summary>
    ''' Loads cover pages into the listbox and updates page count label.
    ''' </summary>
    Private Sub FaxCoverView_Loaded(sender As Object, e As RoutedEventArgs)
        Dim coverPages As List(Of String) = ListHelper.GetListFromGlobalConfig("listbox", "cover_pages")
        CoverPagesLbx.Items.Clear()
        CoverPagesLbx.ItemsSource = coverPages

        ' Update Pages label
        Dim wordApp = WordAppHelper.GetWordApp()
        Dim pageCount = wordApp.ActiveDocument.ComputeStatistics(WdStatistic.wdStatisticPages)
        PagesLbl.Content = pageCount.ToString()
    End Sub

    ''' <summary>
    ''' Handles the Convert button click: creates and saves the fax cover document.
    ''' </summary>
    Private Sub ConvertPdfBtn_Click(sender As Object, e As RoutedEventArgs)
        If CoverPagesLbx.SelectedItem Is Nothing Then
            MsgBoxHelper.Show("Please select a cover page template first.")
            CoverPagesLbx.Focus()
            Exit Sub
        End If

        Dim selectedText As String = CoverPagesLbx.SelectedItem.ToString()
        Dim letter As String = selectedText.Substring(0, 1).ToUpper()

        Dim saveToTemp As Boolean = RadioPdf.IsChecked
        Dim convertToPdf As Boolean = RadioPdf.IsChecked

        ' 📋 New: Read page numbers
        Dim reportPages As Integer = 0
        Dim additionalPages As Integer = 0

        If Integer.TryParse(PagesLbl.Content.ToString(), reportPages) AndAlso
       Integer.TryParse(IncrementerTotalPages.Text.ToString(), additionalPages) Then

            Dim totalPages As Integer = reportPages + additionalPages

            ' 📋 Updated: Pass totalPages and reportPages to the handler
            _handler.CreateFaxCover(letter, saveToTemp, convertToPdf, totalPages, reportPages)

        Else
            MsgBoxHelper.Show("Invalid page number entries. Please check the form.")
        End If
    End Sub

    ''' <summary>
    ''' Handles the Done button click: marks Btn_J as complete and closes the form.
    ''' </summary>
    Private Sub DoneBtn_Click(sender As Object, e As RoutedEventArgs)
        Dim panel = TaskPaneHelper.GetTaskPane()
        panel?.MarkCheckboxAsDone("Btn_J")
        _handler.HandleCloseClick(_hostForm)
    End Sub

End Class
