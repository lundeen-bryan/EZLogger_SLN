Imports System.Windows
Imports System.Windows.Forms

Namespace Handlers
    Public Class MoveCopyHandler

        Public Sub OnMoveCopyClick()
            Dim host As New MoveCopyHost()
            host.TopMost = True
            host.StartPosition = FormStartPosition.CenterScreen
            host.Show()
        End Sub
    End Class
End Namespace

