Imports System.Windows
Imports System.Windows.Controls

Namespace Handlers
	Public Class CoverPageHandler

		Public Sub OnFileSaveHostClick()
			Dim host As New CoverPageHost()
			host.Show()
		End Sub

		' Add your method here to handle a button click:
		Public Sub OnBtnConvertClick()
			MsgBox("You clicked [Describe the action or button here]")
		End Sub
	End Class
End Namespace