Imports System.IO

''' <summary>
''' Provides helper methods for working with user environment paths.
''' </summary>
Public Module EnvironmentHelper

    ''' <summary>
    ''' Returns the current user's temporary file path.
    ''' </summary>
    ''' <returns>The full path to the user's temp folder, ending in a backslash.</returns>
    ''' <example>
    ''' Dim tempPath = EnvironmentHelper.GetUserTempPath()
    ''' ' Result: "C:\Users\lunde\AppData\Local\Temp\"
    ''' </example>
    Public Function GetUserTempPath() As String
        Return Path.GetTempPath()
    End Function

End Module