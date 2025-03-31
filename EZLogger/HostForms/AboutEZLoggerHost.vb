Imports System.Windows.Forms
Imports System.Windows.Forms.Integration

Public Class AboutEZLoggerHost
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim myControl As New AboutEZLogger()
        ElementHost1.Dock = DockStyle.Fill
        ElementHost1.Child = myControl
    End Sub
End Class