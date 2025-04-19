Imports System.Windows
Imports Microsoft.Office.Interop.Word

Namespace Helpers

    Public Module RushStatusHelper

        ''' <summary>
        ''' Calculates rush status based on due date and writes to document properties.
        ''' </summary>
        Public Sub SetRushStatusAndDaysSinceDue(view As ReportTypeView)
            'If Not view.PickCurrentDueDate.SelectedDate.HasValue Then
            'Exit Sub
            'End If

            'Dim dueDate As Date = view.PickCurrentDueDate.SelectedDate.Value.Date
            'Dim today As Date = Date.Today
            'Dim daysTilDue As Integer = DateDiff(DateInterval.Day, today, dueDate)

            '' Write to "Days Since Due" custom property
            'Dim doc As Word.Document = Globals.ThisAddIn.Application.ActiveDocument
            'DocumentPropertyHelper.WriteCustomProperty(doc, "Days Since Due", daysTilDue.ToString())

            '' Set label text
            'If daysTilDue >= 0 Then
            '    view.LabelDaysSinceDueDate.Content = daysTilDue & " til due"
            'Else
            '    view.LabelDaysSinceDueDate.Content = (daysTilDue * -1) & " past due"
            'End If

            '' Determine Rush Status
            'Dim rushText As String
            'Select Case True
            '    Case (daysTilDue > 5 AndAlso daysTilDue < 11)
            '        rushText = "RUSH"
            '    Case (daysTilDue > 0 AndAlso daysTilDue <= 5)
            '        rushText = "SUPER RUSH"
            '    Case (daysTilDue <= 0)
            '        rushText = "PAST DUE"
            '    Case Else
            '        rushText = "ON TIME"
            'End Select

            'DocumentPropertyHelper.WriteCustomProperty(doc, "Rush Status", rushText)
        End Sub

    End Module

End Namespace
