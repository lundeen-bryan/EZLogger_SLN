Imports System.Windows.Forms

Public Class PatientInfoHost
    Inherits Form

    Private Sub PatientInfoHost_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim view As New EZLogger.Views.PatientInfoView()
        ElementHost1.Dock = DockStyle.Fill
        ElementHost1.Child = view
    End Sub
End Class
