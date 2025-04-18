Imports System.Collections.Generic
Imports System.IO
Imports System.Text.Json
Imports System.Windows
Imports System.Windows.Forms
Imports MessageBox = System.Windows.MessageBox

Namespace Helpers

Public Module ConfigHelper


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
    ' TODO config
    Try
    ' Get the full path To local_user_config.json (auto-created If missing)
    Dim localConfigPath As String = GetLocalConfigPath()

    ' Read existing contents of the config file
    Dim json As String = File.ReadAllText(localConfigPath)
    Dim doc = JsonDocument.Parse(json)

    ' Convert current root structure To a dictionary
    Dim rootDict As New Dictionary(Of String, Object)
    For Each prop In doc.RootElement.EnumerateObject()
    rootDict(prop.Name) = prop.Value
    Next

    ' Overwrite Or create the sp_filepath section With the New global config path
    Dim spSection As New Dictionary(Of String, String) From {
    {"global_config_file", globalConfigPath }
    }
    rootDict("sp_filepath") = spSection

    ' Serialize the updated structure back To JSON
    Dim options As New JsonSerializerOptions With {.WriteIndented = True }
    Dim updatedJson As String = JsonSerializer.Serialize(rootDict, options)

    ' Write the updated content To local_user_config.json
    File.WriteAllText(localConfigPath, updatedJson)

    Catch ex As Exception
    MessageBox.Show("Failed To update local_user_config.json: " & ex.Message, "Write Error", MessageBoxButton.OK, MessageBoxImage.Error )
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
    'TODO: config
    Try
    ' Get the user's home directory
    Dim userHome As String = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)

    ' Build the path To the .ezlogger directory
    Dim ezloggerDir As String = Path.Combine(userHome, ".ezlogger")

    ' Build the path To the config file
    Dim configFile As String = Path.Combine(ezloggerDir, "local_user_config.json")

    ' Create the .ezlogger folder If it doesn't exist
    If Not Directory.Exists(ezloggerDir) Then
    Directory.CreateDirectory(ezloggerDir)
    End If

    ' Create a basic config file If it doesn't exist
    If Not File.Exists(configFile) Then
    Dim initialJson = New With {
    .status = "created",
    .created_at = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss")
    }

    Dim options As New JsonSerializerOptions With {.WriteIndented = True }
    Dim json As String = JsonSerializer.Serialize(initialJson, options)
    File.WriteAllText(configFile, json)
    End If

    ' Return the full path
    Return configFile

    Catch ex As Exception
    MessageBox.Show("Error creating local_user_config.json in the user profile: " & ex.Message, "EZLogger Config Error", MessageBoxButton.OK, MessageBoxImage.Error )
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