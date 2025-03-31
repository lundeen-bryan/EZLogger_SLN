Imports System.Windows.Controls

Public Class OpinionRow
    Public Property Choice As String
    Public Property PCCode As String
    Public Property TypicalWording As String
End Class

Partial Public Class OpinionView
    Inherits UserControl

    Public Sub New()
        InitializeComponent()

        ' Populate the ListBox
        OpinionListBox.ItemsSource = New List(Of String) From {
            "1370(b)(1)",
            "1372(a)(1)",
            "PPR",
            "1026.5(b)(1)"
        }
    End Sub

End Class

