' Namespace=EZLogger/Handlers
' Filename=DueDates1370Handler.vb
' !See Label Footer for notes

Imports Microsoft.Office.Interop.Word
Imports EZLogger.Helpers
Imports System.Windows.Forms

Namespace Handlers
    Public Class DueDates1370Handler

        ''' <summary>
        ''' Processes the user's selection of the first due date for a 1370 report.
        ''' </summary>
        ''' <param name="view">The <see cref="DueDates1370View"/> containing UI elements and selections.</param>
        ''' <remarks>
        ''' Performs the following steps:
        ''' 1. Retrieves the active <c>Microsoft.Office.Interop.Word.Document</c>.
        ''' 2. Identifies the selected due date cycle based on the selected radio button.
        ''' 3. Writes the selected report cycle to a custom document property.
        ''' 4. Parses and validates the selected due date.
        ''' 5. Calculates the next due date based on predefined intervals.
        ''' 6. Writes both current and next due dates to document properties.
        ''' 7. Displays a confirmation message to the user.
        ''' </remarks>
        Public Sub HandleAcceptIstDueDate(view As DueDates1370View)
            Dim success As Boolean = True
            Dim doc As Microsoft.Office.Interop.Word.Document = DocumentHelper.GetActiveWordDocument()

            Dim selectedLabel As System.Windows.Controls.Label = Nothing
            Dim reportCycle As String = Nothing

            ' === Step 1: Determine selected report cycle ===
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
                success = False
            End If

            ' === Step 2: Parse selected label into a valid date ===
            Dim currentDueDate As Date
            If success Then
                If Not Date.TryParse(selectedLabel.Content?.ToString(), currentDueDate) Then
                    MsgBoxHelper.Show("The selected due date is invalid or missing.")
                    success = False
                End If
            End If

            ' === Step 3: Determine next label date (optional) ===
            Dim nextDueDate As Date = currentDueDate.AddMonths(6)
            If success Then
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
                        If parsedNextLabel < nextDueDate Then
                            nextDueDate = parsedNextLabel
                        End If
                    End If
                End If
            End If

            ' === Step 4: Write document properties and notify user ===
            If success Then
                If Not String.IsNullOrWhiteSpace(reportCycle) Then
                    DocumentPropertyHelper.WriteCustomProperty(doc, "Report Cycle", reportCycle)
                End If

                DocumentPropertyHelper.WriteCustomProperty(doc, "Due Date", currentDueDate.ToString("MM/dd/yyyy"))
                DocumentPropertyHelper.WriteCustomProperty(doc, "Next Due", nextDueDate.ToString("MM/dd/yyyy"))
                MsgBoxHelper.Show("Report cycle and due dates have been saved.")
            End If
        End Sub

        ''' <summary>
        ''' Handles the "Go Back" action, returning to the previous view and closing the current form.
        ''' </summary>
        ''' <param name="hostForm">The Form instance representing the current view to be closed.</param>
        ''' <remarks>
        ''' This function performs the following actions:
        ''' 1. Attempts to retrieve the "Commitment" custom property from the active Word document.
        ''' 2. Launches the ReportTypeView with the retrieved commitment value.
        ''' 3. Closes the current form (hostForm).
        ''' If no active document is found or the "Commitment" property is not available, an empty string is used.
        ''' </remarks>
        Public Sub HandleGoBackClick(hostForm As Form)
            Dim wordDoc As Word.Document = DocumentHelper.GetActiveWordDocument()
            Dim vstoDoc As Microsoft.Office.Tools.Word.Document = Globals.Factory.GetVstoObject(wordDoc)
            Dim commitmentRaw As String = ""

            If vstoDoc IsNot Nothing Then
                Try
                    'commitmentRaw = doc.CustomDocumentProperties("Commitment").Value.ToString()
                    commitmentRaw = DocumentPropertyHelper.GetPropertyValue(wordDoc, "Commitment")
                Catch ex As Exception
                    commitmentRaw = ""
                End Try
            End If

            Dim reportTypeHandler As New ReportTypeHandler()
            reportTypeHandler.LaunchReportTypeView(commitmentRaw)

            ' Close the current form
            hostForm?.Close()
        End Sub

        ''' <summary>
        ''' Handles the saving of the 1370 choice selected by the user.
        ''' This function processes the selected due date cycle, updates document properties,
        ''' and calculates the next due date based on the user's selection.
        ''' </summary>
        ''' <param name="view">The DueDates1370View instance containing the UI elements and user selections.</param>
        ''' <remarks>
        ''' This function performs the following actions:
        ''' 1. Retrieves the active Word document.
        ''' 2. Determines the selected due date cycle from radio buttons.
        ''' 3. Writes the Report Cycle to document properties.
        ''' 4. Parses and validates the current due date.
        ''' 5. Updates Rush Status and Days Since Due.
        ''' 6. Calculates the next due date based on the selected cycle.
        ''' 7. Writes current and next due dates to document properties.
        ''' 8. Closes the current view and returns to the previous screen.
        ''' </remarks>
        Public Sub HandleSave1370ChoiceClick(view As DueDates1370View)
            ' TODO: use handler instead of this code
            ' Get the active Word document
            Dim wordDoc As Word.Document = DocumentHelper.GetActiveWordDocument()
            Dim vstoDoc As Microsoft.Office.Tools.Word.Document = Globals.Factory.GetVstoObject(wordDoc)

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
                DocumentPropertyHelper.WriteCustomProperty(wordDoc, "Report Cycle", reportCycle)
            End If

            ' Parse current due date from selected label
            Dim currentDueDate As Date
            If Not Date.TryParse(selectedLabel.Content?.ToString(), currentDueDate) Then
                MsgBoxHelper.Show("Invalid or missing current due date.")
                Exit Sub
            End If

            ' Write Rush Status and Days Since Due to doc properties
            RushStatusHelper.SetRushStatusAndDaysSinceDue(currentDueDate)

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
            DocumentPropertyHelper.WriteCustomProperty(wordDoc, "Due Date", currentDueDate.ToString("MM/dd/yyyy"))
            DocumentPropertyHelper.WriteCustomProperty(wordDoc, "Next Due", nextDueDate.ToString("MM/dd/yyyy"))

            ' (Rush Status helper to be added later)

            ' Notify logic complete (no MsgBox per your request)
            HandleGoBackClick(view.HostForm)
        End Sub

        ''' <summary>
        ''' Populates due dates and related labels in the provided ReportTypeView based on values stored in 
        ''' the active Word document properties.
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

            Dim early90str As String = doc.CustomDocumentProperties("Early90Day").Value.ToString
            Dim early90datestr As String = doc.CustomDocumentProperties("Early90Date").Value.ToString

            If Not Date.TryParse(commitmentDateText, parsedDate) Then
                Exit Sub ' If no valid date, just stop
            End If

            Dim parsedEarlyDate As Date
            Dim early90DayValue As Integer = 0
            Dim baseDate As Date = parsedDate
            Dim ninetyDayDate As Date

            If classification = "PC1370" Then
                ' Fill extended date labels
                If Integer.TryParse(early90str, early90DayValue) Then
                    If early90DayValue = 1 AndAlso Date.TryParse(early90datestr, parsedEarlyDate) Then
                        ninetyDayDate = parsedEarlyDate
                    Else
                        ninetyDayDate = baseDate.AddDays(90)
                    End If
                End If

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

            End If
        End Sub


    End Class
End Namespace

' Footer:
''===========================================================================================
'' Filename: .......... DueDates1370Handler.vb
'' Description: ....... manages the logic for calculating, displaying and saving 1370 report due dates
'' Created: ........... 2025-05-02
'' Updated: ........... 2026-04-23
'' Installs to: ....... EZLogger/Handlers
'' Compatibility: ..... VSTO, WPF
'' Contact Author: .... lundeen-bryan
'' Copyright:  ........ ©2025. All rights reserved.
'' Notes: ............. _
' (1) Method Index _
' - HandleAcceptIstDueDate(view As DueDates1370View): Saves the selected
'   report cycle and calculates the next due date based on the first due
'   date.
' - HandleGoBackClick(hostForm As Form): Closes the current form and
'   reopens the ReportTypeView using the Commitment document property.
' - HandleSave1370ChoiceClick(view As DueDates1370View): Saves the
'   selected 1370 report cycle and due dates, updates rush status, and
'   returns to the previous screen.
' - PopulateDueDates(view As DueDates1370View): Populates 1370 due date
'   labels based on the Classification and Commitment date in the active
'   Word document.
' (2) Refactored to use Early90 and Early90Date custom document properties
'   to calculate successive due dates. If 90-Day was early, this affects
'   subsequent due dates and it will now show that to the user. 
''===========================================================================================