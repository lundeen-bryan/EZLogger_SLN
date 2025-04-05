Imports System.Windows
Imports System.Windows.Forms

Namespace EZLogger.HostForms
    Public Class AuthorHandler

        Public Sub OnOpenAuthorFormClick()
            Dim host As New ReportAuthorHost()
            host.TopMost = True
            host.StartPosition = FormStartPosition.CenterScreen
            host.Show()
        End Sub
    End Class
End Namespace
