Imports Microsoft.Office.Interop.Word
Imports System.IO

Namespace Helpers

    ''' <summary>
    ''' Provides helper methods for working with Word templates and documents.
    ''' </summary>
    Public Module WordTemplateHelper

        ''' <summary>
        ''' Creates a new Word document based on a specified template file.
        ''' </summary>
        ''' <param name="templatePath">The full path to the template (.dot or .dotx) file.</param>
        ''' <returns>The newly created Word document, or Nothing if creation fails.</returns>
        Public Function CreateDocumentFromTemplate(templatePath As String) As Document
            Dim functionThatCalls As String = "WordTemplateHelper.CreateDocumentFromTemplate"

            If String.IsNullOrEmpty(templatePath) OrElse Not File.Exists(templatePath) Then
                ' Template path is invalid or missing
                Return Nothing
            End If

            Try
                Dim wordApp As Application = WordAppHelper.GetWordApp()

                ' Create a new document based on the specified template
                Dim newDoc As Document = wordApp.Documents.Add(Template:=templatePath, NewTemplate:=False, DocumentType:=WdNewDocumentType.wdNewBlankDocument, Visible:=True)

                ' Maximize the Word window
                If wordApp.Windows.Count > 0 Then
                    wordApp.Windows(1).WindowState = WdWindowState.wdWindowStateMaximize
                End If

                Return newDoc
            Catch ex As Exception
                    Dim errNum As String = ex.HResult.ToString()
                    Dim errMsg As String = CStr(ex.Message)
                    Dim recommendation As String = "Please confirm the patient number from the report to make sure it matches a patient in ForensicInfo."

                    ErrorHelper.HandleError(functionThatCalls, errNum, errMsg, recommendation)
                Return Nothing
            End Try
        End Function

    End Module

End Namespace
