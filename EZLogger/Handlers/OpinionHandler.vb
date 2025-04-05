Imports System.Windows
Imports System.Windows.Forms

Namespace HostForms
    Public Class OpinionHandler

        Public Sub OnOpenOpinionFormClick()
            Dim host As New OpinionHost()
            host.TopMost = True
            host.StartPosition = FormStartPosition.CenterScreen
            host.Show()
        End Sub
    End Class
End Namespace

