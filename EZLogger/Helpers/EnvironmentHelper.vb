Imports System.IO
Imports System.Text.Json
Imports EZLogger.Helpers

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

    ''' <summary>
    ''' Returns the expected OneDrive "Documents" path using the subpath defined in global_config.json.
    ''' </summary>
    ''' <returns>Full path to the user's synced OneDrive Documents folder.</returns>
    Public Function GetOneDriveDocumentsPath() As String
        Dim userProfile As String = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)
        Dim subPath As String = LoadOneDriveSubPathFromConfig()

        Return Path.Combine(userProfile, subPath)
    End Function

    ''' <summary>
    ''' Loads the OneDrive subpath from the JSON config file.
    ''' </summary>
    ''' <returns>Subpath defined under "paths.oneDriveDocumentsSubPath".</returns>
    Private Function LoadOneDriveSubPathFromConfig() As String
        Dim configPath As String = ConfigPathHelper.GetGlobalConfigPath()

        If Not File.Exists(configPath) Then
            Throw New FileNotFoundException("Configuration file not found: " & configPath)
        End If

        Dim json = File.ReadAllText(configPath)
        Dim doc = JsonDocument.Parse(json)

        Return doc.RootElement _
                  .GetProperty("paths") _
                  .GetProperty("oneDriveDocumentsSubPath") _
                  .GetString()
    End Function

End Module