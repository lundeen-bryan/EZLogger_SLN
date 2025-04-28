Imports System.Windows.Forms
Imports System.Windows.Forms.Integration
Imports EZLogger.Views

Public Class TaskListHost
    Inherits Form

    Public Sub New()
        Me.Text = "EZLogger Task List"
        Me.Width = 540 : Me.Height = 600
        Dim host As New ElementHost With {
            .Dock = DockStyle.Fill,
            .Child = New TaskListView()
        }
        Me.Controls.Add(host)
    End Sub
End Class
