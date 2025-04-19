Imports System.Windows.Forms

Namespace Handlers
    Public Class DueDates1370Handler ' Example: ConfigViewHandler

        Public Sub HandleGoBackClick(hostForm As Form)
            ' Re-open the ReportTypeView
            Dim reportTypeHandler As New ReportTypeHandler()
            reportTypeHandler.LaunchReportTypeView("")

            ' Close the current form
            hostForm?.Close()
        End Sub

    End Class
End Namespace