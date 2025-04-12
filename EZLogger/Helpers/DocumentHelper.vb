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

        ' Check if there is an active document
        If app.Documents.Count = 0 Then
            MessageBox.Show("There is no active document to close.", "No Document Open", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        Dim doc As Document = app.ActiveDocument

        Try
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

End Module