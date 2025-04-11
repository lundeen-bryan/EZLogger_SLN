Imports System.Drawing
Imports System.Windows.Forms

''' <summary>
''' Provides simplified access to the custom WPF-styled message box.
''' Shows the box in modeless mode by default, allowing interaction with Word.
''' </summary>
Public Module MsgBoxHelper

    ''' <summary>
    ''' Shows a simple message box with an OK button.
    ''' </summary>
    ''' <param name="message">The message to display.</param>
    Public Sub Show(message As String)
        Dim config As New MessageBoxConfig With {
            .Message = message,
            .ShowOk = True
        }
        Show(config, Nothing, Nothing)
    End Sub

    ''' <summary>
    ''' Shows a message box and handles the result with a callback.
    ''' </summary>
    ''' <param name="message">The message to display.</param>
    ''' <param name="onResult">Callback that receives the user's button choice.</param>
    Public Sub Show(message As String, onResult As Action(Of CustomMsgBoxResult))
        Dim config As New MessageBoxConfig With {
            .Message = message,
            .ShowOk = True
        }
        Show(config, onResult, Nothing)
    End Sub

    ''' <summary>
    ''' Shows a fully customized message box. Modeless by default.
    ''' </summary>
    ''' <param name="config">Message box configuration (buttons, text).</param>
    ''' <param name="onResult">Callback for result. If null, the box is fire-and-forget.</param>
    ''' <param name="ownerForm">Optional owner for manual positioning.</param>
    Public Sub Show(config As MessageBoxConfig,
                    Optional onResult As Action(Of CustomMsgBoxResult) = Nothing,
                    Optional ownerForm As Form = Nothing)

        Dim control As New CustomMsgBoxControl(config)
        Dim host As New CustomMsgBoxHost(control)

        ' Positioning
        If ownerForm IsNot Nothing Then
            host.StartPosition = FormStartPosition.Manual
            host.Location = New Point(ownerForm.Left + 50, ownerForm.Top + 100)
        Else
            host.StartPosition = FormStartPosition.CenterScreen
        End If

        host.TopMost = True

        ' Result handler
        AddHandler control.ButtonClicked, Sub(result)
                                              host.Result = result
                                              host.Close()
                                              If onResult IsNot Nothing Then
                                                  onResult.Invoke(result)
                                              End If
                                          End Sub

        ' Modeless by default
        host.Show()
    End Sub

End Module
