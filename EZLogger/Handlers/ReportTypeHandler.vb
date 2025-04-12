Imports System.Windows.Forms
Imports EZLogger.Helpers
Imports Application = Microsoft.Office.Interop.Word.Application
Imports Word = Microsoft.Office.Interop.Word



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
                    reportTypeView.LabelFirstDueDate.Content = "(Invalid Date)"
                End If
            Else
                reportTypeView.LabelCommitmentDate.Content = "(Missing)"
                reportTypeView.LabelFirstDueDate.Content = "(Unavailable)"
            End If

            ' Show the modal host form
            host.TopMost = True
            FormPositionHelper.MoveFormToTopLeftOfAllScreens(host, 10, 10)
            host.Show()

            ' Return the selected report type from the ComboBox
            Return reportTypeView.ReportTypeViewCbo.SelectedItem?.ToString()

        End Function

        Public Sub HandleSelectedReportType(report_type As String)
            If String.IsNullOrWhiteSpace(report_type) Then
                MsgBoxHelper.Show("Please select a  report type before confirming.")
            End If
        End Sub

        Public Function GetReportTypes() As List(Of String)
            Return ConfigPathHelper.GetReportTypeList()
        End Function

        Public Sub PopulateDueDates(view As ReportTypeView)
            Dim app As Word.Application = Globals.ThisAddIn.Application
            Dim doc As Word.Document = TryCast(app.ActiveDocument, Word.Document)

            If doc Is Nothing Then
                System.Windows.Forms.MessageBox.Show("No active document.", "EZLogger", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Exit Sub
            End If

            ' Try to read Classification property
            Dim classification As String = ""
            Try
                classification = doc.CustomDocumentProperties("Classification").Value.ToString()
            Catch ex As Exception
                classification = ""
            End Try

            ' Try to read "Expiration" custom property and set LabelMaxDate in MM/dd/yyyy format
            Try
                Dim expirationRaw As String = doc.CustomDocumentProperties("Expiration").Value.ToString()
                Dim expirationDate As Date

                If Date.TryParse(expirationRaw, expirationDate) Then
                    view.LabelMaxDate.Content = expirationDate.ToString("MM/dd/yyyy")
                Else
                    view.LabelMaxDate.Content = expirationRaw ' fallback to raw value
                End If
            Catch ex As Exception
                view.LabelMaxDate.Content = "(Unavailable)"
            End Try

            Dim commitmentDateText As String = view.LabelCommitmentDate.Content?.ToString()
            Dim parsedDate As Date

            If Not Date.TryParse(commitmentDateText, parsedDate) Then
                Exit Sub ' If no valid date, just stop
            End If

            If view.ReportTypeViewCbo.SelectedValue = "" Then
                Exit Sub ' No report type selected
            End If

            If classification = "PC1370" Then
                ' Fill extended date labels
                Dim ninetyDayDate As Date = parsedDate.AddDays(90)
                view.LabelNinetyDay.Content = ninetyDayDate.ToString("MM/dd/yyyy")

                Dim ninemo As Date = ninetyDayDate.AddMonths(6)
                view.LabelNineMonth.Content = ninemo.ToString("MM/dd/yyyy")

                Dim fifteenmo As Date = ninemo.AddMonths(6)
                view.LabelFifteenMonth.Content = fifteenmo.ToString("MM/dd/yyyy")

                Dim twentyonemo As Date = ninemo.AddMonths(12)
                view.LabelTwentyOneMonth.Content = twentyonemo.ToString("MM/dd/yyyy")
            Else
                ' Get today's year
                Dim currentYear As Integer = Date.Today.Year

                ' Build Current Due Date using current year and commitment month/day
                Dim currentDueDate As Date
                Try
                    currentDueDate = New Date(currentYear, parsedDate.Month, parsedDate.Day)
                Catch ex As ArgumentOutOfRangeException
                    ' Handles Feb 29 in a non-leap year by shifting to March 1
                    currentDueDate = New Date(currentYear, parsedDate.Month, 1).AddMonths(1)
                End Try

                ' Next Due Date is 6 months after the current due date
                Dim nextDueDate As Date = currentDueDate.AddMonths(6)

                ' LabelFirstDueDate is always 6 months after original commitment date
                Dim firstDueFromCommitment As Date = parsedDate.AddMonths(6)

                ' Set labels
                view.LabelCommitmentDate2.Content = parsedDate.ToString("MM/dd/yyyy")
                view.LabelFirstDueDate.Content = firstDueFromCommitment.ToString("MM/dd/yyyy")

                ' Set date pickers
                view.PickCurrentDueDate.SelectedDate = currentDueDate
                view.PickNextDueDate.SelectedDate = nextDueDate
            End If
        End Sub

        ''' <summary>
        ''' Checks if the active Word document has early_ninety_day = 1
        ''' </summary>
        ''' <returns>True if early_ninety_day is set to 1, otherwise False</returns>
        Public Function HasEarlyNinetyDayFlag() As Boolean
            Try
                Dim app As Word.Application = Globals.ThisAddIn.Application
                Dim doc As Word.Document = TryCast(app.ActiveDocument, Word.Document)

                If doc Is Nothing Then Return False

                Dim value As Object = doc.CustomDocumentProperties("Early90Day").Value
                Return value IsNot Nothing AndAlso value.ToString() = "1"
            Catch ex As Exception
                ' Property not found or other error; assume not flagged
                Return False
            End Try
        End Function

    End Class

End Namespace

