Imports System.Windows
Imports System.Windows.Controls
Imports EZLogger.Handlers

Public Class TCARListView
    Inherits UserControl

    Public Sub New()
        InitializeComponent()

        _hostForm = hostForm
        _handler = New TCARListHandler(Globals.ThisAddIn.Application)
        WireUpButtons()
    End Sub

    Public Sub WireUpButtons()
        AddHandler Me.Loaded, AddressOf TCARListView_Loaded
        AddHandler DoneBtn.Click, AddressOf DoneBtn_Click
    End Sub

    Private Sub TCARListView_Loaded(sender As Object, e As RoutedEventArgs)
        ' Load TCAR records from the handler
        Dim records As List(Of TCARRecord) = TCARListHandler.LoadAllActive()

        ' Set the DataGrid's item source
        TCARGrid.ItemsSource = records
    End Sub

    ' This method is wired up in XAML and simply delegates to the handler
    Private Sub BtnSelectPatient_Click(sender As Object, e As RoutedEventArgs)
        TCARListHandler.HandleTCARSelect(TCARGrid)
    End Sub

    Private Sub DoneBtn_Click(sender As Object, e As RoutedEventArgs)
        Dim panel = TaskPaneHelper.GetTaskPane()
        panel?.MarkCheckboxAsDone("Btn_D")
        _handler.HandleCloseClick(_hostForm)
    End Sub

End Class
