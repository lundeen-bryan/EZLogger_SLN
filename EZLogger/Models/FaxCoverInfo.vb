Namespace Models

    ''' <summary>
    ''' Holds all parameters needed to build or export a fax cover.
    ''' Mirrors the legacy VBA locals one-to-one.
    ''' </summary>
    Public Class FaxCoverInfo

        Public Property LastName As String ' Last name of the patient
        Public Property FirstName As String ' First name of the patient
        Public Property CourtNumber As String ' Court number for the patient
        Public Property PatientInitials As String ' Initials of the patient so if name is John Smith then initals are J.S.
        Public Property TempFolder As String ' Location of the temp folder
        Public Property ReportType As String ' Report type saved in doc properties
        Public Property Pages As String ' Count of pages in active word document
        Public Property UniqueId As String ' Unique alphanumeric number identifying the document created from CreateUniqueId
        Public Property Evaluator As String ' Evaluator assigned to the case found in doc properties
        Public Property ProcessedBy As String   ' Processed By value in doc properties
        Public Property Month As String   ' MO
        Public Property Day As String   ' DA
        Public Property Year As String   ' YR
        Public Property ReportDate As String ' date of the report found in doc properties
        Public Property County As String ' County in doc properties
        Public Property ApprovedBy As String ' Approved By in doc properties
        Public Property TemplatesPath As String   ' path to the template that is used when coverting to pdf
        Public Property TemplateFileName As String ' filename for the template used when converting to pdf
        Public Property CoverFileName As String
        Public Property CoverType As String

    End Class

End Namespace
