Public Class CustomMsgBoxHandler

    Public Shared Function Show(config As MessageBoxConfig) As CustomMsgBoxResult
        ' Create the control and host form
        Dim control As New CustomMsgBoxControl(config)
        Dim host As New CustomMsgBoxHost(control)

        ' Hook up click logic
        AddHandler control.ButtonClicked, Sub(result)
                                              host.Result = result
                                              host.Close()
                                          End Sub

        ' Show dialog
        host.ShowDialog()
        Return host.Result
    End Function

End Class

