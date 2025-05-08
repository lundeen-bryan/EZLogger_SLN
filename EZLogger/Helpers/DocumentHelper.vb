Imports System.Windows
Imports System.Windows.Forms
Imports System.Windows.Media
Imports System.Windows.Controls
Imports Word = Microsoft.Office.Interop.Word
Imports Application = Microsoft.Office.Interop.Word.Application
Imports EZLogger.Helpers

''' <summary>
''' Provides helper methods for interacting with the active Word document.
''' </summary>
Public Module DocumentHelper

    ''' <summary>
    ''' Safely returns the active Word document if available; otherwise returns Nothing.
    ''' Uses TryCast to avoid casting errors and logs any unexpected failures.
    ''' </summary>
    Public Function GetActiveWordDocument() As Microsoft.Office.Interop.Word.Document
        Const functionName As String = "WordAppHelper.GetActiveWordDocument"

        Try
            Dim app As Application = TryCast(Globals.ThisAddIn.Application, Microsoft.Office.Interop.Word.Application)
            If app Is Nothing Then Return Nothing

            Return TryCast(app.ActiveDocument, Microsoft.Office.Interop.Word.Document)

        Catch ex As Exception
            Dim errNum As String = ex.HResult.ToString()
            Dim errMsg As String = ex.Message
            Dim recommendation As String = "Please close and reopen the Word document, then try again."

            ErrorHelper.HandleError(functionName, errNum, errMsg, recommendation)
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Attempts to silently save the active Word document.
    ''' Will retry up to a defined number of times if save fails.
    ''' </summary>
    ''' <param name="maxRetries">Maximum number of save attempts (default is 10).</param>
    Public Sub TrySaveActiveDocument(Optional maxRetries As Integer = 10)
        Dim doc As Word.Document = GetActiveWordDocument()
        If doc Is Nothing Then Exit Sub

        Dim attempts As Integer = 0
        Do While Not doc.Saved AndAlso attempts < maxRetries
            Try
                doc.Save()
            Catch ex As Exception
                ' Optional: Log error
            End Try
            attempts += 1
        Loop
    End Sub

    ''' <summary>
    ''' Closes the active Word document with optional prompt to user.
    ''' Defaults to silent close without prompting to save.
    ''' </summary>
    ''' <param name="showPrompt">If True, user is asked whether to save changes.</param>
    Public Sub CloseActiveDocument(Optional showPrompt As Boolean = False)
        Dim doc As Word.Document = GetActiveWordDocument()
        If doc Is Nothing Then
            MsgBoxHelper.Show("There is no active document to close.")
            Exit Sub
        End If

        Try
            Dim isPreviouslySaved As Boolean = Not String.IsNullOrWhiteSpace(doc.Path)

            If Not showPrompt Then
                If isPreviouslySaved Then
                    TrySaveActiveDocument()
                Else
                    doc.Close(SaveChanges:=Word.WdSaveOptions.wdDoNotSaveChanges)
                End If
            Else
                doc.Close(SaveChanges:=Word.WdSaveOptions.wdPromptToSaveChanges)
            End If

        Catch ex As Exception
            MsgBoxHelper.Show("The document could not be closed: " & ex.Message)
        End Try

        ' ✅ Reset ReportWizardPanel if available
        Try
            Dim hostForm As ReportWizardTaskPaneContainer = Globals.ThisAddIn.ReportWizardTaskPaneContainer

            If hostForm IsNot Nothing AndAlso hostForm.ElementHost1 IsNot Nothing Then
                Dim wizardPanel = TryCast(hostForm.ElementHost1.Child, ReportWizardPanel)
                If wizardPanel IsNot Nothing Then
                    ResetAllControls(wizardPanel)
                End If
            End If
        Catch ex As Exception
            MsgBox("Could not reset Report Wizard panel: " & ex.Message)
        End Try
    End Sub

    Public Sub ResetAllControls(container As DependencyObject)
        For i As Integer = 0 To VisualTreeHelper.GetChildrenCount(container) - 1
            Dim child As DependencyObject = VisualTreeHelper.GetChild(container, i)

            Select Case True
                Case TypeOf child Is Controls.TextBox
                    CType(child, Controls.TextBox).Clear()

                Case TypeOf child Is Controls.Label
                    CType(child, Controls.Label).Content = ""

                Case TypeOf child Is Controls.CheckBox
                    CType(child, Controls.CheckBox).IsChecked = False

                Case TypeOf child Is Controls.ComboBox
                    CType(child, Controls.ComboBox).SelectedIndex = -1

                Case TypeOf child Is Controls.ListBox
                    CType(child, Controls.ListBox).UnselectAll()

                Case TypeOf child Is DatePicker
                    CType(child, DatePicker).SelectedDate = Nothing
            End Select

            ' Recurse into child elements
            ResetAllControls(child)
        Next
    End Sub

    ''' <summary>
    ''' Saves the active Word document as a .docx file to the specified path.
    ''' </summary>
    ''' <param name="destinationPath">Full path where the .docx file should be saved.</param>
    Public Sub SaveActiveDocumentAsDocx(destinationPath As String)
        Dim doc As Word.Document = GetActiveWordDocument()
        If doc Is Nothing Then
            MsgBoxHelper.Show("No active document found.")
            Exit Sub
        End If

        Try
            doc.SaveAs2(FileName:=destinationPath, FileFormat:=Word.WdSaveFormat.wdFormatXMLDocument)
        Catch ex As Exception
            MsgBoxHelper.Show("Error saving document as Word file: " & ex.Message)
        End Try
    End Sub

End Module
