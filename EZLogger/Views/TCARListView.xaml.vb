Imports System.Windows
Imports System.Windows.Controls
Imports EZLogger.Handlers

Public Class TCARListView
    Inherits UserControl

    Public Sub New()
        InitializeComponent()

        ' Load TCAR records from the handler
        Dim records As List(Of TCARRecord) = TCARListHandler.LoadAllActive()

        ' Set the DataGrid's item source
        TCARGrid.ItemsSource = records
    End Sub

    ' This method is wired up in XAML and simply delegates to the handler
    Private Sub BtnSelectPatient_Click(sender As Object, e As RoutedEventArgs)
        TCARListHandler.HandleTCARSelect(TCARGrid)
    End Sub
End Class
