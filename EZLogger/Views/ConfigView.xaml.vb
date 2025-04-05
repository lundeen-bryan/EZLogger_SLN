Imports System.IO
Imports System.Windows
Imports EZLogger.EZLogger.Helpers

Public Class ConfigView
    Private Sub ConfigView_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        TextBoxDoctors.Text = String.Join(Environment.NewLine, ConfigPathHelper.GetDoctorList())
    End Sub

    Private Sub BtnSaveDoctorsList_Click(sender As Object, e As RoutedEventArgs) Handles BtnSaveDoctorsList.Click
        Dim filePath As String = ConfigPathHelper.GetDoctorListFilePath()
        File.WriteAllText(filePath, TextBoxDoctors.Text)
        MessageBox.Show("Doctor list saved.")
    End Sub

End Class
