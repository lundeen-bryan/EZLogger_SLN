Imports Microsoft.Office.Interop.Word
Imports System.IO

Namespace Helpers

    ''' <summary>
    ''' Provides helper methods for connecting Word documents to external Excel data sources for mail merge operations.
    ''' </summary>
    Public Module MailMergeHelper

        ''' <summary>
        ''' Opens an Excel file as the mail-merge data source, pointing at the specified worksheet.
        ''' </summary>
        ''' <param name="doc">The Word document to connect.</param>
        ''' <param name="excelPath">Full path to the Excel file.</param>
        ''' <param name="sheetName">Name of the worksheet to use (without $).</param>
        Public Sub ConnectToExcelDataSource(doc As Document, excelPath As String, sheetName As String)
            If doc Is Nothing OrElse String.IsNullOrEmpty(excelPath) OrElse Not File.Exists(excelPath) Then Exit Sub

            Dim tableRef = $"[{sheetName}$]"

            Try
                With doc.MailMerge
                    .MainDocumentType = WdMailMergeMainDocType.wdFormLetters
                    .OpenDataSource(
                        Name:=excelPath,
                        ConfirmConversions:=False,
                        ReadOnly:=True,
                        LinkToSource:=True,
                        AddToRecentFiles:=False,
                        Revert:=False,
                        Format:=WdOpenFormat.wdOpenFormatAuto,
                        Connection:="Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & excelPath & ";" &
                                   "Extended Properties=""Excel 12.0;HDR=YES;IMEX=1;""",
                        SQLStatement:="SELECT * FROM " & tableRef
                    )
                End With
            Catch ex As Exception
                ' Optional: log the exception
            End Try
        End Sub

        ''' <summary>
        ''' Unlinks all fields in the document after mail merge completes.
        ''' </summary>
        ''' <param name="doc">The Word document whose merge fields should be unlinked.</param>
        Public Sub UnlinkAllFields(doc As Document)
            If doc Is Nothing Then Exit Sub

            Try
                For Each storyRange As Range In doc.StoryRanges
                    storyRange.Fields.Update()
                    storyRange.Fields.Unlink()
                Next
                doc.MailMerge.MainDocumentType = WdMailMergeMainDocType.wdNotAMergeDocument
            Catch ex As Exception
                ' Optional: log or handle error
            End Try
        End Sub

        ''' <summary>
        ''' Executes the mail merge, filling the fields in the document.
        ''' </summary>
        Public Sub ExecuteMailMerge(doc As Document)
            If doc Is Nothing Then Exit Sub

            Try
                With doc.MailMerge
                    If .MainDocumentType <> WdMailMergeMainDocType.wdNotAMergeDocument AndAlso
               .State = WdMailMergeState.wdMainAndDataSource Then
                        .Destination = WdMailMergeDestination.wdSendToNewDocument
                        .SuppressBlankLines = True
                        .Execute(Pause:=False)
                    End If
                End With
            Catch ex As Exception
                ' Optional: Log error
            End Try
        End Sub

        ''' <summary>
        ''' Moves the MailMerge DataSource to the first record matching the specified County.
        ''' </summary>
        ''' <param name="doc">The Word document connected to the mail merge data source.</param>
        ''' <param name="countyName">The county name to match (case-insensitive).</param>
        Public Sub SelectRecordByCounty(doc As Document, countyName As String)
            If doc Is Nothing OrElse String.IsNullOrEmpty(countyName) Then Exit Sub

            Try
                Dim ds = doc.MailMerge.DataSource
                If ds Is Nothing Then Exit Sub

                ' Loop through the records to find a matching County
                'ds.FirstRecord = WdDefaultListBehavior.wdDefaultFirstRecord
                ds.ActiveRecord = 1 ' Start at first record

                Dim found As Boolean = False

                For i = 1 To ds.RecordCount
                    ds.ActiveRecord = i
                    Dim countyFieldValue = ds.DataFields("County").Value

                    If String.Equals(countyFieldValue.Trim(), countyName.Trim(), StringComparison.OrdinalIgnoreCase) Then
                        found = True
                        Exit For
                    End If
                Next

                ' If no match found, stay at first record
                If Not found Then
                    ds.ActiveRecord = 1
                End If

                ' Limit the merge to this one record
                ds.FirstRecord = ds.ActiveRecord
                ds.LastRecord = ds.ActiveRecord

            Catch ex As Exception
                ' Optional: log error
            End Try
        End Sub

    End Module

End Namespace
' TODO: orphaned