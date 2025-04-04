Imports System.Windows.Forms

Namespace EZLogger.Handlers

    Public Class ReportTypeHandler

        ' ✅ Called when the Confirm Type button is clicked
        '    Shows the host form, passes in the selected report type, waits for user to finish,
        '    then returns the selected value from the new form
        Public Function OnConfirmReportTypeButtonClick(reportType As String) As String
            Dim host As New ReportTypeHost()

            ' Set the selected item on the WPF control inside the ElementHost
            Dim reportTypeView = CType(host.ElementHost1.Child, ReportTypeView)
            reportTypeView.InitialSelectedReportType = reportType

            host.StartPosition = FormStartPosition.CenterScreen

            ' Show the form modally
            host.ShowDialog()

            ' Return the selected value (or Nothing if user closed without selecting)
            Return reportTypeView.ReportTypeViewCbo.SelectedItem?.ToString()
        End Function

        ' ✅ Shared function to return the list of report types
        Public Function GetReportTypes() As List(Of String)
            Return New List(Of String) From {
                "1370(b)(1)",
                "1372(a)(1)",
                "UNLIKELY 1370(b)(1)",
                "PPR"
            }
        End Function

    End Class

End Namespace

