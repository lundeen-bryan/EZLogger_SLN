Imports System.Windows.Forms

Public Class TaskListHost

    Private Sub TaskListHost_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim view As New TaskListView()
        ElementHost1.Child = view

        ' Set form size and title
        Me.ClientSize = New Drawing.Size(600, 540)
        Me.Text = "To Do List Manager"

        ' Optional UI settings
        Me.MinimizeBox = False
        Me.MaximizeBox = False
        Me.ShowIcon = False
        Me.FormBorderStyle = FormBorderStyle.Sizable

        ' Place form in top left of all windows
        FormPositionHelper.MoveFormToTopLeftOfAllScreens(Me, 10, 10)

        ' Optional: manually size and position the ElementHost
        ElementHost1.Width = Me.ClientSize.Width - 40
        ElementHost1.Height = Me.ClientSize.Height - 40
        ElementHost1.Location = New Drawing.Point(20, 20)
    End Sub

End Class