Imports System.Drawing

Namespace Helpers
    Public Module ColorHelper

        ''' <summary>
        ''' Returns the EZLogger highlight blue used for labels and info backgrounds.
        ''' Equivalent to hex color #DCEEFF.
        ''' </summary>
        Public Function GetEzLoggerBlue() As Color
            Return Color.FromArgb(220, 238, 255)
        End Function

        ''' <summary>
        ''' Returns the dark font color used for readability on EZLogger blue.
        ''' </summary>
        Public Function GetTextColorOnEzBlue() As Color
            Return Color.FromArgb(30, 30, 30) ' Dark gray/black
        End Function

    End Module
End Namespace
