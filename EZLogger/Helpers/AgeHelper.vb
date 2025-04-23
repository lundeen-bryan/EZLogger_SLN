' Namespace=EZLogger/Helpers
' Filename=AgeHelper.vb
' !See Label FileFooter for notes
Namespace Helpers

    ''' <summary>
    ''' Helper class for calculating age from a date of birth.
    ''' </summary>
    Public Class AgeHelper

        ''' <summary>
        ''' Calculates the age (in full years) based on a date of birth.
        ''' </summary>
        ''' <param name="birthDate">The patient's date of birth.</param>
        ''' <param name="asOfDate">The reference date to calculate the age from. Defaults to today if not specified.</param>
        ''' <returns>An integer representing the age in completed years.</returns>
        ''' <remarks>
        ''' This method accounts for whether the birthday has occurred yet in the current year.
        ''' If the current month and day are before the birth month and day, one year is subtracted.
        '''
        ''' For example:
        ''' - DOB: April 7, 2000 — As of April 6, 2025 → Age = 24
        ''' - DOB: April 7, 2000 — As of April 7, 2025 → Age = 25
        '''
        ''' This mirrors legacy behavior from the original VBA function 'return_age_int_fnc'.
        ''' </remarks>
        Public Shared Function CalculateAge(birthDate As Date, Optional asOfDate As Date? = Nothing) As Integer
            Dim currentDate As Date = If(asOfDate.HasValue, asOfDate.Value, Date.Today)

            ' Basic age calculation based on year difference
            Dim age As Integer = DateDiff(DateInterval.Year, birthDate, currentDate)

            ' Adjust if the birthday hasn't occurred yet this year
            If currentDate < New Date(currentDate.Year, birthDate.Month, birthDate.Day) Then
                age -= 1
            End If

            Return age
        End Function

    End Class

End Namespace

''FileFooter:
''===========================================================================================
'' Procedure: ......... AgeHelper.vb
'' Description: ....... Calculates the age of the person by DOB up to the month and day
'' Created: ........... 2025-04-22
'' Updated: ........... 2025-04-22
'' Module URL: ........ https://github.com/lundeen-bryan/EZLogger_SLN/tree/dev
'' Installs to: ....... EZLogger/Helpers
'' Compatibility: ..... Word VSTO
'' Contact Author: .... lundeen-bry;an
'' Copyright:  ........ ©2025. All rights reserved.
'' Called by: ......... DocumentPropertyHelper.WriteDataToDocProperties
'' Calls to: .......... n/a
'' Notes: ............. _
' (1) notes_here
''===========================================================================================