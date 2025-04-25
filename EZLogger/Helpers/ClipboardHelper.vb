Imports System.Windows
Imports Microsoft.Office.Interop.Word
Imports Application = Microsoft.Office.Interop.Word.Application

Namespace Helpers

    ''' <summary>
    ''' Provides helper methods for interacting with the system clipboard and Word status bar.
    ''' </summary>
    Public Module ClipboardHelper

        ''' <summary>
        ''' Copies the specified text to the clipboard.
        ''' Automatically handles empty/null strings and clipboard access exceptions.
        ''' </summary>
        ''' <param name="text">The text to copy to the clipboard.</param>
        ''' <returns>True if the operation succeeded, otherwise False.</returns>
        Public Function CopyText(text As String) As Boolean
            If String.IsNullOrWhiteSpace(text) Then
                Return False ' Nothing to copy
            End If

            Try
                Clipboard.SetText(text)
                ShowStatusBarMessage($"{text} was copied to the clipboard")
                Return True

            Catch ex As Exception
                ' Optional: Log the error or show a custom message box
                ' Example: CustomMsgBox.Show("Clipboard error: " & ex.Message)
                ShowStatusBarMessage("Could not copy text to the clipboard.")
                Return False
            End Try
        End Function

        ''' <summary>
        ''' Displays a message in the Word status bar.
        ''' </summary>
        ''' <param name="message">The message to show in the status bar.</param>
        Public Sub ShowStatusBarMessage(message As String)
            Try
                Dim wordApp As Application = Globals.ThisAddIn.Application
                wordApp.StatusBar = message
            Catch ex As Exception
                ' Optional: Handle or log error if Word is not available
            End Try
        End Sub

        ''' <summary>
        ''' Retrieves plain text currently stored on the clipboard.
        ''' </summary>
        ''' <returns>The clipboard text if available, otherwise an empty string.</returns>
        Public Function GetText() As String
            Try
                If Clipboard.ContainsText() Then
                    Return Clipboard.GetText()
                End If
            Catch ex As Exception
                ' Optional: Handle exception or log
            End Try

            Return String.Empty
        End Function

    End Module

End Namespace
