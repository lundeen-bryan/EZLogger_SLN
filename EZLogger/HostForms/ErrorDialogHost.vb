Imports System.Windows.Forms

Public Class ErrorDialogHost

    Private _errorMessage As String
    Private _errorNumber As String
    Private _recommendation As String
    Private _source As String

    Public Sub New(errorMessage As String,
                   errorNumber As String,
                   recommendation As String,
                   source As String,
                   Optional hostForm As Form = Nothing)

        InitializeComponent()

        ' Store error details in private fields
        _errorMessage = errorMessage
        _errorNumber = errorNumber
        _recommendation = recommendation
        _source = source
    End Sub

    Private Sub ErrorDialogHost_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Create the view and assign it to the ElementHost
        Dim view As New ErrorDialogView(Me)

        view.SetErrorFields(_errorMessage, _errorNumber, _recommendation, _source)

        ElementHost1.Child = view

        ' Retain original layout and styling logic
        Me.ClientSize = New Drawing.Size(610, 580)
        Me.Text = ""

        Me.MinimizeBox = False
        Me.MaximizeBox = False
        Me.ShowIcon = False
        Me.FormBorderStyle = FormBorderStyle.FixedSingle

        FormPositionHelper.MoveFormToTopLeftOfAllScreens(Me, 10, 10)

        ElementHost1.Width = Me.ClientSize.Width - 40
        ElementHost1.Height = Me.ClientSize.Height - 40
        ElementHost1.Location = New Drawing.Point(20, 20)
    End Sub

End Class
