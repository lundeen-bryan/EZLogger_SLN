Imports System.Windows.Forms

Public Class BusyHost

    Private Sub BusyHost_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim view As New BusyControl()
        ElementHost1.Child = view

        ' Set form size and title
        Me.ClientSize = New Drawing.Size(210, 60)
        Me.Text = ""

        ' Optional UI settings
        Me.MinimizeBox = False
        Me.MaximizeBox = False
        Me.ShowIcon = False
        Me.FormBorderStyle = FormBorderStyle.None

        ' Optional: center the window
        Me.StartPosition = FormStartPosition.CenterScreen

        ' Optional: manually size and position the ElementHost
        ElementHost1.Width = Me.ClientSize.Width - 40
        ElementHost1.Height = Me.ClientSize.Height - 40
        ElementHost1.Location = New Drawing.Point(20, 20)
    End Sub

End Class