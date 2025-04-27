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

    End Module

End Namespace
