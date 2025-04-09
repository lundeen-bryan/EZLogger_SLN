Imports Microsoft.Office.Interop.Word
Imports Office = Microsoft.Office.Core
Imports System.Windows.Forms
Imports EZLogger.Models
Imports EZLogger.Helpers

Namespace Helpers

    Public Class DocumentPropertyWriter

        ''' <summary>
        ''' Writes a single custom document property to the active Word document.
        ''' </summary>
        ''' <param name="propertyName">The name of the property to write.</param>
        ''' <param name="value">The string value to assign. If blank/null, the property will not be written.</param>
        Public Shared Sub WriteCustomProperty(propertyName As String, value As String)
            Try
                If String.IsNullOrWhiteSpace(propertyName) OrElse String.IsNullOrWhiteSpace(value) Then Exit Sub

                Dim doc As Document = Globals.ThisAddIn.Application.ActiveDocument
                Dim props As Office.DocumentProperties = CType(doc.CustomDocumentProperties, Office.DocumentProperties)

                If props.Cast(Of Office.DocumentProperty).Any(Function(p) p.Name = propertyName) Then
                    props(propertyName).Value = value
                Else
                    props.Add(propertyName, False, Office.MsoDocProperties.msoPropertyTypeString, value)
                End If

            Catch ex As Exception
                MessageBox.Show($"Error writing property '{propertyName}': {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        ''' <summary>
        ''' Writes selected patient details as custom document properties to the active Word document.
        ''' </summary>
        ''' <param name="patient">A PatientCls object loaded from the database.</param>
        Public Shared Sub WriteDataToDocProperties(patient As PatientCls)
            Try
                ' Write fields from PatientCls
                WriteCustomProperty("Patient Number", patient.PatientNumber)
                WriteCustomProperty("Patient Name", patient.FullName)
                WriteCustomProperty("Firstname", patient.FName)
                WriteCustomProperty("Lastname", patient.LName)
                WriteCustomProperty("Program", patient.P)
                WriteCustomProperty("Unit", patient.U)
                WriteCustomProperty("Classification", patient.Classification)
                WriteCustomProperty("County", patient.County)
                WriteCustomProperty("DOB", patient.DOB)

                ' Calculate age using helper
                Dim age As String = AgeHelper.CalculateAge(Date.Parse(patient.DOB)).ToString()
                WriteCustomProperty("Age", age)

                WriteCustomProperty("Commitment", patient.CommitmentDate)
                WriteCustomProperty("Admission", patient.AdmissionDate)
                WriteCustomProperty("Expiration", patient.Expiration)
                WriteCustomProperty("Assigned To", patient.AssignedTo)

                SenderHelper.WriteProcessedBy()

                MessageBox.Show("Document properties have been updated.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)

            Catch ex As Exception
                MessageBox.Show("Error writing document properties: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

    End Class

End Namespace
