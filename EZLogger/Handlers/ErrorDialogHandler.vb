Imports System.Windows.Forms
Imports System.Windows.Forms.Integration
Imports System.Linq

Namespace Handlers

    Public Class ErrorDialogHandler

        ''' <summary>
        ''' Handles OK button click. Closes the error dialog.
        ''' </summary>
        Friend Sub HandleOkClick(hostForm As Form)
            hostForm?.Close()
        End Sub

        ''' <summary>
        ''' Handles Abort button click. Exits the application.
        ''' </summary>
        Friend Sub HandleAbortClick(hostForm As Form)
            Try
                Dim result = MessageBox.Show(
            "Are you sure you want to exit Word and stop processing?",
            "Confirm Exit",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Warning)

                If result = DialogResult.Yes Then
                    Globals.ThisAddIn.Application.Quit()
                End If

            Catch ex As Exception
                MessageBox.Show("Could not exit Word: " & ex.Message,
                        "Abort Failed",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning)
            End Try
        End Sub

        ''' <summary>
        ''' Handles Copy button click. Copies all visible error info to clipboard.
        ''' </summary>
        Friend Sub HandleCopyClick(hostForm As Form)
            If hostForm Is Nothing Then Exit Sub

            ' Access ElementHost1 by name
            Dim elementHost = TryCast(hostForm.Controls("ElementHost1"), ElementHost)
            If elementHost Is Nothing OrElse elementHost.Child Is Nothing Then Exit Sub

            Dim view = TryCast(elementHost.Child, ErrorDialogView)
            If view Is Nothing Then Exit Sub

            ' Combine error info for clipboard
            Dim clipboardText As String =
                $"Date/Time: {view.DateTimeTxt.Text}{Environment.NewLine}" &
                $"Error #: {view.ErrorNumberTxt.Text}{Environment.NewLine}" &
                $"Message: {view.ErrorDescriptionTxt.Text}{Environment.NewLine}" &
                $"Recommendation: {view.RecommendationTxt.Text}"

            Clipboard.SetText(clipboardText)
            MessageBox.Show("Error details copied to clipboard.", "Copied", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Sub

    End Class

End Namespace
