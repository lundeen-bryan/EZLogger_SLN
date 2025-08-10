' Namespace=EZLogger/Handlers
' Filename=WordFooterReader.vb
' !See Label Footer for notes

Imports EZLogger.Helpers

Public Class WordFooterReader

    '--------------------------------------------------------------------------------------
    ' Internal classification for patient number patterns.
    ' We keep this simple and readable (no regex dependency).
    '--------------------------------------------------------------------------------------
    Private Enum PatientNumberPattern
        Valid                   ' exactly 6 digits, dash, 1 digit (######-#)
        TooManyBeforeDash       ' 7+ digits before dash (#######-#)
        TooFewBeforeDash        ' 5 digits before dash (#####-#)
        Unrecognized            ' anything else encountered
    End Enum

    Private NotInheritable Class PatternInfo
        Public Property Pattern As PatientNumberPattern
        Public Property Description As String
        Public Property Recommendation As String
    End Class

    ''' <summary>
    ''' Initiates a search for a patient number in the footer of the active Word document.
    ''' </summary>
    ''' <remarks>
    ''' Searches the primary footer of the first section only (by design for simplicity).
    ''' Uses a Word wildcard pattern that matches 5+ digits, a dash, then 1+ digits.
    ''' For each match, shows an explicit classification (valid vs common typos) and asks
    ''' the user whether to use the candidate anyway. Retries up to 5 times.
    ''' </remarks>
    ''' <param name="onFound">
    ''' Invoked when a candidate is accepted by the user; receives the selected patient number.
    ''' </param>
    ''' <param name="onNotFound">
    ''' Invoked when no candidate is accepted after max retries or nothing is matched.
    ''' </param>
    Public Sub BeginSearchForPatientNumber(
        onFound As System.Action(Of String),
        onNotFound As System.Action)

        Dim functionName As String = "WordFooterReader.BeginSearchForPatientNumber"
        Dim repeatSearch As Integer = 1
        Const MAXREPEATSEARCH As Integer = 5

        Try
            Dim doc As Word.Document = DocumentHelper.GetActiveWordDocument()
            Dim footerRange As Word.Range = doc.Sections(1).Footers(Word.WdHeaderFooterIndex.wdHeaderFooterPrimary).Range

            ResetFindParameters(footerRange)

            ' NOTE: We intentionally broaden to 5+ before dash so we can classify the "5-digit" near-match.
            ' Word wildcard syntax (not .NET regex):
            '   [0-9]{5,}-[0-9]{1,}
            ' Meaning: 5 or more digits, then a dash, then 1 or more digits.
            Dim searchLoop As System.Action = Nothing

            searchLoop = Sub()
                             If repeatSearch > MAXREPEATSEARCH Then
                                 doc.Range(0, 0).Select()
                                 onNotFound.Invoke()
                                 Return
                             End If

                             With footerRange.Find
                                 .Text = "[0-9]{5,}-[0-9]{1,}"   ' expanded from {6,} to {5,} to catch 5-digit near matches
                                 .MatchWildcards = True
                                 .MatchWholeWord = True
                                 .Wrap = Word.WdFindWrap.wdFindStop

                                 If .Execute() Then
                                     Dim rawCandidate As String = footerRange.Text.Trim()
                                     Dim info As PatternInfo = ClassifyPatientNumber(rawCandidate)

                                     ' Build explicit, concise message with classification and recommendation.
                                     Dim message As String =
                                         "Found patient number candidate: " & rawCandidate & vbCrLf &
                                         "Pattern: " & info.Description & If(
                                             String.IsNullOrWhiteSpace(info.Recommendation),
                                             "",
                                             vbCrLf & "Recommendation: " & info.Recommendation
                                         ) & vbCrLf & vbCrLf &
                                         "Use this number anyway?"

                                     Dim config As New MessageBoxConfig With {
                                         .Message = message,
                                         .ShowYes = True,
                                         .ShowNo = True,
                                         .ShowOk = False
                                     }

                                     MsgBoxHelper.Show(config,
                                         Sub(result)
                                             If result = CustomMsgBoxResult.Yes Then
                                                 ClipboardHelper.CopyText(rawCandidate)
                                                 doc.Range(0, 0).Select()
                                                 onFound.Invoke(rawCandidate)
                                             Else
                                                 ' Advance past the current match and try again.
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
            Dim errNum As String = ex.HResult.ToString()
            Dim errMsg As String = CStr(ex.Message)
            Dim recommendation As String = "Please confirm the patient number from the report to make sure it matches a patient in ForensicInfo."
            ErrorHelper.HandleError(functionName, errNum, errMsg, recommendation)
        End Try
    End Sub

    ''' <summary>
    ''' Resets the find parameters for a given Word Range object.
    ''' </summary>
    ''' <param name="rng">The Word.Range object for which to reset find parameters.</param>
    ''' <remarks>
    ''' Clears formatting and sets default values for common find options.
    ''' </remarks>
    Private Sub ResetFindParameters(rng As Word.Range)
        With rng.Find
            .ClearFormatting()
            .Format = False
            .MatchCase = False
            .MatchAllWordForms = False
        End With
    End Sub

    '--------------------------------------------------------------------------------------
    ' Classification helpers
    '--------------------------------------------------------------------------------------

    ''' <summary>
    ''' Classifies a candidate patient number into one of four simple buckets and
    ''' returns user-facing description text plus a recommendation if applicable.
    ''' </summary>
    Private Function ClassifyPatientNumber(candidate As String) As PatternInfo
        Dim info As New PatternInfo()

        ' Keep it simple: split around the single expected dash.
        Dim parts = candidate.Split("-"c)
        If parts.Length <> 2 Then
            info.Pattern = PatientNumberPattern.Unrecognized
            info.Description = "unrecognized format"
            info.Recommendation = "Use the format ######-# (6 digits – dash – 1 digit)."
            Return info
        End If

        Dim leftPart As String = parts(0).Trim()
        Dim rightPart As String = parts(1).Trim()

        If Not (IsAllDigits(leftPart) AndAlso IsAllDigits(rightPart)) Then
            info.Pattern = PatientNumberPattern.Unrecognized
            info.Description = "unrecognized format (non-digit characters detected)"
            info.Recommendation = "Use the format ######-# (digits only)."
            Return info
        End If

        Dim leftLen As Integer = leftPart.Length
        Dim rightLen As Integer = rightPart.Length

        ' Valid: exactly 6 before dash and exactly 1 after dash.
        If leftLen = 6 AndAlso rightLen = 1 Then
            info.Pattern = PatientNumberPattern.Valid
            info.Description = "valid (6 digits – dash – 1 digit)"
            info.Recommendation = ""
            Return info
        End If

        ' Too many before dash: 7 or more before dash (any length after dash >=1)
        If leftLen >= 7 AndAlso rightLen >= 1 Then
            info.Pattern = PatientNumberPattern.TooManyBeforeDash
            info.Description = "7 digits before dash"
            info.Recommendation = "Recommended format is ######-#."
            Return info
        End If

        ' Too few before dash: exactly 5 before dash (common typo you asked to surface)
        If leftLen = 5 AndAlso rightLen = 1 Then
            info.Pattern = PatientNumberPattern.TooFewBeforeDash
            info.Description = "only 5 digits before dash"
            info.Recommendation = "Recommended format is ######-#."
            Return info
        End If

        ' Anything else encountered (e.g., 6-before, 2-after; or 8-before, 2-after, etc.)
        info.Pattern = PatientNumberPattern.Unrecognized
        info.Description = "unrecognized format"
        info.Recommendation = "Recommended format is ######-#."
        Return info
    End Function

    ''' <summary>
    ''' Returns True if the input consists only of ASCII digits 0–9.
    ''' </summary>
    Private Function IsAllDigits(s As String) As Boolean
        If String.IsNullOrEmpty(s) Then Return False
        For Each ch As Char In s
            If ch < "0"c OrElse ch > "9"c Then Return False
        Next
        Return True
    End Function

End Class

' Footer:
''===========================================================================================
'' Filename: .......... WordFooterReader.vb
'' Description: ....... Gets the patient number from the report footer (first section, primary)
'' Created: ........... 2025-05-02
'' Updated: ........... 2025-08-10
'' Installs to: ....... EZLogger/Handlers
'' Compatibility: ..... VSTO, WPF
'' Contact Author: .... lundeen-bryan
'' Copyright:  ........ ©2025. All rights reserved.
'' Notes: ............. _
' (1) Method Index _
' - BeginSearchForPatientNumber(onFound, onNotFound):
'   Scans the primary footer of section 1 using Word wildcards "[0-9]{5,}-[0-9]{1,}".
'   For each match, classifies the candidate as one of:
'     • valid (######-#),
'     • 7 digits before dash,
'     • only 5 digits before dash,
'     • unrecognized format.
'   The dialog shows the detected pattern and a recommendation; the user may choose "Use anyway".
'   If accepted, the candidate is copied to the clipboard and returned via onFound; otherwise,
'   the search advances and repeats, up to 5 attempts before calling onNotFound.
'
' - ResetFindParameters(rng): Clears formatting and resets find options.
'
' (2) Design decisions _
'   • Scope limited to section 1, primary footer for simplicity (by product choice).
'   • No auto-correction; we warn but allow the user to continue.
'   • Simple classification logic (split around dash, digit checks) for easier debugging.
''===========================================================================================
