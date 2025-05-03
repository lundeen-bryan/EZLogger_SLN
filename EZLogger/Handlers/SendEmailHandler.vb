' Namespace=EZLogger/Handlers
' Filename=SendEmailHandler.vb
' !See Label Footer for notes

Imports EZLogger.Helpers
Imports Microsoft.Office.Interop.Outlook
Imports System.Windows.Forms

Public Class SendEmailHandler

    ''' <summary>
    ''' Handles the "Select File" button click.
    ''' </summary>
    Public Function HandleSelectFileClick(ownerForm As Form) As String
        Try
            Dim dialog As New OpenFileDialog() With {
                .Filter = "PDF Files (*.pdf)|*.pdf|All Files (*.*)|*.*",
                .InitialDirectory = TempFileHelper.GetDocumentsFolder()
            }

            If dialog.ShowDialog(ownerForm) = DialogResult.OK Then
                Return dialog.FileName
            End If
        Catch ex As System.Exception
            MessageBox.Show("Error selecting file: " & ex.Message)
        End Try

        Return String.Empty
    End Function

    ''' <summary>
    ''' Handles the "Send" button click.
    ''' </summary>
    Public Sub HandleSendClick(filename As String,
                               lastname As String,
                               firstname As String,
                               reportType As String,
                               Optional hostForm As Form = Nothing)

        If String.IsNullOrWhiteSpace(reportType) Then
            MessageBox.Show("Please select a report type.")
            Exit Sub
        End If

        If String.IsNullOrWhiteSpace(filename) Then
            MessageBox.Show("Please select a file to attach.")
            Exit Sub
        End If

        Try
            Dim outlookApp = New Microsoft.Office.Interop.Outlook.Application()
            Dim mailItem As MailItem = CType(outlookApp.CreateItem(OlItemType.olMailItem), MailItem)

            Dim toAddress = ConfigHelper.GetGlobalConfigValue("Dsh_Holdovers", "email")

            'Sender
            Dim rawSender = DocumentPropertyHelper.GetPropertyValue("Processed By")
            Dim cleanSender = CleanString(rawSender)

            'Subject line
            Dim subject As String = $"DSH-N – {lastname.ToUpper()}, {ToTitleCase(firstname)} – {reportType} report"
            Dim body As String = "Please see the attached report that was processed today." & vbCrLf & vbCrLf & "-" & cleanSender

            With mailItem
                .To = toAddress
                .Subject = subject
                .BodyFormat = OlBodyFormat.olFormatPlain
                .Importance = OlImportance.olImportanceNormal
                .Attachments.Add(filename)
                .Body = body
                .UnRead = False
                .Display()
            End With

            ' Close the SendEmailView after the email is generated
            hostForm?.Close()

        Catch ex As SystemException
            MessageBox.Show("Error preparing email: " & ex.Message)
        End Try
    End Sub

    ''' <summary>
    ''' Converts a name to proper case (first letter uppercase).
    ''' </summary>
    Private Function ToTitleCase(input As String) As String
        Dim ti As Globalization.TextInfo = Globalization.CultureInfo.CurrentCulture.TextInfo
        Return ti.ToTitleCase(input.ToLower())
    End Function

    ''' <summary>
    ''' Cleans a string by removing tabs, carriage returns, extra spaces, and quotes.
    ''' </summary>
    Public Function CleanString(input As String) As String
        Dim result As String = input

        result = result.Replace(vbTab, " ")
        result = result.Replace(vbCr, " ")
        result = result.Replace(vbCrLf, " ")

        Do While result.Contains("  ")
            result = result.Replace("  ", " ")
        Loop

        result = result.Replace("""", "")
        result = result.Replace("'", "")

        Return result.Trim()
    End Function

End Class

' Footer:
''===========================================================================================
'' Filename: .......... SendEmailHandler.vb
'' Description: ....... Handles the task of creating an Outlook email for sending reports
'' Created: ........... 2025-05-02
'' Updated: ........... 2025-05-02
'' Installs to: ....... EZLogger/Handlers
'' Compatibility: ..... VSTO, WPF
'' Contact Author: .... lundeen-bryan
'' Copyright:  ........ ©2025. All rights reserved.
'' Notes: ............. _
' (1) notes_here
''===========================================================================================