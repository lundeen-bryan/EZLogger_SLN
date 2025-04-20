Imports System.Windows

Public Class AddAlertPopup

    Public Property AlertKey As String
    Public Property AlertValue As String

    Private ReadOnly _isCounty As Boolean

    Public Sub New(Optional isCounty As Boolean = False)
        InitializeComponent()
        _isCounty = isCounty
        LabelKey.Content = If(_isCounty, "County Name:", "Patient Number:")
    End Sub

    Private Sub BtnSave_Click(sender As Object, e As RoutedEventArgs) Handles BtnSave.Click
        AlertKey = KeyBox.Text.Trim()
        AlertValue = ValueBox.Text.Trim()

        If String.IsNullOrEmpty(AlertKey) OrElse String.IsNullOrEmpty(AlertValue) Then
            MessageBox.Show("Please enter both a key and a value.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning)
            Return
        End If

        Me.DialogResult = True
    End Sub

    Private Sub BtnCancel_Click(sender As Object, e As RoutedEventArgs) Handles BtnCancel.Click
        Me.DialogResult = False
    End Sub
End Class
