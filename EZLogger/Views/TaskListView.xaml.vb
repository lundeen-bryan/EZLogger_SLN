Imports UserControl = System.Windows.Controls.UserControl

Partial Public Class TaskListView
    Inherits UserControl

    Private _handler As TaskListHandler

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

    Private Sub OnPreviewMouseLeftButtonDown(sender As Object, e As System.Windows.Input.MouseButtonEventArgs)
        ' Will add code later
    End Sub

    Private Sub OnMouseMove(sender As Object, e As System.Windows.Input.MouseEventArgs)
        ' Will add code later
    End Sub

    Private Sub OnDrop(sender As Object, e As System.Windows.DragEventArgs)
        ' Will add code later
    End Sub


End Class
