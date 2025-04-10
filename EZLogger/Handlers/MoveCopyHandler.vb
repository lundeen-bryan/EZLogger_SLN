﻿Imports System.Windows
Imports System.Windows.Forms

Namespace Handlers
    Public Class MoveCopyHandler

        Public Sub OnMoveCopyClick()
            Dim host As New MoveCopyHost()
            host.TopMost = True
            FormPositionHelper.MoveFormToTopLeftOfAllScreens(host)
            host.Show()
        End Sub

        Public Sub HandleSearchClick(ownerForm As Form)
            Dim config As New MessageBoxConfig With {
                .Message = "You pressed the search button.",
                .ShowOk = True
            }

            CustomMsgBoxHandler.Show(config, ownerForm)
        End Sub

        Public Sub HandleSaveAsClick(ownerForm As Form)
            Dim config As New MessageBoxConfig With {
                .Message = "This would normally open a Save As dialog.",
                .ShowOk = True
            }

            CustomMsgBoxHandler.Show(config, ownerForm)
        End Sub

    End Class
End Namespace

