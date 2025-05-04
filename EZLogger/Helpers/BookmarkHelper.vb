Imports Microsoft.Office.Core
Imports Microsoft.Office.Interop.Word

Namespace Helpers

    ''' <summary>
    ''' Provides helper methods to fill bookmarks in a Word document.
    ''' </summary>
    Public Module BookmarkHelper

        ''' <summary>
        ''' Copies custom properties from one document into bookmarks of another.
        ''' </summary>
        Public Sub FillBookmarksFromDocumentProperties(sourceDoc As Document, targetDoc As Document)

            If sourceDoc Is Nothing OrElse targetDoc Is Nothing Then Return

            Try
                Dim props = CType(sourceDoc.CustomDocumentProperties, DocumentProperties)

                For Each prop As DocumentProperty In props
                    Dim name = prop.Name.Replace(" ", "_")

                    If targetDoc.Bookmarks.Exists(name) Then
                        Dim bmRange = targetDoc.Bookmarks(name).Range

                        ' Insert the value
                        bmRange.Text = prop.Value.ToString()

                        ' Recreate the bookmark at the updated range
                        targetDoc.Bookmarks.Add(name, bmRange)

                        ' OPTIONAL: update any fields referencing this bookmark
                        bmRange.Fields.Update()

                        ' Unlink the fields after updating
                        If bmRange.Fields.Count > 0 Then
                            For Each fld As Field In bmRange.Fields
                                fld.Unlink()
                            Next
                        End If
                    End If
                Next

            Catch ex As Exception
                ' Optional: log error or show message
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
