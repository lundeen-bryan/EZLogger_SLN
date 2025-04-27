Imports System.IO

Namespace Helpers

    Public Module LogHelper

        ''' <summary>
        ''' Writes a timestamped debug message to a local development folder.
        ''' </summary>
        ''' <param name="message">The message to write.</param>
        Public Sub LogDebugInfo(message As String)
            Try
                ' Hardcoded path for development logging
                ' TODO change this to a central error logging location saved in global_config.json
                Dim logDir As String = "C:\Users\lunde\repos\cs\ezlogger\EZLogger_SLN\temp\Error_Logs"
                Dim logPath As String = Path.Combine(logDir, "error_log.txt")

                ' Ensure directory exists
                If Not Directory.Exists(logDir) Then
                    Directory.CreateDirectory(logDir)
                End If

                ' Compose timestamped message
                Dim fullMessage As String = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}{Environment.NewLine}"

                ' Append to log file
                File.AppendAllText(logPath, fullMessage)

            Catch ex As Exception
                ' Optional: silently ignore or raise alert
            End Try
        End Sub

        ''' <summary>
        ''' Writes a timestamped error message to the error log.
        ''' </summary>
        ''' <param name="source">The method or class where the error occurred.</param>
        ''' <param name="errorMessage">The error message to write.</param>
        Public Sub LogError(source As String, errorMessage As String)
            Try
                Dim logDir As String = "C:\Users\lunde\repos\cs\ezlogger\EZLogger_SLN\temp\Error_Logs"
                Dim logPath As String = Path.Combine(logDir, "error_log.txt")

                If Not Directory.Exists(logDir) Then
                    Directory.CreateDirectory(logDir)
                End If

                Dim fullMessage As String = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [ERROR] [{source}] {errorMessage}{Environment.NewLine}"

                File.AppendAllText(logPath, fullMessage)

            Catch
                ' Optional: silently fail
            End Try
        End Sub

    End Module

End Namespace
