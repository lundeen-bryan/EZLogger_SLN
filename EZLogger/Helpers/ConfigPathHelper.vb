Imports System.Collections.Generic
Imports System.IO
Imports System.Text.Json
Imports System.Windows

Namespace EZLogger.Helpers
    Public Module ConfigPathHelper

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

        Public Function GetDoctorList() As List(Of String)
            Dim doctorList As New List(Of String)

            Try
                ' Load the local config file
                If Not File.Exists(localConfigPath) Then
                    MessageBox.Show("Local config file not found.")
                    Return doctorList
                End If

                Dim jsonText As String = File.ReadAllText(localConfigPath)
                Using jsonDoc As JsonDocument = JsonDocument.Parse(jsonText)
                    Dim root = jsonDoc.RootElement

                    Dim spFilepath As JsonElement
                    If root.TryGetProperty("sp_filepath", spFilepath) Then
                        Dim doctorsPathElement As JsonElement
                        If spFilepath.TryGetProperty("doctors_list", doctorsPathElement) Then
                            Dim doctorsPath As String = doctorsPathElement.GetString()

                            If File.Exists(doctorsPath) Then
                                doctorList.AddRange(File.ReadAllLines(doctorsPath))
                            Else
                                MessageBox.Show("Doctors list file not found at: " & doctorsPath)
                            End If
                        End If
                    End If
                End Using

            Catch ex As Exception
                MessageBox.Show("Error loading doctors list: " & ex.Message)
            End Try

            Return doctorList
        End Function


    End Module
End Namespace
