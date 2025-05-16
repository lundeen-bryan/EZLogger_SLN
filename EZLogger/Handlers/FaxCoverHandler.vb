' Namespace=EZLogger/Handlers
' Filename=FaxCoverHandler.vb
' !See Label Footer for notes

Imports EZLogger.Helpers
Imports EZLogger.Models
Imports Microsoft.Office.Interop.Word
Imports System.IO
Imports System.Windows.Forms

Namespace Handlers

    Public Class FaxCoverHandler

        ''' <summary>
        ''' Creates a fax cover or exports to PDF depending on the user's choice.
        ''' </summary>
        ''' <param name="letter">Cover type code (A–T).</param>
        ''' <param name="saveToTemp">Whether to save intermediate .docx to Temp.</param>
        ''' <param name="convertToPdf">Whether to export the result to PDF.</param>
        Public Sub CreateFaxCover(letter As String, saveToTemp As Boolean, convertToPdf As Boolean, totalPages As Integer, originalReportPages As Integer)
            ' 1) Get the active document (source forensic report)
            Dim app = WordAppHelper.GetWordApp()
            Dim sourceDoc = app.ActiveDocument

            ' Update Pages property to include extra pages
            DocumentPropertyHelper.WriteCustomProperty(sourceDoc, "Pages", totalPages.ToString())


            ' 2) Special case: "A" = export the report directly & name with county
            If letter.ToUpper().Trim() = "A" Then
                Dim county As String = DocumentPropertyHelper.GetPropertyValue(sourceDoc, "County")
                Dim folder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
                Dim originalName = Path.GetFileNameWithoutExtension(sourceDoc.FullName)
                originalName = originalName & $" {county}"
                ExportPdfHelper.ExportActiveDocumentToPdf(folder, originalName)
                MsgBoxHelper.Show("PDF exported successfully.")
                Return
            End If

            ' 3) Load document properties and template info
            Dim info As FaxCoverInfo = PopulateFaxCoverInfo()
            info.TemplateFileName = Path.Combine(info.TemplatesPath, CoverTemplateMap.GetTemplateFileName(letter))

            ' 4) Create a new document from the template
            Dim coverDoc As Document = WordTemplateHelper.CreateDocumentFromTemplate(info.TemplateFileName)
            If coverDoc Is Nothing Then
                MsgBoxHelper.Show($"Template not found: {info.TemplateFileName}")
                Return
            End If

            Try
                ' 5) Fill bookmarks from the source document
                BookmarkHelper.FillBookmarksFromDocumentProperties(sourceDoc, coverDoc)

                ' 6) If this cover type requires mail merge, connect and merge
                Dim mapInfo = CoverTemplateMap.GetTemplateInfo(letter)
                Dim mergedDoc As Document = coverDoc ' Start assuming mergedDoc = coverDoc

                If mapInfo IsNot Nothing AndAlso mapInfo.NeedsMailMerge Then
                    Dim dataPath = ConfigHelper.GetLocalConfigValue("sp_filepath", mapInfo.MailMergeSourceKey)
                    If File.Exists(dataPath) Then
                        Dim sheet = CoverTemplateMap.GetMailMergeSheet(letter)

                        ' 6a) Connect to Excel
                        MailMergeHelper.ConnectToExcelDataSource(coverDoc, dataPath, sheet)

                        ' 6b) Select correct record by county
                        MailMergeHelper.SelectRecordByCounty(coverDoc, info.County)

                        ' 6c) Execute the mail merge
                        MailMergeHelper.ExecuteMailMerge(coverDoc)

                        ' 6d) Switch to the newly merged document
                        mergedDoc = WordAppHelper.GetWordApp().ActiveDocument

                        ' 6e) Unlink fields
                        MailMergeHelper.UnlinkAllFields(mergedDoc)

                        ' 6f) Close the original template copy (coverDoc)
                        coverDoc.Close(SaveChanges:=False)
                    Else
                        MsgBoxHelper.Show($"Mail merge data source not found: {dataPath}")
                        coverDoc.Close(False)
                        Return
                    End If
                End If

                ' 7) Save merged document to temp folder if requested
                If saveToTemp Then
                    ' Save temp copy of the merged cover page
                    Dim tempPath = TempFileHelper.GetSavePath(mergedDoc, letter, True)
                    mergedDoc.SaveAs2(FileName:=tempPath, FileFormat:=WdSaveFormat.wdFormatDocumentDefault)

                Else
                    ' 🆕 Save the COVER PAGE (mergedDoc) into Documents, NOT the forensic report
                    Dim outputFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
                    Dim originalBase = Path.GetFileNameWithoutExtension(sourceDoc.FullName)
                    Dim coverTypeName = TempFileHelper.GetCoverTypeName(letter)
                    Dim outputName = $"{originalBase} {coverTypeName}.docx"
                    Dim savePath = Path.Combine(outputFolder, outputName)

                    ' Save the merged cover page as .docx
                    mergedDoc.SaveAs2(FileName:=savePath, FileFormat:=WdSaveFormat.wdFormatXMLDocument)

                    MsgBoxHelper.Show("Word cover page saved successfully.")
                End If

                ' 8) Export merged document to PDF if requested
                If convertToPdf Then
                    ' 🆕 Activate the merged document first
                    mergedDoc.Activate()

                    Dim outputFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
                    Dim originalBase = Path.GetFileNameWithoutExtension(sourceDoc.FullName)
                    Dim coverTypeName = TempFileHelper.GetCoverTypeName(letter)
                    Dim outputName = $"{originalBase} {coverTypeName}"

                    ExportPdfHelper.ExportActiveDocumentToPdf(outputFolder, outputName)
                End If

                ' 9) (Optional) You could close mergedDoc if you want, but not required
                mergedDoc.Close(SaveChanges:=False)

            Finally
                ' Nothing else to do here; everything is already handled
            End Try

            ' 10) Success message
            MsgBoxHelper.Show("Cover page generated successfully.")

            ' Restore original Pages property
            Dim pageNumReset = sourceDoc.ComputeStatistics(WdStatistic.wdStatisticPages)
            DocumentPropertyHelper.WriteCustomProperty(sourceDoc, "Pages", pageNumReset.ToString())

        End Sub

        ''' <summary>
        ''' Closes the host WinForm associated with the fax cover view.
        ''' </summary>
        Public Sub HandleCloseClick(hostForm As Form)
            hostForm?.Close()
        End Sub

        ''' <summary>
        ''' Populates and returns a FaxCoverInfo object with information retrieved from document properties and configuration settings.
        ''' </summary>
        ''' <returns>
        ''' A FaxCoverInfo object containing patient information, report details, and relevant paths for fax cover generation.
        ''' </returns>
        ''' <remarks>
        ''' This function retrieves various pieces of information from document properties using the DocumentPropertyHelper,
        ''' parses dates, and sets up necessary paths. It's used to prepare all the required data for generating a fax cover sheet.
        ''' </remarks>
        Private Function PopulateFaxCoverInfo() As FaxCoverInfo
            Dim functionName As String = "FaxCoverHandler.PopulateFaxCoverInfo"
            Dim doc = DocumentHelper.GetActiveWordDocument
            Dim info As New FaxCoverInfo()

            Try
                With info
                    .LastName = DocumentPropertyHelper.GetPropertyValue(doc, "Lastname")
                    .FirstName = DocumentPropertyHelper.GetPropertyValue(doc, "Firstname")
                    .PatientInitials = If(.FirstName.Length > 0, .FirstName(0), "") & If(.LastName.Length > 0, .LastName(0), "")
                    .ReportType = DocumentPropertyHelper.GetPropertyValue(doc, "Report Type")
                    .Pages = DocumentPropertyHelper.GetPropertyValue(doc, "Pages")
                    .UniqueId = DocumentPropertyHelper.GetPropertyValue(doc, "Unique ID")
                    .Evaluator = DocumentPropertyHelper.GetPropertyValue(doc, "Evaluator")
                    .ProcessedBy = DocumentPropertyHelper.GetPropertyValue(doc, "Processed By")
                    .ReportDate = DocumentPropertyHelper.GetPropertyValue(doc, "Report Date")
                    .County = DocumentPropertyHelper.GetPropertyValue(doc, "County")
                    .ApprovedBy = DocumentPropertyHelper.GetPropertyValue(doc, "Approved By")

                    ' Parse date into parts
                    Dim dt As DateTime
                    If DateTime.TryParse(.ReportDate, dt) Then
                        .Month = dt.ToString("MM")
                        .Day = dt.ToString("dd")
                        .Year = dt.Year.ToString()
                    End If

                    ' Paths
                    .TempFolder = TempFileHelper.GetTempFolder()
                    .TemplatesPath = Path.Combine(ConfigHelper.GetLocalConfigValue("sp_filepath", "databases"), "Templates")
                End With

            Catch ex As Exception
                Dim errNum As String = ex.HResult.ToString()
                Dim errMsg As String = CStr(ex.Message)
                Dim recommendation As String = "Error while populating FaxCoverInfo. Complete previous steps before printing cover pages."

                ErrorHelper.HandleError(functionName, errNum, errMsg, recommendation)
            End Try

            Return info
        End Function

        ''' <summary>
        ''' Displays the Fax Cover form in a new window.
        ''' </summary>
        ''' <remarks>
        ''' This method creates a new instance of the FaxCoverHost form and shows it to the user.
        ''' The form is displayed modally, meaning it will block interaction with other windows until it is closed.
        ''' </remarks>
        Public Sub ShowFaxCoverMessage()
            Dim host As New FaxCoverHost()
            host.Show()
        End Sub

    End Class

End Namespace
' Footer:
''===========================================================================================
'' Filename: .......... FaxCoverHandler.vb
'' Description: ....... Provides logic related to generating or managing fax cover sheets in EZLogger
'' Created: ........... 2025-05-02
'' Updated: ........... 2025-05-02
'' Installs to: ....... EZLogger/Handlers
'' Compatibility: ..... VSTO, WPF
'' Contact Author: .... lundeen-bryan
'' Copyright:  ........ ©2025. All rights reserved.
'' Notes: ............. _
' (1) Method Index _
' - CreateFaxCover(letter As String, saveToTemp As Boolean, convertToPdf
'   As Boolean, totalPages As Integer, originalReportPages As Integer):
'   Generates a fax cover sheet using a template and document
'   properties, and optionally exports it to PDF.
' - HandleCloseClick(hostForm As Form): Closes the fax cover form if it
'   is not null.
' - PopulateFaxCoverInfo(): Retrieves and returns document and config
'   values to populate a FaxCoverInfo object for use in cover
'   generation.
' - ShowFaxCoverMessage(): Opens and displays the FaxCoverHost form to
'   the user.
''===========================================================================================
