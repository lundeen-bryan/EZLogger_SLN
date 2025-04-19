Imports System
Imports Microsoft.Office.Interop.Word

Namespace Helpers

    Public Module RushStatusHelper

        ''' <summary>
        ''' Calculates "Days Since Due" and "Rush Status" from the given due date,
        ''' then writes both values to Word document custom properties.
        ''' </summary>
        ''' <param name="dueDate">The current due date to compare with today.</param>
        Public Sub SetRushStatusAndDaysSinceDue(dueDate As Date)
            Dim today As Date = Date.Today
            Dim daysUntilDue As Integer = (dueDate - today).Days

            ' Get the active Word document
            Dim doc As Document = TryCast(Globals.ThisAddIn.Application.ActiveDocument, Document)
            If doc Is Nothing Then Exit Sub

            ' Write "Days Since Due" (can be negative)
            DocumentPropertyHelper.WriteCustomProperty(doc, "Days Since Due", daysUntilDue.ToString())

            ' Determine Rush Status
            Dim rushText As String
            Select Case True
                Case (daysUntilDue > 5 AndAlso daysUntilDue < 11)
                    rushText = "RUSH"
                Case (daysUntilDue > 0 AndAlso daysUntilDue <= 5)
                    rushText = "SUPER RUSH"
                Case (daysUntilDue <= 0)
                    rushText = "PAST DUE"
                Case Else
                    rushText = "ON TIME"
            End Select

            ' Write "Rush Status"
            DocumentPropertyHelper.WriteCustomProperty(doc, "Rush Status", rushText)
        End Sub

    End Module

End Namespace
