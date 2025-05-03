' Namespace=EZLogger/Handlers
' Filename=DueDatePprHandler.vb
' !See Label Footer for notes

Imports EZLogger.Helpers
Imports System.Windows.Forms

Namespace Handlers
    Public Class DueDatePprHandler

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
                    MsgBoxHelper.Show("Please select a Current Due Date before proceeding.")
                End If
            Catch ex As Exception
                MsgBoxHelper.Show($"Error calculating days difference: {ex.Message}")
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

        ''' <summary>
        ''' Saves the current and next PPR due dates to Word document custom properties.
        ''' </summary>
        ''' <param name="view">The DueDatePprView that contains the date pickers.</param>
        Public Sub HandleSavePprChoiceClick(view As DueDatePprView)
            Try
                ' Get the active Word document
                Dim doc As Word.Document = TryCast(Globals.ThisAddIn.Application.ActiveDocument, Word.Document)
                If doc Is Nothing Then
                    MsgBoxHelper.Show("No active Word document found.")
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
                    MsgBoxHelper.Show("Both due dates must be selected to switch them.")
                End If
            Catch ex As Exception
                MsgBoxHelper.Show($"Error switching dates: {ex.Message}")
            End Try
        End Sub

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
                    MsgBoxHelper.Show("Please select a current due date first.")
                End If
            Catch ex As Exception
                MsgBoxHelper.Show($"Error adjusting year: {ex.Message}")
            End Try
        End Sub

    End Class
End Namespace

' Footer:
''===========================================================================================
'' Filename: .......... DueDatePprHandler.vb
'' Description: ....... description
'' Created: ........... 2025-05-02
'' Updated: ........... 2025-05-02
'' Installs to: ....... EZLogger/Handlers
'' Compatibility: ..... VSTO, WPF
'' Contact Author: .... lundeen-bryan
'' Copyright:  ........ ©2025. All rights reserved.
'' Notes: ............. _
' (1) notes_here
''===========================================================================================