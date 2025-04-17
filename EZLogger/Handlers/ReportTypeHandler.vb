Imports System.Windows
Imports System.Windows.Forms
Imports EZLogger.Helpers
Imports Application = Microsoft.Office.Interop.Word.Application
Imports Word = Microsoft.Office.Interop.Word



Namespace Handlers

    Public Class ReportTypeHandler

        ''' <summary>
        ''' Handles the confirmation of the report type selection.
        ''' </summary>
        ''' <param name="commitmentDate">The commitment date as a string, which is used to populate the view.</param>
        ''' <returns>The selected report type as a string, or null if no selection is made.</returns>
        ''' <remarks>
        ''' This function initializes a modal form to display report type options, sets the commitment date label,
        ''' and returns the selected report type from the ComboBox. If the commitment date is invalid or missing,
        ''' appropriate labels are updated to reflect this.
        ''' </remarks>
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
            Return ConfigHelper.GetListFromGlobalConfig("listbox", "report_type")
        End Function

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

        ''' <summary>
        ''' Called when BtnAcceptPPR is clicked. Calculates days until/since due date,
        ''' updates the label, and writes the number to a Word custom property.
        ''' </summary>
        ''' <param name="view">The ReportTypeView instance with the controls.</param>
        Public Sub HandleAcceptPPR(view As ReportTypeView)
            ' Ensure a date is selected
            If Not view.PickCurrentDueDate.SelectedDate.HasValue Then
                Windows.MessageBox.Show("Select the current due date", "Missing Due Date", MessageBoxButton.OK, MessageBoxImage.Warning)
                Exit Sub
            End If

            ' Calculate day difference
            Dim dueDate As Date = view.PickCurrentDueDate.SelectedDate.Value.Date
            Dim today As Date = Date.Today
            Dim daysDifference As Integer = (dueDate - today).Days

            ' Update the label
            view.LabelDaysSinceDueDate.Content = daysDifference.ToString()

            ' Get the active document
            Dim app As Word.Application = Globals.ThisAddIn.Application
            Dim doc As Word.Document = TryCast(app.ActiveDocument, Word.Document)

            If doc Is Nothing Then
                Windows.MessageBox.Show("No active document found.", "EZLogger", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Exit Sub
            End If

            ' Write to custom document property
            DocumentPropertyHelper.WriteCustomProperty(doc, "Days Since Due", daysDifference.ToString())
        End Sub
        Public Sub HandleAcceptIstDueDate(view As ReportTypeView)
            ' Get active Word document
            Dim doc As Word.Document = TryCast(Globals.ThisAddIn.Application.ActiveDocument, Word.Document)
            If doc Is Nothing Then
                MsgBoxHelper.Show("No active Word document found.")
                Exit Sub
            End If

            ' Determine which radio button is selected
            Dim selectedLabel As System.Windows.Controls.Label = Nothing
            Dim reportCycle As String = Nothing

            If view.RadioA.IsChecked Then
                selectedLabel = view.LabelNinetyDay
                reportCycle = view.RadioA.Tag?.ToString()
            ElseIf view.RadioB.IsChecked Then
                selectedLabel = view.LabelNineMonth
                reportCycle = view.RadioB.Tag?.ToString()
            ElseIf view.RadioC.IsChecked Then
                selectedLabel = view.LabelFifteenMonth
                reportCycle = view.RadioC.Tag?.ToString()
            ElseIf view.RadioD.IsChecked Then
                selectedLabel = view.LabelTwentyOneMonth
                reportCycle = view.RadioD.Tag?.ToString()
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
            view.PickCurrentDueDate.SelectedDate = currentDueDate

            ' Determine next label date (if available)
            Dim nextLabelDate As Date = currentDueDate.AddMonths(6) ' fallback default

            Dim nextLabelText As String = Nothing
            If selectedLabel Is view.LabelNinetyDay Then
                nextLabelText = view.LabelNineMonth.Content?.ToString()
            ElseIf selectedLabel Is view.LabelNineMonth Then
                nextLabelText = view.LabelFifteenMonth.Content?.ToString()
            ElseIf selectedLabel Is view.LabelFifteenMonth Then
                nextLabelText = view.LabelTwentyOneMonth.Content?.ToString()
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
            view.PickNextDueDate.SelectedDate = nextLabelDate

            ' Write all document properties
            DocumentPropertyHelper.WriteCustomProperty(doc, "Due Date", currentDueDate.ToString("MM/dd/yyyy"))
            DocumentPropertyHelper.WriteCustomProperty(doc, "Next Due", nextLabelDate.ToString("MM/dd/yyyy"))

            ' Write Report Date (from CurrentReportDate picker)
            If view.CurrentReportDate.SelectedDate.HasValue Then
                Dim reportDate As Date = view.CurrentReportDate.SelectedDate.Value
                DocumentPropertyHelper.WriteCustomProperty(doc, "Report Date", reportDate.ToString("MM/dd/yyyy"))
            End If

            ' Calculate and write Rush Status and Days Since Due
            RushStatusHelper.SetRushStatusAndDaysSinceDue(view)

            MsgBoxHelper.Show("Report cycle and due dates have been saved.")
        End Sub

    End Class

End Namespace

