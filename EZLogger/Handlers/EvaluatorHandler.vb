Imports System.Windows
Imports System.Windows.Forms
Imports EZLogger.Helpers
Imports Microsoft.Office.Interop.Word
Imports System.Windows.Controls
Imports System.Windows.RoutedEventArgs

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

        Public Sub HandleDoneSelectingClick(view As EvaluatorView)
            Try
                Dim selectedEvaluator As String = TryCast(view.AuthorCbo.SelectedItem, String)

                If String.IsNullOrWhiteSpace(selectedEvaluator) Then
                    MsgBoxHelper.Show("Please select an evaluator before saving.")
                    Return
                End If

                Dim doc As Document = Globals.ThisAddIn.Application.ActiveDocument
                DocumentPropertyHelper.WriteCustomProperty(doc, "Evaluator", selectedEvaluator)

                MsgBoxHelper.Show($"Evaluator '{selectedEvaluator}' saved successfully.")

            Catch ex As Exception
                MsgBoxHelper.Show("Error saving evaluator: " & ex.Message)
            End Try
        End Sub

        Public Sub RegisterKeyboardShortcuts(shortcutHelper As ShortcutHandler, view As EvaluatorView)
            ' Control+F → First Page
            shortcutHelper.RegisterShortcut(Keys.F, Keys.Control, Sub()
                                                                      view.BtnAuthorFirstPage.RaiseEvent(New RoutedEventArgs(System.Windows.Controls.Button.ClickEvent))
                                                                  End Sub)

            ' Control+L → Last Page
            shortcutHelper.RegisterShortcut(Keys.L, Keys.Control, Sub()
                                                                      view.BtnAuthorLastPage.RaiseEvent(New RoutedEventArgs(System.Windows.Controls.Button.ClickEvent))
                                                                  End Sub)

            ' Control+S → Save Selection
            shortcutHelper.RegisterShortcut(Keys.S, Keys.Control, Sub()
                                                                      view.BtnAuthorDone.RaiseEvent(New RoutedEventArgs(System.Windows.Controls.Button.ClickEvent))
                                                                  End Sub)

            ' Control+D → Done (mark checkbox and close)
            shortcutHelper.RegisterShortcut(Keys.D, Keys.Control, Sub()
                                                                      view.DoneBtn.RaiseEvent(New RoutedEventArgs(System.Windows.Controls.Button.ClickEvent))
                                                                  End Sub)
        End Sub

    End Class
End Namespace