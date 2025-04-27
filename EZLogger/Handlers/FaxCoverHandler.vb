Imports System.IO
Imports System.Windows.Forms
Imports Microsoft.Office.Interop.Word
Imports EZLogger.Helpers
Imports EZLogger.Models

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
        Public Sub CreateFaxCover(letter As String, saveToTemp As Boolean, convertToPdf As Boolean)
            ' 1) Grab the active document as the source for bookmarks and filename
            Dim sourceDoc As Document = WordAppHelper.GetWordApp().ActiveDocument
            If sourceDoc Is Nothing Then
                MsgBoxHelper.Show("No active document found to base the cover on.")
                Return
            End If

            Select Case letter.ToUpper().Trim()
                Case "A"
                    ' Convert active document directly to PDF
                    Dim folder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
                    ' Use the original document's filename (without extension)
                    Dim originalName = Path.GetFileNameWithoutExtension(sourceDoc.FullName)
                    ExportPdfHelper.ExportActiveDocumentToPdf(folder, originalName)

                Case "B" To "T"
                    ' 2) Load common properties and paths
                    Dim info As FaxCoverInfo = PopulateFaxCoverInfo()
                    info.TemplateFileName = Path.Combine(info.TemplatesPath, CoverTemplateMap.GetTemplateFileName(letter))

                    ' 3) Open the template document
                    Dim coverDoc As Document = WordTemplateHelper.CreateDocumentFromTemplate(info.TemplateFileName)
                    If coverDoc Is Nothing Then
                        MsgBoxHelper.Show($"Template not found: {info.TemplateFileName}")
                        Return
                    End If

                    ' 4) Fill bookmarks from the original document
                    BookmarkHelper.FillBookmarksFromDocumentProperties(sourceDoc, coverDoc)

                    ' 5) Perform mail merge if required
                    Dim mapInfo = CoverTemplateMap.GetTemplateInfo(letter)
                    If mapInfo IsNot Nothing AndAlso mapInfo.NeedsMailMerge Then
                        Dim dataPath = ConfigHelper.GetLocalConfigValue("sp_filepath", mapInfo.MailMergeSourceKey)
                        If File.Exists(dataPath) Then
                            Dim sheet = CoverTemplateMap.GetMailMergeSheet(letter)
                            MailMergeHelper.ConnectToExcelDataSource(coverDoc, dataPath, sheet)
                            MailMergeHelper.UnlinkAllFields(coverDoc)
                        Else
                            MsgBoxHelper.Show($"Mail merge data source not found: {dataPath}")
                            coverDoc.Close(False)
                            Return
                        End If
                    End If

                    ' 6) Save the intermediate .docx to Temp, if requested
                    If saveToTemp Then
                        Dim tempPath = TempFileHelper.GetSavePath(coverDoc, letter, True)
                        coverDoc.SaveAs2(FileName:=tempPath, FileFormat:=WdSaveFormat.wdFormatDocumentDefault)
                    End If

                    ' 7) Export to PDF using the original document's name + cover type
                    If convertToPdf Then
                        Dim outputFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
                        Dim originalBase = Path.GetFileNameWithoutExtension(sourceDoc.FullName)
                        Dim coverTypeName = TempFileHelper.GetCoverTypeName(letter)
                        Dim outputName = $"{originalBase} {coverTypeName}"
                        ExportPdfHelper.ExportActiveDocumentToPdf(outputFolder, outputName)
                    End If

                    ' 8) Close the template doc without saving
                    coverDoc.Close(False)

                Case Else
                    MsgBoxHelper.Show($"Cover type '{letter}' is not implemented yet.")
                    Return
            End Select

            MsgBoxHelper.Show("Cover page generated successfully.")
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
