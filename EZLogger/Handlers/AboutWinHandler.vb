' Namespace=EZLogger/Handlers
' Filename=AboutWinHandler.vb
Imports EZLogger.Models
Imports System.IO
Imports System.Text.Json
Imports System.Windows.Forms

Namespace Handlers
    Public Class AboutWinHandler

        ''' <summary>
        ''' When clicked this closes the About Window
        ''' </summary>
        ''' <remarks>Takes no parameters</remarks>
        Public Sub HandleCloseClick(hostForm As Form)
            hostForm?.Close()
        End Sub

        ''' <summary>
        ''' Loads and parses the About information from the specified configuration file.
        ''' </summary>
        ''' <param name="configFilePath">The full path to the configuration file containing the About information.</param>
        ''' <returns>An AboutInfoResult object containing the parsed About information or an error message if the loading fails.</returns>
        ''' <remarks>
        ''' This function reads a JSON configuration file and extracts version-related information.
        ''' If an error occurs during the process, the error message is stored in the ErrorMessage property of the returned AboutInfoResult.
        ''' </remarks>
        Public Function LoadAboutInfo(configFilePath As String) As AboutInfoResult
            Dim result As New AboutInfoResult()

            Try
                Dim jsonText As String = File.ReadAllText(configFilePath)
                Dim doc As JsonDocument = JsonDocument.Parse(jsonText)

                Dim versionElement As JsonElement = doc.RootElement.GetProperty("version")

                result.CreatedBy = versionElement.GetProperty("created_by").GetString()
                result.SupportEmail = versionElement.GetProperty("support_email").GetString()
                result.LastUpdate = versionElement.GetProperty("date").GetString()
                result.VersionNumber = versionElement.GetProperty("number").GetString()
                result.LatestChange = versionElement.GetProperty("instructions").GetString()

            Catch ex As Exception
                result.ErrorMessage = $"Failed to load About information: {ex.Message}"
            End Try

            Return result
        End Function
    End Class
End Namespace
' Footer:
''===========================================================================================
'' Filename: .......... AboutWinHandler.vb
'' Description: ....... Shows the about EZLogger form
'' Created: ........... 2025-05-02
'' Updated: ........... 2025-05-02
'' Installs to: ....... EZLogger/Handlers
'' Compatibility: ..... VSTO
'' Contact Author: .... lundeen-bryan
'' Copyright:  ........ ©2025. All rights reserved.
'' Notes: ............. _
' (1) notes_here
''===========================================================================================