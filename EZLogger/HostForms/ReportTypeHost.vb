Imports System.Drawing
Imports System.Windows.Forms

Public Class ReportTypeHost

    Private Sub ReportTypeHost_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Set exact client size (excluding title bar, borders)
        Me.ClientSize = New Size(500, 800)

        ' Optional: prevent it from being resized too small
        Me.MinimumSize = New Size(520, 820)

        ' Optional: center on screen
        Me.StartPosition = FormStartPosition.CenterScreen

        ' Clean, consistent spacing
        Me.Padding = New Padding(0)

        Me.TopMost = True
        FormPositionHelper.MoveFormToTopLeftOfAllScreens(Me, 10, 10)
        Me.Show()

        ' Resize the ElementHost manually
        ElementHost1.Width = Me.ClientSize.Width - 40
        ElementHost1.Height = Me.ClientSize.Height - 40
        ElementHost1.Location = New Point(20, 20)
    End Sub

End Class
