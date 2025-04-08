Imports Microsoft.Office.Interop.Word
Imports Office = Microsoft.Office.Core
Imports System.Windows.Forms
Imports EZLogger.Models
Imports EZLogger.Helpers

Namespace Helpers

    Public Class DocumentPropertyWriter

        ''' <summary>
        ''' Writes selected patient details as custom document properties to the active Word document.
        ''' </summary>
        ''' <param name="patient">A PatientCls object loaded from the database.</param>
        ''' <remarks>
        ''' This method is typically called after the user confirms a match between the displayed patient
        ''' data and the contents of the Word report. Properties are written as strings. Age is calculated
        ''' using the AgeHelper.
        ''' </remarks>
        Public Shared Sub WriteDataToDocProperties(patient As PatientCls)
            Try
                Dim doc As Document = Globals.ThisAddIn.Application.ActiveDocument
                Dim props As Office.DocumentProperties = CType(doc.CustomDocumentProperties, Office.DocumentProperties)

                ' Internal helper to safely write or update a custom property
                Dim writeProp = Sub(name As String, value As String)
                                    If String.IsNullOrWhiteSpace(value) Then Exit Sub

                                    If props.Cast(Of Office.DocumentProperty).Any(Function(p) p.Name = name) Then
                                        props(name).Value = value
                                    Else
                                        props.Add(name, False, Office.MsoDocProperties.msoPropertyTypeString, value)
                                    End If
                                End Sub

                ' Write fields from PatientCls
                writeProp("Patient Number", patient.PatientNumber)
                writeProp("Patient Name", patient.FullName)
                writeProp("Firstname", patient.FName)
                writeProp("Lastname", patient.LName)
                writeProp("Program", patient.P)
                writeProp("Unit", patient.U)
                writeProp("Classification", patient.Classification)
                writeProp("County", patient.County)
                writeProp("DOB", patient.DOB)

                ' Calculate age using helper
                Dim age As String = AgeHelper.CalculateAge(Date.Parse(patient.DOB)).ToString()
                writeProp("Age", age)

                writeProp("Commitment", patient.CommitmentDate)
                writeProp("Admission", patient.AdmissionDate)
                writeProp("Expiration", patient.Expiration)
                writeProp("Assigned To", patient.AssignedTo)

                MessageBox.Show("Document properties have been updated.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)

            Catch ex As Exception
                MessageBox.Show("Error writing document properties: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

    End Class

End Namespace
