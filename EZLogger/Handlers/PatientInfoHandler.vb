Imports System.Windows.Forms
Imports EZLogger.EZLogger.Models
Imports EZLogger.EZLogger.Views
Imports EZLogger.Helpers
Imports Microsoft.Office.Core
Imports Microsoft.Office.Interop.Word

Namespace Handlers

    Public Class PatientInfoHandler

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

        Public Sub HandleValidateClick()
            MsgBox("You clicked Validate Fields")
        End Sub

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

        Public Sub HandleAddEditClick()
            MsgBox("You clicked Add/Edit")
        End Sub

        Public Sub HandleCopyClick()
            MsgBox("You clicked Copy")
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

    End Class

End Namespace
