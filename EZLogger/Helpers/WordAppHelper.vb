Imports Microsoft.Office.Interop.Word

Namespace Helpers

    ''' <summary>
    ''' Provides helper methods related to Microsoft Word application and documents.
    ''' </summary>
    Public Module WordAppHelper

        ''' <summary>
        ''' Returns the current Word application instance.
        ''' </summary>
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
