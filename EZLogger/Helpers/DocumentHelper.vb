Imports System.Diagnostics
Imports System.Windows.Forms
Imports Microsoft.Office.Interop.Word
Imports Application = Microsoft.Office.Interop.Word.Application

''' <summary>
''' Provides helper methods for saving and closing the active Word document without user prompts.
''' </summary>
Public Module DocumentHelper

    ''' <summary>
    ''' Attempts to silently save the active Word document.
    ''' Will retry up to a defined number of times if save fails.
    ''' </summary>
    ''' <param name="maxRetries">Maximum number of save attempts (default is 10).</param>
    Public Sub TrySaveActiveDocument(Optional maxRetries As Integer = 10)
        Dim app As Application = Globals.ThisAddIn.Application
        Dim doc As Document = app.ActiveDocument

        Dim attempts As Integer = 0

        Do While Not doc.Saved AndAlso attempts < maxRetries
            Try
                doc.Save()
            Catch ex As Exception
                ' Optional: Log error or display debug info
            End Try
            attempts += 1
        Loop
    End Sub

    ''' <summary>
    ''' Closes the active Word document with optional prompt to user.
    ''' Defaults to silent close without prompting to save.
    ''' </summary>
    ''' <param name="showPrompt">
    ''' If True, user is asked whether to save changes. 
    ''' If False, the document is closed without any dialog.
    ''' </param>
    Public Sub CloseActiveDocument(Optional showPrompt As Boolean = False)
        Dim app As Application = Globals.ThisAddIn.Application
        Dim doc As Document = app.ActiveDocument

        Try
            ' Optional: Show a custom confirmation message before closing
            If Not showPrompt Then
                ' Attempt to save silently first
                TrySaveActiveDocument()

                ' Close the document without prompting
                doc.Close(SaveChanges:=WdSaveOptions.wdDoNotSaveChanges)
            Else
                ' Show default save prompt behavior
                doc.Close(SaveChanges:=WdSaveOptions.wdPromptToSaveChanges)
            End If
        Catch ex As Exception
            MessageBox.Show("The document could not be closed: " & ex.Message, "Close Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try
    End Sub

    ''' <summary>
    ''' Fills a Word bookmark with the specified text. If the bookmark is deleted in the process, it is recreated.
    ''' </summary>
    ''' <param name="bookmarkName">Name of the bookmark to fill.</param>
    ''' <param name="bookmarkValue">Text to insert at the bookmark location.</param>
    ''' <param name="doc">Optional: Word document. Defaults to ActiveDocument.</param>
    Public Sub FillBookmark(bookmarkName As String, bookmarkValue As String, Optional doc As Word.Document = Nothing)
        Try
            If doc Is Nothing Then doc = Globals.ThisAddIn.Application.ActiveDocument

            If doc.Bookmarks.Exists(bookmarkName) Then
                Dim rangeObject As Word.Range = doc.Bookmarks(bookmarkName).Range
                rangeObject.Text = bookmarkValue

                ' Re-create the bookmark since inserting text deletes it
                doc.Bookmarks.Add(Name:=bookmarkName, Range:=rangeObject)
            Else
                ' Log or handle missing bookmark as needed
                Debug.WriteLine($"Bookmark '{bookmarkName}' not found.")
            End If

        Catch ex As Exception
            Debug.WriteLine($"Error in FillBookmark: {ex.Message}")
        End Try
    End Sub

    ''' <summary>
    ''' Updates and unlinks all fields in the document, including headers, footers, and story ranges.
    ''' </summary>
    ''' <param name="doc">The Word document to process.</param>
    Public Sub UnlinkAllFields(doc As Word.Document)
        Try
            For Each storyRange As Word.Range In doc.StoryRanges
                Dim currentRange As Word.Range = storyRange

                ' Follow linked story ranges (headers, footers, etc.)
                Do While Not currentRange Is Nothing
                    currentRange.Fields.Update()
                    currentRange.Fields.Unlink()
                    currentRange = currentRange.NextStoryRange
                Loop
            Next
        Catch ex As Exception
            Debug.WriteLine("Error unlinking fields: " & ex.Message)
        End Try
    End Sub

End Module