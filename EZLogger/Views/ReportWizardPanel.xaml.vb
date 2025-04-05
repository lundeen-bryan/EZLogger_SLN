Imports System.Windows
Imports System.Windows.Controls
Imports EZLogger.Handlers
Imports EZLogger.Helpers

Partial Public Class ReportWizardPanel
    Inherits Controls.UserControl

    Private getPatientNumberHandler As New ReportWizardHandler()
    Private dbhandler As New PatientDatabaseHandler()
    Private rthandler As New ReportTypeHandler()
    Private ophandler As New HostForms.OpinionHandler()
    Private auhandler As New HostForms.AuthorHandler()
    Private chhandler As New HostForms.ChiefApprovalHandler()

    Private Sub FindPatientId_Click(sender As Object, e As RoutedEventArgs)
        Dim patientNumber As String = getPatientNumberHandler.OnSearchButtonClick()

        If Not String.IsNullOrWhiteSpace(patientNumber) Then
            TextBoxPatientNumber.Text = patientNumber
        Else
            MessageBox.Show("No patient number found in the document footer.", "Search Complete", MessageBoxButton.OK, MessageBoxImage.Information)
        End If
    End Sub
    Private Sub ReportWizardPanel_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        ReportTypeCbo.ItemsSource = rthandler.GetReportTypes()

        ' Simulated database value — later this will come from a database or config
        Dim courtNumbers As String = "123456H; 2344R5; 33456T; 33RRT5; 667788H; 9988-STC-456; VVR-45678; 1"
        CourtNumbersTextBlock.Text = courtNumbers
    End Sub

    Private Sub PatientDatabaseButton_Click(sender As Object, e As RoutedEventArgs)
        dbhandler.OnPatientDatabaseButtonClick()
    End Sub

    Private Sub OpenOpinionForm_Click(sender As Object, e As RoutedEventArgs)
        ophandler.OnOpenOpinionFormClick()
    End Sub
    Private Sub OpenAuthorForm_Click(sender As Object, e As RoutedEventArgs)
        auhandler.OnOpenAuthorFormClick()
    End Sub
    Private Sub OpenChiefHost_Click(sender As Object, e As RoutedEventArgs)
        chhandler.OnOpenChiefHostClick()
    End Sub

    Private Sub ConfirmReportTypeButton_Click(sender As Object, e As RoutedEventArgs)
        Dim selectedItem = ReportTypeCbo.SelectedItem

        If selectedItem IsNot Nothing Then
            Dim currentSelection As String = selectedItem.ToString()

            ' Show the form and get the updated value
            Dim newSelection As String = rthandler.OnConfirmReportTypeButtonClick(currentSelection)

            ' Update ComboBox if the value changed
            If Not String.IsNullOrWhiteSpace(newSelection) AndAlso newSelection <> currentSelection Then
                ReportTypeCbo.SelectedItem = newSelection
            End If
        Else
            MessageBox.Show("Please select a report type first.", "No Selection")
        End If
    End Sub

    Private Sub ReportTypeView_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        Dim reportTypes As List(Of String) = ConfigPathHelper.GetReportTypeList()
        ReportTypeCbo.ItemsSource = reportTypes
    End Sub

End Class
