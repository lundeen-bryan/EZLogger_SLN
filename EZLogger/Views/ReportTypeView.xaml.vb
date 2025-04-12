' Top of the file
Imports EZLogger.Handlers
Imports System.Collections.Generic
Imports System.Windows

Public Class ReportTypeView

    Public Property InitialSelectedReportType As String
    Private ReadOnly _handler As ReportTypeHandler
    Private ReadOnly rthandler As ReportTypeHandler

    Public Sub New(Optional hostForm As FormatException = Nothing)
        InitializeComponent()

        _handler = New ReportTypeHandler()
        rthandler = New ReportTypeHandler()

        AddHandler BtnSelectedType.Click, AddressOf BtnSelectedType_Click
        AddHandler Me.Loaded, AddressOf ReportTypeView_Loaded
    End Sub

    Private Sub BtnSelectedType_Click(sender As Object, e As RoutedEventArgs)
        Dim selectedType As String = TryCast(ReportTypeViewCbo.SelectedItem, String)
        _handler.HandleSelectedReportType(selectedType)
        _handler.PopulateDueDates(Me)
    End Sub

    Private Sub ReportTypeView_Loaded(sender As Object, e As RoutedEventArgs)
        LabelEarly90.Visibility = Visibility.Collapsed

        Dim reportTypes As List(Of String) = rthandler.GetReportTypes()
        ReportTypeViewCbo.ItemsSource = reportTypes

        If Not String.IsNullOrEmpty(InitialSelectedReportType) AndAlso reportTypes.Contains(InitialSelectedReportType) Then
            ReportTypeViewCbo.SelectedItem = InitialSelectedReportType
        End If

        ' Ask the handler if early 90-day flag is present
        If rthandler.HasEarlyNinetyDayFlag() Then
            LabelEarly90.Visibility = Visibility.Visible
        End If
    End Sub

End Class