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

    Private Sub ReportTypeView_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        Dim reportTypes As List(Of String) = _handler.GetReportTypes()
        ReportTypeCbo.ItemsSource = reportTypes
    End Sub

    Private Sub WireUpButtons()
        'AddHandler Btn_Close.Click, AddressOf Btn_Close_Click
        AddHandler ReportTypeSelectedBtn.Click, AddressOf ReportTypeSelectedBtn_Click
    End Sub

    Private Sub ReportTypeSelectedBtn_Click(sender As Object, e As RoutedEventArgs)
        Dim selectedReportType As String = TryCast(ReportTypeCbo.SelectedItem, String)
        _handler.ReportTypeSelectedBtnClick(selectedReportType, _hostForm)
    End Sub

End Class