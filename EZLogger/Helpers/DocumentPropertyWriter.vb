Imports Microsoft.Office.Interop.Word
Imports Office = Microsoft.Office.Core
Imports System.Windows.Forms
Imports MessageBox = System.Windows.Forms.MessageBox

Namespace Helpers

    Public Class DocumentPropertyWriter

        Public Shared Sub WriteMailMergeDataToDocProperties()
            Try
                Dim doc As Document = Globals.ThisAddIn.Application.ActiveDocument
                Dim props As Office.DocumentProperties = CType(doc.CustomDocumentProperties, Office.DocumentProperties)

                If doc.MailMerge.MainDocumentType = WdMailMergeMainDocType.wdNotAMergeDocument Then
                    MessageBox.Show("No mail merge data source is linked.", "Missing Data", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    Exit Sub
                End If

                Dim fields = doc.MailMerge.DataSource.DataFields

                ' Helper to write properties
                Dim writeProp = Sub(name As String, value As String)
                                    If String.IsNullOrWhiteSpace(value) Then Exit Sub
                                    If props.Cast(Of Office.DocumentProperty).Any(Function(p) p.Name = name) Then
                                        props(name).Value = value
                                    Else
                                        props.Add(name, False, Office.MsoDocProperties.msoPropertyTypeString, value)
                                    End If
                                End Sub

                ' Write selected fields
                writeProp("Patient Number", fields("patient_number").Value)
                writeProp("Patient Name", fields("fullname").Value)
                writeProp("Firstname", fields("fname").Value)
                writeProp("Lastname", fields("lname").Value)
                writeProp("Program", fields("program").Value)
                writeProp("Unit", fields("u").Value)
                writeProp("Classification", fields("class").Value)
                writeProp("County", fields("county").Value)
                writeProp("DOB", fields("dob").Value)
                writeProp("Age", fields("age").Value)
                writeProp("Gender", fields("sex").Value)
                writeProp("Commitment", fields("commitment_date").Value)
                writeProp("Admission", fields("admission_date").Value)
                writeProp("Expiration", fields("expiration").Value)
                writeProp("Assigned To", fields("assigned_to").Value)

                MessageBox.Show("Document properties have been updated.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)

            Catch ex As Exception
                MessageBox.Show("Error writing document properties: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

    End Class

End Namespace
