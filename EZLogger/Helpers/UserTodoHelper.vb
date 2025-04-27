Imports System.IO
Imports System.Threading

Namespace Helpers

    ''' <summary>
    ''' Provides helper methods for managing user TODO list entries.
    ''' Initially writes to a text file; future versions may evolve into a full TODO list app.
    ''' </summary>
    Public Module UserTodoHelper

        ''' <summary>
        ''' Appends a new TODO entry to the specified file, retrying once on failure.
        ''' </summary>
        ''' <param name="filePath">Full path to the TODO text file.</param>
        ''' <param name="entryText">The text entry to append.</param>
        Public Sub AppendTodoEntry(filePath As String, entryText As String)
            If String.IsNullOrWhiteSpace(filePath) OrElse String.IsNullOrWhiteSpace(entryText) Then Exit Sub

            Dim success As Boolean = False

            For attempt As Integer = 1 To 2
                Try
                    ' Ensure the directory exists
                    Dim directoryPath As String = Path.GetDirectoryName(filePath)
                    If Not Directory.Exists(directoryPath) Then
                        Directory.CreateDirectory(directoryPath)
                    End If

                    ' Append entry with a newline
                    File.AppendAllText(filePath, entryText & Environment.NewLine)

                    success = True
                    Exit For ' Exit loop if successful
                Catch ex As Exception
                    LogHelper.LogError("UserTodoHelper.AppendTodoEntry (Attempt " & attempt & ")", ex.Message)
                    Thread.Sleep(100) ' Small delay before retry
                End Try
            Next

            ' Optionally alert if still failing after retry
            If Not success Then
                System.Windows.MessageBox.Show("Failed to save TODO entry to " & filePath, "TODO Log Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error)
            End If
        End Sub

    End Module

End Namespace
