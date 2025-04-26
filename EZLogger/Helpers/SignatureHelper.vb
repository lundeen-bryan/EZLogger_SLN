Imports Microsoft.Office.Interop.Word
Imports EZLogger.Helpers

Namespace Helpers

    ''' <summary>
    ''' Provides functionality to insert an approval signature into the active Word document.
    ''' </summary>
    Public Module SignatureHelper

        ''' <summary>
        ''' Inserts the selected approver's signature into the active Word document at the current cursor position.
        ''' </summary>
        ''' <param name="approverName">The name selected from the ListboxApproval (e.g., "Morgan", "Powers").</param>
        Public Sub InsertSignature(approverName As String)
            Try
                ' Build the key name (example: "morgan_sig")
                Dim keyName As String = approverName.ToLower() & "_sig"

                ' Retrieve the signature path from the global config
                Dim signaturePath As String = ConfigHelper.GetGlobalConfigValue("report_approvals", keyName)

                ' Validate that the file exists
                If String.IsNullOrEmpty(signaturePath) OrElse Not IO.File.Exists(signaturePath) Then
                    MsgBoxHelper.Show($"Signature file not found: {signaturePath}")
                    Return
                End If

                ' Explicitly refer to Word Application
                Dim app As Microsoft.Office.Interop.Word.Application = Globals.ThisAddIn.Application
                Dim doc As Document = app.ActiveDocument
                Dim sel As Selection = app.Selection

                ' If selection is inside a table, delete it (up to 2 times)
                Dim attempts As Integer = 0
                While sel.Information(WdInformation.wdWithInTable) And attempts < 2
                    sel.Tables(1).Delete()
                    attempts += 1
                End While

                ' Insert the signature document content at the current selection
                sel.InsertFile(FileName:=signaturePath, Range:="", ConfirmConversions:=False, Link:=False, Attachment:=False)

                ' Move cursor up into the inserted content for formatting
                sel.MoveUp(Unit:=WdUnits.wdLine, Count:=3)

                ' Format the newly inserted table/text
                If sel.Tables.Count > 0 Then
                    sel.Tables(1).Select()
                End If

                With sel.Font
                    .Name = "Arial"
                End With

                With sel.ParagraphFormat
                    .LeftIndent = 0
                    .RightIndent = 0
                    .SpaceBefore = 0
                    .SpaceAfter = 0
                    .LineSpacingRule = WdLineSpacing.wdLineSpaceSingle
                    .Alignment = WdParagraphAlignment.wdAlignParagraphLeft
                    .WidowControl = True
                    .Hyphenation = True
                    .FirstLineIndent = 0
                End With

            Catch ex As Exception
                ' Basic error handling with MessageBox
                MsgBoxHelper.Show("An error occurred while inserting the signature:" & vbCrLf & ex.Message)
            End Try
        End Sub

    End Module

End Namespace
