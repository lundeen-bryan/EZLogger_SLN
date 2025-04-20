Imports System.IO
Imports System.Windows
Imports System.Windows.Controls
Imports EZLogger.Helpers
Imports EZLogger.Handlers
Imports MessageBox = System.Windows.MessageBox

Public Class ConfigView
    Inherits UserControl

    Private ReadOnly _handler As ConfigViewHandler

    Public Sub New(hostForm As Forms.Form)
        InitializeComponent()

        ' Set up the handler
        _handler = New ConfigViewHandler()

        ' Wire up buttons
        AddHandler BtnCreateConfig.Click, AddressOf BtnCreateConfig_Click
        AddHandler BtnSaveDoctorsList.Click, AddressOf BtnSaveDoctorsList_Click
        AddHandler BtnSaveConfig.Click, AddressOf BtnSaveConfig_Click
        AddHandler AddAlertButton.Click, AddressOf AddAlertButton_Click
        AddHandler EditAlertButton.Click, AddressOf EditAlertButton_Click
        AddHandler DeleteAlertButton.Click, AddressOf DeleteAlertButton_Click
        AddHandler AddCountyAlertButton.Click, AddressOf AddCountyAlertButton_Click
        AddHandler EditCountyAlertButton.Click, AddressOf EditCountyAlertButton_Click
        AddHandler DeleteCountyAlertButton.Click, AddressOf DeleteCountyAlertButton_Click
        AddHandler EditEmail.Click, AddressOf BtnEditEmail_Click
    End Sub

    ' Move "Loaded" logic here
    Private Sub ConfigView_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        ' Load doctor list (this already works)
        TextBoxDoctors.Text = String.Join(Environment.NewLine, ListHelper.GetDoctorList())

        ' Step 1: Build expected path to local config file
        Dim localConfigPath As String = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
            ".ezlogger\local_user_config.json"
        )

        ' Step 2: If local config file is missing, prompt user to create it
        If Not File.Exists(localConfigPath) Then
            txtblock_local_config.Text = "No config found. Please click the [C] button to create one."
            txtblock_global_config.Text = "No global config path available."
            Return
        End If

        ' Step 3: Show local config file path
        txtblock_local_config.Text = localConfigPath

        ' Step 4: Load global config file path from inside the local config
        Dim globalConfigPath As String = ConfigHelper.GetGlobalConfigPath()

        If String.IsNullOrEmpty(globalConfigPath) Then
            txtblock_global_config.Text = "Global config path not set. Please click [C] to configure it."
        Else
            txtblock_global_config.Text = globalConfigPath
        End If
    End Sub

    Private Sub BtnCreateConfig_Click(sender As Object, e As RoutedEventArgs)
        _handler.HandleCreateConfigClick()
    End Sub

    Private Sub BtnSaveConfig_Click(sender As Object, e As RoutedEventArgs)
        _handler.HandleSaveConfigClick()
    End Sub

    Private Sub AddAlertButton_Click(sender As Object, e As RoutedEventArgs)
        _handler.AddAlertButtonClick()
    End Sub

    Private Sub EditAlertButton_Click(sender As Object, e As RoutedEventArgs)
        _handler.EditAlertButtonClick()
    End Sub

    Private Sub DeleteAlertButton_Click(sender As Object, e As RoutedEventArgs)
        _handler.DeleteAlertButtonClick()
    End Sub

    Private Sub AddCountyAlertButton_Click(sender As Object, e As RoutedEventArgs)
        _handler.AddCountyAlertButtonClick()
    End Sub

    Private Sub EditCountyAlertButton_Click(sender As Object, e As RoutedEventArgs)
        _handler.EditCountyAlertButtonClick()
    End Sub

    Private Sub DeleteCountyAlertButton_Click(sender As Object, e As RoutedEventArgs)
        _handler.DeleteCountyAlertButtonClick()
    End Sub

    Private Sub BtnEditEmail_Click(sender As Object, e As RoutedEventArgs)
        _handler.HandleEditEmailClick(radio_secretaries, radio_friday, radio_competent)
    End Sub

    Private Sub BtnSaveDoctorsList_Click(sender As Object, e As RoutedEventArgs)
        Dim filePath As String = ListHelper.GetDoctorListFilePath()
        File.WriteAllText(filePath, TextBoxDoctors.Text)
        MessageBox.Show("Doctor list saved.")
    End Sub
End Class
