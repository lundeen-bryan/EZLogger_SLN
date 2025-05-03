Imports EZLogger.Handlers
Imports EZLogger.Helpers
Imports UserControl = System.Windows.Controls.UserControl
Imports System.Windows
Imports System.Windows.Forms

Public Class TCARListView
    Inherits UserControl

    Private ReadOnly _handler As New TCARListHandler()
    Private ReadOnly _hostForm As Form

    Public Sub New(Optional hostForm As Form = Nothing)
        InitializeComponent()

        _hostForm = hostForm
        _handler = New TCARListHandler()
        WireUpButtons()
    End Sub

    Public Sub WireUpButtons()
        AddHandler Me.Loaded, AddressOf TCARListView_Loaded
        AddHandler DoneBtn.Click, AddressOf DoneBtn_Click
    End Sub

    Private Sub TCARListView_Loaded(sender As Object, e As RoutedEventArgs)
        ' Load TCAR records from the handler
        Dim records As List(Of TCARRecord) = _handler.LoadAllActive()

        ' Set the DataGrid's item source
        TCARGrid.ItemsSource = records
    End Sub

    ' This method is wired up in XAML and simply delegates to the handler
    Private Sub BtnSelectPatient_Click(sender As Object, e As RoutedEventArgs)
        _handler.HandleTCARSelect(TCARGrid)
    End Sub

    Private Sub DoneBtn_Click(sender As Object, e As RoutedEventArgs)
        Dim panel = TaskPaneHelper.GetTaskPane()
        panel?.MarkCheckboxAsDone("Btn_D")
        _handler.HandleCloseClick(_hostForm)
    End Sub

End Class
