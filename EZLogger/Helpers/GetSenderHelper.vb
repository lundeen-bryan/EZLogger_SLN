Imports EZLogger.Helpers
Imports Microsoft.Office.Interop.Word

''' <summary>
''' Provides helper methods to identify and log the sender of a document.
''' </summary>
Public Module SenderHelper

    ''' <summary>
    ''' Gets the name of the user from Word's Application.UserName, cleaned up for logging.
    ''' </summary>
    ''' <returns>Sender name in a simplified format.</returns>
    Public Function GetSenderName() As String
        Dim rawUsername As String = Globals.ThisAddIn.Application.UserName

        If Not String.IsNullOrWhiteSpace(rawUsername) Then
            ' If format is "Last, First", switch to "First Last"
            If rawUsername.Contains(",") Then
                Dim commaParts() As String = rawUsername.Split(","c)
                If commaParts.Length = 2 Then
                    Dim first As String = commaParts(1).Trim()
                    Return first ' ✅ Just the first name
                End If
            End If

            ' If no comma, try to get the first word as the first name
            Dim spaceParts() As String = rawUsername.Trim().Split(" "c)
            If spaceParts.Length >= 1 Then
                Return spaceParts(0) ' ✅ Return just the first word
            End If
        End If

        ' Fallback to local config
        Dim fallbackName As String = ConfigPathHelper.GetLocalUserFirstName()
        If Not String.IsNullOrWhiteSpace(fallbackName) Then
            Return fallbackName
        End If

        Return "(Unknown User)"
    End Function

    ''' <summary>
    ''' Writes the cleaned sender name into the Word document's custom property "Processed By".
    ''' </summary>
    Public Sub WriteProcessedBy()
        Dim senderName As String = GetSenderName()
        DocumentPropertyManager.WriteCustomProperty("Processed By", senderName)
    End Sub

End Module