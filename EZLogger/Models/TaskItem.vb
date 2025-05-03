Imports System
Imports System.ComponentModel
Imports System.Xml.Serialization

<Serializable>
Public Class TaskItem
    Implements INotifyPropertyChanged

    ''' <summary>
    ''' When the object’s property changes, this event fires.
    ''' </summary>
    Public Event PropertyChanged As PropertyChangedEventHandler _
        Implements INotifyPropertyChanged.PropertyChanged

    ' Backing fields
    Private _dateAdded As DateTime
    Private _notes As String
    Private _isCompleted As Boolean

    ''' <summary>
    ''' Helper to raise PropertyChanged.
    ''' </summary>
    Protected Sub OnPropertyChanged(propName As String)
        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(propName))
    End Sub

    ''' <summary>
    ''' The date/time this task was created.
    ''' </summary>
    <XmlElement("DateAdded")>
    Public Property DateAdded As DateTime
        Get
            Return _dateAdded
        End Get
        Set(value As DateTime)
            If _dateAdded <> value Then
                _dateAdded = value
                OnPropertyChanged(NameOf(DateAdded))
            End If
        End Set
    End Property

    ''' <summary>
    ''' Arbitrary text describing what to follow up.
    ''' </summary>
    <XmlElement("Notes")>
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

    ''' <summary>
    ''' True when the user checks the task as done.
    ''' </summary>
    <XmlElement("IsCompleted")>
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

    ''' <summary>
    ''' Parameterless ctor for XML serialization.
    ''' </summary>
    Public Sub New()
        _dateAdded = DateTime.Now
        _notes = String.Empty
        _isCompleted = False
    End Sub

End Class
