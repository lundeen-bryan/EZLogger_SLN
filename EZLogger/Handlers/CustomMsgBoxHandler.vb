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

        host.Show()
        Return host.Result
    End Function

    Public Shared Sub ShowNonModal(config As MessageBoxConfig,
                               onResult As Action(Of CustomMsgBoxResult),
                               Optional ownerForm As Form = Nothing)

        Dim control As New CustomMsgBoxControl(config)
        Dim host As New CustomMsgBoxHost(control)

        If ownerForm IsNot Nothing Then
            host.StartPosition = FormStartPosition.Manual
            host.Location = New Point(ownerForm.Left + 50, ownerForm.Top + 100)
        Else
            host.StartPosition = FormStartPosition.CenterScreen
        End If

        host.TopMost = True

        ' Hook button clicks to call the provided callback, then close the window
        AddHandler control.ButtonClicked, Sub(result)
                                              host.Result = result
                                              host.Close()
                                              onResult?.Invoke(result)
                                          End Sub

        host.Show() ' Modeless
    End Sub

End Class

