Imports System.Windows.Forms
Imports System.Windows.Forms.Integration
Imports System.Windows
Imports EZLogger

Public Class CustomMsgBoxHost
    Inherits Form

    Public Property Result As CustomMsgBoxResult = CustomMsgBoxResult.None

    Public Sub New(control As CustomMsgBoxControl)
        Me.Text = "EZLogger Message"
        Me.StartPosition = FormStartPosition.CenterScreen
        Me.FormBorderStyle = FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.BackColor = Drawing.Color.Black

        ' 🔥 New: let WPF measure its size BEFORE embedding
        control.Measure(New Size(Double.PositiveInfinity, Double.PositiveInfinity))
        control.Arrange(New Rect(0, 0, control.DesiredSize.Width, control.DesiredSize.Height))

        Dim width = CInt(Math.Ceiling(control.DesiredSize.Width))
        Dim height = CInt(Math.Ceiling(control.DesiredSize.Height))

        ' 🔧 Add padding for border/margins if needed
        Me.Width = width + 40
        Me.Height = height + 60

        ' Embed it
        Dim host As New ElementHost With {
            .Child = control,
            .Dock = DockStyle.Fill
        }

        Me.Controls.Add(host)
    End Sub
End Class
