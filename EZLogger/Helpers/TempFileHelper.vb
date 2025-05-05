Imports System.IO

Namespace Helpers

    ''' <summary>
    ''' Provides helper methods for generating save paths for temporary or permanent Word files.
    ''' </summary>
    Public Module TempFileHelper

        ''' <summary>
        ''' Gets the save path for the Word file, based on whether saving to temp or documents.
        ''' </summary>
        ''' <param name="doc">The Word document (optional, used for metadata if needed later).</param>
        ''' <param name="letter">The cover letter code selected (A, B, C, etc.).</param>
        ''' <param name="saveToTemp">If True, save to the Temp folder; otherwise save to Documents.</param>
        ''' <returns>The full file path where the document should be saved.</returns>
        Public Function GetSavePath(doc As Object, letter As String, saveToTemp As Boolean) As String
            Dim baseFolder As String

            If saveToTemp Then
                baseFolder = GetTempFolder()
            Else
                baseFolder = GetDocumentsFolder()
            End If

            Dim lastName As String = DocumentPropertyHelper.GetPropertyValue("Lastname")
            Dim firstName As String = DocumentPropertyHelper.GetPropertyValue("Firstname")
            Dim timeStamp As String = Now.ToString("HHmm")

            ' Get a readable cover type name based on letter
            Dim coverType As String = GetCoverTypeName(letter)

            ' Compose file name
            Dim fileName As String = $"{lastName}, {firstName} {coverType} {timeStamp}.docx"

            ' Clean illegal characters just in case
            fileName = RemoveIllegalFileNameChars(fileName)

            Return Path.Combine(baseFolder, fileName)
        End Function

        ''' <summary>
        ''' Gets the user's Temp folder path.
        ''' </summary>
        Public Function GetTempFolder() As String
            Return Path.GetTempPath()
        End Function

        ''' <summary>
        ''' Gets the user's Documents folder path.
        ''' </summary>
        Public Function GetDocumentsFolder() As String
            Return Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
        End Function

        ''' <summary>
        ''' Returns a human-readable name for the cover type based on the letter selected.
        ''' (Simple placeholder; can be expanded later if needed.)
        ''' </summary>
        Public Function GetCoverTypeName(letter As String) As String
            Select Case letter.ToUpper()
                Case "A" : Return "Standard Fax Cover"
                Case "B" : Return "Court Fax Cover"
                Case "C" : Return "Sheriff Fax Cover"
                Case "D" : Return "CONREP Fax Cover"
                Case "E" : Return "DA Fax Cover"
                Case "F" : Return "PPR Cover"
                Case "G" : Return "WIC Fax Cover"
                Case "H" : Return "Non-Extension Cover"
                Case "I" : Return "Extension Cover"
                Case "J" : Return "Renewal Cover"
                Case "K" : Return "1370 90-Day Cover"
                Case "L" : Return "1372(e) CERT"
                Case "M" : Return "1372 CERT"
                Case "N" : Return "Sheriff Unlikely Cover"
                Case "O" : Return "Court Unlikely Cover"
                Case "P" : Return "Sheriff Unlikely c1 Cover"
                Case "Q" : Return "Court Unlikely b1 Cover"
                Case "R" : Return "TCAR Updated"
                Case "S" : Return "Court Email"
                Case "T" : Return "Sheriff Email"
                Case Else
                    Return "UnknownCover"
            End Select
        End Function

        ''' <summary>
        ''' Removes illegal file name characters from a string.
        ''' </summary>
        Public Function RemoveIllegalFileNameChars(fileName As String) As String
            Dim invalidChars = Path.GetInvalidFileNameChars()
            For Each c In invalidChars
                fileName = fileName.Replace(c, "_"c)
            Next
            Return fileName
        End Function

    End Module

End Namespace
