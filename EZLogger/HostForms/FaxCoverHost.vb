Imports System.Drawing
Imports System.Windows.Forms

Public Class FaxCoverHost

    Private Sub FaxCoverHost_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Set window size
        Me.ClientSize = New Size(470, 640)
        Me.StartPosition = FormStartPosition.CenterScreen
        Me.MinimumSize = New Size(470, 640)

        ' Size and position ElementHost
        ElementHost1.Width = Me.ClientSize.Width - 40
        ElementHost1.Height = Me.ClientSize.Height - 40
        ElementHost1.Location = New Point(20, 20)

        ' Load WPF user control into ElementHost
        ElementHost1.Child = New FaxCoverView()
    End Sub

End Class
