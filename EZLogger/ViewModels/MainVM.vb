Imports System.ComponentModel

Public Class MainVM
    Implements INotifyPropertyChanged

    Private _courtNumbers As String
    Public Property CourtNumbers As String
        Get
            Return _courtNumbers
        End Get
        Set(value As String)
            _courtNumbers = value
            OnPropertyChanged(NameOf(CourtNumbers))
        End Set
    End Property

    Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

    Protected Sub OnPropertyChanged(propertyName As String)
        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(propertyName))
    End Sub
End Class
