Imports Microsoft.Office.Interop.Word
Imports System.Windows.Forms


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

                MsgBoxHelper.Show("Document properties have been updated.")

            Catch ex As Exception
                MsgBoxHelper.Show("Error writing document properties: " & ex.Message)
            End Try
        End Sub

        ''' <summary>
        ''' Writes a single custom document property to the active Word document.
        ''' </summary>
        ''' <param name="doc">The target Word document.</param>
        ''' <param name="name">The name of the property.</param>
        ''' <param name="value">The value to write.</param>
        Public Shared Sub WriteCustomProperty(doc As Document, name As String, value As String)
            Try
                Dim props As Office.DocumentProperties = CType(doc.CustomDocumentProperties, Office.DocumentProperties)

                If String.IsNullOrWhiteSpace(value) Then Exit Sub

                If props.Cast(Of Office.DocumentProperty).Any(Function(p) p.Name = name) Then
                    props(name).Value = value
                Else
                    props.Add(name, False, Office.MsoDocProperties.msoPropertyTypeString, value)
                End If

            Catch ex As Exception
                MessageBox.Show("Error writing custom property: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub
        Public Shared Function GetCustomProperty(name As String) As String
            Try
                Dim doc As Document = Globals.ThisAddIn.Application.ActiveDocument
                Dim props As Office.DocumentProperties = CType(doc.CustomDocumentProperties, Office.DocumentProperties)

                If props.Cast(Of Office.DocumentProperty).Any(Function(p) p.Name = name) Then
                    Return props(name).Value.ToString()
                End If

            Catch ex As Exception
                ' Optional: log or ignore
            End Try

            Return String.Empty
        End Function

        ''' <summary>
        ''' Writes key metadata to the built-in document properties of the active Word document.
        ''' </summary>
        ''' <param name="patientName">Patient's full name.</param>
        ''' <param name="reportType">Type of forensic report.</param>
        ''' <param name="reportDate">Date of the report (string in "yyyy-MM-dd" format).</param>
        ''' <param name="program">Program name or code.</param>
        ''' <param name="unit">Unit or team identifier.</param>
        ''' <param name="evaluator">Name of the evaluator or author.</param>
        ''' <param name="processedBy">Name of the person who processed the report.</param>
        ''' <param name="county">The county the report pertains to.</param>
        Public Sub SaveBuiltInProperties(patientName As String,
                                  reportType As String,
                                  reportDate As String,
                                  program As String,
                                  unit As String,
                                  evaluator As String,
                                  processedBy As String,
                                  county As String)

            Dim doc As Document = Globals.ThisAddIn.Application.ActiveDocument
            Dim todaysDate As String = DateTime.Now.ToString("yyyy-MM-dd")

            ' Ensure reportDate is formatted consistently
            Dim formattedReportDate As String
            If DateTime.TryParse(reportDate, Nothing) Then
                formattedReportDate = DateTime.Parse(reportDate).ToString("yyyy-MM-dd")
            Else
                formattedReportDate = reportDate ' fallback if it's already formatted
            End If

            ' Format Title: Proper-case name + report type + date
            Dim titleValue As String = StrConv(patientName, VbStrConv.ProperCase) & " " &
                               reportType & " " &
                               formattedReportDate

            ' Format Subject: Program and Unit
            Dim subjectValue As String = "Program " & program & " Unit " & unit

            ' Format Comments: Processed by info
            Dim commentsValue As String = "Processed by " & processedBy & " " & todaysDate & vbCrLf &
                                  "For " & county

            Try
                doc.BuiltInDocumentProperties("Title").Value = titleValue
                doc.BuiltInDocumentProperties("Subject").Value = subjectValue
                doc.BuiltInDocumentProperties("Author").Value = evaluator
                doc.BuiltInDocumentProperties("Company").Value = "Unit " & unit
                doc.BuiltInDocumentProperties("Comments").Value = commentsValue
            Catch ex As Exception
                ' You might want to log this or show a message to the user
                System.Diagnostics.Debug.WriteLine("Failed to write built-in properties: " & ex.Message)
            End Try

        End Sub

    End Class

End Namespace
