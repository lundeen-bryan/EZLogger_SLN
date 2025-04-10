Imports System.Windows
Imports System.Windows.Controls
Imports EZLogger.Handlers
Imports EZLogger.Helpers
Imports MessageBox = System.Windows.MessageBox
Imports EZLogger.ViewModels

Partial Public Class ReportWizardPanel
    Inherits Controls.UserControl

    Public Property ViewModel As MainVM
    Private ReadOnly _handler As ReportWizardHandler

    Public Sub New(Optional viewModel As MainVM = Nothing)
        InitializeComponent()

        ' Always use a ViewModel, even if caller didn't provide one
        If viewModel IsNot Nothing Then
            Me.ViewModel = viewModel
        Else
            Me.ViewModel = New MainVM()
        End If

        Me.DataContext = Me.ViewModel

        ' Handler uses the same ViewModel
        _handler = New ReportWizardHandler(Me.ViewModel)

        ' Wire up buttons
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
    Public Sub New()
        Me.New(New MainVM()) ' Delegate to the main constructor with a new ViewModel
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
            MessageBox.Show("No patient number found in the document footer.", "Search Complete", MessageBoxButton.OK, MessageBoxImage.Information)
        End If
    End Sub

    Private Sub LookupDatabase_Click(sender As Object, e As RoutedEventArgs)
        Dim patientNumber As String = TextBoxPatientNumber.Text
        _handler.LookupPatientAndWriteProperties(patientNumber)
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
            MessageBox.Show("Please select a report type first.", "No Selection")
        End If
    End Sub

    Private Sub ReportWizardPanel_Loaded(sender As Object, e As RoutedEventArgs)
        Dim reportTypes As List(Of String) = ConfigPathHelper.GetReportTypeList()
        ReportTypeCbo.ItemsSource = reportTypes
    End Sub

End Class

