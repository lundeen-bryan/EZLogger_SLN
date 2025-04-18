' NavigationHelper.vb
Namespace Helpers
    Public Module NavigationHelper

        ''' <summary>
        ''' Navigates the Word application to the first page of the document.
        ''' </summary>
        Public Sub GoToFirstPage(wordApp As Word.Application)
            Try
                wordApp.Selection.GoTo(What:=Word.WdGoToItem.wdGoToPage, Which:=Word.WdGoToDirection.wdGoToFirst)
            Catch ex As Exception
                MsgBoxHelper.Show("Error navigating to first page: " & ex.Message)
            End Try
        End Sub

        ''' <summary>
        ''' Navigates the Word application to the last page of the document.
        ''' </summary>
        Public Sub GoToLastPage(wordApp As Word.Application)
            Try
                wordApp.Selection.GoTo(What:=Word.WdGoToItem.wdGoToPage, Which:=Word.WdGoToDirection.wdGoToLast)
            Catch ex As Exception
                MsgBoxHelper.Show("Error navigating to last page: " & ex.Message)
            End Try
        End Sub

    End Module
End Namespace
