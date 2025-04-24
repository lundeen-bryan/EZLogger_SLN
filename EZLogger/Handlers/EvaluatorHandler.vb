Imports System.Windows.Forms
Imports Microsoft.Office.Interop.Word

Namespace Handlers
    Public Class EvaluatorHandler ' Example: ConfigViewHandler

        Public Sub OnOpenEvaluatorViewClick()
            Dim host As New EvaluatorHost()
            host.Show()
        End Sub
        Public Sub HandleCloseClick(form As Form)
            If form IsNot Nothing Then form.Close()
        End Sub
        Public Sub HandleAddAuthorClick()
            MsgBox("You clicked Add New Author")
        End Sub

        Public Sub HandleFirstPageClick()
            Try
                Dim doc As Document = Globals.ThisAddIn.Application.ActiveDocument
                Dim sel As Selection = Globals.ThisAddIn.Application.Selection
                sel.GoTo(What:=WdGoToItem.wdGoToPage, Name:="1")
            Catch ex As Exception
                MsgBoxHelper.Show("Could not go to first page: " & ex.Message)
            End Try
        End Sub

        Public Sub HandleLastPageClick()
            Try
                Dim doc As Document = Globals.ThisAddIn.Application.ActiveDocument
                Dim sel As Selection = Globals.ThisAddIn.Application.Selection
                Dim totalPages As Integer = doc.ComputeStatistics(WdStatistic.wdStatisticPages)
                sel.GoTo(What:=WdGoToItem.wdGoToPage, Name:=totalPages.ToString())
            Catch ex As Exception
                MsgBoxHelper.Show("Could not go to last page: " & ex.Message)
            End Try
        End Sub

        Public Sub HandleDoneSelectingClick()
            MsgBox("You clicked Done Selecting")
        End Sub


    End Class
End Namespace