Imports System.Windows
Imports System.Windows.Controls
Imports EZLogger.Handlers
Imports EZLogger.Helpers
Imports System.Windows.Forms

Public Class UpdateInfoView

    Private ReadOnly _handler As New PatientInfoHandler()
    Private ReadOnly _hostForm As Form

    Public Sub New(Optional hostForm As Form = Nothing)
        InitializeComponent()
        _hostForm = hostForm
    End Sub
    Private Sub BtnSaveProperty_Click(sender As Object, e As RoutedEventArgs) Handles BtnSaveProperty.Click
        _handler.HandleSavePropertyClick(Me)
    End Sub
    Private Sub BtnCalendar_Click(sender As Object, e As RoutedEventArgs) Handles BtnCalendar.Click
        ' Simulate a popup by temporarily making it visible
        HiddenDatePicker.Visibility = Visibility.Visible
        HiddenDatePicker.IsDropDownOpen = True
    End Sub
    Private Sub HiddenDatePicker_SelectedDateChanged(sender As Object, e As SelectionChangedEventArgs) Handles HiddenDatePicker.SelectedDateChanged
        If HiddenDatePicker.SelectedDate.HasValue Then
            TxtbxPropertyValue.Text = HiddenDatePicker.SelectedDate.Value.ToString("MM/dd/yyyy")
        End If

        HiddenDatePicker.Visibility = Visibility.Collapsed
    End Sub
    Private Sub BtnGenerateId_Click(sender As Object, e As RoutedEventArgs) Handles BtnGenerateId.Click
        Dim uniqueId As String = DocumentPropertyHelper.CreateUniqueIdFromProperties()

        If Not String.IsNullOrWhiteSpace(uniqueId) Then
            TxtbxPropertyValue.Text = uniqueId
        Else
            MsgBoxHelper.Show("Could not generate ID. Make sure required properties are filled in.")
        End If
    End Sub
    Private Sub BtnClose_Click(sender As Object, e As RoutedEventArgs) Handles BtnClose.Click
        'If _hostForm IsNot Nothing Then _hostForm.Close()
        _handler.HandleCloseClick(_hostForm)
    End Sub

End Class
