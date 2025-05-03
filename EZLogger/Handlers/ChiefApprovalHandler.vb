' Namespace=EZLogger/Handlers
' Filename=ChiefApprovalHandler.vb
' !See Label Footer for notes

Imports EZLogger.Helpers
Imports System.Windows.Forms

Namespace Handlers
    Public Class ChiefApprovalHandler

        ''' <summary>
        ''' Handles the approval process when an approver is selected.
        ''' </summary>
        ''' <param name="selectedName">The name of the selected approver.</param>
        ''' <remarks>
        ''' This method performs the following actions:
        ''' 1. Validates that an approver name has been selected.
        ''' 2. Writes the selected approver's name to the document's custom properties.
        ''' 3. Displays a confirmation message to the user.
        ''' </remarks>
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
        ''' Handles the closing of a specified form.
        ''' </summary>
        ''' <param name="form">The Form object to be closed. If null, no action is taken.</param>
        ''' <remarks>
        ''' This method checks if the provided form is not null before attempting to close it.
        ''' It's typically used to safely close a form when a close action is triggered.
        ''' </remarks>
        Public Sub HandleCloseClick(hostForm As Form)
            hostForm?.Close
        End Sub

        ''' <summary>
        ''' Handles the process of inserting a signature when an approver is selected.
        ''' </summary>
        ''' <param name="selectedName">The name of the selected approver whose signature will be inserted.</param>
        ''' <remarks>
        ''' This method performs the following actions:
        ''' 1. Validates that an approver name has been selected.
        ''' 2. If valid, calls the SignatureHelper to insert the signature of the selected approver.
        ''' 3. If no name is selected, displays an error message to the user.
        ''' </remarks>
        Public Sub HandleSignatureClick(selectedName As String)
            ' Validate the selected name
            If String.IsNullOrWhiteSpace(selectedName) Then
                MsgBoxHelper.Show("Please select an approver name before inserting a signature.")
                Return
            End If

            ' Call the helper to insert the signature
            SignatureHelper.InsertSignature(selectedName)
        End Sub

        ''' <summary>
        ''' Opens and displays the Chief Approval Host window.
        ''' </summary>
        ''' <remarks>
        ''' This method creates a new instance of the ChiefApprovalHost form and shows it to the user.
        ''' It is typically called when the user initiates the chief approval process.
        ''' </remarks>
        Public Sub OnOpenChiefHostClick()
            Dim host As New ChiefApprovalHost()
            host.Show()
        End Sub

    End Class
End Namespace
' Footer:
''===========================================================================================
'' Filename: .......... ChiefApprovalHandler.vb
'' Description: ....... Handles the ChiefApprovalView buttons
'' Created: ........... 2025-05-02
'' Updated: ........... 2025-05-02
'' Installs to: ....... EZLogger/Handlers
'' Compatibility: ..... VSTO
'' Contact Author: .... lundeen-bryan
'' Copyright:  ........ ©2025. All rights reserved.
'' Notes: ............. _
' (1) notes_here
''===========================================================================================