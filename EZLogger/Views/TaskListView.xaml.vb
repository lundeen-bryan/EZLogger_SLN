Imports System.Windows
Imports System.Windows.Controls
Imports System.Windows.Input
Imports System.Windows.Media
Imports UserControl = System.Windows.Controls.UserControl

Partial Public Class TaskListView
    Inherits UserControl

    Private _handler As TaskListHandler
    Private _startPoint As Point
    Private _draggedItem As TaskItem

    Public Sub New()
        InitializeComponent()
        _handler = New TaskListHandler()
        Me.DataContext = _handler
        AddHandler RemoveCompletedBtn.Click, AddressOf OnRemoveCompleted
        AddHandler TasksDataGrid.PreviewMouseLeftButtonDown, AddressOf OnPreviewMouseLeftButtonDown
        AddHandler TasksDataGrid.MouseMove, AddressOf OnMouseMove
        AddHandler TasksDataGrid.Drop, AddressOf OnDrop
    End Sub

    Private Sub OnRemoveCompleted(sender As Object, e As System.Windows.RoutedEventArgs)
        _handler.RemoveCompletedTasks()
    End Sub

    Private Sub OnPreviewMouseLeftButtonDown(sender As Object, e As MouseButtonEventArgs)
        _startPoint = e.GetPosition(Nothing)

        Dim element = CType(e.OriginalSource, DependencyObject)
        While element IsNot Nothing AndAlso Not TypeOf element Is DataGridRow
            element = VisualTreeHelper.GetParent(element)
        End While

        If element IsNot Nothing Then
            Dim row = CType(element, DataGridRow)

            ' Check if the row item really is a TaskItem before casting
            Dim possibleItem = TryCast(row.Item, TaskItem)
            If possibleItem IsNot Nothing Then
                _draggedItem = possibleItem
            Else
                _draggedItem = Nothing
            End If
        Else
            _draggedItem = Nothing
        End If
    End Sub

    Private Sub OnMouseMove(sender As Object, e As MouseEventArgs)
        If e.LeftButton = MouseButtonState.Pressed AndAlso _draggedItem IsNot Nothing Then
            Dim currentPosition = e.GetPosition(Nothing)
            Dim diff = _startPoint - currentPosition

            ' Check if moved enough distance to start drag
            If Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance OrElse
           Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance Then

                ' Begin drag-drop operation
                Dim data = New DataObject(GetType(TaskItem), _draggedItem)
                DragDrop.DoDragDrop(TasksDataGrid, data, DragDropEffects.Move)
            End If
        End If
    End Sub

    Private Sub OnDrop(sender As Object, e As DragEventArgs)
        If _draggedItem Is Nothing Then Exit Sub

        Dim target = TryCast(GetRowItemUnderMouse(e), TaskItem)
        If target Is Nothing OrElse target Is _draggedItem Then Exit Sub

        Dim oldIndex = _handler.Tasks.IndexOf(_draggedItem)
        Dim newIndex = _handler.Tasks.IndexOf(target)

        If oldIndex >= 0 AndAlso newIndex >= 0 AndAlso oldIndex <> newIndex Then
            _handler.Tasks.Move(oldIndex, newIndex)
        End If

        ' Clear dragged item
        _draggedItem = Nothing
    End Sub

    Private Function GetRowItemUnderMouse(e As DragEventArgs) As Object
        Dim element = CType(e.OriginalSource, DependencyObject)
        While element IsNot Nothing AndAlso Not TypeOf element Is DataGridRow
            element = VisualTreeHelper.GetParent(element)
        End While

        If element IsNot Nothing Then
            Dim row = CType(element, DataGridRow)
            Return row.Item
        End If

        Return Nothing
    End Function

End Class
