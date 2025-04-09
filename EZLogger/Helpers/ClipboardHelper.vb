Imports System.Windows

''' <summary>
''' Provides helper methods for interacting with the system clipboard.
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
            Return True
        Catch ex As Exception
            ' Optionally log the error or show a custom message box
            ' For now, just return False to indicate failure
            Return False
        End Try
    End Function

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
            ' Handle exception as needed
        End Try

        Return String.Empty
    End Function

End Module