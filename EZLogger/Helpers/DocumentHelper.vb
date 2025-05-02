Imports System.Windows
Imports System.Windows.Forms
Imports System.Windows.Media
Imports System.Windows.Controls
Imports Microsoft.Office.Interop.Word
Imports Application = Microsoft.Office.Interop.Word.Application

''' <summary>
''' Provides helper methods for saving and closing the active Word document without user prompts.
''' </summary>
Public Module DocumentHelper

    ''' <summary>
    ''' Attempts to silently save the active Word document.
    ''' Will retry up to a defined number of times if save fails.
    ''' </summary>
    ''' <param name="maxRetries">Maximum number of save attempts (default is 10).</param>
    Public Sub TrySaveActiveDocument(Optional maxRetries As Integer = 10)
        Dim app As Application = Globals.ThisAddIn.Application
        Dim doc As Document = app.ActiveDocument

        Dim attempts As Integer = 0

        Do While Not doc.Saved AndAlso attempts < maxRetries
            Try
                doc.Save()
            Catch ex As Exception
                ' Optional: Log error or display debug info
            End Try
            attempts += 1
        Loop
    End Sub

    ''' <summary>
    ''' Closes the active Word document with optional prompt to user.
    ''' Defaults to silent close without prompting to save.
    ''' </summary>
    ''' <param name="showPrompt">
    ''' If True, user is asked whether to save changes.
    ''' If False, the document is closed without any dialog.
    ''' </param>
    Public Sub CloseActiveDocument(Optional showPrompt As Boolean = False)
        Dim app As Application = Globals.ThisAddIn.Application

        ' Check if there is an active document
        If app.Documents.Count = 0 Then
            MsgBoxHelper.Show("There is no active document to close.")
            Exit Sub
        End If

        Dim doc As Document = app.ActiveDocument

        Try
            ' Check if the document has been previously saved
            Dim isPreviouslySaved As Boolean = Not String.IsNullOrWhiteSpace(doc.Path)

            If Not showPrompt Then
                If isPreviouslySaved Then
                    ' Attempt to save silently first
                    TrySaveActiveDocument()
                Else
                    ' Close the document without prompting
                    doc.Close(SaveChanges:=WdSaveOptions.wdDoNotSaveChanges)
                End If
            Else
                ' Show default save prompt behavior
                doc.Close(SaveChanges:=WdSaveOptions.wdPromptToSaveChanges)
            End If

        Catch ex As Exception
            MsgBoxHelper.Show("The document could not be closed: " & ex.Message)
        End Try
    End Sub

    ''' <summary>
    ''' Saves the active Word document as a .docx file to the specified path.
    ''' </summary>
    ''' <param name="destinationPath">Full path where the .docx file should be saved.</param>
    Public Sub SaveActiveDocumentAsDocx(destinationPath As String)
        Dim app As Application = Globals.ThisAddIn.Application
        Dim doc As Document = app.ActiveDocument

        Try
            If doc IsNot Nothing Then
                doc.SaveAs2(FileName:=destinationPath, FileFormat:=WdSaveFormat.wdFormatXMLDocument)
            End If
        Catch ex As Exception
            MsgBoxHelper.Show("Error saving document as Word file: " & ex.Message)
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

End Module