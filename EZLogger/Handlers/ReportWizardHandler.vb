' Namespace=EZLogger/Handlers
' Filename=ReportWizardHandler.vb
' !See Label Footer for notes

Imports System.Windows
Imports EZLogger.Helpers
Imports Microsoft.Office.Interop.Word
Imports MessageBox = System.Windows.MessageBox
Imports System.Threading.Tasks.Task
Imports Application = Microsoft.Office.Interop.Word.Application

Namespace Handlers
    Public Class ReportWizardHandler

        ''' <summary>
        ''' Called by Btn_A. Searches the Word document footer for a patient number
        ''' and populates the TextBox if found. Alerts the user if not found.
        ''' Formerly named SearchAndPopulatePatientNumber
        ''' </summary>
        ''' <param name="panel">The ReportWizardPanel that owns the controls.</param>
        Public Sub ShowBtnAMessage(panel As ReportWizardPanel)

            Dim reader As New WordFooterReader()

            reader.BeginSearchForPatientNumber(
                onFound:=Sub(patientNumber)
                             panel.TextBoxPatientNumber.Text = patientNumber
                             panel.Btn_A_Checkbox.IsChecked = True
                         End Sub,
                onNotFound:=Sub()
                                MsgBoxHelper.Show("No patient number found in the document footer.")
                            End Sub
            )

        End Sub

        ''' <summary>
        ''' Refreshes the patient name label in the Report Wizard panel.
        ''' </summary>
        ''' <param name="panel">The ReportWizardPanel instance containing the label to be updated.</param>
        ''' <remarks>
        ''' This method retrieves the patient name from the document properties and updates the corresponding label in the panel.
        ''' </remarks>
        Public Sub RefreshPatientNameLabel(panel As ReportWizardPanel)
            Dim name As String = DocumentPropertyHelper.GetPropertyValue("Patient Name")
            panel.LabelPatientName.Content = name
        End Sub

        ''' <summary>
        ''' Called by Btn_B. Looks up a patient by number, shows confirmation,
        ''' writes data to custom document properties, and updates the UI.
        ''' Formerly called LookupPatientAndWriteProperties
        ''' </summary>
        ''' <param name="patientNumber">Patient number to look up.</param>
        ''' <param name="panel">The ReportWizardPanel that owns the controls.</param>
        Public Sub ShowBtnBMessage(patientNumber As String, panel As ReportWizardPanel)

            Dim wordApp As Application = WordAppHelper.GetWordApp()

            Dim History As String = DocumentPropertyHelper.GetPropertyValue("Logged")
            If History <> "" Then
                If DateDiff("d", History, Date.Today()) > 2 Then
                    MsgBoxHelper.Show("The last time this report was opened and logged was " & History & " so you might not need to process this report again.")
                End If
            End If

            ' Convert from formatted (e.g., 123456-7) to raw database format (e.g., 41234567)
            patientNumber = ReverseFormatPatientNumber(patientNumber).ToString()

            If String.IsNullOrWhiteSpace(patientNumber) Then
                MsgBoxHelper.Show("No patient number found. Please use the Search button first.")
                Return
            End If

            ' Retrieve patient data from EZL
            Dim patient = DatabaseHelper.GetPatientByNumber(patientNumber)

            If patient IsNot Nothing Then

                ' Retrieve court number via stored procedure and assign to patient object
                Dim courtNumber As String = DatabaseHelper.GetCourtNumberByPatientNumber(patientNumber)
                If Not String.IsNullOrWhiteSpace(courtNumber) Then
                    patient.CourtNumber = courtNumber
                End If

                ' Construct confirmation message
                Dim message As String =
            $"Full Name: {patient.PatientName}" & Environment.NewLine &
            $"Classification: {patient.Classification}" & Environment.NewLine &
            $"Expiration: {DateTime.Parse(patient.Expiration).ToString("MM/dd/yyyy")}" & Environment.NewLine &
            $"County: {patient.County}" & Environment.NewLine &
            $"DOB: {DateTime.Parse(patient.DOB).ToString("MM/dd/yyyy")}" & Environment.NewLine &
            If(Not String.IsNullOrEmpty(patient.CourtNumber), $"Court Number: {patient.CourtNumber}" & Environment.NewLine & Environment.NewLine, "") &
            "Does this information match the report?"

                ' Show custom confirmation box with Yes/No
                Dim config As New MessageBoxConfig With {
                    .Message = message,
                    .ShowYes = True,
                    .ShowNo = True,
                    .ShowOk = False
                }

                MsgBoxHelper.Show(config, Sub(result)
                                              If result = CustomMsgBoxResult.Yes Then
                                                  ' Write patient data and sender to document properties
                                                  DocumentPropertyHelper.WriteDataToDocProperties(patient)
                                                  SenderHelper.WriteProcessedBy(Globals.ThisAddIn.Application.ActiveDocument)
                                                  DocumentPropertyHelper.WriteCustomProperty(wordApp?.ActiveDocument, "Logged", Date.Today())

                                                  ' Refresh UI
                                                  RefreshPatientNameLabel(panel)
                                                  panel.Btn_B_Checkbox.IsChecked = True

                                                  ' Show any configured alerts
                                                  AlertHelper.ShowCountyAlertIfExists(patient.County)
                                                  AlertHelper.ShowPatientAlertIfExists(FormatPatientNumber(patient.PatientNumber))

                                              Else
                                                  MsgBoxHelper.Show("Please check the patient number and try again.")
                                              End If
                                          End Sub)

            Else
                MsgBoxHelper.Show("No patient record found.")
            End If

        End Sub

        ''' <summary>
        ''' Called by Btn_C. Launches the Report Type View, using the commitment
        ''' date from the current Word document.
        ''' </summary>
        Public Sub ShowBtnCMessage()
            ' Retrieve commitment date from Word document
            Dim commitmentDate As String = DocumentPropertyHelper.GetPropertyValue("Commitment")

            ' ' Launch the report type selection dialog
            ' Dim doc As Word.Document = TryCast(Globals.ThisAddIn.Application.ActiveDocument, Word.Document)
            ' Dim commitmentRaw As String = ""

            ' If doc IsNot Nothing Then
            '     Try
            '         commitmentRaw = doc.CustomDocumentProperties("Commitment").Value.ToString()
            '     Catch ex As Exception
            '         commitmentRaw = ""
            '     End Try
            ' End If

            Dim reportTypeHandler As New ReportTypeHandler()
            reportTypeHandler.LaunchReportTypeView(commitmentDate)

        End Sub

        Public Sub ShowBtnDMessage()
            Dim tcarForm As New TCARListHost()
            tcarForm.Show()
        End Sub

        ''' <summary>
        ''' Looks up the CONREP provider value in the HLV Excel workbook using the provided patient number.
        ''' Displays a busy indicator during processing. If a match is found, the provider value is saved
        ''' to the current Word document's custom properties under the key "CONREP".
        ''' </summary>
        ''' <param name="patientNumber">The patient number used as a lookup key in the HLV Excel workbook.</param>
        ''' <remarks>
        ''' This method runs the Excel lookup on a background thread using Tasks.Task.Run to avoid blocking the UI.
        ''' It shows a BusyHost form with an indeterminate progress bar during the lookup.
        ''' If a provider is found, it is written to the custom document property "CONREP".
        ''' </remarks>
        Public Async Sub ShowBtnEMessage(patientNumber As String)
            Dim provider As String = Nothing
            Dim busyForm As New BusyHost()
            busyForm.Show()

            Await Delay(100) ' Give UI time to render the BusyControl

            Try
                provider = Await Run(Function()
                                         Return ExcelHelper.GetProviderFromHLV(patientNumber)
                                     End Function)
            Finally
                busyForm.Close()
            End Try

            If Not String.IsNullOrWhiteSpace(provider) Then
                DocumentPropertyHelper.WriteCustomProperty(Globals.ThisAddIn.Application.ActiveDocument, "CONREP", provider)
                MsgBoxHelper.Show($"Provider found and saved to CONREP: {provider}")
            Else
                MsgBoxHelper.Show($"No provider found for patient number: {patientNumber}")
            End If
        End Sub

        Public Sub ShowBtnFMessage()
            Dim opHandler As New OpinionHandler(Globals.ThisAddIn.Application)
            opHandler.OnOpenOpinionFormClick()
        End Sub

        Public Sub ShowBtnGMessage()
            Dim auHandler As New EvaluatorHandler()
            auHandler.OnOpenEvaluatorViewClick()
        End Sub

        Public Sub ShowBtnHMessage()
            Dim chHandler As New ChiefApprovalHandler()
            chHandler.OnOpenChiefHostClick()
        End Sub

        Public Sub ShowBtnIMessage()
            Dim mvHandler As New SaveFileHandler()
            mvHandler.ShowSaveMessage()
        End Sub

        Public Sub ShowBtnJMessage()
            Dim fxHandler As New FaxCoverHandler()
            fxHandler.ShowFaxCoverMessage()
        End Sub

        Public Sub ShowBtnKMessage()
            Try
                Dim doc As Document = Globals.ThisAddIn.Application.ActiveDocument

                If doc Is Nothing Then
                    MessageBox.Show("No active document found.", "Error", MessageBoxButton.OK, MessageBoxImage.Error)
                    Exit Sub
                End If

                ' Call PrcHandler to process the report
                PrcHandler.SaveProcessedReport(doc)

                ' Show success message
                MessageBox.Show("The report has been processed and logged successfully.", "K Function Complete", MessageBoxButton.OK, MessageBoxImage.Information)

            Catch ex As Exception
                LogHelper.LogError("ReportWizardHandler.ShowBtnKMessage", ex.Message)
                MessageBox.Show("An error occurred while processing the report.", "Processing Error", MessageBoxButton.OK, MessageBoxImage.Error)
            End Try
        End Sub

        Public Sub ShowBtnLMessage()
            Try
                Dim doc As Microsoft.Office.Interop.Word.Document = Globals.ThisAddIn.Application.ActiveDocument

                If doc Is Nothing Then
                    MessageBox.Show("No active document found.", "Error", MessageBoxButton.OK, MessageBoxImage.Error)
                    Exit Sub
                End If

                AlertHelper.AddAlertsToTaskList(doc)

                MsgBoxHelper.Show("Patient and county alerts added to the task list.")

            Catch ex As Exception
                LogHelper.LogError("ReportWizardHandler.ShowBtnLMessage", ex.Message)
                MessageBox.Show("An error occurred while processing alerts.", "Processing Error", MessageBoxButton.OK, MessageBoxImage.Error)
            End Try
        End Sub

    End Class
End Namespace

' Footer:
''===========================================================================================
'' Filename: .......... ReportWizardHandler.vb
'' Description: ....... Main handler for the entry point of the app, for the task panel
'' Created: ........... 2025-05-02
'' Updated: ........... 2025-05-02
'' Installs to: ....... EZLogger/Handlers
'' Compatibility: ..... VSTO, WPF
'' Contact Author: .... lundeen-bryan
'' Copyright:  ........ ©2025. All rights reserved.
'' Notes: ............. _
' (1) notes_here
''===========================================================================================