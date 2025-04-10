Imports System.Collections.Generic
Imports System.IO
Imports System.Reflection
Imports System.Text.Json
Imports System.Windows

Namespace Helpers
    Public Module ConfigPathHelper

        Public Function GetGlobalConfigPath() As String
#If DEBUG Then
' 📌  CONFIG PATH PRODUCTION LOGIC.md 📝 🗑️
            ' Development override path
            Dim devPath As String = "C:\Users\lunde\repos\cs\ezlogger\EZLogger_SLN\temp\EzLogger_Databases\global_config.json"
            If File.Exists(devPath) Then Return devPath
#End If

            ' Production: Load path from local_user_config.json
            Dim localConfigPath As String = GetLocalUserConfigPath(True)
            If String.IsNullOrWhiteSpace(localConfigPath) Then Return String.Empty

            Dim globalPath = GetJsonValue(localConfigPath, "sp_filepath.global_config_file")
            If File.Exists(globalPath) Then Return globalPath

            MessageBox.Show("Global config not found at path from local config:" & vbCrLf & globalPath, "Missing Config", MessageBoxButton.OK, MessageBoxImage.Warning)
            Return String.Empty
        End Function

        Public Function GetLocalUserConfigPath(Optional validateExistence As Boolean = False) As String
            Dim documentsPath As String = LoadOneDriveOrFallbackPath()
            If String.IsNullOrWhiteSpace(documentsPath) Then
                If validateExistence Then
                    MessageBox.Show("Unable to determine OneDrive or fallback path.", "Path Error", MessageBoxButton.OK, MessageBoxImage.Warning)
                End If
                Return String.Empty
            End If

            Dim configPath As String = Path.Combine(documentsPath, ".ezlogger", "local_user_config.json")

            If validateExistence AndAlso Not File.Exists(configPath) Then
                MessageBox.Show("Local config file not found at:" & Environment.NewLine & configPath, "Missing Config", MessageBoxButton.OK, MessageBoxImage.Warning)
                Return String.Empty
            End If

            Return configPath
        End Function

        Public Function LoadOneDriveOrFallbackPath() As String
            Try
                Dim configPath As String = GetGlobalConfigPath()
                If String.IsNullOrWhiteSpace(configPath) OrElse Not File.Exists(configPath) Then Return String.Empty

                Dim jsonText As String = File.ReadAllText(configPath)
                Using doc = JsonDocument.Parse(jsonText)
                    Dim root = doc.RootElement

                    Dim userpathsElement As JsonElement
                    If root.TryGetProperty("userpaths", userpathsElement) Then
                        Dim oneDrivePath As String = ""
                        Dim fallbackPath As String = ""

                        Dim oneDriveElement As JsonElement
                        If userpathsElement.TryGetProperty("oneDriveDocumentsSubPath", oneDriveElement) Then
                            oneDrivePath = oneDriveElement.GetString()
                        End If

                        Dim fallbackElement As JsonElement
                        If userpathsElement.TryGetProperty("fallbackDocumentsPath", fallbackElement) Then
                            fallbackPath = fallbackElement.GetString()
                        End If

                        Dim userProfile = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)
                        Dim primaryFullPath = Path.Combine(userProfile, oneDrivePath.TrimStart("\"c))
                        If Directory.Exists(primaryFullPath) Then Return primaryFullPath

                        If Directory.Exists(fallbackPath) Then Return fallbackPath
                    End If
                End Using
            Catch ex As Exception
                MessageBox.Show("Error reading OneDrive or fallback path: " & ex.Message, "Path Error", MessageBoxButton.OK, MessageBoxImage.Error)
            End Try
            Return String.Empty
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
                    If root.TryGetProperty("listbox", root) AndAlso root.TryGetProperty("opinions", root) Then
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
                Dim localConfigPath As String = GetLocalUserConfigPath(True)
                If String.IsNullOrWhiteSpace(localConfigPath) Then Return doctorList

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
            Return doctorList
        End Function

        Public Function GetDoctorListFilePath() As String
            Try
                Dim localConfigPath As String = GetLocalUserConfigPath(True)
                If String.IsNullOrWhiteSpace(localConfigPath) Then Return String.Empty

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

        Public Function GetDatabasePath() As String
            Try
                Dim localConfigPath As String = GetLocalUserConfigPath(True)
                If String.IsNullOrWhiteSpace(localConfigPath) Then Return String.Empty

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

        Public Function GetJsonValue(filePath As String, keyPath As String) As String
            Try
                If Not File.Exists(filePath) Then Return String.Empty

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

                    If currentElement.ValueKind = JsonValueKind.String Then
                        Return currentElement.GetString()
                    Else
                        Return currentElement.ToString()
                    End If
                End Using
            Catch ex As Exception
                MessageBox.Show("Error reading JSON value: " & ex.Message, "JSON Error", MessageBoxButton.OK, MessageBoxImage.Error)
                Return String.Empty
            End Try
        End Function

        Public Function GetLocalUserFirstName() As String
            Dim localConfigPath As String = GetLocalUserConfigPath(True)
            Return GetJsonValue(localConfigPath, "user.firstname")
        End Function

    End Module
End Namespace
