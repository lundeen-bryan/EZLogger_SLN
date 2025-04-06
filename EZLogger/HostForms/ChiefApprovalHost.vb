Imports EZLogger.EZLogger.Views
Imports System.Windows.Forms

Public Class ChiefApprovalHost
    Inherits Form

    Public Sub New()
        InitializeComponent()
        ElementHost1.Child = New ChiefApprovalView(Me) ' 👈 pass Me (the form) into the view
    End Sub
End Class
