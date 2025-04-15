Imports System.Windows
Imports System.Windows.Forms

Namespace Handlers

    ''' <summary>
    ''' Provides logic for save-related operations in EZLogger.
    ''' </summary>
    Public Class SaveFileHandler

        ''' <summary>
        ''' Displays a message box as a placeholder for save file logic.
        ''' </summary>
        Public Sub ShowSaveMessage()
            Dim host As New SaveFileHost()
            host.TopMost = True
            FormPositionHelper.MoveFormToTopLeftOfAllScreens(host, 10, 10)
            host.Show()
        End Sub

    End Class

End Namespace
