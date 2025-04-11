Imports System.IO
Imports Microsoft.Office.Interop.Word

''' <summary>
''' Provides methods for extracting parts of a file name from a full file path or the active Word document.
''' </summary>
Public Module FileNameHelper

    ''' <summary>
    ''' Extracts a portion of the file path based on the specified choice.
    ''' </summary>
    ''' <param name="choice">
    ''' 1 = Base file name (no extension, remove extra dots) [default] <br/>
    ''' 2 = File extension only <br/>
    ''' 3 = Full file path
    ''' </param>
    ''' <param name="filePath">
    ''' Optional: If not provided, uses the currently active Word document's full path.
    ''' </param>
    ''' <returns>The requested file name part as a string.</returns>
    ''' <example>
    ''' Dim baseName = FileNameHelper.GetFilePart(1) ' Returns "Report_123"
    ''' Dim ext = FileNameHelper.GetFilePart(2)      ' Returns ".docx"
    ''' Dim fullPath = FileNameHelper.GetFilePart(3) ' Returns "C:\Reports\Report_123.docx"
    ''' </example>
    Public Function GetFilePart(choice As Integer, Optional filePath As String = Nothing) As String
        ' Fallback to active document if no path provided
        If String.IsNullOrWhiteSpace(filePath) Then
            Dim doc As Document = Globals.ThisAddIn.Application.ActiveDocument
            filePath = doc.FullName
        End If

        ' If still empty, return empty string
        If String.IsNullOrWhiteSpace(filePath) Then
            Return String.Empty
        End If

        Select Case choice
            Case 1 ' Base name without extension, clean up extra periods
                Dim fileName As String = Path.GetFileNameWithoutExtension(filePath)
                If fileName.Count(Function(c) c = "."c) >= 1 Then
                    fileName = RemoveAllButLastPeriod(fileName)
                End If
                Return fileName

            Case 2 ' File extension only
                Return Path.GetExtension(filePath)

            Case 3 ' Full file path
                Return Path.GetFullPath(filePath)

            Case Else
                Throw New ArgumentOutOfRangeException(NameOf(choice), "Choice must be 1, 2, or 3.")
        End Select
    End Function

    ''' <summary>
    ''' Removes all but the last period from a file name.
    ''' For example: "patient.report.123" becomes "patientreport.123"
    ''' </summary>
    ''' <param name="input">The file name to clean.</param>
    ''' <returns>A cleaned version with only the last period.</returns>
    Private Function RemoveAllButLastPeriod(input As String) As String
        Dim lastPeriodIndex As Integer = input.LastIndexOf("."c)
        If lastPeriodIndex <= 0 Then Return input

        Dim before = input.Substring(0, lastPeriodIndex).Replace(".", "")
        Dim after = input.Substring(lastPeriodIndex)
        Return before & after
    End Function

End Module