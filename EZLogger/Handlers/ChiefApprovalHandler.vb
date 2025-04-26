Imports System.Windows
Imports System.Windows.Forms
Imports stdole
Imports EZLogger.Helpers

Namespace Handlers
    Public Class ChiefApprovalHandler

        Public Sub OnOpenChiefHostClick()
            Dim host As New ChiefApprovalHost()
            host.Show()
        End Sub

        Public Sub HandleApprovalClick(selectedName As String)
            ' Validate the selected name
            If String.IsNullOrWhiteSpace(selectedName) Then
                MsgBoxHelper.Show("Please select an approver name before approving the report.")
                Return
            End If

            ' Write the selected approver into the document property
            Dim doc As Microsoft.Office.Interop.Word.Document = Globals.ThisAddIn.Application.ActiveDocument
            DocumentPropertyHelper.WriteCustomProperty(doc, "Approved By", selectedName)

            ' Optional: Confirm to the user
            MsgBoxHelper.Show($"'{selectedName}' has been set as the Approver.")
        End Sub

        ''' <summary>
        ''' Handles inserting the selected approver's signature into the document.
        ''' </summary>
        ''' <param name="selectedName">The approver name selected from the ListboxApproval.</param>
        Public Sub HandleSignatureClick(selectedName As String)
            ' Validate the selected name
            If String.IsNullOrWhiteSpace(selectedName) Then
                MsgBoxHelper.Show("Please select an approver name before inserting a signature.")
                Return
            End If

            ' Call the helper to insert the signature
            SignatureHelper.InsertSignature(selectedName)
        End Sub
        Public Sub HandleCloseClick(form As Form)
            If form IsNot Nothing Then form.Close()
        End Sub

    End Class
End Namespace

