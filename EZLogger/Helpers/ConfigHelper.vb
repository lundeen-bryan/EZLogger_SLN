Imports System.Collections.Generic
Imports System.IO
Imports System.Text.Json
Imports System.Windows
Imports System.Windows.Forms
Imports MessageBox = System.Windows.MessageBox

Namespace Helpers

Public Module ConfigHelper

        ''' <summary>
        ''' Retrieves a list of strings from the global_config.json file using the specified section and key.
        ''' Example: section = "listbox", key = "report_type"
        ''' </summary>
        ''' <param name="section">Top-level section in the config (e.g., "listbox").</param>
        ''' <param name="key">Key within the section (e.g., "report_type").</param>
        ''' <returns>A list of strings from the specified location in the config.</returns>
        ''' <exception cref="FileNotFoundException">Thrown when the config file is missing.</exception>
        ''' <exception cref="KeyNotFoundException">Thrown if the section or key is missing.</exception>
        Public Function GetListFromGlobalConfig(section As String, key As String) As List(Of String)
            ' TODO:list
            Dim resultList As New List(Of String)

            Try
                Dim globalPath As String = GetGlobalConfigPath()

                If String.IsNullOrWhiteSpace(globalPath) OrElse Not File.Exists(globalPath) Then
                    Throw New FileNotFoundException("Global config file not found at: " & globalPath)
                End If

                Dim jsonText As String = File.ReadAllText(globalPath)
                Dim jsonDoc As JsonDocument = JsonDocument.Parse(jsonText)
                Dim root As JsonElement = jsonDoc.RootElement

                Dim sectionElement As JsonElement
                If Not root.TryGetProperty(section, sectionElement) Then
                    Throw New KeyNotFoundException($"Missing section '{section}' in config.")
                End If

                Dim keyElement As JsonElement
                If Not sectionElement.TryGetProperty(key, keyElement) Then
                    Throw New KeyNotFoundException($"Missing key '{key}' in section '{section}'.")
                End If

                For Each item In keyElement.EnumerateArray()
                    resultList.Add(item.GetString())
                Next

            Catch ex As Exception
                Throw New ApplicationException($"Error loading list from section '{section}', key '{key}'.", ex)
            End Try

            Return resultList
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

    ''' <summary>
    ''' Loads a list of doctor names from the text file defined in local_user_config.json.
    ''' The list is sorted alphabetically before being returned.
    ''' </summary>
    ''' <returns>A sorted List(Of String) containing one doctor name per line.</returns>
Public Function GetDoctorList() As List(Of String)
    'TODO:list
    Dim configPath As String = GetLocalConfigPath()

    Dim doctorList As New List(Of String)

    Try
    ' Path To the local user config file (Set earlier in this module)
    If Not File.Exists(configPath) Then
    MessageBox.Show("Local config file Not found.")
    Return doctorList
    End If

    ' Read the contents of the local_user_config.json file
    Dim jsonText As String = File.ReadAllText(configPath)

    ' Parse the JSON into a usable structure
    Using jsonDoc As JsonDocument = JsonDocument.Parse(jsonText)
    Dim root = jsonDoc.RootElement

    ' Navigate To the sp_filepath section
    Dim spFilepath As JsonElement
    If root.TryGetProperty("sp_filepath", spFilepath) Then

    ' Try To Get the doctors_list path from the config
    Dim doctorsPathElement As JsonElement
    If spFilepath.TryGetProperty("doctors_list", doctorsPathElement) Then
    Dim doctorsPath As String = doctorsPathElement.GetString()

    ' If the file exists, read all lines into the list
    If File.Exists(doctorsPath) Then
    doctorList.AddRange(File.ReadAllLines(doctorsPath))

    ' ✅ Sort the list alphabetically (A To Z)
    doctorList.Sort()
    Else
    MessageBox.Show("Doctors list file Not found at: " & doctorsPath)
    End If
    End If
    End If
    End Using

    Catch ex As Exception
    MessageBox.Show("Error loading doctors list: " & ex.Message)
    End Try

    ' Return the sorted list (Or empty list If something failed)
    Return doctorList
    End Function


Public Function GetDoctorListFilePath() As String
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
    Dim pathElement As JsonElement
    If spFilepath.TryGetProperty("doctors_list", pathElement) Then
    Return pathElement.GetString()
    End If
    End If
    End Using
    Catch ex As Exception
    MessageBox.Show("Error reading doctor list path: " & ex.Message)
    End Try

    Return String.Empty
    End Function

    ''' <summary>
    ''' Returns the full path To the EZLogger SQLite database from local_user_config.json.
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