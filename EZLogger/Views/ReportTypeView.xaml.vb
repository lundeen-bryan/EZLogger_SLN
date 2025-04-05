' Top of the file
Imports EZLogger.Handlers
Imports System.Collections.Generic
Imports System.Windows

Public Class ReportTypeView

    ' ✅ 1. Property to receive selected report type
    Public Property InitialSelectedReportType As String

    ' ✅ 2. Handler for shared report types
    Private rthandler As New ReportTypeHandler()

    ' ✅ 3. When the view loads, populate and set selection
    Private Sub ReportTypeView_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        Dim reportTypes As List(Of String) = rthandler.GetReportTypes()
        ReportTypeViewCbo.ItemsSource = reportTypes

        If Not String.IsNullOrEmpty(InitialSelectedReportType) AndAlso reportTypes.Contains(InitialSelectedReportType) Then
            ReportTypeViewCbo.SelectedItem = InitialSelectedReportType
        End If
    End Sub

End Class
