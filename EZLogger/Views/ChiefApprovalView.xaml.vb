Imports System.Windows
Imports System.Windows.Controls
Imports EZLogger.HostForms
Imports EZLogger.Handlers ' Make sure this matches your actual namespace and folder

Namespace EZLogger.Views
    Partial Public Class ChiefApprovalView
        Inherits UserControl

        Private ReadOnly _handler As ChiefApprovalHandler

        Public Sub New()
            InitializeComponent()

            ' Create instance of the handler
            _handler = New ChiefApprovalHandler()

            ' Wire up the buttons
            AddHandler BtnApproval.Click, AddressOf BtnApproval_Click
            AddHandler BtnSignature.Click, AddressOf BtnSignature_Click
        End Sub

        Private Sub BtnApproval_Click(sender As Object, e As RoutedEventArgs)
            _handler.HandleApprovalClick()
        End Sub

        Private Sub BtnSignature_Click(sender As Object, e As RoutedEventArgs)
            _handler.HandleSignatureClick()
        End Sub
    End Class
End Namespace

