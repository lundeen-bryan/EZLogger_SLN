Imports System.Windows.Forms
Imports EZLogger.Helpers
Imports Microsoft.Office.Interop.Word
Imports System.IO

Namespace Handlers

    ''' <summary>
    ''' Provides logic related to generating or managing fax cover sheets in EZLogger.
    ''' </summary>
    Public Class FaxCoverHandler

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
        ''' Handles creating the fax cover or exporting to PDF depending on the selection.
        ''' </summary>
        Public Sub CreateFaxCover(letter As String, saveToTemp As Boolean, convertToPdf As Boolean)
            Try
                If String.IsNullOrEmpty(letter) Then Exit Sub

                If letter = "A" Then
                    ' Special case: Convert the current active document to PDF
                    Dim saveFolder As String = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
                    Dim fileNameWithoutExtension As String = GetSafeActiveDocumentNameWithoutExtension()

                    ExportPdfHelper.ExportActiveDocumentToPdf(saveFolder, fileNameWithoutExtension)

                    MsgBoxHelper.Show("Successfully exported the current document as a PDF to your Documents folder.")
                    Exit Sub
                End If

                ' TODO: Implement other letter cases (B-T) for template creation later.

            Catch ex As Exception
                MsgBoxHelper.Show("An error occurred while creating the fax cover: " & ex.Message)
            End Try
        End Sub

        ''' <summary>
        ''' Generates a safe file name based on the active Word document name.
        ''' </summary>
        Private Function GetSafeActiveDocumentNameWithoutExtension() As String
            Try
                Dim doc As Document = WordAppHelper.GetWordApp().ActiveDocument
                If doc Is Nothing Then Return "ExportedDocument"

                Dim fileName = System.IO.Path.GetFileNameWithoutExtension(doc.Name)
                For Each c In System.IO.Path.GetInvalidFileNameChars()
                    fileName = fileName.Replace(c, "_"c)
                Next
                Return fileName

            Catch
                Return "ExportedDocument"
            End Try
        End Function

    End Class

End Namespace
