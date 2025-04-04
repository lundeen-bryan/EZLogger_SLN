Imports System.Windows
Imports System.Windows.Forms

Namespace EZLogger.HostForms
    Public Class OpinionHandler

        Public Sub OnOpenOpinionFormClick()
            Dim host As New OpinionHost()
            host.StartPosition = FormStartPosition.CenterScreen
            host.Show()
        End Sub
    End Class
End Namespace

