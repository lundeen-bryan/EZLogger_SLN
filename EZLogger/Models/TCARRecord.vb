Public Class TCARRecord
    Public Property Casenum As String
    Public Property Tcar As Integer
    Public Property PatientName As String
    Public Property Subdate As String
    Public Property OpID As Integer

    Public ReadOnly Property OpinionDescription As String
        Get
            Select Case OpID
                Case 1 : Return "Unlikely"
                Case 2 : Return "Competent"
                Case 3 : Return "Not Yet Competent"
                Case Else : Return "Unknown"
            End Select
        End Get
    End Property
End Class
