Imports System.Windows
Imports System.Windows.Controls.UserControl
Imports System.Windows.Forms
Imports EZLogger.Helpers

Namespace EZLogger.Views

    Partial Public Class MoveCopyView
        Inherits System.Windows.Controls.UserControl ' ✅ Explicit WPF UserControl

        Private ReadOnly _hostForm As Form
        Private ReadOnly _handler As New Handlers.MoveCopyHandler()

        Public Sub New(Optional hostForm As Form = Nothing)
            InitializeComponent()
            _hostForm = hostForm

            AddHandler BtnSearchPatientId.Click, AddressOf BtnSearchPatientId_Click
            AddHandler BtnSaveAs.Click, AddressOf BtnSaveAs_Click
        End Sub

        Private Sub BtnSearchPatientId_Click(sender As Object, e As RoutedEventArgs)
            _handler.HandleSearchClick(_hostForm)
        End Sub

        Private Sub BtnSaveAs_Click(sender As Object, e As RoutedEventArgs)
            _handler.HandleSaveAsClick(_hostForm)
        End Sub

    End Class

End Namespace