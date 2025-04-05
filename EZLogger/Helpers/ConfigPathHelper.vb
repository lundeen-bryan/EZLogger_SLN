Imports System.Collections.Generic
Imports System.IO
Imports System.Text.Json
Imports System.Windows

Namespace Helpers
    Public Module ConfigPathHelper

        ' In production, replace this hardcoded path with the user's actual Documents path:
        ' Example: Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), ".ezlogger", "local_user_config.json")
        ' Or use: $"C:\Users\{Environment.UserName}\OneDrive - Department of State Hospitals\Documents\.ezlogger\local_user_config.json"
        ' Hardcoded local config path for now
        Private ReadOnly localConfigPath As String = "C:\Users\lunde\repos\cs\ezlogger\EZLogger_SLN\temp\local_user_config.json"

        ''' <summary>
        ''' Reads the local_user_config.json and extracts the path to the global config file.
        ''' </summary>
        ''' <returns>Full path to the global config file, or an empty string if not found.</returns>
        Public Function GetGlobalConfigPath() As String
            Try
                If Not File.Exists(localConfigPath) Then
                    MessageBox.Show("Local config file not found at:" & Environment.NewLine & localConfigPath, "Missing Config", MessageBoxButton.OK, MessageBoxImage.Warning)
                    Return String.Empty
                End If

                Dim jsonText As String = File.ReadAllText(localConfigPath)
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
            Dim doctorList As New List(Of String)

            Try
                ' Path to the local user config file (set earlier in this module)
                If Not File.Exists(localConfigPath) Then
                    MessageBox.Show("Local config file not found.")
                    Return doctorList
                End If

                ' Read the contents of the local_user_config.json file
                Dim jsonText As String = File.ReadAllText(localConfigPath)

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
            Try
                If Not File.Exists(localConfigPath) Then
                    MessageBox.Show("Local config file not found.")
                    Return String.Empty
                End If

                Dim jsonText As String = File.ReadAllText(localConfigPath)
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

    End Module
End Namespace
