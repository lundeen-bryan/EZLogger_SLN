Imports System.Windows
Imports System.Windows.Forms

Namespace HostForms
    Public Class AuthorHandler

        Public Sub OnOpenAuthorFormClick()
            Dim host As New ReportAuthorHost()
            host.TopMost = True
            host.StartPosition = FormStartPosition.CenterScreen
            host.Show()
        End Sub
        Public Sub HandleCloseClick(form As Form)
            If form IsNot Nothing Then form.Close()
        End Sub
        Public Sub HandleAddAuthorClick()
            MsgBox("You clicked Add New Author")
        End Sub

        Public Sub HandleFirstPageClick()
            MsgBox("You clicked First Page")
        End Sub

        Public Sub HandleLastPageClick()
            MsgBox("You clicked Last Page")
        End Sub

        Public Sub HandleDoneSelectingClick()
            MsgBox("You clicked Done Selecting")
        End Sub

    End Class
End Namespace
