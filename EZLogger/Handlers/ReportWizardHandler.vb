Imports System.Windows
Imports System.Windows.Forms
Imports EZLogger.Helpers
Imports Microsoft.Office.Interop.Word
Imports MessageBox = System.Windows.MessageBox

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

            If String.IsNullOrWhiteSpace(patientNumber) Then
                MsgBoxHelper.Show("No patient number found. Please use the Search button first.")
                Return
            End If

            Dim patient = DatabaseHelper.GetPatientByNumber(patientNumber)

            If patient IsNot Nothing Then
                Dim message As String =
            $"Full Name: {patient.FullName}" & Environment.NewLine &
            $"Classification: {patient.Classification}" & Environment.NewLine &
            $"Court Number: {patient.CourtNumbers}" & Environment.NewLine &
            $"Expiration: {DateTime.Parse(patient.Expiration).ToString("MM/dd/yyyy")}" & Environment.NewLine &
            $"County: {patient.County}" & Environment.NewLine &
            $"DOB: {DateTime.Parse(patient.DOB).ToString("MM/dd/yyyy")}" & Environment.NewLine & Environment.NewLine &
            "Does this information match the report?"

                Dim config As New MessageBoxConfig With {
                    .Message = message,
                    .ShowYes = True,
                    .ShowNo = True,
                    .ShowOK = False
                }

                MsgBoxHelper.Show(config, Sub(result)
                                              If result = CustomMsgBoxResult.Yes Then
                                                  DocumentPropertyHelper.WriteDataToDocProperties(patient)
                                                  SenderHelper.WriteProcessedBy(Globals.ThisAddIn.Application.ActiveDocument)
                                                  RefreshPatientNameLabel(panel)
                                                  panel.Btn_B_Checkbox.IsChecked = True
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

            ' Launch the report type selection dialog
            Dim doc As Word.Document = TryCast(Globals.ThisAddIn.Application.ActiveDocument, Word.Document)
            Dim commitmentRaw As String = ""

            If doc IsNot Nothing Then
                Try
                    commitmentRaw = doc.CustomDocumentProperties("Commitment").Value.ToString()
                Catch ex As Exception
                    commitmentRaw = ""
                End Try
            End If

            Dim reportTypeHandler As New ReportTypeHandler()
            reportTypeHandler.LaunchReportTypeView(commitmentRaw)

        End Sub

        Public Sub ShowBtnDMessage()
            Dim tcarForm As New TCARListHost()
            tcarForm.Show()
        End Sub

        Public Sub ShowBtnEMessage()
            MessageBox.Show("This will present the HTV list.", "Coming Soon", MessageBoxButton.OK, MessageBoxImage.Information)
        End Sub
        Public Sub ShowBtnFMessage()
            Dim opHandler As New OpinionHandler(Globals.ThisAddIn.Application)
            opHandler.OnOpenOpinionFormClick()
        End Sub

        Public Sub ShowBtnGMessage()
            Dim auHandler As New AuthorHandler()
            auHandler.OnOpenAuthorFormClick()
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
            MessageBox.Show("This will handle function K.", "Coming Soon", MessageBoxButton.OK, MessageBoxImage.Information)
        End Sub

        Public Sub ShowBtnLMessage()
            MessageBox.Show("This will handle function L.", "Coming Soon", MessageBoxButton.OK, MessageBoxImage.Information)
        End Sub

    End Class

End Namespace
