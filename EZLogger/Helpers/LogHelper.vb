Imports System.IO

Namespace Helpers

    Public Module LogHelper

        ''' <summary>
        ''' Appends a line to the _LogTheseFiles.txt file containing full report details.
        ''' </summary>
        ''' <param name="patientNumber">Patient number associated with the report.</param>
        ''' <param name="reportType">Type of the report.</param>
        ''' <param name="sender">Name of the person processing the report.</param>
        ''' <param name="fileName">Name of the report file (not full path).</param>
        Public Sub AppendToLogTheseFiles(patientNumber As String, reportType As String, sender As String, fileName As String)
            Try
                ' Get the full path to _LogTheseFiles.txt
                Dim logFilePath As String = ConfigHelper.GetLogTheseFilesPath()

                ' Build the line to append
                Dim logLine As String = $"{DateTime.Now:MM/dd/yyyy HH:mm} | {patientNumber} | {reportType} | {sender} | {fileName}"

                ' Append the line to the file
                File.AppendAllText(logFilePath, logLine & Environment.NewLine)

            Catch ex As Exception
                ' Optional: Log the error or show a message box
                MsgBox($"Failed to append to _LogTheseFiles.txt: {ex.Message}", MsgBoxStyle.Critical)
            End Try
        End Sub

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
        ''' Writes a timestamped error message to the error log defined in global_config.json.
        ''' Exits silently if config value is missing.
        ''' </summary>
        ''' <param name="source">The method or class where the error occurred.</param>
        ''' <param name="errorMessage">The error message to write.</param>
        Public Sub LogError(source As String, errorMessage As String)
            Dim logDir As String = ConfigHelper.GetGlobalConfigValue("database", "error_folder")
            If String.IsNullOrWhiteSpace(logDir) Then Exit Sub

            Dim logPath As String = Path.Combine(logDir, "error_log.txt")
            Dim fullMessage As String = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [ERROR] [{source}] {errorMessage}{Environment.NewLine}"

            File.AppendAllText(logPath, fullMessage)
        End Sub

    End Module

End Namespace
