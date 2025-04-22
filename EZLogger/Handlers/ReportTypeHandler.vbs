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

            ' === Show Report Type if present ===
            Try
                Dim doc As Word.Document = TryCast(Globals.ThisAddIn.Application.ActiveDocument, Word.Document)
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

        '''<summary>
        ''' Saves the selected report type to the current active Word Document's custom properties.
        '''</summary>
        '''<param name="report_type">The selected report type from the combobox</param>
        '''<remarks>
        ''' This funciton checks if the report type is non-empty then
        ''' confirms the presense of an active Word Document, and then
        ''' writes the value to a custom property named "Report Type"
        '''</remarks>
        Public Sub HandleSelectedReportType(report_type As String)
            If String.IsNullOrWhiteSpace(report_type) Then
                MsgBoxHelper.Show("Please select a  report type before confirming.")
                Exit Sub
            End If

            ' Write the selected report type to the custom property
            Dim doc As Word.Document = TryCast(Globals.ThisAddIn.Application.ActiveDocument, Word.Document)
            If doc IsNot Nothing Then
                DocumentPropertyHelper.WriteCustomProperty(doc, "Report Type", report_type)
                ' MsgBoxHelper.Show("Report type has been saved to the document.")
            Else
                MsgBoxHelper.Show("No active Word document found.")
            End If
        End Sub

        '''<summary>
        '''Retreieves a list of available report types from the global_config.json file
        '''</summary>
        '''</returns> A list of report type strings for use in Comboboxes or other UI elements</returns>
        '''<remarks>
        '''This function loads report types from the report_type key
        '''inside the listbox section of the global_config.json file
        '''</remarks>
        Public Function GetReportTypes() As List(Of String)
            Return ListHelper.GetListFromGlobalConfig("listbox", "report_type")
        End Function

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
            Dim doc As Word.Document = TryCast(Globals.ThisAddIn.Application.ActiveDocument, Word.Document)
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

        ''' <summary>
        ''' Handles the click event for the "Report Type Selected" button.
        ''' </summary>
        ''' <param name="hostForm">The parent form that hosts the button, which will be closed after the action is performed.</param>
        ''' <remarks>
        ''' This method launches the DueDates1370View and closes the provided host form.
        ''' </remarks>
        Public Sub ReportTypeSelectedBtnClick(selectedReportType As String, reportDate As String, hostForm As Form)
            If String.IsNullOrWhiteSpace(selectedReportType) Then
                MsgBoxHelper.Show("Please select a report type before continuing.")
                Exit Sub
            End If

            HandleSelectedReportType(selectedReportType)

            ' Save the selected report date to Word doc properties
            If Not String.IsNullOrWhiteSpace(reportDate) Then
                Dim doc As Word.Document = TryCast(Globals.ThisAddIn.Application.ActiveDocument, Word.Document)
                If doc IsNot Nothing Then
                    DocumentPropertyHelper.WriteCustomProperty(doc, "Report Date", reportDate)
                End If
            End If

            ' === Continue with due date logic ===
            Dim typesNeedingDueDates As New HashSet(Of String)(StringComparer.OrdinalIgnoreCase) From {
                "1370(b)(1)",
                "1372(a)(1)",
                "1372(e)",
                "UNLIKELY 1370(b)(1)",
                "UNLIKELY 1370(c)(1)"
            }

            If typesNeedingDueDates.Contains(selectedReportType.Trim()) Then
                LaunchDueDates1370View()
            Else
                LaunchDueDatesPprView()
            End If

            hostForm?.Close()
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
                Dim doc As Word.Document = TryCast(Globals.ThisAddIn.Application.ActiveDocument, Word.Document)
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
                Dim doc As Word.Document = TryCast(Globals.ThisAddIn.Application.ActiveDocument, Word.Document)
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

        '''<summary>
        '''Closes the ReportTypeView form when the Done button is clicked in ReportTypeView
        '''</summary>
        '''<param name="form">The instance of the form to be closed (ReportTypeView)</param>
        '''<remarks>
        '''This function is called when the user presses DoneBtn. In that function the
        '''checkbox is checked before calling this function and closing the view window.
        '''</remarks>
        Public Sub HandleCloseClick(form As Form)
            If form IsNot Nothing Then form.Close()
        End Sub

    End Class

End Namespace

