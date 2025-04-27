Imports System.IO
Imports System.Windows.Forms
Imports Microsoft.Office.Interop.Word
Imports EZLogger.Helpers
Imports EZLogger.Models
Imports System.Diagnostics

Namespace Handlers

    ''' <summary>
    ''' Provides logic related to generating or managing fax cover sheets in EZLogger.
    ''' </summary>
    Public Class FaxCoverHandler

        Private Const Sep As String = "\"

        ''' <summary>
        ''' Displays the Fax Cover form (hosted view).
        ''' </summary>
        Public Sub ShowFaxCoverMessage()
            Dim host As New FaxCoverHost()
            host.Show()
        End Sub

        ''' <summary>
        ''' Closes the host WinForm associated with the fax cover view.
        ''' </summary>
        Public Sub HandleCloseClick(hostForm As Form)
            hostForm?.Close()
        End Sub

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


            ' 2) Special case: "A" = export the report directly
            If letter.ToUpper().Trim() = "A" Then
                Dim folder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
                Dim originalName = Path.GetFileNameWithoutExtension(sourceDoc.FullName)
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
            DocumentPropertyHelper.WriteCustomProperty(sourceDoc, "Pages", originalReportPages.ToString())

        End Sub

        ''' <summary>
        ''' Reads document properties and config paths into a FaxCoverInfo object.
        ''' </summary>
        Private Function PopulateFaxCoverInfo() As FaxCoverInfo
            Dim info As New FaxCoverInfo()
            With info
                .LastName = DocumentPropertyHelper.GetPropertyValue("Lastname")
                .FirstName = DocumentPropertyHelper.GetPropertyValue("Firstname")
                .PatientInitials = If(.FirstName.Length > 0, .FirstName(0), "") & If(.LastName.Length > 0, .LastName(0), "")
                .ReportType = DocumentPropertyHelper.GetPropertyValue("Report Type")
                .Pages = DocumentPropertyHelper.GetPropertyValue("Pages")
                .UniqueId = DocumentPropertyHelper.GetPropertyValue("Unique ID")
                .Evaluator = DocumentPropertyHelper.GetPropertyValue("Evaluator")
                .ProcessedBy = DocumentPropertyHelper.GetPropertyValue("Processed By")
                .ReportDate = DocumentPropertyHelper.GetPropertyValue("Report Date")
                .County = DocumentPropertyHelper.GetPropertyValue("County")
                .ApprovedBy = DocumentPropertyHelper.GetPropertyValue("Approved By")

                ' Parse date into parts
                Dim dt As DateTime
                If DateTime.TryParse(.ReportDate, dt) Then
                    .Month = dt.ToString("MM")
                    .Day = dt.ToString("dd")
                    .Year = dt.Year.ToString()
                End If

                ' Paths
                .TempFolder = TempFileHelper.GetTempFolder()
                .TemplatesPath = ConfigHelper.GetLocalConfigValue("sp_filepath", "databases") & Sep & "Templates"
            End With
            Return info
        End Function

    End Class
End Namespace
