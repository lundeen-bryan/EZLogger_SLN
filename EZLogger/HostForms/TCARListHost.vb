Imports System.Windows.Forms

Public Class TCARListHost
    Inherits Form

    Private Sub TCARListHost_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim view As New TCARListView(Me)
        ElementHost1.Child = view

        ' Updated form size
        Me.ClientSize = New Drawing.Size(900, 500)
        Me.Text = "View TCAR Referrals"
    End Sub

End Class
