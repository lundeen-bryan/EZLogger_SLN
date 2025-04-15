Imports System.Windows
Imports System.Windows.Forms

Namespace Handlers
    Public Class OpinionHandler

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

        Public Sub HandleCloseClick(form As Form)
            If form IsNot Nothing Then form.Close()
        End Sub

    End Class
End Namespace

