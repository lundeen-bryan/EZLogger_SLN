Imports EZLogger.Helpers
Imports EZLogger.HostForms
Imports System.Windows.Forms

Namespace Helpers

    ''' <summary>
    ''' Centralized error handler for logging and displaying error dialogs.
    ''' </summary>
    Public Module ErrorHelper

        ''' <summary>
        ''' Handles an application error by logging it and showing an error dialog.
        ''' </summary>
        ''' <param name="source">The module or method where the error occurred.</param>
        ''' <param name="errorNumber">A string representation of the error number (e.g. HResult).</param>
        ''' <param name="errorMessage">The main error message from the exception.</param>
        ''' <param name="recommendation">A user-facing suggestion or fix.</param>
        ''' <param name="hostForm">Optional: form to anchor the error dialog near.</param>
        Public Sub HandleError(source As String,
                               errorNumber As String,
                               errorMessage As String,
                               recommendation As String,
                               Optional hostForm As Form = Nothing)

            ' 1. Format the full message to log
            Dim user As String = Environment.UserName
            Dim logMessage As String = $"User: {user}, Err#: {errorNumber}, Source: {source}, Message: {errorMessage}"

            ' 2. Log it
            LogHelper.LogError(source, logMessage)

            ' 3. Show the error dialog with details
            Dim dialogHost As New ErrorDialogHost(errorMessage, errorNumber, recommendation, source, hostForm)
            dialogHost.Show()
        End Sub

    End Module

End Namespace
