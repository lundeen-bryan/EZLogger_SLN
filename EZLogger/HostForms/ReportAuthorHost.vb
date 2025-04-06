Imports EZLogger.EZLogger.Views
Imports System.Windows.Forms
Imports System.Windows.Forms.Integration

Public Class ReportAuthorHost
    Inherits Form

    Private Sub ReportAuthorHost_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim view As New ReportAuthorView(Me)
        ElementHost1.Child = view
    End Sub

End Class
