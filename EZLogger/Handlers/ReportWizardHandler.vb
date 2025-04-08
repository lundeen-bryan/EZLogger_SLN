Imports Microsoft.Office.Interop.Word
Imports System.Windows
Imports System.Windows.Forms
Imports EZLogger.Models
Imports EZLogger.Helpers
Imports MessageBox = System.Windows.MessageBox

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
        Public Sub LookupPatientAndWriteProperties(patientNumber As String)
            If String.IsNullOrWhiteSpace(patientNumber) Then
                MessageBox.Show("No patient number found. Please use the Search button first.", "Missing Data", MessageBoxButton.OK, MessageBoxImage.Warning)
                Return
            End If

            Dim patient = DatabaseHelper.GetPatientByNumber(patientNumber)

            If patient IsNot Nothing Then
                Dim message As String =
                    $"Full Name: {patient.FullName}" & Environment.NewLine &
                    $"County: {patient.County}" & Environment.NewLine &
                    $"DOB: {DateTime.Parse(patient.DOB).ToString("MM/dd/yyyy")}" & Environment.NewLine & Environment.NewLine &
                    "Does this information match the report?"

                Dim result As MessageBoxResult = MessageBox.Show(message, "Patient Details", MessageBoxButton.YesNo, MessageBoxImage.Question)

                If result = MessageBoxResult.Yes Then
                    DocumentPropertyWriter.WriteDataToDocProperties(patient)
                Else
                    MessageBox.Show("Please check the patient number and try again.", "No Match", MessageBoxButton.OK, MessageBoxImage.Information)
                End If
            Else
                MessageBox.Show("No patient record found.", "Not Found", MessageBoxButton.OK, MessageBoxImage.Information)
            End If
        End Sub

    End Class

End Namespace
