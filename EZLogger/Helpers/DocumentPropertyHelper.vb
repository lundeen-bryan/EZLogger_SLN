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
                Dim doc As Document = DocumentHelper.GetActiveWordDocument()
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
                writeProp("Name", patient.LName & ", " & patient.FName)
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
        ''' Safely writes a custom document property without relying on VSTO-hosted interfaces.
        ''' Works with Interop.Word.DocumentClass and avoids E_NOINTERFACE exceptions.
        ''' </summary>
        ''' <param name="doc">The Word document object (Interop only).</param>
        ''' <param name="name">The property name to set.</param>
        ''' <param name="value">The value to assign.</param>
        Public Shared Sub WriteCustomProperty(doc As Document, name As String, value As String)
            Try
                If doc Is Nothing OrElse String.IsNullOrWhiteSpace(name) Then Exit Sub

                ' Late bind to the CustomDocumentProperties collection
                Dim props As Object = doc.CustomDocumentProperties

                Dim found As Boolean = False

                ' Loop over existing properties to see if the name already exists
                For Each prop As Object In props
                    Dim propName As String = CStr(prop.Name)
                    If String.Equals(propName, name, StringComparison.OrdinalIgnoreCase) Then
                        prop.Value = value
                        found = True
                        Exit For
                    End If
                Next

                ' If not found, add it
                If Not found Then
                    props.Add(name, False, Microsoft.Office.Core.MsoDocProperties.msoPropertyTypeString, value)
                End If

            Catch ex As Exception
                ErrorHelper.HandleError("DocumentPropertyHelper.WriteCustomProperty", ex.HResult.ToString(), ex.Message,
                                "Could not write document property. Make sure the file is open and not read-only.")
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
                Dim doc As Document = DocumentHelper.GetActiveWordDocument()
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
                Dim doc = DocumentHelper.GetActiveWordDocument()
                ' Helper to retrieve document properties
                Dim getProp = Function(name As String) DocumentPropertyHelper.GetPropertyValue(doc, name)

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
                Dim doc As Document = DocumentHelper.GetActiveWordDocument()
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
        ''' Retrieves the value of a custom document property from the specified Word document.
        ''' Returns an empty string if the property does not exist or an error occurs.
        ''' </summary>
        ''' <param name="doc">The Word document to retrieve the property from.</param>
        ''' <param name="propertyName">The name of the custom document property.</param>
        ''' <param name="caseInsensitive">If true, performs a case-insensitive comparison.</param>
        ''' <returns>The property value as a string, or an empty string if not found or an error occurs.</returns>
        Public Shared Function GetPropertyValue(doc As Microsoft.Office.Interop.Word.Document,
                                        propertyName As String,
                                        Optional caseInsensitive As Boolean = False) As String
            Try
                If doc Is Nothing Then Return String.Empty

                For Each prop As Office.DocumentProperty In doc.CustomDocumentProperties
                    If String.Compare(prop.Name, propertyName, caseInsensitive) = 0 Then
                        Return prop.Value.ToString()
                    End If
                Next

            Catch ex As Exception
                ' Optionally log or swallow the error
            End Try

            Return String.Empty
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
                Dim doc As Document = DocumentHelper.GetActiveWordDocument()
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
                Dim doc As Document = DocumentHelper.GetActiveWordDocument()
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
