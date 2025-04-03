Imports System.Windows
Imports System.Windows.Controls
Imports EZLogger.EZLogger.Handlers

Partial Public Class ReportWizardPanel
    Inherits Controls.UserControl

    Private handler As New ReportWizardHandler()
    Private dbhandler As New PatientDatabaseHandler()
    Private rthandler As New ReportTypeHandler()

    Private Sub FindPatientId_Click(sender As Object, e As RoutedEventArgs)
        handler.OnSearchButtonClick()
    End Sub
    Private Sub ReportWizardPanel_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        Dim reportTypes As New List(Of String) From {
        "1370(b)(1)",
        "UNLIKELY 1370(b)(1)",
        "1372(a)(1)"
    }

        ReportTypeCbo.ItemsSource = reportTypes
    End Sub

    Private Sub PatientDatabaseButton_Click(sender As Object, e As RoutedEventArgs)
        dbhandler.OnPatientDatabaseButtonClick()
    End Sub

    Private Sub ConfirmReportTypeButton_Click(sender As Object, e As RoutedEventArgs)
        'Dim selectedItem As ComboBoxItem = TryCast(ReportTypeCbo.SelectedItem, ComboBoxItem)
        Dim selectedItem As String = TryCast(ReportTypeCbo.SelectedItem, String)

        If selectedItem IsNot Nothing Then
            'Dim reportType As String = selectedItem.Content.ToString()
            rthandler.OnConfirmReportTypeButtonClick(selectedItem)
        Else
            MessageBox.Show("Please select a report type first.", "No Selection")
        End If
    End Sub
End Class
