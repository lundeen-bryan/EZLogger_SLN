Imports System.Windows
Imports System.Windows.Controls
Imports System.Windows.Forms ' Needed for Form
Imports EZLogger.Helpers
Imports EZLogger.Handlers

Public Class FaxCoverView
    Inherits System.Windows.Controls.UserControl

    Private ReadOnly _handler As FaxCoverHandler
    Private ReadOnly _hostForm As Form ' ✅ Store the host WinForm

    ' ✅ Modified constructor to accept optional host
    Public Sub New(Optional hostForm As Form = Nothing)
        InitializeComponent()

        _hostForm = hostForm
        _handler = New FaxCoverHandler()
        WireUpButtons()
    End Sub

    Public Sub WireUpButtons()
        ' Wire up buttons
        ' Load combo data when the view is created
        AddHandler Me.Loaded, AddressOf FaxCoverView_Loaded
        AddHandler DoneBtn.Click, AddressOf DoneBtn_Click

    End Sub

    '''<summary>
    '''Checks the checkbox and closes the form
    '''</summary>
    Private Sub DoneBtn_Click(sender As Object, e As RoutedEventArgs)
        Dim panel = TaskPaneHelper.GetTaskPane()
        panel?.MarkCheckboxAsDone("Btn_J")
        _handler.HandleCloseClick(_hostForm)
    End Sub

    ''' <summary>
    ''' Loads cover page options and updates the page count from the active Word document.
    ''' </summary>
    Private Sub FaxCoverView_Loaded(sender As Object, e As RoutedEventArgs)
        ' Load available cover pages into the listbox
        Dim coverPages As List(Of String) = ListHelper.GetListFromGlobalConfig("listbox", "cover_pages")
        CoverPagesLbx.Items.Clear()
        CoverPagesLbx.ItemsSource = coverPages

        ' Update the PagesLbl with the number of pages in the active Word document
        PagesLbl.Content = WordAppHelper.GetActiveDocumentPageCount().ToString()
    End Sub

End Class
