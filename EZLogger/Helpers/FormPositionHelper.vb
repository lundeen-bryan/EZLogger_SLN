Imports System.Drawing
Imports System.Windows.Forms

''' <summary>
''' Provides helper methods to position WinForms forms in a multi-monitor environment.
''' </summary>
Public Module FormPositionHelper

    ''' <summary>
    ''' Moves the specified form to a position relative to the top-left corner of the virtual desktop,
    ''' with optional X/Y offset support.
    ''' </summary>
    ''' <param name="form">The WinForms form to move.</param>
    ''' <param name="leftOffset">Horizontal offset in pixels from the left edge of the virtual desktop.</param>
    ''' <param name="topOffset">Vertical offset in pixels from the top edge of the virtual desktop.</param>
    Public Sub MoveFormToTopLeftOfAllScreens(form As Form,
                                             Optional leftOffset As Integer = 0,
                                             Optional topOffset As Integer = 0)
        If form Is Nothing Then Exit Sub

        form.StartPosition = FormStartPosition.Manual

        Dim virtualTopLeft As Point = SystemInformation.VirtualScreen.Location
        form.Left = virtualTopLeft.X + leftOffset
        form.Top = virtualTopLeft.Y + topOffset
    End Sub

End Module
