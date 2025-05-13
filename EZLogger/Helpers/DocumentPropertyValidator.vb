' Namespace=EZLogger/Helpers
' Filename=DocumentPropertyValidator.vb
' !See Label Footer for notes

Imports Microsoft.Office.Core
Imports Microsoft.Office.Interop.Word

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
                Dim doc As Document = DocumentHelper.GetActiveWordDocument()
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

' Footer:
''===========================================================================================
'' Filename: .......... DocumentPropertyValidator.vb
'' Description: ....... "validates" or copies the names from list as custom doc property names
'' Created: ........... 2025-05-12
'' Updated: ........... 2025-05-12
'' Installs to: ....... EZLogger/Helpers
'' Compatibility: ..... VSTO
'' Contact Author: .... lundeen-bryan
'' Copyright:  ........ ©2025. All rights reserved.
'' Notes: ............. _
' (1) May need to use InitializeVsto
''===========================================================================================