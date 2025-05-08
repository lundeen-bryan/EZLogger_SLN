Imports Microsoft.Office.Interop.Word

Namespace Helpers

    ''' <summary>
    ''' Provides helper methods related to Microsoft Word application and documents.
    ''' </summary>
    Public Module WordAppHelper

        ''' <summary>
        ''' Safely retrieves the Word Application instance from the current VSTO Add-in.
        ''' Returns Nothing if Word is not fully initialized.
        ''' </summary>
        Public Function GetWordApp() As Microsoft.Office.Interop.Word.Application
            Try
                Return TryCast(Globals.ThisAddIn.Application, Microsoft.Office.Interop.Word.Application)
            Catch ex As Exception
                ErrorHelper.HandleError("WordAppHelper.GetWordApp", ex.HResult.ToString(), ex.Message, "Please restart Word and try again.")
                Return Nothing
            End Try
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
