Imports Word = Microsoft.Office.Interop.Word
Imports System.Windows
Imports System.Windows.Forms
Imports MessageBox = System.Windows.MessageBox
Imports Clipboard = System.Windows.Clipboard

Public Class WordFooterReader

    Public Function FindPatientNumberInFooter() As String
        Dim functionThatCalls As String = "FindPatientNumberInFooter"
        Dim patientNumber As String = ""
        Dim numberLocated As Boolean = False
        Dim repeatSearch As Integer = 1
        Const maxRepeatSearch As Integer = 5

        Try
            Dim doc As Word.Document = Globals.ThisAddIn.Application.ActiveDocument
            Dim footerRange As Word.Range = doc.Sections(1).Footers(Word.WdHeaderFooterIndex.wdHeaderFooterPrimary).Range

            ResetFindParameters(footerRange)

            Do Until numberLocated OrElse repeatSearch > maxRepeatSearch
                With footerRange.Find
                    .Text = "[0-9]{6,}-[0-9]{1,}"
                    .MatchWildcards = True
                    .MatchWholeWord = True
                    .Wrap = Word.WdFindWrap.wdFindStop

                    If .Execute() Then
                        Dim userChoice = MessageBox.Show("Does this look like a matching patient number?" & vbCrLf & footerRange.Text, "Confirm", MessageBoxButtons.YesNo)
                        If userChoice = DialogResult.Yes Then
                            patientNumber = footerRange.Text.Trim()
                            numberLocated = True
                            Clipboard.SetText(patientNumber)
                        Else
                            footerRange.MoveStart(Word.WdUnits.wdWord, -1)
                        End If
                    End If
                End With

                repeatSearch += 1
            Loop

            doc.Range(0, 0).Select()

        Catch ex As Exception
            LogError(patientNumber, ex.Message, functionThatCalls)
        End Try

        Return patientNumber
    End Function

    Private Sub ResetFindParameters(rng As Word.Range)
        With rng.Find
            .ClearFormatting()
            .Format = False
            .MatchCase = False
            .MatchAllWordForms = False
        End With
    End Sub

    Private Sub LogError(foundText As String, errorMessage As String, source As String)
        ' Add your own error logging logic here
        MessageBox.Show($"Error in {source}:{vbCrLf}{errorMessage}", "Error")
    End Sub

End Class
