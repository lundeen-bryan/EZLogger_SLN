Imports EZLogger.EZLogger.Helpers
Imports System.Windows

Public Class ReportAuthorView

    Private Sub ReportAuthorView_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        Dim doctors As List(Of String) = ConfigPathHelper.GetDoctorList()
        CboAuthor.ItemsSource = doctors
        CboAuthor.IsEditable = True
        CboAuthor.IsTextSearchEnabled = True
    End Sub

End Class
