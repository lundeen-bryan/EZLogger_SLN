Namespace EZLogger.Models

    Public Class ReportTypeEntry
        Public Property Choice As String
        Public Property PcCode As String
        Public Property TypicalWording As String

        Public Shared Function GetReportTypes() As List(Of ReportTypeEntry)
            Return New List(Of ReportTypeEntry) From {
                New ReportTypeEntry With {.Choice = "Competent", .PcCode = "1372(a)(1)", .TypicalWording = "returned to court as competent to stand trial"},
                New ReportTypeEntry With {.Choice = "Not Yet Competent", .PcCode = "1370(b)(1)", .TypicalWording = "not yet competent to stand trial"},
                New ReportTypeEntry With {.Choice = "UNLIKELY 1370(b)(1)", .PcCode = "1370(b)(1)", .TypicalWording = "not competent… no substantial likelihood… to regain competence in the foreseeable future"},
                New ReportTypeEntry With {.Choice = "UNLIKELY 1370(c)(1)", .PcCode = "1370(c)(1)", .TypicalWording = "not competent… no substantial likelihood… to regain competence… AND within 90 days of expiration"},
                New ReportTypeEntry With {.Choice = "Retain and Treat", .PcCode = "1026/2972", .TypicalWording = "Select this if the patient is NOT being recommended for COT. May say: 'Should status and progress indicate…'"},
                New ReportTypeEntry With {.Choice = "Restored", .PcCode = "1026/2972", .TypicalWording = "Basically choose this if they are recommended for COT."},
                New ReportTypeEntry With {.Choice = "Malingering", .PcCode = "1370", .TypicalWording = "Select this if the report specifically states the patient is malingering."}
            }
        End Function
    End Class

End Namespace
