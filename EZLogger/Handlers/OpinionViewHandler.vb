Imports System.Windows.Forms

Namespace Handlers

    Public Class OpinionViewHandler

        Public Sub HandleOpinionOkClick(opinion As String)
            If String.IsNullOrWhiteSpace(opinion) Then
                MsgBox("Please select an opinion before clicking OK.")
            Else
                MsgBox("You selected: " & opinion)
            End If
        End Sub

        Public Sub HandleOpinionFirstPageClick()
            MsgBox("You clicked First Page (Opinion)")
        End Sub

        Public Sub HandleOpinionLastPageClick()
            MsgBox("You clicked Last Page (Opinion)")
        End Sub

        ' ✅ Add this new method for the Close button
        Public Sub HandleCloseClick(form As Form)
            If form IsNot Nothing Then form.Close()
        End Sub

    End Class

End Namespace
