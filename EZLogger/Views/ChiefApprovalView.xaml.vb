Imports System.Windows
Imports System.Windows.Controls
Imports System.Windows.Forms ' ✅ You need this for Form
Imports EZLogger.Handlers
Imports EZLogger.Helpers

Namespace EZLogger.Views

    Partial Public Class ChiefApprovalView
        Inherits Windows.Controls.UserControl

        Private ReadOnly _handler As ChiefApprovalHandler
        Private ReadOnly _hostForm As Form

        ' ✅ Single constructor with optional hostForm
        Public Sub New(Optional hostForm As Form = Nothing)
            InitializeComponent()

            _hostForm = hostForm
            _handler = New ChiefApprovalHandler()

            AddHandler BtnApproval.Click, AddressOf BtnApproval_Click
            AddHandler BtnSignature.Click, AddressOf BtnSignature_Click
            AddHandler BtnClose.Click, AddressOf BtnClose_Click
            ' Load combo data when the view is created
            AddHandler Me.Loaded, AddressOf ChiefApprovalView_Loaded

        End Sub

        Private Sub ChiefApprovalView_Loaded(sender As Object, e As RoutedEventArgs)
            Dim chiefs As List(Of String) = ConfigHelper.GetListFromGlobalConfig("listbox", "chief_approvals")
            ListboxApproval.Items.Clear()
            ListboxApproval.ItemsSource = chiefs
        End Sub

        Private Sub BtnApproval_Click(sender As Object, e As RoutedEventArgs)
            _handler.HandleApprovalClick()
        End Sub

        Private Sub BtnSignature_Click(sender As Object, e As RoutedEventArgs)
            _handler.HandleSignatureClick()
        End Sub

        Private Sub BtnClose_Click(sender As Object, e As RoutedEventArgs)
            _handler.HandleCloseClick(_hostForm)
        End Sub

    End Class

End Namespace
