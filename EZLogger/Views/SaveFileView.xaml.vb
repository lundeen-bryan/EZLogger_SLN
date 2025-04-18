Imports System.Windows
Imports EZLogger.Helpers
Imports EZLogger.Handlers
Imports System.Windows.Forms

Public Class SaveFileView
    Inherits Controls.UserControl

    Private ReadOnly _handler As New SaveFileHandler()
    Private ReadOnly _hostForm As Form
    Private ReadOnly rthandler As New ReportTypeHandler()

    Public Sub New(Optional hostForm As Form = Nothing)
        InitializeComponent()
        _hostForm = hostForm
        WireUpButtons()
    End Sub

    Private Sub WireUpButtons()
        AddHandler BtnDone.Click, AddressOf Btn_Close_Click
        AddHandler BtnSearchPatientId.Click, AddressOf BtnSearchPatientId_Click
    End Sub
    Private Sub BtnSearchPatientId_Click(sender As Object, e As RoutedEventArgs) Handles BtnSearchPatientId.Click
        _handler.HandleSearchPatientIdClick(Me)
    End Sub

    Private Sub SaveFileView_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        Dim reportTypes As List(Of String) = rthandler.GetReportTypes()
        ReportTypeCbo.ItemsSource = reportTypes
    End Sub
    Private Sub Btn_Close_Click(sender As Object, e As RoutedEventArgs)
        _handler.HandleCloseClick(_hostForm)
    End Sub

End Class