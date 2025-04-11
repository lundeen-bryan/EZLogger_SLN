Imports System.Windows.Forms
Imports EZLogger.Helpers

Namespace Handlers

    Public Class ReportTypeHandler

        ' ✅ Called when the Confirm Type button is clicked
        '    Shows the host form, passes in the selected report type, waits for user to finish,
        '    then returns the selected value from the new form
        Public Function OnConfirmReportTypeButtonClick(commitmentDate As String) As String

            Dim host As New ReportTypeHost()

            ' Get the WPF view hosted inside the ElementHost
            Dim reportTypeView = CType(host.ElementHost1.Child, ReportTypeView)

            ' Set the commitment date label
            If Not String.IsNullOrWhiteSpace(commitmentDate) Then
                Dim parsedDate As Date
                If Date.TryParse(commitmentDate, parsedDate) Then
                    reportTypeView.LabelCommitmentDate.Content = parsedDate.ToString("MM/dd/yyyy")
                Else
                    reportTypeView.LabelCommitmentDate.Content = commitmentDate
                End If
            End If

            ' Show the modal host form
            host.TopMost = True
            host.StartPosition = FormStartPosition.CenterScreen
            host.ShowDialog()

            ' Return the selected report type from the ComboBox
            Return reportTypeView.ReportTypeViewCbo.SelectedItem?.ToString()

        End Function

        Public Sub HandleSelectedReportType(report_type As String)
            If String.IsNullOrWhiteSpace(report_type) Then
                MsgBox("Please select a  report type before confirming.")
            Else
                MsgBox("You selected report type: " & report_type)
            End If
        End Sub

        Public Function GetReportTypes() As List(Of String)
            Return ConfigPathHelper.GetReportTypeList()
        End Function

    End Class

End Namespace

