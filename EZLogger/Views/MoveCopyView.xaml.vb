Imports System.Windows
Imports System.Windows.Controls
Imports EZLogger.Helpers
Imports EZLogger.Enums

Namespace EZLogger.Views

    Partial Public Class MoveCopyView
        Inherits UserControl

        Public Sub New()
            InitializeComponent()

            AddHandler BtnSearchPatientId.Click, AddressOf BtnSearchPatientId_Click
        End Sub

        Private Sub BtnSearchPatientId_Click(sender As Object, e As RoutedEventArgs)
            Dim config As New MessageBoxConfig With {
                .Message = "You pressed the search button.",
                .ShowOk = True
            }

            CustomMsgBoxHandler.Show(config)
        End Sub

    End Class

End Namespace
