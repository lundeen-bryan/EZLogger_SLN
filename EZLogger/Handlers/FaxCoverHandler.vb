Imports EZLogger.Helpers
Imports System.Windows.Controls

Public Class FaxCoverHandler

    ''' <summary>
    ''' Populates the ListBoxCoverPages control with items from config listbox.cover_pages
    ''' </summary>
    ''' <param name="view">The FaxCoverView user control</param>
    Public Sub LoadCoverPages(view As FaxCoverView)
        Try
            Dim coverPages As List(Of String) = ConfigHelper.GetListFromGlobalConfig("listbox", "cover_pages")
            view.ListBoxCoverPages.ItemsSource = coverPages
            view.ListBoxCoverPages.SelectedIndex = 0 ' Optional: Pre-select first item
        Catch ex As Exception
            System.Windows.MessageBox.Show("Failed to load cover pages: " & ex.Message)
        End Try
    End Sub

    Public Sub OnOpenFaxHostClick()
        Dim host As New FaxCoverHost()
        host.TopMost = True
        FormPositionHelper.MoveFormToTopLeftOfAllScreens(host, 10, 10)
        host.Show()
    End Sub
End Class
