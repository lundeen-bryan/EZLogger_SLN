Imports System.IO
Imports System.Text.Json
Imports EZLogger.Models

Namespace Handlers
    Public Class AboutWinHandler

        ''' <summary>
        ''' Reads version metadata from the global config file.
        ''' </summary>
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