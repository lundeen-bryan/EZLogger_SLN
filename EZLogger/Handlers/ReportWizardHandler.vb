Imports Microsoft.Office.Interop.Word
Imports System.Windows
Imports System.Windows.Forms
Imports MessageBox = System.Windows.MessageBox

Namespace EZLogger.Handlers

    Public Class ReportWizardHandler

        Public Function OnSearchButtonClick() As String
            Dim reader As New WordFooterReader()
            Return reader.FindPatientNumberInFooter()
        End Function

    End Class

End Namespace


