Imports System.Windows
Imports System.Windows.Forms
Imports EZLogger.Helpers
Imports EZLogger.Handlers ' If needed for the handler reference
Imports MessageBox = System.Windows.MessageBox
Imports Microsoft.Office.Interop.Word

Namespace Handlers

    Public Class ReportWizardHandler

        ''' <summary>
        ''' Reads the patient number from the document footer.
        ''' </summary>
        Public Function OnSearchButtonClick() As String
            Dim reader As New WordFooterReader()
            Return reader.FindPatientNumberInFooter()
        End Function

        ''' <summary>
        ''' Looks up patient info by patient number, prompts the user to confirm,
        ''' and writes custom document properties if the patient is a match.
        ''' </summary>
        ''' <param name="patientNumber">The patient number to look up.</param>
        Public Sub LookupPatientAndWriteProperties(patientNumber As String, panel As ReportWizardPanel)
            If String.IsNullOrWhiteSpace(patientNumber) Then
                MessageBox.Show("No patient number found. Please use the Search button first.",
                                "Missing Data",
                                MessageBoxButton.OK,
                                MessageBoxImage.Warning)
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

                ' Show modeless CustomMsgBox and continue in the callback
                CustomMsgBoxHandler.ShowNonModal(config, Sub(result)
                                                             If result = CustomMsgBoxResult.Yes Then
                                                                 DocumentPropertyWriter.WriteDataToDocProperties(patient)
                                                                 SenderHelper.WriteProcessedBy(Globals.ThisAddIn.Application.ActiveDocument)
                                                                 panel.RefreshPatientNameLabel()
                                                             Else
                                                                 MessageBox.Show("Please check the patient number and try again.",
                                                                                 "No Match",
                                                                                 MessageBoxButton.OK,
                                                                                 MessageBoxImage.Information)
                                                             End If
                                                         End Sub)

            Else
                MessageBox.Show("No patient record found.",
                                "Not Found",
                                MessageBoxButton.OK,
                                MessageBoxImage.Information)
            End If
        End Sub

    End Class

End Namespace
