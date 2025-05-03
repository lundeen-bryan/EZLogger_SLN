Imports EZLogger.Helpers
Imports UserControl = System.Windows.Controls.UserControl
Imports System.Windows
Imports System.Windows.Forms

Public Class SendEmailView
    Inherits UserControl

    Private ReadOnly _handler As New SendEmailHandler()
    Private ReadOnly _hostForm As Form

    Public Sub New(Optional hostForm As Form = Nothing)
        InitializeComponent()
        _hostForm = hostForm
        WireUpButtons()
    End Sub

    Private Sub WireUpButtons()
        AddHandler SelectFileBtn.Click, AddressOf SelectFileBtn_Click
        AddHandler SendBtn.Click, AddressOf SendBtn_Click
        AddHandler Me.Loaded, AddressOf SendEmailView_Loaded
    End Sub
    Private Sub SendEmailView_Loaded(sender As Object, e As RoutedEventArgs)
        Try
            TextBoxLastname.Text = DocumentPropertyHelper.GetPropertyValue("LastName", caseInsensitive:=True)
            TextBoxFirstname.Text = DocumentPropertyHelper.GetPropertyValue("FirstName", caseInsensitive:=True)
        Catch ex As Exception
            MsgBoxHelper.Show("Error loading patient name from document properties: " & ex.Message)
        End Try
    End Sub

    Private Sub SelectFileBtn_Click(sender As Object, e As RoutedEventArgs)
        Me.TextBoxFilename.Text = _handler.HandleSelectFileClick(_hostForm)
    End Sub

    Private Sub SendBtn_Click(sender As Object, e As RoutedEventArgs)
        Dim Lname As String = Me.TextBoxLastname.Text
        Dim Fname As String = Me.TextBoxFirstname.Text
        Dim Filename As String = TextBoxFilename.Text
        Dim ReportType As String = GetSelectedReportType()
        _handler.HandleSendClick(filename:=Filename, lastname:=Lname, firstname:=Fname, reportType:=ReportType, hostForm:=_hostForm)
    End Sub
    Private Function GetSelectedReportType() As String
        If Radio1370b.IsChecked Then Return Radio1370b.Tag.ToString()
        If Radio1370c.IsChecked Then Return Radio1370c.Tag.ToString()
        If Radio1372a.IsChecked Then Return Radio1372a.Tag.ToString()
        Return ""
    End Function

End Class