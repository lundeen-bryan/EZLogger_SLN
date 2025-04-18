Imports System.Collections.Generic
Imports System.IO
Imports System.Text.Json
Imports System.Windows
Imports System.Windows.Forms
Imports MessageBox = System.Windows.MessageBox

Namespace Helpers

Public Module ListHelper

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


End Module
End Namespace
