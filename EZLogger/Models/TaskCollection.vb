Imports System
Imports System.Collections.Generic
Imports System.Xml.Serialization

<XmlRoot("Tasks")>
Public Class TaskCollection

    <XmlElement("Task")>
    Public Property Items As List(Of TaskItem)

    Public Sub New()
        Items = New List(Of TaskItem)()
    End Sub

End Class
