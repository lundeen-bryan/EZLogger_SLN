' Namespace=EZLogger/Handlers
' Filename=PrcHandler.vb
' !See Label Footer for notes

Imports EZLogger.Helpers
Imports Microsoft.Office.Interop.Word
Imports System.Diagnostics
Imports System.IO
Imports System.Windows

Namespace Handlers

    ''' <summary>
    ''' Coordinates the process of saving processed report data:
    ''' 1. Updates SharePoint metadata (SPHelper)
    ''' 2. Inserts record into EZL_PRC table (DatabaseHelper)
    ''' 3. Appends entry to the user TODO list (_LogTheseFiles.txt) (UserTodoHelper)
    ''' </summary>
    Public Module PrcHandler
        Private Function IsValidWordDoc(doc As Document) As Boolean
            Try
                Dim dummy = doc.Name ' Triggers COM if invalid
                Return True
            Catch
                Return False
            End Try
        End Function

        Private Sub AppendToTodoLog(doc As Document)
            Try
                Dim todoEntry As String = $"{GetDocProp(doc, "Patient Name")}{vbTab}" &
                                  $"{GetDocProp(doc, "Report Type")}{vbTab}" &
                                  $"{SafeFormatDateDisplay(GetDocProp(doc, "Report Date"))}{vbTab}" &
                                  $"P{GetDocProp(doc, "Program")}"

                Dim todoFilePath As String = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "_LogTheseFiles.txt")
                UserTodoHelper.AppendTodoEntry(todoFilePath, todoEntry)

            Catch ex As Exception
                ErrorHelper.HandleError("PrcHandler.AppendToTodoLog", ex.HResult.ToString(), ex.Message,
                                "Failed to append report entry to the ToDo list. Please check the file path and permissions.")
            End Try
        End Sub

        ''' <summary>
        ''' Inserts the processed report data into the EZL_PRC table and returns whether it succeeded.
        ''' </summary>
        ''' <param name="doc">The Word document associated with the report.</param>
        ''' <param name="prcData">The key-value pairs to insert into the EZL_PRC table.</param>
        ''' <returns>True if the insert succeeded; otherwise, False.</returns>
        Private Function InsertProcessedReport(prcData As Dictionary(Of String, Object)) As Boolean
            Try
                Dim normalizedData = DatabaseHelper.NormalizePrcData(prcData)
                DatabaseHelper.InsertPrcTable(normalizedData)
                Return True

            Catch ex As Exception
                ErrorHelper.HandleError("PrcHandler.InsertProcessedReport", ex.HResult.ToString(), ex.Message,
                                "Failed to log the processed report to the PRC table. Please verify the document is open and has valid data.")
                Return False
            End Try
        End Function

        ''' <summary>
        ''' Adds the report filename to the Tasks.xml list if it does not already exist.
        ''' </summary>
        ''' <param name="doc">The Word document whose filename will be checked and potentially added to the task list.</param>
        Private Sub AddToTaskListIfMissing(doc As Document)
            Try
                Dim fileName As String = Path.GetFileName(doc.FullName)
                Dim taskHandler As New TaskListHandler()

                Dim alreadyExists As Boolean =
            taskHandler.Tasks.Any(Function(t) String.Equals(t.Notes, fileName, StringComparison.OrdinalIgnoreCase))

                If Not alreadyExists Then
                    taskHandler.AddTaskFromReport(fileName)
                End If

            Catch ex As Exception
                ErrorHelper.HandleError("PrcHandler.AddToTaskListIfMissing", ex.HResult.ToString(), ex.Message,
                                "Failed to add the report to the task list. Please check the task list structure and try again.")
            End Try
        End Sub


        ''' <summary>
        ''' Builds the key-value pairs required for inserting a row into the EZL_PRC table.
        ''' </summary>
        ''' <param name="doc">The active Word document.</param>
        ''' <returns>A dictionary containing all relevant PRC fields and values.</returns>
        Private Function BuildPrcData(doc As Document) As Dictionary(Of String, Object)
            Try
                Return New Dictionary(Of String, Object) From {
                    {"PatientNumber", GetDocProp(doc, "Patient Number")},
                    {"FirstPatientNumber", GetDocProp(doc, "First Patient Number")},
                    {"Created", DateTime.UtcNow.ToString("yyyy-MM-dd")},
                    {"Filename", Path.GetFileName(doc.FullName)},
                    {"PatientName", GetDocProp(doc, "Patient Name")},
                    {"Name", GetDocProp(doc, "Name")},
                    {"DueDate", SafeFormatDate(GetDocProp(doc, "Due Date"))},
                    {"RushStatus", GetDocProp(doc, "Rush Status")},
                    {"ReportDate", SafeFormatDate(GetDocProp(doc, "Report Date"))},
                    {"ReportType", GetDocProp(doc, "Report Type")},
                    {"ReportCycle", GetDocProp(doc, "Report Cycle")},
                    {"County", GetDocProp(doc, "County")},
                    {"Classification", GetDocProp(doc, "Classification")},
                    {"Evaluator", GetDocProp(doc, "Evaluator")},
                    {"ApprovedBy", GetDocProp(doc, "Approved By")},
                    {"ProcessedBy", GetDocProp(doc, "Processed By")},
                    {"Program", GetDocProp(doc, "Program")},
                    {"Unit", GetDocProp(doc, "Unit")},
                    {"DueDateOffset", GetDocProp(doc, "Days Since Due")},
                    {"Commitment", GetDocProp(doc, "Commitment")},
                    {"Admission", GetDocProp(doc, "Admission")},
                    {"Expiration", GetDocProp(doc, "Expiration")},
                    {"CourtNumber", GetDocProp(doc, "Court Number")},
                    {"Charges", GetDocProp(doc, "Charges")},
                    {"Sex", GetDocProp(doc, "Sex")},
                    {"Dob", SafeFormatDate(GetDocProp(doc, "DOB"))},
                    {"Age", GetDocProp(doc, "Age")},
                    {"Language", GetDocProp(doc, "Language")},
                    {"Pages", GetDocProp(doc, "Pages")},
                    {"Psychiatrist", GetDocProp(doc, "Psychiatrist")},
                    {"UID", GetDocProp(doc, "Unique ID")},
                    {"MinuteOrder", GetDocProp(doc, "Minute Order")},
                    {"Malingering", GetDocProp(doc, "Malingering")},
                    {"IMO", GetDocProp(doc, "IMO")},
                    {"JBCT", GetDocProp(doc, "JBCT")},
                    {"TCAR", SafeFormatDate(GetDocProp(doc, "TCAR Referral Date"))},
                    {"TcarOffset", GetDocProp(doc, "Days Since TCAR")}
                }

            Catch ex As Exception
                ErrorHelper.HandleError("PrcHandler.BuildPrcData", ex.HResult.ToString(), ex.Message,
                                "Failed to build PRC data. Please confirm that all required document properties are present.")
                Return New Dictionary(Of String, Object)
            End Try
        End Function

        ''' <summary>
        ''' Returns True if the 'PrcInserted' custom document property equals "true" (case-insensitive).
        ''' </summary>
        ''' <param name="doc">The Word document to check.</param>
        ''' <returns>True if marked as inserted; otherwise, False.</returns>
        Public Function ConfirmReportInPrc(doc As Word.Document) As Boolean
            Dim value As String = DocumentPropertyHelper.GetPropertyValue(doc, "PrcInserted", caseInsensitive:=True)
            Return value.Trim().ToLower() = "true"
        End Function


        ''' <summary>
        ''' Coordinates the process of saving a completed report: appends to ToDo list, updates task list, and logs to PRC.
        ''' </summary>
        ''' <param name="doc">The Word document containing the report data.</param>
        Public Sub SaveProcessedReport(doc As Document)
            Const functionName As String = "PrcHandler.SaveProcessedReport"
            Dim recommendation As String =
                "One of the required steps failed: logging to ToDo, saving the file, or writing to the database. " &
                "Please close and reopen the document, confirm all fields are filled in, and try again. " &
                "If the issue continues, use the copy button in this error dialog to copy the error and show it to the developers."

            If doc Is Nothing Then Exit Sub

            Try
                ' Step 1: Validate document
                If Not IsValidWordDoc(doc) Then
                    MessageBox.Show("The document is no longer available or is invalid.", "Invalid Document", MessageBoxButton.OK, MessageBoxImage.Warning)
                    Exit Sub
                End If

                ' Step 2: Append to ToDo log
                AppendToTodoLog(doc)

                ' Step 3: Add to TaskList if not already present
                AddToTaskListIfMissing(doc)

                ' Step 4: Save document (to ensure any updated metadata is persisted)
                doc.Save()

                ' Step 5: Build PRC data
                Dim prcData As Dictionary(Of String, Object) = BuildPrcData(doc)

                ' Step 6: Insert into SQL last if this report wasn't alreadyh logged
                If ConfirmReportInPrc(doc) Then
                    MsgBoxHelper.Show("This report has already been saved in the Processed Report Container (EZL_PRC).")
                    Exit Sub
                End If

                Dim successfulInsert As Boolean = InsertProcessedReport(prcData)
                If Not successfulInsert Then
                    MessageBox.Show("There was an error logging the report to the PRC table. Please try again or contact support.",
                            "Insert Failed", MessageBoxButton.OK, MessageBoxImage.Error)
                Else
                    DocumentPropertyHelper.MarkReportAsInserted(doc)
                End If

            Catch ex As Exception
                Dim errNum As String = ex.HResult.ToString()
                Dim errMsg As String = CStr(ex.Message)

                ErrorHelper.HandleError(functionName, errNum, errMsg, recommendation)
            End Try
        End Sub


        ''' <summary>
        ''' Retrieves a document property value safely.
        ''' Returns empty string if property does not exist or error occurs.
        ''' </summary>
        Private Function GetDocProp(doc As Document, propName As String) As String
            Try
                Return doc.CustomDocumentProperties(propName).Value.ToString()
            Catch
                Return String.Empty
            End Try
        End Function

        ''' <summary>
        ''' Formats a date string to ISO (yyyy-MM-dd) or returns empty if invalid.
        ''' </summary>
        Private Function SafeFormatDate(dateString As String) As Object
            Dim dt As DateTime
            If DateTime.TryParse(dateString, dt) Then
                Return dt.ToString("yyyy-MM-dd")
            Else
                Return DBNull.Value
            End If
        End Function

        ''' <summary>
        ''' Formats a date string to human-readable (MM/dd/yyyy) or returns empty if invalid.
        ''' </summary>
        Private Function SafeFormatDateDisplay(dateString As String) As String
            Dim dt As DateTime
            If DateTime.TryParse(dateString, dt) Then
                Return dt.ToString("MM/dd/yyyy")
            Else
                Return String.Empty
            End If
        End Function

    End Module

End Namespace

' Footer:
''===========================================================================================
'' Filename: .......... PrcHandler.vb
'' Description: ....... Handles the adding of data to the EZL_PRC table
'' Created: ........... 2025-05-02
'' Updated: ........... 2025-05-02
'' Installs to: ....... EZLogger/Handlers
'' Compatibility: ..... VSTO, WPF
'' Contact Author: .... lundeen-bryan
'' Copyright:  ........ ©2025. All rights reserved.
'' Notes: ............. _
' (1) notes_here
''===========================================================================================