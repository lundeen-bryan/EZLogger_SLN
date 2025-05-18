' Namespace=EZLogger/Handlers
' Filename=ReportTypeHandler.vb
' !See Label Footer for notes

Imports EZLogger.Helpers
Imports System.Windows
Imports System.Windows.Forms

Namespace Handlers

    Public Class ReportTypeHandler

        '''<summary>
        '''Retreieves a list of available report types from the global_config.json file
        '''</summary>
        '''<returns> A list of report type strings for use in Comboboxes or other UI elements</returns>
        '''<remarks>
        '''This function loads report types from the report_type key
        '''inside the listbox section of the global_config.json file
        '''</remarks>
        Public Function GetReportTypes() As List(Of String)
            Return ListHelper.GetListFromGlobalConfig("listbox", "report_type")
        End Function

        '''<summary>
        '''Closes the ReportTypeView form when the Done button is clicked in ReportTypeView
        '''</summary>
        '''<param name="form">The instance of the form to be closed (ReportTypeView)</param>
        '''<remarks>
        '''This function is called when the user presses DoneBtn. In that function the
        '''checkbox is checked before calling this function and closing the view window.
        '''</remarks>
        Public Sub HandleCloseClick(hostForm As Form)
            hostForm?.Close()
        End Sub

        '''<summary>
        ''' Saves the selected report date to the current active Word Document's custom properties.
        '''</summary>
        '''<param name="reportDate">The selected report type from the combobox</param>
        '''<remarks>
        ''' This funciton checks if the report date is non-empty then
        ''' confirms the presense of an active Word Document, and then
        ''' writes the value to a custom property named "Report Date"
        '''</remarks>
        Public Sub HandleSelectedReportDate(reportDate As String)
            If String.IsNullOrWhiteSpace(reportDate) Then
                MsgBoxHelper.Show("Please select a report date before confirming.")
            Else
                Dim doc As Word.Document = DocumentHelper.GetActiveWordDocument()
                If doc IsNot Nothing Then
                    DocumentPropertyHelper.WriteCustomProperty(doc, "Report Date", reportDate)
                Else
                    MsgBoxHelper.Show("No active Word document found.")
                End If
            End If
        End Sub

        '''<summary>
        ''' Saves the selected report type to the current active Word Document's custom properties.
        '''</summary>
        '''<param name="reportType">The selected report type from the combobox</param>
        '''<remarks>
        ''' This funciton checks if the report type is non-empty then
        ''' confirms the presense of an active Word Document, and then
        ''' writes the value to a custom property named "Report Type"
        '''</remarks>
        Public Sub HandleSelectedReportType(reportType As String)
            If String.IsNullOrWhiteSpace(reportType) Then
                MsgBoxHelper.Show("Please select a  report type before confirming.")
            Else
                ' Write the selected report type to the custom property
                Dim doc As Word.Document = DocumentHelper.GetActiveWordDocument()
                If doc IsNot Nothing Then
                    DocumentPropertyHelper.WriteCustomProperty(doc, "Report Type", reportType)
                Else
                    MsgBoxHelper.Show("No active Word document found.")
                End If
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

        '''<summary>
        '''Launch the dueDates1370View window and populates it with
        '''relevant data from the active Word document</summary>
        '''<remarks>
        '''This function manually constructs and initializes the
        '''DueDates1370View and its host form (DueDates1370Host).  It
        '''retrieves the "Commitment" custom document property and sets
        '''the CommitmentDateLbl in the view. If the "early_nintey_day"
        '''flag is set in the document, it also makes the Early 90DayLbl
        '''visible. Form layout and position are explicitly set in the
        '''handler to ensure proper display bypassing the usual Form_load
        '''logic in the host form. The DueDate1370Handler is called to
        '''populate the view with calculated due dates.
        '''</remarks>
        Public Sub LaunchDueDates1370View()
            Dim host As New DueDates1370Host()
            Dim view As New DueDates1370View(host)
            host.ElementHost1.Child = view

            ' Set CommitmentDateLbl using custom doc property
            Dim doc As Word.Document = DocumentHelper.GetActiveWordDocument()
            If doc IsNot Nothing Then
                Try
                    Dim commitmentRaw As String = doc.CustomDocumentProperties("Commitment").Value.ToString()
                    Dim parsedDate As Date
                    If Date.TryParse(commitmentRaw, parsedDate) Then
                        view.CommitmentDateLbl.Content = parsedDate.ToString("MM/dd/yyyy")
                    Else
                        view.CommitmentDateLbl.Content = commitmentRaw ' Fallback
                    End If
                Catch ex As Exception
                    view.CommitmentDateLbl.Content = "(Missing)"
                End Try
            End If


            ' === Layout Note ===
            ' Normally, window sizing and positioning would be handled in the code-behind
            ' of the Host form (DueDates1370Host.vb) using the Form_Load event.
            ' However, in this case, we are manually constructing and wiring up the WPF view
            ' (DueDates1370View) inside this handler, and we need to avoid overwriting it.
            ' Therefore, we also apply layout and positioning logic here in the handler to ensure
            ' the form and its embedded WPF view are initialized and displayed correctly.
            ' Now call the handler
            Dim handler As New DueDates1370Handler()
            handler.PopulateDueDates(view)

            ' Show or hide Early90DayLbl based on document flag
            If HasEarlyNinetyDayFlag() Then
                view.Early90DayLbl.Visibility = Visibility.Visible
            Else
                view.Early90DayLbl.Visibility = Visibility.Hidden
            End If

            host.ClientSize = New Drawing.Size(375, 565)
            host.Text = ""
            host.MinimizeBox = False
            host.MaximizeBox = False
            host.ShowIcon = False
            host.FormBorderStyle = FormBorderStyle.FixedSingle
            host.TopMost = True

            FormPositionHelper.MoveFormToTopLeftOfAllScreens(host, 10, 10)
            host.ElementHost1.Width = host.ClientSize.Width - 40
            host.ElementHost1.Height = host.ClientSize.Height - 40
            host.ElementHost1.Location = New Drawing.Point(20, 20)

            host.Show()
        End Sub

        '''<summary>
        '''Launch the DueDatesPprView window and populates it with
        '''relevant data from the active Word document</summary>
        '''<remarks>
        '''This function manually constructs and initializes the
        '''DueDatesPprView and its host form (DueDatesPprHost).  It
        '''retrieves the "Commitment" custom document property and sets
        '''the CommitmentDateLbl in the view.
        '''The DueDatesPprHandler is called to populate the view with
        '''calculated due dates.
        '''</remarks>
        Public Sub LaunchDueDatesPprView()
            Dim host As New DueDatePprHost()
            Dim view As New DueDatePprView(host)
            host.ElementHost1.Child = view

            ' Optional: read commitment date from Word and prefill the textbox
            Try
                Dim doc As Word.Document = DocumentHelper.GetActiveWordDocument()
                If doc IsNot Nothing Then
                    Dim commitmentRaw As Object = doc.CustomDocumentProperties("Commitment").Value
                    Dim parsedDate As Date
                    If Date.TryParse(commitmentRaw.ToString(), parsedDate) Then
                        view.CommitmentDateTxt.Text = parsedDate.ToString("MM/dd/yyyy")
                        view.FirstDueDateTxt.Text = parsedDate.AddMonths(6).ToString("MM/dd/yyyy")
                    End If
                End If
            Catch ex As Exception
                ' If not found or invalid, silently continue with empty fields
            End Try

            ' Layout & styling (matches DueDates1370View)
            host.ClientSize = New Drawing.Size(660, 560)
            host.Text = ""
            host.MinimizeBox = False
            host.MaximizeBox = False
            host.ShowIcon = False
            host.FormBorderStyle = FormBorderStyle.FixedSingle
            host.TopMost = True

            FormPositionHelper.MoveFormToTopLeftOfAllScreens(host, 10, 10)

            host.ElementHost1.Width = host.ClientSize.Width - 40
            host.ElementHost1.Height = host.ClientSize.Height - 40
            host.ElementHost1.Location = New Drawing.Point(20, 20)

            Try
                Dim doc As Word.Document = DocumentHelper.GetActiveWordDocument()
                If doc IsNot Nothing Then
                    ' Prefill CommitmentDateTxt and FirstDueDateTxt
                    Dim commitmentRaw As Object = doc.CustomDocumentProperties("Commitment").Value
                    Dim commitmentDate As Date
                    If Date.TryParse(commitmentRaw.ToString(), commitmentDate) Then
                        view.CommitmentDateTxt.Text = commitmentDate.ToString("MM/dd/yyyy")
                        view.FirstDueDateTxt.Text = commitmentDate.AddMonths(6).ToString("MM/dd/yyyy")

                        ' === MOVE THESE HERE ===
                        ' Set CurrentDueDatePick and NextDueDatePick
                        Try
                            Dim currentYearDate As New Date(Date.Today.Year, commitmentDate.Month, commitmentDate.Day)
                            view.CurrentDueDatePick.SelectedDate = currentYearDate
                            view.NextDueDatePick.SelectedDate = currentYearDate.AddMonths(6)
                        Catch ex As Exception
                            view.CurrentDueDatePick.SelectedDate = Nothing
                            view.NextDueDatePick.SelectedDate = Nothing
                        End Try
                    End If

                    ' Set MaxDateTxt from Expiration
                    Try
                        Dim expirationRaw As Object = doc.CustomDocumentProperties("Expiration").Value
                        Dim expirationDate As Date
                        If Date.TryParse(expirationRaw.ToString(), expirationDate) Then
                            view.MaxDateTxt.Text = expirationDate.ToString("MM/dd/yyyy")
                        End If
                    Catch ex As Exception
                        view.MaxDateTxt.Text = ""
                    End Try
                End If
            Catch ex As Exception
                ' Silent fallback
            End Try

            host.Show()
        End Sub

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
        Public Function LaunchReportTypeView(commitmentDate As String) As String
            Dim host As New ReportTypeHost()

            ' Create the view manually so we can control wiring
            Dim view As New ReportTypeView(host)
            host.ElementHost1.Child = view

            ' Set the commitment date label
            If Not String.IsNullOrWhiteSpace(commitmentDate) Then
                Dim parsedDate As Date
                If Date.TryParse(commitmentDate, parsedDate) Then
                    view.CommitmentDateLbl.Content = parsedDate.ToString("MM/dd/yyyy")
                Else
                    view.CommitmentDateLbl.Content = commitmentDate
                End If
            Else
                view.CommitmentDateLbl.Content = "(Missing)"
            End If


            DocumentHelper.GetActiveWordDocument()
            ' === Show Report Type if present ===
            Try
                Dim doc As Word.Document = DocumentHelper.GetActiveWordDocument()
                If doc IsNot Nothing Then
                    Dim reportTypeValue As String = doc.CustomDocumentProperties("Report Type").Value.ToString()
                    If Not String.IsNullOrWhiteSpace(reportTypeValue) Then
                        view.ReportTypeCbo.SelectedItem = reportTypeValue
                    End If
                End If
            Catch ex As Exception
                ' Do nothing if missing
            End Try

            ' ✅ Populate the ComboBox
            Dim reportTypes As List(Of String) = GetReportTypes()
            view.ReportTypeCbo.ItemsSource = reportTypes

            ' Show form
            host.Show()

            ' Return selected value (if any)
            Return view.ReportTypeCbo.SelectedItem?.ToString()
        End Function

        ''' <summary>
        ''' Handles the process of setting extension or renewal due dates for the given Word document and report type.
        ''' </summary>
        ''' <param name="doc">The active Word document to update.</param>
        ''' <param name="reportType">The selected report type, used to determine the due date calculation logic.</param>
        ''' <remarks>
        ''' Prompts the user to confirm or enter an extension due date, validates the input, and writes the
        ''' "CurrentDueDate" and "NextDueDate" custom properties to the document. The next due date is set to
        ''' one year after the entered date, or two years if the report type is "1026.5(B)(1)".
        ''' Displays error messages if the expiration date is missing/invalid or if the user input is not a valid date.
        ''' </remarks>
        Private Sub HandleExtensionDueDate(doc As Word.Document, reportType As String)
            Dim expirationDateStr As String = DocumentPropertyHelper.GetPropertyValue(doc, "Expiration")
            Dim expirationDate As Date

            If Not Date.TryParse(expirationDateStr, expirationDate) Then
                MsgBox("Missing or invalid expiration date in document properties.", MsgBoxStyle.Critical)
                Exit Sub
            End If

            Dim defaultDueDate As Date = expirationDate.AddMonths(-6)
            If defaultDueDate < Now Then defaultDueDate = defaultDueDate.AddYears(2)

            Dim input = InputBox("Please enter/confirm the Extension Due Date:", "Extension/Renewal Due Date", defaultDueDate.ToShortDateString())
            Dim userDueDate As Date

            If Not Date.TryParse(input, userDueDate) Then
                MsgBox("Invalid date format. Please enter a valid date.", MsgBoxStyle.Critical)
                Exit Sub
            End If

            Dim nextDueDate As Date = If(
                reportType.Equals("1026.5(B)(1)", StringComparison.OrdinalIgnoreCase),
                userDueDate.AddYears(2),
                userDueDate.AddYears(1)
            )

            DocumentPropertyHelper.WriteCustomProperty(doc, "CurrentDueDate", userDueDate.ToShortDateString())
            DocumentPropertyHelper.WriteCustomProperty(doc, "NextDueDate", nextDueDate.ToShortDateString())

            MsgBox("Due dates saved successfully.")
        End Sub

        ''' <summary>
        ''' Handles the click event for the "Report Type Selected" button.
        ''' </summary>
        ''' <param name="hostForm">The parent form that hosts the button, which will be closed after the action is performed.</param>
        Public Sub ReportTypeSelectedBtnClick(selectedReportType As String, reportDate As String, hostForm As Form)
            '^--If the report type or date are null or white space then we need to get that before proceeding

            If String.IsNullOrWhiteSpace(selectedReportType) Then
                MsgBoxHelper.Show("Please select a report type before continuing.")
            ElseIf String.IsNullOrWhiteSpace(reportDate) Then
                MsgBoxHelper.Show("Please select a report date, or the initial date of this report, before continuing.")
            Else
                HandleSelectedReportType(selectedReportType)
                '^--Save the report type in custom doc properties
                HandleSelectedReportDate(reportDate)
                '^--Save the report date in custom doc properties

                Dim wordDoc As Word.Document = DocumentHelper.GetActiveWordDocument()

                Select Case selectedReportType.Trim().ToUpperInvariant()

                    Case "PPR", "MDSO", "COT", "1026.2(L)", "1026.2(B)", "1026(C)", "IMD"
                        If selectedReportType.Equals("1026(C)", StringComparison.OrdinalIgnoreCase) Then
                            MsgBoxHelper.Show(
                                "A 1026(c) is handled the same way as a COT is handled." & vbCrLf & vbCrLf &
                                "Select the due date as tomorrow followed by 1-year from now as the next due. "
                            )
                        End If

                        If selectedReportType.Equals("COT", StringComparison.OrdinalIgnoreCase) Then
                            Dim cotOpinion As String = InputBox("What is the COT opinion?", "COT Opinion", "not COT")
                            ' TODO: Store cotOpinion if needed
                        End If

                        LaunchDueDatesPprView()

                    Case "1370(B)(1)", "1372(A)(1)", "1372(E)", "UNLIKELY 1370(B)(1)", "UNLIKELY 1370(C)(1)"
                        LaunchDueDates1370View()

                    Case "1026.5(B)(1)", "2972"
                        HandleExtensionDueDate(wordDoc, selectedReportType)

                    Case Else
                        MsgBoxHelper.Show("The selected report type does not have a defined due date process. Please check configuration.")
                End Select

            End If

        End Sub

    End Class

