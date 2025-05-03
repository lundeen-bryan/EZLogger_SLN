' Namespace=EZLogger/Handlers
' Filename=PatientInfoHandler.vb
' !See Label Footer for notes

Imports System.Windows.Forms
Imports EZLogger.EZLogger.Models
Imports EZLogger.EZLogger.Views
Imports EZLogger.Helpers
Imports Microsoft.Office.Core
Imports Microsoft.Office.Interop.Word

Namespace Handlers

    Public Class PatientInfoHandler

        ''' <summary>
        ''' Handles the click event for saving a custom document property.
        ''' This method retrieves the property name and value from the view,
        ''' performs basic validation, and then saves or updates the property
        ''' in the active document.
        ''' </summary>
        ''' <param name="view">The UpdateInfoView instance containing the UI elements.</param>
        ''' <remarks>
        ''' This method uses the DocumentPropertyHelper to write the custom property
        ''' and displays success or error messages using MsgBoxHelper.
        ''' </remarks>
        Public Sub HandleSavePropertyClick(view As UpdateInfoView)
            Try
                Dim propName As String = view.TxbxPropertyName.Text.Trim()
                Dim propValue As String = view.TxtbxPropertyValue.Text.Trim()

                ' Basic validation
                If String.IsNullOrWhiteSpace(propName) Then
                    MsgBoxHelper.Show("Please enter a property name.")
                    Return
                End If

                Dim doc As Document = Globals.ThisAddIn.Application.ActiveDocument

                ' Use helper to write or update the property
                DocumentPropertyHelper.WriteCustomProperty(doc, propName, propValue)

                MsgBoxHelper.Show($"Property '{propName}' was saved successfully.")

            Catch ex As Exception
                MsgBoxHelper.Show("Failed to save the property: " & ex.Message)
            End Try
        End Sub

        ''' <summary>
        ''' Handles the click event for adding or editing patient information.
        ''' This method retrieves the selected entry from the data grid, reads custom document properties,
        ''' and opens a new form for updating patient information.
        ''' </summary>
        ''' <param name="view">The PatientInfoView instance containing the UI elements and data grid.</param>
        ''' <remarks>
        ''' This method performs the following actions:
        ''' 1. Retrieves the selected entry from the data grid.
        ''' 2. Reads custom document properties for patient name and number.
        ''' 3. Creates and shows a new UpdateInfoHost form with an UpdateInfoView.
        ''' 4. Populates the new form with the selected entry's data (if available) and patient information.
        ''' 5. Displays an error message if the form fails to open.
        ''' </remarks>
        Public Sub HandleAddEditClick(view As PatientInfoView)
            Try
                ' Selected row in the grid
                Dim selectedEntry = TryCast(view.DataGridPtInfo.SelectedItem, DocPropertyEntry)

                ' Pull from the current Word document
                Dim doc As Document = Globals.ThisAddIn.Application.ActiveDocument
                Dim patientName As String = ""
                Dim patientNumber As String = ""

                ' Try to read custom doc properties
                For Each prop As DocumentProperty In doc.CustomDocumentProperties
                    If prop.Name.Equals("Patient Name", StringComparison.OrdinalIgnoreCase) Then
                        patientName = TryCast(prop.Value, String)
                    ElseIf prop.Name.Equals("Patient Number", StringComparison.OrdinalIgnoreCase) Then
                        patientNumber = TryCast(prop.Value, String)
                    End If
                Next

                ' Create and show the UpdateInfoHost and View
                Dim hostForm As New UpdateInfoHost()
                Dim updateView As New UpdateInfoView(hostForm)

                ' Assign to host form
                hostForm.ElementHost1.Child = updateView

                ' Set values
                If selectedEntry IsNot Nothing Then
                    updateView.InitialPropertyName = selectedEntry.PropertyName
                    updateView.InitialPropertyValue = selectedEntry.Value
                End If

                ' Set patient name and number for the new view
                updateView.InitialPatientName = patientName
                updateView.InitialPatientNumber = patientNumber

                ' Show form
                hostForm.Show()

            Catch ex As Exception
                MsgBoxHelper.Show("Failed to open Update Info form: " & ex.Message)
            End Try
        End Sub

        ''' <summary>
        ''' Loads custom document properties from the active document and displays them in the PatientInfoView.
        ''' </summary>
        ''' <param name="view">The PatientInfoView instance where the properties will be displayed.</param>
        ''' <remarks>
        ''' This method performs the following actions:
        ''' 1. Retrieves custom document properties from the active document.
        ''' 2. Formats specific date fields to MM/dd/yyyy format.
        ''' 3. Creates a list of DocPropertyEntry objects from the properties.
        ''' 4. Sets the ItemsSource of the DataGridPtInfo in the view to the list of properties.
        ''' 5. Displays an error message if loading fails.
        ''' </remarks>
        Public Sub LoadCustomDocProperties(view As PatientInfoView)
            Try
                Dim doc As Document = Globals.ThisAddIn.Application.ActiveDocument
                Dim properties As New List(Of DocPropertyEntry)

                ' Fields that should be displayed in MM/dd/yyyy format
                Dim dateFields As HashSet(Of String) = New HashSet(Of String)(StringComparer.OrdinalIgnoreCase) From {
                    "DOB", "Commitment", "Admission", "Expiration", "TCAR Referral Date"
                }

                For Each prop As DocumentProperty In doc.CustomDocumentProperties
                    Dim value As String = TryCast(prop.Value, String)

                    ' Try to format only if the field is a known date field
                    If dateFields.Contains(prop.Name) Then
                        Dim parsedDate As DateTime
                        If DateTime.TryParse(value, parsedDate) Then
                            value = parsedDate.ToString("MM/dd/yyyy")
                        End If
                    End If

                    properties.Add(New DocPropertyEntry With {
                        .PropertyName = prop.Name,
                        .Value = value
                    })
                Next

                view.DataGridPtInfo.ItemsSource = properties

            Catch ex As Exception
                MessageBox.Show("Unable to load document properties: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        Public Sub HandleRefreshClick(view As PatientInfoView)
            LoadCustomDocProperties(view)
        End Sub

        ''' <summary>
        ''' Handles the click event for validating custom document properties.
        ''' </summary>
        ''' <param name="view">The PatientInfoView instance containing the UI elements.</param>
        ''' <remarks>
        ''' This method performs the following actions:
        ''' 1. Validates required custom properties using DocumentPropertyValidator.
        ''' 2. Refreshes the list of custom properties in the view.
        ''' 3. Displays a success message if validation is successful.
        ''' 4. Displays an error message if an exception occurs during validation.
        ''' </remarks>
        Public Sub HandleValidateClick(view As PatientInfoView)
            Try
                ' Step 1: Validate custom properties
                DocumentPropertyValidator.ValidateRequiredCustomProperties()

                ' Step 2: Optionally refresh the listbox after adding missing properties
                LoadCustomDocProperties(view)

                MessageBox.Show("Custom properties validated successfully.")

            Catch ex As Exception
                MessageBox.Show("Error during validation: " & ex.Message)
            End Try
        End Sub

        ''' <summary>
        ''' Handles the click event for deleting a selected custom document property.
        ''' </summary>
        ''' <param name="view">The PatientInfoView instance containing the UI elements and data grid.</param>
        ''' <remarks>
        ''' This method performs the following actions:
        ''' 1. Retrieves the selected entry from the data grid.
        ''' 2. If no entry is selected, displays a message to the user.
        ''' 3. Deletes the selected custom property using DocumentPropertyHelper.
        ''' 4. Refreshes the list of custom properties in the view.
        ''' 5. Displays an error message if deletion fails.
        ''' </remarks>
        Public Sub HandleDeleteClick(view As PatientInfoView)
            Try
                Dim selectedEntry = TryCast(view.DataGridPtInfo.SelectedItem, DocPropertyEntry)
                If selectedEntry Is Nothing Then
                    MsgBoxHelper.Show("Please select a property to delete.")
                    Return
                End If

                DocumentPropertyHelper.DeleteCustomProperty(selectedEntry.PropertyName)
                LoadCustomDocProperties(view) ' Refresh list

            Catch ex As Exception
                MsgBoxHelper.Show("Failed to delete selected property: " & ex.Message)
            End Try
        End Sub

        ''' <summary>
        ''' Handles the click event for deleting all custom document properties.
        ''' </summary>
        ''' <param name="view">The PatientInfoView instance containing the UI elements and data grid.</param>
        ''' <remarks>
        ''' This method performs the following actions:
        ''' 1. Displays a confirmation dialog to the user.
        ''' 2. If confirmed, deletes all custom document properties using DocumentPropertyHelper.
        ''' 3. Refreshes the list of custom properties in the view.
        ''' 4. Displays an error message if deletion fails.
        ''' </remarks>
        Public Sub HandleDeleteAllClick(view As PatientInfoView)
            Try
                Dim confirm = MessageBox.Show("Are you sure you want to delete all custom document properties?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
                If confirm = DialogResult.Yes Then
                    DocumentPropertyHelper.DeleteAllCustomProperties()
                    LoadCustomDocProperties(view) ' Refresh list
                End If
            Catch ex As Exception
                MsgBoxHelper.Show("Failed to delete all properties: " & ex.Message)
            End Try
        End Sub

        ''' <summary>
        ''' Handles the click event for copying the selected property value to the clipboard.
        ''' </summary>
        ''' <param name="view">The PatientInfoView instance containing the UI elements and data grid.</param>
        ''' <remarks>
        ''' This method performs the following actions:
        ''' 1. Retrieves the selected entry from the data grid.
        ''' 2. If an entry is selected and has a non-empty value, copies the value to the clipboard.
        ''' 3. Displays a message indicating the copied value or that no value was selected.
        ''' </remarks>
        Public Sub HandleCopyClick(view As PatientInfoView)
            Dim selectedEntry = TryCast(view.DataGridPtInfo.SelectedItem, DocPropertyEntry)

            If selectedEntry IsNot Nothing AndAlso Not String.IsNullOrWhiteSpace(selectedEntry.Value) Then
                Clipboard.SetText(selectedEntry.Value)
                MsgBoxHelper.Show("Copied: " & selectedEntry.Value)
            Else
                MsgBoxHelper.Show("No value selected to copy.")
            End If
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

        Public Sub HandleCloseClick(form As Form)
            If form IsNot Nothing Then form.Close()
        End Sub

    End Class

End Namespace

' Footer:
''===========================================================================================
'' Filename: .......... PatientInfoHandler.vb
'' Description: ....... Handles button clicks from PatientInfoView
'' Created: ........... 2025-05-02
'' Updated: ........... 2025-05-02
'' Installs to: ....... EZLogger/Handlers
'' Compatibility: ..... VSTO, WPF
'' Contact Author: .... lundeen-bryan
'' Copyright:  ........ ©2025. All rights reserved.
'' Notes: ............. _
' (1) notes_here
''===========================================================================================