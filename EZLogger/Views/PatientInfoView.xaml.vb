Imports System.Windows
Imports System.Windows.Forms
Imports EZLogger.Handlers

Namespace EZLogger.Views

	Partial Public Class PatientInfoView
		Inherits System.Windows.Controls.UserControl

		Private ReadOnly _handler As PatientInfoHandler
		Private ReadOnly _hostForm As New Form()

		Public Event CloseButtonClicked(sender As Object, e As EventArgs)

		Public Sub New(Optional hostForm As Form = Nothing)
			InitializeComponent()

			_handler = New PatientInfoHandler()

			' Hook up all button events to dedicated subroutines
			AddHandler Me.Loaded, AddressOf PatientInfoView_Loaded
			AddHandler BtnDelete.Click, AddressOf BtnDelete_Click
			AddHandler BtnDeleteAll.Click, AddressOf BtnDeleteAll_Click
			AddHandler BtnFirstPage.Click, AddressOf BtnFirstPage_Click
			AddHandler BtnLastPage.Click, AddressOf BtnLastPage_Click
			AddHandler BtnAddEdit.Click, AddressOf BtnAddEdit_Click
			AddHandler BtnRefresh.Click, AddressOf BtnRefresh_Click
		End Sub

		Private Sub PatientInfoView_Loaded(sender As Object, e As RoutedEventArgs)
			_handler.LoadCustomDocProperties(Me)
		End Sub

		Private Sub BtnDelete_Click(sender As Object, e As RoutedEventArgs)
			_handler.HandleDeleteClick(Me)
		End Sub

		Private Sub BtnDeleteAll_Click(sender As Object, e As RoutedEventArgs)
			_handler.HandleDeleteAllClick(Me)
		End Sub
		Private Sub BtnRefresh_Click(sender As Object, e As RoutedEventArgs)
			_handler.LoadCustomDocProperties(Me)
		End Sub
		Private Sub BtnFirstPage_Click(sender As Object, e As RoutedEventArgs)
			_handler.HandleFirstPageClick()
		End Sub

		Private Sub BtnLastPage_Click(sender As Object, e As RoutedEventArgs)
			_handler.HandleLastPageClick()
		End Sub
		Private Sub BtnAddEdit_Click(sender As Object, e As RoutedEventArgs)
			_handler.HandleAddEditClick(Me)
		End Sub

	End Class

End Namespace
