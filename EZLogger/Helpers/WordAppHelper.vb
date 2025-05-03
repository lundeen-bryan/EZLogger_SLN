Imports Microsoft.Office.Interop.Word

Namespace Helpers

    ''' <summary>
    ''' Provides helper methods related to Microsoft Word application and documents.
    ''' </summary>
    Public Module WordAppHelper

        ''' <summary>
        ''' Returns the current global instance of the Word Application from the active VSTO Add-in.
        ''' </summary>
        ''' <returns>The Microsoft Word Application object associated with the running Add-in.</returns>
        ''' <remarks>
        ''' This function centralizes access to the Word application so that other classes don't need
        ''' to pass it around via constructors or parameters. Safe to call from anywhere in the project.
        ''' </remarks>
        Public Function GetWordApp() As Application
            Return Globals.ThisAddIn.Application
        End Function

        ''' <summary>
        ''' Returns the total number of pages in the active Word document.
        ''' </summary>
        Public Function GetActiveDocumentPageCount() As Integer
            Try
                Dim wordApp As Application = GetWordApp()
                Dim activeDoc As Document = wordApp?.ActiveDocument

                If activeDoc IsNot Nothing Then
                    Return activeDoc.ComputeStatistics(WdStatistic.wdStatisticPages)
                End If
            Catch ex As Exception
                ' Optional: log error if needed
            End Try

            Return 0
        End Function

    End Module

End Namespace