End Namespace
' Footer:
''===========================================================================================
'' Filename: .......... ReportTypeHandler.vb
'' Description: ....... Handles button clicks in the ReportTypeView
'' Created: ........... 2025-05-02
'' Updated: ........... 2025-05-02
'' Installs to: ....... EZLogger/Handlers
'' Compatibility: ..... VSTO, WPF
'' Contact Author: .... lundeen-bryan
'' Copyright:  ........ ©2025. All rights reserved.
'' Notes: ............. _
' (1) Method Index _
' - GetReportTypes(): Retrieves a list of report types from the global
'   configuration file for use in ComboBoxes.
' - HandleCloseClick(hostForm As Form): Closes the ReportTypeView form
'   if it is not null.
' - HandleSelectedReportDate(reportDate As String): Saves the selected
'   report type to the Word document's custom properties.
' - HandleSelectedReportType(reportType As String): Saves the selected
'   report type to the Word document's custom properties.
' - HasEarlyNinetyDayFlag(): Returns True if the document property
'   "Early90Day" is set to 1, otherwise False.
' - LaunchDueDates1370View(): Opens and populates the DueDates1370View
'   form using data from the active document.
' - LaunchDueDatesPprView(): Opens and populates the DueDatesPprView
'   form using data from the active document.
' - LaunchReportTypeView(commitmentDate As String): Displays the
'   ReportTypeView form, sets the commitment date, and returns the
'   selected report type.
' - ReportTypeSelectedBtnClick(selectedReportType As String, reportDate
'   As String, hostForm As Form): Saves the selected report type and
'   date, then opens the appropriate due dates form and closes the host
'   form.
''===========================================================================================
