' Namespace=EZLogger/Handlers
' Filename=WordFooterReader.vb
' !See Label Footer for notes

Imports EZLogger.Helpers
Imports MessageBox = System.Windows.Messagebox

Public Class WordFooterReader

    ''' <summary>
    ''' Initiates a search for a patient number in the footer of the active Word document.
    ''' </summary>
    ''' <remarks>
    ''' This method searches for a pattern matching a patient number in the primary footer of the first section of the active document.
    ''' It uses a regular expression pattern to match numbers in the format of at least 6 digits, followed by a hyphen and at least one more digit.
    ''' The search is repeated up to 5 times if necessary.
    ''' </remarks>
    ''' <param name="onFound">An Action delegate that is invoked when a patient number is found and confirmed by the user. It receives the found patient number as a string parameter.</param>
    ''' <param name="onNotFound">An Action delegate that is invoked when no patient number is found after the maximum number of search attempts.</param>
    Public Sub BeginSearchForPatientNumber(
        onFound As System.Action(Of String),
        onNotFound As System.Action)

        Dim functionThatCalls As String = "BeginSearchForPatientNumber"
        Dim repeatSearch As Integer = 1
        Const maxRepeatSearch As Integer = 5

        Try
            Dim doc As Word.Document = Globals.ThisAddIn.Application.ActiveDocument
            Dim footerRange As Word.Range = doc.Sections(1).Footers(Word.WdHeaderFooterIndex.wdHeaderFooterPrimary).Range

            ResetFindParameters(footerRange)

            Dim searchLoop As System.Action = Nothing

            searchLoop = Sub()
                             If repeatSearch > maxRepeatSearch Then
                                 doc.Range(0, 0).Select()
                                 onNotFound.Invoke()
                                 Return
                             End If

                             With footerRange.Find
                                 .Text = "[0-9]{6,}-[0-9]{1,}"
                                 .MatchWildcards = True
                                 .MatchWholeWord = True
                                 .Wrap = Word.WdFindWrap.wdFindStop

                                 If .Execute() Then
                                     Dim config As New MessageBoxConfig With {
                                         .Message = "Does this look like a matching patient number?" & vbCrLf & footerRange.Text,
                                         .ShowYes = True,
                                         .ShowNo = True,
                                         .ShowOk = False
                                     }

                                     MsgBoxHelper.Show(config, Sub(result)
                                                                   If result = CustomMsgBoxResult.Yes Then
                                                                       Dim foundText = footerRange.Text.Trim()
                                                                       ClipboardHelper.CopyText(foundText)
                                                                       doc.Range(0, 0).Select()
                                                                       onFound.Invoke(foundText)
                                                                   Else
                                                                       footerRange.Start = footerRange.End
                                                                       repeatSearch += 1
                                                                       searchLoop.Invoke()
                                                                   End If
                                                               End Sub)
                                 Else
                                     repeatSearch += 1
                                     searchLoop.Invoke()
                                 End If
                             End With
                         End Sub

            searchLoop.Invoke()

        Catch ex As Exception
            LogError("", ex.Message, functionThatCalls)
            onNotFound.Invoke()
        End Try
    End Sub

    ''' <summary>
    ''' Resets the find parameters for a given Word Range object.
    ''' </summary>
    ''' <param name="rng">The Word.Range object for which to reset find parameters.</param>
    ''' <remarks>
    ''' This method clears any existing formatting and sets default values for various find options:
    ''' - Clears any existing formatting
    ''' - Disables format matching
    ''' - Disables case-sensitive matching
    ''' - Disables matching all word forms
    ''' </remarks>
    Private Sub ResetFindParameters(rng As Word.Range)
        With rng.Find
            .ClearFormatting()
            .Format = False
            .MatchCase = False
            .MatchAllWordForms = False
        End With
    End Sub

	' TODO: re-write this as a helper function to log errors
    Private Sub LogError(foundText As String, errorMessage As String, source As String)
        MessageBox.Show($"Error in {source}:{vbCrLf}{errorMessage}", "Error")
    End Sub

End Class

' Footer:
''===========================================================================================
'' Filename: .......... WordFooterReader.vb
'' Description: ....... Gets the patient number from the report footer
'' Created: ........... 2025-05-02
'' Updated: ........... 2025-05-02
'' Installs to: ....... EZLogger/Handlers
'' Compatibility: ..... VSTO, WPF
'' Contact Author: .... lundeen-bryan
'' Copyright:  ........ ©2025. All rights reserved.
'' Notes: ............. _
' (1) notes_here
''===========================================================================================