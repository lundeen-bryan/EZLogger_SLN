Imports Microsoft.Office.Interop.Word
Imports System.IdentityModel.Protocols.WSTrust
Imports System.Windows.Forms
Imports MessageBox = System.Windows.MessageBox

Namespace Helpers

    ''' <summary>
    ''' Helper class for reading and writing both custom and built-in document properties in Word.
    ''' </summary>
    Public Class DocumentPropertyHelper

        ' ========================
        ' === WRITE OPERATIONS ===
        ' ========================

        ''' <summary>
        ''' Writes all patient data as custom document properties to the active Word document.
        ''' </summary>
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

                writeProp("Patient Number", FormatPatientNumber(patient.PatientNumber))
                writeProp("First Patient Number", FormatPatientNumber(patient.FirstPatientNumber))
                writeProp("Patient Name", patient.PatientName)
                writeProp("Firstname", patient.FName)
                writeProp("Lastname", patient.LName)
                writeProp("Program", patient.Program)
                writeProp("Unit", patient.Unit)
                writeProp("Classification", patient.Classification)
                writeProp("County", patient.County)
                writeProp("Bed Status", patient.BedStatus)
                writeProp("Court Number", patient.CourtNumber)
                writeProp("DOB", patient.DOB)
                writeProp("Sex", patient.Sex)

                ' Age calculated using a separate helper
                Dim age As String = AgeHelper.CalculateAge(Date.Parse(patient.DOB)).ToString()
                writeProp("Age", age)

                writeProp("Commitment", patient.CommitmentDate)
                writeProp("Admission", patient.AdmissionDate)
                writeProp("Expiration", patient.Expiration)
                writeProp("Assigned To", patient.Evaluator)
                writeProp("Early90Day", patient.EarlyNinetyDay.ToString())

            Catch ex As Exception
                MsgBoxHelper.Show("Error writing document properties: " & ex.Message)
            End Try
        End Sub

        ''' <summary>
        ''' Writes a single custom document property to the given Word document.
        ''' </summary>
        Public Shared Sub WriteCustomProperty(doc As Document, name As String, value As String)
            Try
                Dim props As Office.DocumentProperties = CType(doc.CustomDocumentProperties, Office.DocumentProperties)

                'If String.IsNullOrWhiteSpace(value) Then Exit Sub
                '^--If no value is passed exit sub, commented out to give blank properties

                If props.Cast(Of Office.DocumentProperty).Any(Function(p) p.Name = name) Then
                    props(name).Value = value
                Else
                    props.Add(name, False, Office.MsoDocProperties.msoPropertyTypeString, value)
                End If
            Catch ex As Exception
                MessageBox.Show("Error writing custom property: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        ''' <summary>
        ''' Writes built-in document properties such as Title, Author, Subject, etc.
        ''' </summary>
        Public Shared Sub SaveBuiltInProperties(patientName As String,
                                                reportType As String,
                                                reportDate As String,
                                                program As String,
                                                unit As String,
                                                evaluator As String,
                                                processedBy As String,
                                                county As String)
            Try
                Dim doc As Document = Globals.ThisAddIn.Application.ActiveDocument
                Dim todaysDate As String = DateTime.Now.ToString("yyyy-MM-dd")

                Dim formattedReportDate As String
                If DateTime.TryParse(reportDate, Nothing) Then
                    formattedReportDate = DateTime.Parse(reportDate).ToString("yyyy-MM-dd")
                Else
                    formattedReportDate = reportDate
                End If

                Dim titleValue As String = StrConv(patientName, VbStrConv.ProperCase) & " " & reportType & " " & formattedReportDate
                Dim subjectValue As String = "Program " & program & " Unit " & unit
                Dim commentsValue As String = "Processed by " & processedBy & " " & todaysDate & vbCrLf & "For " & county

                doc.BuiltInDocumentProperties("Title").Value = titleValue
                doc.BuiltInDocumentProperties("Subject").Value = subjectValue
                doc.BuiltInDocumentProperties("Author").Value = evaluator
                doc.BuiltInDocumentProperties("Company").Value = "Unit " & unit
                doc.BuiltInDocumentProperties("Comments").Value = commentsValue

            Catch ex As Exception
                System.Diagnostics.Debug.WriteLine("Failed to write built-in properties: " & ex.Message)
            End Try
        End Sub

        ''' <summary>
        ''' Builds a unique ID string from standard document properties.
        ''' </summary>
        Public Shared Function CreateUniqueIdFromProperties() As String
            Try
                ' Helper to retrieve document properties
                Dim getProp = Function(name As String) DocumentPropertyHelper.GetPropertyValue(name)

                ' Extract necessary fields
                Dim patientNumber = getProp("Patient Number")
                Dim reportType = getProp("Report Type")
                Dim reportDateStr = getProp("Report Date")

                ' Parse the date safely
                Dim reportDate As Date
                If Not Date.TryParse(reportDateStr, reportDate) Then
                    MsgBoxHelper.Show("Invalid or missing Report Date.")
                    Return String.Empty
                End If

                ' Format components
                Dim mo As String = reportDate.Month.ToString().PadLeft(2, "0"c)
                Dim da As String = reportDate.Day.ToString().PadLeft(2, "0"c)
                Dim currentTime As String = "|" & Date.Now.ToString("HHmmss")

                ' Build unique ID
                Dim baseId As String
                If reportType = "PPR" Then
                    baseId = $"{patientNumber},{reportType.Substring(0, 3)}{mo}{da}"
                Else
                    baseId = $"{patientNumber},{reportType.Substring(0, Math.Min(4, reportType.Length))}{mo}{da}"
                End If

                Return baseId & currentTime

            Catch ex As Exception
                MsgBoxHelper.Show("Error creating unique ID: " & ex.Message)
                Return String.Empty
            End Try
        End Function

        ' ========================
        ' === READ OPERATIONS ===
        ' ========================

        ''' <summary>
        ''' Checks if a custom document property exists in the active document.
        ''' </summary>
        Public Shared Function PropertyExists(propertyName As String, Optional caseInsensitive As Boolean = False) As Boolean
            Try
                Dim doc As Document = Globals.ThisAddIn.Application.ActiveDocument
                If doc Is Nothing Then Return False

                For Each prop As Office.DocumentProperty In doc.CustomDocumentProperties
                    If String.Compare(prop.Name, propertyName, caseInsensitive) = 0 Then
                        Return True
                    End If
                Next
            Catch ex As Exception
            End Try

            Return False
        End Function

        ''' <summary>
        ''' Attempts to read a custom document property value as a string.
        ''' </summary>
        Public Shared Function GetPropertyValue(propertyName As String, Optional caseInsensitive As Boolean = False) As String
            Try
                Dim doc As Document = Globals.ThisAddIn.Application.ActiveDocument
                If doc Is Nothing Then Return Nothing

                For Each prop As Office.DocumentProperty In doc.CustomDocumentProperties
                    If String.Compare(prop.Name, propertyName, caseInsensitive) = 0 Then
                        Return prop.Value.ToString()
                    End If
                Next
            Catch ex As Exception
                ' Log or swallow errors as needed
            End Try

            Return Nothing
        End Function

        ''' <summary>
        ''' Returns True if the specified property exists and equals the given value.
        ''' </summary>
        Public Shared Function PropertyEquals(propertyName As String, expectedValue As String, Optional caseInsensitive As Boolean = False) As Boolean
            Dim actualValue As String = GetPropertyValue(propertyName, caseInsensitive)
            Return String.Equals(actualValue, expectedValue, StringComparison.OrdinalIgnoreCase)
        End Function

        ''' <summary>
        ''' Deletes a specific custom document property from the active Word document.
        ''' </summary>
        Public Shared Sub DeleteCustomProperty(propertyName As String)
            Try
                Dim doc As Document = Globals.ThisAddIn.Application.ActiveDocument
                Dim props As Office.DocumentProperties = CType(doc.CustomDocumentProperties, Office.DocumentProperties)

                If props.Cast(Of Office.DocumentProperty).Any(Function(p) p.Name = propertyName) Then
                    props(propertyName).Delete()
                End If

            Catch ex As Exception
                MessageBox.Show("Error deleting property: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        ''' <summary>
        ''' Deletes all custom document properties from the active Word document.
        ''' </summary>
        Public Shared Sub DeleteAllCustomProperties()
            Try
                Dim doc As Document = Globals.ThisAddIn.Application.ActiveDocument
                Dim props As Office.DocumentProperties = CType(doc.CustomDocumentProperties, Office.DocumentProperties)

                ' Copy property names to avoid modifying the collection while iterating
                Dim namesToDelete = props.Cast(Of Office.DocumentProperty).Select(Function(p) p.Name).ToList()

                For Each name In namesToDelete
                    props(name).Delete()
                Next

            Catch ex As Exception
                MessageBox.Show("Error deleting all properties: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

    End Class

End Namespace
