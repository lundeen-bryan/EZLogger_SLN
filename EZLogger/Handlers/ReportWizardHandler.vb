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
        ''' </summary>
        ''' <param name="panel">The ReportWizardPanel that owns the controls.</param>
        Public Sub SearchAndPopulatePatientNumber(panel As ReportWizardPanel)

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
        ''' Called by Btn_B. Looks up a patient by number, shows confirmation,
        ''' writes data to custom document properties, and updates the UI.
        ''' </summary>
        ''' <param name="patientNumber">Patient number to look up.</param>
        ''' <param name="panel">The ReportWizardPanel that owns the controls.</param>
        Public Sub LookupPatientAndWriteProperties(patientNumber As String, panel As ReportWizardPanel)

            If String.IsNullOrWhiteSpace(patientNumber) Then
                MsgBoxHelper.Show("No patient number found. Please use the Search button first.")
                Return
            End If

            Dim patient = DatabaseHelper.GetPatientByNumber(patientNumber)

            If patient IsNot Nothing Then
                Dim message As String =
            $"Full Name: {patient.FullName}" & Environment.NewLine &
            $"County: {patient.County}" & Environment.NewLine &
            $"DOB: {DateTime.Parse(patient.DOB).ToString("MM/dd/yyyy")}" & Environment.NewLine & Environment.NewLine &
            "Does this information match the report?"

                Dim config As New MessageBoxConfig With {
            .Message = message,
            .ShowYes = True,
            .ShowNo = True
        }

                MsgBoxHelper.Show(config, Sub(result)
                                              If result = CustomMsgBoxResult.Yes Then
                                                  DocumentPropertyHelper.WriteDataToDocProperties(patient)
                                                  SenderHelper.WriteProcessedBy(Globals.ThisAddIn.Application.ActiveDocument)
                                                  panel.RefreshPatientNameLabel()
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
        ''' Called by Btn_C. Launches the Report Type wizard, using the commitment
        ''' date from the current Word document.
        ''' </summary>
        Public Sub LaunchReportTypeWizard()
            ' Retrieve commitment date from Word document
            Dim commitmentDate As String = DocumentPropertyHelper.GetPropertyValue("Commitment")

            ' Launch the report type selection dialog
            Dim rtHandler As New ReportTypeHandler()
            rtHandler.OnConfirmReportTypeButtonClick(commitmentDate)
        End Sub

        Public Sub ShowBtnDMessage()
            Dim tcarForm As New TCARListHost()
            tcarForm.Show()
        End Sub

        Public Sub ShowBtnEMessage()
            MessageBox.Show("This will present the HTV list.", "Coming Soon", MessageBoxButton.OK, MessageBoxImage.Information)
        End Sub
        Public Sub ShowBtnFMessage()
            MessageBox.Show("This will handle function F.", "Coming Soon", MessageBoxButton.OK, MessageBoxImage.Information)
        End Sub

        Public Sub ShowBtnGMessage()
            MessageBox.Show("This will handle function G.", "Coming Soon", MessageBoxButton.OK, MessageBoxImage.Information)
        End Sub

        Public Sub ShowBtnHMessage()
            MessageBox.Show("This will handle function H.", "Coming Soon", MessageBoxButton.OK, MessageBoxImage.Information)
        End Sub

        Public Sub ShowBtnIMessage()
            MessageBox.Show("This will handle function I.", "Coming Soon", MessageBoxButton.OK, MessageBoxImage.Information)
        End Sub

        Public Sub ShowBtnJMessage()
            MessageBox.Show("This will handle function J.", "Coming Soon", MessageBoxButton.OK, MessageBoxImage.Information)
        End Sub

        Public Sub ShowBtnKMessage()
            MessageBox.Show("This will handle function K.", "Coming Soon", MessageBoxButton.OK, MessageBoxImage.Information)
        End Sub

        Public Sub ShowBtnLMessage()
            MessageBox.Show("This will handle function L.", "Coming Soon", MessageBoxButton.OK, MessageBoxImage.Information)
        End Sub

    End Class

End Namespace
