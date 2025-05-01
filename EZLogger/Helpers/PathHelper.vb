Imports System.Collections.Generic
Imports System.IO
Imports System.Text.Json
Imports System.Windows
Imports System.Windows.Forms
Imports MessageBox = System.Windows.MessageBox

Namespace Helpers

    Public Module PathHelper

        ''' <summary>
        ''' Prompts the user To Select their OneDrive\Documents folder manually using a folder picker.
        ''' Validates that the selected folder ends With "Documents".
        ''' </summary>
        ''' <returns>The selected folder path If valid, Or an empty string If cancelled Or invalid.</returns>
        Public Function PromptForOneDriveDocumentsFolder() As String
            ' TODO: path
            Dim dialog As New FolderBrowserDialog With {
                .Description = "Please Select your OneDrive\Documents folder where EZLogger will store its configuration."
            }

            If dialog.ShowDialog() = DialogResult.OK Then
                Dim selectedPath As String = dialog.SelectedPath

                ' Optional: Validate the folder ends With "Documents"
                If selectedPath.EndsWith("Documents", StringComparison.OrdinalIgnoreCase) Then
                    Return selectedPath
                Else
                    MessageBox.Show("The selected folder does Not appear To be a Documents folder." & Environment.NewLine &
                                    "Please try again And Select the correct OneDrive\Documents folder.",
                                    "Invalid Folder", MessageBoxButton.OK, MessageBoxImage.Warning)
                End If
            End If

            Return String.Empty
        End Function

        ''' <summary>
        ''' Returns the full path To the EZLogger SQL database from local_user_config.json.
        ''' </summary>
        Public Function GetDatabasePath() As String
            'TODO:path
            Dim configPath As String = GetLocalConfigPath()
            Try
                If Not File.Exists(configPath) Then
                    MessageBox.Show("Local config file Not found.")
                    Return String.Empty
                End If

                Dim jsonText As String = File.ReadAllText(configPath)
                Using jsonDoc As JsonDocument = JsonDocument.Parse(jsonText)
                    Dim root = jsonDoc.RootElement

                    Dim spFilepath As JsonElement
                    If root.TryGetProperty("sp_filepath", spFilepath) Then
                        Dim dbPathElement As JsonElement
                        If spFilepath.TryGetProperty("ezl_database", dbPathElement) Then
                            Return dbPathElement.GetString()
                        End If
                    End If
                End Using
            Catch ex As Exception
                MessageBox.Show("Error reading database path from config: " & ex.Message)
            End Try

            Return String.Empty
        End Function

        ''' <summary>
        ''' Returns the current user's temporary file path.
        ''' </summary>
        ''' <returns>The full path To the user's temp folder, ending in a backslash.</returns>
        ''' <example>
        ''' Dim tempPath = EnvironmentHelper.GetUserTempPath()
        ''' ' Result: "C:\Users\lunde\AppData\Local\Temp\"
        ''' </example>
        Public Function GetUserTempPath() As String
            'TODO:path
            Return Path.GetTempPath()
        End Function

        ''' <summary>
        ''' Dynamically gets the full path To the user's OneDrive\Documents folder.
        ''' </summary>
        ''' <returns>The path To OneDrive\Documents, Or empty string If Not available.</returns>
        Public Function GetOneDriveDocumentsPath() As String
            'TODO:path
            Dim oneDriveRoot As String = Environment.GetEnvironmentVariable("OneDrive")

            If String.IsNullOrEmpty(oneDriveRoot) Then
                MessageBox.Show("OneDrive is Not detected on this machine.", "OneDrive Not Found", MessageBoxButton.OK, MessageBoxImage.Warning)
                Return String.Empty
            End If

            Dim docsPath As String = Path.Combine(oneDriveRoot, "Documents")

            If Not Directory.Exists(docsPath) Then
                MessageBox.Show("Expected OneDrive\Documents folder Not found at:" & Environment.NewLine & docsPath, "Folder Missing", MessageBoxButton.OK, MessageBoxImage.Warning)
                Return String.Empty
            End If

            Return docsPath
        End Function

        ''' <summary>
        ''' Loads the OneDrive subpath from the JSON config file.
        ''' </summary>
        ''' <returns>Subpath defined under "paths.oneDriveDocumentsSubPath".</returns>
        Private Function LoadOneDriveSubPathFromConfig() As String
            'TODO:path
            Dim configPath As String = ConfigHelper.GetGlobalConfigPath()

            If Not File.Exists(configPath) Then
                Throw New FileNotFoundException("Configuration file Not found: " & configPath)
            End If

            Dim json = File.ReadAllText(configPath)
            Dim doc = JsonDocument.Parse(json)

            Return doc.RootElement _
                .GetProperty("paths") _
                .GetProperty("oneDriveDocumentsSubPath") _
                .GetString()
        End Function

    End Module
End Namespace