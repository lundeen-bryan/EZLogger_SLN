Imports EZLogger.Helpers
Imports Microsoft.Office.Interop.Excel
Imports System.Diagnostics
Imports System.Runtime.InteropServices
Imports System.Windows

Public Module ExcelHelper

    ''' <summary>
    ''' Exports a DataTable to a new Excel workbook and saves it as SummaryReport.xlsx in the user's Documents folder.
    ''' </summary>
    ''' <param name="dt">The DataTable containing EZL records.</param>
    Public Sub ExportDataTableToSummaryExcel(dt As System.Data.DataTable)
        If dt Is Nothing OrElse dt.Rows.Count = 0 Then
            MsgBoxHelper.Show("No data to export.")
            Exit Sub
        End If

        Dim xlApp As New Microsoft.Office.Interop.Excel.Application()
        Dim xlBook As Workbook = Nothing
        Dim xlSheet As Worksheet = Nothing

        Try
            xlApp.Visible = False
            xlBook = xlApp.Workbooks.Add()
            xlSheet = CType(xlBook.Sheets(1), Worksheet)

            ' Write headers
            For col As Integer = 0 To dt.Columns.Count - 1
                xlSheet.Cells(1, col + 1).Value = dt.Columns(col).ColumnName
            Next

            ' Write data
            For row As Integer = 0 To Math.Min(50, dt.Rows.Count - 1) ' TEMPORARY CAP FOR DEBUGGING
                For col As Integer = 0 To dt.Columns.Count - 1
                    Dim value As Object = dt.Rows(row)(col)
                    xlSheet.Cells(row + 2, col + 1).Value = If(value IsNot Nothing, value.ToString(), "")
                Next
            Next

            ' Autosize columns
            xlSheet.Columns.AutoFit()

            ' Save to user's Documents folder using helper
            Dim savePath As String = IO.Path.Combine(TempFileHelper.GetDocumentsFolder(), "SummaryReport.xlsx")
            xlBook.SaveAs(savePath)

            MsgBoxHelper.Show("Summary report saved to: " & savePath)

        Catch ex As Exception
            Dim errNum As String = ex.HResult.ToString()
            Dim errMsg As String = CStr(ex.Message)
            Dim recommendation As String = "Please confirm the patient number from the report to make sure it matches a patient in ForensicInfo."

            ErrorHelper.HandleError("PrcHandler.SaveProcessedReport", errNum, errMsg, recommendation)

        Finally
            ' Cleanup
            If xlBook IsNot Nothing Then xlBook.Close(False)
            If xlApp IsNot Nothing Then xlApp.Quit()

            If xlSheet IsNot Nothing Then Marshal.ReleaseComObject(xlSheet)
            If xlBook IsNot Nothing Then Marshal.ReleaseComObject(xlBook)
            If xlApp IsNot Nothing Then Marshal.ReleaseComObject(xlApp)
        End Try
    End Sub

    ''' <summary>
    ''' Searches for the specified patient number in the HLV Excel file and returns the matching provider name.
    ''' </summary>
    ''' <param name="patientNumber">The patient number to search for.</param>
    ''' <returns>The provider name if found; otherwise, returns Nothing.</returns>
    Public Function GetProviderFromHLV(patientNumber As String) As String
        Dim xlApp As New Microsoft.Office.Interop.Excel.Application()
        xlApp.Visible = False
        Dim xlBook As Workbook = Nothing
        Dim xlSheet As Worksheet = Nothing

        Try
            Dim filePath As String = ConfigHelper.GetLocalConfigValue("sp_filepath", "hlv_data")
            If String.IsNullOrWhiteSpace(filePath) OrElse Not IO.File.Exists(filePath) Then
                Return Nothing
            End If

            xlApp = New Microsoft.Office.Interop.Excel.Application()
            xlBook = xlApp.Workbooks.Open(filePath, ReadOnly:=True)
            xlSheet = CType(xlBook.Sheets("HLV"), Worksheet)

            Dim usedRange As Range = xlSheet.UsedRange
            Dim headerRow As Integer = 1

            Dim patientCol As Integer = -1
            Dim providerCol As Integer = -1

            ' Find the column indexes based on header names
            For col = 1 To usedRange.Columns.Count
                Dim header As String = TryCast((usedRange.Cells(headerRow, col)).Value2, String)
                If String.Equals(header, "patient_number", StringComparison.OrdinalIgnoreCase) Then
                    patientCol = col
                ElseIf String.Equals(header, "provider", StringComparison.OrdinalIgnoreCase) Then
                    providerCol = col
                End If
            Next

            If patientCol = -1 OrElse providerCol = -1 Then
                Return Nothing ' required columns not found
            End If

            ' Loop through rows to find the match
            For row = headerRow + 1 To usedRange.Rows.Count
                Dim value As Object = usedRange.Cells(row, patientCol).Value2
                If value IsNot Nothing AndAlso value.ToString().Trim() = patientNumber.Trim() Then
                    Return TryCast(usedRange.Cells(row, providerCol).Value2, String)
                End If
            Next

            Return Nothing ' no match found
        Catch ex As Exception
            ' Optional: Log error
            Return Nothing
        Finally
            ' Cleanup Excel Interop properly
            If xlBook IsNot Nothing Then xlBook.Close(False)
            If xlApp IsNot Nothing Then xlApp.Quit()

            If xlSheet IsNot Nothing Then Marshal.ReleaseComObject(xlSheet)
            If xlBook IsNot Nothing Then Marshal.ReleaseComObject(xlBook)
            If xlApp IsNot Nothing Then Marshal.ReleaseComObject(xlApp)
        End Try
    End Function

End Module
