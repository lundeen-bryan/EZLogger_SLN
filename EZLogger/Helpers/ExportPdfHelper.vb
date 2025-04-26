Imports Microsoft.Office.Interop.Word
Imports System.IO

Namespace Helpers

    ''' <summary>
    ''' Provides helper methods for exporting Word documents to PDF format.
    ''' </summary>
    Public Module ExportPdfHelper

        ''' <summary>
        ''' Exports the currently active Word document to a PDF file.
        ''' </summary>
        ''' <param name="destinationFolder">The folder path where the PDF should be saved.</param>
        ''' <param name="fileNameWithoutExtension">The base file name to use (no extension).</param>
        Public Sub ExportActiveDocumentToPdf(destinationFolder As String, fileNameWithoutExtension As String)
            Try
                Dim wordApp As Microsoft.Office.Interop.Word.Application = WordAppHelper.GetWordApp()
                Dim doc As Document = wordApp.ActiveDocument

                If doc Is Nothing Then Exit Sub
                If Not Directory.Exists(destinationFolder) Then
                    Directory.CreateDirectory(destinationFolder)
                End If

                Dim pdfPath As String = Path.Combine(destinationFolder, fileNameWithoutExtension & ".pdf")

                doc.ExportAsFixedFormat(
                    OutputFileName:=pdfPath,
                    ExportFormat:=WdExportFormat.wdExportFormatPDF,
                    OpenAfterExport:=False,
                    OptimizeFor:=WdExportOptimizeFor.wdExportOptimizeForPrint,
                    Range:=WdExportRange.wdExportAllDocument,
                    Item:=WdExportItem.wdExportDocumentContent,
                    IncludeDocProps:=True,
                    KeepIRM:=True,
                    CreateBookmarks:=WdExportCreateBookmarks.wdExportCreateWordBookmarks,
                    DocStructureTags:=True,
                    BitmapMissingFonts:=True,
                    UseISO19005_1:=False)

            Catch ex As Exception
                ' Optional: log or swallow errors safely
            End Try
        End Sub

    End Module

End Namespace
