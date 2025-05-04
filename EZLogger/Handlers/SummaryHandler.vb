Imports EZLogger.Helpers
Imports System.Data
Imports System.Data.SqlClient

Namespace Handlers
    Public Class SummaryHandler ' Example: ConfigViewHandler

        ''' <summary>
        ''' Retrieves all records from the EZL table in SQL Server.
        ''' </summary>
        ''' <returns>A DataTable containing all EZL rows.</returns>
        Public Function GetAllPrcRecords() As DataTable
            Dim dt As New System.Data.DataTable()
            Dim connStr As String = ConfigHelper.GetGlobalConfigValue("database", "connectionString")

            If String.IsNullOrWhiteSpace(connStr) Then
                MsgBoxHelper.Show("SQL Server connection string not found in global_config.json.")
                Return dt
            End If

            Try
                Using conn As New SqlConnection(connStr)
                    conn.Open()

                    Dim sql As String = "
                        SELECT * FROM EZL_PRC
                        WHERE Created >= DATEADD(DAY, -7, GETDATE())
                    "

                    Using cmd As New SqlCommand(sql, conn)
                        Using reader As SqlDataReader = cmd.ExecuteReader()
                            dt.Load(reader)
                        End Using
                    End Using
                End Using

            Catch ex As Exception
                Dim errNum As String = ex.HResult.ToString()
                Dim errMsg As String = CStr(ex.Message)
                Dim recommendation As String = "Please confirm the patient number from the report to make sure it matches a patient in ForensicInfo."

                ErrorHelper.HandleError("PrcHandler.SaveProcessedReport", errNum, errMsg, recommendation)
            End Try

            Return dt
        End Function

        ''' <summary>
        ''' Handles the Summary button click by exporting all EZL records to SummaryReport.xlsx.
        ''' </summary>
        Public Async Sub HandleSummaryClick()
            Dim busyForm As New BusyHost()
            busyForm.Show()

            Await System.Threading.Tasks.Task.Delay(100) ' Give BusyControl time to render

            Dim records As System.Data.DataTable = Nothing

            Try
                ' Pull data on background thread — safe
                records = Await System.Threading.Tasks.Task.Run(Function()
                                                                    Return GetAllPrcRecords()
                                                                End Function)

                If records Is Nothing OrElse records.Rows.Count = 0 Then
                    MsgBoxHelper.Show("No records found in the EZL_PRC table.")
                    Exit Sub
                End If

                ' ⚠️ RUN Excel export on UI thread to avoid InvalidOperationException
                ExcelHelper.ExportDataTableToSummaryExcel(records)

            Catch ex As Exception
                Dim errNum As String = ex.HResult.ToString()
                Dim errMsg As String = ex.Message
                Dim recommendation As String = "Please ensure the EZL_PRC table is accessible."

                ErrorHelper.HandleError("SummaryHandler.HandleSummaryClick", errNum, errMsg, recommendation)

            Finally
                busyForm.Close()
            End Try
        End Sub

        Public Sub RunSummaryWithBusy()
            Dim busyForm As New BusyHost()
            busyForm.Show()

            ' Use Dispatcher to re-enter STA context
            Dim syncContext = System.Threading.SynchronizationContext.Current

            System.Threading.Tasks.Task.Run(Sub()
                                                Try
                                                    ' Pull data in background
                                                    Dim records = GetAllPrcRecords()

                                                    If records Is Nothing OrElse records.Rows.Count = 0 Then
                                                        syncContext.Post(Sub()
                                                                             busyForm.Close()
                                                                             MsgBoxHelper.Show("No records found in the EZL_PRC table.")
                                                                         End Sub, Nothing)
                                                        Return
                                                    End If

                                                    ' Back to STA thread for Excel export
                                                    syncContext.Post(Sub()
                                                                         ExcelHelper.ExportDataTableToSummaryExcel(records)
                                                                         busyForm.Close()
                                                                     End Sub, Nothing)

                                                Catch ex As Exception
                                                    syncContext.Post(Sub()
                                                                         busyForm.Close()
                                                                         ErrorHelper.HandleError("SummaryHandler.RunSummaryWithBusy", ex.HResult.ToString(), ex.Message, "Excel export failed.")
                                                                     End Sub, Nothing)
                                                End Try
                                            End Sub)
        End Sub


    End Class
End Namespace