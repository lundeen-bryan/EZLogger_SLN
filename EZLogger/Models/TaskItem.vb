Imports System
Imports System.Xml.Serialization

<Serializable>
Public Class TaskItem

    <XmlElement("DateAdded")>
    Public Property DateAdded As DateTime

    <XmlElement("Notes")>
    Public Property Notes As String

    <XmlElement("IsCompleted")>
    Public Property IsCompleted As Boolean

    Public Sub New()
        ' Parameterless constructor required for XmlSerializer
        DateAdded = DateTime.Now
        Notes = String.Empty
        IsCompleted = False
    End Sub

End Class
