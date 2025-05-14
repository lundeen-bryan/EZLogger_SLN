' Namespace=EZLogger/Helpers
' Filename=TimerHelper.vb
' !See Label Footer for notes

Imports System.Windows.Controls
Imports System.Windows.Forms
Imports Dispatcher = System.Windows.Threading.Dispatcher

Namespace Helpers

    Public Module TimerHelper

        ''' <summary>
        ''' Disables a WPF button temporarily and re-enables it after the specified duration.
        ''' </summary>
        Public Sub DisableTemporarily(btn As System.Windows.Controls.Button, duration As Integer)
            If btn Is Nothing OrElse duration <= 0 Then Exit Sub

            btn.IsEnabled = False

            Dim dispatcherTimer As New System.Windows.Threading.DispatcherTimer()
            dispatcherTimer.Interval = TimeSpan.FromMilliseconds(duration)

            AddHandler dispatcherTimer.Tick, Sub(sender, e)
                                                 btn.IsEnabled = True
                                                 dispatcherTimer.Stop()
                                             End Sub
            dispatcherTimer.Start()
        End Sub

        ''' <summary>
        ''' Disables a WinForms button temporarily and re-enables it after the specified duration.
        ''' </summary>
        Public Sub DisableTemporarily(btn As System.Windows.Forms.Button, duration As Integer)
            If btn Is Nothing OrElse duration <= 0 Then Exit Sub

            btn.Enabled = False

            Dim t As New Timer() With {.Interval = duration}
            AddHandler t.Tick, Sub(sender, e)
                                   btn.Enabled = True
                                   t.Stop()
                                   t.Dispose()
                               End Sub
            t.Start()
        End Sub

    End Module

End Namespace

' Footer:
''===========================================================================================
'' Procedure: ......... TimerHelper.vb
'' Description: ....... Helps create a timout for buttons pressed and other uses for timer
'' Version: ........... 1.0.0 - major.minor.patch
'' Created: ........... 2025-05-14
'' Updated: ........... 2025-05-14
'' Installs to: ....... EZLogger/Helpers
'' Contact Author: .... lundeen-bryan
'' Copyright:  ........ n/a ©2025. All rights reserved.
'' Notes: ............. _
 '(1) notes_here
''===========================================================================================