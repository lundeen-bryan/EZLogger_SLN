Imports System.Windows

Namespace EZLogger.Handlers

	Public Class ReportTypeHandler
		Public Sub OnConfirmReportTypeButtonClick(reportType As String)
			MessageBox.Show($"You confirmed the report type: {reportType}", "Report Type Click")
		End Sub
	End Class
End Namespace

