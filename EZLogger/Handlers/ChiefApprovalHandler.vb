Imports System.Windows
Imports System.Windows.Forms
Imports stdole

Namespace Handlers
    Public Class ChiefApprovalHandler

        Public Sub OnOpenChiefHostClick()
            Dim host As New ChiefApprovalHost()
            host.TopMost = True
            host.StartPosition = FormStartPosition.CenterScreen
            host.Show()
        End Sub

        Public Sub HandleApprovalClick()
            MsgBox("You clicked Approved By")
        End Sub

        Public Sub HandleSignatureClick()
            MsgBox("You clicked Insert Signature")
        End Sub

        Public Sub HandleCloseClick(form As Form)
            If form IsNot Nothing Then form.Close()
        End Sub

    End Class
End Namespace

