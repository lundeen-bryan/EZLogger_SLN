Imports System.Windows
Imports System.Windows.Forms
Imports EZLogger.Helpers
Imports Microsoft.Office.Interop.Word
Imports MessageBox = System.Windows.MessageBox
Imports System.Threading.Tasks.Task
Imports System.Diagnostics

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

            patientNumber = ReverseFormatPatientNumber(patientNumber).ToString()

            If String.IsNullOrWhiteSpace(patientNumber) Then
                MsgBoxHelper.Show("No patient number found. Please use the Search button first.")
                Return
            End If

            Dim patient = DatabaseHelper.GetPatientByNumber(patientNumber)

            If patient IsNot Nothing Then
                Dim message As String =
            $"Full Name: {patient.FullName}" & Environment.NewLine &
            $"Classification: {patient.Classification}" & Environment.NewLine &
            $"Expiration: {DateTime.Parse(patient.Expiration).ToString("MM/dd/yyyy")}" & Environment.NewLine &
            $"County: {patient.County}" & Environment.NewLine &
            $"DOB: {DateTime.Parse(patient.DOB).ToString("MM/dd/yyyy")}" & Environment.NewLine & Environment.NewLine &
            "Does this information match the report?"

                ' TODO EZL_CTN when getting new schema fix this
                '$"Court Number: {patient.CourtNumbers}" & Environment.NewLine &

                Dim config As New MessageBoxConfig With {
                    .Message = message,
                    .ShowYes = True,
                    .ShowNo = True,
                    .ShowOk = False
                }

                MsgBoxHelper.Show(config, Sub(result)
                                              If result = CustomMsgBoxResult.Yes Then
                                                  DocumentPropertyHelper.WriteDataToDocProperties(patient)
                                                  SenderHelper.WriteProcessedBy(Globals.ThisAddIn.Application.ActiveDocument)
                                                  RefreshPatientNameLabel(panel)
                                                  panel.Btn_B_Checkbox.IsChecked = True

                                                  ' ✅ Show alerts — one after another if both exist
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
