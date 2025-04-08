Imports System.Windows
Imports System.Windows.Controls
Imports EZLogger.Handlers
Imports EZLogger.Helpers

Partial Public Class ReportWizardPanel
    Inherits Controls.UserControl

    Private ReadOnly _handler As ReportWizardHandler

    Public Sub New()
        InitializeComponent()
        _handler = New ReportWizardHandler()

        AddHandler BtnCoverPageForm.Click, AddressOf BtnCoverPageForm_Click
        AddHandler FindPatientId.Click, AddressOf FindPatientId_Click
        AddHandler LookupDatabase.Click, AddressOf LookupDatabase_Click
        AddHandler BtnOpenOpinionForm.Click, AddressOf BtnOpenOpinionForm_Click
        AddHandler BtnSelectAuthor.Click, AddressOf BtnSelectAuthor_Click
        AddHandler BtnSelectChief.Click, AddressOf BtnSelectChief_Click
        AddHandler ConfirmTypeBtn.Click, AddressOf ConfirmReportType_Click
        AddHandler Me.Loaded, AddressOf ReportWizardPanel_Loaded
        AddHandler BtnSaveForm.Click, AddressOf BtnSaveForm_Click
    End Sub

    Private Sub BtnSaveForm_Click(sender As Object, e As RoutedEventArgs)
        Dim mvhandler As New MoveCopyHandler()
        mvhandler.OnMoveCopyClick()
    End Sub

    Private Sub BtnCoverPageForm_Click(sender As Object, e As RoutedEventArgs)
        Dim fileHandler As New CoverPageHandler()
        fileHandler.OnFileSaveHostClick()
    End Sub

    Private Sub FindPatientId_Click(sender As Object, e As RoutedEventArgs)
        Dim patientNumber As String = _handler.OnSearchButtonClick()
        If Not String.IsNullOrWhiteSpace(patientNumber) Then
            TextBoxPatientNumber.Text = patientNumber
        Else
            Windows.MessageBox.Show("No patient number found in the document footer.", "Search Complete", MessageBoxButton.OK, MessageBoxImage.Information)
        End If
    End Sub

    Private Sub LookupDatabase_Click(sender As Object, e As RoutedEventArgs)
        Dim patientNumber As String = TextBoxPatientNumber.Text
        Dim handler As New ReportWizardHandler()
        handler.LookupPatientAndWriteProperties(patientNumber)
    End Sub

    Private Sub BtnOpenOpinionForm_Click(sender As Object, e As RoutedEventArgs)
        Dim opHandler As New OpinionHandler()
        opHandler.OnOpenOpinionFormClick()
    End Sub

    Private Sub BtnSelectAuthor_Click(sender As Object, e As RoutedEventArgs)
        Dim auHandler As New AuthorHandler()
        auHandler.OnOpenAuthorFormClick()
    End Sub

    Private Sub BtnSelectChief_Click(sender As Object, e As RoutedEventArgs)
        Dim chHandler As New ChiefApprovalHandler()
        chHandler.OnOpenChiefHostClick()
    End Sub

    Private Sub ConfirmReportType_Click(sender As Object, e As RoutedEventArgs)
        Dim rHandler As New ReportTypeHandler()
        Dim selectedItem = ReportTypeCbo.SelectedItem
        If selectedItem IsNot Nothing Then
            Dim currentSelection As String = selectedItem.ToString()
            Dim newSelection As String = rHandler.OnConfirmReportTypeButtonClick(currentSelection)
            If Not String.IsNullOrWhiteSpace(newSelection) AndAlso newSelection <> currentSelection Then
                ReportTypeCbo.SelectedItem = newSelection
            End If
        Else
            Windows.MessageBox.Show("Please select a report type first.", "No Selection")
        End If
    End Sub

    Private Sub ReportWizardPanel_Loaded(sender As Object, e As RoutedEventArgs)
        Dim reportTypes As List(Of String) = ConfigPathHelper.GetReportTypeList()
        ReportTypeCbo.ItemsSource = reportTypes

        ' Optional: Pre-load something into CourtNumbersTextBlock
        CourtNumbersTextBlock.Text = "123456H; 2344R5; 33456T; 33RRT5; 667788H; 9988-STC-456; VVR-45678; 1"
    End Sub

End Class
