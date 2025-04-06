Imports System.Windows.Forms

Public Class OpinionHost
    Inherits Form

    Private Sub OpinionHost_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' ✅ Pass Me (the host form) to the view
        Dim view As New OpinionView(Me)

        ElementHost1.Dock = DockStyle.Fill
        ElementHost1.Child = view
    End Sub
End Class
