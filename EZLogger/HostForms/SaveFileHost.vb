Imports System.Windows.Forms
Imports System.Windows.Forms.Integration
Imports Views ' Adjust namespace if needed

Public Class SaveFileHost
    Inherits Form

    Private Sub SaveFileHost_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim view As New SaveFileView(Me)
        ElementHost1.Dock = DockStyle.Fill
        ElementHost1.Child = view

        ' Set form size and title
        Me.ClientSize = New Drawing.Size(415, 725)

        ' Optional UI settings
        Me.MinimizeBox = False
        Me.MaximizeBox = False
        Me.ShowIcon = False
        Me.FormBorderStyle = FormBorderStyle.FixedSingle

        ' Optional: manually size and position the ElementHost
        ElementHost1.Width = Me.ClientSize.Width - 40
        ElementHost1.Height = Me.ClientSize.Height - 40
        ElementHost1.Location = New Drawing.Point(20, 20)
    End Sub

End Class