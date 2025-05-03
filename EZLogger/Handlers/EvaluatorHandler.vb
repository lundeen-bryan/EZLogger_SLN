' Namespace=EZLogger/Handlers
' Filename=EvaluatorHandler.vb
' !See Label Footer for notes

Imports EZLogger.Helpers
Imports Microsoft.Office.Interop.Word
Imports System.Windows
Imports System.Windows.Forms

Namespace Handlers
    ''' <summary>
    ''' Adds a new author to the list of doctors
    ''' </summary>
    ''' <remarks>Config manager also adds authors</remarks>
    Public Class EvaluatorHandler ' Example: ConfigViewHandler

        ''' <summary>
        ''' Adds a new author to the doctors list
        ''' </summary>
        ''' <remarks>See config manager where you can also add authors/doctors</remarks>
        Public Sub HandleAddAuthorClick()
            MsgBox("You clicked Add New Author")
        End Sub

        ''' <summary>
        ''' Handles the close action for a given form.
        ''' </summary>
        ''' <param name="form">The Form object to be closed.</param>
        ''' <remarks>
        ''' This method checks if the provided form is not null before attempting to close it.
        ''' If the form is null, no action is taken.
        ''' </remarks>
        Public Sub HandleCloseClick(hostForm As Form)
            hostForm?.Close()
        End Sub

        ''' <summary>
        ''' Handles the action when the user finishes selecting an evaluator.
        ''' This method saves the selected evaluator as a custom property in the active document.
        ''' </summary>
        ''' <param name="view">The EvaluatorView instance containing the UI elements, including the combo box with the selected evaluator.</param>
        ''' <remarks>
        ''' This method performs the following actions:
        ''' 1. Retrieves the selected evaluator from the combo box.
        ''' 2. Validates that an evaluator has been selected.
        ''' 3. Saves the selected evaluator as a custom property in the active document.
        ''' 4. Displays a success message if the evaluator is saved successfully.
        ''' 5. Displays an error message if any exception occurs during the process.
        ''' </remarks>
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

        ''' <summary>
        ''' Navigates to the first page of the active document.
        ''' </summary>
        ''' <remarks>
        ''' This method attempts to move the cursor to the beginning of the first page in the currently active document.
        ''' If an error occurs during this process, it displays an error message to the user.
        ''' </remarks>
        ''' <exception cref="Exception">
        ''' Thrown when there's an error in navigating to the first page. The error message is displayed to the user.
        ''' </exception>
        Public Sub HandleFirstPageClick()
            Try
                Dim doc As Document = Globals.ThisAddIn.Application.ActiveDocument
                Dim sel As Selection = Globals.ThisAddIn.Application.Selection
                sel.GoTo(What:=WdGoToItem.wdGoToPage, Name:="1")
            Catch ex As Exception
                MsgBoxHelper.Show("Could not go to first page: " & ex.Message)
            End Try
        End Sub

        ''' <summary>
        ''' Navigates to the last page of the active document.
        ''' </summary>
        ''' <remarks>
        ''' This method attempts to move the cursor to the beginning of the last page in the currently active document.
        ''' It first calculates the total number of pages in the document and then uses that information to navigate.
        ''' If an error occurs during this process, it displays an error message to the user.
        ''' </remarks>
        ''' <exception cref="Exception">
        ''' Thrown when there's an error in navigating to the last page. The error message is displayed to the user.
        ''' </exception>
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

        ''' <summary>
        ''' Opens the Evaluator View by creating and showing a new EvaluatorHost instance.
        ''' </summary>
        ''' <remarks>
        ''' This method is typically called when the user requests to open the Evaluator View,
        ''' such as clicking a button or selecting a menu item.
        ''' </remarks>
        Public Sub OnOpenEvaluatorViewClick()
            Dim host As New EvaluatorHost()
            host.Show()
        End Sub

        ''' <summary>
        ''' Registers keyboard shortcuts for various actions in the EvaluatorView.
        ''' </summary>
        ''' <param name="shortcutHelper">The ShortcutHandler object used to register the shortcuts.</param>
        ''' <param name="view">The EvaluatorView instance containing the UI elements to be triggered by the shortcuts.</param>
        ''' <remarks>
        ''' This method sets up the following keyboard shortcuts:
        ''' - Control+F: Navigate to the first page
        ''' - Control+L: Navigate to the last page
        ''' - Control+S: Save the current selection
        ''' - Control+D: Mark as done and close the view
        ''' Each shortcut is associated with a corresponding button click event in the EvaluatorView.
        ''' </remarks>
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

' Footer:
''===========================================================================================
'' Filename: .......... EvaluatorHandler.vb
'' Description: ....... Manages interaction for the EvaluatorView interface
'' Created: ........... 2025-05-02
'' Updated: ........... 2025-05-02
'' Installs to: ....... EZLogger/Handlers
'' Compatibility: ..... Excel,Word,etc.
'' Contact Author: .... author
'' Copyright:  ........ ©2025. All rights reserved.
'' Notes: ............. _
' (1) notes_here
''===========================================================================================