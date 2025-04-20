Imports System.Windows
Imports System.Windows.Forms
Imports EZLogger.Helpers
Imports MessageBox = System.Windows.MessageBox
Imports Word = Microsoft.Office.Interop.Word

Namespace Handlers

    Public Class DueDatePprHandler

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
        ''' Called when the user clicks Go Back.
        ''' </summary>
        ''' <param name="hostForm">The form to close.</param>
        Public Sub HandleGoBackClick(hostForm As Form)
            ' Re-open the ReportTypeView
            Dim reportTypeHandler As New ReportTypeHandler()
            reportTypeHandler.LaunchReportTypeView("")

            hostForm?.Close()
        End Sub

    End Class

End Namespace
