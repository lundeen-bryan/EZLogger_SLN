Imports System.Windows

Namespace Helpers

    Public Module UIHelper

        ''' <summary>
        ''' Simulates a WPF button click using the routed event system.
        ''' </summary>
        Public Sub TriggerButtonClick(button As System.Windows.Controls.Button)
            button.RaiseEvent(New RoutedEventArgs(System.Windows.Controls.Button.ClickEvent))
        End Sub

    End Module

End Namespace
