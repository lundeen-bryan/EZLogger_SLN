Imports System.Windows
Imports System.Windows.Forms

Namespace Handlers

    ''' <summary>
    ''' Provides logic related to generating or managing fax cover sheets in EZLogger.
    ''' </summary>
    Public Class FaxCoverHandler

        ''' <summary>
        ''' Displays a placeholder message box for fax cover logic.
        ''' </summary>
        Public Sub ShowFaxCoverMessage()
            Dim host As New FaxCoverHost()
            host.Show()
        End Sub

        Public Sub HandleCloseClick(hostForm As Form)
            hostForm?.Close()
        End Sub

    End Class

End Namespace