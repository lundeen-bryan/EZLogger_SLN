Imports System.Collections.Generic
Imports System.IO
Imports System.Text.Json
Imports System.Windows
Imports System.Windows.Forms
Imports MessageBox = System.Windows.MessageBox

Namespace Helpers

    Public Module ConfigHelper

        ''' <summary>
        ''' Ensures and returns the path to the local_user_config.json file inside %USERPROFILE%\.ezlogger.
        ''' If the file or folder does not exist, it will be created automatically.
        ''' </summary>
        Public Function GetLocalConfigPath() As String
            Return EnsureLocalUserConfigFileExists()
        End Function

        ''' <summary>
        ''' Updates the local_user_config.json file (in %USERPROFILE%\.ezlogger)
        ''' to include the selected path to the global_config.json under sp_filepath.global_config_file.
        ''' </summary>
        ''' <param name="globalConfigPath">The full path to the selected global_config.json file.</param>
        Public Sub UpdateLocalConfigWithGlobalPath(globalConfigPath As String)
            Try
                ' Get the full path to local_user_config.json (auto-created if missing)
                Dim localConfigPath As String = GetLocalConfigPath()

                ' Read existing contents of the config file
                Dim json As String = File.ReadAllText(localConfigPath)
                Dim doc = JsonDocument.Parse(json)

                ' Convert current root structure to a dictionary
                Dim rootDict As New Dictionary(Of String, Object)
                For Each prop In doc.RootElement.EnumerateObject()
                    rootDict(prop.Name) = prop.Value
                Next

                ' Overwrite or create the sp_filepath section with the new global config path
                Dim spSection As New Dictionary(Of String, String) From {
            {"global_config_file", globalConfigPath}
        }
                rootDict("sp_filepath") = spSection

                ' Serialize the updated structure back to JSON
                Dim options As New JsonSerializerOptions With {.WriteIndented = True}
                Dim updatedJson As String = JsonSerializer.Serialize(rootDict, options)

                ' Write the updated content to local_user_config.json
                File.WriteAllText(localConfigPath, updatedJson)

            Catch ex As Exception
                MessageBox.Show("Failed to update local_user_config.json: " & ex.Message, "Write Error", MessageBoxButton.OK, MessageBoxImage.Error)
            End Try
        End Sub

        ''' <summary>
        ''' Prompts the user to select their global_config.json file and returns the selected path.
        ''' </summary>
        ''' <returns>Full path to the selected config file, or empty string if cancelled.</returns>
        Public Function PromptForGlobalConfigFile() As String
            Dim dialog As New OpenFileDialog With {
        .Title = "Select your EZLogger global_config.json file",
        .Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*"
    }

            If dialog.ShowDialog() = DialogResult.OK Then
                Dim selectedFile As String = dialog.FileName

                If File.Exists(selectedFile) AndAlso selectedFile.EndsWith(".json", StringComparison.OrdinalIgnoreCase) Then
                    Return selectedFile
                Else
                    MessageBox.Show("The selected file is not a valid JSON file.", "Invalid Selection", MessageBoxButton.OK, MessageBoxImage.Warning)
                End If
            End If

            Return String.Empty
        End Function

        ''' <summary>
        ''' Prompts the user to select their OneDrive\Documents folder manually using a folder picker.
        ''' Validates that the selected folder ends with "Documents".
        ''' </summary>
        ''' <returns>The selected folder path if valid, or an empty string if cancelled or invalid.</returns>
        Public Function PromptForOneDriveDocumentsFolder() As String
            Dim dialog As New FolderBrowserDialog With {
        .Description = "Please select your OneDrive\Documents folder where EZLogger will store its configuration."
    }

            If dialog.ShowDialog() = DialogResult.OK Then
                Dim selectedPath As String = dialog.SelectedPath

                ' Optional: Validate the folder ends with "Documents"
                If selectedPath.EndsWith("Documents", StringComparison.OrdinalIgnoreCase) Then
                    Return selectedPath
                Else
                    MessageBox.Show("The selected folder does not appear to be a Documents folder." & Environment.NewLine &
                            "Please try again and select the correct OneDrive\Documents folder.",
                            "Invalid Folder", MessageBoxButton.OK, MessageBoxImage.Warning)
                End If
            End If

            Return String.Empty
        End Function

        ''' <summary>
        ''' Ensures the local EZLogger configuration folder and config file exist in the user's home directory.
        ''' </summary>
        ''' <returns>The full path to local_user_config.json, or an empty string if failed.</returns>
        Public Function EnsureLocalUserConfigFileExists() As String
            Try
                ' Get the user's home directory
                Dim userHome As String = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)

                ' Build the path to the .ezlogger directory
                Dim ezloggerDir As String = Path.Combine(userHome, ".ezlogger")

                ' Build the path to the config file
                Dim configFile As String = Path.Combine(ezloggerDir, "local_user_config.json")

                ' Create the .ezlogger folder if it doesn't exist
                If Not Directory.Exists(ezloggerDir) Then
                    Directory.CreateDirectory(ezloggerDir)
                End If

                ' Create a basic config file if it doesn't exist
                If Not File.Exists(configFile) Then
                    Dim initialJson = New With {
                        .status = "created",
                        .created_at = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss")
                    }

                    Dim options As New JsonSerializerOptions With {.WriteIndented = True}
                    Dim json As String = JsonSerializer.Serialize(initialJson, options)
                    File.WriteAllText(configFile, json)
                End If

                ' Return the full path
                Return configFile

            Catch ex As Exception
                MessageBox.Show("Error creating local_user_config.json in the user profile: " & ex.Message, "EZLogger Config Error", MessageBoxButton.OK, MessageBoxImage.Error)
                Return String.Empty
            End Try
        End Function

        ''' <summary>
        ''' Reads the local_user_config.json and extracts the path to the global config file.
        ''' </summary>
        ''' <returns>Full path to the global config file, or an empty string if not found.</returns>
        Public Function GetGlobalConfigPath() As String
            Dim configPath As String = GetLocalConfigPath()

            Try
                If Not File.Exists(configPath) Then
                    MessageBox.Show("Local config file not found at:" & Environment.NewLine & configPath, "Missing Config", MessageBoxButton.OK, MessageBoxImage.Warning)
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

                    MessageBox.Show("The key 'sp_filepath.global_config_file' was not found in the local config.", "Config Error", MessageBoxButton.OK, MessageBoxImage.Error)
                    Return String.Empty
                End Using

            Catch ex As Exception
                MessageBox.Show("Error reading local config:" & Environment.NewLine & ex.Message, "Config Error", MessageBoxButton.OK, MessageBoxImage.Error)
                Return String.Empty
            End Try
        End Function

        Public Function GetOpinionList() As List(Of String)
            Dim opinionList As New List(Of String)

            Try
                Dim globalPath As String = GetGlobalConfigPath()

                If String.IsNullOrWhiteSpace(globalPath) OrElse Not File.Exists(globalPath) Then
                    MessageBox.Show("Global config file not found at:" & Environment.NewLine & globalPath, "Missing Config", MessageBoxButton.OK, MessageBoxImage.Warning)
                    Return opinionList
                End If

                Dim jsonText As String = File.ReadAllText(globalPath)
                Using jsonDoc As JsonDocument = JsonDocument.Parse(jsonText)
                    Dim root = jsonDoc.RootElement

                    If root.TryGetProperty("listbox", root) AndAlso
               root.TryGetProperty("opinions", root) Then

                        For Each item In root.EnumerateArray()
                            opinionList.Add(item.GetString())
                        Next
                    Else
                        MessageBox.Show("Unable to find 'listbox.opinions' in the global config.", "Config Error", MessageBoxButton.OK, MessageBoxImage.Error)
                    End If
                End Using

            Catch ex As Exception
                MessageBox.Show("Error loading opinion list: " & ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error)
            End Try

            Return opinionList
        End Function

        Public Function GetReportTypeList() As List(Of String)
            Dim reportTypes As New List(Of String)

            Try
                Dim globalPath As String = GetGlobalConfigPath()

                If String.IsNullOrWhiteSpace(globalPath) OrElse Not File.Exists(globalPath) Then
                    MessageBox.Show("Global config file not found at:" & Environment.NewLine & globalPath, "Missing Config", MessageBoxButton.OK, MessageBoxImage.Warning)
                    Return reportTypes
                End If

                Dim jsonText As String = File.ReadAllText(globalPath)
                Using jsonDoc As JsonDocument = JsonDocument.Parse(jsonText)
                    Dim root = jsonDoc.RootElement

                    Dim listboxElement As JsonElement
                    If root.TryGetProperty("listbox", listboxElement) Then
                        Dim reportTypeElement As JsonElement
                        If listboxElement.TryGetProperty("report_type", reportTypeElement) Then
                            For Each item In reportTypeElement.EnumerateArray()
                                reportTypes.Add(item.GetString())
                            Next
                        End If
                    End If

                End Using

            Catch ex As Exception
                MessageBox.Show("Error loading report types: " & ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error)
            End Try

            Return reportTypes
        End Function

        ''' <summary>
        ''' Loads a list of doctor names from the text file defined in local_user_config.json.
        ''' The list is sorted alphabetically before being returned.
        ''' </summary>
        ''' <returns>A sorted List(Of String) containing one doctor name per line.</returns>
        Public Function GetDoctorList() As List(Of String)
            Dim configPath As String = GetLocalConfigPath()

            Dim doctorList As New List(Of String)

            Try
                ' Path to the local user config file (set earlier in this module)
                If Not File.Exists(configPath) Then
                    MessageBox.Show("Local config file not found.")
                    Return doctorList
                End If

                ' Read the contents of the local_user_config.json file
                Dim jsonText As String = File.ReadAllText(configPath)

                ' Parse the JSON into a usable structure
                Using jsonDoc As JsonDocument = JsonDocument.Parse(jsonText)
                    Dim root = jsonDoc.RootElement

                    ' Navigate to the sp_filepath section
                    Dim spFilepath As JsonElement
                    If root.TryGetProperty("sp_filepath", spFilepath) Then

                        ' Try to get the doctors_list path from the config
                        Dim doctorsPathElement As JsonElement
                        If spFilepath.TryGetProperty("doctors_list", doctorsPathElement) Then
                            Dim doctorsPath As String = doctorsPathElement.GetString()

                            ' If the file exists, read all lines into the list
                            If File.Exists(doctorsPath) Then
                                doctorList.AddRange(File.ReadAllLines(doctorsPath))

                                ' ✅ Sort the list alphabetically (A to Z)
                                doctorList.Sort()
                            Else
                                MessageBox.Show("Doctors list file not found at: " & doctorsPath)
                            End If
                        End If
                    End If
                End Using

            Catch ex As Exception
                MessageBox.Show("Error loading doctors list: " & ex.Message)
            End Try

            ' Return the sorted list (or empty list if something failed)
            Return doctorList
        End Function


        Public Function GetDoctorListFilePath() As String
            Dim configPath As String = GetLocalConfigPath()

            Try
                If Not File.Exists(configPath) Then
                    MessageBox.Show("Local config file not found.")
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
        ''' Returns the full path to the EZLogger SQLite database from local_user_config.json.
        ''' </summary>
        Public Function GetDatabasePath() As String
            Dim configPath As String = GetLocalConfigPath()
            Try
                If Not File.Exists(configPath) Then
                    MessageBox.Show("Local config file not found.")
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
        ''' <returns>The full path to the user's temp folder, ending in a backslash.</returns>
        ''' <example>
        ''' Dim tempPath = EnvironmentHelper.GetUserTempPath()
        ''' ' Result: "C:\Users\lunde\AppData\Local\Temp\"
        ''' </example>
        Public Function GetUserTempPath() As String
            Return Path.GetTempPath()
        End Function

        ''' <summary>
        ''' Dynamically gets the full path to the user's OneDrive\Documents folder.
        ''' </summary>
        ''' <returns>The path to OneDrive\Documents, or empty string if not available.</returns>
        Public Function GetOneDriveDocumentsPath() As String
            Dim oneDriveRoot As String = Environment.GetEnvironmentVariable("OneDrive")

            If String.IsNullOrEmpty(oneDriveRoot) Then
                MessageBox.Show("OneDrive is not detected on this machine.", "OneDrive Not Found", MessageBoxButton.OK, MessageBoxImage.Warning)
                Return String.Empty
            End If

            Dim docsPath As String = Path.Combine(oneDriveRoot, "Documents")

            If Not Directory.Exists(docsPath) Then
                MessageBox.Show("Expected OneDrive\Documents folder not found at:" & Environment.NewLine & docsPath, "Folder Missing", MessageBoxButton.OK, MessageBoxImage.Warning)
                Return String.Empty
            End If

            Return docsPath
        End Function

        ''' <summary>
        ''' Loads the OneDrive subpath from the JSON config file.
        ''' </summary>
        ''' <returns>Subpath defined under "paths.oneDriveDocumentsSubPath".</returns>
        Private Function LoadOneDriveSubPathFromConfig() As String
            Dim configPath As String = ConfigHelper.GetGlobalConfigPath()

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
End Namespace
