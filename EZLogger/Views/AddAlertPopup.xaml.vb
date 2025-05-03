Imports System.Windows

Public Class AddAlertPopup

    Private ReadOnly _isCounty As Boolean
    Public Property AlertKey As String
    Public Property AlertValue As String


    Public Sub New(Optional isCounty As Boolean = False)
        InitializeComponent()
        _isCounty = isCounty
        LabelKey.Content = If(_isCounty, "County Name:", "Patient Number:")
        WireUpButtons()
    End Sub

    Private Sub WireUpButtons()
        AddHandler BtnSave.Click, AddressOf BtnSave_Click
        AddHandler CancelBtn.Click, AddressOf CancelBtn_Click
    End Sub

    Private Sub BtnSave_Click(sender As Object, e As RoutedEventArgs)
        AlertKey = KeyBox.Text.Trim()
        AlertValue = ValueBox.Text.Trim()

        If String.IsNullOrEmpty(AlertKey) OrElse String.IsNullOrEmpty(AlertValue) Then
            MessageBox.Show("Please enter both a key and a value.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning)
            Return
        End If

        Me.DialogResult = True
    End Sub

    Private Sub CancelBtn_Click(sender As Object, e As RoutedEventArgs)
        Me.DialogResult = False
    End Sub
End Class
