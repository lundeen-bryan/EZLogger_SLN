Imports Microsoft.Office.Interop.Word

Namespace Helpers

    ''' <summary>
    ''' Provides helper methods to identify and log the sender of a document.
    ''' </summary>
    Public Module SenderHelper

        ''' <summary>
        ''' Gets the first name of the user from Application.UserName, cleaned for use in document properties.
        ''' </summary>
        ''' <returns>First name of the sender.</returns>
        Public Function GetSenderName() As String
            Dim rawUsername As String = Globals.ThisAddIn.Application.UserName

            If String.IsNullOrWhiteSpace(rawUsername) Then
                Return "(Unknown User)"
            End If

            ' If format is "Last, First", extract first
            If rawUsername.Contains(",") Then
                Dim parts = rawUsername.Split(","c)
                If parts.Length > 1 Then
                    rawUsername = parts(1).Trim()
                End If
            End If

            ' Strip domain if it's an email format (e.g., "john.doe@agency.gov")
            If rawUsername.Contains("@") Then
                rawUsername = rawUsername.Split("@"c)(0).Trim()
            End If

            ' Optionally clean punctuation or unwanted characters
            Return CleanSenderName(rawUsername)
        End Function

        ''' <summary>
        ''' Optional basic cleanup to match VBA behavior.
        ''' </summary>
        Private Function CleanSenderName(name As String) As String
            ' Remove periods or extra whitespace if needed
            Return name.Replace(".", " ").Trim()
        End Function

        ''' <summary>
        ''' Writes the cleaned sender name into the Word document's custom property "Processed By".
        ''' </summary>
        ''' <param name="doc">The active Word document.</param>
        Public Sub WriteProcessedBy(doc As Document)
            Dim senderName As String = GetSenderName()
            DocumentPropertyHelper.WriteCustomProperty(doc, "Processed By", senderName)
        End Sub

    End Module

End Namespace
