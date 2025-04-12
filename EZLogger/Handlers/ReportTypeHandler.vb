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
            ' Set the commitment date label
            If Not String.IsNullOrWhiteSpace(commitmentDate) Then
                Dim parsedDate As Date
                If Date.TryParse(commitmentDate, parsedDate) Then
                    reportTypeView.LabelCommitmentDate.Content = parsedDate.ToString("MM/dd/yyyy")

                    ' Now set LabelFirstDueDate to 6 months later
                    Dim firstDueDate As Date = parsedDate.AddMonths(6)
                    reportTypeView.LabelFirstDueDate.Content = firstDueDate.ToString("MM/dd/yyyy")
                Else
                    reportTypeView.LabelCommitmentDate.Content = commitmentDate
                    reportTypeView.LabelFirstDueDate.Content = "(Invalid Date)"
                End If
            Else
                reportTypeView.LabelCommitmentDate.Content = "(Missing)"
                reportTypeView.LabelFirstDueDate.Content = "(Unavailable)"
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

        Public Sub PopulateDueDates(view As ReportTypeView)
            Dim commitmentDateText As String = view.LabelCommitmentDate.Content?.ToString()
            Dim commitmentDate As Date
            If Date.TryParse(commitmentDateText, commitmentDate) Then
                Dim ninetyDayDate As Date = commitmentDate.AddDays(90)
                view.LabelNinetyDay.Content = ninetyDayDate.ToString("MM/dd/yyyy")
                Dim ninemo As Date = ninetyDayDate.AddMonths(6)
                view.LabelNineMonth.Content = ninemo.ToString("MM/dd/yyyy")
                Dim fifteenmo As Date = ninemo.AddMonths(6)
                view.LabelFifteenMonth.Content = fifteenmo.ToString("MM/dd/yyyy")
                Dim twentyonemo As Date = ninemo.AddMonths(12)
                view.LabelTwentyOneMonth.Content = twentyonemo.ToString("MM/dd/yyyy")
            End If
        End Sub
    End Class

End Namespace

