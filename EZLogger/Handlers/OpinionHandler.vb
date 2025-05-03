' Namespace=EZLogger/Handlers
' Filename=OpinionHandler.vb
' !See Label Footer for notes

Imports EZLogger.Helpers
Imports System.Windows.Forms

Namespace Handlers
    Public Class OpinionHandler

        Private ReadOnly _wordApp As Word.Application

        Public Sub New(wordApp As Word.Application)
            _wordApp = wordApp
        End Sub


        ''' <summary>
        ''' Opens the Opinion form, positions it at the top-left corner of all screens with specified offsets,
        ''' and ensures it stays on top of other windows.
        ''' </summary>
        Public Sub OnOpenOpinionFormClick()
            Dim host As New OpinionHost()
            host.TopMost = True
            FormPositionHelper.MoveFormToTopLeftOfAllScreens(host, 10, 10)
            host.Show()
        End Sub

        ''' <summary>
        ''' Handles the click event of the OK button in the Opinion form.
        ''' </summary>
        ''' <param name="opinion">A string representing the selected opinion.</param>
        ''' <remarks>
        ''' This method performs the following actions:
        ''' 1. Validates if an opinion has been selected.
        ''' 2. If valid, attempts to write the opinion to the active Word document's custom properties.
        ''' 3. Displays appropriate messages to the user based on the operation's success.
        ''' </remarks>
        Public Sub HandleOpinionOkClick(opinion As String)
            If String.IsNullOrWhiteSpace(opinion) Then
                MsgBox("Please select an opinion before clicking Save.")
            Else
                ' Write the selected report type to the custom property
                Dim doc As Word.Document = TryCast(Globals.ThisAddIn.Application.ActiveDocument, Word.Document)
                If doc IsNot Nothing Then
                    DocumentPropertyHelper.WriteCustomProperty(doc, "Opinion", opinion)
                    MsgBoxHelper.Show("Opinion has been saved to the document.")
                Else
                    MsgBoxHelper.Show("No active Word document found.")
                End If
            End If
        End Sub

        Public Sub HandleOpinionFirstPageClick()
            NavigationHelper.GoToFirstPage(_wordApp)
        End Sub

        Public Sub HandleOpinionLastPageClick()
            NavigationHelper.GoToLastPage(_wordApp)
        End Sub

        Public Sub HandleCloseClick(hostForm As Form)
            hostForm?.Close()
        End Sub

    End Class
End Namespace

' Footer:
''===========================================================================================
'' Filename: .......... OpinionHandler.vb
'' Description: ....... Handles the button events on the OpinionView
'' Created: ........... 2025-05-02
'' Updated: ........... 2025-05-02
'' Installs to: ....... EZLogger/Handlers
'' Compatibility: ..... VSTO, WPF
'' Contact Author: .... lundeen-bryan
'' Copyright:  ........ ©2025. All rights reserved.
'' Notes: ............. _
' (1) notes_here
''===========================================================================================