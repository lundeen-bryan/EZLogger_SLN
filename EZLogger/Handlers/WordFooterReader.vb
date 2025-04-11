Imports Word = Microsoft.Office.Interop.Word
Imports System.Windows.Forms
Imports Clipboard = System.Windows.Clipboard
Imports EZLogger.Helpers

Public Class WordFooterReader

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
                                         .ShowNo = True
                                     }

                                     MsgBoxHelper.Show(config, Sub(result)
                                                                   If result = CustomMsgBoxResult.Yes Then
                                                                       Dim foundText = footerRange.Text.Trim()
                                                                       Clipboard.SetText(foundText)
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

    Private Sub ResetFindParameters(rng As Word.Range)
        With rng.Find
            .ClearFormatting()
            .Format = False
            .MatchCase = False
            .MatchAllWordForms = False
        End With
    End Sub

    Private Sub LogError(foundText As String, errorMessage As String, source As String)
        MessageBox.Show($"Error in {source}:{vbCrLf}{errorMessage}", "Error")
    End Sub

End Class
