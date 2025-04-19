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

            ' ✅ Populate the ComboBox
            Dim reportTypes As List(Of String) = GetReportTypes()
            view.ReportTypeCbo.ItemsSource = reportTypes

            ' Show form
            host.Show()

            ' Return selected value (if any)
            Return view.ReportTypeCbo.SelectedItem?.ToString()
        End Function

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
        Public Sub ReportTypeSelectedBtnClick(selectedReportType As String, hostForm As Form)
            If String.IsNullOrWhiteSpace(selectedReportType) Then
                MsgBoxHelper.Show("Please select a report type before continuing.")
                Exit Sub
            End If

            HandleSelectedReportType(selectedReportType)

            Dim typesNeedingDueDates As New HashSet(Of String)(StringComparer.OrdinalIgnoreCase) From {
                "1370(b)(1)",
                "1372(a)(1)",
                "1372(e)",
                "UNLIKELY 1370(b)(1)",
                "UNLIKELY 1370(c)(1)"
            }

            If typesNeedingDueDates.Contains(selectedReportType.Trim()) Then
                LaunchDueDates1370View()
                hostForm?.Close()
            Else
                MsgBoxHelper.Show($"Report type '{selectedReportType}' does not require due date tracking.")
            End If
        End Sub

        '''' <summary>
        '''' Called when BtnAcceptPPR is clicked. Calculates days until/since due date,
        '''' updates the label, and writes the number to a Word custom property.
        '''' </summary>
        '''' <param name="view">The ReportTypeView instance with the controls.</param>
        'Public Sub HandleAcceptPPR(view As ReportTypeView)
        '    ' Ensure a date is selected
        '    If Not view.PickCurrentDueDate.SelectedDate.HasValue Then
        '        Windows.MessageBox.Show("Select the current due date", "Missing Due Date", MessageBoxButton.OK, MessageBoxImage.Warning)
        '        Exit Sub
        '    End If

        '    ' Calculate day difference
        '    Dim dueDate As Date = view.PickCurrentDueDate.SelectedDate.Value.Date
        '    Dim today As Date = Date.Today
        '    Dim daysDifference As Integer = (dueDate - today).Days

        '    ' Update the label
        '    view.LabelDaysSinceDueDate.Content = daysDifference.ToString()

        '    ' Get the active document
        '    Dim app As Word.Application = Globals.ThisAddIn.Application
        '    Dim doc As Word.Document = TryCast(app.ActiveDocument, Word.Document)

        '    If doc Is Nothing Then
        '        Windows.MessageBox.Show("No active document found.", "EZLogger", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        '        Exit Sub
        '    End If

        '    ' Write to custom document property
        '    DocumentPropertyHelper.WriteCustomProperty(doc, "Days Since Due", daysDifference.ToString())
        'End Sub

    End Class

End Namespace

