Imports System.Drawing
Imports System.Windows.Forms

Public Class CustomMsgBoxHandler

    Public Shared Function Show(config As MessageBoxConfig, Optional ownerForm As Form = Nothing) As CustomMsgBoxResult
        Dim control As New CustomMsgBoxControl(config)
        Dim host As New CustomMsgBoxHost(control)

        If ownerForm IsNot Nothing Then
            host.StartPosition = FormStartPosition.Manual
            host.Location = New Point(ownerForm.Left + 50, ownerForm.Top + 100)
        Else
            host.StartPosition = FormStartPosition.CenterScreen
        End If

        host.TopMost = True

        AddHandler control.ButtonClicked, Sub(result)
                                              host.Result = result
                                              host.Close()
                                          End Sub

        host.ShowDialog()
        Return host.Result
    End Function

End Class

