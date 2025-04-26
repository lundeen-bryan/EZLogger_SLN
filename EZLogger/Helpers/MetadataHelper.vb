Imports Microsoft.Office.Interop.Word

Namespace Helpers

    ''' <summary>
    ''' Provides helper methods to set Word document built-in properties.
    ''' </summary>
    Public Module MetadataHelper

        ''' <summary>
        ''' Saves key built-in document properties (Title, Subject, Author, Company, Comments) based on provided values.
        ''' </summary>
        Public Sub SaveBuiltProperties(
            patientName As String,
            reportType As String,
            reportDate As String,
            program As String,
            unit As String,
            evaluator As String,
            processedBy As String,
            county As String)

            Try
                Dim wordApp As Microsoft.Office.Interop.Word.Application = WordAppHelper.GetWordApp()
                Dim doc As Document = wordApp.ActiveDocument
                If doc Is Nothing Then Exit Sub

                Dim todayDate As String = Now.ToString("yyyy-MM-dd")
                Dim formattedReportDate As String = DateTime.Parse(reportDate).ToString("yyyy-MM-dd")

                ' Set Title
                doc.BuiltInDocumentProperties("Title").Value =
                    $"{StrConv(patientName, VbStrConv.ProperCase)} {reportType} {formattedReportDate}"

                ' Set Subject
                doc.BuiltInDocumentProperties("Subject").Value =
                    $"Program {program} Unit {unit}"

                ' Set Author
                doc.BuiltInDocumentProperties("Author").Value = evaluator

                ' Set Company
                doc.BuiltInDocumentProperties("Company").Value = $"Unit {unit}"

                ' Set Comments
                doc.BuiltInDocumentProperties("Comments").Value =
                    $"Processed by {processedBy} {todayDate}{vbCrLf}For {county}"

            Catch ex As Exception
                ' Optional: log or silently swallow error
            End Try

        End Sub

    End Module

End Namespace
