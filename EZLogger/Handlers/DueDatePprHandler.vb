Imports System.Windows
Imports System.Windows.Forms
Imports EZLogger.EZLogger
Imports EZLogger.Helpers
Imports MessageBox = System.Windows.MessageBox
Imports Word = Microsoft.Office.Interop.Word

Namespace Handlers

    Public Class DueDatePprHandler

        ''' <summary>
        ''' Called when the user clicks SwitchDatesBtn to swap current and next due dates.
        ''' </summary>
        ''' <param name="view">The DueDatePprView instance.</param>
        Public Sub HandleSwitchDatesClick(view As DueDatePprView)
            Try
                Dim current As Date? = view.CurrentDueDatePick.SelectedDate
                Dim nextDue As Date? = view.NextDueDatePick.SelectedDate

                ' Swap only if both dates are set
                If current.HasValue AndAlso nextDue.HasValue Then
                    view.CurrentDueDatePick.SelectedDate = nextDue
                    view.NextDueDatePick.SelectedDate = current
                Else
                    MessageBox.Show("Both due dates must be selected to switch them.", "Missing Dates", MessageBoxButton.OK, MessageBoxImage.Warning)
                End If
            Catch ex As Exception
                MessageBox.Show($"Error switching dates: {ex.Message}", "EZLogger", MessageBoxButton.OK, MessageBoxImage.Error)
            End Try
        End Sub

        ''' <summary>
        ''' Called when the user clicks Continue to save their selected PPR date info.
        ''' </summary>
        ''' <param name="view">The DueDatePprView instance with user-entered data.</param>
        Public Sub HandleSavePprChoiceClick(view As DueDatePprView)
            Try
                Dim doc As Word.Document = TryCast(Globals.ThisAddIn.Application.ActiveDocument, Word.Document)
                If doc Is Nothing Then
                    MessageBox.Show("No active Word document found.", "EZLogger", MessageBoxButton.OK, MessageBoxImage.Warning)
                    Exit Sub
                End If

                ' Get commitment date from the textbox
                Dim commitmentRaw As String = view.CommitmentDateTxt.Text.Trim()
                Dim commitmentDate As Date

                If Date.TryParse(commitmentRaw, commitmentDate) Then
                    ' Calculate first due date (commitment + 6 months)
                    Dim firstDueDate As Date = commitmentDate.AddMonths(6)
                    view.FirstDueDateTxt.Text = firstDueDate.ToString("MM/dd/yyyy")

                    ' Optional: Save values to Word custom properties
                    DocumentPropertyHelper.WriteCustomProperty(doc, "Commitment", commitmentDate.ToString("MM/dd/yyyy"))
                    DocumentPropertyHelper.WriteCustomProperty(doc, "First Due Date", firstDueDate.ToString("MM/dd/yyyy"))

                    MessageBox.Show("First PPR Due Date calculated and saved.", "EZLogger")

                Else
                    MessageBox.Show("Invalid commitment date.", "EZLogger", MessageBoxButton.OK, MessageBoxImage.Error)
                End If

            Catch ex As Exception
                MessageBox.Show($"Unexpected error: {ex.Message}", "EZLogger", MessageBoxButton.OK, MessageBoxImage.Error)
            End Try
        End Sub

        ''' <summary>
        ''' Called when the user clicks AcceptDatesBtn to calculate days since due.
        ''' </summary>
        ''' <param name="view">The DueDatePprView instance.</param>
        Public Sub HandleAcceptDatesClick(view As DueDatePprView)
            Try
                If view.CurrentDueDatePick.SelectedDate.HasValue Then
                    Dim selectedDate As Date = view.CurrentDueDatePick.SelectedDate.Value.Date
                    Dim today As Date = Date.Today
                    Dim daysDifference As Integer = (selectedDate - today).Days

                    ' Positive if due date is in the future, negative if overdue
                    view.DaysSinceTxt.Text = daysDifference.ToString()
                Else
                    MessageBox.Show("Please select a Current Due Date before proceeding.", "Missing Date", MessageBoxButton.OK, MessageBoxImage.Warning)
                End If
            Catch ex As Exception
                MessageBox.Show($"Error calculating days difference: {ex.Message}", "EZLogger", MessageBoxButton.OK, MessageBoxImage.Error)
            End Try
        End Sub

        ''' <summary>
        ''' Called when the user clicks Go Back.
        ''' </summary>
        ''' <param name="hostForm">The form to close.</param>
        Public Sub HandleGoBackClick(hostForm As Form)
            Dim doc As Word.Document = TryCast(Globals.ThisAddIn.Application.ActiveDocument, Word.Document)
            Dim commitmentRaw As String = ""

            If doc IsNot Nothing Then
                Try
                    commitmentRaw = doc.CustomDocumentProperties("Commitment").Value.ToString()
                Catch ex As Exception
                    commitmentRaw = ""
                End Try
            End If

            Dim reportTypeHandler As New ReportTypeHandler()
            reportTypeHandler.LaunchReportTypeView(commitmentRaw)

            hostForm?.Close()
        End Sub

    End Class

End Namespace
