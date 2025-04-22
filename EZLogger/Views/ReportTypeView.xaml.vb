Imports System.Windows
Imports EZLogger.Helpers
Imports EZLogger.Handlers
Imports System.Windows.Forms

Public Class ReportTypeView
    Inherits Controls.UserControl

    Private ReadOnly _handler As New ReportTypeHandler()
    Private ReadOnly _hostForm As Form

    Public Sub New(Optional hostForm As Form = Nothing)
        InitializeComponent()
        _hostForm = hostForm
        WireUpButtons()
    End Sub

    Private Sub WireUpButtons()
        AddHandler Me.Loaded, AddressOf ReportTypeView_Loaded
        AddHandler ReportTypeSelectedBtn.Click, AddressOf ReportTypeSelectedBtn_Click
        AddHandler DoneBtn.Click, AddressOf DoneBtn_Click
    End Sub

    Private Sub ReportTypeView_Loaded(sender As Object, e As RoutedEventArgs)
        Dim reportTypes As List(Of String) = _handler.GetReportTypes()
        ReportTypeCbo.ItemsSource = reportTypes
    End Sub

    Private Sub ReportTypeSelectedBtn_Click(sender As Object, e As RoutedEventArgs)
        Dim selectedReportType As String = TryCast(ReportTypeCbo.SelectedItem, String)
        Dim selectedReportDate As String = GetSelectedReportDate()

        _handler.ReportTypeSelectedBtnClick(selectedReportType, selectedReportDate, _hostForm)
    End Sub

    Public Function GetSelectedReportDate() As String
        If CurrentReportDate.SelectedDate.HasValue Then
            Return CurrentReportDate.SelectedDate.Value.ToString("MM/dd/yyyy")
        Else
            Return String.Empty
        End If
    End Function

    Private Sub DoneBtn_Click(sender As Object, e As RoutedEventArgs)
        Dim panel = TaskPaneHelper.GetTaskPane()
        panel?.MarkCheckboxAsDone("Btn_C")
        _handler.HandleCloseClick(_hostForm)
    End Sub

End Class