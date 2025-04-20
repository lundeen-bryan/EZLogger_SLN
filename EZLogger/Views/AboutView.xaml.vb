Imports System.Windows
Imports System.Windows.Controls
Imports System.IO
Imports System.Text.Json
Imports EZLogger.Helpers
Imports System.Windows.Forms
Imports System.Windows.Media.Imaging
Imports MessageBox = System.Windows.MessageBox

Namespace EZLogger.Views

    Partial Public Class AboutView
        Inherits Windows.Controls.UserControl

        Private ReadOnly _hostForm As Form
        Private ReadOnly configFilePath As String = ConfigHelper.GetGlobalConfigPath()

        Public Sub New(Optional hostForm As Form = Nothing)
            InitializeComponent()
            _hostForm = hostForm
            LoadAboutInfo()

            AddHandler BtnHelp.Click, AddressOf BtnHelp_Click
            AddHandler BtnGoBack.Click, AddressOf BtnGoBack_Click
        End Sub

        ''' <summary>
        ''' Loads the version info section from the global config.
        ''' </summary>
        Private Sub LoadAboutInfo()
            Try
                Dim jsonText As String = File.ReadAllText(configFilePath)
                Dim doc As JsonDocument = JsonDocument.Parse(jsonText)

                Dim versionElement As JsonElement = doc.RootElement.GetProperty("version")

                TxtCreatedBy.Text = versionElement.GetProperty("created_by").GetString()
                TxtSupportContact.Text = versionElement.GetProperty("support_email").GetString()
                TxtLastUpdate.Text = versionElement.GetProperty("date").GetString()
                TxtVersion.Text = versionElement.GetProperty("number").GetString()
                TxtLatestChange.Text = versionElement.GetProperty("instructions").GetString()

            Catch ex As Exception
                MessageBox.Show("Failed to load About information: " & ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error)
            End Try
        End Sub

        Private Sub BtnHelp_Click(sender As Object, e As RoutedEventArgs)
            MessageBox.Show("Help file not yet available.", "Coming Soon", MessageBoxButton.OK, MessageBoxImage.Information)
        End Sub

        Private Sub BtnGoBack_Click(sender As Object, e As RoutedEventArgs)
            _hostForm.Close()
        End Sub
    End Class

End Namespace
