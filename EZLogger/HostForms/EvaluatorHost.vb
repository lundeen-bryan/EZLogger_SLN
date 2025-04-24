Imports System.Windows.Forms.Integration
Imports EZLogger.Views ' Adjust namespace if needed

Public Class EvaluatorHost

    Private Sub EvaluatorHost_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim view As New EvaluatorView(Me)
        ElementHost1.Child = view

        ' Set form size and title
        Me.ClientSize = New Drawing.Size(510, 665)
        Me.Text = ""

        ' Optional UI settings
        Me.MinimizeBox = False
        Me.MaximizeBox = False
        Me.ShowIcon = False
        Me.FormBorderStyle = FormBorderStyle.FixedSingle

        ' Optional: center the window
        Me.TopMost = True
        FormPositionHelper.MoveFormToTopLeftOfAllScreens(Me, 10, 10)

        ' Optional: manually size and position the ElementHost
        ElementHost1.Width = Me.ClientSize.Width - 40
        ElementHost1.Height = Me.ClientSize.Height - 40
        ElementHost1.Location = New Drawing.Point(20, 20)
    End Sub

End Class