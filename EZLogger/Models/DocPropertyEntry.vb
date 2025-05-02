Namespace EZLogger.Models

    ''' <summary>
    ''' Represents a single custom document property in a Word document.
    ''' Used for displaying property name/value pairs in the Patient Info view.
    ''' </summary>
    Public Class DocPropertyEntry

        ''' <summary>
        ''' The name of the custom document property.
        ''' </summary>
        Public Property PropertyName As String

        ''' <summary>
        ''' The value assigned to the custom document property.
        ''' </summary>
        Public Property Value As String

        Public Property PatientName As String

        Public Property PatientNumber As String

    End Class

End Namespace
