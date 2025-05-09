﻿Imports EZLogger.Handlers
Imports EZLogger.Helpers
Imports UserControl = System.Windows.Controls.UserControl
Imports System.Windows
Imports System.Windows.Forms
Imports MessageBox = System.Windows.MessageBox

Partial Public Class AboutView
    Inherits UserControl

    Private ReadOnly _hostForm As Form
    Private ReadOnly configFilePath As String = ConfigHelper.GetGlobalConfigPath()
    Private ReadOnly _handler As New AboutWinHandler()

    Public Sub New(Optional hostForm As Form = Nothing)
        InitializeComponent()
        _hostForm = hostForm
        LoadAboutInfo()
        WireUpButtons()
    End Sub

    Private Sub WireUpButtons()
        AddHandler BtnHelp.Click, AddressOf BtnHelp_Click
        AddHandler BtnGoBack.Click, AddressOf BtnGoBack_Click
        AddHandler ConfigBtn.Click, AddressOf ConfigBtn_Click
        AddHandler Me.Loaded, AddressOf OnViewLoaded
    End Sub

    Private Sub OnViewLoaded(sender As Object, e As RoutedEventArgs)
        LoadAboutInfo()
    End Sub

    Private Sub ConfigBtn_Click(sender As Object, e As RoutedEventArgs)
        Dim hostform As New ConfigHost()
        ' TODO find way to show ConfigView and bring to the front
        _hostForm.Hide()
        hostform.Show()
        hostform.BringToFront()
        hostform.Activate()
    End Sub

    ''' <summary>
    ''' Loads version information from the global config using AboutViewHandler.
    ''' Displays the data in UI text fields or shows an error message if loading fails.
    ''' </summary>
    Private Sub LoadAboutInfo()
        Dim result = _handler.LoadAboutInfo(ConfigHelper.GetGlobalConfigPath())

        If result.HasError Then
            MessageBox.Show(result.ErrorMessage, "Error", MessageBoxButton.OK, MessageBoxImage.Error)
            Return
        End If

        TxtCreatedBy.Text = result.CreatedBy
        TxtSupportContact.Text = result.SupportEmail
        TxtLastUpdate.Text = result.LastUpdate
        TxtVersion.Text = result.VersionNumber
        TxtLatestChange.Text = result.LatestChange
    End Sub

    Private Sub BtnHelp_Click(sender As Object, e As RoutedEventArgs)
        MessageBox.Show("Help file not yet available.", "Coming Soon", MessageBoxButton.OK, MessageBoxImage.Information)
    End Sub

    Private Sub BtnGoBack_Click(sender As Object, e As RoutedEventArgs)
        _hostForm.Close()
    End Sub

    Private Sub BtnGoBack_Click_1(sender As Object, e As RoutedEventArgs) Handles BtnGoBack.Click
        _hostForm?.Close()
    End Sub
End Class
