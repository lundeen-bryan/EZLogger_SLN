Imports Microsoft.Office.Interop.Word
Imports System.IO

Namespace Helpers

    ''' <summary>
    ''' Provides helper methods for connecting Word documents to external Excel datasources for mail merge operations.
    ''' </summary>
    Public Module MailMergeHelper

        Public Sub ConnectToExcelDataSource(doc As Document, excelPath As String, Optional filterCounty As String = Nothing)
            If doc Is Nothing OrElse String.IsNullOrEmpty(excelPath) OrElse Not File.Exists(excelPath) Then Exit Sub

            Try
                With doc.MailMerge
                    .MainDocumentType = WdMailMergeMainDocType.wdFormLetters
                    .OpenDataSource(Name:=excelPath,
                                    ConfirmConversions:=False,
                                    ReadOnly:=True,
                                    LinkToSource:=True,
                                    AddToRecentFiles:=False,
                                    Revert:=False,
                                    Format:=WdOpenFormat.wdOpenFormatAuto)
                End With
            Catch ex As Exception
                ' Optional: log the exception
            End Try
        End Sub

        ''' <summary>
        ''' Fills bookmarks using fields from the mail merge data source.
        ''' Expandable: simply add new entries to the dictionary if needed.
        ''' </summary>
        ''' <param name="doc">The Word document whose bookmarks are to be filled.</param>
        Public Sub FillAddressBookmarksFromDataSource(doc As Document)
            If doc Is Nothing Then Exit Sub

            Try
                Dim datasource = doc.MailMerge.DataSource

                If datasource IsNot Nothing AndAlso datasource.DataFields.Count > 0 Then
                    ' Define mappings: {BookmarkName} -> {DataFieldName}
                    Dim fieldMappings As New Dictionary(Of String, String) From {
                        {"Phone", "Phone"},
                        {"Fax", "Fax"},
                        {"Street", "Street"},
                        {"City", "City"},
                        {"Zip", "Zip"},
                        {"CONREP", "conrep_name"},
                        {"Sheriff_Name", "Name"},
                        {"CourtAddress", "CourtAddress"} ' Example of adding a new bookmark easily
                    }

                    For Each kvp In fieldMappings
                        Dim bookmarkName = kvp.Key
                        Dim dataFieldName = kvp.Value

                        ' Safely check if the DataField exists before trying to fill it
                        If DataFieldExists(datasource, dataFieldName) Then
                            BookmarkHelper.InsertTextIntoBookmark(doc, bookmarkName, datasource.DataFields(dataFieldName).Value)
                        End If
                    Next
                End If
            Catch ex As Exception
                ' Optional: log or handle error
            End Try
        End Sub

        ''' <summary>
        ''' Unlinks all fields in the document after mail merge.
        ''' </summary>
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
        ''' Checks if a DataField exists in the MailMerge DataSource.
        ''' </summary>
        Private Function DataFieldExists(datasource As MailMergeDataSource, fieldName As String) As Boolean
            Try
                For i As Integer = 1 To datasource.DataFields.Count
                    If String.Equals(datasource.DataFields(i).Name, fieldName, StringComparison.OrdinalIgnoreCase) Then
                        Return True
                    End If
                Next
            Catch
                ' Ignore errors
            End Try
            Return False
        End Function

    End Module

End Namespace
