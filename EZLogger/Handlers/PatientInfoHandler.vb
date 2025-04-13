Imports System.Windows.Forms
Imports EZLogger.EZLogger.Models
Imports EZLogger.EZLogger.Views
Imports Microsoft.Office.Core
Imports Microsoft.Office.Interop.Word

Namespace Handlers

    Public Class PatientInfoHandler

        Public Sub LoadCustomDocProperties(view As PatientInfoView)
            Try
                Dim doc As Document = Globals.ThisAddIn.Application.ActiveDocument
                Dim properties As New List(Of DocPropertyEntry)

                For Each prop As DocumentProperty In doc.CustomDocumentProperties
                    properties.Add(New DocPropertyEntry With {
                .PropertyName = prop.Name,
                .Value = prop.Value?.ToString()
            })
                Next

                view.DataGridPtInfo.ItemsSource = properties

            Catch ex As Exception
                MessageBox.Show("Unable to load document properties: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        Public Sub HandleCloseClick(form As Form)
            If form IsNot Nothing Then form.Close()
        End Sub

        Public Sub HandleRefreshClick()
            MsgBox("You clicked Refresh List")
        End Sub

        Public Sub HandleValidateClick()
            MsgBox("You clicked Validate Fields")
        End Sub

        Public Sub HandleDeleteClick()
            MsgBox("You clicked Delete")
        End Sub

        Public Sub HandleDeleteAllClick()
            MsgBox("You clicked Delete All")
        End Sub

        Public Sub HandleAddEditClick()
            MsgBox("You clicked Add/Edit")
        End Sub

        Public Sub HandleCopyClick()
            MsgBox("You clicked Copy")
        End Sub

        Public Sub HandleFirstPageClick()
            MsgBox("You clicked First Page")
        End Sub

        Public Sub HandleLastPageClick()
            MsgBox("You clicked Last Page")
        End Sub

    End Class

End Namespace
