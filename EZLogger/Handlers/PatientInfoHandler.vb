Imports System.Windows.Forms

Namespace Handlers

    Public Class PatientInfoHandler

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
