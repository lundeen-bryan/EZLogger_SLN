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
                    {"patient_number", GetDocProp(doc, "Patient Number")},
                    {"created", DateTime.UtcNow.ToString("yyyy-MM-dd")},
                    {"filename", Path.GetFileName(doc.FullName)},
                    {"fullname", GetDocProp(doc, "Patient Name")},
                    {"due_date", SafeFormatDate(GetDocProp(doc, "Due Date"))},
                    {"rush_status", GetDocProp(doc, "Rush Status")},
                    {"report_date", SafeFormatDate(GetDocProp(doc, "Report Date"))},
                    {"report_type", GetDocProp(doc, "Report Type")},
                    {"report_cycle", GetDocProp(doc, "Report Cycle")},
                    {"county", GetDocProp(doc, "County")},
                    {"class", GetDocProp(doc, "Classification")},
                    {"evaluator", GetDocProp(doc, "Evaluator")},
                    {"approved_by", GetDocProp(doc, "Approved By")},
                    {"processed_by", GetDocProp(doc, "Processed By")},
                    {"program", GetDocProp(doc, "Program")},
                    {"unit", GetDocProp(doc, "Unit")},
                    {"days_since_due", GetDocProp(doc, "Days Since Due")},
                    {"commitment", GetDocProp(doc, "Commitment")},
                    {"admission", GetDocProp(doc, "Admission")},
                    {"expiration", GetDocProp(doc, "Expiration")},
                    {"court_numbers", GetDocProp(doc, "Court Number")},
                    {"charges", GetDocProp(doc, "Charges")},
                    {"sex", GetDocProp(doc, "Gender")},
                    {"dob", SafeFormatDate(GetDocProp(doc, "DOB"))},
                    {"age", GetDocProp(doc, "Age")},
                    {"language", GetDocProp(doc, "Language")},
                    {"pages", GetDocProp(doc, "Pages")},
                    {"psychiatrist", GetDocProp(doc, "Psychiatrist")},
                    {"unique_id", GetDocProp(doc, "Unique ID")},
                    {"malingering", GetDocProp(doc, "Malingering")},
                    {"imo", GetDocProp(doc, "IMO")},
                    {"jbct", GetDocProp(doc, "JBCT")},
                    {"tcar_date", SafeFormatDate(GetDocProp(doc, "TCAR Referral Date"))},
                    {"days_since_tcar", GetDocProp(doc, "Days Since TCAR")}
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
