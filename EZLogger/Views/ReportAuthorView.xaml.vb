Imports System.Windows
Imports System.Windows.Controls
Imports System.Windows.Forms
Imports EZLogger.Handlers
Imports EZLogger.Helpers
Imports EZLogger.HostForms

Namespace EZLogger.Views

    Partial Public Class ReportAuthorView
        Inherits System.Windows.Controls.UserControl

        Private ReadOnly _hostForm As Form
        Private ReadOnly _handler As AuthorHandler

        ' Design-time support constructor
        Public Sub New()
            Me.New(Nothing)
        End Sub

        ' Runtime constructor
        Public Sub New(Optional hostForm As Form = Nothing)
            InitializeComponent()

            _hostForm = hostForm
            _handler = New AuthorHandler()

            AddHandler BtnAddAuthor.Click, AddressOf BtnAddAuthor_Click
            AddHandler BtnAuthorFirstPage.Click, AddressOf BtnAuthorFirstPage_Click
            AddHandler BtnAuthorLastPage.Click, AddressOf BtnAuthorLastPage_Click
            AddHandler BtnAuthorDone.Click, AddressOf BtnAuthorDone_Click
            AddHandler BtnClose.Click, AddressOf BtnClose_Click
            AddHandler Me.Loaded, AddressOf ReportAuthorView_Loaded
        End Sub

        Private Sub BtnAddAuthor_Click(sender As Object, e As RoutedEventArgs)
            _handler.HandleAddAuthorClick()
        End Sub

        Private Sub BtnAuthorFirstPage_Click(sender As Object, e As RoutedEventArgs)
            _handler.HandleFirstPageClick()
        End Sub

        Private Sub BtnAuthorLastPage_Click(sender As Object, e As RoutedEventArgs)
            _handler.HandleLastPageClick()
        End Sub

        Private Sub BtnAuthorDone_Click(sender As Object, e As RoutedEventArgs)
            _handler.HandleDoneSelectingClick()
        End Sub

        Private Sub BtnClose_Click(sender As Object, e As RoutedEventArgs)
            _handler.HandleCloseClick(_hostForm)
        End Sub
        Private Sub ReportAuthorView_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
            Dim doctors As List(Of String) = ConfigPathHelper.GetDoctorList()
            CboAuthor.ItemsSource = doctors
        End Sub

    End Class

End Namespace
