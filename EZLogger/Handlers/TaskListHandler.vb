' Namespace=EZLogger/Handlers
' Filename=TaskListHandler.vb
' !See Label Footer for notes

Imports System.Collections.ObjectModel
Imports System.Collections.Specialized
Imports System.ComponentModel

Public Class TaskListHandler

    Public ReadOnly Property Tasks As ObservableCollection(Of TaskItem)

    Public Sub New()
        ' Load existing tasks
        Dim list = TasksIO.LoadTasks()
        Tasks = New ObservableCollection(Of TaskItem)(list)

        ' When items are added/removed, re-hook events and save
        AddHandler Tasks.CollectionChanged, AddressOf OnCollectionChanged

        ' Hook existing items
        For Each ti As TaskItem In Tasks
            AddHandler ti.PropertyChanged, AddressOf OnItemChanged
        Next
    End Sub

    ''' <summary>
    ''' Adds a new task to the task list based on the report filename.
    ''' </summary>
    ''' <param name="fileName">The name of the report file (no path).</param>
    Public Sub AddTaskFromReport(fileName As String)
        Try
            ' Create new task item
            Dim newTask As New TaskItem With {
                .Notes = fileName,
                .DateAdded = DateTime.Now,
                .IsCompleted = False
            }

            ' Add to in-memory list
            Tasks.Add(newTask)

            ' Save updated list to Tasks.xml
            TasksIO.SaveTasks(Tasks.ToList())

        Catch ex As Exception
            MsgBox($"Failed to add new task: {ex.Message}", MsgBoxStyle.Critical)
        End Try
    End Sub

    ''' <summary>
    ''' Handles changes in the Tasks collection, specifically when new items are added.
    ''' </summary>
    ''' <param name="sender">The source of the collection changed event.</param>
    ''' <param name="e">A NotifyCollectionChangedEventArgs that contains the event data.</param>
    ''' <remarks>
    ''' This method attaches the OnItemChanged event handler to each new TaskItem added to the collection.
    ''' After processing new items, it calls SaveAll() to persist the changes.
    ''' </remarks>
    Private Sub OnCollectionChanged(sender As Object, e As NotifyCollectionChangedEventArgs)
        ' Hook new items
        If e.NewItems IsNot Nothing Then
            For Each itm In e.NewItems.OfType(Of TaskItem)()
                AddHandler itm.PropertyChanged, AddressOf OnItemChanged
            Next
        End If
        SaveAll()
    End Sub

    ''' <summary>
    ''' Remove all tasks marked completed.
    ''' </summary>
    Public Sub RemoveCompletedTasks()
        For i = Tasks.Count - 1 To 0 Step -1
            If Tasks(i).IsCompleted Then
                Tasks.RemoveAt(i)
            End If
        Next
        SaveAll()
    End Sub

    Private Sub OnItemChanged(sender As Object, e As PropertyChangedEventArgs)
        SaveAll()
    End Sub

    Private Sub SaveAll()
        TasksIO.SaveTasks(Tasks.ToList())
    End Sub

End Class

' Footer:
''===========================================================================================
'' Filename: .......... TaskListHandler.vb
'' Description: ....... Handles the task list, adding and updating
'' Created: ........... 2025-05-02
'' Updated: ........... 2025-05-02
'' Installs to: ....... EZLogger/Handlers
'' Compatibility: ..... VSTO, WPF
'' Contact Author: .... lundeen-bryan
'' Copyright:  ........ ©2025. All rights reserved.
'' Notes: ............. _
' (1) notes_here
''===========================================================================================