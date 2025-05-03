Imports EZLogger.Handlers
Imports EZLogger.Helpers
Imports System.Windows
Imports System.Windows.Forms

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

            WireUpButtons()
        End Sub

        Public Sub WireUpButtons()
            ' Load combo data when the view is created
            AddHandler Me.Loaded, AddressOf ChiefApprovalView_Loaded

            AddHandler ApprovalBtn.Click, AddressOf ApprovalBtn_Click
            AddHandler BtnSignature.Click, AddressOf BtnSignature_Click
            AddHandler DoneBtn.Click, AddressOf DoneBtn_Click
        End Sub

        Private Sub ChiefApprovalView_Loaded(sender As Object, e As RoutedEventArgs)
            Dim chiefs As List(Of String) = ListHelper.GetListFromGlobalConfig("listbox", "chief_approvals")
            ListboxApproval.Items.Clear()
            ListboxApproval.ItemsSource = chiefs
        End Sub

        Private Sub ApprovalBtn_Click(sender As Object, e As RoutedEventArgs)
            Dim selectedName As String = TryCast(ListboxApproval.SelectedItem, String)
            _handler.HandleApprovalClick(selectedName)
        End Sub

        Private Sub BtnSignature_Click(sender As Object, e As RoutedEventArgs)
            ' Get the selected name from the Listbox
            Dim selectedName As String = TryCast(ListboxApproval.SelectedItem, String)

            ' Pass it to the handler
            _handler.HandleSignatureClick(selectedName)
        End Sub

        Private Sub DoneBtn_Click(sender As Object, e As RoutedEventArgs)
            Dim panel = TaskPaneHelper.GetTaskPane()
            panel?.MarkCheckboxAsDone("Btn_H")
            _handler.HandleCloseClick(_hostForm)
        End Sub

    End Class

End Namespace
