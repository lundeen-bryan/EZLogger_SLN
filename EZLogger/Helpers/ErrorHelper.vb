Imports System.Windows.Forms
Imports EZLogger.HostForms

Namespace Helpers

    ''' <summary>
    ''' Centralized error handler for displaying and logging errors.
    ''' </summary>
    Public Module ErrorHelper

        ''' <summary>
        ''' Handles an application error by showing a dialog and logging the details.
        ''' </summary>
        ''' <param name="source">The module, method, or button name where the error occurred.</param>
        ''' <param name="errorNumber">A string representing the error number or code.</param>
        ''' <param name="errorMessage">The main error message to display to the user.</param>
        ''' <param name="recommendation">A suggested action or message to help the user.</param>
        ''' <param name="hostForm">Optional: the form to position the error dialog near.</param>
        Public Sub HandleError(source As String,
                               errorNumber As String,
                               errorMessage As String,
                               recommendation As String,
                               Optional hostForm As Form = Nothing)

            ' TODO: Send error details to the ErrorDialogView via ErrorDialogHost
            ' Example: Dim dialogHost As New ErrorDialogHost(...)
            ' Then set the relevant fields in ErrorDialogView (e.g. DateTimeTxt, ErrorNumberTxt, etc.)

            ' TODO: Log the error message to error_log.txt
            ' Example: LogHelper.LogError(source, composedMessage)

            ' --- Temporary MsgBox placeholder for dev testing ---
            MessageBox.Show($"[Error] {source}{vbCrLf}{vbCrLf}{errorMessage}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Sub

    End Module

End Namespace
