Imports System.Windows.Forms
Imports System.Windows.Forms.Integration
Imports EZLogger.Views ' Adjust if your view is in a different namespace

Public Class ConfigHost

    Private Sub ConfigHost_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim view As New ConfigView(Me)
        ElementHost1.Child = view

        ' Set form size and title
        Me.ClientSize = New Drawing.Size(850, 700)
        Me.Text = ""

        ' Optional UI settings
        Me.MinimizeBox = False
        Me.MaximizeBox = False
        Me.ShowIcon = False
        Me.FormBorderStyle = FormBorderStyle.FixedSingle

        ' Optional: center the window
        Me.StartPosition = FormStartPosition.CenterScreen

        ' Manually size and position the ElementHost
        ElementHost1.Width = Me.ClientSize.Width - 40
        ElementHost1.Height = Me.ClientSize.Height - 40
        ElementHost1.Location = New Drawing.Point(20, 20)
    End Sub

End Class
