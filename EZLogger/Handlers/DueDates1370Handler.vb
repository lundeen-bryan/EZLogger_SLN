Imports System.Windows.Forms
Imports EZLogger.Helpers
Imports Application = Microsoft.Office.Interop.Word.Application
Imports Word = Microsoft.Office.Interop.Word

Namespace Handlers
    Public Class DueDates1370Handler ' Example: ConfigViewHandler

        Public Sub HandleGoBackClick(hostForm As Form)
            ' Re-open the ReportTypeView
            Dim reportTypeHandler As New ReportTypeHandler()
            reportTypeHandler.LaunchReportTypeView("")

            ' Close the current form
            hostForm?.Close()
        End Sub

        Public Sub HandleAcceptIstDueDate(view As DueDates1370View)
            ' Get active Word document
            Dim doc As Word.Document = TryCast(Globals.ThisAddIn.Application.ActiveDocument, Word.Document)
            If doc Is Nothing Then
                MsgBoxHelper.Show("No active Word document found.")
                Exit Sub
            End If

            ' Determine which radio button is selected
            Dim selectedLabel As System.Windows.Controls.Label = Nothing
            Dim reportCycle As String = Nothing

            If view.NinetyDayRdo.IsChecked Then
                selectedLabel = view.NinetyDayLbl
                reportCycle = view.NinetyDayRdo.Tag?.ToString()
            ElseIf view.NineMoRdo.IsChecked Then
                selectedLabel = view.NineMoLbl
                reportCycle = view.NineMoRdo.Tag?.ToString()
            ElseIf view.FifteenMoRdo.IsChecked Then
                selectedLabel = view.FifteenMoLbl
                reportCycle = view.FifteenMoRdo.Tag?.ToString()
            ElseIf view.TwentyOneMoRdo.IsChecked Then
                selectedLabel = view.TwentyOneMoLbl
                reportCycle = view.TwentyOneMoRdo.Tag?.ToString()
            Else
                MsgBoxHelper.Show("You must select a due date cycle before continuing.")
                Exit Sub
            End If

            ' Set Report Cycle property
            If Not String.IsNullOrWhiteSpace(reportCycle) Then
                DocumentPropertyHelper.WriteCustomProperty(doc, "Report Cycle", reportCycle)
            End If

            ' Parse selected label content as current due date
            Dim currentDueDate As Date
            If Not Date.TryParse(selectedLabel.Content?.ToString(), currentDueDate) Then
                MsgBoxHelper.Show("The selected due date is invalid or missing.")
                Exit Sub
            End If

            ' Set CurrentDueDatePicker with this value
            'view.PickCurrentDueDate.SelectedDate = currentDueDate

            ' Determine next label date (if available)
            Dim nextLabelDate As Date = currentDueDate.AddMonths(6) ' fallback default

            Dim nextLabelText As String = Nothing
            If selectedLabel Is view.NinetyDayLbl Then
                nextLabelText = view.NineMoLbl.Content?.ToString()
            ElseIf selectedLabel Is view.NineMoLbl Then
                nextLabelText = view.FifteenMoLbl.Content?.ToString()
            ElseIf selectedLabel Is view.FifteenMoLbl Then
                nextLabelText = view.TwentyOneMoLbl.Content?.ToString()
            End If

            If Not String.IsNullOrWhiteSpace(nextLabelText) Then
                Dim parsedNextLabel As Date
                If Date.TryParse(nextLabelText, parsedNextLabel) Then
                    ' Choose whichever is earlier: nextLabel OR +6 months
                    If parsedNextLabel < nextLabelDate Then
                        nextLabelDate = parsedNextLabel
                    End If
                End If
            End If

            ' Set NextDueDatePicker
            ''''view.PickNextDueDate.SelectedDate = nextLabelDate

            ' Write all document properties
            DocumentPropertyHelper.WriteCustomProperty(doc, "Due Date", currentDueDate.ToString("MM/dd/yyyy"))
            DocumentPropertyHelper.WriteCustomProperty(doc, "Next Due", nextLabelDate.ToString("MM/dd/yyyy"))

            ' Write Report Date (from CurrentReportDate picker)
            'If view.CurrentReportDate.SelectedDate.HasValue Then
            '    Dim reportDate As Date = view.CurrentReportDate.SelectedDate.Value
            '    DocumentPropertyHelper.WriteCustomProperty(doc, "Report Date", reportDate.ToString("MM/dd/yyyy"))
            'End If

            ' Calculate and write Rush Status and Days Since Due
            'RushStatusHelper.SetRushStatusAndDaysSinceDue(view)

            MsgBoxHelper.Show("Report cycle and due dates have been saved.")
        End Sub

        ''' <summary>
        ''' Populates due dates and related labels in the provided ReportTypeView based on the active Word document.
        ''' </summary>
        ''' <param name="view">The ReportTypeView instance containing the controls to update.</param>
        ''' <remarks>
        ''' The function is triggered when the user selects the button that says I selected the report type
        ''' This function retrieves custom properties from the active Word document, such as "Classification" and "Expiration",
        ''' and uses them to calculate and populate various due dates. If the classification is "PC1370", extended due dates
        ''' are calculated. Otherwise, standard due dates are determined based on the commitment date.
        ''' If no active document is found or required properties are missing, appropriate labels are updated to reflect this.
        ''' </remarks>

        Public Sub PopulateDueDates(view As DueDates1370View)
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
                    view.MaxDateLbl.Content = expirationDate.ToString("MM/dd/yyyy")
                Else
                    view.MaxDateLbl.Content = expirationRaw ' fallback to raw value
                End If
            Catch ex As Exception
                view.MaxDateLbl.Content = "(Unavailable)"
            End Try

            Dim commitmentDateText As String = view.CommitmentDateLbl.Content?.ToString()
            Dim parsedDate As Date

            If Not Date.TryParse(commitmentDateText, parsedDate) Then
                Exit Sub ' If no valid date, just stop
            End If

            'If view.ReportTypeViewCbo.SelectedValue = "" Then
            '    Exit Sub ' No report type selected
            'End If

            If classification = "PC1370" Then
                ' Fill extended date labels
                Dim ninetyDayDate As Date = parsedDate.AddDays(90)
                view.NinetyDayLbl.Content = ninetyDayDate.ToString("MM/dd/yyyy")

                Dim ninemo As Date = ninetyDayDate.AddMonths(6)
                view.NineMoLbl.Content = ninemo.ToString("MM/dd/yyyy")

                Dim fifteenmo As Date = ninemo.AddMonths(6)
                view.FifteenMoLbl.Content = fifteenmo.ToString("MM/dd/yyyy")

                Dim twentyonemo As Date = ninemo.AddMonths(12)
                view.TwentyOneMoLbl.Content = twentyonemo.ToString("MM/dd/yyyy")
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
                'view.LabelCommitmentDate2.Content = parsedDate.ToString("MM/dd/yyyy")
                'view.LabelFirstDueDate.Content = firstDueFromCommitment.ToString("MM/dd/yyyy")

                '' Set date pickers
                'view.PickCurrentDueDate.SelectedDate = currentDueDate
                'view.PickNextDueDate.SelectedDate = nextDueDate
            End If
        End Sub

        Public Sub HandleSave1370ChoiceClick(view As DueDates1370View)
            ' Get the active Word document
            Dim doc As Word.Document = TryCast(Globals.ThisAddIn.Application.ActiveDocument, Word.Document)
            If doc Is Nothing Then
                MsgBoxHelper.Show("No active Word document found.")
                Exit Sub
            End If

            ' Determine which radio button is selected and map to its corresponding label
            Dim selectedLabel As System.Windows.Controls.Label = Nothing
            Dim reportCycle As String = Nothing

            If view.NinetyDayRdo.IsChecked Then
                selectedLabel = view.NinetyDayLbl
                reportCycle = view.NinetyDayRdo.Tag?.ToString()
            ElseIf view.NineMoRdo.IsChecked Then
                selectedLabel = view.NineMoLbl
                reportCycle = view.NineMoRdo.Tag?.ToString()
            ElseIf view.FifteenMoRdo.IsChecked Then
                selectedLabel = view.FifteenMoLbl
                reportCycle = view.FifteenMoRdo.Tag?.ToString()
            ElseIf view.TwentyOneMoRdo.IsChecked Then
                selectedLabel = view.TwentyOneMoLbl
                reportCycle = view.TwentyOneMoRdo.Tag?.ToString()
            Else
                MsgBoxHelper.Show("You must select a due date cycle before saving.")
                Exit Sub
            End If

            ' Write Report Cycle to document properties
            If Not String.IsNullOrWhiteSpace(reportCycle) Then
                DocumentPropertyHelper.WriteCustomProperty(doc, "Report Cycle", reportCycle)
            End If

            ' Parse current due date from selected label
            Dim currentDueDate As Date
            If Not Date.TryParse(selectedLabel.Content?.ToString(), currentDueDate) Then
                MsgBoxHelper.Show("Invalid or missing current due date.")
                Exit Sub
            End If

            ' Determine the next due date
            Dim nextDueDate As Date = currentDueDate ' default to same date (for 21-month case)
            If selectedLabel Is view.NinetyDayLbl Then
                Date.TryParse(view.NineMoLbl.Content?.ToString(), nextDueDate)
            ElseIf selectedLabel Is view.NineMoLbl Then
                Date.TryParse(view.FifteenMoLbl.Content?.ToString(), nextDueDate)
            ElseIf selectedLabel Is view.FifteenMoLbl Then
                Date.TryParse(view.TwentyOneMoLbl.Content?.ToString(), nextDueDate)
            End If
            ' If TwentyOneMoLbl is selected, nextDueDate remains the same as currentDueDate

            ' Write due dates to document properties
            DocumentPropertyHelper.WriteCustomProperty(doc, "Due Date", currentDueDate.ToString("MM/dd/yyyy"))
            DocumentPropertyHelper.WriteCustomProperty(doc, "Next Due", nextDueDate.ToString("MM/dd/yyyy"))

            ' Calculate days since due (can be negative if due date is in the future)
            Dim daysSinceDue As Integer = (currentDueDate - Date.Today).Days
            DocumentPropertyHelper.WriteCustomProperty(doc, "Days Since Due", daysSinceDue.ToString())

            ' (Rush Status helper to be added later)

            ' Notify logic complete (no MsgBox per your request)
            HandleGoBackClick(view.HostForm)
        End Sub


    End Class
End Namespace