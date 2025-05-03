Imports EZLogger.Handlers
Imports System.Windows
Imports System.Windows.Controls
Imports MessageBox = System.Windows.MessageBox

Public Class ConfigView
    Inherits UserControl

    Private ReadOnly _handler As ConfigViewHandler

    Public Sub New(hostForm As Forms.Form)
        InitializeComponent()

        ' Set up the handler
        _handler = New ConfigViewHandler()
        WireUpButtons()
    End Sub

    Private Sub WireUpButtons()
        ' Wire up buttons
        AddHandler BtnCreateConfig.Click, AddressOf BtnCreateConfig_Click
        AddHandler BtnSaveDoctorsList.Click, AddressOf BtnSaveDoctorsList_Click
        AddHandler BtnSaveConfig.Click, AddressOf BtnSaveConfig_Click
        AddHandler AddAlertButton.Click, AddressOf AddAlertButton_Click
        AddHandler DeleteAlertBtn.Click, AddressOf DeleteAlertBtn_Click
        AddHandler AddCountyAlertButton.Click, AddressOf AddCountyAlertButton_Click
        AddHandler DeleteCountyAlertBtn.Click, AddressOf DeleteCountyAlertBtn_Click
        AddHandler EditEmail.Click, AddressOf BtnEditEmail_Click
        AddHandler DeleteAlertBtn.Click, AddressOf DeleteAlertBtn_Click
    End Sub

    ' Move "Loaded" logic here
    Private Sub ConfigView_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        Dim result = _handler.HandleViewLoaded()

        TextBoxDoctors.Text = String.Join(Environment.NewLine, result.DoctorList)
        txtblock_local_config.Text = result.LocalConfigPath
        txtblock_global_config.Text = result.GlobalConfigPathMessage
        ForensicDatabaseTxtBlk.Text = result.ForensicDatabasePath
        ForensicLibraryPathTxtBlk.Text = result.ForensicLibraryPath
        ForensicOfficePathTxtBlk.Text = result.ForensicOfficePath
        Dim alertList As List(Of String) = _handler.LoadPatientAlerts()
        AlertsListBox.ItemsSource = alertList
        Dim countyAlertList As List(Of String) = _handler.LoadCountyAlerts()
        CountyAlertsListBox.ItemsSource = countyAlertList

        If countyAlertList.Count = 0 Then
            CountyAlertsListBox.ItemsSource = New List(Of String) From {"(No county alerts configured)"}
        End If

    End Sub

    Private Sub BtnCreateConfig_Click(sender As Object, e As RoutedEventArgs)
        _handler.HandleCreateConfigClick()
    End Sub

    Private Sub BtnSaveConfig_Click(sender As Object, e As RoutedEventArgs)
        _handler.HandleSaveConfigClick()
    End Sub

    Private Sub DeleteAlertBtn_Click(sender As Object, e As RoutedEventArgs)
        Dim selected As String = CType(AlertsListBox.SelectedItem, String)
        If String.IsNullOrWhiteSpace(selected) Then
            MessageBox.Show("Please select a patient alert to delete.")
            Return
        End If

        Dim key = selected.Split("="c)(0).Trim()

        Dim result = MessageBox.Show(
        $"Are you sure you want to delete the alert for patient: {key}?",
        "Confirm Delete",
        MessageBoxButton.YesNo,
        MessageBoxImage.Warning)

        If result = MessageBoxResult.Yes Then
            _handler.DeletePatientAlertByKey(key)
            AlertsListBox.ItemsSource = _handler.LoadPatientAlerts()
        End If
    End Sub

    Private Sub DeleteCountyAlertBtn_Click(sender As Object, e As RoutedEventArgs)
        Dim selected As String = CType(CountyAlertsListBox.SelectedItem, String)
        If String.IsNullOrWhiteSpace(selected) Then
            MessageBox.Show("Please select a county alert to delete.")
            Return
        End If

        Dim key = selected.Split("="c)(0).Trim()

        Dim result = MessageBox.Show(
        $"Are you sure you want to delete the alert for county: {key}?",
        "Confirm Delete",
        MessageBoxButton.YesNo,
        MessageBoxImage.Warning)

        If result = MessageBoxResult.Yes Then
            _handler.DeleteCountyAlertByKey(key)
            CountyAlertsListBox.ItemsSource = _handler.LoadCountyAlerts()
        End If
    End Sub

    Private Sub BtnEditEmail_Click(sender As Object, e As RoutedEventArgs)
        _handler.HandleEditEmailClick(radio_secretaries, radio_friday, radio_competent)
    End Sub

    Private Sub BtnSaveDoctorsList_Click(sender As Object, e As RoutedEventArgs)
        _handler.SaveDoctorsList(TextBoxDoctors.Text)
    End Sub

    Private Sub AddAlertButton_Click(sender As Object, e As RoutedEventArgs)
        _handler.AddAlertButtonClick()
        AlertsListBox.ItemsSource = _handler.LoadPatientAlerts()
    End Sub

    Private Sub AddCountyAlertButton_Click(sender As Object, e As RoutedEventArgs)
        _handler.AddCountyAlertButtonClick()
        CountyAlertsListBox.ItemsSource = _handler.LoadCountyAlerts()
    End Sub

End Class
