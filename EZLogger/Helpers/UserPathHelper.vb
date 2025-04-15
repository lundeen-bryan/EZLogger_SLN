Imports System.IO

''' <summary>
''' Provides helper methods to retrieve user-specific file system paths.
''' </summary>
Public Module UserPathHelper

    ''' <summary>
    ''' Returns the expected OneDrive "Documents" path for a User who syncs with OneDrive.
    ''' </summary>
    ''' <returns>Full path to the user's synced OneDrive Documents folder.</returns>
    ''' <example>
    ''' Dim path As String = UserPathHelper.GetOneDriveDocumentsPath()
    ''' ' Output: "C:\Users\lunde\OneDrive - Department of State Hospitals\Documents"
    ''' </example>
    Public Function GetOneDriveDocumentsPath() As String
        Dim userProfile As String = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)

        ' Customize this part based on your org's OneDrive naming
        Dim oneDriveSubPath As String = "OneDrive - Department of State Hospitals\Documents"

        Return Path.Combine(userProfile, oneDriveSubPath)
    End Function

End Module