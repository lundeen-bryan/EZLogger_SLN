' Namespace=EZLogger/Helpers
' Filename=TasksIO.vb
' !See Label Footer for notes

Imports EZLogger.Helpers
Imports System.IO
Imports System.Xml.Serialization

Module TasksIO

    ' Determine the path for Tasks.xml beside your existing user config
    Private ReadOnly Property TasksFilePath As String
        Get
            Dim configJsonPath = ConfigHelper.GetLocalConfigPath()
            Dim folder = Path.GetDirectoryName(configJsonPath)
            Return Path.Combine(folder, "Tasks.xml")
        End Get
    End Property

    ''' <summary>
    ''' Reads all tasks from Tasks.xml. If the file is missing or invalid, returns an empty list.
    ''' </summary>
    Public Function LoadTasks() As List(Of TaskItem)
        Try
            If Not File.Exists(TasksFilePath) Then
                Return New List(Of TaskItem)()
            End If

            Dim xs = New XmlSerializer(GetType(TaskCollection))
            Using fs As New FileStream(TasksFilePath, FileMode.Open, FileAccess.Read, FileShare.Read)
                Dim col = CType(xs.Deserialize(fs), TaskCollection)
                Return col.Items
            End Using

        Catch ex As Exception
            MsgBoxHelper.Show($"Failed to load tasks:{Environment.NewLine}{ex.Message}")
            Return New List(Of TaskItem)()
        End Try
    End Function

    ''' <summary>
    ''' Saves the given tasks list to Tasks.xml, overwriting the previous file.
    ''' </summary>
    Public Sub SaveTasks(tasks As List(Of TaskItem))
        Try
            ' Ensure directory exists
            Dim dir = Path.GetDirectoryName(TasksFilePath)
            If Not Directory.Exists(dir) Then Directory.CreateDirectory(dir)

            Dim xs = New XmlSerializer(GetType(TaskCollection))
            Using fs As New FileStream(TasksFilePath, FileMode.Create, FileAccess.Write, FileShare.None)
                xs.Serialize(fs, New TaskCollection() With {.Items = tasks})
            End Using

        Catch ex As Exception
            MsgBoxHelper.Show($"Failed to save tasks:{Environment.NewLine}{ex.Message}")
        End Try
    End Sub

End Module

' Footer:
''===========================================================================================
'' Procedure: ......... TasksIO.vb
'' Description: ....... Helps with adding tasks to task list
'' Version: ........... 1.0.0 - major.minor.patch
'' Created: ........... 2025-05-14
'' Updated: ........... 2025-05-14
'' Installs to: ....... EZLogger/Helpers
'' Contact Author: .... lundeen-bryan
'' Copyright:  ........ n/a ©2025. All rights reserved.
'' Notes: ............. _
 '(1) notes_here
''===========================================================================================