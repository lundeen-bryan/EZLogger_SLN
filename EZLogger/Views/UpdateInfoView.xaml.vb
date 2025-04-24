' Namespace=EZLogger/Views
' Filename=UpdateInfoView.xaml.vb
' !See Label Footer for notes

Imports System.Windows
Imports System.Windows.Controls
Imports EZLogger.Handlers
Imports EZLogger.Helpers
Imports System.Windows.Forms

Public Class UpdateInfoView

    Private ReadOnly _handler As New PatientInfoHandler()
    Private ReadOnly _hostForm As Form
    Public Property InitialPropertyName As String = ""
    Public Property InitialPropertyValue As String = ""

    Public Sub New(Optional hostForm As Form = Nothing)
        InitializeComponent()
        _hostForm = hostForm
        _handler = New PatientInfoHandler()
        WireUpButtons()
    End Sub

    Public Sub WireUpButtons()
        AddHandler Me.Loaded, AddressOf UpdateInfoView_Loaded
        AddHandler BtnSaveProperty.Click, AddressOf BtnSaveProperty_Click
        AddHandler BtnCalendar.Click, AddressOf BtnCalendar_Click
        AddHandler HiddenDatePicker.SelectedDateChanged, AddressOf HiddenDatePicker_SelectedDateChanged
        AddHandler BtnGenerateId.Click, AddressOf BtnGenerateId_Click
        AddHandler DoneBtn.Click, AddressOf DoneBtn_Click
    End Sub

    Private Sub UpdateInfoView_Loaded(sender As Object, e As RoutedEventArgs)
        TxbxPropertyName.Text = InitialPropertyName
        TxtbxPropertyValue.Text = InitialPropertyValue
    End Sub


    Private Sub BtnSaveProperty_Click(sender As Object, e As RoutedEventArgs)
        _handler.HandleSavePropertyClick(Me)
    End Sub

    Private Sub BtnCalendar_Click(sender As Object, e As RoutedEventArgs)
        ' Simulate a popup by temporarily making it visible
        HiddenDatePicker.Visibility = Visibility.Visible
        HiddenDatePicker.IsDropDownOpen = True
    End Sub

    Private Sub HiddenDatePicker_SelectedDateChanged(sender As Object, e As SelectionChangedEventArgs)
        If HiddenDatePicker.SelectedDate.HasValue Then
            TxtbxPropertyValue.Text = HiddenDatePicker.SelectedDate.Value.ToString("MM/dd/yyyy")
        End If

        HiddenDatePicker.Visibility = Visibility.Collapsed
    End Sub

    Private Sub BtnGenerateId_Click(sender As Object, e As RoutedEventArgs)
        Dim uniqueId As String = DocumentPropertyHelper.CreateUniqueIdFromProperties()

        If Not String.IsNullOrWhiteSpace(uniqueId) Then
            TxtbxPropertyValue.Text = uniqueId
        Else
            MsgBoxHelper.Show("Could not generate ID. Make sure required properties are filled in.")
        End If
    End Sub

    Private Sub DoneBtn_Click(sender As Object, e As RoutedEventArgs)
        'If _hostForm IsNot Nothing Then _hostForm.Close()
        _handler.HandleCloseClick(_hostForm)
    End Sub

End Class

''Footer:
''===========================================================================================
'' Procedure: ......... UpdateInfoView.xaml.vb/
'' Description: ....... Updates info from the Patient Info form
'' Version: ........... 1.0.0 - major.minor.patch
'' Created: ........... 2025-04-23
'' Updated: ........... 2025-04-23
'' Installs to: ....... EZLogger/Views
'' Compatibility: ..... Word, VSTO
'' Contact Author: .... lundeen-bryan
'' Copyright:  ........ n/a ©2025. All rights reserved.
'' Notes: ............. _
' (1) See Wiki article: 📌  Getting data from one view to another.md 📝 🗑️
''===========================================================================================