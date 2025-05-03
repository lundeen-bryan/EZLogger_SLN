Imports EZLogger.Handlers

Public Class EvaluatorHost

    Private _shortcutHelper As ShortcutHandler
    Private _view As EvaluatorView

    Private Sub EvaluatorHost_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        _view = New EvaluatorView(Me)
        ElementHost1.Child = _view

        _shortcutHelper = New ShortcutHandler(Me)

        Dim handler As New EvaluatorHandler()
        handler.RegisterKeyboardShortcuts(_shortcutHelper, _view)

        ' Set form size and title
        Me.ClientSize = New Drawing.Size(510, 665)
        Me.Text = ""

        ' Optional UI settings
        Me.MinimizeBox = False
        Me.MaximizeBox = False
        Me.ShowIcon = False
        Me.FormBorderStyle = FormBorderStyle.FixedSingle
        Me.KeyPreview = True

        ' Optional: center the window
        Me.TopMost = True
        FormPositionHelper.MoveFormToTopLeftOfAllScreens(Me, 10, 10)

        ' Optional: manually size and position the ElementHost
        ElementHost1.Width = Me.ClientSize.Width - 40
        ElementHost1.Height = Me.ClientSize.Height - 40
        ElementHost1.Location = New Drawing.Point(20, 20)
    End Sub

End Class