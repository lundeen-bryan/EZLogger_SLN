Imports Microsoft.Office.Core
Imports Microsoft.Office.Interop.Word

Namespace Helpers

    ''' <summary>
    ''' Provides helper methods to fill bookmarks in a Word document.
    ''' </summary>
    Public Module BookmarkHelper

        ''' <summary>
        ''' Fills bookmarks in the document based on the custom document properties.
        ''' Property names will have spaces replaced by underscores when matching bookmarks.
        ''' </summary>
        ''' <param name="doc">The Word document where bookmarks will be filled.</param>
        Public Sub FillBookmarksFromDocumentProperties(doc As Document)
            If doc Is Nothing Then Exit Sub

            Try
                Dim sourceProperties As DocumentProperties = CType(doc.CustomDocumentProperties, DocumentProperties)

                For Each prop As DocumentProperty In sourceProperties
                    Dim bookmarkName As String = prop.Name.Replace(" ", "_")

                    If doc.Bookmarks.Exists(bookmarkName) Then
                        InsertTextIntoBookmark(doc, bookmarkName, prop.Value.ToString())
                    End If
                Next
            Catch ex As Exception
                ' Optional: Add logging or error handling if desired
            End Try
        End Sub

        ''' <summary>
        ''' Fills a single bookmark with a given value, creating a new bookmark if necessary.
        ''' </summary>
        ''' <param name="doc">The Word document containing the bookmark.</param>
        ''' <param name="bookmarkName">The name of the bookmark.</param>
        ''' <param name="value">The text value to insert into the bookmark.</param>
        Public Sub InsertTextIntoBookmark(doc As Document, bookmarkName As String, value As String)
            If doc Is Nothing OrElse String.IsNullOrEmpty(bookmarkName) Then Exit Sub

            Try
                If doc.Bookmarks.Exists(bookmarkName) Then
                    Dim bookmarkRange As Range = doc.Bookmarks(bookmarkName).Range
                    bookmarkRange.Text = value

                    ' Re-add the bookmark to preserve it after insertion
                    doc.Bookmarks.Add(bookmarkName, bookmarkRange)
                End If
            Catch ex As Exception
                ' Optional: log or handle errors
            End Try
        End Sub

    End Module

End Namespace
