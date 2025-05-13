' Namespace=EZLogger/Helpers
' Filename=ExportPdfHelper.vb
' !See Label Footer for notes

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
            Const FUNCTIONNAME As String = "ExportPdfHelper.ExportActiveDocumentToPdf"
            Dim recommendation As String = "Please ensure a valid Word document is active and try exporting again."

            Try
                Dim wordApp As Microsoft.Office.Interop.Word.Application = WordAppHelper.GetWordApp()
                Dim doc As Document = wordApp.ActiveDocument

                ' Throw exception if document is null
                If doc Is Nothing Then
                    Throw New ArgumentNullException("doc", "Active document cannot be null. " & recommendation)
                End If

                ' Create destination folder if it doesn't exist
                If Not Directory.Exists(destinationFolder) Then
                    Directory.CreateDirectory(destinationFolder)
                End If

                ' Construct PDF file path
                Dim pdfPath As String = Path.Combine(destinationFolder, fileNameWithoutExtension & ".pdf")

                ' Export document to PDF
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
                Dim errNum As String = ex.HResult.ToString()
                Dim errMsg As String = ex.Message

                ' Handle the exception (log, display error message, etc.)
                ErrorHelper.HandleError(FUNCTIONNAME, errNum, errMsg, recommendation)
            End Try
        End Sub
    End Module

End Namespace

' Footer:
''===========================================================================================
'' Filename: .......... ExportPdfHelper.vb
'' Description: ....... Prints or converts a word document to a pdf
'' Created: ........... 2025-05-12
'' Updated: ........... 2025-05-12
'' Installs to: ....... EZLogger/Helpers
'' Compatibility: ..... VSTO
'' Contact Author: .... lundeen-bryan
'' Copyright:  ........ ©2025. All rights reserved.
'' Notes: ............. _
' (1) notes_here
''===========================================================================================