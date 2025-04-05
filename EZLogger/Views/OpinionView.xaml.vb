Imports System.Windows
Imports System.Windows.Controls
Imports EZLogger.EZLogger.Helpers
Imports EZLogger.Helpers

Public Class OpinionView

    Private Sub OpinionView_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        ' Get the list of opinions from the global config
        Dim opinions As List(Of String) = ConfigPathHelper.GetOpinionList()

        ' Set the ComboBox items
        OpinionCbo.Items.Clear()
        OpinionCbo.ItemsSource = opinions
    End Sub

End Class
