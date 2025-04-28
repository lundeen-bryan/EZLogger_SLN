Imports System.ComponentModel

Namespace Models

    Public Class TaskItem
        Implements INotifyPropertyChanged

        Private _dateAdded As DateTime
        Private _notes As String
        Private _isCompleted As Boolean

        Public Property DateAdded As DateTime
            Get
                Return _dateAdded
            End Get
            Set(value As DateTime)
                If Not _dateAdded.Equals(value) Then
                    _dateAdded = value
                    OnPropertyChanged(NameOf(DateAdded))
                End If
            End Set
        End Property

        Public Property Notes As String
            Get
                Return _notes
            End Get
            Set(value As String)
                If _notes <> value Then
                    _notes = value
                    OnPropertyChanged(NameOf(Notes))
                End If
            End Set
        End Property

        Public Property IsCompleted As Boolean
            Get
                Return _isCompleted
            End Get
            Set(value As Boolean)
                If _isCompleted <> value Then
                    _isCompleted = value
                    OnPropertyChanged(NameOf(IsCompleted))
                End If
            End Set
        End Property

        Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

        Protected Sub OnPropertyChanged(propName As String)
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(propName))
        End Sub

        Public Sub New()
            _dateAdded = DateTime.Now
            _notes = String.Empty
            _isCompleted = False
        End Sub

    End Class

End Namespace
