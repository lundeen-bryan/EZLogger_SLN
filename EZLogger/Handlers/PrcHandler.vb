' Namespace=EZLogger/Handlers
' Filename=PrcHandler.vb
' !See Label Footer for notes

Imports Microsoft.Office.Interop.Word
Imports EZLogger.Helpers
Imports System.IO

Namespace Handlers

    ''' <summary>
    ''' Coordinates the process of saving processed report data:
    ''' 1. Updates SharePoint metadata (SPHelper)
    ''' 2. Inserts record into EZL_PRC table (DatabaseHelper)
    ''' 3. Appends entry to the user TODO list (_LogTheseFiles.txt) (UserTodoHelper)
    ''' </summary>
    Public Module PrcHandler

        ''' <summary>
        ''' Processes a completed report: updates SharePoint, SQL, and local TODO log.
        ''' </summary>
        ''' <param name="doc">The Word document containing report metadata.</param>
        Public Sub SaveProcessedReport(doc As Document)
            If doc Is Nothing Then Exit Sub

            Try
                ' Step 1: Update SharePoint metadata
                SpHelper.UpdateMetadata(doc)

                ' Step 2: Prepare data for SQL insertion
                Dim prcData As New Dictionary(Of String, Object) From {
                    {"PatientNumber", GetDocProp(doc, "Patient Number")},
                    {"FirstPatientNumber", GetDocProp(doc, "First Patient Number")},
                    {"Created", DateTime.UtcNow.ToString("yyyy-MM-dd")},
                    {"Filename", Path.GetFileName(doc.FullName)},
                    {"PatientName", GetDocProp(doc, "Patient Name")},
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
                    {"Sex", GetDocProp(doc, "Gender")},
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

                DatabaseHelper.InsertPrcTable(prcData)

                ' Step 3: Append to _LogTheseFiles.txt
                Dim todoEntry As String = $"{GetDocProp(doc, "Patient Name")}{vbTab}" &
                                          $"{GetDocProp(doc, "Report Type")}{vbTab}" &
                                          $"{SafeFormatDateDisplay(GetDocProp(doc, "Report Date"))}{vbTab}" &
                                          $"P{GetDocProp(doc, "Program")}"

                Dim todoFilePath As String = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "_LogTheseFiles.txt")

                UserTodoHelper.AppendTodoEntry(todoFilePath, todoEntry)

                ' Step 4: Add to TaskList (Tasks.xml) if not already present
                Dim fileName As String = Path.GetFileName(doc.FullName)
                Dim taskHandler As New TaskListHandler()

                Dim alreadyExists As Boolean = taskHandler.Tasks.Any(Function(t) String.Equals(t.Notes, fileName, StringComparison.OrdinalIgnoreCase))

                If Not alreadyExists Then
                    taskHandler.AddTaskFromReport(fileName)
                End If


                ' Save document to finalize SharePoint changes
                doc.Save()

            Catch ex As Exception
                LogHelper.LogError("PrcHandler.SaveProcessedReport", ex.Message)
                System.Windows.MessageBox.Show("An error occurred while saving the processed report.", "Processing Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error)
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