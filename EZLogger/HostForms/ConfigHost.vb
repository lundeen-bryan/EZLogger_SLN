Imports System.Windows.Forms
Imports System.Windows.Forms.Integration
Imports EZLogger

Public Class ConfigHost
    Inherits Form

    Private ReadOnly elementHost As ElementHost
    Private ReadOnly configView As ConfigView

    Public Sub New()
        ' Set up form properties
        Me.Text = "EZLogger Config"
        Me.ClientSize = New Drawing.Size(850, 700)
        Me.StartPosition = FormStartPosition.CenterScreen
        Me.FormBorderStyle = FormBorderStyle.FixedDialog
        Me.MaximizeBox = False

        ' Initialize WPF control
        configView = New ConfigView(Me)

        ' Initialize and configure ElementHost
        elementHost = New ElementHost With {
            .Dock = DockStyle.Fill,
            .Child = configView
        }

        ' Add the ElementHost to the form
        Me.Controls.Add(elementHost)
    End Sub
End Class
