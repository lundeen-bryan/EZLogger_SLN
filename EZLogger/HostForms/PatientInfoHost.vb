Imports System.Windows.Forms
Imports System.Windows.Forms.Integration
Imports EZLogger.EZLogger.Views

Public Class PatientInfoHost
    Inherits Form

    Private Sub PatientInfoHost_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim view As New PatientInfoView(Me)
        ElementHost1.Dock = DockStyle.Fill
        ElementHost1.Child = view
    End Sub
End Class
