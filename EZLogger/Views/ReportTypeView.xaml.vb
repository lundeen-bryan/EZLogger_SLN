Imports System.Windows
Imports EZLogger.Handlers
Imports EZLogger.Helpers
Imports System.Collections.Generic

Partial Public Class ReportTypeView
    Inherits System.Windows.Controls.UserControl

    Public Property InitialSelectedReportType As String
    Private Const usDate As String = "MM/dd/yyyy"
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
    End Sub

    Private Sub ReportTypeView_Loaded(sender As Object, e As RoutedEventArgs)
        ' Read commitment date directly from Word custom property
        Dim commitmentDateString As String = DocumentPropertyManager.ReadCustomProperty("Commitment")
        Dim commitmentDate As DateTime

        If DateTime.TryParse(commitmentDateString, commitmentDate) Then
            CommitmentLbl.Content = commitmentDate.ToString(usDate)
            FirstPprDue.Content = commitmentDate.AddMonths(6).ToString(usDate)
        End If

        ' Load report type list
        Dim reportTypes As List(Of String) = rthandler.GetReportTypes()
        ReportTypeViewCbo.ItemsSource = reportTypes

        If Not String.IsNullOrEmpty(InitialSelectedReportType) AndAlso reportTypes.Contains(InitialSelectedReportType) Then
            ReportTypeViewCbo.SelectedItem = InitialSelectedReportType
        End If
    End Sub
End Class
