Imports System.Collections.ObjectModel
Imports System.Collections.Specialized
Imports EZLogger.Models
Imports EZLogger.Helpers

Namespace Handlers

    Public Class TaskListHandler

        Public ReadOnly Property Tasks As ObservableCollection(Of TaskItem)

        Public Sub New()
            Dim list = ConfigHelper.LoadTasks()
            Tasks = New ObservableCollection(Of TaskItem)(list)

            ' Save whenever items are added/removed
            AddHandler Tasks.CollectionChanged, AddressOf OnCollectionChanged

            ' Save when any property changes on an item
            For Each itm In Tasks
                AddHandler itm.PropertyChanged, AddressOf OnItemPropertyChanged
            Next
        End Sub

        Private Sub OnCollectionChanged(sender As Object, e As NotifyCollectionChangedEventArgs)
            If e.NewItems IsNot Nothing Then
                For Each itm In e.NewItems.OfType(Of TaskItem)()
                    AddHandler itm.PropertyChanged, AddressOf OnItemPropertyChanged
                Next
            End If
            SaveAll()
        End Sub

        Private Sub OnItemPropertyChanged(sender As Object, e As ComponentModel.PropertyChangedEventArgs)
            SaveAll()
        End Sub

        ''' <summary>
        ''' Removes all completed tasks from the collection.
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
            ConfigHelper.SaveTasks(Tasks.ToList())
        End Sub

    End Class

End Namespace
