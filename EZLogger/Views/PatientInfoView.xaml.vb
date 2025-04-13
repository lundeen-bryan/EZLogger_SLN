Imports System.Windows
Imports System.Windows.Forms
Imports EZLogger.Handlers

Namespace EZLogger.Views

	Partial Public Class PatientInfoView
		Inherits System.Windows.Controls.UserControl

		Private ReadOnly _handler As PatientInfoHandler
		Private ReadOnly _hostForm As Form

		Public Sub New(Optional hostForm As Form = Nothing)
			InitializeComponent()

			_hostForm = hostForm
			_handler = New PatientInfoHandler()

			' AddHandler examples
			AddHandler BtnClose.Click, AddressOf BtnClose_Click
			AddHandler Me.Loaded, AddressOf PatientInfoView_Loaded
		End Sub

		' Example button click
		Private Sub BtnClose_Click(sender As Object, e As RoutedEventArgs)
			_handler.HandleCloseClick(_hostForm)
		End Sub
		Private Sub PatientInfoView_Loaded(sender As Object, e As RoutedEventArgs)
			_handler.LoadCustomDocProperties(Me)
		End Sub

	End Class
End Namespace