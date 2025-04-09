Imports System.Windows
Imports System.Windows.Forms

Namespace Handlers
    Public Class OpinionHandler

        Public Sub OnOpenOpinionFormClick()
            Dim host As New OpinionHost()
            host.TopMost = True

            FormPositionHelper.MoveFormToTopLeftOfAllScreens(host, 10, 10)
            host.Show()
        End Sub
    End Class
End Namespace

