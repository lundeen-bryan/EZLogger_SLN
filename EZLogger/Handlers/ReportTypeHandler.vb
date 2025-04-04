Imports System.Windows
Imports System.Windows.Forms

Namespace EZLogger.Handlers

	Public Class ReportTypeHandler
		Public Sub OnConfirmReportTypeButtonClick(reportType As String)
			Dim host As New ReportTypeHost()
			host.StartPosition = FormStartPosition.CenterScreen
			host.Show()
		End Sub
	End Class
End Namespace

