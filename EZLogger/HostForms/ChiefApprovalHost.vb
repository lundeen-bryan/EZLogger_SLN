Imports EZLogger.EZLogger.Views
Imports System.Windows.Forms

Public Class ChiefApprovalHost

    Private Sub ChiefApprovalHost_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim view As New ChiefApprovalView(Me)
        ElementHost1.Child = view

        ' Set form size and title
        Me.ClientSize = New Drawing.Size(445, 560)
        Me.Text = ""

        ' Optional UI settings
        Me.MinimizeBox = False
        Me.MaximizeBox = False
        Me.ShowIcon = False
        Me.FormBorderStyle = FormBorderStyle.FixedSingle
        Me.TopMost = True

        ' Optional: center the window
        FormPositionHelper.MoveFormToTopLeftOfAllScreens(Me, 10, 10)

        ' Optional: manually size and position the ElementHost
        ElementHost1.Width = Me.ClientSize.Width - 40
        ElementHost1.Height = Me.ClientSize.Height - 40
        ElementHost1.Location = New Drawing.Point(20, 20)
    End Sub

End Class