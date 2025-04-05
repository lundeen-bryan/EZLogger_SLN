Imports System.Windows
Imports System.Windows.Forms

Namespace Handlers
    Public Class ConfigViewHandler
        Public Sub HandleCreateConfigClick()
            MsgBox("You clicked Create Config")
        End Sub

        Public Sub HandleSaveConfigClick()
            MsgBox("You clicked Save Config")
        End Sub

        Public Sub AddAlertButtonClick()
            MsgBox("You clicked Add Alert button")
        End Sub

        Public Sub EditAlertButtonClick()
            MsgBox("You clicked Edit Alert button")
        End Sub

        Public Sub DeleteAlertButtonClick()
            MsgBox("You clicked Delete Alert button")
        End Sub

        Public Sub AddCountyAlertButtonClick()
            MsgBox("You clicked Add County Alert button")
        End Sub

        Public Sub EditCountyAlertButtonClick()
            MsgBox("You clicked Edit County Alert button")
        End Sub

        Public Sub DeleteCountyAlertButtonClick()
            MsgBox("You clicked Delete County Alert button")
        End Sub

        Public Sub HandleEditEmailClick(r1 As System.Windows.Controls.RadioButton, r2 As System.Windows.Controls.RadioButton, r3 As System.Windows.Controls.RadioButton)
            If r1.IsChecked = True Then
                MsgBox("Secretaries radio is selected")
            ElseIf r2.IsChecked = True Then
                MsgBox("Friday radio is selected")
            ElseIf r3.IsChecked = True Then
                MsgBox("Competent radio is selected")
            Else
                MsgBox("No option is selected")
            End If
        End Sub

    End Class
End Namespace
