Imports System.Collections.Generic
Imports System.IO
Imports System.Text.Json
Imports System.Windows

Namespace Helpers
    Public Module ConfigPathHelper

        ''' <summary>
        ''' Gets the global_config.json filepath from the root folder
        ''' </summary>
        ''' <returns>Full path to the global config file, or an empty string if not found.</returns>
        Public Function GetGlobalConfigPath() As String
            Try
            	' Determine the directory of the solution file
                Dim exeDir As String = Path.GetDirectoryName(Assembly.GetExecutingAssembply().Location)
                Dim solutionDir As String = Directory.GetParent(Directory.GetParent(exeDir).FullName).FullName

                ' Combine with expected global config filename
                Dim globalConfigPath As String = Path.Combine(solutionDir, "global_config.json")

                ' Check if the file exists
                If Not File.Exists(globalConfigPath) Then
                    MessageBox.Show("Global config file not found at: " & Environment.NewLine & globalConfigPath, "Missing Config", MessageBoxButton.Ok, MessageBoxImage.Warning)
                    Return String.Empty
                End If

                Return globalConfigPath
            Catch ex As Exception
                MessageBox.Show("Error locating the global config." & ex.Message, "Error", MessageBoxButon.Ok, MessageBoxImage.Error)
                ReturnString.Empty
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

        ''' <summary>
        ''' Returns the full path to the EZLogger SQLite database from local_user_config.json.
        ''' </summary>
        Public Function GetDatabasePath() As String
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
        ''' Retrieves a string value from a JSON file given a dot-notated key path (e.g., "user.firstname").
        ''' Returns an empty string if the file or key is not found.
        ''' </summary>
        ''' <param name="filePath">Full path to the JSON file.</param>
        ''' <param name="keyPath">Dot-notated path to the value (e.g., "user.firstname").</param>
        ''' <returns>The string value at the specified key path, or empty string if not found.</returns>
        Public Function GetJsonValue(filePath As String, keyPath As String) As String
            Try
                If Not File.Exists(filePath) Then
                    Return String.Empty
                End If

                Dim jsonText As String = File.ReadAllText(filePath)
                Using jsonDoc As JsonDocument = JsonDocument.Parse(jsonText)
                    Dim currentElement As JsonElement = jsonDoc.RootElement

                    For Each key In keyPath.Split("."c)
                        If currentElement.ValueKind = JsonValueKind.Object AndAlso currentElement.TryGetProperty(key, currentElement) Then
                            Continue For
                        Else
                            Return String.Empty
                        End If
                    Next

                    ' At this point, currentElement should be the final property
                    If currentElement.ValueKind = JsonValueKind.String Then
                        Return currentElement.GetString()
                    Else
                        Return currentElement.ToString() ' fallback: convert other types to string
                    End If
                End Using

            Catch ex As Exception
                MessageBox.Show("Error reading JSON value: " & ex.Message, "JSON Error", MessageBoxButton.OK, MessageBoxImage.Error)
                Return String.Empty
            End Try

        End Function
        ''' <summary>
        ''' Retrieves the user's first name from the local_user_config.json file.
        ''' </summary>
        Public Function GetLocalUserFirstName() As String
            Return GetJsonValue(localConfigPath, "user.firstname")
        End Function

    End Module
End Namespace
