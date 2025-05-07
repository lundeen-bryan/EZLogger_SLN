Imports Microsoft.Office.Interop.Word
Imports Microsoft.Office.Core

Namespace Helpers
    Public Module DocumentPropertyValidator

        ''' <summary>
        ''' Ensures all expected document custom properties exist in the active Word document.
        ''' If missing, they are added with an empty value.
        ''' </summary>
        Public Sub ValidateRequiredCustomProperties()
            Dim requiredProps As String() = {
                "Patient Number", "Patient Name", "Unique ID", "Court Number", "Charges",
                "Evaluator", "Assigned To", "Approved By", "Classification", "County",
                "Report Date", "Due Date", "Days Since Due", "Next Due", "Commitment",
                "Admission", "Expiration", "DOB", "Age", "Sex", "Program", "Unit",
                "Rush Status", "Processed By", "Report Type", "Pages"
            }

            Try
                Dim doc As Document = Globals.ThisAddIn.Application.ActiveDocument
                Dim existingProps As New HashSet(Of String)(StringComparer.OrdinalIgnoreCase)

                For Each prop As DocumentProperty In doc.CustomDocumentProperties
                    existingProps.Add(prop.Name)
                Next

                For Each propName As String In requiredProps
                    If Not existingProps.Contains(propName) Then
                        DocumentPropertyHelper.WriteCustomProperty(doc, propName, "")
                    End If
                Next

            Catch ex As Exception
                MsgBoxHelper.Show("Error validating custom properties: " & ex.Message)
            End Try
        End Sub

    End Module
End Namespace
