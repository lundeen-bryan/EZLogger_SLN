Imports System.Windows
Imports System.Windows.Controls
Imports System.Windows.Forms ' Needed for Form
Imports EZLogger.Helpers
Imports EZLogger.Handlers

Public Class FaxCoverView
    Inherits System.Windows.Controls.UserControl

    Private ReadOnly _handler As FaxCoverHandler
    Private ReadOnly _hostForm As Form ' ✅ Store the host WinForm

    ' ✅ Modified constructor to accept optional host
    Public Sub New(Optional hostForm As Form = Nothing)
        InitializeComponent()

        _hostForm = hostForm
        _handler = New FaxCoverHandler()

        ' Load combo data when the view is created
        AddHandler Me.Loaded, AddressOf FaxCoverView_Loaded

        ' Wire up buttons
    End Sub

    Private Sub FaxCoverView_Loaded(sender As Object, e As RoutedEventArgs)
        Dim coverPages As List(Of String) = ConfigHelper.GetListFromGlobalConfig("listbox", "cover_pages")
        ListBoxCoverPages.Items.Clear()
        ListBoxCoverPages.ItemsSource = coverPages
    End Sub
End Class
