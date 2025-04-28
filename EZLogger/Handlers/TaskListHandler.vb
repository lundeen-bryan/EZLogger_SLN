Imports System
Imports System.Collections.ObjectModel
Imports System.Collections.Specialized
Imports System.ComponentModel
Imports Models
Imports Helpers

Public Class TaskListHandler

    Public ReadOnly Property Tasks As ObservableCollection(Of TaskItem)

    Public Sub New()
        ' Load existing tasks
        Dim list = TasksIO.LoadTasks()
        Tasks = New ObservableCollection(Of TaskItem)(list)

        ' When items are added/removed, re-hook events and save
        AddHandler Tasks.CollectionChanged, AddressOf OnCollectionChanged

        ' Hook existing items
        For Each ti In Tasks
            AddHandler ti.PropertyChanged, AddressOf OnItemChanged
        Next
    End Sub

    Private Sub OnCollectionChanged(sender As Object, e As NotifyCollectionChangedEventArgs)
        ' Hook new items
        If e.NewItems IsNot Nothing Then
            For Each itm In e.NewItems.OfType(Of TaskItem)()
                AddHandler itm.PropertyChanged, AddressOf OnItemChanged
            Next
        End If
        SaveAll()
    End Sub

    Private Sub OnItemChanged(sender As Object, e As PropertyChangedEventArgs)
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

    Private Sub SaveAll()
        TasksIO.SaveTasks(Tasks.ToList())
    End Sub

End Class
