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

    Private Sub PatientDatabaseButton_Click(sender As Object, e As RoutedEventArgs)
        dbhandler.OnPatientDatabaseButtonClick()
    End Sub

    Private Sub ConfirmReportTypeButton_Click(sender As Object, e As RoutedEventArgs)
        Dim selectedItem As ComboBoxItem = TryCast(ReportTypeCbo.SelectedItem, ComboBoxItem)

        If selectedItem IsNot Nothing Then
            Dim reportType As String = selectedItem.Content.ToString()
            rthandler.OnConfirmReportTypeButtonClick(reportType)
        Else
            MessageBox.Show("Please select a report type first.", "No Selection")
        End If
    End Sub
End Class
