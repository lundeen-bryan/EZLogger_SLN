' Namespace=EZLogger/Handlers
' Filename=SaveFileHandler.vb
' !See Label Footer for notes

Imports EZLogger.Helpers
Imports System.Globalization
Imports System.Windows
Imports System.Windows.Forms
Imports Microsoft.Office.Interop.Word
Imports MessageBox = System.Windows.MessageBox

Namespace Handlers
    Public Class SaveFileHandler

        Private _moveRootPath As String
        Private _copyRootPath As String

        ''' <summary>
        ''' Loads Move and Copy root folder paths from local_user_config.json.
        ''' Shows warning if critical paths are missing.
        ''' </summary>
        Public Sub LoadRootPaths()
            Try
                ' Read Move (all_penal_codes) path
                _moveRootPath = ConfigHelper.GetGlobalConfigValue("cdo_filepath", "all_penal_codes")

                ' Read Copy (user_forensic_library) path
                _copyRootPath = ConfigHelper.GetLocalConfigValue("sp_filepath", "user_forensic_library")

                ' Validate both paths
                If String.IsNullOrWhiteSpace(_moveRootPath) OrElse String.IsNullOrWhiteSpace(_copyRootPath) Then
                    MsgBoxHelper.Show("Your file move/copy paths are missing. Please recreate your local config file to proceed.")
                End If

            Catch ex As Exception
                MsgBoxHelper.Show("Failed to load folder paths: " & ex.Message)
            End Try
        End Sub

        ''' <summary>
        ''' Handles the Save As operation: opens Save As dialog, saves the file,
        ''' and deletes the old file if moving.
        ''' </summary>
        Public Sub HandleSaveAsClick(view As SaveFileView)
            Try
                ' Validate user selection
                If Not (view.RadioMove.IsChecked Or view.RadioCopy.IsChecked) Then
                    MsgBoxHelper.Show("Please select Move or Copy before saving.")
                    Exit Sub
                End If

                ' Build the suggested full path (full file path we want to suggest)
                Dim suggestedPath As String = BuildDestinationPath(view)
                If String.IsNullOrWhiteSpace(suggestedPath) Then
                    MsgBoxHelper.Show("Cannot generate the destination path. Make sure all fields are filled correctly.")
                    Exit Sub
                End If

                ' Set initial folder from root config, and filename separately
                Dim initialFolder As String
                If view.RadioMove.IsChecked Then
                    initialFolder = _moveRootPath
                ElseIf view.RadioCopy.IsChecked Then
                    initialFolder = _copyRootPath
                Else
                    initialFolder = "" ' Should never happen due to earlier validation
                End If

                Dim initialFilename As String = System.IO.Path.GetFileName(suggestedPath)

                ' Open Save As dialog
                Dim saveDialog As New SaveFileDialog With {
                    .Title = "Save As...",
                    .InitialDirectory = initialFolder,
                    .FileName = initialFilename,
                    .Filter = "Word Documents (*.docx)|*.docx|All Files (*.*)|*.*"
                }

                If saveDialog.ShowDialog() = DialogResult.OK Then
                    Dim savePath As String = saveDialog.FileName

                    ' Capture old file path before SaveAs
                    Dim doc As Document = Globals.ThisAddIn.Application.ActiveDocument
                    Dim oldFilePath As String = doc.FullName

                    ' NEW: Set built-in document properties before saving
                    MetadataHelper.SaveBuiltProperties(
                        patientName:=view.LblPatientName.Content?.ToString(),
                        reportType:=view.ReportTypeCbo.Text,
                        reportDate:=view.ReportDatePicker.SelectedDate?.ToString(),
                        program:=view.LblProgram.Content?.ToString(),
                        unit:=view.LblUnit.Content?.ToString(),
                        evaluator:=DocumentPropertyHelper.GetPropertyValue("Evaluator"),
                        processedBy:=DocumentPropertyHelper.GetPropertyValue("Processed By"),
                        county:=DocumentPropertyHelper.GetPropertyValue("County")
                    )

                    ' Save to new location
                    doc.SaveAs2(FileName:=savePath)

                    ' Delete the old file if moving
                    If view.RadioMove.IsChecked Then
                        TryDeleteOldFile(oldFilePath)
                    End If

                    ' Show success message
                    Dim message As String = If(view.RadioMove.IsChecked,
                                       "The file was successfully moved and the original file was deleted.",
                                       "The file was successfully copied.")
                    MsgBoxHelper.Show(message)
                End If

            Catch ex As Exception
                MsgBoxHelper.Show("Failed to save file: " & ex.Message)
            End Try
        End Sub



        ''' <summary>
        ''' Attempts to delete the old file after a successful Move operation.
        ''' </summary>
        Private Sub TryDeleteOldFile(oldFilePath As String)
            Try
                ' Extra safety: handle legacy .doc files
                If System.IO.File.Exists(oldFilePath) Then
                    System.IO.File.Delete(oldFilePath)
                Else
                    ' If old file was .doc and SaveAs created .docx, try deleting the .doc version
                    Dim possibleDocPath As String = System.IO.Path.ChangeExtension(oldFilePath, ".doc")
                    If System.IO.File.Exists(possibleDocPath) Then
                        System.IO.File.Delete(possibleDocPath)
                    End If
                End If
            Catch ex As Exception
                MsgBoxHelper.Show("The original file could not be deleted. Please delete it manually later.")
            End Try
        End Sub


        ''' <summary>
        ''' Handles the ShowPath button click.
        ''' Builds the destination file path, displays it in the view, and copies folder name to clipboard.
        ''' </summary>
        Public Sub HandleShowPathClick(view As SaveFileView)
            Try
                ' Validate user selection
                If Not (view.RadioMove.IsChecked Or view.RadioCopy.IsChecked) Then
                    MsgBoxHelper.Show("Please select Move or Copy before generating the file path.")
                    Exit Sub
                End If

                ' Build the destination path
                Dim destinationPath As String = BuildDestinationPath(view)

                ' Display the new path in the TextBlock
                view.NewFileNameTextBlock.Text = destinationPath

                ' Copy folder name (patient name) to clipboard
                Dim patientName = view.LblPatientName.Content?.ToString()
                If Not String.IsNullOrWhiteSpace(patientName) Then
                    ClipboardHelper.CopyText(patientName)
                End If

            Catch ex As Exception
                MsgBoxHelper.Show("Failed to generate file path: " & ex.Message)
            End Try
        End Sub

        ''' <summary>
        ''' Builds the full destination file path based on patient name, report type, and report date.
        ''' </summary>
        Private Function BuildDestinationPath(view As SaveFileView) As String
            ' Base path depending on Move/Copy choice
            Dim basePath As String
            If view.RadioMove.IsChecked Then
                basePath = _moveRootPath
            ElseIf view.RadioCopy.IsChecked Then
                basePath = _copyRootPath
            Else
                Return String.Empty
            End If

            ' Pull data from view
            Dim Lname As String = DocumentPropertyHelper.GetPropertyValue("Lastname")
            Dim Fname As String = DocumentPropertyHelper.GetPropertyValue("Firstname").ToLower()
            Dim textInfo As TextInfo = CultureInfo.CurrentCulture.TextInfo
            Dim patientName As String = Lname & ", " & textInfo.ToTitleCase(Fname)
            Dim reportType As String = view.ReportTypeCbo.Text
            Dim reportDate As Date? = view.ReportDatePicker.SelectedDate

            ' Validate minimal fields
            If String.IsNullOrWhiteSpace(patientName) OrElse String.IsNullOrWhiteSpace(reportType) OrElse Not reportDate.HasValue Then
                Return String.Empty
            End If

            ' Build folder based on first letter of patient name
            Dim firstLetter As String = If(String.IsNullOrEmpty(patientName), "", patientName.Substring(0, 1).ToUpper())

            ' Format date as yyyy-MM-dd
            Dim formattedDate As String = reportDate.Value.ToString("yyyy-MM-dd")

            ' Combine filename
            Dim filename As String = $"{patientName} {reportType} {formattedDate}.docx"

            ' Build full path
            Dim fullPath As String = System.IO.Path.Combine(basePath, firstLetter, filename)

            Return fullPath
        End Function

        ''' <summary>
        ''' Displays a message box as a placeholder for save file logic.
        ''' </summary>
        Public Sub ShowSaveMessage()
            Dim host As New SaveFileHost()
            host.Show()
        End Sub
        Public Sub HandleCloseClick(hostForm As Form)
            hostForm?.Close()
        End Sub

        ''' <summary>
        ''' Loads Word document properties and populates controls in SaveFileView.
        ''' </summary>
        Public Sub HandleSearchPatientIdClick(view As SaveFileView)
            Try
                Dim GetProp = Function(name As String) DocumentPropertyHelper.GetPropertyValue(name)

                view.TxtPatientId.Text = GetProp("Patient Number")

                Dim reportType As String = GetProp("Report Type")
                If view.ReportTypeCbo.Items.Contains(reportType) Then
                    view.ReportTypeCbo.SelectedItem = reportType
                End If

                Dim dateStr = GetProp("Report Date")
                If Date.TryParse(dateStr, Nothing) Then
                    view.ReportDatePicker.SelectedDate = Date.Parse(dateStr)
                Else
                    view.ReportDatePicker.SelectedDate = Nothing
                End If

                view.LblPatientName.Content = GetProp("Patient Name")
                view.LblProgram.Content = GetProp("Program")
                view.LblUnit.Content = GetProp("Unit")
                view.LblClassification.Content = GetProp("Classification")

                ' Set the unique document id here in the custom document properties
                Dim uniqueDocumentId As String = DocumentPropertyHelper.CreateUniqueIdFromProperties()
                Dim doc As Word.Document = WordAppHelper.GetWordApp().ActiveDocument
                DocumentPropertyHelper.WriteCustomProperty(doc, "Unique ID", uniqueDocumentId)

            Catch ex As Exception
                MessageBox.Show("Failed to load document properties: " & ex.Message,
                                    "Error", MessageBoxButton.OK, MessageBoxImage.Error)
            End Try
        End Sub
    End Class
End Namespace

' Footer:
''===========================================================================================
'' Filename: .......... SaveFileHandler.vb
'' Description: ....... Provides logic for save-related operations in EZLogger.
'' Created: ........... 2025-05-02
'' Updated: ........... 2025-05-02
'' Installs to: ....... EZLogger/Handlers
'' Compatibility: ..... VSTO, WPF
'' Contact Author: .... lundeen-bryan
'' Copyright:  ........ ©2025. All rights reserved.
'' Notes: ............. _
' (1) notes_here
''===========================================================================================