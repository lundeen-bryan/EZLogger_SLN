Imports System.Collections.Generic
Imports System.IO
Imports System.Text.Json
Imports System.Windows
Imports System.Windows.Forms
Imports EZLogger.Models
Imports MessageBox = System.Windows.MessageBox

Namespace Helpers

Public Module ConfigHelper

        ''' <summary>
        ''' Prompts the user to select a folder and returns the selected path.
        ''' </summary>
        ''' <param name="title">The description shown in the folder picker dialog.</param>
        ''' <returns>Selected folder path, or empty string if canceled.</returns>
        Public Function PromptForFolder(title As String) As String
            Dim dialog As New OpenFileDialog With {
                .Title = title,
                .CheckFileExists = False,
                .CheckPathExists = True,
                .FileName = "Select Folder"
            }

            If dialog.ShowDialog() = DialogResult.OK Then
                Return Path.GetDirectoryName(dialog.FileName)
            End If

            Return String.Empty
        End Function

        ''' <summary>
        ''' Ensures And returns the path To the local_user_config.json file inside %USERPROFILE%\.ezlogger.
        ''' If the file Or folder does Not exist, it will be created automatically.
        ''' </summary>
        Public Function GetLocalConfigPath() As String
    Return EnsureLocalUserConfigFileExists()
End Function

        ''' <summary>
        ''' Updates the local_user_config.json file (in %USERPROFILE%\.ezlogger)
        ''' To include the selected path To the global_config.json under sp_filepath.global_config_file.
        ''' </summary>
        ''' <param name="globalConfigPath">The full path To the selected global_config.json file.</param>
        Public Sub UpdateLocalConfigWithGlobalPath(globalConfigPath As String)
            Try
                Dim localConfigPath As String = GetLocalConfigPath()
                Dim json As String = File.ReadAllText(localConfigPath)

                Dim config = JsonSerializer.Deserialize(Of LocalUserConfig)(json)

                If config Is Nothing Then
                    Throw New Exception("Unable to parse local config file.")
                End If

                If config.sp_filepath Is Nothing Then
                    config.sp_filepath = New SPFilePathSection()
                End If

                config.sp_filepath.global_config_file = globalConfigPath

                Dim options As New JsonSerializerOptions With {.WriteIndented = True}
                Dim updatedJson As String = JsonSerializer.Serialize(config, options)

                File.WriteAllText(localConfigPath, updatedJson)

            Catch ex As Exception
                MessageBox.Show("Failed to update local_user_config.json: " & ex.Message,
                        "Write Error", MessageBoxButton.OK, MessageBoxImage.Error)
            End Try
        End Sub

        ''' <summary>
        ''' Prompts the user To Select their global_config.json file And returns the selected path.
        ''' </summary>
        ''' <returns>Full path To the selected config file, Or empty string If cancelled.</returns>
        Public Function PromptForGlobalConfigFile() As String
    ' TODO: config
    Dim dialog As New OpenFileDialog With {
    .Title = "Select your EZLogger global_config.json file",
    .Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*"
    }

    If dialog.ShowDialog() = DialogResult.OK Then
    Dim selectedFile As String = dialog.FileName

    If File.Exists(selectedFile) AndAlso selectedFile.EndsWith(".json", StringComparison.OrdinalIgnoreCase) Then
    Return selectedFile
    Else
    MessageBox.Show("The selected file is Not a valid JSON file.", "Invalid Selection", MessageBoxButton.OK, MessageBoxImage.Warning)
    End If
    End If

    Return String.Empty
    End Function

        ''' <summary>
        ''' Ensures the local EZLogger configuration folder And config file exist in the user's home directory.
        ''' </summary>
        ''' <returns>The full path To local_user_config.json, Or an empty string If failed.</returns>
        Public Function EnsureLocalUserConfigFileExists() As String
            Try
                Dim userHome As String = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)
                Dim ezloggerDir As String = Path.Combine(userHome, ".ezlogger")
                Dim configFile As String = Path.Combine(ezloggerDir, "local_user_config.json")

                If Not Directory.Exists(ezloggerDir) Then
                    Directory.CreateDirectory(ezloggerDir)
                End If

                If Not File.Exists(configFile) Then
                    Dim config As New LocalUserConfig With {
                ._comment = "EZLogger Local User Configuration File - last updated " & DateTime.Now.ToString("yyyy-MM-dd"),
                .this_config = New ThisConfigSection With {
                    ._comment = "Identifies this as a user-specific config file",
                    .name = "user config"
                },
                .sp_filepath = New SPFilePathSection With {
                    ._comment = "Local file paths used by the user for templates, contact databases, and shared config references",
                    .databases = "TBD",
                    .user_forensic_database = "TBD",
                    .user_forensic_library = "TBD",
                    .court_contact = "TBD",
                    .da_contact_database = "TBD",
                    .doctors_list = "TBD",
                    .global_config_file = "",
                    .hlv_data = "TBD",
                    .hlv_due = "TBD",
                    .ods_filepath = "TBD",
                    .properties_list = "TBD",
                    .sheriff_addresses = "TBD",
                    .templates = "TBD",
                    .ezl_database = "TBD"
                },
                .edo_filepath = New EDOFilePathSection With {
                    ._comment = "Shortcuts to shared drive paths, relative to a network root",
                    .forensic_office = "TBD",
                    .processed_reports = "TBD",
                    .tcars_folder = "TBD"
                }
            }

                    Dim options As New JsonSerializerOptions With {.WriteIndented = True}
                    Dim json As String = JsonSerializer.Serialize(config, options)
                    File.WriteAllText(configFile, json)
                End If

                Return configFile

            Catch ex As Exception
                MessageBox.Show("Error creating local_user_config.json in the user profile: " & ex.Message,
                        "EZLogger Config Error", MessageBoxButton.OK, MessageBoxImage.Error)
                Return String.Empty
            End Try
        End Function

        ''' <summary>
        ''' Reads the local_user_config.json And extracts the path To the global config file.
        ''' </summary>
        ''' <returns>Full path To the global config file, Or an empty string If Not found.</returns>
        Public Function GetGlobalConfigPath() As String
    'TODO:config
    Dim configPath As String = GetLocalConfigPath()

    Try
    If Not File.Exists(configPath) Then
    MessageBox.Show("Local config file Not found at:" & Environment.NewLine & configPath, "Missing Config", MessageBoxButton.OK, MessageBoxImage.Warning)
    Return String.Empty
    End If

    Dim jsonText As String = File.ReadAllText(configPath)
    Using jsonDoc As JsonDocument = JsonDocument.Parse(jsonText)
    Dim root = jsonDoc.RootElement

    Dim spFilepath As JsonElement
    If root.TryGetProperty("sp_filepath", spFilepath) Then
    Dim globalPathElement As JsonElement
    If spFilepath.TryGetProperty("global_config_file", globalPathElement) Then
    Return globalPathElement.GetString()
    End If
    End If

    MessageBox.Show("The key 'sp_filepath.global_config_file' was Not found in the local config.", "Config Error", MessageBoxButton.OK, MessageBoxImage.Error )
    Return String.Empty
    End Using

    Catch ex As Exception
    MessageBox.Show("Error reading local config:" & Environment.NewLine & ex.Message, "Config Error", MessageBoxButton.OK, MessageBoxImage.Error )
    Return String.Empty
    End Try
    End Function

    End Module
    End Namespace