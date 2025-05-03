Imports System.Xml.Serialization

<XmlRoot("Tasks")>
Public Class TaskCollection
    Public Sub New()
        Items = New List(Of TaskItem)()
    End Sub

    <XmlElement("Task")>
    Public Property Items As List(Of TaskItem)

End Class
