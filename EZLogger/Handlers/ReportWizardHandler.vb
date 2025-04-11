Imports System.Windows.Forms
Imports EZLogger.Helpers
Imports Microsoft.Office.Interop.Word
Imports EZLogger.Enums

Namespace Handlers

    Public Class ReportWizardHandler

        ''' <summary>
        ''' Looks up patient info by patient number, prompts the user to confirm,
        ''' and writes custom document properties if the patient is a match.
        ''' </summary>
        ''' <param name="patientNumber">The patient number to look up.</param>
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
                                                  DocumentPropertyWriter.WriteDataToDocProperties(patient)
                                                  SenderHelper.WriteProcessedBy(Globals.ThisAddIn.Application.ActiveDocument)
                                                  panel.RefreshPatientNameLabel()
                                              Else
                                                  MsgBoxHelper.Show("Please check the patient number and try again.")
                                              End If
                                          End Sub)
            Else
                MsgBoxHelper.Show("No patient record found.")
            End If

        End Sub

    End Class

End Namespace
