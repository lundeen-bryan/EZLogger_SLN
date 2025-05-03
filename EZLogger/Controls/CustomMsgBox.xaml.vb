Imports System.Windows
Imports System.Windows.Controls

Public Class CustomMsgBoxControl
    Inherits UserControl

    Public Event ButtonClicked(result As CustomMsgBoxResult)

    Private _config As MessageBoxConfig

    Public Sub New(config As MessageBoxConfig)
        InitializeComponent()

        _config = config

        ' Set text
        MessageText.Text = config.Message

        ' Show/hide buttons
        YesButton.Visibility = If(config.ShowYes, Visibility.Visible, Visibility.Collapsed)
        NoButton.Visibility = If(config.ShowNo, Visibility.Visible, Visibility.Collapsed)
        OkButton.Visibility = If(config.ShowOk, Visibility.Visible, Visibility.Collapsed)

        ' Wire buttons
        AddHandler YesButton.Click, Sub() RaiseEvent ButtonClicked(CustomMsgBoxResult.Yes)
        AddHandler NoButton.Click, Sub() RaiseEvent ButtonClicked(CustomMsgBoxResult.No)
        AddHandler OkButton.Click, Sub() RaiseEvent ButtonClicked(CustomMsgBoxResult.OK)
    End Sub
End Class
