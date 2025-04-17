Imports System
Imports System.Windows.Forms
Imports System.Windows.Forms.Keys
Imports System.Windows.Input
Imports System.Windows.Forms.Integration
Imports EZLogger.Helpers
Imports EZLogger.Helpers.UIHelper ' Helps to call the UIHelper module
Imports System.Windows

Public Class UpdateInfoHost

    Private _shortcutHandler As ShortcutHandler

    Private Sub UpdateInfoHost_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Instantiate and assign the WPF UserControl
        Dim view As New UpdateInfoView(Me)
        ElementHost1.Child = view

        ' Set form size and title
        Me.ClientSize = New Drawing.Size(485, 770)

        ' Optional UI settings
        Me.MinimizeBox = False
        Me.MaximizeBox = False
        Me.ShowIcon = False
        Me.FormBorderStyle = FormBorderStyle.FixedSingle
        Me.StartPosition = FormStartPosition.CenterScreen

        ' Optional manual sizing
        ElementHost1.Width = Me.ClientSize.Width - 40
        ElementHost1.Height = Me.ClientSize.Height - 40
        ElementHost1.Location = New Drawing.Point(20, 20)

        ' Enable form-wide keyboard preview
        Me.KeyPreview = True

        ' Set up shortcut handler
        _shortcutHandler = New ShortcutHandler(Me)

        ' Alt+S = Trigger Save button in UpdateInfoView
        _shortcutHandler.RegisterShortcut(Keys.S, Keys.Alt, Sub()
                                                                TriggerButtonClick(view.BtnSaveProperty)
                                                            End Sub)
        ' Alt+G = Generate ID
        _shortcutHandler.RegisterShortcut(Keys.G, Keys.Alt, Sub()
                                                                TriggerButtonClick(view.BtnGenerateId)
                                                            End Sub)

        ' Alt+E = Open Evaluator
        _shortcutHandler.RegisterShortcut(Keys.E, Keys.Alt, Sub()
                                                                TriggerButtonClick(view.BtnEvaluator)
                                                            End Sub)

        ' Alt+D = Open Calendar
        _shortcutHandler.RegisterShortcut(Keys.D, Keys.Alt, Sub()
                                                                TriggerButtonClick(view.BtnCalendar)
                                                            End Sub)
        ' Alt+C = Close Form
        _shortcutHandler.RegisterShortcut(Keys.C, Keys.Alt, Sub()
                                                                TriggerButtonClick(view.BtnClose)
                                                            End Sub)

    End Sub

End Class
