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

        ' Try to find the clicked row
        Dim element = CType(e.OriginalSource, DependencyObject)
        While element IsNot Nothing AndAlso Not TypeOf element Is DataGridRow
            element = VisualTreeHelper.GetParent(element)
        End While

        If element IsNot Nothing Then
            Dim row = CType(element, DataGridRow)
            _draggedItem = CType(row.Item, TaskItem)
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

    Private Sub OnDrop(sender As Object, e As System.Windows.DragEventArgs)
        ' Will add code later
    End Sub


End Class
