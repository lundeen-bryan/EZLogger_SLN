' Namespace=EZLogger/Views
' Filename=SaveFileView.xaml.vb
' !See Label Footer for notes

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
        AddHandler Me.Loaded, AddressOf SaveFileView_Loaded
        AddHandler DoneBtn.Click, AddressOf DoneBtn_Click
        AddHandler SearchPatientIdBtn.Click, AddressOf SearchPatientIdBtn_Click
    End Sub
    Private Sub SearchPatientIdBtn_Click(sender As Object, e As RoutedEventArgs)
        _handler.HandleSearchPatientIdClick(Me)
    End Sub

    Private Sub SaveFileView_Loaded(sender As Object, e As RoutedEventArgs)
        Dim reportTypes As List(Of String) = rthandler.GetReportTypes()
        ReportTypeCbo.ItemsSource = reportTypes
    End Sub

    Private Sub DoneBtn_Click(sender As Object, e As RoutedEventArgs)
        Dim panel = TaskPaneHelper.GetTaskPane()
        panel?.MarkCheckboxAsDone("Btn_I")
        _handler.HandleCloseClick(_hostForm)
    End Sub

End Class

' Footer:
''===========================================================================================
'' Procedure: ......... SaveFileView.xaml.vb/
'' Description: ....... Saves copies of files with proper naming convention and deletes old
'' Version: ........... 1.0.0 - major.minor.patch
'' Created: ........... 2025-04-23
'' Updated: ........... 2025-04-23
'' Module URL: ........ weburl
'' Installs to: ....... EZLogger/Views
'' Compatibility: ..... Word VSTO
'' Contact Author: .... lundeen-bryan
'' Copyright:  ........ n/a ©2025. All rights reserved.
'' Notes: ............. _
'  (1) notes_here
''===========================================================================================