Imports System.Windows
Imports System.Windows.Controls
Imports System.IO
Imports System.Text.Json
Imports EZLogger.Helpers
Imports System.Windows.Forms
Imports System.Windows.Media.Imaging
Imports MessageBox = System.Windows.MessageBox
Imports stdole
Imports EZLogger.Handlers

Namespace EZLogger.Views

    Partial Public Class AboutView
        Inherits Windows.Controls.UserControl

        Private ReadOnly _hostForm As Form
        Private ReadOnly configFilePath As String = ConfigHelper.GetGlobalConfigPath()
        Private ReadOnly _handler As New AboutWinHandler()

        Public Sub New(Optional hostForm As Form = Nothing)
            InitializeComponent()
            _hostForm = hostForm
            LoadAboutInfo()

            AddHandler BtnHelp.Click, AddressOf BtnHelp_Click
            AddHandler BtnGoBack.Click, AddressOf BtnGoBack_Click
            AddHandler Me.Loaded, AddressOf OnViewLoaded
        End Sub
        Private Sub OnViewLoaded(sender As Object, e As RoutedEventArgs)
            LoadAboutInfo()
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
    End Class

End Namespace
