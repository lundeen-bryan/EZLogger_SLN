Imports System.Windows
Imports System.Windows.Forms
Imports EZLogger.EZLogger
Imports EZLogger.Helpers
Imports MessageBox = System.Windows.MessageBox
Imports Word = Microsoft.Office.Interop.Word

Namespace Handlers

    Public Class DueDatePprHandler

        ''' <summary>
        ''' Called when the user clicks the Year Down button to subtract one year from the current due date.
        ''' </summary>
        ''' <param name="view">The DueDatePprView instance.</param>
        Public Sub HandleYearDownClick(view As DueDatePprView)
            Try
                If view.CurrentDueDatePick.SelectedDate.HasValue Then
                    Dim selectedDate As Date = view.CurrentDueDatePick.SelectedDate.Value
                    Dim newDate As Date = selectedDate.AddYears(-1)
                    view.CurrentDueDatePick.SelectedDate = newDate
                Else
                    MessageBox.Show("Please select a current due date first.", "Missing Date", MessageBoxButton.OK, MessageBoxImage.Warning)
                End If
            Catch ex As Exception
                MessageBox.Show($"Error adjusting year: {ex.Message}", "EZLogger", MessageBoxButton.OK, MessageBoxImage.Error)
            End Try
        End Sub

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
        ''' Saves the current and next PPR due dates to Word document custom properties.
        ''' </summary>
        ''' <param name="view">The DueDatePprView that contains the date pickers.</param>
        Public Sub HandleSavePprChoiceClick(view As DueDatePprView)
            Try
                ' Get the active Word document
                Dim doc As Word.Document = TryCast(Globals.ThisAddIn.Application.ActiveDocument, Word.Document)
                If doc Is Nothing Then
                    MessageBox.Show("No active Word document found.", "EZLogger", MessageBoxButton.OK, MessageBoxImage.Warning)
                    Exit Sub
                End If

                ' Extract selected dates from the date pickers
                Dim currentDueDate As Nullable(Of Date) = view.CurrentDueDatePick.SelectedDate
                Dim nextDueDate As Nullable(Of Date) = view.NextDueDatePick.SelectedDate

                ' Validate and write each date if present
                If currentDueDate.HasValue Then
                    DocumentPropertyHelper.WriteCustomProperty(doc, "Current Due Date", currentDueDate.Value.ToString("MM/dd/yyyy"))
                End If

                If nextDueDate.HasValue Then
                    DocumentPropertyHelper.WriteCustomProperty(doc, "Next Due Date", nextDueDate.Value.ToString("MM/dd/yyyy"))
                End If

                MsgBoxHelper.Show("PPR due dates saved to document properties.")

            Catch ex As Exception
                MsgBoxHelper.Show($"Unexpected error: {ex.Message}")
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
